using _06_Inventory.Api.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _06_Inventory.Api.Model.Enumerations
{
    public class MessagesResponseTypes : Enumeration
    {
        public static MessagesResponseTypes Info = new MessagesResponseTypes(nameof(Info), "Información");
        public static MessagesResponseTypes Warning = new MessagesResponseTypes(nameof(Warning), "Advertencia");
        public static MessagesResponseTypes Danger = new MessagesResponseTypes(nameof(Danger), "Error");
        public static MessagesResponseTypes Success = new MessagesResponseTypes(nameof(Success), "Exitoso");


        public static IEnumerable<MessagesResponseTypes> List() => new[] { Info, Warning, Danger, Success };

        public MessagesResponseTypes(string key, string name) : base(key, name) { }

        public static MessagesResponseTypes FromKey(string key)
        {
            var item = List().SingleOrDefault(s => s.Key == key);

            if (item == null)
            {
                throw new ArgumentException($"Valores posibles para {nameof(MessagesResponseTypes)}: {string.Join(",", List().Select(s => s.Name))}");
            }

            return item;
        }
    }
}
