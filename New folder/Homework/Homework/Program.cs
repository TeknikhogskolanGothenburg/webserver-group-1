using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;

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
                while(true)
                {
                    listener.Start();
                    foreach (string s in prefixes)
                    {
                        listener.Prefixes.Add(s);
                    }
                    if (prefixes == null || prefixes.Length == 0)
                    {
                        throw new ArgumentException("prefixes");

                    }
                    Console.WriteLine("Listening...try again if you want.");



                    // Note: The GetContext method blocks while waiting for a request. 
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    // Obtain a response object.
                    HttpListenerResponse response = context.Response;
                    // Construct a response.
                    SiteResources FilesOnServer = new SiteResources();

                    response.Headers.Set(new HttpRequestHeader(), "Md5");

                    byte[] buffer = new byte[] { };
                    Console.WriteLine(request.RawUrl);
                    
                    //response.ContentType = EnAvFunctionerna(request.RawUrl);
                    //buffer = DenAndraFunctionen(request.RawUrl);
                    //MimeMapping.GetMimeMapping;
                    //File.ReadAllBytes;                    

                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // You must close the output stream.
                    output.Close();                    
                    listener.Stop();
                }
            }
            catch(WebException e)
            {
                Console.WriteLine(e.Status);
            }
        }
    }
}
