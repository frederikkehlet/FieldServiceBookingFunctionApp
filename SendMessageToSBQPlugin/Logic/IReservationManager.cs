﻿using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMessageToSBQPlugin
{
    public interface IReservationManager
    {   
        Task HandleReservation(Entity reservation);
    }
}
