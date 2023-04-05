
namespace BitcoinOrdinal.netcore.Models.BitcoinCore {
    public class BlockModel {
        public string hash { get; set; }
        public int confirmations { get; set; }
        public uint height { get; set; }
        public int version { get; set; }
        public string versionHex { get; set; }
        public string merkleroot { get; set; }
        public uint time { get; set; }
        public uint mediantime { get; set; }
        public long nonce { get; set; }
        public string bits { get; set; }
        public double difficulty { get; set; }
        public string chainwork { get; set; }
        public int nTx { get; set; }
        public string previousblockhash { get; set; }
        public string nextblockhash { get; set; }
        public int strippedsize { get; set; }
        public int size { get; set; }
        public int weight { get; set; }
        public List<string> tx { get; set; }
    }
}
