using System;
using System.Linq;

namespace Domain.Models
{
    public class PluginStepMessage
    {
        public Guid PrimaryEntityId { get; set; }
        public string MessageName { get; set; }
        public PreEntityImage[] PreEntityImages { get; set; }

        public PreEntityImage GetPreEntityImage(string imageName)
        {
            return PreEntityImages.Where(image => image.key == imageName).FirstOrDefault();
        }
    }         

    public class PreEntityImage
    {
        public string key { get; set; }
        public Value value { get; set; }

        public object? GetValue(string key)
        {
            return value.Attributes.Where(attr => attr.key == key).FirstOrDefault()?.value;
        }
    }

    public class Value
    {
        public Attribute[] Attributes { get; set; }
    }

    public class Attribute
    {
        public string key { get; set; }
        public object value { get; set; }
    }

    public class Lookup
    {
        public Guid Id { get; set; }
    }
}
