using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Main.Util
{
    public static class MainEx
    {
        public static List<string> GetPropertyNamesByAttributeType<T>(this object obj)
        {
            List<string> propertyNames = new List<string>();

            var ps = TypeDescriptor.GetProperties(obj);
            for (int i = 0; i < ps.Count; i++)
            {
                var p = ps[i];

                foreach (var attribute in p.Attributes)
                {
                    if (attribute is T)
                    {
                        propertyNames.Add(p.Name);
                    }
                }
            }

            return propertyNames;
        }

        public static string GetNameByOrder(this string prefix, IEnumerable<string> existNames, int numberLength = 2, int startNumber = 0)
        {
            var format = "D" + numberLength;
            var local = existNames.ToList();

            if (!local.Any())
                return prefix + "-" + startNumber.ToString(format);

            var matches = local.Where(x => x.StartsWith(prefix)).ToList();
            if (!matches.Any())
                return prefix + "-" + startNumber.ToString(format);

            var numbers = matches.Select(x => int.Parse(x.Split('-')[1]));
            var maxNumber = numbers.Max();

            var nameByOrder = prefix + "-" + (maxNumber + 1).ToString(format);
            return nameByOrder;
        }

    }
}
