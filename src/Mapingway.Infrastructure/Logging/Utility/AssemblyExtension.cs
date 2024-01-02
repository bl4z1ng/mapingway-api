using System.Reflection;

namespace Mapingway.Infrastructure.Logging.Utility;

public static class AssemblyExtension
{
    public static string GetAssemblyName(this Assembly assembly)
    {
        var product = assembly.GetCustomAttribute<AssemblyProductAttribute>();

        return string.IsNullOrWhiteSpace(product?.Product)
            ? assembly.GetName().Name!
            : product.Product;
    }

    public static string GetAssemblyVersion(this Assembly assembly)
    {
        var version = assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version;

        if (string.IsNullOrWhiteSpace(version))
        {
            version = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        }

        return string.IsNullOrWhiteSpace(version) ? "1.0.0" : version;
    }
}
