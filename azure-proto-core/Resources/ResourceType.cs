﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace azure_proto_core
{
    /// <summary>
    /// Structure respresenting a resource type
    /// </summary>
    public class ResourceType : IEquatable<ResourceType>, IEquatable<string>, IComparable<ResourceType>, IComparable<string>
    {
        /// <summary>
        /// The "none" resource type
        /// </summary>
        public static readonly ResourceType None = new ResourceType
        {
            Namespace = string.Empty,
            Type = string.Empty
        };

        private ResourceType()
        {
        }

        public ResourceType(string resourceIdOrType)
        {
            Parse(resourceIdOrType);
        }

        public string Namespace { get; private set; }

        public string Type { get; private set; }

        public ResourceType Parent
        {
            get
            {
                var parts = Type.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) return ResourceType.None;
                var list = new List<string>(parts); //ASK: this line is never hit? parts always has length 1
                list.RemoveAt(list.Count - 1);
                return new ResourceType($"{Namespace}/{string.Join("/", list.ToArray())}");
            }
        }

        internal void Parse(string resourceIdOrType)
        {
            resourceIdOrType = resourceIdOrType?.Trim('/');
            if (string.IsNullOrWhiteSpace(resourceIdOrType))
            {
                throw new ArgumentOutOfRangeException(nameof(resourceIdOrType));
            }

            var parts = resourceIdOrType.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (parts.Count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(resourceIdOrType));//ASK: how to catch exceptions in Assert
            }
            if (parts.Count == 1)
            {
                // Simple resource type
                Type = parts[0];
                Namespace = "Microsoft.Resources";
            }
            if (parts.Contains(ResourceIdentifier.KnownKeys.ProviderNamespace))
            {
                // it is a resource id from a provider
                var index = parts.IndexOf(ResourceIdentifier.KnownKeys.ProviderNamespace);
                for (int i = index; i >= 0; --i)
                {
                    parts.RemoveAt(i);
                }
                if (parts.Count < 3)
                {
                    throw new ArgumentOutOfRangeException(nameof(resourceIdOrType), "Invalid resource id.");
                }

                var type = new List<string>();
                for (int i = 1; i < parts.Count; i += 2)
                {
                    type.Add(parts[i]);
                }

                Namespace = parts[0];
                Type = string.Join("/", type);
            }
            else if (parts[0].Contains('.'))
            {
                // it is a full type name
                Namespace = parts[0];
                Type = string.Join("/", parts.Skip(Math.Max(0, 1)).Take(parts.Count() - 1)); //ask Mark?
            }
            else if (parts.Count % 2 == 0)
            {
                // primitive resource manager resource id
                Namespace = "Microsoft.Resources";
                Type = parts[parts.Count - 2];
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(resourceIdOrType));
            }
        }

        public override string ToString()
        {
            return $"{Namespace}/{Type}";
        }

        public bool Equals(ResourceType other)
        {
            return string.Equals(this.ToString(), other.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        public bool Equals(string other)
        {
            return string.Equals(this.ToString(), other, StringComparison.InvariantCultureIgnoreCase);
        }

        public int CompareTo(ResourceType other)
        {
            if (this.ToString() == ResourceType.None && other.ToString() == ResourceType.None)
                return 0;
            else if (this.ToString() == ResourceType.None)
                return -1;
            return this.ToString().CompareTo(other.ToString());
        }

        public int CompareTo(string other)
        {
            if(this.ToString() == ResourceType.None && other == ResourceType.None)
                return 0;
            else if (this.ToString() == ResourceType.None)
                return -1;
            return this.ToString().CompareTo(other);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var resourceObj = obj as ResourceType;
            if (resourceObj != null) return Equals(resourceObj);
            var stringObj = obj as string;
            if (stringObj != null) return Equals(stringObj);
            return false;  //ASK: if it should be false
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static implicit operator ResourceType(string other) => new ResourceType(other); //ASK: Add null check for all of these - use Object.Reference Equals?
        public static bool operator ==(ResourceType source, string target) => source.Equals(target);
        public static bool operator ==(string source, ResourceType target) => target.Equals(source);
        public static bool operator ==(ResourceType source, ResourceType target) => source.Equals(target);
        public static bool operator !=(ResourceType source, string target) => !source.Equals(target);
        public static bool operator !=(string source, ResourceType target) => !target.Equals(source);
        public static bool operator !=(ResourceType source, ResourceType target) => !source.Equals(target);
    }
}
