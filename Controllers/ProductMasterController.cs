using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Models;
using ClinicApp.ViewModels;
using ClinicApp.Data;
using MySql.Data.MySqlClient;

namespace ClinicApp.Controllers
{
    public class ProductMasterController : Controller
    {
        private readonly ILogger<ProductMasterController> _logger;
        public ProductMasterViewModel _productMasterVM;
        private readonly ClinicAppMySqlDbContext _db;
        private readonly AutoMapper.IMapper _mapper;
        public ProductMasterController(ILogger<ProductMasterController> logger, ClinicAppMySqlDbContext db, ProductMasterViewModel productMasterVM, AutoMapper.IMapper mapper)
        {
            _logger = logger;
            _productMasterVM = productMasterVM;
            _db = db;
            _productMasterVM.ErrorMessage = string.Empty;
            _mapper = mapper;
        }

        // GET: ProductMasters
        public IActionResult Index()
        {
            List<ProductMaster> products = new List<ProductMaster>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"SELECT * FROM ProductMaster", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new ProductMaster
                {
                    ProductCode = reader.GetInt32("ProductCode"),
                    ProductName = reader.GetString("ProductName"),
                    GenericName = reader.GetString("GenericName"),
                    ProductGroup_List = reader.GetString("ProductGroup_List"),
                    ProductGroup_Txt = reader.GetString("ProductGroup_Txt"),
                    PurchaseUnit_List = reader.GetString("PurchaseUnit_List"),
                    PurchaseUnit_Txt = reader.GetString("PurchaseUnit_Txt"),
                    Packing = reader.GetInt32("Packing"),
                    StockUnit_List = reader.GetString("StockUnit_List"),
                    StockUnit_Txt = reader.GetString("StockUnit_Txt"),
                    Rack_List = reader.GetString("Rack_List"),
                    Rack_Txt = reader.GetString("Rack_Txt"),
                    Bin_List = reader.GetString("Bin_List"),
                    Bin_Txt = reader.GetString("Bin_Txt"),
                    ProductType_List = reader.GetString("ProductType_List"),
                    ProductType_Txt = reader.GetString("ProductType_Txt"),
                    Manufacturer_List = reader.GetString("Manufacturer_List"),
                    Manufacturer_Txt = reader.GetString("Manufacturer_Txt"),
                    Remarks = reader.GetString("Remarks"),
                    PurchaseRate = reader.GetDecimal("PurchaseRate"),
                    PurchaseDiscount = reader.GetDecimal("PurchaseDiscount"),
                    MRP = reader.GetDecimal("MRP"),
                    SaleRate = reader.GetDecimal("SaleRate"),
                    SaleDiscount = reader.GetDecimal("SaleDiscount"),
                    TaxPercent = reader.GetDecimal("TaxPercent"),
                    HSN = reader.GetString("HSN"),
                    MinStock = reader.GetDecimal("MinStock"),
                    MaxStock = reader.GetDecimal("MaxStock"),
                    MinOrder = reader.GetDecimal("MinOrder"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt"),
                });
            }
            List<ProductMasterViewModel> productMasterVMList = new List<ProductMasterViewModel>();
            foreach (var product in products)
            {
                ProductMasterViewModel productMasterViewModel = _mapper.Map<ProductMasterViewModel>(product);
                productMasterVMList.Add(productMasterViewModel);
            }

            return View(productMasterVMList);
        }

