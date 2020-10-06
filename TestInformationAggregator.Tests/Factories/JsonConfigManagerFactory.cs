using System;
using System.Collections.Generic;
using System.Text;
using TestInformationAggregator.Services;

namespace TestInformationAggregator.Tests.Factories
{
    public class JsonConfigManagerFactory
    {
        public JsonConfigManager GetEntity(string configFileName)
        {
            return new JsonConfigManager(configFileName);
        }
    }
}
