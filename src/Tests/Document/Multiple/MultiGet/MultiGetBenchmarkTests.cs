using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using Bogus;
using Elasticsearch.Net;
using Nest;

namespace Tests.Document.Multiple.MultiGet
{
    [Config(typeof(FastRunConfig))]
    public class MultiGetBenchmarkTests
    {
        private const string IndexName = "multiget-test-index";
        private static readonly ElasticClient Client =
            new ElasticClient(new ConnectionSettings(
                new SingleNodeConnectionPool(new Uri("http://localhost:9200")),
                new InMemoryConnection(ResponseBytes)).InferMappingFor<Document>(m => m.IndexName(IndexName)));

        private static byte[] ResponseBytes => Encoding.UTF8.GetBytes(
            @"{""docs"":[
                {""_index"":""multiget-test-index"",""_type"":""document"",""_id"":""1F9382431C394860B407485794ED9E74"",""_version"":1,""found"":true,""_source"":{""id"":""1F9382431C394860B407485794ED9E74""}},
                {""_index"":""multiget-test-index"",""_type"":""document"",""_id"":""36E1184AC5464D2ABF6245B2DD468C04"",""_version"":1,""found"":true,""_source"":{""id"":""36E1184AC5464D2ABF6245B2DD468C04""}},
                {""_index"":""multiget-test-index"",""_type"":""document"",""_id"":""4A9D5429657240B294C1EE786B875135"",""_version"":1,""found"":true,""_source"":{""id"":""4A9D5429657240B294C1EE786B875135""}},
                {""_index"":""multiget-test-index"",""_type"":""document"",""_id"":""F5FCED83157940C2A0F7EC36D4320FF3"",""_version"":1,""found"":true,""_source"":{""id"":""F5FCED83157940C2A0F7EC36D4320FF3""}},
                {""_index"":""multiget-test-index"",""_type"":""document"",""_id"":""DE72611193534758AEE5AAB3DD3B2529"",""_version"":1,""found"":true,""_source"":{""id"":""DE72611193534758AEE5AAB3DD3B2529""}}]}");

        [Benchmark]
        public IMultiGetResponse MultiGetDeserialization()
        {
            var response = Client.MultiGet(mg => mg
                    .Index(IndexName)
                    .Get<Document>(m => m.Id("1F9382431C394860B407485794ED9E74"))
                    .Get<Document>(m => m.Id("36E1184AC5464D2ABF6245B2DD468C04"))
                    .Get<Document>(m => m.Id("4A9D5429657240B294C1EE786B875135"))
                    .Get<Document>(m => m.Id("F5FCED83157940C2A0F7EC36D4320FF3"))
                    .Get<Document>(m => m.Id("DE72611193534758AEE5AAB3DD3B2529"))
            );

            return response;
        }

        public class Document
        {
            public string Id { get; set; }

            public static Faker<Document> Generator { get; } =
                new Faker<Document>()
                    .RuleFor(p => p.Id, f => Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpperInvariant());
        }
    }
}