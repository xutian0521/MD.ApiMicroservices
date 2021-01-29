using MD.UtilsApiService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Net.Http;

namespace MD.UtilsApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFController : ControllerBase
    {
        PDFService _PDFService;
        public PDFController(PDFService pDFService)
        {
            _PDFService = pDFService;
        }
        [HttpPost("PDFSetPicWatermark")]
        public FileContentResult PDFSetPicWatermark([FromForm] IFormFile inputPDFByte, [FromForm] IFormFile modelPicByte,
            [FromForm] float absoluteX, [FromForm] float absoluteY, [FromForm] float scalePercent = 40f, [FromForm] float rotationDegrees = 0f)
        {
            byte[] inputPDFBytes= new byte[inputPDFByte.Length];
            inputPDFByte.OpenReadStream().Read(inputPDFBytes, 0, (int)inputPDFByte.Length);
            byte[] modelPicBytes = new byte[modelPicByte.Length];
            modelPicByte.OpenReadStream().Read(modelPicBytes, 0, (int)modelPicByte.Length);
            var bytes = _PDFService.PDFSetPicWatermark(inputPDFBytes, modelPicBytes, absoluteX, absoluteY, scalePercent, rotationDegrees);

            return new FileContentResult(bytes, "application/pdf");
        }

        [HttpPost("PDFSetPicWatermarkByFileUrl")]
        public async Task<FileContentResult> PDFSetPicWatermarkByFileUrl([FromForm] string inputPDFPath, [FromForm] string modelPicPath,
    [FromForm] float absoluteX, [FromForm] float absoluteY, [FromForm] float scalePercent = 40f, [FromForm] float rotationDegrees = 0f)
        {
            //NetworkStream stream = new NetworkStream();
            HttpClient client = new HttpClient();
            var r1 = await client.GetAsync(inputPDFPath);
            var r2 = await client.GetAsync(modelPicPath);
            byte[] inputPDFBytes = await r1.Content.ReadAsByteArrayAsync();
            byte[] modelPicBytes = await r2.Content.ReadAsByteArrayAsync();
            var bytes = _PDFService.PDFSetPicWatermark(inputPDFBytes, modelPicBytes, absoluteX, absoluteY, scalePercent, rotationDegrees);

            return new FileContentResult(bytes, "application/pdf");
        }
    }
}
