using NBitcoin;
using NBitcoin.DataEncoders;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Unicode;

namespace goat.netcore {
    public class Goat {


        private static readonly HttpClient _httpClient = new();

        /// <summary>
        /// Gets the inscription data from a locally hosted Bitcoin core node using RPC
        /// </summary>
        /// <param name="bitcoinTxId"></param>
        /// <returns></returns>
        public string ProcessRawTx(string bitcoinTxId) {
            // query nBitcoin with a bitcoin transaction id
            /*var tx = NBitcoin.Transaction.Parse(bitcoinTxId, Network.Main);

            // get witness data from a bitcoin transaction
            var witness = tx.Inputs[0].WitScript;

            IEnumerable<byte[]> pushes = witness.Pushes;
            if (pushes.Count() >= 5) { // "ord"

                // Convert UTF8 to string
                var utf8_inscruption = Encoding.UTF8.GetString(pushes.ElementAt(5));
                return utf8_inscruption;
            }*/
            return null;
        }

        /// <summary>
        /// Gets the inscription data from a blockchain data provider API
        /// </summary>
        /// <param name="bitcoinTxId"></param>
        /// <param name="bcDataProviderType"></param>
        /// <returns></returns>
        public async Task<OrdinalData> CreateAPIQuery(string bitcoinTxId, BcDataProviderType bcDataProviderType) {
            // Create a HTTP request
            using (var request = new HttpRequestMessage()) {
                request.Method = HttpMethod.Get;

                request.RequestUri = new Uri(string.Format(bcDataProviderType.Description(), bitcoinTxId));

                try {
                    using (var response = await _httpClient.SendAsync(request)) {
                        // Get the response content
                        var content = response.Content.ReadAsStringAsync().Result;
                        // Return the content

                        if (content != null) {
                            dynamic? json = JsonConvert.DeserializeObject(content);
                            dynamic? inputs = json?.inputs[0];
                            string witness = Convert.ToString(inputs?.witness);

                            if (witness == null) {
                                throw new Exception(string.Format("Error parsing API response from {0}", bcDataProviderType.ToString()));
                            }
                            OrdinalData ordinal = DecodeOrdinalFromWitnessData(bitcoinTxId, witness);
                            return ordinal;
                        }
                    }
                }
                catch (Exception exp) {
                    throw new Exception("Error requesting API ", exp);
                }
            }
            return null;
        }


