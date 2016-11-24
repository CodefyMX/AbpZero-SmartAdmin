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
            Lang = lang;
            SerializeContent(contentType);
        }

        //public string SerializeContent(TContentType obj)
        //{
        //    dynamic objToSerialize = new ExpandoObject();
        //    objToSerialize.Content = obj;
        //    var shareableProps = ClassCheckers.HasAttribute(typeof(IsSharedProperty), obj.GetType().GetProperties(), obj);
        //    objToSerialize.SharedProps = shareableProps;
        //    var json = new JavaScriptSerializer().Serialize(objToSerialize);
        //    Properties = json;
        //    return json;
        //}
        public string SerializeContent(TContentType obj)
        {
            var json = new JavaScriptSerializer().Serialize(obj);
            Properties = json;
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
