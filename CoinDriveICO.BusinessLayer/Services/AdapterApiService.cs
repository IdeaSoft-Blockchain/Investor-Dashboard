using System;
using System.Collections.Generic;
using System.Linq;
using CoinDriveICO.Framework.JsonStructures.AdapterApi;
using System.Threading.Tasks;
using CoinDriveICO.Framework.RequestHelper;
using CoinDriveICO.Framework.SettingsModels;
using Microsoft.Extensions.Options;

namespace CoinDriveICO.BusinessLayer.Services
{
    public interface IAdapterApiService
    {
        Task<Response<AdapterInfo>> GetAdapterInfoAsync(string adapterName);
        Task<Response<AddressInfo>> GenerateRefillAddressAsync(string adapterName, int userId, string tag, bool isNewRequired);
        Task<Response<IEnumerable<TransactionInfo>>> GetTransactionHistoryAsync(string adapterName, int? userId = null);
        // TODO: Add method which marks transactions as processed
        Task<Response<string>> MarkTransactionAsConfirmed(IEnumerable<string> ids, string adapterName);
    }

    public class AdapterApiService : IAdapterApiService
    {
        private readonly string _baseAddress;
        private readonly string _apiKey;
        private readonly string _authorizationScheme;

        public AdapterApiService(IOptions<IcoAdapterSettings> adapterSettings)
        {
            _baseAddress = adapterSettings.Value.BaseApiUrl;
            _apiKey = adapterSettings.Value.AuthorizationApiKey;
            _authorizationScheme = adapterSettings.Value.AuthorizationScheme;
        }

        public async Task<Response<AdapterInfo>> GetAdapterInfoAsync(string adapterName)
        {
            var endpoint = ConcatBaseAdressWithEndpoint("api/management/adapters/info");
            var payload = new AdapterRequest(adapterName);
            var response = await RequestHelper.SendGetAsync<AdapterRequest, Response<AdapterInfo>>(endpoint,
                                                                                             payload,
                                                                                             _authorizationScheme,
                                                                                             _apiKey);
            return response;
        }

        public async Task<Response<AddressInfo>> GenerateRefillAddressAsync(string adapterName, int userId, string tag, bool isNewRequired)
        {
            var endpoint = ConcatBaseAdressWithEndpoint("/api/management/addresses/create");
            var getPayload = new AdapterRequest(adapterName);
            var postPayload = new AddressRequest
            {
                UserId = userId,
                Tag = tag,
                GenerateNew = isNewRequired
            };
            var response = await RequestHelper.SendPostWithMixedPayloadAsync<AdapterRequest, AddressRequest, Response<AddressInfo>>(
                endpoint,
                getPayload,
                postPayload,
                _authorizationScheme,
                _apiKey);
            return response;
        }

        public async Task<Response<IEnumerable<TransactionInfo>>> GetTransactionHistoryAsync(string adapterName, int? userId = null)
        {
            var endpoint = ConcatBaseAdressWithEndpoint("api/management/addresses/gettransactions");
            var getPayload = new AdapterRequest(adapterName);
            var response =
                await RequestHelper.SendGetAsync<AdapterRequest, Response<IEnumerable<TransactionInfo>>>(endpoint, getPayload,
                    _authorizationScheme, _apiKey);
            if (userId.HasValue)
            {
                response.Value = response.Value.Where(x => x.UserId == userId);
            }
            return response;
        }

        public async Task<Response<string>> MarkTransactionAsConfirmed(IEnumerable<string> ids, string adapterName)
        {
            var endpoint = ConcatBaseAdressWithEndpoint("api/management/addresses/setprocessed");
            var getPayload = new AdapterRequest(adapterName);
            var postPayload = new SetProcessedRequest
            {
                Ids = ids.ToList()
            };
            var response =
                await RequestHelper.SendPostWithMixedPayloadAsync<AdapterRequest, SetProcessedRequest, Response<string>>(endpoint,
                    getPayload, postPayload, _authorizationScheme, _apiKey);
            return response;
        }

        private string ConcatBaseAdressWithEndpoint(string endpointUrl)
        {
            string result = _baseAddress + endpointUrl;
            if (_baseAddress.EndsWith('/'))
            {
                if (endpointUrl.StartsWith('/'))
                {
                    result = _baseAddress + endpointUrl.Substring(1, endpointUrl.Length - 1);
                }
            }
            else
            {
                if (!endpointUrl.StartsWith('/'))
                {
                    result = _baseAddress + '/' + endpointUrl;
                }
            }
            return result;
        }
    }
}