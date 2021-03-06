﻿using System;
using System.Linq;
using System.Net;
using NUnit.Framework;

namespace Neo4jClient.Test.GraphClientTests
{
    [TestFixture]
    public class QueryNodeIndexTests
    {
        [Test]
        [Obsolete]
        public void ShouldReturnQueryResults()
        {
            //Arrange
            using (var testHarness = new RestTestHarness
            {
                {
                    MockRequest.Get("/index/node/indexName?query=name%3Afoo"),
                    MockResponse.Json(HttpStatusCode.OK, 
                        @"[{ 'self': 'http://foo/db/data/node/456',
                          'data': { 'Name': 'Foo' },
                          'create_relationship': 'http://foo/db/data/node/456/relationships',
                          'all_relationships': 'http://foo/db/data/node/456/relationships/all',
                          'all_typed relationships': 'http://foo/db/data/node/456/relationships/all/{-list|&|types}',
                          'incoming_relationships': 'http://foo/db/data/node/456/relationships/in',
                          'incoming_typed relationships': 'http://foo/db/data/node/456/relationships/in/{-list|&|types}',
                          'outgoing_relationships': 'http://foo/db/data/node/456/relationships/out',
                          'outgoing_typed relationships': 'http://foo/db/data/node/456/relationships/out/{-list|&|types}',
                          'properties': 'http://foo/db/data/node/456/properties',
                          'property': 'http://foo/db/data/node/456/property/{key}',
                          'traverse': 'http://foo/db/data/node/456/traverse/{returnType}'
                        }]")
                }
            })
            {
                var graphClient = testHarness.CreateAndConnectGraphClient();

                var results = graphClient
                    .QueryIndex<TestNode>("indexName", IndexFor.Node, "name:foo")
                    .ToArray();

                Assert.AreEqual(1, results.Count());
                var result = results[0];
                Assert.AreEqual(456, result.Reference.Id);
                Assert.AreEqual("Foo", result.Data.Name);
            }
        }

        public class TestNode
        {
            public string Name { get; set; }
        }
    }
}
