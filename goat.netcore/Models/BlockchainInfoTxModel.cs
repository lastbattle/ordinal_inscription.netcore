using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goat.netcore.Models {

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Input {
        public long sequence { get; set; }
        public string witness { get; set; }
        public string script { get; set; }
        public int index { get; set; }
        public PrevOut prev_out { get; set; }
    }

    public class Out {
        public int type { get; set; }
        public bool spent { get; set; }
        public int value { get; set; }
        public List<object> spending_outpoints { get; set; }
        public int n { get; set; }
        public object tx_index { get; set; }
        public string script { get; set; }
        public string addr { get; set; }
    }

    public class PrevOut {
        public string addr { get; set; }
        public int n { get; set; }
        public string script { get; set; }
        public List<SpendingOutpoint> spending_outpoints { get; set; }
        public bool spent { get; set; }
        public long tx_index { get; set; }
        public int type { get; set; }
        public int value { get; set; }
    }

    public class BlockchainInfoTxModel {
        public string hash { get; set; }
        public int ver { get; set; }
        public int vin_sz { get; set; }
        public int vout_sz { get; set; }
        public int size { get; set; }
        public int weight { get; set; }
        public int fee { get; set; }
        public string relayed_by { get; set; }
        public int lock_time { get; set; }
        public long tx_index { get; set; }
        public bool double_spend { get; set; }
        public int time { get; set; }
        public uint block_index { get; set; }
        public uint block_height { get; set; }
        public List<Input> inputs { get; set; }
        public List<Out> @out { get; set; }
    }

    public class SpendingOutpoint {
        public int n { get; set; }
        public long tx_index { get; set; }
    }

}
