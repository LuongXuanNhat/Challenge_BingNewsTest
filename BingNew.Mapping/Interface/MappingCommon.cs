using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.Mapping.Interface
{
    public static class MappingCommon
    {
        public static Type FindTypeByName(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type? type = null;
            foreach (var assembly in assemblies)
            {
                type ??= assembly.GetTypes().ToList().Find(x => x.Name.Equals(typeName));
            }
            return type ?? throw new InvalidOperationException("Type not found!");
        }
    }
}
