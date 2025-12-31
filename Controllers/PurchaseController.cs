using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClinicApp.ViewModels;
using ClinicApp.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace ClinicApp.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly ClinicAppMySqlDbContext _db;

        public PurchaseController(ClinicAppMySqlDbContext db)
        {
            _db = db;
        }

        // ============================
        // INDEX
        // ============================
        public IActionResult Index()
        {
            var list = new List<PurchaseViewModel>();

            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("GetPurchaseList", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new PurchaseViewModel
                {
                    PurchaseId = Convert.ToInt32(r["PurchaseId"]),
                    PurchaseNo = r["PurchaseNo"].ToString(),
                    PurchaseDate = Convert.ToDateTime(r["PurchaseDate"]),
                    SupplierName = r["SupplierName"].ToString(),
                    TotalQty = Convert.ToDecimal(r["TotalQty"]),
                    TotalAmount = Convert.ToDecimal(r["TotalAmount"]),
                    TotalDiscount = Convert.ToDecimal(r["TotalDiscount"]),
                    TotalTax = Convert.ToDecimal(r["TotalTax"]),
                    RoundOff = Convert.ToDecimal(r["RoundOff"]),
                    NetAmount = Convert.ToDecimal(r["NetAmount"])
                });
            }

            return View("~/Views/Pharmacy/Purchase/Index.cshtml", list);
        }

        // ============================
        // CREATE - GET
        // ============================
        public IActionResult Create()
        {
            var vm = new PurchaseViewModel
            {
                PurchaseNo = "PUR-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                PurchaseDate = DateTime.Today,
                Items = new List<PurchaseItemViewModel>(),
                SupplierList = GetSupplierList(),
                ProductList = GetProductList()
            };

            return View("~/Views/Pharmacy/Purchase/Create.cshtml", vm);
        }

        // ============================
        // CREATE - POST
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PurchaseViewModel vm)
        {
            if (vm.Items == null || vm.Items.Count == 0)
            {
                ModelState.AddModelError("", "Please add at least one item.");
            }

            // if (!ModelState.IsValid)
            // {
            //     vm.SupplierList = GetSupplierList();
            //     vm.ProductList = GetProductList();
            //     return View("~/Views/Pharmacy/Purchase/Create.cshtml", vm);
            // }

            using var conn = _db.GetConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();

            try
            {
                int purchaseId;

                // ---- Insert Purchase Master ----
                using (var cmd = new MySqlCommand("CreatePurchase", conn, tx))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_PurchaseNo", vm.PurchaseNo);
                    cmd.Parameters.AddWithValue("p_PurchaseDate", vm.PurchaseDate);
                    cmd.Parameters.AddWithValue("p_SupplierId", vm.SupplierId);
                    cmd.Parameters.AddWithValue("p_TotalQty", vm.TotalQty);
                    cmd.Parameters.AddWithValue("p_TotalAmount", vm.TotalAmount);
                    cmd.Parameters.AddWithValue("p_TotalDiscount", vm.TotalDiscount);
                    cmd.Parameters.AddWithValue("p_TotalTax", vm.TotalTax);
                    cmd.Parameters.AddWithValue("p_RoundOff", vm.RoundOff);
                    cmd.Parameters.AddWithValue("p_NetAmount", vm.NetAmount);
                    cmd.Parameters.AddWithValue("p_Remarks", vm.Remarks ?? "");
                    cmd.Parameters.AddWithValue("p_SupplierInvoiceNo", vm.SupplierInvoiceNo ?? "");
                    cmd.Parameters.AddWithValue("p_SupplierInvoiceDate", vm.SupplierInvoiceDate);

                    purchaseId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // ---- Insert Items + Update Stock ----
                foreach (var item in vm.Items)
                {
                    using (var cmd = new MySqlCommand("CreatePurchaseItem", conn, tx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_PurchaseId", purchaseId);
                        cmd.Parameters.AddWithValue("p_ProductId", item.ProductId);
                        cmd.Parameters.AddWithValue("p_BatchNo", item.BatchNo ?? "");
                        cmd.Parameters.AddWithValue("p_ExpiryDate", item.ExpiryDate);
                        cmd.Parameters.AddWithValue("p_Qty", item.Qty);
                        cmd.Parameters.AddWithValue("p_FreeQty", item.FreeQty);
                        cmd.Parameters.AddWithValue("p_Rate", item.Rate);
                        cmd.Parameters.AddWithValue("p_MRP", item.MRP);
                        cmd.Parameters.AddWithValue("p_DiscountPercent", item.DiscountPercent);
                        cmd.Parameters.AddWithValue("p_TaxPercent", item.TaxPercent);
                        cmd.Parameters.AddWithValue("p_NetAmount", item.NetAmount);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new MySqlCommand("UpdateStockAfterPurchase", conn, tx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_ProductId", item.ProductId);
                        cmd.Parameters.AddWithValue("p_Qty", item.Qty + item.FreeQty);
                        cmd.Parameters.AddWithValue("p_PurchaseId", purchaseId);
                        cmd.ExecuteNonQuery();
                    }
                }

                tx.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                tx.Rollback();
                ModelState.AddModelError("", ex.Message);
                vm.SupplierList = GetSupplierList();
                vm.ProductList = GetProductList();
                return View("~/Views/Pharmacy/Purchase/Create.cshtml", vm);
            }
        }

        // ============================
        // EDIT - GET
        // ============================
        public IActionResult Edit(int id)
        {
            var vm = new PurchaseViewModel { Items = new List<PurchaseItemViewModel>() };

            using var conn = _db.GetConnection();
            conn.Open();

            using (var cmd = new MySqlCommand("GetPurchaseById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_Id", id);

                using var r = cmd.ExecuteReader();
                if (!r.Read()) return NotFound();

                vm.PurchaseId = Convert.ToInt32(r["PurchaseId"]);
                vm.PurchaseNo = r["PurchaseNo"].ToString();
                vm.PurchaseDate = Convert.ToDateTime(r["PurchaseDate"]);
                vm.SupplierId = Convert.ToInt32(r["SupplierId"]);
                vm.SupplierInvoiceNo = r["SupplierInvoiceNo"].ToString();
                vm.SupplierInvoiceDate = r["SupplierInvoiceDate"] as DateTime?;
                vm.TotalQty = Convert.ToDecimal(r["TotalQty"]);
                vm.TotalAmount = Convert.ToDecimal(r["TotalAmount"]);
                vm.TotalDiscount = Convert.ToDecimal(r["TotalDiscount"]);
                vm.TotalTax = Convert.ToDecimal(r["TotalTax"]);
                vm.RoundOff = Convert.ToDecimal(r["RoundOff"]);
                vm.NetAmount = Convert.ToDecimal(r["NetAmount"]);
                vm.Remarks = r["Remarks"].ToString();
            }

            using (var cmd = new MySqlCommand("GetPurchaseItemsByPurchaseId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_PurchaseId", id);

                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    vm.Items.Add(new PurchaseItemViewModel
                    {
                        ProductId = Convert.ToInt32(r["ProductId"]),
                        ProductName = r["ProductName"].ToString(),
                        BatchNo = r["BatchNo"].ToString(),
                        ExpiryDate = r["ExpiryDate"] as DateTime?,
                        UOM = r["UOM"].ToString(),
                        Qty = Convert.ToDecimal(r["Qty"]),
                        FreeQty = Convert.ToDecimal(r["FreeQty"]),
                        Rate = Convert.ToDecimal(r["Rate"]),
                        MRP = Convert.ToDecimal(r["MRP"]),
                        DiscountPercent = Convert.ToDecimal(r["DiscountPercent"]),
                        TaxPercent = Convert.ToDecimal(r["TaxPercent"]),
                        NetAmount = Convert.ToDecimal(r["NetAmount"])
                    });
                }
            }

            vm.SupplierList = GetSupplierList();
            vm.ProductList = GetProductList();

            return View("~/Views/Pharmacy/Purchase/Edit.cshtml", vm);
        }

        // ============================
        // EDIT - POST
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PurchaseViewModel vm)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();

            try
            {
                using (var cmd = new MySqlCommand("UpdatePurchase", conn, tx))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_PurchaseId", vm.PurchaseId);
                    cmd.Parameters.AddWithValue("p_PurchaseDate", vm.PurchaseDate);
                    cmd.Parameters.AddWithValue("p_SupplierId", vm.SupplierId);
                    cmd.Parameters.AddWithValue("p_TotalQty", vm.TotalQty);
                    cmd.Parameters.AddWithValue("p_TotalAmount", vm.TotalAmount);
                    cmd.Parameters.AddWithValue("p_TotalDiscount", vm.TotalDiscount);
                    cmd.Parameters.AddWithValue("p_TotalTax", vm.TotalTax);
                    cmd.Parameters.AddWithValue("p_RoundOff", vm.RoundOff);
                    cmd.Parameters.AddWithValue("p_NetAmount", vm.NetAmount);
                    cmd.Parameters.AddWithValue("p_Remarks", vm.Remarks ?? "");
                    cmd.Parameters.AddWithValue("p_SupplierInvoiceNo", vm.SupplierInvoiceNo ?? "");
                    cmd.Parameters.AddWithValue("p_SupplierInvoiceDate", vm.SupplierInvoiceDate);
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new MySqlCommand("DeletePurchaseItems", conn, tx))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_PurchaseId", vm.PurchaseId);
                    cmd.ExecuteNonQuery();
                }

                foreach (var item in vm.Items)
                {
                    using var cmd = new MySqlCommand("CreatePurchaseItem", conn, tx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_PurchaseId", vm.PurchaseId);
                    cmd.Parameters.AddWithValue("p_ProductId", item.ProductId);
                    cmd.Parameters.AddWithValue("p_BatchNo", item.BatchNo ?? "");
                    cmd.Parameters.AddWithValue("p_ExpiryDate", item.ExpiryDate);
                    cmd.Parameters.AddWithValue("p_Qty", item.Qty);
                    cmd.Parameters.AddWithValue("p_FreeQty", item.FreeQty);
                    cmd.Parameters.AddWithValue("p_Rate", item.Rate);
                    cmd.Parameters.AddWithValue("p_MRP", item.MRP);
                    cmd.Parameters.AddWithValue("p_DiscountPercent", item.DiscountPercent);
                    cmd.Parameters.AddWithValue("p_TaxPercent", item.TaxPercent);
                    cmd.Parameters.AddWithValue("p_NetAmount", item.NetAmount);
                    cmd.ExecuteNonQuery();
                }

                tx.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                tx.Rollback();
                vm.SupplierList = GetSupplierList();
                vm.ProductList = GetProductList();
                return View("~/Views/Pharmacy/Purchase/Edit.cshtml", vm);
            }
        }

        // ============================
        // DETAILS - GET
        // ============================
        public IActionResult Details(int id)
        {
            var vm = new PurchaseViewModel { Items = new List<PurchaseItemViewModel>() };

            using var conn = _db.GetConnection();
            conn.Open();

            using (var cmd = new MySqlCommand("GetPurchaseById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_Id", id);

                using var r = cmd.ExecuteReader();
                if (!r.Read()) return NotFound();

                vm.PurchaseId = Convert.ToInt32(r["PurchaseId"]);
                vm.PurchaseNo = r["PurchaseNo"].ToString();
                vm.PurchaseDate = Convert.ToDateTime(r["PurchaseDate"]);
                vm.SupplierId = Convert.ToInt32(r["SupplierId"]);
                vm.SupplierName = r["SupplierName"].ToString();
                vm.SupplierInvoiceNo = r["SupplierInvoiceNo"].ToString();
                vm.SupplierInvoiceDate = r["SupplierInvoiceDate"] as DateTime?;
                vm.TotalQty = Convert.ToDecimal(r["TotalQty"]);
                vm.TotalAmount = Convert.ToDecimal(r["TotalAmount"]);
                vm.TotalDiscount = Convert.ToDecimal(r["TotalDiscount"]);
                vm.TotalTax = Convert.ToDecimal(r["TotalTax"]);
                vm.RoundOff = Convert.ToDecimal(r["RoundOff"]);
                vm.NetAmount = Convert.ToDecimal(r["NetAmount"]);
                vm.Remarks = r["Remarks"].ToString();
            }

            using (var cmd = new MySqlCommand("GetPurchaseItemsByPurchaseId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_PurchaseId", id);

                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    vm.Items.Add(new PurchaseItemViewModel
                    {
                        ProductName = r["ProductName"].ToString(),
                        BatchNo = r["BatchNo"].ToString(),
                        ExpiryDate = r["ExpiryDate"] as DateTime?,
                        UOM = r["UOM"].ToString(),
                        Qty = Convert.ToDecimal(r["Qty"]),
                        FreeQty = Convert.ToDecimal(r["FreeQty"]),
                        Rate = Convert.ToDecimal(r["Rate"]),
                        MRP = Convert.ToDecimal(r["MRP"]),
                        DiscountPercent = Convert.ToDecimal(r["DiscountPercent"]),
                        TaxPercent = Convert.ToDecimal(r["TaxPercent"]),
                        NetAmount = Convert.ToDecimal(r["NetAmount"])
                    });
                }
            }

            return View("~/Views/Pharmacy/Purchase/Details.cshtml", vm);
        }

        // ============================
        // HELPERS
        // ============================
        private List<SelectListItem> GetSupplierList()
        {
            var list = new List<SelectListItem>();
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("SELECT id, ledger_name FROM ledgermaster", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new SelectListItem
                {
                    Value = r["id"].ToString(),
                    Text = r["ledger_name"].ToString()
                });
            }
            return list;
        }

        private List<SelectListItem> GetProductList()
        {
            var list = new List<SelectListItem>();
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("SELECT ProductCode, ProductName FROM productmaster", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new SelectListItem
                {
                    Value = r["ProductCode"].ToString(),
                    Text = r["ProductName"].ToString()
                });
            }
            return list;
        }
    }
}
