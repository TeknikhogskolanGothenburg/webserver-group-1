﻿using System;
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
                while(true)
                {
                listener.Start();
                if (prefixes == null || prefixes.Length == 0)
                {
                    throw new ArgumentException("prefixes");

                }
                    // Add the prefixes.
                    foreach (string s in prefixes)
                    {
                        listener.Prefixes.Add(s);
                    }
                    Console.WriteLine("Listening...try again if you want.");
                    // Note: The GetContext method blocks while waiting for a request. 
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    // Obtain a response object.
                    HttpListenerResponse response = context.Response;
                    // Construct a response.
                    SiteResources FilesOnServer = new SiteResources();
                    string test = "here:" + request.RawUrl;
                    Console.WriteLine(test);
                    Console.ReadKey();
                    context.Response.ContentType = MimeMapping.GetMimeMapping(FilesOnServer.FilePaths[6]);
                    byte[] buffer1 = File.ReadAllBytes(FilesOnServer.FilePaths[6]);

                    context.Response.ContentType = MimeMapping.GetMimeMapping(FilesOnServer.FilePaths[2]);
                    byte[] buffer2 = File.ReadAllBytes(FilesOnServer.FilePaths[2]);

                    // Get a response stream and write the response to it.

                    response.ContentLength64 = buffer1.Length;
            
                    Stream output = response.OutputStream;
                    output.Write(buffer1, 0, buffer1.Length);
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
