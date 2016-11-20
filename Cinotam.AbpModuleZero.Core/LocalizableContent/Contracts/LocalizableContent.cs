using Cinotam.AbpModuleZero.LocalizableContent.Helpers;
using System;
using System.Web.Script.Serialization;

namespace Cinotam.AbpModuleZero.LocalizableContent.Contracts
{
    public class LocalizableContent<T, TContentType> : ILocalizableContent<T, TContentType>
        where T : class
        where TContentType : class
    {
        public LocalizableContent(T entity, TContentType contentType, string lang)
        {
            var queryObj = QueryObj.CreateQueryObj(entity);
            var dtoObj = QueryObj.CreateQueryObj(typeof(TContentType));
            EntityId = queryObj.EntityId;
            EntityName = queryObj.EntityName;
            EntityDtoName = dtoObj.EntityName;
            Properties = SerializeContent(contentType);
            Lang = lang;
        }

        public string SerializeContent(TContentType obj)
        {
            var json = new JavaScriptSerializer().Serialize(obj);
            return json;
        }


        public static TContentType DeserializeContent(string json)
        {
            try
            {
                var obj = new JavaScriptSerializer().Deserialize<TContentType>(json);
                return obj;
            }
            catch (Exception)
            {
                throw new Exception("Error deserializing content");
            }

        }

        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string Lang { get; set; }
        public string Properties { get; set; }
        public string EntityDtoName { get; set; }
    }
}
