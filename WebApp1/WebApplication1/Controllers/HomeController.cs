using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(new FirmAssetClassMapModel());
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase firmsFile, HttpPostedFileBase assetClassFile)
        {
            Dictionary<string, string> firm = new Dictionary<string, string>();
            Dictionary<string, string> assetClass = new Dictionary<string, string>();
            Dictionary<string, FirmInfoDetailModel> map = new Dictionary<string, FirmInfoDetailModel>();

            string firmsFilePath = string.Empty;
            string assetClassFilePath = string.Empty;

            if (firmsFile != null && assetClassFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                firmsFilePath = path + Path.GetFileName(firmsFile.FileName);
                string firmsFileExtension = Path.GetExtension(firmsFile.FileName);

                assetClassFilePath = path + Path.GetFileName(assetClassFile.FileName);
                string assetClassFileExtension = Path.GetExtension(assetClassFile.FileName);

                firmsFile.SaveAs(firmsFilePath);
                assetClassFile.SaveAs(assetClassFilePath);

                string conString = string.Empty;
                switch (firmsFileExtension)
                {
                    case ".xls": //For Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //For Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }

                conString = string.Format(conString, firmsFilePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            DataTable dt = new DataTable();
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();

                            foreach (DataRow row in dt.Rows)
                            {
                                firm[row["Firm ID"].ToString()] = row["Firm Name"].ToString();
                                /*System.Diagnostics.Debug.WriteLine("ID:" + row["Firm ID"].ToString() + ", Name:" + firm[row["Firm ID"].ToString()]);*/
                            }
                        }
                    }
                }

                switch (assetClassFileExtension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }

                conString = string.Format(conString, assetClassFilePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            DataTable dt = new DataTable();
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema1;
                            dtExcelSchema1 = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema1.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();

                            foreach (DataRow row in dt.Rows)
                            {
                                string InterestedFirmId = row["Interested Firms (ID)"].ToString();
                                FirmInfoDetailModel firmInfoDetail;
                                if (map.ContainsKey(InterestedFirmId))
                                {
                                    firmInfoDetail = map[InterestedFirmId];
                                } else
                                {
                                    firmInfoDetail = new FirmInfoDetailModel
                                    {
                                        FirmId = InterestedFirmId,
                                        FirmName = firm[InterestedFirmId]
                                    };
                                }
                                firmInfoDetail.AssetClassModel.Add(new AssetClassModel
                                {
                                    AssetClassId = row["Asset Class ID"].ToString(),
                                    AssetClassName = row["Asset Class Name"].ToString()
                                });

                                map[InterestedFirmId] = firmInfoDetail;
                                assetClass[row["Asset Class ID"].ToString()] = row["Asset Class Name"].ToString();
                                /*System.Diagnostics.Debug.WriteLine("ID:" + row["Asset Class ID"].ToString() + ", Name:" + assetClass[row["Asset Class Id"].ToString()]);
                                System.Diagnostics.Debug.WriteLine("InterestedFirmId: " + row["Interested Firms (ID)"].ToString());*/
                            }
                        }
                    }
                }
            }
            /*System.Diagnostics.Debug.WriteLine("********************************");
            foreach (KeyValuePair<string, FirmInfoDetailModel> entry in map)
            {
                System.Diagnostics.Debug.WriteLine("Firm Id: " + entry.Key + ", Firm Name: " + entry.Value.FirmName + ", Asset Class Id: " + entry.Value.AssetClassModel[0].AssetClassId + "Asset Class Name: " + entry.Value.AssetClassModel[0].AssetClassName);
            }
            System.Diagnostics.Debug.WriteLine("********************************");*/
            FirmAssetClassMapModel firmAssetClassMapModel = new FirmAssetClassMapModel();
            firmAssetClassMapModel.firms = firm;
            firmAssetClassMapModel.assetClasses = assetClass;
            firmAssetClassMapModel.map = map;
            return View(firmAssetClassMapModel);
        }
    }
}