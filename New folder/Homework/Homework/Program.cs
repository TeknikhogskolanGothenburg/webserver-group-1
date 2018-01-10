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
        

        static void Main(string[] prefixes)

        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");
            try
            {

             while(true)
                {
                // Create a listener.
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add("http://localhost:8080/");
                listener.Start();

                Console.WriteLine("Listening...try again if you want.");
                // Note: The GetContext method blocks while waiting for a request. 
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                // Construct a response.

                string responseString = "<HTML><BODY>hej Agneta</BODY></HTML>";
                string test = request.RawUrl.

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;

                Stream output = response.OutputStream;

                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
                listener.Stop();
                }

                
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Status);
            }

        }


    }
}
