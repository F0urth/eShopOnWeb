namespace eShopOnWeb.SaveOrderInCosmos.Data.Model;

using Newtonsoft.Json;

public class Order
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public int OrderNumber { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public decimal Total { get; set; }
    public Address ShippingAddress { get; set; }
    public List<Item> OrderItems { get; set; } = new();
}
