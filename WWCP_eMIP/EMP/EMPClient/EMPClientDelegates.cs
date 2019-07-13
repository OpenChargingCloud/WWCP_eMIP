/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
 * This file is part of WWCP eMIP <https://github.com/OpenChargingCloud/WWCP_eMIP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.EMP
{

    #region OnHeartbeatRequest/-Response

    /// <summary>
    /// A delegate called whenever a heartbeat will be send upstream.
    /// </summary>
    public delegate Task OnSendHeartbeatRequestDelegate (DateTime                                 LogTimestamp,
                                                         DateTime                                 RequestTimestamp,
                                                         IEMPClient                               Sender,
                                                         String                                   SenderId,
                                                         EventTracking_Id                         EventTrackingId,
                                                         Partner_Id                               PartnerId,
                                                         Operator_Id                              OperatorId,
                                                         Transaction_Id?                          TransactionId,
                                                         TimeSpan                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever a heartbeat had been sent upstream.
    /// </summary>
    public delegate Task OnSendHeartbeatResponseDelegate(DateTime                                 LogTimestamp,
                                                         DateTime                                 RequestTimestamp,
                                                         IEMPClient                               Sender,
                                                         String                                   SenderId,
                                                         EventTracking_Id                         EventTrackingId,
                                                         Partner_Id                               PartnerId,
                                                         Operator_Id                              OperatorId,
                                                         Transaction_Id?                          TransactionId,
                                                         TimeSpan                                 RequestTimeout,
                                                         HeartbeatResponse                        Result,
                                                         TimeSpan                                 Duration);

    #endregion


    #region OnSetServiceAuthorisationRequest/-Response

    /// <summary>
    /// A delegate called whenever a SetServiceAuthorisation will be send upstream.
    /// </summary>
    public delegate Task OnSetServiceAuthorisationRequestDelegate (DateTime                          LogTimestamp,
                                                                   DateTime                          RequestTimestamp,
                                                                   IEMPClient                        Sender,
                                                                   String                            SenderId,
                                                                   EventTracking_Id                  EventTrackingId,

                                                                   Partner_Id                        PartnerId,
                                                                   Operator_Id                       OperatorId,
                                                                   EVSE_Id                           EVSEId,
                                                                   User_Id                           UserId,
                                                                   Service_Id                        RequestedServiceId,
                                                                   RemoteStartStopValues             AuthorisationValue,
                                                                   Boolean                           IntermediateCDRRequested,

                                                                   Transaction_Id?                   TransactionId,
                                                                   PartnerServiceSession_Id?         PartnerServiceSessionId,
                                                                   Contract_Id?                      UserContractIdAlias,
                                                                   IEnumerable<MeterReport>          MeterLimits,
                                                                   String                            Parameter,
                                                                   Booking_Id?                       BookingId,
                                                                   Booking_Id?                       SalePartnerBookingId,

                                                                   TimeSpan                          RequestTimeout);

    /// <summary>
    /// A delegate called whenever a SetServiceAuthorisation had been sent upstream.
    /// </summary>
    public delegate Task OnSetServiceAuthorisationResponseDelegate(DateTime                          LogTimestamp,
                                                                   DateTime                          RequestTimestamp,
                                                                   IEMPClient                        Sender,
                                                                   String                            SenderId,
                                                                   EventTracking_Id                  EventTrackingId,

                                                                   Partner_Id                        PartnerId,
                                                                   Operator_Id                       OperatorId,
                                                                   EVSE_Id                           EVSEId,
                                                                   User_Id                           UserId,
                                                                   Service_Id                        RequestedServiceId,
                                                                   RemoteStartStopValues             AuthorisationValue,
                                                                   Boolean                           IntermediateCDRRequested,

                                                                   Transaction_Id?                   TransactionId,
                                                                   PartnerServiceSession_Id?         PartnerServiceSessionId,
                                                                   Contract_Id?                      UserContractIdAlias,
                                                                   IEnumerable<MeterReport>          MeterLimits,
                                                                   String                            Parameter,
                                                                   Booking_Id?                       BookingId,
                                                                   Booking_Id?                       SalePartnerBookingId,

                                                                   TimeSpan                          RequestTimeout,
                                                                   SetServiceAuthorisationResponse   Result,
                                                                   TimeSpan                          Duration);

    #endregion

}
