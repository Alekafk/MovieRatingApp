using MediaRatingApp.Business;
using MediaRatingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

//using User;

namespace MediaRatingApp.Presentation
{
    internal class HttpServer
    {
        public static string prefixes = "http://localhost:8080/";
        public static async Task Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(prefixes);
            listener.Start();
            Console.WriteLine("[Postman] Listening... | URL: " + prefixes);

            while (true)
            {
                HttpListenerContext context = listener.GetContext(); // ironpdf.com/blog/net-help/httplistener-csharp/
                
                _ = Task.Run(() => HandleRequest(context)); //discard, used in the bg for RESTful apps without being terminated
            }
        }

        private async static Task<string> ReadRequestBody(HttpListenerRequest request) {
            if (!request.HasEntityBody)
            {
                return string.Empty;
            }
            using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
            return await reader.ReadToEndAsync();
        }

        private async static Task WriteResponse(HttpListenerResponse response, string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }

        private async static Task WriteJsonResponse(HttpListenerResponse response, object data)
        {
            response.ContentType = "application/json";
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            await WriteResponse(response, json);
        }

        private static async Task HandleRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            try
            {
                if (request.Url.AbsolutePath == "/api/users/register" && request.HttpMethod == "POST")
                {
                    string jsonString = await ReadRequestBody(context.Request);
                    if (string.IsNullOrEmpty(jsonString))
                    {
                        response.StatusCode = 400;
                        await WriteJsonResponse(context.Response, new { error = "Request body is required" });
                        return;
                    }
                    User registerUser = JsonSerializer.Deserialize<User>(jsonString,jsonOptions);

                    if (UserController.UserExists(registerUser.Username))
                    {
                        response.StatusCode = 409;
                        await WriteJsonResponse(context.Response, new { error = "User already exists" });
                        return;

                    }
                    //TODO: call business layer to save user data, get token
                    UserController.AddUser(registerUser);

                    //created
                    response.StatusCode = 201;
                    await WriteJsonResponse(context.Response, new { message = "User registered successfully!" });
                    return;
                } 



                else if (request.Url.AbsolutePath == "/api/users/login" && request.HttpMethod == "POST")
                {
                    string jsonString = await ReadRequestBody(context.Request);
                    if (string.IsNullOrEmpty(jsonString))
                    {
                        response.StatusCode = 400;
                        await WriteJsonResponse(context.Response, new { error = "Request body is required" });
                        return;
                    }
                    
                    User loginUser = JsonSerializer.Deserialize<User>(jsonString, jsonOptions);
                    bool userExists = UserController.UserExists(loginUser.Username);


                    string token = TokenGenerator.GenerateToken();
                    //login success
                    response.StatusCode = 200;
                    await WriteJsonResponse(context.Response, new { message = $"User logged in successfully! | Token: {token}",
                    token = token});
                    return;
                }
                else
                {
                    response.StatusCode = 404;
                    await WriteJsonResponse(context.Response, new { error = "No endpoint found" });
                }
            }
            catch (JsonException)
            {
                context.Response.StatusCode = 400;
                await WriteJsonResponse(context.Response, new { error = "Invalid JSON in request body" });
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                await WriteJsonResponse(context.Response, new { error = $"Server error: {ex.Message}" });
            }

        }


        
    }
}
