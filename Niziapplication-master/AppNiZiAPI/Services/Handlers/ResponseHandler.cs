using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace AppNiZiAPI.Services.Handlers
{
    public interface IResponseHandler
    {
        IActionResult ForgeResponse(Dictionary<ServiceDictionaryKey, object> dictionary);
    }

    public class ResponseHandler : IResponseHandler
    {

        public IActionResult ForgeResponse(Dictionary<ServiceDictionaryKey, object> dictionary)
        {
            IActionResult actionResult = dictionary.ContainsKey(ServiceDictionaryKey.VALUE)
            ? (ActionResult)new OkObjectResult(dictionary[ServiceDictionaryKey.VALUE])
            : new BadRequestResult();

            if (dictionary.ContainsKey(ServiceDictionaryKey.ERROR))
                actionResult = ForgeErrorResponse(dictionary);

            return actionResult;
        }

        private IActionResult ForgeErrorResponse(Dictionary<ServiceDictionaryKey, object> dictionary)
        {
            IActionResult actionResult = new BadRequestObjectResult(dictionary[ServiceDictionaryKey.ERROR]);

            if (dictionary.ContainsKey(ServiceDictionaryKey.HTTPSTATUSCODE))
            {
                switch (dictionary[ServiceDictionaryKey.HTTPSTATUSCODE])
                {
                    case HttpStatusCode.NotFound:
                        actionResult = new NotFoundObjectResult(dictionary[ServiceDictionaryKey.ERROR]);
                        break;
                    case HttpStatusCode.Forbidden:
                        actionResult = new BadRequestObjectResult("(403) Forbidden: " + dictionary[ServiceDictionaryKey.ERROR]);
                        break;
                    default:
                        actionResult = new BadRequestObjectResult(dictionary[ServiceDictionaryKey.ERROR]);
                        break;
                }
            }

            return actionResult;
        }
    }
}