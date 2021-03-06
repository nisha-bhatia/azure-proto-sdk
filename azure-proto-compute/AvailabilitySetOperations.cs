﻿using Azure;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Core;
using System.Threading;
using System.Threading.Tasks;

namespace azure_proto_compute
{
    public class AvailabilitySetOperations : ResourceOperationsBase<AvailabilitySet>, ITaggableResource<AvailabilitySet>, IDeletableResource
    {
        internal AvailabilitySetOperations(ArmResourceOperations genericOperations)
            : base(genericOperations)
        {
        }

        internal AvailabilitySetOperations(AzureResourceManagerClientOptions options, TrackedResource resource)
            : base(options, resource)
        {
        }

        internal AvailabilitySetOperations(AzureResourceManagerClientOptions options, ResourceIdentifier id)
            : base(options, id)
        {
        }

        public static readonly ResourceType ResourceType = "Microsoft.Compute/availabilitySets";

        protected override ResourceType ValidResourceType => ResourceType;

        private AvailabilitySetsOperations Operations => GetClient((uri, cred) =>
            new ComputeManagementClient(
                uri,
                Id.Subscription,
                cred,
                ClientOptions.Convert<ComputeManagementClientOptions>())).AvailabilitySets;

        public ArmResponse<Response> Delete()
        {
            return new ArmResponse(Operations.Delete(Id.ResourceGroup, Id.Name));
        }

        public async Task<ArmResponse<Response>> DeleteAsync(CancellationToken cancellationToken = default)
        {
            return new ArmResponse(await Operations.DeleteAsync(Id.ResourceGroup, Id.Name, cancellationToken));
        }

        public ArmOperation<Response> StartDelete(CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(Operations.Delete(Id.ResourceGroup, Id.Name, cancellationToken));
        }

        public async Task<ArmOperation<Response>> StartDeleteAsync(CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(await Operations.DeleteAsync(Id.ResourceGroup, Id.Name, cancellationToken));
        }
        public override ArmResponse<AvailabilitySet> Get()
        {
            return new PhArmResponse<AvailabilitySet, Azure.ResourceManager.Compute.Models.AvailabilitySet>(
                Operations.Get(Id.ResourceGroup, Id.Name),
                a =>
                {
                    Resource = new AvailabilitySetData(a);
                    return new AvailabilitySet(ClientOptions, Resource as AvailabilitySetData);
                });
        }

        public async override Task<ArmResponse<AvailabilitySet>> GetAsync(CancellationToken cancellationToken = default)
        {
            return new PhArmResponse<AvailabilitySet, Azure.ResourceManager.Compute.Models.AvailabilitySet>(
                await Operations.GetAsync(Id.ResourceGroup, Id.Name, cancellationToken),
                a =>
                {
                    Resource = new AvailabilitySetData(a);
                    return new AvailabilitySet(ClientOptions, Resource as AvailabilitySetData);
                });
        }

        public ArmOperation<AvailabilitySet> Update(AvailabilitySetUpdate patchable)
        {
            return new PhArmOperation<AvailabilitySet, Azure.ResourceManager.Compute.Models.AvailabilitySet>(
                Operations.Update(Id.ResourceGroup, Id.Name, patchable),
                a =>
                {
                    Resource = new AvailabilitySetData(a);
                    return new AvailabilitySet(ClientOptions, Resource as AvailabilitySetData);
                });
        }

        public async Task<ArmOperation<AvailabilitySet>> UpdateAsync(AvailabilitySetUpdate patchable, CancellationToken cancellationToken = default)
        {
            return new PhArmOperation<AvailabilitySet, Azure.ResourceManager.Compute.Models.AvailabilitySet>(
                await Operations.UpdateAsync(Id.ResourceGroup, Id.Name, patchable, cancellationToken),
                a =>
                {
                    Resource = new AvailabilitySetData(a);
                    return new AvailabilitySet(ClientOptions, Resource as AvailabilitySetData);
                });
        }

        public ArmOperation<AvailabilitySet> StartAddTag(string key, string value)
        {
            var patchable = new AvailabilitySetUpdate();
            patchable.Tags[key] = value;
            return Update(patchable);
        }

        public Task<ArmOperation<AvailabilitySet>> StartAddTagAsync(string key, string value, CancellationToken cancellationToken = default)
        {
            var patchable = new AvailabilitySetUpdate();
            patchable.Tags[key] = value;
            return UpdateAsync(patchable);
        }
    }
}
