using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FirmAssetClassMapModel
    {
        ///<summary>
        /// Initialize Dictionaries.
        ///</summary>
        public FirmAssetClassMapModel()
        {
            firms = new Dictionary<string, string>();
            assetClasses = new Dictionary<string, string>();
            map = new Dictionary<string, FirmInfoDetailModel>();
        }

        ///<summary>
        /// Gets or sets Firms Dictionary.
        ///</summary>
        public Dictionary<string, string> firms { get; set; }

        ///<summary>
        /// Gets or sets AssetClass Dictionary.
        ///</summary>
        public Dictionary<string, string> assetClasses { get; set; }

        ///<summary>
        /// Gets or sets Firm - Asset Class Dictionary.
        ///</summary>
        public Dictionary<string, FirmInfoDetailModel> map { get; set; }
    }
}