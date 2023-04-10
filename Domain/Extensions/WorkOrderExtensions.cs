using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class WorkOrderExtensions
    {
        public static string GetFormattedAddress(this msdyn_workorder workOrder)
        {
            return $"{workOrder.msdyn_Address1}, {workOrder.msdyn_PostalCode} {workOrder.msdyn_City}, {workOrder.msdyn_Country}";
        }
    }
}
