using BitcoinOrdinal.netcore.Ordinal;
using NBitcoin.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BitcoinOrdinal.netcore.Models.Goat {

    /// <summary>
    /// Goat model 
    /// https://ipfs.io/ipfs/QmYyucgBQVfs9JXZ2MtmkGPAhgUjNgyGE6rcJT1KybQHhp/index.html#goatfile
    /// </summary>
    public class GoatModel {

        [YamlMember(Alias="version")]
        public long Version { get; set; }

        [YamlMember(Alias = "alpaca")]
        public AlpacaOrLlamaModel alpaca { get; set; }

        [YamlMember(Alias = "llama")]
        public AlpacaOrLlamaModel llama { get; set; }


        public partial class AlpacaOrLlamaModel {
            [YamlMember(Alias = "7b")]
            public string The7B { get; set; }

            [YamlMember(Alias = "13b")]
            public string The13B { get; set; }

            [YamlMember(Alias = "30b")]
            public string The30B { get; set; }

            [YamlMember(Alias = "65b")]
            public string The65B { get; set; }
        }

        /// <summary>
        /// Deserializes the bitcoin ordinal data into GoatModel
        /// </summary> 
        public static GoatModel DeserializeGoatData(OrdinalData ordinalData) {
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml
                .Build();

            //yml contains a string containing your YAML
            string yamlStr = UTF8Encoding.UTF8.GetString(ordinalData.Metadata);
            var goat = deserializer.Deserialize<GoatModel>(yamlStr);
            return goat;
        }
    }
}
