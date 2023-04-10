using BitcoinOrdinal.netcore.BitcoinCore;
using BitcoinOrdinal.netcore.Models.Goat;
using BitcoinOrdinal.netcore.Ordinal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nito.AsyncEx;
using System.Diagnostics;

namespace BitcoinOrdinal.netcore.test
{

    [TestClass]
    public class UnitTest1 {

        /// <summary>
        ///  Test the deserialization of goat ordinal data
        /// </summary>
        [TestMethod]
        public void Test_GoatOrdinalData() {
            BitcoinOrdinal btcordinal = new();

            AsyncContext.Run((async () => {
                // https://blockstream.info/tx/15a3aecd63494487ca96472c56caf9a13532d7bae46ce8c1ee80da94ec4c7535i0
                OrdinalData ordinal = await btcordinal.QueryOrdinalData("15a3aecd63494487ca96472c56caf9a13532d7bae46ce8c1ee80da94ec4c7535", BcDataProviderType.BlockStream);

                GoatModel goatordinal = GoatModel.DeserializeGoatData(ordinal);

                Assert.IsNotNull(ordinal, "ordinal for blockchainInfo is null");
                Assert.IsNotNull(goatordinal, "ordinal is not goat");
            }));
        }

        /// <summary>
        /// Test fetching from a random ordinal data source until one is found
        /// </summary>
        [TestMethod]
        public void Test_RandomDataSource() {
            BitcoinOrdinal btcordinal = new();

            AsyncContext.Run((async () => {
                OrdinalData ordinal = await btcordinal.QueryOrdinalData("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db");

                Assert.IsNotNull(ordinal, "ordinal for random is null");
            }));
            // attempt 2 to test for cache.
            AsyncContext.Run((async () => {
                OrdinalData ordinal = await btcordinal.QueryOrdinalData("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db");

                Assert.IsNotNull(ordinal, "ordinal for random is null");
            }));
        }

        /// <summary>
        /// Test fetching ordinal data from bitcoin core RPC
        /// </summary>
        [TestMethod]
        public void Test_BitcoinCoreRPC() {
            BitcoinOrdinal btcordinal = new("127.0.0.1", 8332, "", "");

            AsyncContext.Run((async () => {
                OrdinalData ordinal = await btcordinal.QueryOrdinalData("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db", BcDataProviderType.BitcoinCoreRPC, 782675);

                Assert.IsNotNull(ordinal, "ordinal for bitcoin core is null");
            }));
        }

        /// <summary>
        /// Test fetching ordinal data from blockstream.info and blockstream.info
        /// </summary>
        [TestMethod]
        public void Test_BlockchainInfoRPC() {
            BitcoinOrdinal btcordinal = new();

            AsyncContext.Run((async () => {
                OrdinalData ordinal = await btcordinal.QueryOrdinalData("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db", BcDataProviderType.BlockchainInfo);

                Assert.IsNotNull(ordinal, "ordinal for blockchainInfo is null");
            }));
            AsyncContext.Run((async () => {
                OrdinalData ordinal = await btcordinal.QueryOrdinalData("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db", BcDataProviderType.BlockStream);

                Assert.IsNotNull(ordinal, "ordinal for blockstream is null");
            }));
        }
    }
}