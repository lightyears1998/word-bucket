using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordBucket.Services
{
    public class ServiceBase
    {
        public bool IsInitialize { get; private set; }

        public virtual async Task InitializeAsync()
        {
            await Task.CompletedTask;
            IsInitialize = true;
        }
    }
}
