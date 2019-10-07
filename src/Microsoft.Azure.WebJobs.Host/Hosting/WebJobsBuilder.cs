// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs.Host.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.WebJobs.Host
{
    internal class WebJobsBuilder : IWebJobsBuilder
    {
        public WebJobsBuilder(IServiceCollection services)
        {
            Services.ServiceCollection = services ?? throw new ArgumentNullException(nameof(services));
        }

        public ServiceCollectionDecorator Services { get; }

       // TODO - okay to have decorator rather than ISC here? 
    }
}
