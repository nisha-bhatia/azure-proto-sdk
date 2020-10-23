﻿using Azure;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using azure_proto_core;
using azure_proto_core.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace azure_proto_network
{
    public class PublicIpAddressContainer : ResourceContainerOperations<PublicIpAddressOperations, PhPublicIPAddress>
    {
        public PublicIpAddressContainer(ArmClientContext context, PhResourceGroup resourceGroup) : base(context, resourceGroup) { }

        public override ResourceType ResourceType => "Microsoft.Network/publicIpAddresses";

        public override ArmResponse<PublicIpAddressOperations> Create(string name, PhPublicIPAddress resourceDetails, CancellationToken cancellationToken = default)
        {
            var operation = Operations.StartCreateOrUpdate(Id.ResourceGroup, name, resourceDetails, cancellationToken);
            return new PhArmResponse<PublicIpAddressOperations, PublicIPAddress>(
                operation.WaitForCompletionAsync(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult(), 
                n => new PublicIpAddressOperations(ClientContext, new PhPublicIPAddress(n)));
        }

        public async override Task<ArmResponse<PublicIpAddressOperations>> CreateAsync(string name, PhPublicIPAddress resourceDetails, CancellationToken cancellationToken = default)
        {
            var operation = await Operations.StartCreateOrUpdateAsync(Id.ResourceGroup, name, resourceDetails, cancellationToken).ConfigureAwait(false);
            return new PhArmResponse<PublicIpAddressOperations, PublicIPAddress>(
                await operation.WaitForCompletionAsync(cancellationToken).ConfigureAwait(false),
                n => new PublicIpAddressOperations(ClientContext, new PhPublicIPAddress(n)));
        }

        public override ArmOperation<PublicIpAddressOperations> StartCreate(string name, PhPublicIPAddress resourceDetails, CancellationToken cancellationToken = default)
        {
            return new PhArmOperation<PublicIpAddressOperations, PublicIPAddress>(
                Operations.StartCreateOrUpdate(Id.ResourceGroup, name, resourceDetails, cancellationToken),
                n => new PublicIpAddressOperations(ClientContext, new PhPublicIPAddress(n)));
        }

        public async override Task<ArmOperation<PublicIpAddressOperations>> StartCreateAsync(string name, PhPublicIPAddress resourceDetails, CancellationToken cancellationToken = default)
        {
            return new PhArmOperation<PublicIpAddressOperations, PublicIPAddress>(
                await Operations.StartCreateOrUpdateAsync(Id.ResourceGroup, name, resourceDetails, cancellationToken).ConfigureAwait(false),
                n => new PublicIpAddressOperations(ClientContext, new PhPublicIPAddress(n)));
        }

        public ArmBuilder<PublicIpAddressOperations, PhPublicIPAddress> Construct(Location location = null)
        {
            var ipAddress = new PublicIPAddress()
            {
                PublicIPAddressVersion = IPVersion.IPv4.ToString(),
                PublicIPAllocationMethod = IPAllocationMethod.Dynamic,
                Location = location ?? DefaultLocation,
            };

            return new ArmBuilder<PublicIpAddressOperations, PhPublicIPAddress>(this, new PhPublicIPAddress(ipAddress));
        }

        public Pageable<PublicIpAddressOperations> List(ArmSubstringFilter filter = null, int? top = null, CancellationToken cancellationToken = default)
        {
            ArmFilterCollection filters = new ArmFilterCollection(PhPublicIPAddress.ResourceType);
            filters.SubstringFilter = filter;
            return ResourceListOperations.ListAtContext<PublicIpAddressOperations, PhPublicIPAddress>(ClientContext, Id, filters, top, cancellationToken);
        }

        public AsyncPageable<PublicIpAddressOperations> ListAsync(ArmSubstringFilter filter = null, int? top = null, CancellationToken cancellationToken = default)
        {
            ArmFilterCollection filters = new ArmFilterCollection(PhPublicIPAddress.ResourceType);
            filters.SubstringFilter = filter;
            return ResourceListOperations.ListAtContextAsync<PublicIpAddressOperations, PhPublicIPAddress>(ClientContext, Id, filters, top, cancellationToken);
        }

        internal PublicIPAddressesOperations Operations => GetClient<NetworkManagementClient>((uri, cred) => new NetworkManagementClient(Id.Subscription, uri, cred)).PublicIPAddresses;
    }
}