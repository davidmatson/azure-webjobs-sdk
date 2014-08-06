﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Jobs.Host.Bindings;

namespace Microsoft.Azure.Jobs.Host.Executors
{
    internal interface IBindingSource
    {
        Task<IReadOnlyDictionary<string, IValueProvider>> BindAsync(ValueBindingContext context);
    }
}