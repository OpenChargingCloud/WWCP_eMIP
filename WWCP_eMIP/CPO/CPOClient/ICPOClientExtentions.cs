/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
{

    /// <summary>
    /// Extension methods for the eMIP CPO client interface.
    /// </summary>
    public static class ICPOClientExtensions
    {

        #region SendHeartbeat            (this CPOClient, PartnerId, OperatorId, TransactionId = null, ...)

        /// <summary>
        /// Send a heartbeat.
        /// </summary>
        /// <param name="CPOClient">An eMIP CPO client.</param>
        /// 
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<HeartbeatResponse>>

            SendHeartbeat(this ICPOClient     CPOClient,
                          Partner_Id          PartnerId,
                          Operator_Id         OperatorId,
                          Transaction_Id?     TransactionId       = null,

                          HTTPRequest         HTTPRequest         = null,
                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id    EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null)


                => CPOClient.SendHeartbeat(new HeartbeatRequest(PartnerId,
                                                                OperatorId,
                                                                TransactionId,

                                                                HTTPRequest,
                                                                Timestamp,
                                                                CancellationToken,
                                                                EventTrackingId,
                                                                RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion


        #region SetEVSEAvailabilityStatus(this CPOClient, PartnerId, OperatorId, EVSEId, StatusEventDate, AvailabilityStatus, TransactionId = null, ...)

        /// <summary>
        /// Upload the given EVSE availability status.
        /// </summary>
        /// <param name="CPOClient">An eMIP CPO client.</param>
        /// 
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="StatusEventDate">The timestamp of the status change.</param>
        /// <param name="AvailabilityStatus">The EVSE availability status.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="AvailabilityStatusUntil">An optional timestamp until which the given availability status is valid.</param>
        /// <param name="AvailabilityStatusComment">An optional comment about the availability status.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<SetEVSEAvailabilityStatusResponse>>

            SetEVSEAvailabilityStatus(this ICPOClient              CPOClient,
                                      Partner_Id                   PartnerId,
                                      Operator_Id                  OperatorId,
                                      EVSE_Id                      EVSEId,
                                      DateTime                     StatusEventDate,
                                      EVSEAvailabilityStatusTypes  AvailabilityStatus,
                                      Transaction_Id?              TransactionId               = null,
                                      DateTime?                    AvailabilityStatusUntil     = null,
                                      String                       AvailabilityStatusComment   = null,

                                      HTTPRequest                  HTTPRequest                 = null,
                                      DateTime?                    Timestamp                   = null,
                                      CancellationToken?           CancellationToken           = null,
                                      EventTracking_Id             EventTrackingId             = null,
                                      TimeSpan?                    RequestTimeout              = null)


                => CPOClient.SetEVSEAvailabilityStatus(new SetEVSEAvailabilityStatusRequest(PartnerId,
                                                                                            OperatorId,
                                                                                            EVSEId,
                                                                                            StatusEventDate,
                                                                                            AvailabilityStatus,
                                                                                            TransactionId,
                                                                                            AvailabilityStatusUntil,
                                                                                            AvailabilityStatusComment,

                                                                                            HTTPRequest,
                                                                                            Timestamp,
                                                                                            CancellationToken,
                                                                                            EventTrackingId,
                                                                                            RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion

        #region SetEVSEBusyStatus        (this CPOClient, PartnerId, OperatorId, EVSEId, StatusEventDate, BusyStatus, TransactionId = null, ...)

        /// <summary>
        /// Upload the given EVSE busy status.
        /// </summary>
        /// <param name="CPOClient">An eMIP CPO client.</param>
        /// 
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="StatusEventDate">The timestamp of the status change.</param>
        /// <param name="BusyStatus">The EVSE busy status.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="BusyStatusUntil">An optional timestamp until which the given busy status is valid.</param>
        /// <param name="BusyStatusComment">An optional comment about the busy status.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<SetEVSEBusyStatusResponse>>

            SetEVSEBusyStatus(this ICPOClient      CPOClient,
                              Partner_Id           PartnerId,
                              Operator_Id          OperatorId,
                              EVSE_Id              EVSEId,
                              DateTime             StatusEventDate,
                              EVSEBusyStatusTypes  BusyStatus,
                              Transaction_Id?      TransactionId       = null,
                              DateTime?            BusyStatusUntil     = null,
                              String               BusyStatusComment   = null,

                              HTTPRequest          HTTPRequest         = null,
                              DateTime?            Timestamp           = null,
                              CancellationToken?   CancellationToken   = null,
                              EventTracking_Id     EventTrackingId     = null,
                              TimeSpan?            RequestTimeout      = null)


                => CPOClient.SetEVSEBusyStatus(new SetEVSEBusyStatusRequest(PartnerId,
                                                                            OperatorId,
                                                                            EVSEId,
                                                                            StatusEventDate,
                                                                            BusyStatus,
                                                                            TransactionId,
                                                                            BusyStatusUntil,
                                                                            BusyStatusComment,

                                                                            HTTPRequest,
                                                                            Timestamp,
                                                                            CancellationToken,
                                                                            EventTrackingId,
                                                                            RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion


        #region GetServiceAuthorisation  (this CPOClient, PartnerId, OperatorId, EVSEId, UserId, RequestedServiceId, ...)

        /// <summary>
        /// Request an service authorisation.
        /// </summary>
        /// <param name="CPOClient">An eMIP CPO client.</param>
        /// 
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="UserId">The user identification.</param>
        /// <param name="RequestedServiceId">The service identification for which an authorisation is requested.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="PartnerServiceSessionId">An optional partner session identification.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<GetServiceAuthorisationResponse>>

            GetServiceAuthorisation(this ICPOClient            CPOClient,
                                    Partner_Id                 PartnerId,
                                    Operator_Id                OperatorId,
                                    EVSE_Id                    EVSEId,
                                    User_Id                    UserId,
                                    Service_Id                 RequestedServiceId,
                                    Transaction_Id?            TransactionId             = null,
                                    PartnerServiceSession_Id?  PartnerServiceSessionId   = null,

                                    HTTPRequest                HTTPRequest               = null,
                                    DateTime?                  Timestamp                 = null,
                                    CancellationToken?         CancellationToken         = null,
                                    EventTracking_Id           EventTrackingId           = null,
                                    TimeSpan?                  RequestTimeout            = null)


                => CPOClient.GetServiceAuthorisation(new GetServiceAuthorisationRequest(PartnerId,
                                                                                        OperatorId,
                                                                                        EVSEId,
                                                                                        UserId,
                                                                                        RequestedServiceId,
                                                                                        TransactionId,
                                                                                        PartnerServiceSessionId,

                                                                                        HTTPRequest,
                                                                                        Timestamp,
                                                                                        CancellationToken,
                                                                                        EventTrackingId,
                                                                                        RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion

        #region SetChargeDetailRecord    (this CPOClient, PartnerId, OperatorId, ChargeDetailRecord, ...)

        /// <summary>
        /// Upload the given charge detail record.
        /// </summary>
        /// <param name="CPOClient">An eMIP CPO client.</param>
        /// 
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="ChargeDetailRecord">The charge detail record.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<SetChargeDetailRecordResponse>>

            SetChargeDetailRecord(this ICPOClient     CPOClient,
                                  Partner_Id          PartnerId,
                                  Operator_Id         OperatorId,
                                  ChargeDetailRecord  ChargeDetailRecord,
                                  Transaction_Id?     TransactionId       = null,

                                  HTTPRequest         HTTPRequest         = null,
                                  DateTime?           Timestamp           = null,
                                  CancellationToken?  CancellationToken   = null,
                                  EventTracking_Id    EventTrackingId     = null,
                                  TimeSpan?           RequestTimeout      = null)


                => CPOClient.SetChargeDetailRecord(new SetChargeDetailRecordRequest(PartnerId,
                                                                                    OperatorId,
                                                                                    ChargeDetailRecord,
                                                                                    TransactionId,

                                                                                    HTTPRequest,
                                                                                    Timestamp,
                                                                                    CancellationToken,
                                                                                    EventTrackingId,
                                                                                    RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion


    }

}
