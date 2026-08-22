using DistribuiFlow.Application.DTOs.Products;
using DistribuiFlow.Application.Exceptions;
using DistribuiFlow.Application.Interfaces;
using DistribuiFlow.Application.Interfaces.Repositories;
using DistribuiFlow.Application.Interfaces.Services;
using DistribuiFlow.Domain.Entities;

namespace DistribuiFlow.Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductResponse> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var existingProduct = await _productRepository.GetByCodeAsync(
            request.Code,
            cancellationToken);

        if (existingProduct is not null)
        {
            throw new ConflictException(
                $"Já existe um produto cadastrado com o código '{request.Code}'.");
        }

        var product = new Product(
            request.Code,
            request.Name,
            request.Price,
            request.Stock);

        await _productRepository.AddAsync(
            product,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(product);
    }

    public async Task<ProductResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                $"Produto com identificador '{id}' não encontrado.");
        }

        return MapToResponse(product);
    }

    public async Task<IReadOnlyCollection<ProductResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(
            cancellationToken);

        return products
            .Select(MapToResponse)
            .ToArray();
    }

    private static ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Sku,
            product.Name,
            product.UnitPrice,
            product.StockQuantity,
            product.IsActive);
    }
}