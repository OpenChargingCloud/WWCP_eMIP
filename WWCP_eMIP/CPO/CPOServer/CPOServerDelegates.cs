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

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    #region OnSetServiceAuthorisationDelegate

    /// <summary>
    /// A delegate called whenever a SetServiceAuthorisation request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the logging request.</param>
    /// <param name="RequestTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="PartnerId">The partner identification.</param>
    /// <param name="OperatorId">The operator identification.</param>
    /// <param name="TargetOperatorId">The target operator identification.</param>
    /// <param name="EVSEId">The EVSE identification.</param>
    /// <param name="UserId">The user identification.</param>
    /// <param name="RequestedServiceId">The service identification for which an authorisation is requested.</param>
    /// <param name="ServiceSessionId">The service session identification.</param>
    /// <param name="RemoteStartStopValue">Whether to start or stop the charging process.</param>
    /// <param name="IntermediateCDRRequested">Whether the eMSP wishes to receive intermediate charging session records.</param>
    /// <param name="TransactionId">An optional transaction identification.</param>
    /// <param name="UserContractIdAlias">Anonymized alias of the contract id between the end-user and the eMSP.</param>
    /// 
    /// <param name="RequestTimeout">The timeout of this request.</param>
    public delegate Task

        OnSetServiceAuthorisationRequestDelegate(DateTime                   LogTimestamp,
                                                 DateTime                   RequestTimestamp,
                                                 CPOServer                  Sender,
                                                 String                     SenderId,
                                                 EventTracking_Id           EventTrackingId,

                                                 Partner_Id                 PartnerId,
                                                 Operator_Id                OperatorId,
                                                 Operator_Id                TargetOperatorId,
                                                 EVSE_Id                    EVSEId,
                                                 User_Id                    UserId,
                                                 Service_Id                 RequestedServiceId,
                                                 ServiceSession_Id          ServiceSessionId,
                                                 RemoteStartStopValues      RemoteStartStopValue,
                                                 Boolean                    IntermediateCDRRequested,
                                                 Transaction_Id?            TransactionId,
                                                 Contract_Id?               UserContractIdAlias,
                                                 IEnumerable<MeterReport>   MeterLimits,
                                                 String                     Parameter,
                                                 Booking_Id?                BookingId,

                                                 TimeSpan?                  RequestTimeout);


    /// <summary>
    /// Initiate a service authorisation.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">A 'set service authorisation' request.</param>
    public delegate Task<SetServiceAuthorisationResponse>

        OnSetServiceAuthorisationDelegate(DateTime                        Timestamp,
                                          CPOServer                       Sender,
                                          SetServiceAuthorisationRequest  Request);


    /// <summary>
    /// A delegate called whenever a response to a SetServiceAuthorisation request was sent.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="PartnerProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="PartnerSessionId">The unique identification of this charging session on the partner side.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="EVCOId">The unique identification of the e-mobility account.</param>
    /// 
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The result of the request.</param>
    /// <param name="Duration">The time between request and response.</param>
    public delegate Task

        OnSetServiceAuthorisationResponseDelegate(DateTime                             Timestamp,
                                                  CPOServer                            Sender,
                                                  String                               SenderId,
                                                  EventTracking_Id                     EventTrackingId,

                                                  Partner_Id                           PartnerId,
                                                  Operator_Id                          OperatorId,
                                                  Operator_Id                          TargetOperatorId,
                                                  EVSE_Id                              EVSEId,
                                                  User_Id                              UserId,
                                                  Service_Id                           RequestedServiceId,
                                                  ServiceSession_Id                    ServiceSessionId,
                                                  RemoteStartStopValues                RemoteStartStopValue,
                                                  Boolean                              IntermediateCDRRequested,
                                                  Transaction_Id?                      TransactionId,
                                                  Contract_Id?                         UserContractIdAlias,
                                                  IEnumerable<MeterReport>             MeterLimits,
                                                  String                               Parameter,
                                                  Booking_Id?                          BookingId,

                                                  TimeSpan                             RequestTimeout,
                                                  SetServiceAuthorisationResponse      Result,
                                                  TimeSpan                             Duration);

    #endregion

    #region OnSetSessionActionDelegate

    /// <summary>
    /// A delegate called whenever a SetSessionAction request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the logging request.</param>
    /// <param name="RequestTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="PartnerId">The partner identification.</param>
    /// <param name="OperatorId">The operator identification.</param>
    /// <param name="TargetOperatorId">The target operator identification.</param>
    /// <param name="ServiceSessionId">The service session identification.</param>
    /// <param name="SessionAction">The session action.</param>
    /// <param name="TransactionId">An optional transaction identification.</param>
    /// <param name="ExecPartnerSessionId">The partner service session identification.</param>
    /// 
    /// <param name="RequestTimeout">The timeout of this request.</param>
    public delegate Task

        OnSetSessionActionRequestDelegate(DateTime                   LogTimestamp,
                                          DateTime                   RequestTimestamp,
                                          CPOServer                  Sender,
                                          String                     SenderId,
                                          EventTracking_Id           EventTrackingId,

                                          Partner_Id                 PartnerId,
                                          Operator_Id                OperatorId,
                                          Operator_Id                TargetOperatorId,
                                          ServiceSession_Id          ServiceSessionId,
                                          SessionAction              SessionAction,
                                          Transaction_Id?            TransactionId,
                                          PartnerServiceSession_Id?  ExecPartnerSessionId,

                                          TimeSpan?                  RequestTimeout);


    /// <summary>
    /// Initiate a SetSessionAction.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">A 'set service authorisation' request.</param>
    public delegate Task<SetSessionActionRequestResponse>

        OnSetSessionActionDelegate(DateTime                 Timestamp,
                                   CPOServer                Sender,
                                   SetSessionActionRequestRequest  Request);


    /// <summary>
    /// A delegate called whenever a response to a SetSessionAction request was sent.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="PartnerId">The partner identification.</param>
    /// <param name="OperatorId">The operator identification.</param>
    /// <param name="TargetOperatorId">The target operator identification.</param>
    /// <param name="ServiceSessionId">The service session identification.</param>
    /// <param name="SessionAction">The session action.</param>
    /// <param name="TransactionId">An optional transaction identification.</param>
    /// <param name="ExecPartnerSessionId">The partner service session identification.</param>
    /// 
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The result of the request.</param>
    /// <param name="Duration">The time between request and response.</param>
    public delegate Task

        OnSetSessionActionResponseDelegate(DateTime                   Timestamp,
                                           CPOServer                  Sender,
                                           String                     SenderId,
                                           EventTracking_Id           EventTrackingId,

                                           Partner_Id                 PartnerId,
                                           Operator_Id                OperatorId,
                                           Operator_Id                TargetOperatorId,
                                           ServiceSession_Id          ServiceSessionId,
                                           SessionAction              SessionAction,
                                           Transaction_Id?            TransactionId,
                                           PartnerServiceSession_Id?  ExecPartnerSessionId,

                                           TimeSpan                   RequestTimeout,
                                           SetSessionActionRequestResponse   Result,
                                           TimeSpan                   Duration);

    #endregion

}
