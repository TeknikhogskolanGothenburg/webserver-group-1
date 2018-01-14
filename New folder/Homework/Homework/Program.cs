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
                  
          
                        byte[] buffer = new byte[] { };
                    string pureRequestString = resources.CleanRawUrl(request.RawUrl); //klar
                                            
                   // Console.WriteLine("RAW: " + request.RawUrl);                    
                    if (pureRequestString == "")
                    {
                        buffer = new byte[0];   //vad gör denna? Den kommer aldrig att gå in här eftersom fel 404 kastas.
                    }
                    else
                    {
                        buffer = resources.GetOutputContent(pureRequestString); //skickar hela Root:en
                        //setting more headers
                        response.ContentType = resources.GetOutputType(pureRequestString);
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
