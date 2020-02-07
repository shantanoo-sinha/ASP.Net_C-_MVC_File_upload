using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FirmInfoDetailModel
    {
        ///<summary>
        /// Gets or sets FirmId.
        ///</summary>
        public string FirmId { get; set; }

        ///<summary>
        /// Gets or sets FirmName.
        ///</summary>
        public string FirmName { get; set; }

        ///<summary>
        /// Gets or sets AssetClassModel List.
        ///</summary>
        public List<AssetClassModel> AssetClassModel { get; set; }

        ///<summary>
        /// Initiate AssetClassModel List.
        ///</summary>
        public FirmInfoDetailModel()
        {
            AssetClassModel = new List<AssetClassModel>();
        }
    }
}