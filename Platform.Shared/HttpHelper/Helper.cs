using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Platform.Shared.HttpHelper
{
    public class Helper<DTO> : IHelper<DTO> where DTO : class
    {
        #region Get
        public OperationResult<DTO>? Get(string endPoint, string id)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{endPoint}/{id}");
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            HttpClient httpClient = new();

            HttpResponseMessage response = httpClient.Send(request);

            var stream = response.Content.ReadAsStream();
            OperationResult<DTO>? dtoModelList = JsonSerializer.Deserialize<OperationResult<DTO>>(stream, jsonOptions);
            return dtoModelList;
        }

        public OperationResult<List<DTO>>? GetAll(string endPoint)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, endPoint);
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            HttpClient httpClient = new();

            HttpResponseMessage response = httpClient.Send(request);

            var stream = response.Content.ReadAsStream();
            OperationResult<List<DTO>>? dtoModelList = JsonSerializer.Deserialize<OperationResult<List<DTO>>>(stream, jsonOptions);
            return dtoModelList;
        }
        #endregion

        #region Post
        public OperationResult<DTO>? Post(string endPoint, DTO element)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, endPoint);
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            HttpClient httpClient = new();

            var json = JsonSerializer.Serialize(element);
            request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = httpClient.Send(request);

            var stream = response.Content.ReadAsStream();
            OperationResult<DTO>? dtoModelList = JsonSerializer.Deserialize<OperationResult<DTO>>(stream, jsonOptions);
            return dtoModelList;
        }

        #endregion

        #region Patch
        public OperationResult<DTO>? Patch(string endPoint, DTO element)
        {
            using var request = new HttpRequestMessage(HttpMethod.Patch, endPoint);
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            HttpClient httpClient = new();

            var json = JsonSerializer.Serialize(element);
            request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = httpClient.Send(request);

            var stream = response.Content.ReadAsStream();
            OperationResult<DTO>? dtoModelList = JsonSerializer.Deserialize<OperationResult<DTO>>(stream, jsonOptions);
            return dtoModelList;
        }
        #endregion

        #region Put
        public OperationResult<DTO>? Put(string endPoint, DTO element)
        {
            using var request = new HttpRequestMessage(HttpMethod.Put, endPoint);
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            HttpClient httpClient = new();

            var json = JsonSerializer.Serialize(element);
            request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = httpClient.Send(request);

            var stream = response.Content.ReadAsStream();
            OperationResult<DTO>? dtoModelList = JsonSerializer.Deserialize<OperationResult<DTO>>(stream, jsonOptions);
            return dtoModelList;
        }
        #endregion
    }
}
