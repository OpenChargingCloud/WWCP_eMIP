/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    ///// <summary>
    ///// A delegate for filtering EVSE status records.
    ///// </summary>
    ///// <param name="EVSEStatusRecord">An EVSE status record.</param>
    //public delegate Boolean IncludeEVSEStatusRecordsDelegate(EVSEStatusRecord  EVSEStatusRecord);


    #region OnHeartbeatRequest/-Response

    /// <summary>
    /// A delegate called whenever a heartbeat will be send upstream.
    /// </summary>
    public delegate Task OnSendHeartbeatRequestDelegate (DateTime                                 LogTimestamp,
                                                         DateTime                                 RequestTimestamp,
                                                         ICPOClient                               Sender,
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
                                                         ICPOClient                               Sender,
                                                         String                                   SenderId,
                                                         EventTracking_Id                         EventTrackingId,
                                                         Partner_Id                               PartnerId,
                                                         Operator_Id                              OperatorId,
                                                         Transaction_Id?                          TransactionId,
                                                         TimeSpan                                 RequestTimeout,
                                                         HeartbeatResponse                        Result,
                                                         TimeSpan                                 Duration);

    #endregion


    #region OnSetEVSEAvailabilityStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever an EVSE availability status will be send upstream.
    /// </summary>
    public delegate Task OnSetEVSEAvailabilityStatusRequestDelegate (DateTime                                 LogTimestamp,
                                                                     DateTime                                 RequestTimestamp,
                                                                     ICPOClient                               Sender,
                                                                     String                                   SenderId,
                                                                     EventTracking_Id                         EventTrackingId,

                                                                     Partner_Id                               PartnerId,
                                                                     Operator_Id                              OperatorId,
                                                                     EVSE_Id                                  EVSEId,
                                                                     DateTime                                 StatusEventDate,
                                                                     EVSEAvailabilityStatusTypes              AvailabilityStatus,
                                                                     Transaction_Id?                          TransactionId,
                                                                     DateTime?                                AvailabilityStatusUntil,
                                                                     String                                   AvailabilityStatusComment,

                                                                     TimeSpan                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever an EVSE availability status had been sent upstream.
    /// </summary>
    public delegate Task OnSetEVSEAvailabilityStatusResponseDelegate(DateTime                                 LogTimestamp,
                                                                     DateTime                                 RequestTimestamp,
                                                                     ICPOClient                               Sender,
                                                                     String                                   SenderId,
                                                                     EventTracking_Id                         EventTrackingId,

                                                                     Partner_Id                               PartnerId,
                                                                     Operator_Id                              OperatorId,
                                                                     EVSE_Id                                  EVSEId,
                                                                     DateTime                                 StatusEventDate,
                                                                     EVSEAvailabilityStatusTypes              AvailabilityStatus,
                                                                     Transaction_Id?                          TransactionId,
                                                                     DateTime?                                AvailabilityStatusUntil,
                                                                     String                                   AvailabilityStatusComment,

                                                                     TimeSpan                                 RequestTimeout,
                                                                     SetEVSEAvailabilityStatusResponse        Result,
                                                                     TimeSpan                                 Duration);

    #endregion

    #region OnSetEVSEBusyStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever an EVSE busy status will be send upstream.
    /// </summary>
    public delegate Task OnSetEVSEBusyStatusRequestDelegate (DateTime                                 LogTimestamp,
                                                             DateTime                                 RequestTimestamp,
                                                             ICPOClient                               Sender,
                                                             String                                   SenderId,
                                                             EventTracking_Id                         EventTrackingId,

                                                             Partner_Id                               PartnerId,
                                                             Operator_Id                              OperatorId,
                                                             EVSE_Id                                  EVSEId,
                                                             DateTime                                 StatusEventDate,
                                                             EVSEBusyStatusTypes                      BusyStatus,
                                                             Transaction_Id?                          TransactionId,
                                                             DateTime?                                BusyStatusUntil,
                                                             String                                   BusyStatusComment,

                                                             TimeSpan                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever an EVSE busy status had been sent upstream.
    /// </summary>
    public delegate Task OnSetEVSEBusyStatusResponseDelegate(DateTime                                 LogTimestamp,
                                                             DateTime                                 RequestTimestamp,
                                                             ICPOClient                               Sender,
                                                             String                                   SenderId,
                                                             EventTracking_Id                         EventTrackingId,

                                                             Partner_Id                               PartnerId,
                                                             Operator_Id                              OperatorId,
                                                             EVSE_Id                                  EVSEId,
                                                             DateTime                                 StatusEventDate,
                                                             EVSEBusyStatusTypes                      BusyStatus,
                                                             Transaction_Id?                          TransactionId,
                                                             DateTime?                                BusyStatusUntil,
                                                             String                                   BusyStatusComment,

                                                             TimeSpan                                 RequestTimeout,
                                                             SetEVSEBusyStatusResponse                Result,
                                                             TimeSpan                                 Duration);

    #endregion


    #region OnGetServiceAuthorisationRequest/-Response

    /// <summary>
    /// A delegate called whenever a service authorisation request will be send upstream.
    /// </summary>
    public delegate Task OnGetServiceAuthorisationRequestDelegate (DateTime                                 LogTimestamp,
                                                                   DateTime                                 RequestTimestamp,
                                                                   ICPOClient                               Sender,
                                                                   String                                   SenderId,
                                                                   EventTracking_Id                         EventTrackingId,

                                                                   Partner_Id                               PartnerId,
                                                                   Operator_Id                              OperatorId,
                                                                   EVSE_Id                                  EVSEId,
                                                                   User_Id                                  UserId,
                                                                   Service_Id                               RequestedServiceId,
                                                                   Transaction_Id?                          TransactionId,
                                                                   PartnerServiceSession_Id?                PartnerServiceSessionId,

                                                                   TimeSpan                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever a service authorisation request had been sent upstream.
    /// </summary>
    public delegate Task OnGetServiceAuthorisationResponseDelegate(DateTime                                 LogTimestamp,
                                                                   DateTime                                 RequestTimestamp,
                                                                   ICPOClient                               Sender,
                                                                   String                                   SenderId,
                                                                   EventTracking_Id                         EventTrackingId,

                                                                   Partner_Id                               PartnerId,
                                                                   Operator_Id                              OperatorId,
                                                                   EVSE_Id                                  EVSEId,
                                                                   User_Id                                  UserId,
                                                                   Service_Id                               RequestedServiceId,
                                                                   Transaction_Id?                          TransactionId,
                                                                   PartnerServiceSession_Id?                PartnerServiceSessionId,

                                                                   TimeSpan                                 RequestTimeout,
                                                                   GetServiceAuthorisationResponse          Result,
                                                                   TimeSpan                                 Duration);

    #endregion

    #region OnSetChargeDetailRecordRequest/-Response

    /// <summary>
    /// A delegate called whenever new charge detail record will be send upstream.
    /// </summary>
    public delegate Task OnSetChargeDetailRecordRequestDelegate (DateTime                                 LogTimestamp,
                                                                 DateTime                                 RequestTimestamp,
                                                                 ICPOClient                               Sender,
                                                                 String                                   SenderId,
                                                                 EventTracking_Id                         EventTrackingId,

                                                                 Partner_Id                               PartnerId,
                                                                 Operator_Id                              OperatorId,
                                                                 ChargeDetailRecord                       ChargeDetailRecord,
                                                                 Transaction_Id?                          TransactionId,

                                                                 TimeSpan                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever a charge detail record had been sent upstream.
    /// </summary>
    public delegate Task OnSetChargeDetailRecordResponseDelegate(DateTime                                 LogTimestamp,
                                                                 DateTime                                 RequestTimestamp,
                                                                 ICPOClient                               Sender,
                                                                 String                                   SenderId,
                                                                 EventTracking_Id                         EventTrackingId,

                                                                 Partner_Id                               PartnerId,
                                                                 Operator_Id                              OperatorId,
                                                                 ChargeDetailRecord                       ChargeDetailRecord,
                                                                 Transaction_Id?                          TransactionId,

                                                                 TimeSpan                                 RequestTimeout,
                                                                 SetChargeDetailRecordResponse            Result,
                                                                 TimeSpan                                 Duration);

    #endregion

}
