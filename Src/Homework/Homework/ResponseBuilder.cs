﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.Security.Cryptography;

namespace Homework
{
    class ResponseBuilder
    {
        private static HttpListenerResponse Response { get; set; }
        private static string CleanUrl { get; set; }
        private static SiteResources Resources { get; set; }
        public static byte[] Buffer { get; set; }

        public static HttpListenerResponse Build(HttpListenerContext context, SiteResources resources)
        {
            // setting all class properties to default values
            Resources = resources;                          
            Response = context.Response;
            CleanUrl = CleanRawUri(context.Request.RawUrl);
            Buffer = new byte[0];

            AddCookie(context.Request);

            // Main logic for the 3 request senarios: 'special', 'resource' and 'bad'(requests we can't yet handle)  
            // special
            if (SpecialRequest(context.Request.RawUrl))
            {
                // our server can handle the '/dynamic' and the '/counter' request
                HandleSpecialRequest(context.Request);
            }
            // resource
            else if (ResourceExists())
            {
                // this is what we worked on in class, ei: a normal request that looks for resources in the content folder
                Buffer = File.ReadAllBytes(Resources.FilePaths[CleanUrl]);
                Response.ContentType = MimeMapping.GetMimeMapping(Resources.FilePaths[CleanUrl]);                                
                Response.StatusCode = 200;
            }
            // bad
            else
            {
                Response.StatusCode = 404;
            }

            AddMd5HashHeader();
            AddExpiresHeader();

            return Response;
        }

        private static void AddCookie(HttpListenerRequest request)
        {
            // adding new cookie
            if (request.Cookies.Count == 0)
            {
                Response.Cookies.Add(new Cookie("counter", "1"));
            }
            else // updating exsisting cookie
            {
                foreach (Cookie c in request.Cookies)
                {
                    if (c.Name == "counter")  
                    {
                        Cookie CounterCookie = c;
                        CounterCookie.Value = (Convert.ToInt32(c.Value) + 1).ToString();
                        Response.Cookies.Add(c);
                    }
                }
            }

        }

        private static void HandleSpecialRequest(HttpListenerRequest request)
        {
            switch (request.RawUrl.Substring(0, 8))
            {
                case "/dynamic":
                    // fail if 2 variables are not given
                    if (request.QueryString.Count < 2)
                    {
                        Buffer = Encoding.UTF8.GetBytes("Missing input value");
                        Response.ContentType = "text/html";
                        Response.StatusCode = 500;
                    }
                    else
                    {
                        int num1 = Convert.ToInt32(request.QueryString.Get(0));
                        int num2 = Convert.ToInt32(request.QueryString.Get(1));
                        int sum = num1 + num2;
                        // the "Accept" header determines which type of format we will return
                        if (request.Headers.Get("Accept") == "application/xml")
                        {
                            Buffer = Encoding.UTF8.GetBytes("<result><value>" + sum + "</value></result>");
                            Response.ContentType = "application/xml";
                        }
                        else
                        {
                            Buffer = Encoding.UTF8.GetBytes("<html><body>" + sum + "</body></html>");
                            Response.ContentType = "text/html";
                        }
                        Response.StatusCode = 200;
                    }
                    break;

                case "/counter":

                        Buffer = Encoding.UTF8.GetBytes(Response.Cookies[0].Value);
                        Response.ContentType = "text/html";
                        Response.StatusCode = 200;

                    break;
            }
        }

        private static void AddExpiresHeader()
        {
            Response.Headers.Add("Expires", GetExpiresValue(1));
        }
        //no ETag added if buffer is empty
        private static void AddMd5HashHeader()
        {
            if (Buffer.Length > 0)
            {
                MD5 md5Hash = MD5.Create();
                md5Hash.ComputeHash(Buffer);
                StringBuilder str = new StringBuilder();
                foreach(byte b in md5Hash.Hash)
                {
                    str.Append(b.ToString("x2"));
                }
                Response.Headers.Add("ETag", str.ToString());
            }
        }

        //make url request compatible with our resource dictionary
        private static string CleanRawUri(string raw)
        {
            if (raw[raw.Length - 1] == '/')
            {
                return raw + "index.html";
            }
            if (Resources.FilePaths.Keys.Contains(raw))
            {
                return raw;
            }
            else
            {
                return "";
            }
        }

        // determines if our server can handle this non-resource based request
        private static bool SpecialRequest(string url)  
        {
            if (url.Length > 7)
            {
                if (url.Substring(0, 8) == "/dynamic" || url.Substring(0, 8) == "/counter")
                    return true;
            }
            return false;
        }

        // determines if requested resource exists on our server
        private static bool ResourceExists()
        {
            if (Resources.FilePaths.Keys.Contains(CleanUrl))    
                return true;

            return false;                      
        }

        //returns a future date in string format
        private static string GetExpiresValue(int years)
        {
            return DateTime.UtcNow.AddYears(years).ToString("o");   
        }
    }
}