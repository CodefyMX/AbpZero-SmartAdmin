using System;
using System.Web.Script.Serialization;

namespace Cinotam.AbpModuleZero.Attachments.Contracts
{
    public class HasAttachment<TEntity> : IHasAttachment<TEntity> where TEntity : class
    {
        public string Description { get; set; }
        public string ContentUrl { get; set; }
        public bool Active { get; set; }
        public bool StoredInCdn { get; set; }
        public TEntity Entity { get; set; }
        public string Properties { get; set; }

        public string SerializeContent<TProperties>(TProperties obj)
        {
            var json = new JavaScriptSerializer().Serialize(obj);
            Properties = json;
            return json;
        }


        public static TProperties DeserializeContent<TProperties>(string json)
        {
            try
            {
                var obj = new JavaScriptSerializer().Deserialize<TProperties>(json);
                return obj;
            }
            catch (Exception)
            {
                throw new Exception("Error deserializing content");
            }

        }
        public HasAttachment(TEntity entity,
            string fileLocation,
            bool storedInCdn,
            bool active,
            string description)
        {
            StoredInCdn = storedInCdn;
            ContentUrl = fileLocation;
            Active = active;
            Description = description;
            Entity = entity;
        }

    }
}
