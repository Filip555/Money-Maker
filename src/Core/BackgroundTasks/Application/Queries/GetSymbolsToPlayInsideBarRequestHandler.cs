using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace BackgroundTasks.Application.Queries
{
    using Api.Application.Models;

    public class GetSymbolsToPlayInsideBarRequestHandler : IRequestHandler<GetSymbolsToPlayInsideBarRequest, List<InsideBarView>>
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<GetSymbolsToPlayInsideBarRequestHandler> _logger;
        private readonly IConfiguration _configuration;

        public GetSymbolsToPlayInsideBarRequestHandler(IHttpClientFactory clientFactory, ILogger<GetSymbolsToPlayInsideBarRequestHandler> logger, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<List<InsideBarView>> Handle(GetSymbolsToPlayInsideBarRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var requestApi = new HttpRequestMessage(HttpMethod.Get, _configuration["ApiClient"] + "/GetInsideBars");
                var response = await client.SendAsync(requestApi, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<InsideBarView>>(responseStream);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetSymbolsToPlayInsideBarRequestHandler");
                throw ex;
            }
        }
    }
}
