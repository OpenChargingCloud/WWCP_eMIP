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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.EMP
{

    #region OnGetServiceAuthorisationDelegate

    /// <summary>
    /// A delegate called whenever a GetServiceAuthorisation request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the logging request.</param>
    /// <param name="RequestTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="TransactionId">An optional transaction identification.</param>
    /// <param name="PartnerId">The partner identification.</param>
    /// <param name="OperatorId">The operator identification.</param>
    /// <param name="TargetOperatorId">The target operator identification.</param>
    /// <param name="EVSEId">The EVSE identification.</param>
    /// <param name="UserId">The user identification.</param>
    /// <param name="RequestedServiceId">The service identification for which an authorisation is requested.</param>
    /// <param name="ServiceSessionId">The optional service session identification.</param>
    /// <param name="BookingId">The optional booking identification.</param>
    /// 
    /// <param name="RequestTimeout">The timeout of this request.</param>
    public delegate Task

        OnGetServiceAuthorisationRequestDelegate(DateTime            LogTimestamp,
                                                 DateTime            RequestTimestamp,
                                                 EMPServer           Sender,
                                                 String              SenderId,
                                                 EventTracking_Id    EventTrackingId,

                                                 Transaction_Id?     TransactionId,
                                                 Partner_Id          PartnerId,
                                                 Operator_Id         OperatorId,
                                                 Operator_Id         TargetOperatorId,
                                                 EVSE_Id             EVSEId,
                                                 User_Id             UserId,
                                                 Service_Id          RequestedServiceId,
                                                 ServiceSession_Id?  ServiceSessionId,
                                                 Booking_Id?         BookingId,

                                                 TimeSpan?           RequestTimeout);


    /// <summary>
    /// Initiate a GetServiceAuthorisation.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">A 'set service authorisation' request.</param>
    public delegate Task<GetServiceAuthorisationResponse>

        OnGetServiceAuthorisationDelegate(DateTime                        Timestamp,
                                          EMPServer                       Sender,
                                          GetServiceAuthorisationRequest  Request);


    /// <summary>
    /// A delegate called whenever a response to a GetServiceAuthorisation request was sent.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="TransactionId">An optional transaction identification.</param>
    /// <param name="PartnerId">The partner identification.</param>
    /// <param name="OperatorId">The operator identification.</param>
    /// <param name="TargetOperatorId">The target operator identification.</param>
    /// <param name="EVSEId">The EVSE identification.</param>
    /// <param name="UserId">The user identification.</param>
    /// <param name="RequestedServiceId">The service identification for which an authorisation is requested.</param>
    /// <param name="ServiceSessionId">The optional service session identification.</param>
    /// <param name="BookingId">The optional booking identification.</param>
    /// 
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The result of the request.</param>
    /// <param name="Duration">The time between request and response.</param>
    public delegate Task

        OnGetServiceAuthorisationResponseDelegate(DateTime                         Timestamp,
                                                  EMPServer                        Sender,
                                                  String                           SenderId,
                                                  EventTracking_Id                 EventTrackingId,

                                                  Transaction_Id?                  TransactionId,
                                                  Partner_Id                       PartnerId,
                                                  Operator_Id                      OperatorId,
                                                  Operator_Id                      TargetOperatorId,
                                                  EVSE_Id                          EVSEId,
                                                  User_Id                          UserId,
                                                  Service_Id                       RequestedServiceId,
                                                  ServiceSession_Id?               ServiceSessionId,
                                                  Booking_Id?                      BookingId,

                                                  TimeSpan                         RequestTimeout,
                                                  GetServiceAuthorisationResponse  Result,
                                                  TimeSpan                         Duration);

    #endregion

}
