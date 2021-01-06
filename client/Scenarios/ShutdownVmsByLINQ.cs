﻿using azure_proto_compute;
using Azure.ResourceManager.Core;
using System;
using System.Linq;

namespace client
{
    class ShutdownVmsByLINQ : Scenario
    {
        public override void Execute()
        {
            var createMultipleVms = new CreateMultipleVms(Context);
            createMultipleVms.Execute();

            var client = new AzureResourceManagerClient();
            foreach (var sub in client.Subscriptions().List())
            {
                foreach (var vm in sub.ListVirtualMachinesByName("mc").Where(vm => vm.Data.Name.Contains("foo")))
                {
                    var instance = new VirtualMachineOperations(vm);
                    instance.PowerOff();
                }
            }

            var resourceGroup = new AzureResourceManagerClient().ResourceGroup(Context.SubscriptionId, Context.RgName);

            resourceGroup.VirtualMachines().List().Select(vm =>
            {
                var parts = vm.Id.Name.Split('-');
                var n = Convert.ToInt32(parts[parts.Length - 2]);
                return (vm, n);
            })
                .Where(tuple => tuple.n % 2 == 0)
                .ToList()
                .ForEach(tuple =>
                {
                    Console.WriteLine($"Stopping {tuple.vm.Id.Name}");
                    tuple.vm.PowerOff();
                    Console.WriteLine($"Starting {tuple.vm.Id.Name}");
                    tuple.vm.PowerOn();
                });
        }
    }
}
