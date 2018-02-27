using System.Collections.Generic;

namespace CoinDriveICO.Framework.JsonStructures.AdapterApi
{
    public class SetProcessedRequest
    {
        /// <summary>
        /// List of unique keys of processed transactions
        /// </summary>
        /// <value>
        /// 
        /// </value>
        public List<string> Ids { get; set; }
    }
}