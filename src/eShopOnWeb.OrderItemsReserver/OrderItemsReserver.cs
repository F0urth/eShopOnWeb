using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace eShopOnWeb.OrderItemsReserver;

using Azure.Storage.Blobs;

public static class OrderItemsReserver
{
    private const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=learnbloborders;AccountKey=q95BY1dyJOO3oS6bfKFfxYC8j4V45eoy8kRzNZz2GIjIpyEVDw+Pa/2/FC9cAZt/UjefR/yi8Bp/+AStkzIPiw==;EndpointSuffix=core.windows.net";

    [Function("OrderItemsReserver")]
    public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("OrderItemsReserver");
        logger.LogInformation("Begin execution");
        var data = await req.ReadAsStringAsync();
        var blobContainerClient = new BlobContainerClient(ConnectionString, "orders");

        await blobContainerClient.CreateIfNotExistsAsync();
        BlobClient blobClient = blobContainerClient.GetBlobClient($"{Guid.NewGuid().ToString()}.json");
        await blobClient.UploadAsync(BinaryData.FromString(data));
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync(data);

        return response;
        
    }
}
