using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SendMessageToSBQPlugin.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SendMessageToSBQPlugin.Logic.Implementation
{
    public class ReservationManager : IReservationManager
    {
        private readonly IOrganizationService _service;
        private readonly ISBClient _sbClient;

        public ReservationManager(IOrganizationService service)
        {
            _service = service;
            _sbClient = new SBClient();
        }

        public async Task HandleReservation(Entity reservation)
        {
            Reservation reservationMessage = CreateReservationMessage(reservation);
            await _sbClient.SendMessageAsync(JsonSerializer.Serialize(reservationMessage));
        }

        private Reservation CreateReservationMessage(Entity reservation)
        {
            Entity workOrder = RetrieveWorkOrderFor(reservation);
            Entity user = RetrieveResourceUserFor(reservation);

            return new Reservation
            {
                Start = reservation.GetAttributeValue<DateTime>("starttime"),
                End = reservation.GetAttributeValue<DateTime>("starttime"),
                WorkOrderNumber = workOrder.GetAttributeValue<string>("msdyn_name"),
                ResourceUserId = user.GetAttributeValue<Guid>("azureactivedirectoryobjectid"),
                Address = new Address
                {
                    Street = workOrder.GetAttributeValue<string>("msdyn_address1"),
                    City = workOrder.GetAttributeValue<string>("msdyn_city"),
                    PostalCode = workOrder.GetAttributeValue<string>("msdyn_postalcode"),
                    Country = workOrder.GetAttributeValue<string>("msdyn_country")
                }
            };
        }

        private Entity RetrieveWorkOrderFor(Entity reservation)
        {
            return _service.Retrieve("msdyn_workorder",
                        reservation.GetAttributeValue<EntityReference>("_msdyn_workorder_value").Id,
                        new ColumnSet(
                            "msdyn_name",
                            "msdyn_address1",
                            "msdyn_city",
                            "msdyn_postalcode",
                            "msdyn_country")
                        );
        }

        private Entity RetrieveResourceUserFor(Entity reservation)
        {   
            Entity resource = _service.Retrieve("bookableresource",
                reservation.GetAttributeValue<EntityReference>("_resource_value").Id, 
                new ColumnSet("bookableresourceid"));

            return _service.Retrieve("systemuser",
                resource.GetAttributeValue<EntityReference>("_userid_value").Id,
                new ColumnSet("azureactivedirectoryobjectid"));
        }
    }
}