        // GET: ProductMaster/Details/5
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            ProductMaster product = null;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
            SELECT * FROM ProductMaster WHERE ProductCode=@ProductCode", conn);
            cmd.Parameters.AddWithValue("@ProductCode", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                product = new ProductMaster
                {
                    ProductCode = reader.GetInt32("ProductCode"),
                    ProductName = reader.GetString("ProductName"),
                    GenericName = reader.GetString("GenericName"),
                    ProductGroup_List = reader.GetString("ProductGroup_List"),
                    ProductGroup_Txt = reader.GetString("ProductGroup_Txt"),
                    PurchaseUnit_List = reader.GetString("PurchaseUnit_List"),
                    PurchaseUnit_Txt = reader.GetString("PurchaseUnit_Txt"),
                    Packing = reader.GetInt32("Packing"),
                    StockUnit_List = reader.GetString("StockUnit_List"),
                    StockUnit_Txt = reader.GetString("StockUnit_Txt"),
                    Rack_List = reader.GetString("Rack_List"),
                    Rack_Txt = reader.GetString("Rack_Txt"),
                    Bin_List = reader.GetString("Bin_List"),
                    Bin_Txt = reader.GetString("Bin_Txt"),
                    ProductType_List = reader.GetString("ProductType_List"),
                    ProductType_Txt = reader.GetString("ProductType_Txt"),
                    Manufacturer_List = reader.GetString("Manufacturer_List"),
                    Manufacturer_Txt = reader.GetString("Manufacturer_Txt"),
                    Remarks = reader.GetString("Remarks"),
                    PurchaseRate = reader.GetDecimal("PurchaseRate"),
                    PurchaseDiscount = reader.GetDecimal("PurchaseDiscount"),
                    MRP = reader.GetDecimal("MRP"),
                    SaleRate = reader.GetDecimal("SaleRate"),
                    SaleDiscount = reader.GetDecimal("SaleDiscount"),
                    TaxPercent = reader.GetDecimal("TaxPercent"),
                    HSN = reader.GetString("HSN"),
                    MinStock = reader.GetDecimal("MinStock"),
                    MaxStock = reader.GetDecimal("MaxStock"),
                    MinOrder = reader.GetDecimal("MinOrder"),
                    IsActive = reader.GetBoolean("IsActive"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                };
            }
            ProductMasterViewModel productMasterVM = _mapper.Map<ProductMasterViewModel>(product);
            return View(productMasterVM);
        }

