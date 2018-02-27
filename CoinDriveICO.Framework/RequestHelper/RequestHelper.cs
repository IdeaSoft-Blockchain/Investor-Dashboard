using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoinDriveICO.Framework.RequestHelper
{
    public static class RequestHelper
    {
        /// <summary>
        /// Sends the post with mixed payload.
        /// </summary>
        /// <typeparam name="TGetRequest">The type of the get request.</typeparam>
        /// <typeparam name="TPostRequest">The type of the post request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="getPayload">The get payload.</param>
        /// <param name="postPayload">The post payload.</param>
        /// <param name="authScheme">The authentication scheme.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns></returns>
        public static async Task<TResponse> SendPostWithMixedPayloadAsync<TGetRequest, TPostRequest, TResponse>(
            string url, 
            TGetRequest getPayload, 
            TPostRequest postPayload, 
            string authScheme = null,
            string authHeaderValue = null)
        {
            using (var client = new HttpClient())
            {
                if (authHeaderValue != null && authScheme != null)
                {
                    client.SetAuthorizationHeader(authScheme, authHeaderValue);
                }
                var stringGetPayload = ConvertObjectToGetRequestPayload(getPayload);
                var stringPostPayload = ConvertObjectToPostPayload(postPayload);
                var result = await client.SendPostCoreAsync<TResponse>(url + stringGetPayload, stringPostPayload);
                return result;

            }
        }

        /// <summary>
        /// Sends the post.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="authScheme">The authentication scheme.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns></returns>
        public static async Task<TResponse> SendPostAsync<TRequest, TResponse>(
                    string url,
                    TRequest payload,
                    string authScheme = null,
                    string authHeaderValue = null)
        {
            using (var client = new HttpClient())
            {
                if (authHeaderValue != null && authScheme != null)
                {
                    client.SetAuthorizationHeader(authScheme, authHeaderValue);
                }
                var transformedPayload = ConvertObjectToPostPayload(payload);
                var result = await client.SendPostCoreAsync<TResponse>(url, transformedPayload);
                return result;
            }
        }

        /// <summary>
        /// Sends the get.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="requestPayload">The request payload.</param>
        /// <param name="authScheme">The authentication scheme.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns></returns>
        public static async Task<TResponse> SendGetAsync<TRequest,TResponse>(
            string url, 
            TRequest requestPayload, 
            string authScheme = null,
            string authHeaderValue = null)
        {
            using (var client = new HttpClient())
            {
                if (authHeaderValue != null && authScheme != null)
                {
                    client.SetAuthorizationHeader(authScheme,authHeaderValue);
                }
                //client.BaseAddress = new Uri(url);
                var requestStringParameters = ConvertObjectToGetRequestPayload(requestPayload);
                var rawResponse = await client.GetStringAsync(url + requestStringParameters);
                return JsonConvert.DeserializeObject<TResponse>(rawResponse);
            }
        }

        /// <summary>
        /// Sends the post core asynchronous.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="postPayload">The post payload.</param>
        /// <returns></returns>
        private static async Task<TResponse> SendPostCoreAsync<TResponse>(this HttpClient client, string url, StringContent postPayload)
        {
            postPayload.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var rawResponse = await client.PostAsync(url, postPayload);
            var responseString = await rawResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResponse>(responseString);
            return result;
        }

        /// <summary>
        /// Converts the object to post payload.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        private static StringContent ConvertObjectToPostPayload(object source)
        {
            return new StringContent(JsonConvert.SerializeObject(source));
        }

        /// <summary>
        /// Converts the object to get request payload part.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        private static string ConvertObjectToGetRequestPayload(object source)
        {
            var dict = ConvertObjectToDictionary(source);
            return ConvertDictionaryToGetRequestPayload(dict);
        }

        /// <summary>
        /// Converts the dictionary to get request payload.
        /// </summary>
        /// <param name="toConvert">To convert.</param>
        /// <returns></returns>
        private static string ConvertDictionaryToGetRequestPayload(Dictionary<string, string> toConvert)
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append('?');
            using (var iterator = toConvert.GetEnumerator())
            {
                iterator.MoveNext();
                do
                {
                    var current = iterator.Current;
                    AppendStringKeyValuePair(resultBuilder,current);
                } while (MoveIteratorAndAppendSeparator(resultBuilder, iterator, '&'));
            }
            return resultBuilder.ToString();

            // Moves iterator to next element and if it's present appends separator to specified string builder
            bool MoveIteratorAndAppendSeparator(StringBuilder builder, IEnumerator iterator, char separator)
            {
                if (iterator.MoveNext())
                {
                    builder.Append(separator);
                    return true;
                }
                return false;
            }

            // Appends key value pair to string in format of "key=value"
            void AppendStringKeyValuePair(StringBuilder builder, KeyValuePair<string, string> pair)
            {
                builder.Append(pair.Key)
                    .Append("=")
                    .Append(pair.Value);
            }
        }

        /// <summary>
        /// Converts the object to dictionary.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        private static Dictionary<string, string> ConvertObjectToDictionary(object source)
        {
            return source.GetType()
                         .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                         .ToDictionary(prop => prop.Name, prop => prop.GetValue(source, null).ToString());
        }

        /// <summary>
        /// Sets the authorization header.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="authScheme">The authentication scheme.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        private static void SetAuthorizationHeader(this HttpClient client, string authScheme, string authHeaderValue)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authScheme, authHeaderValue);
        }
    }
}