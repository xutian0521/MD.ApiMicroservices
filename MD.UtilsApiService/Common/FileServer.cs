using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MD.UtilsApiService.Common
{
    public class FileServer
    {
        public static string UploadToFileServer(string path, string owner, string subdir)
        {
            //http://58.242.86.120:7013/api/upload?token=157620e3af96c202942a3d36a417571e&owner=ESB&subdir=贷款结清证明
            string fileServerApi = System.Configuration.ConfigurationManager.AppSettings["uploadUrl"];
            string fileServerAccessUrl = System.Configuration.ConfigurationManager.AppSettings["img_host"];
            string parms = $"token=157620e3af96c202942a3d36a417571e&owner={owner}&subdir={subdir}";


            var postData = parms;//转换成：para1=1&para2=2&para3=3
            var postUrl = string.Format("{0}?{1}", fileServerApi, postData);//拼接url
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
            request.AllowAutoRedirect = true;
            request.Method = "POST";

            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);

            //请求头部信息 
            StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] bArr = new byte[fs.Length];
            fs.Read(bArr, 0, bArr.Length);
            fs.Close();

            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            string content = sr.ReadToEnd();
            try
            {
                var JsonData = JsonConvert.DeserializeObject<dynamic>(content);
                string mPath = JsonData.data.FrPath;
                mPath = mPath.Replace("F:/img", "");
                mPath = mPath.Replace("E:/img", "");
                string httpUrl = fileServerAccessUrl + mPath;
                return httpUrl;
            }
            catch (Exception)
            {

                return content;
            }

        }
    }
}
