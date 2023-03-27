using goat.netcore.Ordinal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Nito.AsyncEx;
using System.Diagnostics;

namespace goat.netcore.test
{

    [TestClass]
    public class UnitTest1 {

        [TestMethod]
        public void Test_BitcoinCoreRPC() {
            //Goat g = new Goat();
            //g.ProcessRawTx("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db");


        }

        [TestMethod]
        public void Test_BlockchainInfoRPC() {
            Goat g = new Goat();

            AsyncContext.Run(async () => {
                OrdinalData ordinal = await g.CreateAPIQuery("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db", BcDataProviderType.BlockchainInfo);
                if (ordinal != null) {
                    Debug.WriteLine(ordinal);
                }
            });
            AsyncContext.Run(async () => {
                OrdinalData ordinal = await g.CreateAPIQuery("167b24f615b9c35c39064e314adc4fdb802ed1050ecf649ce887859ee3c5f6db", BcDataProviderType.BlockStream);
                if (ordinal != null) {
                    Debug.WriteLine(ordinal);
                }
            });
        }
    }
}