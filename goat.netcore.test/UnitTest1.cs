using goat.netcore.BitcoinCore;
using goat.netcore.Ordinal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nito.AsyncEx;
using System.Diagnostics;

namespace goat.netcore.test
{

    [TestClass]
    public class UnitTest1 {

        [TestMethod]
        public void Test_RandomDataSource() {
            Goat g = new();

            AsyncContext.Run(async () => {
                OrdinalData ordinal = await g.QueryOrdinalData("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db");

                Assert.IsNotNull(ordinal, "ordinal for random is null");
            });
            // attempt 2 to test for cache.
            AsyncContext.Run(async () => {
                OrdinalData ordinal = await g.QueryOrdinalData("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db");

                Assert.IsNotNull(ordinal, "ordinal for random is null");
            });
        }

        [TestMethod]
        public void Test_BitcoinCoreRPC() {
            Goat g = new();
 
            AsyncContext.Run(async () => {
                OrdinalData ordinal = await g.QueryOrdinalData("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db", BcDataProviderType.BitcoinCoreRPC);

                Assert.IsNotNull(ordinal, "ordinal for bitcoin core is null");
            });
        }

        [TestMethod]
        public void Test_BlockchainInfoRPC() {
            Goat g = new();

            AsyncContext.Run(async () => {
                OrdinalData ordinal = await g.QueryOrdinalData("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db", BcDataProviderType.BlockchainInfo);

                Assert.IsNotNull(ordinal, "ordinal for blockchainInfo is null");
            });
            AsyncContext.Run(async () => {
                OrdinalData ordinal = await g.QueryOrdinalData("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db", BcDataProviderType.BlockStream);

                Assert.IsNotNull(ordinal, "ordinal for blockstream is null");
            });
        }
    }
}