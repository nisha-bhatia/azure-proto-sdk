﻿using azure_proto_core;

namespace azure_proto_compute
{
    public class AzureVm : AzureResource
    {
        public AzureVm(IResource resourceGroup, PhVirtualMachine vm) : base(resourceGroup, vm) { }

        public void Stop()
        {
            var computeClient = Clients.ComputeClient;
            var result = computeClient.VirtualMachines.StartPowerOff(Parent.Name, Model.Name).WaitForCompletionAsync().Result;
        }

        public void Start()
        {
            var computeClient = Clients.ComputeClient;
            var result = computeClient.VirtualMachines.StartStart(Parent.Name, Model.Name).WaitForCompletionAsync().Result;
        }
    }
}