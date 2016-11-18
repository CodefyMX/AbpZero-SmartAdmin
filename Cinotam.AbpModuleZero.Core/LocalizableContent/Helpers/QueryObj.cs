using System;

namespace Cinotam.AbpModuleZero.LocalizableContent.Helpers
{
    public class QueryObj
    {
        protected QueryObj() { }

        public string EntityName { get; set; }
        public string EntityId { get; set; }

        public static QueryObj CreateQueryObj<T>(T element)
        {
            var elm = element.GetType();
            var propertyInfo = elm.GetProperty("Id");

            if (propertyInfo == null) throw new NullReferenceException(nameof(propertyInfo));

            var idVal = propertyInfo.GetValue(element);

            var entityName = elm.FullName;
            return new QueryObj()
            {
                EntityId = idVal.ToString(),
                EntityName = entityName,
            };
        }
        public static QueryObj CreateQueryObj(Type type)
        {
            var entityName = type.FullName;
            return new QueryObj()
            {
                EntityName = entityName,
            };
        }
    }
}
