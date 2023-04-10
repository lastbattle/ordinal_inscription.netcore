using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinOrdinal.netcore.Models.BitcoinCore {

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class TransactionModel {
        public string txid { get; set; }
        public string hash { get; set; }
        public int version { get; set; }
        public int size { get; set; }
        public int vsize { get; set; }
        public int weight { get; set; }
        public int locktime { get; set; }
        public List<Vin> vin { get; set; }
        public List<Vout> vout { get; set; }
    }

    public class ScriptPubKey {
        public string asm { get; set; }
        public string desc { get; set; }
        public string hex { get; set; }
        public string address { get; set; }
        public string type { get; set; }
    }

    public class ScriptSig {
        public string asm { get; set; }
        public string hex { get; set; }
    }

    public class Vin {
        public string txid { get; set; }
        public int vout { get; set; }
        public ScriptSig scriptSig { get; set; }
        public List<string> txinwitness { get; set; }
        public long sequence { get; set; }
    }

    public class Vout {
        public double value { get; set; }
        public int n { get; set; }
        public ScriptPubKey scriptPubKey { get; set; }
    }

}
