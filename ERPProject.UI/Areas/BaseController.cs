using ERPProject.Entity.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ERPProject.UI.Areas
{
    public class BaseController : Controller
    {
        private readonly HttpClient _httpClient;
        public BaseController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:7075/api/");
        }
        protected async Task<ApiResponse<T>> UpdateAsync<T>(T p, string url) where T : class
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("Token"));
            var jsonData = JsonConvert.SerializeObject(p);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync(url, stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonDataw = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<ApiResponse<T>>(jsonDataw);
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                return value;
            }

            return null;
        }
        protected async Task<ApiResponse<T>> AddAsync<T>(T p, string url) where T : class
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("Token"));
            var jsonData = JsonConvert.SerializeObject(p);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync(url, stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {

                var jsonDataw = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<ApiResponse<T>>(jsonDataw);
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                return value;
            }
            var jsonDataw2 = await responseMessage.Content.ReadAsStringAsync();
            var value2 = JsonConvert.DeserializeObject<ApiResponse<T>>(jsonDataw2);
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            return value2;
        }
        protected async Task<bool> DeleteAsync(string url)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("Token"));

            HttpResponseMessage responseMessage = await _httpClient.DeleteAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                return true;
            }

            return false;
        }
        protected async Task<ApiResponse<List<T>>> GetAllAsync<T>(string url) where T : class
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("Token"));
            var responseMessage = await _httpClient.GetAsync(url);

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<ApiResponse<List<T>>>(jsonData);
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                return value;


            }
            else if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                var forbiddenResponse = new ApiResponse<List<T>>
                    (
                        data: null,
                        statustCode: 403,
                        hataBilgisi: null,
                        mesaj: "Yetkisiz"
                    );

                return forbiddenResponse;

            }
            else if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                var unauthorizedResponse = new ApiResponse<List<T>>
                    (
                        data: null,
                        statustCode: 401,
                        hataBilgisi: null,
                        mesaj: "Oturum Açılmadı"
                    );

                return unauthorizedResponse;

            }

            return null;
        }
        protected async Task<ApiResponse<T>> GetAsync<T>(string url) where T : class
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("Token"));
            var responseMessage = await _httpClient.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<ApiResponse<T>>(jsonData);
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                return value;
            }

            return null;
        }
    }
}
