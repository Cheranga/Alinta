using System;
using System.Collections.Generic;
using System.Text;
using Alinta.Core;

namespace Alinta.Services.Abstractions.Requests
{
    public class SearchCustomersRequest : IValidate
    {
        public string Name { get; }

        public SearchCustomersRequest(string name)
        {
            Name = name;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
