using System;
using Nest;

namespace Startup
{
    public class ElasticSearchClientFactory
    {
        private IElasticClient _elasticClient;
        private Uri _hostUri;

        public IElasticClient Client
        {
            get
            {
                if (_elasticClient == null)
                {
                    _elasticClient = Connect();
                }
                return _elasticClient;
            }
        }

        public ElasticSearchClientFactory(string esProtocolScheme, string esHostname, int esHostPort)
        {
            _hostUri = new Uri($"{esProtocolScheme}://{esHostname}:{esHostPort}");
            //_hostUri = new Uri($"{esProtocolScheme}://{esHostname}");
        }
        
        private IElasticClient Connect()
        {
            var settings = new ConnectionSettings(_hostUri);
            _elasticClient = new ElasticClient(settings);
            
            return _elasticClient;
        }
    }
}