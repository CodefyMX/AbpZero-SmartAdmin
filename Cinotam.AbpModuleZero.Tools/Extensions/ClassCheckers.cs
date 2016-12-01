using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cinotam.AbpModuleZero.Tools.Extensions
{
    public static class ClassCheckers
    {
        public static IDictionary<string, string> HasAttribute(Type attribute, IEnumerable<PropertyInfo> properties, object obj)
        {
            var dic = new Dictionary<string, string>();
            foreach (var propertyInfo in properties)
            {

                var equals = propertyInfo.GetCustomAttributes(attribute, false).Length > 0;
                if (equals)
                {
                    dic.Add(propertyInfo.Name, propertyInfo.GetValue(obj).ToString());
                }
            }
            return dic;
        }
        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperties().FirstOrDefault(a => a.Name == propertyName) != null;
        }
    }
}
