﻿#region license
// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using MbUnit.Framework;
using SolrNet.Impl.FieldSerializers;
using SolrNet.Impl.QuerySerializers;

namespace SolrNet.Tests {
    [TestFixture]
    public class SolrRequiredQueryTests {

        public string Serialize(object q) {
            var serializer = new DefaultQuerySerializer(new DefaultFieldSerializer());
            return serializer.Serialize(q);
        }

        [Test]
        public void SimpleQuery() {
            var q = new SolrQuery("desc:samsung");
            var requiredq = new SolrRequiredQuery(q);
            Assert.AreEqual("+desc:samsung", Serialize(requiredq));
        }

        [Test]
        public void QueryByField() {
            var q = new SolrQueryByField("desc", "samsung");
            var requiredq = new SolrRequiredQuery(q);
            Assert.AreEqual("+desc:(samsung)", Serialize(requiredq));
        }

        [Test]
        public void RangeQuery() {
            var q = new SolrQueryByRange<decimal>("price", 100, 200);
            var requiredq = new SolrRequiredQuery(q);
            Assert.AreEqual("+price:[100 TO 200]", Serialize(requiredq));
        }

        [Test]
        public void QueryInList() {
            var q = new SolrQueryInList("desc", "samsung", "hitachi", "fujitsu");
            var requiredq = new SolrRequiredQuery(q);
            Assert.AreEqual("+(desc:(samsung) OR desc:(hitachi) OR desc:(fujitsu))", Serialize(requiredq));
        }

        [Test]
        public void MultipleCriteria() {
            var q = SolrMultipleCriteriaQuery.Create(new SolrQueryByField("desc", "samsung"), new SolrQueryByRange<decimal>("price", 100, 200));
            var requiredq = new SolrRequiredQuery(q);
            Assert.AreEqual("+(desc:(samsung)  price:[100 TO 200])", Serialize(requiredq));
        }

        [Test]
        public void MultipleCriteria_required() {
            var q = SolrMultipleCriteriaQuery.Create(new SolrQueryByField("desc", "samsung"), new SolrQueryByRange<decimal>("price", 100, 200));
            Assert.AreEqual("+(desc:(samsung)  price:[100 TO 200])", Serialize(q.Required()));
        }

        [Test]
        public void RequiredQuery_is_AbstractSolrQuery() {
            AbstractSolrQuery q = new SolrQuery("").Required();
        }
    }
}