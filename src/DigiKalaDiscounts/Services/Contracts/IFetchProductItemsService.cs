using DigiKalaDiscounts.Models;

namespace DigiKalaDiscounts.Services.Contracts;

public interface IFetchProductItemsService
{
    Task<List<ProductItem>> GetProductItemsAsync();
}
