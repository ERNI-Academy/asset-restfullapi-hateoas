using ERNI.Api.Hateoas.Dto;
using System.Reflection;

namespace ERNI.Api.Hateoas.Services;

public class DataShaper : IDataShaper
{
    public PropertyInfo[] Properties { get; set; }

    public IEnumerable<ResponseDto> ShapeData(object entity, string fieldsString)
    {
        if (entity.GetType().GetTypeInfo().GetInterfaces().Any(i => i.GetTypeInfo() == typeof(System.Collections.IEnumerable)))
        {
            return ShapeData(entity as IEnumerable<object>, fieldsString);
        }

        FillProperties(entity);
        var requiredProperties = GetRequiredProperties(fieldsString);

        return new List<ResponseDto> { FetchDataForEntity(entity, requiredProperties) };
    }

    private IEnumerable<ResponseDto> ShapeData(IEnumerable<object> entities, string fieldsString)
    {
        FillProperties(entities.FirstOrDefault());
        var requiredProperties = GetRequiredProperties(fieldsString);

        return FetchData(entities, requiredProperties);
    }

    private void FillProperties(object item)
    {
        Properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
    {
        var requiredProperties = new List<PropertyInfo>();

        if (!string.IsNullOrWhiteSpace(fieldsString))
        {
            var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var field in fields)
            {
                var property = Properties.FirstOrDefault(pi => pi.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

                if (property == null)
                    continue;

                requiredProperties.Add(property);
            }
        }
        else
        {
            requiredProperties = Properties.ToList();
        }

        return requiredProperties;
    }

    private IEnumerable<ResponseDto> FetchData(IEnumerable<object> entities, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedData = new List<ResponseDto>();

        foreach (var entity in entities)
        {
            var shapedObject = FetchDataForEntity(entity, requiredProperties);
            shapedData.Add(shapedObject);
        }

        return shapedData;
    }

    private static ResponseDto FetchDataForEntity(object entity, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedObject = new ResponseDto();

        foreach (var property in requiredProperties)
        {
            var objectPropertyValue = property.GetValue(entity);
            shapedObject.TryAdd(property.Name, objectPropertyValue);
        }

        return shapedObject;
    }
}
