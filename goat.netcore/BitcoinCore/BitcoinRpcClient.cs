using goat.netcore.Models.BitcoinCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goat.netcore.BitcoinCore {

    /// <summary>
    /// The bitcoin RPC wrapper client class.
    /// https://github.com/lastbattle/goat.netcore
    /// </summary>
    public class BitcoinRpcClient {
        private readonly string _rpcUrl;
        private readonly HttpClient _httpClient;

        private static readonly Dictionary<uint, BlockModel> blockTxKeyValuePairs = new Dictionary<uint, BlockModel>();

        /// <summary>
        /// Constructor for the BitcoinRpcClient
        /// </summary>
        /// <param name="rpcUrl"></param>
        /// <param name="rpcUser"></param>
        /// <param name="rpcPassword"></param>
        public BitcoinRpcClient(string rpcUrl, string rpcUser, string rpcPassword) {
            this._rpcUrl = rpcUrl;
            this._httpClient = new HttpClient();

            var byteArray = Encoding.ASCII.GetBytes($"{rpcUser}:{rpcPassword}");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        #region Internal
        /// <summary>
        /// Send the RPC request to the bitcoin core node.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private async Task<JObject> SendRequestAsync(string method, params object[] parameters) {
            var request = new {
                jsonrpc = "1.0",
                id = Guid.NewGuid().ToString(),
                method = method,
                @params = parameters
            };
            string jsonStr = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_rpcUrl, content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            JObject responseObject = JObject.Parse(responseJson);

            /*if (responseObject["error"] != null) {
                throw new InvalidOperationException($"RPC Error: {responseObject["error"]["message"]}");
            }*/
            return responseObject;
        }
        #endregion

        #region Custom methods
        /// <summary>
        /// Iterate through every Bitcoin block from 'startblockNumber' and find a specific transaction ID 
        /// </summary>
        /// <param name="startblockNumber"></param>
        /// <param name="txId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TransactionModel> FindTransactionFromBlockNo(uint startblockNumber, string txId) {
            uint bestBlockCount = await GetBlockCountAsync();

            if (bestBlockCount < startblockNumber)
                throw new Exception("start block number is lower than the current best block in the bitcoin core node. bestblockCount = " + bestBlockCount);

            // First attempt to find the transaction to see if its already indexed.


            // Then find the transaction block by block if none available
            for (uint i = startblockNumber; i < bestBlockCount; i++) {
                BlockModel block;

                if (!blockTxKeyValuePairs.ContainsKey(i)) {
                    string iBlockHash = await GetBlockHash(i);
                    block = await GetBlock(iBlockHash);

                    lock (blockTxKeyValuePairs) {
                        if (!blockTxKeyValuePairs.ContainsKey(i)) // check again
                            blockTxKeyValuePairs.Add(i, block);
                    }
                } else {
                    // this block is already synced
                    block = blockTxKeyValuePairs[i];
                }

                if (block != null && block.tx.Contains(txId)) 
                {
                    string rawTx = await GetRawTransaction(txId, block.hash);
                    //Debug.WriteLine(rawTx);

                    JObject decodeTx = await DecodeRawTransaction(rawTx);
                    TransactionModel transactionModel = JsonConvert.DeserializeObject<TransactionModel>(decodeTx["result"].ToString());

                    //Debug.WriteLine(transactionModel);

                    return transactionModel;
                }
                Debug.WriteLine("Current block ID: " + i);
            }

            return null;
        }
        #endregion

        #region RPCs
        /// <summary>
        /// Gets the raw transaction data from txId and the blockHash that the transaction is in.
        /// </summary>
        /// <param name="txId"></param>
        /// <param name="blockHash"></param>
        /// <returns>Return the raw transaction data.</returns>
        public async Task<string> GetRawTransaction(string txId, string blockHash) {
            var response = await SendRequestAsync("getrawtransaction", new object[] { txId, false, blockHash });

            return response["result"].ToString(); // the raw tx string data
        }

        /// <summary>
        /// Get the JSON object representing the serialized, hex-encoded transaction.
        /// 
        /// By default, this call only returns a transaction if it is in the mempool. If -txindex is enabled
        /// and no blockhash argument is passed, it will return the transaction if it is in the mempool or any block.
        /// If a blockhash argument is passed, it will return the transaction if
        /// the specified block is available and the transaction is in that block.
        /// </summary>
        /// <param name="transactionData"></param>
        /// <returns>Return a JSON object representing the serialized, hex-encoded transaction.</returns>
        public async Task<JObject> DecodeRawTransaction(string transactionData) {
            JObject response = await SendRequestAsync("decoderawtransaction", new object[] { transactionData });

            return response; // the raw tx string data
        }

        /// <summary>
        /// Returns hash of block in best-block-chain at height provided.
        /// </summary>
        /// <param name="blockHeight"></param>
        /// <returns></returns>
        public async Task<string> GetBlockHash(uint blockHeight) {
            var response = await SendRequestAsync("getblockhash", new object[] { blockHeight });

            return response["result"].ToString();
        }

        /// <summary>
        /// Get the block by the blockHash
        /// </summary>
        /// <param name="blockHeight"></param>
        /// <returns></returns>
        public async Task<BlockModel> GetBlock(string blockhash) {
            JObject response = await SendRequestAsync("getblock", new object[] { blockhash });

            // deserialize json to object
            BlockModel Block = JsonConvert.DeserializeObject<BlockModel>(response["result"].ToString());
            return Block;
        }

        /// <summary>
        /// Gets the best block hash currently synced on the node.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBestBlockHashAsync() {
            var response = await SendRequestAsync("getbestblockhash");

            return response["result"].ToString();
        }

        /// <summary>
        /// Gets the best block height currently synced on the node
        /// </summary>
        /// <returns></returns>
        public async Task<uint> GetBlockCountAsync() {
            var response = await SendRequestAsync("getblockcount");

            return response["result"].ToObject<uint>();
        }

        #endregion
    }
}
