using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMessageToSBQPlugin.Logic
{
    public interface ISBClient
    {
        Task SendMessageAsync(string message);
    }
}
