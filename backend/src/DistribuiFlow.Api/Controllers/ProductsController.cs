using DistribuiFlow.Application.DTOs.Products;
using DistribuiFlow.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistribuiFlow.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> CreateAsync(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { id = product.Id },
            product);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(product);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ProductResponse>>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(
            cancellationToken);

        return Ok(products);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> UpdateAsync(
        Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(product);
    }

    [HttpPatch("{id:guid}/stock/increase")]
    public async Task<ActionResult<ProductResponse>> IncreaseStockAsync(
        Guid id,
        [FromBody] ChangeStockRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productService.IncreaseStockAsync(
            id,
            request,
            cancellationToken);

        return Ok(product);
    }

    [HttpPatch("{id:guid}/stock/decrease")]
    public async Task<ActionResult<ProductResponse>> DecreaseStockAsync(
        Guid id,
        [FromBody] ChangeStockRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productService.DecreaseStockAsync(
            id,
            request,
            cancellationToken);

        return Ok(product);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<ActionResult<ProductResponse>> DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var product = await _productService.DeactivateAsync(
            id,
            cancellationToken);

        return Ok(product);
    }
}