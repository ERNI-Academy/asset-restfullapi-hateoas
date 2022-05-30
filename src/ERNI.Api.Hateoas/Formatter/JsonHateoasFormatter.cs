using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ERNI.Api.Hateoas.Formatter;

public class JsonHateoasFormatter : OutputFormatter
{
    public JsonHateoasFormatter()
    {
        SupportedMediaTypes.Add("application/json+hateoas");
    }

    public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
    {
                
        var response = context.HttpContext.Response;

        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        if (context.Object is SerializableError error)
        {
            var errorOutput = JsonConvert.SerializeObject(error, serializerSettings);
            response.ContentType = "application/json";
            return response.WriteAsync(errorOutput);
        }

        var genericFormatter = new GenericFormatter(context);
        var resultResponse = genericFormatter.GetResultResponse();

        var output = JsonConvert.SerializeObject(resultResponse, serializerSettings);
        response.ContentType = "application/json+hateoas";
        return response.WriteAsync(output);
    }
}
