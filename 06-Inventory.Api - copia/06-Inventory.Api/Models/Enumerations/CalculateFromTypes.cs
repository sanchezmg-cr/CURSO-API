using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _06_Inventory.Api.SeedWork;
namespace Payable.API.Model.Enumerations
{
    public class CalculateFromTypes : Enumeration
    {
        public static CalculateFromTypes DHA = new CalculateFromTypes(nameof(DHA), "Dias habiles");
        public static CalculateFromTypes DLA = new CalculateFromTypes(nameof(DLA), "Dias laborados");

        public static IEnumerable<CalculateFromTypes> List() => new[] { DHA, DLA };

        public CalculateFromTypes(string key, string name) : base(key, name) { }

        public static CalculateFromTypes FromKey(string key)
        {
            var item = List().SingleOrDefault(s => s.Key == key);

            if (item == null)
            {
                throw new ArgumentException($"Valores posibles para {nameof(CalculateFromTypes)}: {string.Join(",", List().Select(s => s.Name))}");
            }

            return item;
        }
    }
}
