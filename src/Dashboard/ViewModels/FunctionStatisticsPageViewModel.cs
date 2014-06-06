﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.ViewModels
{
    public class FunctionStatisticsPageViewModel
    {
        public FunctionStatisticsViewModel[] Entries { get; set; }
        public bool HasMore { get; set; }

        // TODO: consider moving these elsewhere as they are not related to functions only
        public string StorageAccountName { get; set; }
        public bool IsOldHost { get; set; }
    }
}