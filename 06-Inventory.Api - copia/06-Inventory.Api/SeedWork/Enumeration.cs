using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace _06_Inventory.Api.SeedWork
{
    public class Enumeration : IComparable
    {
        public string Name { get; private set; }

        public string Key { get; private set; }
        public string SecondKey { get; private set; }

        protected Enumeration()
        {
        }

        protected Enumeration(string key, string name)
        {
            Key = key;
            Name = name;
        }



        protected Enumeration(string key, string name, string secondKey)
        {
            Key = key;
            Name = name;
            SecondKey = secondKey;
        }



        public override string ToString()
        {
            return Name;
        }



        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = new T();
                var locatedValue = info.GetValue(instance) as T;

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Key.Equals(otherValue.Key);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public static T FromValue<T>(string value) where T : Enumeration, new()
        {
            var matchingItem = Parse<T, string>(value, "value", item => item.Key == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));

                throw new InvalidOperationException(message);
            }

            return matchingItem;
        }

        public int CompareTo(object other)
        {
            return Key.CompareTo(((Enumeration)other).Key);
        }
    }
}
