﻿using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using azure_proto_core.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace azure_proto_core
{
    /// <summary>
    /// The entry point for all ARM clients.  Note that, we may not want to take a dirrect dependency on Azure.Identity, so we may make the
    /// credential required.
    /// TODO: What is appropriate naming for ArmClient , given that we would not liek to make distinctions between data and management.
    /// </summary>
    public class ArmClient
    {
        internal static readonly string DefaultUri = "https://management.azure.com";

        public Dictionary<string, string> ApiVersionOverrides { get; private set; }

        public ArmClient(ArmClientOptions options = null) : this(new Uri(DefaultUri), new DefaultAzureCredential(), null, options ?? new ArmClientOptions()) { }

        public ArmClient(string defaultSubscriptionId, ArmClientOptions options = null) : this(new Uri(DefaultUri), new DefaultAzureCredential(), defaultSubscriptionId, options ?? new ArmClientOptions()) { }

        public ArmClient(TokenCredential credential, string defaultSubscriptionId, ArmClientOptions options = null) : this(new Uri(DefaultUri), credential, defaultSubscriptionId, options ?? new ArmClientOptions()) { }

        public ArmClient(Uri baseUri, TokenCredential credential, ArmClientOptions options = null) : this(baseUri, credential, null, options ?? new ArmClientOptions()) { }

        public ArmClientOptions ClientOptions { get; private set; }

        public ArmClient(Uri baseUri, TokenCredential credential, string defaultSubscriptionId, ArmClientOptions options)
        {
            ClientOptions = options;
            ClientContext = new ArmClientContext(baseUri, credential);
            defaultSubscriptionId ??= GetDefaultSubscription().ConfigureAwait(false).GetAwaiter().GetResult();
            DefaultSubscription = new SubscriptionOperations(ClientContext, new ResourceIdentifier($"/subscriptions/{defaultSubscriptionId}"), options);
            ApiVersionOverrides = new Dictionary<string, string>();
        }

        public SubscriptionOperations DefaultSubscription { get; private set; }

        internal virtual ArmClientContext ClientContext { get; }

        public SubscriptionOperations Subscription(PhSubscriptionModel subscription) => new SubscriptionOperations(this.ClientContext, subscription, ClientOptions);

        /// <summary>
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public SubscriptionOperations Subscription(ResourceIdentifier subscription) => new SubscriptionOperations(this.ClientContext, subscription, ClientOptions);

        public SubscriptionOperations Subscription(string subscription) => new SubscriptionOperations(this.ClientContext, subscription, ClientOptions);

        public SubscriptionContainerOperations Subscriptions()
        {
            return new SubscriptionContainerOperations(this.ClientContext, ClientOptions);
        }

        public AsyncPageable<PhLocation> ListLocationsAsync(string subscriptionId = null, CancellationToken token = default(CancellationToken))
        {
            async Task<AsyncPageable<PhLocation>> PageableFunc()
            {
                if (string.IsNullOrWhiteSpace(subscriptionId))
                {
                    subscriptionId = (await GetDefaultSubscription(token));
                    if (subscriptionId == null)
                    {
                        throw new InvalidOperationException("Please select a default subscription");
                    }
                }

                return new PhWrappingAsyncPageable<Azure.ResourceManager.Resources.Models.Location, PhLocation>(SubscriptionsClient.ListLocationsAsync(subscriptionId, token), s => new PhLocation(s));

            }

            return new PhTaskDeferringAsyncPageable<PhLocation>(PageableFunc);

        }

        public Pageable<PhLocation> ListLocations(string subscriptionId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                subscriptionId = GetDefaultSubscription(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
                if (subscriptionId == null)
                {
                    throw new InvalidOperationException("Please select a default subscription");
                }
            }

            return new PhWrappingPageable<Azure.ResourceManager.Resources.Models.Location, PhLocation>(SubscriptionsClient.ListLocations(subscriptionId, cancellationToken), s => new PhLocation(s));
        }

        public async IAsyncEnumerable<azure_proto_core.Location> ListAvailableLocationsAsync(ResourceType resourceType, [EnumeratorCancellation] CancellationToken cancellationToken = default(CancellationToken))
        {
            var subscriptionId = (await GetDefaultSubscription(cancellationToken));
            if (subscriptionId == null)
            {
                throw new InvalidOperationException("Please select a default subscription");
            }

            await foreach (var location in ListAvailableLocationsAsync(subscriptionId, resourceType, cancellationToken))
            {
                yield return location;
            }
        }

        public async IAsyncEnumerable<azure_proto_core.Location> ListAvailableLocationsAsync(string subscription, ResourceType resourceType, [EnumeratorCancellation] CancellationToken cancellationToken = default(CancellationToken))
        {
            await foreach (var provider in GetResourcesClient(subscription).Providers.ListAsync(expand: "metadata", cancellationToken: cancellationToken).WithCancellation(cancellationToken))
            {
                if (string.Equals(provider.Namespace, resourceType?.Namespace, StringComparison.InvariantCultureIgnoreCase))
                {
                    var foundResource = provider.ResourceTypes.FirstOrDefault(p => resourceType.Equals(p.ResourceType));
                    foreach (var location in foundResource.Locations)
                    {
                        yield return new azure_proto_core.Location(location);
                    }
                }
            }
        }

        public IEnumerable<azure_proto_core.Location> ListAvailableLocations(ResourceType resourceType, CancellationToken cancellationToken = default(CancellationToken))
        {
            var subscription = GetDefaultSubscription().ConfigureAwait(false).GetAwaiter().GetResult();
            return ListAvailableLocations(subscription, resourceType, cancellationToken);
        }

        public IEnumerable<azure_proto_core.Location> ListAvailableLocations(string subscription, ResourceType resourceType, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetResourcesClient(subscription).Providers.List(expand: "metadata", cancellationToken: cancellationToken)
                .FirstOrDefault(p => string.Equals(p.Namespace, resourceType?.Namespace, StringComparison.InvariantCultureIgnoreCase))
                .ResourceTypes.FirstOrDefault(r => resourceType.Equals(r.ResourceType))
                .Locations.Select(l => new azure_proto_core.Location(l));
        }

        public ResourceGroupOperations ResourceGroup(string subscription, string resourceGroup)
        {
            return new ResourceGroupOperations(this.ClientContext, $"/subscriptions/{subscription}/resourceGroups/{resourceGroup}", ClientOptions);
        }

        public ResourceGroupOperations ResourceGroup(ResourceIdentifier resourceGroup)
        {
            return new ResourceGroupOperations(this.ClientContext, resourceGroup, ClientOptions);
        }
        public ResourceGroupOperations ResourceGroup(PhResourceGroup resourceGroup)
        {
            return new ResourceGroupOperations(this.ClientContext, resourceGroup.Id, ClientOptions);
        }

        public T GetResourceOperationsBase<T>(TrackedResource resource) where T : TrackedResource
        {
            return Activator.CreateInstance(typeof(T), ClientContext, resource) as T;
        }

        public T GetResourceOperationsBase<T>(ResourceIdentifier resource) where T : OperationsBase
        {
            return Activator.CreateInstance(typeof(T), ClientContext, resource) as T;
        }

        public T GetResourceOperationsBase<T>(string subscription, string resourceGroup, string name) where T : OperationsBase
        {
            return null;
        }

        public ArmResponse<TOperations> CreateResource<TContainer, TOperations, TResource>(string subscription, string resourceGroup, string name, TResource model, azure_proto_core.Location location = default)
            where TResource : TrackedResource
            where TOperations : ResourceOperationsBase<TOperations, TResource>
            where TContainer : ResourceContainerOperations<TOperations, TResource>
        {
            if (location == null)
            {
                location = azure_proto_core.Location.Default;
            }

            TContainer container = Activator.CreateInstance(typeof(TContainer), ClientContext, new ArmResource($"/subscriptions/{subscription}/resourceGroups/{resourceGroup}", location)) as TContainer;

            return container.Create(name, model);
        }

        /// <summary>
        /// Fill in the default subscription in the simple case (passed in, or only one subscription available)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task<string> GetDefaultSubscription(CancellationToken token = default(CancellationToken))
        {
            string sub = DefaultSubscription?.Id?.Subscription;
            if (null == sub)
            {
               sub = await this.Subscriptions().GetDefaultSubscription();
            }
            return sub;
        }

        internal SubscriptionsOperations SubscriptionsClient => GetResourcesClient(Guid.NewGuid().ToString()).Subscriptions;

        internal ResourcesManagementClient GetResourcesClient(string subscription) => ClientContext.GetClient((uri, credential) => new ResourcesManagementClient(uri, subscription, credential));
    }
}
