namespace eShopOnWeb.SaveOrderInCosmos.Data.Dto;

public class OrderDto
{
    public int OrderNumber { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public decimal Total { get; set; }
    public AddressDto ShipToAddress { get; set; }
    public List<ItemDto> OrderItems { get; set; } = new();
}
