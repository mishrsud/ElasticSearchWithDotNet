using System;
using System.Configuration;
using Ploeh.AutoFixture;

namespace Startup
{
    internal class Program
    {
        private static string _scheme;
        private static string _host;
        private static int _port;

        public static void Main(string[] args)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            ReadConfig();
            
            var esClientFactory = new ElasticSearchClientFactory(_scheme, _host, _port);
            var elasticClient = esClientFactory.Client;

            for (int counter = 0; counter < 100; counter++)
            {
                var item = CreateTestData();
                var response = esClientFactory.Client.IndexAsync(item, idx => idx.Index("test-harness-index")).Result;
                Console.WriteLine($"Item Created with Id: {item.Id}, Is Response valid: {response.IsValid}");
            }

            Console.WriteLine("Done, press any key to exit");
            Console.ReadLine();
        }

        private static DummyObject CreateTestData()
        {
            var dummyData = new Fixture().Build<DummyObject>().Create();
            return dummyData;
        }

        private static void ReadConfig()
        {
            _scheme = ConfigurationManager.AppSettings["Scheme"];
            _host = ConfigurationManager.AppSettings["Host"];
            _port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
        }
    }
}