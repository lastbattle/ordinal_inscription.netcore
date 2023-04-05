using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinOrdinal.netcore {

    /// <summary>
    /// Blockchain data provider types
    /// Description = the API URL
    /// </summary>
    public enum BcDataProviderType {
        [Description("https://blockchain.info/rawtx/{0}")]
        BlockchainInfo,


        [Description("https://blockstream.info/api/tx/{0}")]
        BlockStream, // https://github.com/Blockstream/esplora/blob/master/API.md

        BitcoinCoreRPC,
    }
}
