﻿using azure_proto_compute;
using azure_proto_core;
using System;
using System.Linq;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            Scenario scenario = null;
            try
            {
                scenario = ScenarioFactory.GetScenario(Scenarios.CreateSingleVmExample);
                scenario.Execute();
            }
            finally
            {
                foreach (var rgId in Scenario.CleanUp)
                {
                    ResourceIdentifier id = new ResourceIdentifier(rgId);
                    var rg = new ArmClient().Subscription(id.Subscription).ResourceGroup(id);
                    Console.WriteLine($"--------Deleting {rg.Context.Name}--------");
                    var rgModel = rg.GetModelIfNewer();
                    if (rgModel != null)
                    {
                        _ = rg.DeleteAsync();
                    }
                }
            }
        }
    }
}