        #region Internal
        /// <summary>
        /// Decodes the Ordinal data from a witness hex script.
        /// </summary>
        /// <param name="witnessHex"></param>
        /// <returns></returns>
        private OrdinalData DecodeOrdinalFromWitnessData(string bitcoinTxId, string witnessHex) {
            BitcoinStream stream = new(Encoders.Hex.DecodeData(witnessHex));

            if (!stream.ProtocolCapabilities.SupportWitness) {
                throw new Exception(string.Format("The transaction id {0} is not a witness transaction.", bitcoinTxId));
            }
            WitScript witness = WitScript.Load(stream);

            IEnumerable<byte[]> pushes = witness.Pushes;
            foreach (byte[] b in pushes) {
                Script script = new Script(b);

                Debug.WriteLine(script.ToString());

                bool bIsOrdDataRegion = false;
                OrdinalData data = new();

                foreach (Op op in script.ToOps()) {
                    // OP_1 indicates that the next push contains the content type
                    // and OP_0 indicates that subsequent data pushes contain the content itself.
                    // Multiple data pushes must be used for large inscriptions, as one of taproot's few restrictions is that individual data pushes may not be larger than 520 bytes.

                    /*OP_FALSE
                     *  OP_IF
                     *  OP_PUSH "ord"
                     *  OP_1
                     *  OP_PUSH "text/plain;charset=utf-8"
                     *  OP_0
                     *  OP_PUSH "Hello, world!"
                     *  OP_ENDIF*/
                    // https://docs.ordinals.com/inscriptions.html

                    if ((byte)op.Code == 3) {
                        if (op.PushData == null)
                            continue;
                        // Convert UTF8 bytes to string
                        string pushDataString = System.Text.Encoding.UTF8.GetString(op.PushData);

                        if (pushDataString == "ord") { // specification standard to filter out other junk https://docs.ordinals.com/inscriptions.html
                            bIsOrdDataRegion = true; // flag
                        }
                        //Debug.WriteLine(op.ToString());
                    }
                    else if ((byte)op.Code == 9) {
                        string pushDataString = System.Text.Encoding.UTF8.GetString(op.PushData);

                        data.MetadataType = pushDataString; // set
                    }
                    else if (op.Code == OpcodeType.OP_PUSHDATA2 && bIsOrdDataRegion) {
                        if (op.PushData == null)
                            continue;

                        data.Metadata = op.PushData; // set
                        return data;
                    }
                }
                /*
                 * 
                 * 2b699194e41d48 OP_IF OP_UNKNOWN(0xed) 6b8c3fec1d7f100325baa82bf0ec3f22ed0aa93b97 e OP_DEPTH OP_UNKNOWN(0xfd) a737b713920eded10ba5 0
                 * 117f692257b2331233b5705ce9c682be8719ff1b2b64cbca290bd6faeb54423e [OP_CHECKSIG dfeb9d208701] [OP_DROP 0] [OP_IF 6f7264] [1 696d6167652f706e67] [0 89504e470d0a1a0a0000000d49484452000000360000003608020000000327fd8a0000000467414d410000b18f0bfc6105000000017352474200aece1ce9000000097048597300000ec300000ec301c76fa864000002514944415468deed99bd4a03411485a74840d1c6c242d4c22aa0b1d04a10120b094454ec6c04312882222a88f847888d565a6950b11285348a0fe00bd80afa0e163e81e07a926baec3ec3aab9bc96e90190ec3dd9b09f7db33bbb3938d705e2e1a5cc2225a448b5837c4c144826470a4791745a5e96be3531a16cd4453ed782cf61325f2f8347a448d916c61f4889e94325f64887a23cd5af87f113f9ecf7fa254f86864944bb706b1819e2e1322aea8b929aefbcadfe9458d7ceffdeb2427572c6baf84a48e2c4c44e273bb08501d65c888b8e6327ddd8a90bf6e1ff6a69c110e2a8aaaea8d78393f0a2d0d7dc371c603919850eeaed287e022509c4a634a25e3e9a2105517eb8d4834a857281488c99d01a5075fa02bb226446e72c607b1ba9a8637d17cc72813ed0921c389d06e17aaaadc2e887d09c240f45c71587a8230265ad93572f3450cf004af09b1b7a3ed4f2ed2394489c804d42bbb46f6d834229a1c48bd2fa29c0fcce78338d7d992cfe7d1cb01c77a4488c6287c261151e07165ea75739a042cee494c4c526c4386c6e034a03ab8e838a0b9cd8d411cb845e7703ada7f931d60b7284006f932fad99b78f8 805a93d960bb719d8b87a93eb7d2e934058a8bf2b4d2e117e2f6130988e67f18d0350726eee580c6a0f0c841295b28ceee4cce6d74ed9f24111393cc2716eecd23824f0e94435679fa2a100757e333ab89e9e59e4c2e357fb44558acc07c665edea1fce2f971666d1701898ce4c35af88c212a979a3b63117fb19990173c7726e2b711cabee1379b9d685c9469ccbe73b2ff1858448b68112da245b4880dab4feb0262817f6e5c6f0000000049454e44ae426082] OP_ENDIF
                 * OP_UNKNOWN(0xc1) 7f692257b2331233b5705ce9c682be8719 OP_UNKNOWN(0xff) 0
                */
                //Debug.WriteLine(script.ToString());
            }
            throw new Exception(string.Format("No ordinal data found in the witness transaction: {0}.", bitcoinTxId));
        }
        #endregion
    }

}