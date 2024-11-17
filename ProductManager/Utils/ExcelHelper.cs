using OfficeOpenXml;
using OfficeOpenXml.Style;
using ProductManager.Entities;
using ProductManager.Interfaces;
using ProductManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Utils;

public class ExcelHelper
{
    const char ID_COL = 'A';
    const char NAME_COL = 'B';
    const char PRICE_COL = 'C';
    const char AMOUNT_COL = 'D';
    const char TOTAL_COL = 'E';
    const byte START_ROW = 3;

    public static void Export(string filename, List<Product> products)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package  = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Products");

            // Column header
            worksheet.Cells[2, 1].Value = "Mã sản phẩm";
            worksheet.Cells[2, 1].Style.Font.Bold = true;
            worksheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            
            worksheet.Cells[2, 2].Value = "Tên sản phẩm";
            worksheet.Cells[2, 2].Style.Font.Bold = true;
            worksheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            
            worksheet.Cells[2, 3].Value = "Giá";
            worksheet.Cells[2, 3].Style.Font.Bold = true;
            worksheet.Cells[2, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            
            worksheet.Cells[2, 4].Value = "Số lượng";
            worksheet.Cells[2, 4].Style.Font.Bold = true;
            worksheet.Cells[2, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Cells[2, 5].Value = "Thành tiền";
            worksheet.Cells[2, 5].Style.Font.Bold = true;
            worksheet.Cells[2, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            // Data
            for (int i = 0; i < products.Count; i++)
            {
                worksheet.Cells[$"{ID_COL}{i + START_ROW}"].Value = products[i].Id;
                worksheet.Cells[$"{NAME_COL}{i + START_ROW}"].Value = products[i].Name;
                worksheet.Cells[$"{PRICE_COL}{i + START_ROW}"].Value = products[i].Price;
                worksheet.Cells[$"{PRICE_COL}{i + START_ROW}"].Style.Numberformat.Format = "#,##0";
                worksheet.Cells[$"{AMOUNT_COL}{i + START_ROW}"].Value = products[i].Amount;
                worksheet.Cells[$"{AMOUNT_COL}{i + START_ROW}"].Style.Numberformat.Format = "#,##0";

                worksheet.Cells[$"{TOTAL_COL}{i + START_ROW}"].Style.Numberformat.Format = "#,##0";
            }
            
            // Formula
            worksheet.Cells[$"{TOTAL_COL}{START_ROW}:{TOTAL_COL}{START_ROW + products.Count - 1}"].Formula = $"{PRICE_COL}{START_ROW} * {AMOUNT_COL}{START_ROW}";

            // Header
            worksheet.Cells[1, 1].Value = "QUẢN LÝ SẢN PHẨM";
            worksheet.Cells[$"{ID_COL}1:{TOTAL_COL}1"].Merge = true;
            worksheet.Cells[$"{ID_COL}1:{TOTAL_COL}1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[$"{ID_COL}1:{TOTAL_COL}1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[$"{ID_COL}1:{TOTAL_COL}1"].Style.Font.Bold = true;
            worksheet.Cells[$"{ID_COL}1:{TOTAL_COL}1"].Style.Font.Size = 20;

            worksheet.Cells.AutoFitColumns();

            package.SaveAs(filename);
        }
    }

    public static List<Product> Import(string filename)
    {
        List<Product> products = new List<Product>();
        List<string> idList = new List<string>();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(filename))
        {
            var worksheet = package.Workbook.Worksheets[0];

            int i = 0;
            while (worksheet.Cells[$"{ID_COL}{i + START_ROW}"].Value != null)
            {
                products.Add(new Product
                {
                    Id = worksheet.Cells[$"{ID_COL}{i + START_ROW}"].Value.ToString(),
                    Name = worksheet.Cells[$"{NAME_COL}{i + START_ROW}"].Value.ToString(),
                    Price = int.Parse(worksheet.Cells[$"{PRICE_COL}{i + START_ROW}"].Value.ToString() ?? "0"),
                    Amount = int.Parse(worksheet.Cells[$"{AMOUNT_COL}{i + START_ROW}"].Value.ToString() ?? "0")
                });

                if (idList.Contains(products.Last().Id ?? ""))
                    throw new Exception("Mã sản phẩm trùng lặp");

                idList.Add(products.Last().Id ?? "");

                i++;
            }
        }

        return products;
    }
}
