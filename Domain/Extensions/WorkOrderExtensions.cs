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
