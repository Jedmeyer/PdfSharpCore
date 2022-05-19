using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;
using QRCoder;
using System;
using System.IO;
using static QRCoder.PayloadGenerator;

namespace PdfSharpCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalFontSettings.FontResolver = new FontResolver();
            var document = new PdfDocument();

            using (Stream stream = new MemoryStream(GenerateQRCode()))
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);

                var font = new XFont("OpenSans", 20, XFontStyle.Bold);

                var qrCode = XImage.FromStream(() => stream);
                gfx.DrawString("Access your Link:", font, XBrushes.Black, new XRect(20, 20, page.Width, 30), XStringFormats.Center);
                gfx.DrawImage(qrCode, new XPoint(15, 50));

                document.Save(Path.Combine(Directory.GetCurrentDirectory(), "result.pdf."));
            };
        }


        
        static byte[] GenerateQRCode()
        {
            Url generator = new Url("https://github.com/codebude/QRCoder/");
            string payload = generator.ToString();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            using (var qrCode = new BitmapByteQRCode(qrCodeData))
            {
                return qrCode.GetGraphic(20);
            }

        }
    }
}
