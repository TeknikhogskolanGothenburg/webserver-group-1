using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;
using System.Security.Cryptography;
namespace Homework
{
    class Program
    {
        public static HttpListener listener = new HttpListener();

        static void Main(string[] prefixes)
        {            
            FileReader myFileReader = new FileReader();
            SiteResources a = new SiteResources();            
            try
            {
                if (!HttpListener.IsSupported)
                {
                    Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                    return;
                }
                // URI prefixes are required,
                // for example "http://contoso.com:8080/index/".
                // Add the prefixes.
                    foreach (string s in prefixes)
                    {
                        listener.Prefixes.Add(s);
                    }
                    if (prefixes == null || prefixes.Length == 0)
                    {
                        throw new ArgumentException("prefixes");

                    }
                    listener.Start();
                    Console.WriteLine("Listening...try again if you want.");
                while(true)
                {
                    // Note: The GetContext method blocks while waiting for a request. 
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    // Obtain a response object.
                    HttpListenerResponse response = context.Response;
                    // Construct a response.
                    SiteResources resources = new SiteResources();

                    //set header status code
                    response.StatusCode = resources.GetStatusCode(request.RawUrl);

                    int cookieCount = 0;
                    foreach (Cookie c in request.Cookies)
                    {
                        cookieCount += 1;
                    }

                    if(cookieCount > 0)
                    {
                        string cookieValue = request.Cookies[0].Value;
                        Console.WriteLine("THERE ARE COOKIES PRESENT!!! " + cookieValue + "  How many cookies :" + cookieCount);
                        Cookie updatedCookie = request.Cookies[0];
                        //updatedCookie.Value = "counter=" + (Convert.ToInt32(cookieValue.Substring(8, cookieValue.Length - 8)) + 1).ToString();

                        response.Cookies.Add(updatedCookie);
                    }
                    else
                    {
                        response.Cookies.Add(new Cookie("superAwsomeWebServer_sessionCookie", "counter=1"));
                    }

                    byte[] buffer = new byte[] { };
                    string pureRequestString = resources.CleanRawUrl(request.RawUrl);
                                            
                    Console.WriteLine("RAW: " + request.RawUrl);                    
                    if (pureRequestString == "")
                    {
                        buffer = new byte[0];
                    }                    
                    else
                    {
                        if (pureRequestString == "/dynamic")
                        {                            
                            response.ContentType = "text/html";
                            if (request.QueryString.Count == 2)
                            {
                                bool xml = false;
                                foreach (string header in request.Headers.AllKeys)
                                {
                                    Console.WriteLine("Header :" + header);
                                    if(request.Headers.Get("Accept") == "application/xml")
                                    {
                                        xml = true;
                                    }
                                }
                                int sum = Convert.ToInt32(request.QueryString.Get(0)) + Convert.ToInt32(request.QueryString.Get(1));
                                if (xml)
                                {
                                    buffer = Encoding.UTF8.GetBytes("<result><value>" + sum + "</value></result>");
                                    response.ContentType = "application/xml";
                                }
                                else
                                {
                                    buffer = Encoding.UTF8.GetBytes("<html><body>" + sum + "</body></html>");
                                }
                            }
                            else
                            {
                                buffer = Encoding.UTF8.GetBytes("Missing input value");
                                response.StatusCode = 500;
                            }
                        }
                        else
                        {
                            buffer = resources.GetOutputContent(pureRequestString);
                            response.ContentType = resources.GetOutputType(pureRequestString);

                        }
                        //setting more headers
                        response.Headers.Add("Expires", resources.GetExpiresValue());
                        // Get a response stream and write the response to it.
                    }
                    response.ContentLength64 = buffer.Length;
                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // You must close the output stream.
                    output.Close();
                    
                }
            }
            catch(WebException e)
            {
                Console.WriteLine(e.Status);
            }
        }
    }
}
