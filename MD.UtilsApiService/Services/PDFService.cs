using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MD.UtilsApiService.Services
{
    public class PDFService
    {
        public byte[] PDFSetPicWatermark(byte[] inputPDFByte, byte[] modelPicByte,
            float absoluteX, float absoluteY,  float scalePercent =40f, float rotationDegrees = 0f)
        {
            //throw new NotImplementedException();
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            FileStream fileStream = null;
            try
            {
                pdfReader = new PdfReader(inputPDFByte);
                int numberOfPages = pdfReader.NumberOfPages;

                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);

                float width = psize.Width;

                float height = psize.Height;
                var executablePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string tempPath = executablePath + "temp.pdf";
                fileStream = new FileStream(tempPath, FileMode.Create);
                pdfStamper = new PdfStamper(pdfReader, fileStream);

                PdfContentByte waterMarkContent;

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(modelPicByte);
                image.ScalePercent(scalePercent);
                image.GrayFill = 100;//透明度，灰色填充
                //image.Rotation = 1;//旋转, (和下面旋转角度 公用一个属性, 旋转角度更容易设置)
                image.RotationDegrees = rotationDegrees;//旋转角度
                //水印的位置 
                if (absoluteX < 0)
                {
                    absoluteX = width / 2 - image.Width;
                }

                //image.SetAbsolutePosition(left, (height - image.Height) - top);
                image.SetAbsolutePosition(absoluteX, absoluteY);


                //每一页加水印,也可以设置某一页加水印 
                for (int i = 1; i <= numberOfPages; i++)
                {
                    waterMarkContent = pdfStamper.GetUnderContent(i);//内容下层加水印
                    //waterMarkContent = pdfStamper.GetOverContent(i);//内容上层加水印

                    waterMarkContent.AddImage(image);
                }
                pdfStamper.Close();
                pdfReader.Close();
                fileStream.Close();
                fileStream.Dispose();
                var bytes = File.ReadAllBytes(tempPath);
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
                return bytes;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {

            }
        }

        public byte[] PDFSetPicWatermark2(string inputfilepath, string ModelPicName,
    float top, float left, int scalePercent, int grayFill, float RotationDegrees)
        {
            //throw new NotImplementedException();
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            FileStream fileStream = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath);
                int numberOfPages = pdfReader.NumberOfPages;

                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);

                float width = psize.Width;

                float height = psize.Height;
                var executablePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string tempPath = executablePath + "temp.pdf";
                fileStream = new FileStream(tempPath, FileMode.Create);
                pdfStamper = new PdfStamper(pdfReader, fileStream);

                PdfContentByte waterMarkContent;

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(ModelPicName);
                image.ScalePercent(40);
                image.GrayFill = 100;//透明度，灰色填充
                //image.Rotation = 1;//旋转
                image.RotationDegrees = RotationDegrees;//旋转角度
                //水印的位置 
                if (left < 0)
                {
                    left = width / 2 - image.Width;
                }

                //image.SetAbsolutePosition(left, (height - image.Height) - top);
                image.SetAbsolutePosition(left, top);


                //每一页加水印,也可以设置某一页加水印 
                for (int i = 1; i <= numberOfPages; i++)
                {
                    waterMarkContent = pdfStamper.GetUnderContent(i);//内容下层加水印
                    //waterMarkContent = pdfStamper.GetOverContent(i);//内容上层加水印

                    waterMarkContent.AddImage(image);
                }
                pdfStamper.Close();
                pdfReader.Close();
                fileStream.Close();
                fileStream.Dispose();
                var bytes = File.ReadAllBytes(tempPath);
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
                return bytes;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {

            }
        }
    }
}
