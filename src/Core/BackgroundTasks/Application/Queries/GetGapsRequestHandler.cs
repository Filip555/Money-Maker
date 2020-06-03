using System;
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
    public class GetGapsRequestHandler : IRequestHandler<GetGapsRequest, GapList>
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<GetGapsRequestHandler> _logger;
        private readonly IConfiguration _configuration;

        public GetGapsRequestHandler(IHttpClientFactory clientFactory, ILogger<GetGapsRequestHandler> logger, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<GapList> Handle(GetGapsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var requestApi = new HttpRequestMessage(HttpMethod.Get, _configuration["ApiClient"] + "/getGaps");
                var response = await client.SendAsync(requestApi, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GapList>(responseStream);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetGapsRequestHandler");
                throw ex;
            }
        }
    }
}
