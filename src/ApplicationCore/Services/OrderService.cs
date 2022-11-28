using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

using System.Net.Http;
using System.Net.Mime;
using System.Text;
using global::Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

public class OrderService : IOrderService
{
    private static readonly HttpClient _client = new();

    private readonly IRepository<Order> _orderRepository;
    private readonly IUriComposer _uriComposer;
    private readonly IRepository<Basket> _basketRepository;
    private readonly IRepository<CatalogItem> _itemRepository;

    private const string FunctionUrl = "https://functionapp-221128044653.azurewebsites.net/api/SaveOrderInCosmos";

    private const string QueueConnection =
        "Endpoint=sb://learn-last-task-service-bus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=iCBrjTQZpM/E9qmwwAJQ+M2h+jVeBlGNtNRyIaOyeGU=";

    private const string QueueName = "order-reserved";
    public OrderService(IRepository<Basket> basketRepository,
        IRepository<CatalogItem> itemRepository,
        IRepository<Order> orderRepository,
        IUriComposer uriComposer)
    {
        _orderRepository = orderRepository;
        _uriComposer = uriComposer;
        _basketRepository = basketRepository;
        _itemRepository = itemRepository;
    }

    public async Task CreateOrderAsync(int basketId, Address shippingAddress)
    {
        var basketSpec = new BasketWithItemsSpecification(basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

        Guard.Against.Null(basket, nameof(basket));
        Guard.Against.EmptyBasketOnCheckout(basket.Items);

        var catalogItemsSpecification = new CatalogItemsSpecification(basket.Items.Select(item => item.CatalogItemId).ToArray());
        var catalogItems = await _itemRepository.ListAsync(catalogItemsSpecification);

        var items = basket.Items.Select(basketItem =>
        {
            var catalogItem = catalogItems.First(c => c.Id == basketItem.CatalogItemId);
            var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, _uriComposer.ComposePicUri(catalogItem.PictureUri));
            var orderItem = new OrderItem(itemOrdered, basketItem.UnitPrice, basketItem.Quantity);
            return orderItem;
        }).ToList();

        var order = new Order(basket.BuyerId, shippingAddress, items);
        
        var obj = JsonConvert.SerializeObject(order);
        var jsonContent = new StringContent(obj, Encoding.UTF8, MediaTypeNames.Application.Json);
        var _ = await _client.PostAsync(FunctionUrl, jsonContent);

        await using var serviceBusClient = new ServiceBusClient(QueueConnection);
        await using var serviceBusSender = serviceBusClient.CreateSender(QueueName);

        await serviceBusSender.SendMessageAsync(new ServiceBusMessage(obj));
        await _orderRepository.AddAsync(order);
    }
}
