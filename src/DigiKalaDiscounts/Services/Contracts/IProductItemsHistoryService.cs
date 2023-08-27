using DigiKalaDiscounts.Models;

namespace DigiKalaDiscounts.Services.Contracts;

public interface IProductItemsHistoryService
{
    void AddIncompleteItem(ProductItem item, string error);
    void SaveChanges();
    bool ShouldProcessItem(ProductItem item);
    void MarkAsCompletedItem(ProductItem item);
    ProductItem? FindItem(ProductItem item);
    ProductItem? FindItem(string id);
}
