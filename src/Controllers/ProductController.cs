using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository ??
                throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetProducts()
        {
            var productsFromRepo = _productRepository.GetProducts();
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(productsFromRepo));
        }

        [HttpGet("{productId}", Name = "GetProduct")]
        public ActionResult<ProductDto> GetProduct(int productId)
        {
            var productFromRepo = _productRepository.GetProduct(productId);

            if (productFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductDto>(productFromRepo));
        }

        [HttpPost]
        public ActionResult<ProductDto> CreateProduct(ProductForCreationDto product)
        {
            var productEntity = _mapper.Map<Product>(product);
            _productRepository.AddProduct(productEntity);
            _productRepository.Save();

            var productToReturn = _mapper.Map<ProductDto>(productEntity);
            return CreatedAtRoute("GetProduct",
                new { productId = productToReturn.Id }, productToReturn);
        }

        [HttpPut("{productId}")]
        public IActionResult UpdateProduct(int productId, ProductForUpdateDto product)
        {
            var productFromRepo = _productRepository.GetProduct(productId);

            if (productFromRepo == null)
            {
                var productToAdd = _mapper.Map<Product>(product);
                productToAdd.Id = productId;

                _productRepository.AddProduct(productToAdd);
                _productRepository.Save();

                var productToReturn = _mapper.Map<ProductDto>(productToAdd);

                return CreatedAtRoute("GetProduct",
                    new { productId = productToReturn.Id },
                    productToReturn);
            }

            _mapper.Map(product, productFromRepo);

            _productRepository.UpdateProduct(productFromRepo);
            _productRepository.Save();

            return NoContent();
        }

        [HttpPatch("{productId}")]
        public ActionResult PartiallyUpdateProduct(int productId, JsonPatchDocument<ProductForUpdateDto> patchDocument)
        {
            var productFromRepo = _productRepository.GetProduct(productId);

            if (productFromRepo == null)
            {
                var productDto = new ProductForUpdateDto();
                patchDocument.ApplyTo(productDto, ModelState);

                if (!TryValidateModel(productDto))
                {
                    return ValidationProblem(ModelState);
                }

                var productToAdd = _mapper.Map<Product>(productDto);
                productToAdd.Id = productId;

                _productRepository.AddProduct(productToAdd);
                _productRepository.Save();

                var productToReturn = _mapper.Map<ProductDto>(productToAdd);

                return CreatedAtRoute("GetProduct",
                    new { productId = productToReturn.Id },
                    productToReturn);
            }

            var productToPatch = _mapper.Map<ProductForUpdateDto>(productFromRepo);
            patchDocument.ApplyTo(productToPatch, ModelState);

            if (!TryValidateModel(productToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(productToPatch, productFromRepo);

            _productRepository.UpdateProduct(productFromRepo);
            _productRepository.Save();

            return NoContent();
        }

        [HttpDelete("{productId}")]
        public ActionResult DeleteProduct(int productId)
        {
            var productFromRepo = _productRepository.GetProduct(productId);

            if (productFromRepo == null)
            {
                return NotFound();
            }

            _productRepository.DeleteProduct(productFromRepo);
            _productRepository.Save();

            return NoContent();
        }

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
