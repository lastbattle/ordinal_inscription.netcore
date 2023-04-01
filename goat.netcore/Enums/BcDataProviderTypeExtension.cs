using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goat.netcore.Enums
{

    public static class BcDataProviderTypeExtension {

        public static string Description(this BcDataProviderType value) {
            // get attributes  
            var field = value.GetType().GetField(value.ToString());

            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes == null)
                throw new Exception("No [Description] attribute specified for " + attributes.ToString() + " in BcDataProviderType enum.");

            // return description
            return attributes.Any() ? ((DescriptionAttribute)attributes.ElementAt(0)).Description : "Description Not Found";
        }
    }
}
