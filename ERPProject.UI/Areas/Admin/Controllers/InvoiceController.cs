using ClosedXML.Excel;
using ERPProject.Entity.DTO.InvoiceDetailDTO;
using ERPProject.Entity.DTO.InvoiceDTO;
using ERPProject.UI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using System.Xml;

namespace ERPProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InvoiceController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly string url = "https://localhost:7075/";
        public InvoiceController(HttpClient httpClient, IWebHostEnvironment hostingEnvironment) : base(httpClient)
        {
            _hostingEnvironment = hostingEnvironment;

        }
        [HttpGet("/Admin/Faturalar")]
        public async Task<IActionResult> Index()
        {
            var val = await GetAllAsync<InvoiceDTOResponse>(url + "GetInvoices");
            var val2 = await GetAllAsync<InvoiceDetailDTOResponse>(url + "GetInvoiceDetails");
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Muhasebe" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            InvoiceVM invoiceVM = new InvoiceVM()
            {
                Invoices = val.Data,
                InvoiceDetail = val2.Data
            };
            return View(invoiceVM);
        }
        [HttpGet("/Admin/Fatura")]
        public async Task<IActionResult> Get(long id)
        {
            var val = await GetAsync<InvoiceDTOResponse>(url + "GetInvoice/" + id);
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Muhasebe" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val.Data != null)
            {
                return View(val);
            }
            if (val== null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return RedirectToAction("Forbidden", "Home");
        }
        [HttpPost("/Admin/FaturaEkle")]
        public async Task<IActionResult> Add(IFormFile FileUpload)
        {
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Muhasebe" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            XmlDocument xmlVerisi = new XmlDocument();
            xmlVerisi.Load("https://www.tcmb.gov.tr/kurlar/today.xml");
            decimal usd = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "USD")).InnerText.Replace('.', ','));
            decimal eur = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "EUR")).InnerText.Replace('.', ','));
            decimal jpy = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "JPY")).InnerText.Replace('.', ','));

            if (FileUpload != null)
            {
                System.Data.DataTable dt = new System.Data.DataTable();

                // excel dosyamızı stream'e çeviriyoruz
                using (var ms = new MemoryStream())
                {
                    FileUpload.CopyTo(ms);

                    // excel dosyamızı streamden okuyoruz
                    using (var workbook = new XLWorkbook(ms))
                    {
                        var worksheet = workbook.Worksheet(1); // sayfa 
                                                               // sayfada kaç sütun kullanılmış onu buluyoruz ve sütunları DataTable'a ekliyoruz, ilk satırda sütun başlıklarımız var
                        int i, n = worksheet.Columns().Count();
                        for (i = 1; i <= n; i++)
                        {
                            dt.Columns.Add(worksheet.Cell(1, i).Value.ToString());
                        }

                        // sayfada kaç satır kullanılmış onu buluyoruz ve DataTable'a satırlarımızı ekliyoruz
                        n = worksheet.Rows().Count();
                        for (i = 2; i <= n; i++)
                        {
                            DataRow dr = dt.NewRow();

                            int j, k = worksheet.Columns().Count();
                            for (j = 1; j <= k; j++)
                            {
                                // i= satır index, j=sütun index, closedXML worksheet için indexler 1'den başlıyor, ama datatable için 0'dan başladığı için j-1 diyoruz
                                dr[j - 1] = worksheet.Cell(i, j).Value;
                            }

                            dt.Rows.Add(dr);
                        }
                        InvoiceDTORequest dTORequest = null;
                        long id = 0;
                        for (i = 0; i < n - 1; i++)
                        {
                            DataRow row = dt.Rows[i];
                            dTORequest = new InvoiceDTORequest
                            {
                                SupplierName = row["Tedarikci"].ToString(),
                                CompanyName = row["Şirket"].ToString(),
                                TotalPrice = Convert.ToInt32(row["Fiyat"]),
                                PriceStatus = row["Para Birimi"].ToString(),
                                InvoiceDate = Convert.ToDateTime(row["Tarih"])
                            };
                        }
                        if (dTORequest.PriceStatus == "USD")
                        {
                            dTORequest.PriceStatus = "1";
                            dTORequest.Rate = usd;
                        }
                        else if (dTORequest.PriceStatus == "EUR")
                        {
                            dTORequest.PriceStatus = "2";
                            dTORequest.Rate = eur;
                        }
                        else if (dTORequest.PriceStatus == "JPY")
                        {
                            dTORequest.PriceStatus = "3";
                            dTORequest.Rate = jpy;
                        }
                        else if (dTORequest.PriceStatus == "TRY")
                        {
                            dTORequest.PriceStatus = "0";
                            dTORequest.Rate = 1;
                        }
                        dTORequest.AddedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
                        dTORequest.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
                        var val = await AddAsync(dTORequest, url + "AddInvoice");
                        id = val.Data.Id;
                        InvoiceDetailDTORequest invoiceDetailDTORequest = null;
                        for (i = 0; i < n - 2; i++)
                        {
                            DataRow row = dt.Rows[i];
                            invoiceDetailDTORequest = new InvoiceDetailDTORequest
                            {
                                ProductName = row["Tedarikci"].ToString(),
                                Price = Convert.ToInt32(row["Fiyat"]),
                                Quantity = Convert.ToInt32(row["Miktar"]),
                                QuantityUnit = row["Birim"].ToString(),
                                PriceStatus = row["Para Birimi"].ToString(),
                                InvoiceId = id,
                            };
                            #region birim
                            if (invoiceDetailDTORequest.QuantityUnit.ToLower().Contains("metre"))
                            {
                                invoiceDetailDTORequest.QuantityUnit = "1";
                            }
                            else if (invoiceDetailDTORequest.QuantityUnit.ToLower().Contains("kilometre"))
                            {
                                invoiceDetailDTORequest.QuantityUnit = "2";

                            }
                            else if (invoiceDetailDTORequest.QuantityUnit.ToLower().Contains("mililitre"))
                            {
                                invoiceDetailDTORequest.QuantityUnit = "3";

                            }
                            else if (invoiceDetailDTORequest.QuantityUnit.ToLower().Contains("litre"))
                            {
                                invoiceDetailDTORequest.QuantityUnit = "4";

                            }
                            else if (invoiceDetailDTORequest.QuantityUnit.ToLower().Contains("kilogram"))
                            {
                                invoiceDetailDTORequest.QuantityUnit = "5";

                            }
                            else if (invoiceDetailDTORequest.QuantityUnit.ToLower().Contains("ton"))
                            {
                                invoiceDetailDTORequest.QuantityUnit = "6";

                            }
                            else if (invoiceDetailDTORequest.QuantityUnit.ToLower().Contains("adet"))
                            {
                                invoiceDetailDTORequest.QuantityUnit = "7";

                            }
                            else if (invoiceDetailDTORequest.QuantityUnit.ToLower().Contains("koli"))
                            {
                                invoiceDetailDTORequest.QuantityUnit = "8";

                            }
                            else if (invoiceDetailDTORequest.QuantityUnit.ToLower().Contains("metrekare") || invoiceDetailDTORequest.QuantityUnit == "m²")
                            {
                                invoiceDetailDTORequest.QuantityUnit = "9";

                            }
                            else if (invoiceDetailDTORequest.QuantityUnit.ToLower().Contains("metreküp") || invoiceDetailDTORequest.QuantityUnit == "m³")
                            {
                                invoiceDetailDTORequest.QuantityUnit = "10";

                            }
                            #endregion
                            #region money
                            if (dTORequest.PriceStatus == "USD")
                            {
                                dTORequest.PriceStatus = "1";
                                dTORequest.Rate = usd;
                            }
                            else if (dTORequest.PriceStatus == "EUR")
                            {
                                dTORequest.PriceStatus = "2";
                                dTORequest.Rate = eur;
                            }
                            else if (dTORequest.PriceStatus == "JPY")
                            {
                                dTORequest.PriceStatus = "3";
                                dTORequest.Rate = jpy;
                            }
                            else if (dTORequest.PriceStatus == "TRY")
                            {
                                dTORequest.PriceStatus = "0";
                                dTORequest.Rate = 1;
                            }
                            #endregion
                            var val2 = await AddAsync(invoiceDetailDTORequest, url + "AddInvoiceDetail");
                        }

                        //if (val)
                        //{
                        //    return RedirectToAction("Index", "Invoice");

                        //}
                        return RedirectToAction("Index", "Invoice");

                    }
                }
            }
            return RedirectToAction("Index", "Invoice");
        }
        [HttpPost("/Admin/FaturaGuncelle")]
        public async Task<IActionResult> Update(InvoiceDTORequest p)
        {
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Muhasebe" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await UpdateAsync(p, url + "UpdateInvoice");
            if (val == null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val.Data!=null)
            {
                return RedirectToAction("Index", "Invoice");

            }
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return RedirectToAction("Forbidden", "Home");

        }
        [HttpPost("/Admin/FaturaSil")]
        public async Task<IActionResult> Delete(long id)
        {
            var val = await DeleteAsync(url + "RemoveInvoice/" + id);
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Muhasebe" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val==null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val)
            {
                return RedirectToAction("Index", "Invoice");

            }
            return RedirectToAction("Forbidden", "Home");
        }
        [HttpGet("/Admin/Raporlama")]
        public async Task<IActionResult> Report(DateTime startDate, DateTime endDate)
        {
            string dep = HttpContext.Session.GetString("DepartmentName");
            if (dep == "Muhasebe" || dep == "Admin" || dep == "Yönetim")
            {
                var val = await GetAllAsync<InvoiceDTOResponse>(url + "GetInvoices");
                var val2 = await GetAllAsync<InvoiceDetailDTOResponse>(url + "GetInvoiceDetails");
                if (val.StatusCode == 401)
                {
                    return RedirectToAction("Unauthorized", "Home");
                }
                else if (val.StatusCode == 403)
                {
                    return RedirectToAction("Forbidden", "Home");
                }

                InvoiceVM invoiceVM = new InvoiceVM()
                {
                    Invoices = val.Data,
                    InvoiceDetail = val2.Data
                };
                return View(invoiceVM);
            }
            return RedirectToAction("Forbidden", "Home");

        }
        [HttpPost("/Admin/Raporlama")]
        public async Task<IActionResult> Report(string datefilter)
        {
            string dep = HttpContext.Session.GetString("DepartmentName");
            if (dep == "Muhasebe" || dep == "Admin" || dep == "Yönetim")
            {
                if (datefilter == null)
                {
                    return RedirectToAction("Report", "Invoice");
                }
                var date = datefilter.Replace(" ", "").Replace("/", ".");
                var val = await GetAllAsync<InvoiceDTOResponse>(url + "GetInvoicesByDate/" + date);
                var val2 = await GetAllAsync<InvoiceDetailDTOResponse>(url + "GetInvoiceDetails");
                if (val.StatusCode == 401)
                {
                    return RedirectToAction("Unauthorized", "Home");
                }
                else if (val.StatusCode == 403)
                {
                    return RedirectToAction("Forbidden", "Home");
                }

                InvoiceVM invoiceVM = new InvoiceVM()
                {
                    Invoices = val.Data,
                    InvoiceDetail = val2.Data
                };
                return View(invoiceVM);
            }
            return RedirectToAction("Forbidden", "Home");
        }


    }
}

