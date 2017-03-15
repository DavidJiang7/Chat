using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Chat.Utils
{
    public class Http
    {
        /// <summary>
        /// 随机UserAgent
        /// </summary>
        private static string[] UserAgent = {
        "Mozilla/5.0 (compatible, MSIE 10.0, Windows NT, DigExt)",
        "Mozilla/4.0 (compatible, MSIE 7.0, Windows NT 5.1, 360SE)",
        "Mozilla/4.0 (compatible, MSIE 8.0, Windows NT 6.0, Trident/4.0)",
        "Mozilla/5.0 (compatible, MSIE 9.0, Windows NT 6.1, Trident/5.0,",
        "Opera/9.80 (Windows NT 6.1, U, en) Presto/2.8.131 Version/11.11",
        "Mozilla/4.0 (compatible, MSIE 7.0, Windows NT 5.1, TencentTraveler 4.0)",
        "Mozilla/5.0 (Windows, U, Windows NT 6.1, en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50",
        "Mozilla/5.0 (Macintosh, Intel Mac OS X 10_7_0) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11",
        "Mozilla/5.0 (Macintosh, U, Intel Mac OS X 10_6_8, en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50",
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/7.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.3)",
        "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko",
        "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:50.0) Gecko/20100101 Firefox/50.0",
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.75 Safari/537.36",
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36"
        };
        /// <summary>
        /// 随机返回一条UserAgent
        /// </summary>
        /// <returns></returns>
        public static string GetRandUserAgent()
        {
            Random ran = new Random();
            //Console.WriteLine("随机示例输出：{0}", ran.Next(10, 10));
            return UserAgent[ran.Next(UserAgent.Length)];
        }

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="encode">字符编码</param>
        /// <returns>返回html页面</returns>
        public static string Get(string url, Encoding encode)
        {
            if (url == "")
            {
                return "";
            }
            try
            {
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
                webReq.Method = "GET";
                webReq.UserAgent = GetRandUserAgent();
                //Console.WriteLine("当前随机的UserAgent：" + webReq.UserAgent);
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponseAsync().Result;
                StreamReader sr = new StreamReader(response.GetResponseStream(), encode);
                string data = sr.ReadToEnd();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    sr.Close();
                    response.Close();
                    Console.WriteLine(data);
                    return "";
                }
                else
                {
                    sr.Close();
                    response.Close();
                    return data;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }
        

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <returns>返回json数据字符串</returns>
        public static string Post(string url, IDictionary<string, object> parameters, Encoding charset)
        {
            try
            {
                //HTTPSQ请求  
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = url.Length;

                //request.UserAgent = DefaultUserAgent;
                //如果需要POST数据     
                if (!(parameters == null || parameters.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        }
                        i++;
                    }
                    byte[] data = charset.GetBytes(buffer.ToString());
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                response.Close();
                //Console.WriteLine(data);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }
    }
}
