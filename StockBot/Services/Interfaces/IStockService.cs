using Entities;

namespace StockBot.Services.Interfaces
{
    public interface IStockService
    {
        Stock GetStock(string code);
    }
}
