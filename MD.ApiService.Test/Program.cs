using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MD.ApiService.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Test1Async();
        }

        public static async Task Test1Async()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/json"));


            string pdfName = "异地贷款职工住房公积金缴存使用证明-编8.pdf";
            string pngName = "ElectronicSeal_hbgjj.png";

            var executablePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var b1 = System.IO.File.ReadAllBytes(executablePath + "/" + pdfName);
            var b2 = System.IO.File.ReadAllBytes(executablePath + "/" + pngName);

            var content = new MultipartFormDataContent();
            //string boundary = string.Format("--{0}", DateTime.Now.Ticks.ToString("x"));
            //content.Headers.Add("ContentType", $"multipart/form-data, boundary={boundary}");
            content.Add(new StringContent("111"), "absoluteX");
            content.Add(new StringContent("500"), "absoluteY");
            content.Add(new StringContent("1"), "scalePercent");
            content.Add(new StringContent("2"), "rotationDegrees");
            var binaryContent = new ByteArrayContent(b1);
            binaryContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            content.Add(binaryContent, "inputPDFByte", pdfName);
            var binaryContent2 = new ByteArrayContent(b2);
            binaryContent2.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            content.Add(binaryContent2, "modelPicByte", pngName);
            var result = await client.PostAsync("/api/PDF/PDFSetPicWatermark", content);
            result.EnsureSuccessStatusCode();
        }
    }
}
