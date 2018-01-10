using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Homework
{
    class Program
    {
        public static HttpListener listener = new HttpListener();

        static void Main(string[] prefixes)
        {
            FileReader myFileReader = new FileReader();
            
            
            try
            {

                if (!HttpListener.IsSupported)
                {
                    Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                    return;
                }
                // URI prefixes are required,
                // for example "http://contoso.com:8080/index/".
                if (prefixes == null || prefixes.Length == 0)
                {
                    throw new ArgumentException("prefixes");

                }
                while(listener.IsListening)
                {
                    // Add the prefixes.
                    foreach (string s in prefixes)
                    {
                        listener.Prefixes.Add(s);
                    }
                    listener.Start();
                    Console.WriteLine("Listening...try again if you want.");
                    // Note: The GetContext method blocks while waiting for a request. 
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    // Obtain a response object.
                    HttpListenerResponse response = context.Response;
                    // Construct a response.

                    string test = "";
                    foreach (string line in context.Request.Headers.AllKeys)
                    {
                        test += line;
                    }

                    context.Response.ContentType ="text/html";

                    string responseString = myFileReader.Index;
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                    // Get a response stream and write the response to it.
            
                    response.ContentLength64 = buffer.Length;
            
                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
            
                    // You must close the output stream.
                    output.Close();
                    listener.Stop();

                    //useful: Environment.CurrentDirectory
                }
            }
            catch(WebException e)
            {
                Console.WriteLine(e.Status);
            }
        }

    }
}
