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
                while (true)
                {
                    // Note: The GetContext method blocks while waiting for a request. 
                    HttpListenerContext context = listener.GetContext();
                    // Construct a response.
                    HttpListenerResponse response = ResponseBuilder.Build(context, new SiteResources());
                    //Console.WriteLine("Method :" + request.HttpMethod);                    
                    byte[] buffer = ResponseBuilder.Buffer;
                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // You must close the output stream.
                    output.Close();
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Status);
            }
        }
    }
}
