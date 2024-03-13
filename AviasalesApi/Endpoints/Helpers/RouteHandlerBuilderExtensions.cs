using AviasalesApi.Attributes;
using Castle.Core.Internal;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace AviasalesApi.Endpoints.Helpers
{
    public static class RouteHandlerBuilderExtensions
    {
        public static void WithOpenApiCustomParameters(this RouteHandlerBuilder builder, Type type)
        {
            builder.WithOpenApi(op =>
            {
                var props = type.GetProperties();
                foreach (var prop in props)
                {
                    if (prop.GetAttribute<ComplexPropertyAttribute>() != null)
                    {
                        foreach (var nestedProp in prop.PropertyType.GetProperties())
                        {
                            AddProp(op, nestedProp);
                        }
                    }
                    else
                    {
                        AddProp(op, prop);
                    }
                    
                }
                return op;
            });
        }

        private static void AddProp(OpenApiOperation op, PropertyInfo prop)
        {
            var isRequired = prop.GetCustomAttribute<System.Runtime.CompilerServices.RequiredMemberAttribute>();
            op.Parameters.Add(new()
            {
                Name = prop.Name,
                In = ParameterLocation.Query,
                Required = isRequired != null
            });
        }
    }
}
