using System;
using System.Web;

namespace Cinotam.AbpModuleZero.Tools.DatatablesJsModels.ReflectionHelpers
{
    public static class InputBuilder
    {
        public static T BuildInputByRequest<T>(HttpRequestBase request)
        {
            //Get type
            var obj = typeof(T);
            //Create a new instance of type
            var instance = (T)Activator.CreateInstance(typeof(T));
            //Look for all properties in the type
            foreach (var props in obj.GetProperties())
            {
                //Get the name of the property
                var prop = props.Name;

                //Create a new property info
                var propertyInfo = obj.GetProperty(props.Name);

                //Gets the string value from the request params based on the property name
                var stringValue = request.Params.GetValues(prop);

                //If its empty just ignore it
                if (stringValue == null) continue;

                //Gets the first value
                var value = stringValue[0];
                try
                {
                    if (propertyInfo.PropertyType == typeof(Guid))
                    {
                        var convertedGuid = Guid.Parse(value);
                        //Tries to convert the value type to the required value type for the generic object
                        propertyInfo.SetValue(instance, convertedGuid, null);
                    }
                    else
                    {
                        var convertedValue = Convert.ChangeType(value, propertyInfo.PropertyType);
                        //Tries to convert the value type to the required value type for the generic object
                        propertyInfo.SetValue(instance, convertedValue, null);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return instance;
        }
    }
}
