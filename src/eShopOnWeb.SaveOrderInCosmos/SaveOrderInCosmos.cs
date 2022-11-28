namespace eShopOnWeb.SaveOrderInCosmos;

using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Data.Dto;
using Microsoft.Extensions.Logging;
using Services;

public static class SaveOrderInCosmos
{
    private static SaveDataInCosmosService _cosmosService = new();
    
    [Function("SaveOrderInCosmos")]
    public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("SaveOrderInCosmos");
        
        var inputOrder = await req.ReadFromJsonAsync<OrderDto>();
        logger.LogInformation("Order deserialized");

        await _cosmosService.SaveOrder(inputOrder!);
        logger.LogInformation("Order Saved");
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        return response;
        
    }
}
