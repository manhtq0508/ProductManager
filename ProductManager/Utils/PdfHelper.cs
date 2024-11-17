using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using ProductManager.Entities;

using Cell = iText.Layout.Element.Cell;
using TextAlignment = iText.Layout.Properties.TextAlignment;

namespace ProductManager.Utils;


public class PdfHelper
{
    public static void Export(string filename, List<Product> products)
    {
        using (PdfWriter writer = new PdfWriter(filename))
        using (PdfDocument pdf = new PdfDocument(writer))
        using (Document document = new Document(pdf))
        {
            PdfFont font = PdfFontFactory.CreateFont("c:/windows/fonts/arial.ttf", PdfEncodings.IDENTITY_H);
            document.SetFont(font);

            Paragraph header = new Paragraph("DANH SÁCH SẢN PHẨM");
            header
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(25)
                .SetBold();

            document.Add(header);

            var table = new Table(5).UseAllAvailableWidth();

            Cell idHeader = new Cell().Add(
                new Paragraph("Mã sản phẩm")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
            );
            table.AddHeaderCell(idHeader);

            Cell nameHeader = new Cell().Add(
                new Paragraph("Tên sản phẩm")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
            );
            table.AddHeaderCell(nameHeader);

            Cell priceHeader = new Cell().Add(
                new Paragraph("Giá")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetBold()
            );
            table.AddHeaderCell(priceHeader);

            Cell amountHeader = new Cell().Add(
                new Paragraph("Số lượng")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
            );
            table.AddHeaderCell(amountHeader);

            Cell totalHeader = new Cell().Add(
                new Paragraph("Thành tiền")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
            );
            table.AddHeaderCell(totalHeader);

            foreach (Product product in products)
            {
                Cell id = new Cell().Add(
                    new Paragraph(product.Id)
                        .SetTextAlignment(TextAlignment.CENTER)
                );
                table.AddCell(id);

                Cell name = new Cell().Add(
                    new Paragraph(product.Name)
                        .SetTextAlignment(TextAlignment.LEFT)
                );
                table.AddCell(name);

                Cell price = new Cell().Add(
                    new Paragraph(product.Price.ToString("#,##0"))
                        .SetTextAlignment(TextAlignment.RIGHT)
                );
                table.AddCell(price);

                Cell amount = new Cell().Add(
                    new Paragraph(product.Amount.ToString("#,##0"))
                        .SetTextAlignment(TextAlignment.RIGHT)
                );
                table.AddCell(amount);

                var totalValue = product.Price * product.Amount;
                Cell total = new Cell().Add(
                    new Paragraph(totalValue.ToString("#,##0"))
                        .SetTextAlignment(TextAlignment.RIGHT)
                );
                table.AddCell(total);
            }

            document.Add(table);
            document.Close();
        }
    }
}