        // GET: ProductMaster/Create
        public IActionResult Create()
        {
            /*
            List<SelectListItem> roles = new List<SelectListItem>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT RoleId, RoleName FROM Roles", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                roles.Add(new SelectListItem
                {
                    Value = reader.GetGuid("RoleId").ToString(),
                    Text = reader.GetString("RoleName")
                });
            }
            _appUserVM.Roles = roles;
            //ViewData["RoleID"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            return View(_appUserVM);
            */
            return View(_productMasterVM);
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductMasterViewModel productMasterViewModel)
        {
            // if (!ModelState.IsValid)
            // {
            //     List<SelectListItem> roles = new List<SelectListItem>();
            //     using var connection = _db.GetConnection();
            //     connection.Open();
            //     var command = new MySqlCommand("SELECT RoleId, RoleName FROM Roles", connection);
            //     using var reader = command.ExecuteReader();
            //     while (reader.Read())
            //     {
            //         roles.Add(new SelectListItem
            //         {
            //             Value = reader.GetGuid("RoleId").ToString(),
            //             Text = reader.GetString("RoleName")
            //         });
            //     }
            //     _appUserVM.Roles = roles;
            //     return View(_appUserVM);
            // }
            // else
            // {

            ProductMaster productMaster = _mapper.Map<ProductMaster>(productMasterViewModel);
            productMaster.CreatedAt = DateTime.Now; 
            productMaster.UpdatedAt = DateTime.Now;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"INSERT INTO ProductMaster 
                (ProductName, GenericName, ProductGroup_List, ProductGroup_Txt, PurchaseUnit_List, PurchaseUnit_Txt, Packing, StockUnit_List, StockUnit_Txt, 
                Rack_List, Rack_Txt, Bin_List, Bin_Txt, ProductType_List, ProductType_Txt, Manufacturer_List, Manufacturer_Txt, Remarks, 
                PurchaseRate, PurchaseDiscount, MRP, SaleRate, SaleDiscount, TaxPercent, HSN, MinStock, MaxStock, MinOrder, IsActive, CreatedAt, UpdatedAt) 
                VALUES (@ProductName, @GenericName, @ProductGroup_List, @ProductGroup_Txt, @PurchaseUnit_List, @PurchaseUnit_Txt, @Packing, @StockUnit_List, @StockUnit_Txt, 
                @Rack_List, @Rack_Txt, @Bin_List, @Bin_Txt, @ProductType_List, @ProductType_Txt, @Manufacturer_List, @Manufacturer_Txt, @Remarks, 
                @PurchaseRate, @PurchaseDiscount, @MRP, @SaleRate, @SaleDiscount, @TaxPercent, @HSN, @MinStock, @MaxStock, @MinOrder, @IsActive, @CreatedAt, @UpdatedAt)", conn);
            cmd.Parameters.AddWithValue("@ProductName", productMaster.ProductName);
            cmd.Parameters.AddWithValue("@GenericName", productMaster.GenericName);
            cmd.Parameters.AddWithValue("@ProductGroup_List", productMaster.ProductGroup_List);
            cmd.Parameters.AddWithValue("@ProductGroup_Txt", productMaster.ProductGroup_Txt);
            cmd.Parameters.AddWithValue("@PurchaseUnit_List", productMaster.PurchaseUnit_List);
            cmd.Parameters.AddWithValue("@PurchaseUnit_Txt", productMaster.PurchaseUnit_Txt);
            cmd.Parameters.AddWithValue("@Packing", productMaster.Packing);
            cmd.Parameters.AddWithValue("@StockUnit_List", productMaster.StockUnit_List);
            cmd.Parameters.AddWithValue("@StockUnit_Txt", productMaster.StockUnit_Txt);
            cmd.Parameters.AddWithValue("@Rack_List", productMaster.Rack_List);
            cmd.Parameters.AddWithValue("@Rack_Txt", productMaster.Rack_Txt);
            cmd.Parameters.AddWithValue("@Bin_List", productMaster.Bin_List);
            cmd.Parameters.AddWithValue("@Bin_Txt", productMaster.Bin_Txt);
            cmd.Parameters.AddWithValue("@ProductType_List", productMaster.ProductType_List);
            cmd.Parameters.AddWithValue("@ProductType_Txt", productMaster.ProductType_Txt);
            cmd.Parameters.AddWithValue("@Manufacturer_List", productMaster.Manufacturer_List);
            cmd.Parameters.AddWithValue("@Manufacturer_Txt", productMaster.Manufacturer_Txt);
            cmd.Parameters.AddWithValue("@Remarks", productMaster.Remarks);
            cmd.Parameters.AddWithValue("@PurchaseRate", productMaster.PurchaseRate);
            cmd.Parameters.AddWithValue("@PurchaseDiscount", productMaster.PurchaseDiscount);
            cmd.Parameters.AddWithValue("@MRP", productMaster.MRP);
            cmd.Parameters.AddWithValue("@SaleRate", productMaster.SaleRate);
            cmd.Parameters.AddWithValue("@SaleDiscount", productMaster.SaleDiscount);
            cmd.Parameters.AddWithValue("@TaxPercent", productMaster.TaxPercent);
            cmd.Parameters.AddWithValue("@HSN", productMaster.HSN);
            cmd.Parameters.AddWithValue("@MinStock", productMaster.MinStock);
            cmd.Parameters.AddWithValue("@MaxStock", productMaster.MaxStock);
            cmd.Parameters.AddWithValue("@MinOrder", productMaster.MinOrder);
            cmd.Parameters.AddWithValue("@IsActive", productMaster.IsActive);
            cmd.Parameters.AddWithValue("@CreatedAt", productMaster.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", productMaster.UpdatedAt);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
            //}
            //ViewData["RoleID"] = new SelectList(_context.Roles, "RoleId", "RoleId", applicationUser.RoleID);
        }

        // GET: ProductMaster/Edit/5
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            /*
            List<SelectListItem> Roles = new List<SelectListItem>();
            using var conn = _db.GetConnection();
            conn.Open();

            var roleCmd = new MySqlCommand("SELECT RoleId, RoleName FROM Roles", conn);
            using var roleReader = roleCmd.ExecuteReader();
            while (roleReader.Read())
            {
                Roles.Add(new SelectListItem
                {
                    Value = Convert.ToString(roleReader.GetGuid("RoleId")),
                    Text = roleReader.GetString("RoleName")
                });
            }
            roleReader.Close();
            */
            using var conn = _db.GetConnection();
            conn.Open();
            ProductMaster product = null;
            var cmd = new MySqlCommand("SELECT * FROM ProductMaster WHERE ProductCode=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                product = new ProductMaster
                {
                    ProductCode = reader.GetInt32("ProductCode"),
                    ProductName = reader.GetString("ProductName"),
                    GenericName = reader.GetString("GenericName"),
                    ProductGroup_List = reader.GetString("ProductGroup_List"),
                    ProductGroup_Txt = reader.GetString("ProductGroup_Txt"),
                    PurchaseUnit_List = reader.GetString("PurchaseUnit_List"),
                    PurchaseUnit_Txt = reader.GetString("PurchaseUnit_Txt"),
                    Packing = reader.GetInt32("Packing"),
                    StockUnit_List = reader.GetString("StockUnit_List"),
                    StockUnit_Txt = reader.GetString("StockUnit_Txt"),
                    Rack_List = reader.GetString("Rack_List"),
                    Rack_Txt = reader.GetString("Rack_Txt"),
                    Bin_List = reader.GetString("Bin_List"),
                    Bin_Txt = reader.GetString("Bin_Txt"),
                    ProductType_List = reader.GetString("ProductType_List"),
                    ProductType_Txt = reader.GetString("ProductType_Txt"),
                    Manufacturer_List = reader.GetString("Manufacturer_List"),
                    Manufacturer_Txt = reader.GetString("Manufacturer_Txt"),
                    Remarks = reader.GetString("Remarks"),
                    PurchaseRate = reader.GetDecimal("PurchaseRate"),
                    PurchaseDiscount = reader.GetDecimal("PurchaseDiscount"),
                    MRP = reader.GetDecimal("MRP"),
                    SaleRate = reader.GetDecimal("SaleRate"),
                    SaleDiscount = reader.GetDecimal("SaleDiscount"),
                    TaxPercent = reader.GetDecimal("TaxPercent"),
                    HSN = reader.GetString("HSN"),
                    MinStock = reader.GetDecimal("MinStock"),
                    MaxStock = reader.GetDecimal("MaxStock"),
                    MinOrder = reader.GetDecimal("MinOrder"),
                    IsActive = reader.GetBoolean("IsActive"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                };
            }

            ProductMasterViewModel productMasterVM = _mapper.Map<ProductMasterViewModel>(product);
            return View(productMasterVM);
            //ViewData["RoleID"] = new SelectList(_context.Roles, "RoleId", "RoleId", applicationUser.RoleID);
        }

        // POST: ProductMaster/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ProductMasterViewModel productMasterViewModel)
        {
            if (id != productMasterViewModel.ProductCode)
            {
                return NotFound();
            }
            ProductMaster productMaster = _mapper.Map<ProductMaster>(productMasterViewModel);
            productMaster.UpdatedAt = DateTime.Now;
            
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"UPDATE ProductMaster SET 
                ProductName=@ProductName, GenericName=@GenericName, ProductGroup_List=@ProductGroup_List, ProductGroup_Txt=@ProductGroup_Txt, PurchaseUnit_List=@PurchaseUnit_List, PurchaseUnit_Txt=@PurchaseUnit_Txt, Packing=@Packing, 
                StockUnit_List=@StockUnit_List, StockUnit_Txt=@StockUnit_Txt, Rack_List=@Rack_List, Rack_Txt=@Rack_Txt, Bin_List=@Bin_List, Bin_Txt=@Bin_Txt, ProductType_List=@ProductType_List, ProductType_Txt=@ProductType_Txt, Manufacturer_List=@Manufacturer_List, Manufacturer_Txt=@Manufacturer_Txt, Remarks=@Remarks, 
                PurchaseRate=@PurchaseRate, PurchaseDiscount=@PurchaseDiscount, MRP=@MRP, SaleRate=@SaleRate, SaleDiscount=@SaleDiscount, TaxPercent=@TaxPercent, HSN=@HSN, MinStock=@MinStock, 
                MaxStock=@MaxStock, MinOrder=@MinOrder, IsActive=@IsActive, UpdatedAt=@UpdatedAt WHERE ProductCode=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", productMaster.ProductCode);
            cmd.Parameters.AddWithValue("@ProductName", productMaster.ProductName);
            cmd.Parameters.AddWithValue("@GenericName", productMaster.GenericName);
            cmd.Parameters.AddWithValue("@ProductGroup_List", productMaster.ProductGroup_List);
            cmd.Parameters.AddWithValue("@ProductGroup_Txt", productMaster.ProductGroup_Txt);
            cmd.Parameters.AddWithValue("@PurchaseUnit_List", productMaster.PurchaseUnit_List);
            cmd.Parameters.AddWithValue("@PurchaseUnit_Txt", productMaster.PurchaseUnit_Txt);
            cmd.Parameters.AddWithValue("@Packing", productMaster.Packing);
            cmd.Parameters.AddWithValue("@StockUnit_List", productMaster.StockUnit_List);
            cmd.Parameters.AddWithValue("@StockUnit_Txt", productMaster.StockUnit_Txt);
            cmd.Parameters.AddWithValue("@Rack_List", productMaster.Rack_List);
            cmd.Parameters.AddWithValue("@Rack_Txt", productMaster.Rack_Txt);
            cmd.Parameters.AddWithValue("@Bin_List", productMaster.Bin_List);
            cmd.Parameters.AddWithValue("@Bin_Txt", productMaster.Bin_Txt);
            cmd.Parameters.AddWithValue("@ProductType_List", productMaster.ProductType_List);
            cmd.Parameters.AddWithValue("@ProductType_Txt", productMaster.ProductType_Txt);
            cmd.Parameters.AddWithValue("@Manufacturer_List", productMaster.Manufacturer_List);
            cmd.Parameters.AddWithValue("@Manufacturer_Txt", productMaster.Manufacturer_Txt);
            cmd.Parameters.AddWithValue("@Remarks", productMaster.Remarks);
            cmd.Parameters.AddWithValue("@PurchaseRate", productMaster.PurchaseRate);
            cmd.Parameters.AddWithValue("@PurchaseDiscount", productMaster.PurchaseDiscount);
            cmd.Parameters.AddWithValue("@MRP", productMaster.MRP);
            cmd.Parameters.AddWithValue("@SaleRate", productMaster.SaleRate);
            cmd.Parameters.AddWithValue("@SaleDiscount", productMaster.SaleDiscount);
            cmd.Parameters.AddWithValue("@TaxPercent", productMaster.TaxPercent);
            cmd.Parameters.AddWithValue("@HSN", productMaster.HSN);
            cmd.Parameters.AddWithValue("@MinStock", productMaster.MinStock);
            cmd.Parameters.AddWithValue("@MaxStock", productMaster.MaxStock);
            cmd.Parameters.AddWithValue("@MinOrder", productMaster.MinOrder);
            cmd.Parameters.AddWithValue("@IsActive", productMaster.IsActive);
            cmd.Parameters.AddWithValue("@CreatedAt", productMaster.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", productMaster.UpdatedAt);
            cmd.ExecuteNonQuery();

            // if (ModelState.IsValid)
            // {
            return RedirectToAction(nameof(Index));
            //}
            /*
            ApplicationUserViewModel appUserVM = _mapper.Map<ApplicationUserViewModel>(applicationUser);
            List<SelectListItem> Roles = new List<SelectListItem>();
            var roleCmd = new MySqlCommand("SELECT RoleId, RoleName FROM Roles", conn);
            using var roleReader = roleCmd.ExecuteReader();
            while (roleReader.Read())
            {
                Roles.Add(new SelectListItem
                {
                    Value = roleReader.GetString("RoleId"),
                    Text = roleReader.GetString("RoleName")
                });
            }
            roleReader.Close();
            appUserVM.Roles = Roles;
            //ViewData["RoleID"] = new SelectList(_context.Roles, "RoleId", "RoleId", applicationUser.RoleID);
            return View(appUserVM);
            */
        }

        // GET: ProductMaster/Delete/5
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            ProductMaster product = null;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM ProductMaster WHERE ProductCode=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                product = new ProductMaster
                {
                    ProductCode = reader.GetInt32("ProductCode"),
                    ProductName = reader.GetString("ProductName"),
                    GenericName = reader.GetString("GenericName"),
                    ProductGroup_List = reader.GetString("ProductGroup_List"),
                    ProductGroup_Txt = reader.GetString("ProductGroup_Txt"),
                    PurchaseUnit_List = reader.GetString("PurchaseUnit_List"),
                    PurchaseUnit_Txt = reader.GetString("PurchaseUnit_Txt"),
                    Packing = reader.GetInt32("Packing"),
                    StockUnit_List = reader.GetString("StockUnit_List"),
                    StockUnit_Txt = reader.GetString("StockUnit_Txt"),
                    Rack_List = reader.GetString("Rack_List"),
                    Rack_Txt = reader.GetString("Rack_Txt"),
                    Bin_List = reader.GetString("Bin_List"),
                    Bin_Txt = reader.GetString("Bin_Txt"),
                    ProductType_List = reader.GetString("ProductType_List"),
                    ProductType_Txt = reader.GetString("ProductType_Txt"),
                    Manufacturer_List = reader.GetString("Manufacturer_List"),
                    Manufacturer_Txt = reader.GetString("Manufacturer_Txt"),
                    Remarks = reader.GetString("Remarks"),
                    PurchaseRate = reader.GetDecimal("PurchaseRate"),
                    PurchaseDiscount = reader.GetDecimal("PurchaseDiscount"),
                    MRP = reader.GetDecimal("MRP"),
                    SaleRate = reader.GetDecimal("SaleRate"),
                    SaleDiscount = reader.GetDecimal("SaleDiscount"),
                    TaxPercent = reader.GetDecimal("TaxPercent"),
                    HSN = reader.GetString("HSN"),
                    MinStock = reader.GetDecimal("MinStock"),
                    MaxStock = reader.GetDecimal("MaxStock"),
                    MinOrder = reader.GetDecimal("MinOrder"),
                    IsActive = reader.GetBoolean("IsActive"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                };
            }
            ProductMasterViewModel productMasterVM = _mapper.Map<ProductMasterViewModel>(product);
            return View(productMasterVM);
        }

        // POST: ProductMaster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM ProductMaster WHERE ProductCode=@ProductCode", conn);
            cmd.Parameters.AddWithValue("@ProductCode", id);
            cmd.ExecuteNonQuery();
            return RedirectToAction(nameof(Index));
        }
    }
}
