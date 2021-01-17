using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MD.UtilsApiService.Model
{
    public class PDFSetPicWatermarkViewModel
    {
        public IFormFile inputPDFByte { get; set; }
        public IFormFile modelPicByte { get; set; }
        public float absoluteX { get; set; }
        public float absoluteY { get; set; }
        public float scalePercent { get; set; }
        public float rotationDegrees { get; set; }
    }
}
