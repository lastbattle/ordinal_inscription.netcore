using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goat.netcore.Ordinal
{
    public class OrdinalData
    {
        /// <summary>
        /// text/plain;charset=utf-8
        /// </summary>
        public string MetadataType
        {
            get; set;
        }

        /// <summary>
        /// Hello, world!
        /// </summary>
        public byte[] Metadata
        {
            get; set;
        }
    }
}
