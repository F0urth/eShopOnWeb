namespace eShopOnWeb.SaveOrderInCosmos.Services;

using System.Diagnostics;
using Data.Dto;
using Data.Model;
using Microsoft.Azure.Cosmos;

public class SaveDataInCosmosService
{
    private static readonly string _dataConnectionString =
        "AccountEndpoint=https://learn-cosmosdb-last-task.documents.azure.com:443/;AccountKey=FcHJsip1VOgwRIPkY6hKDHf8auQHUhwbFnlewdrGq2BEaL4bsOibCT2b8kXjJAZWcKrTM0j9lR7DACDbK8G6Yw==;";

    private const string DatabaseId = "Orders";
    private const string ContainerId = "Order";
    
    public async Task SaveOrder(OrderDto dto)
    {
        var model = FromDto(dto);

        using var cosmosClient = new CosmosClient(_dataConnectionString);
        var container = cosmosClient.GetContainer(DatabaseId, ContainerId);

        var response = await container.CreateItemAsync(model);

        Debug.WriteLine("Item created");
    }

    private static Order FromDto(OrderDto dto)
    {
        if (dto is null)
        {
            return null;
        }
        
        return new()
        {
            Id = Guid.NewGuid().ToString(),
            OrderNumber = dto.OrderNumber,
            OrderDate = dto.OrderDate,
            Total = dto.Total,
            ShippingAddress = FromDto(dto.ShipToAddress),
            OrderItems = FromDto(dto.OrderItems)
        };
    }

    private static Address FromDto(AddressDto dto)
    {
        if (dto is null)
        {
            return null;
        }
        
        return new()
        {
            Street = dto.Street,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            ZipCode = dto.ZipCode
        };
    }

    private static List<Item> FromDto(List<ItemDto> dtos)
    {
        if (dtos is null)
        {
            return null;
        }
        
        return dtos.Select(dto => new Item
        {
            ProductName = dto.ProductName,
            UnitPrice = dto.UnitPrice,
            Units = dto.Units,
        }).ToList();
    }
}
