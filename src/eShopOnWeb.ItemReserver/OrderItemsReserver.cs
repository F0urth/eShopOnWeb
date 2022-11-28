using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;

namespace eShopOnWeb.ItemReserver;

public static class OrderItemsReserver
{
    private static readonly HttpClient _client = new();
    private const string EmailAppUrl =
        "https://prod-62.westeurope.logic.azure.com:443/workflows/0cdbc6154b0b4de8b8ccdaea539e5449/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=lKiyHHP3NJjhgUvP4pBlHdkhZGH28HK97PgaoahYwK4";
    private const string ConnectionString =
        "DefaultEndpointsProtocol=https;AccountName=learnlasttaskstorage;AccountKey=deTckMxjXPYV+zUnGc+AviYjpNtXb2yEgQa0tTub5ZL2VGCtN+VFnXuEakZ7HJyQgRPLNXSnbGkh+AStbcxv7g==;EndpointSuffix=core.windows.net";
    
    [FunctionName("OrderItemsReserver")]
    public static async Task Run([ServiceBusTrigger("order-reserved", Connection = "Endpoint=sb://learn-last-task-service-bus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=iCBrjTQZpM/E9qmwwAJQ+M2h+jVeBlGNtNRyIaOyeGU=")] string myQueueItem, FunctionContext context)
    {
        var logger = context.GetLogger("OrderItemsReserver");
        logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        try
        {
            var data = myQueueItem;
            var blobContainerClient = new BlobContainerClient(ConnectionString, "orders");

            await blobContainerClient.CreateIfNotExistsAsync();
            BlobClient blobClient = blobContainerClient.GetBlobClient($"{Guid.NewGuid().ToString()}.json");
            await blobClient.UploadAsync(BinaryData.FromString(data));
        }
        catch (Exception)
        {
            await _client.PostAsync(EmailAppUrl, new StringContent(string.Empty));
        }
        
    }
}
