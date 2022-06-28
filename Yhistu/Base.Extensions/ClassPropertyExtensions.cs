using System.Reflection;

namespace Base.Extensions;

public class SkipPropertyAttribute : Attribute
{
}

public static class ClassPropertyExtensions
{
        public static PropertyInfo[] GetFilteredProperties(this Type type)
        {
            return type.GetProperties()
                .Where(pi => !Attribute.IsDefined(pi, typeof(SkipPropertyAttribute)))
                .ToArray();
        }
}