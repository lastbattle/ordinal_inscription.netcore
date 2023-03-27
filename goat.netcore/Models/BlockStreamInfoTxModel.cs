using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goat.netcore.Models {
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Prevout {
        public string scriptpubkey { get; set; }
        public string scriptpubkey_asm { get; set; }
        public string scriptpubkey_type { get; set; }
        public string scriptpubkey_address { get; set; }
        public int value { get; set; }
    }

    public class BlockStreamInfoTxModel {
        public string txid { get; set; }
        public int version { get; set; }
        public int locktime { get; set; }
        public List<Vin> vin { get; set; }
        public List<Vout> vout { get; set; }
        public int size { get; set; }
        public int weight { get; set; }
        public int fee { get; set; }
        public Status status { get; set; }
    }

    public class Status {
        public bool confirmed { get; set; }
        public uint block_height { get; set; }
        public string block_hash { get; set; }
        public uint block_time { get; set; }
    }

    public class Vin {
        public string txid { get; set; }
        public int vout { get; set; }
        public Prevout prevout { get; set; }
        public string scriptsig { get; set; }
        public string scriptsig_asm { get; set; }
        public List<string> witness { get; set; }
        public bool is_coinbase { get; set; }
        public long sequence { get; set; }
    }

    public class Vout {
        public string scriptpubkey { get; set; }
        public string scriptpubkey_asm { get; set; }
        public string scriptpubkey_type { get; set; }
        public string scriptpubkey_address { get; set; }
        public int value { get; set; }
    }


}
