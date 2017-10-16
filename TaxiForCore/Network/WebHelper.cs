using System.IO;
using System.Net;
using System.Text;
using TaxiForCore.StringHelper;

namespace TaxiForCore.Network
{
    public class WebResponseType
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
    }

    /// <summary>
    /// WEB请求帮助类
    /// </summary>
    public class WebHelper
    {
        /// <summary>
        /// Get方法请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task<WebResponseType> GetAsync(string url) => await BaseRequestAsync(url, "get");

        /// <summary>
        /// POST方法请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="context"></param>
        /// <param name="contentType">默认值“application/json”</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task<WebResponseType> PostAsync(string url, string context, string contentType = "application/json") => await BaseRequestAsync(url, "POST", context, contentType);


        /// <summary>
        /// 请求的基础方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="context"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task<WebResponseType> BaseRequestAsync(string url, string method,string context=null, string contentType=null)
        {
            WebResponseType content = new WebResponseType();
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = contentType;
            request.Method = method.ToLower();
            try
            {
                // context 不为空
                if (!context.IsNullOrEmpty())
                {
                    // 写入发送内容
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(context);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }
                
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    Stream receive = response.GetResponseStream();
                    StreamReader reader = new StreamReader(receive, Encoding.UTF8);
                    content.StatusCode = response.StatusCode;
                    content.Content = reader.ReadToEnd();
                }
            }
            catch (WebException exception)
            {
                using (var response = (HttpWebResponse)exception.Response)
                {
                    content.StatusCode = response.StatusCode;
                    Stream receive = response.GetResponseStream();
                    StreamReader reader = new StreamReader(receive, Encoding.UTF8);
                    content.Content = reader.ReadToEnd();
                }
            }

            return content;
        }
    }
}
