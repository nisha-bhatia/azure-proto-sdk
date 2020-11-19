﻿using Azure;
using System.Threading;
using System.Threading.Tasks;

namespace azure_proto_core
{
    public interface ITagable<TOperations, TResource>
        where TResource:Resource 
        where TOperations: ITagable<TOperations, TResource>
    {
        ArmOperation<TOperations> AddTag(string key, string value);
        Task<ArmOperation<TOperations>> AddTagAsync(string key, string value, CancellationToken cancellationToken = default);
    }
}
