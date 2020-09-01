﻿using azure_proto_compute;
using azure_proto_core;
using System;

namespace client
{
    class StartFromVm : Scenario
    {
        public override void Execute()
        {
            var createVm = new CreateSingleVmExample(Context);
            createVm.Execute();
            var client = new ArmClient();
            //retrieve from lowest level, doesn't give ability to walk up and down the container structure
            var vm = client.GetResourceOperations<PhVirtualMachine>(Context.SubscriptionId, Context.RgName, Context.VmName).SafeGet();
            Console.WriteLine($"Found VM {vm.Id}");

            //retrieve from lowest level inside management package gives ability to walk up and down
            var rg = client.ResourceGroup(Context.SubscriptionId, Context.RgName);
            var vm2 = rg.Vm(Context.VmName).SafeGet();
            Console.WriteLine($"Found VM {vm2.Id}");
        }
    }
}