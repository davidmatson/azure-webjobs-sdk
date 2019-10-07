// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.WebJobs.Host.Hosting
{
    public class ServiceCollectionDecorator : IServiceCollection
    {
        public IServiceCollection ServiceCollection { get; set; }

        private HashSet<ServiceDescriptor> _diff; // tracks any service collection modification.

        public ServiceCollectionDecorator(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
            _diff = new HashSet<ServiceDescriptor>();
        }

        public ServiceDescriptor this[int index] { get => ServiceCollection[index]; set => throw new NotImplementedException(); }

        public int Count => ServiceCollection.Count;

        public bool IsReadOnly => ServiceCollection.IsReadOnly;

        public void Add(ServiceDescriptor item)
        {
            _diff.Add(item);
            ServiceCollection.Add(item);
        }

        public void Clear()
        {
            _diff.Clear();
            ServiceCollection.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
           return ServiceCollection.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            ServiceCollection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
           return ServiceCollection.GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            return ServiceCollection.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            _diff.Add(item);
            ServiceCollection.Insert(index, item);
        }

        public bool Remove(ServiceDescriptor item)
        {
            _diff.Remove(item);
            return ServiceCollection.Remove(item);
        }

        public void RemoveAt(int index)
        {
            ServiceDescriptor item = ServiceCollection[index];
            _diff.Remove(item);
            ServiceCollection.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ServiceCollection.GetEnumerator();
        }
    }
}
