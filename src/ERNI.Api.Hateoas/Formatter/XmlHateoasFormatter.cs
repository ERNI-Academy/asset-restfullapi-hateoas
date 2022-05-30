using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using System.Xml.Serialization;

namespace ERNI.Api.Hateoas.Formatter;

public class XmlHateoasFormatter : OutputFormatter
{
    public XmlHateoasFormatter()
    {
        SupportedMediaTypes.Add("application/xml+hateoas");
    }

    public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
    {
        var response = context.HttpContext.Response;

        if (context.Object is SerializableError error)
        {
            var streamError = new MemoryStream();
            var errorSerializer = new XmlSerializer(typeof(SerializableError));
            errorSerializer.Serialize(streamError, error);
            response.ContentType = "application/xml";
            return response.WriteAsync(Encoding.Default.GetString(streamError.ToArray()));
        }

        var genericFormatter = new GenericFormatter(context);
        var resultResponse = genericFormatter.GetResultResponse();

        var stream = new MemoryStream();
        var serializer = new XmlSerializer(resultResponse.GetType());
        serializer.Serialize(stream, resultResponse);

        var output = Encoding.Default.GetString(stream.ToArray());
        response.ContentType = "application/xml+hateoas";
        return response.WriteAsync(output);
    }
}
