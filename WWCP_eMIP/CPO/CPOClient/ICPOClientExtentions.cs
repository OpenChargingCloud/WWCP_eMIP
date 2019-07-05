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
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// Extention methods for the eMIP CPO client interface.
    /// </summary>
    public static class ICPOClientExtentions
    {

        #region SendHeartbeat(PartnerId, OperatorId, TransactionId = null, ...)

        /// <summary>
        /// Send a heartbeat.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<HeartbeatResponse>>

            SendHeartbeat(this ICPOClient     ICPOClient,
                          Partner_Id          PartnerId,
                          Operator_Id         OperatorId,
                          Transaction_Id?     TransactionId       = null,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id    EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null)


                => ICPOClient.SendHeartbeat(new HeartbeatRequest(PartnerId,
                                                                 OperatorId,
                                                                 TransactionId,

                                                                 Timestamp,
                                                                 CancellationToken,
                                                                 EventTrackingId,
                                                                 RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion


        #region SetEVSEAvailabilityStatus(PartnerId, OperatorId, EVSEId, StatusEventDate, AvailabilityStatus, TransactionId = null, ...)

        /// <summary>
        /// Upload the given EVSE availability status.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="StatusEventDate">The timestamp of the status change.</param>
        /// <param name="AvailabilityStatus">The EVSE availability status.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="AvailabilityStatusUntil">An optional timestamp until which the given availability status is valid.</param>
        /// <param name="AvailabilityStatusComment">An optional comment about the availability status.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<SetEVSEAvailabilityStatusResponse>>

            SetEVSEAvailabilityStatus(this ICPOClient              ICPOClient,
                                      Partner_Id                   PartnerId,
                                      Operator_Id                  OperatorId,
                                      EVSE_Id                      EVSEId,
                                      DateTime                     StatusEventDate,
                                      EVSEAvailabilityStatusTypes  AvailabilityStatus,
                                      Transaction_Id?              TransactionId               = null,
                                      DateTime?                    AvailabilityStatusUntil     = null,
                                      String                       AvailabilityStatusComment   = null,

                                      DateTime?                    Timestamp                   = null,
                                      CancellationToken?           CancellationToken           = null,
                                      EventTracking_Id             EventTrackingId             = null,
                                      TimeSpan?                    RequestTimeout              = null)


                => ICPOClient.SetEVSEAvailabilityStatus(new SetEVSEAvailabilityStatusRequest(PartnerId,
                                                                                             OperatorId,
                                                                                             EVSEId,
                                                                                             StatusEventDate,
                                                                                             AvailabilityStatus,
                                                                                             TransactionId,
                                                                                             AvailabilityStatusUntil,
                                                                                             AvailabilityStatusComment,

                                                                                             Timestamp,
                                                                                             CancellationToken,
                                                                                             EventTrackingId,
                                                                                             RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion

        #region SetEVSEBusyStatus(PartnerId, OperatorId, EVSEId, StatusEventDate, BusyStatus, TransactionId = null, ...)

        /// <summary>
        /// Upload the given EVSE busy status.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="StatusEventDate">The timestamp of the status change.</param>
        /// <param name="BusyStatus">The EVSE busy status.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="BusyStatusUntil">An optional timestamp until which the given busy status is valid.</param>
        /// <param name="BusyStatusComment">An optional comment about the busy status.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<SetEVSEBusyStatusResponse>>

            SetEVSEBusyStatus(this ICPOClient      ICPOClient,
                              Partner_Id           PartnerId,
                              Operator_Id          OperatorId,
                              EVSE_Id              EVSEId,
                              DateTime             StatusEventDate,
                              EVSEBusyStatusTypes  BusyStatus,
                              Transaction_Id?      TransactionId       = null,
                              DateTime?            BusyStatusUntil     = null,
                              String               BusyStatusComment   = null,

                              DateTime?            Timestamp           = null,
                              CancellationToken?   CancellationToken   = null,
                              EventTracking_Id     EventTrackingId     = null,
                              TimeSpan?            RequestTimeout      = null)


                => ICPOClient.SetEVSEBusyStatus(new SetEVSEBusyStatusRequest(PartnerId,
                                                                             OperatorId,
                                                                             EVSEId,
                                                                             StatusEventDate,
                                                                             BusyStatus,
                                                                             TransactionId,
                                                                             BusyStatusUntil,
                                                                             BusyStatusComment,

                                                                             Timestamp,
                                                                             CancellationToken,
                                                                             EventTrackingId,
                                                                             RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion


        #region GetServiceAuthorisation(PartnerId, OperatorId, EVSEId, UserId, RequestedServiceId, ...)

        /// <summary>
        /// Request an service authorisation.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="UserId">The user identification.</param>
        /// <param name="RequestedServiceId">The service identification for which an authorisation is requested.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="PartnerServiceSessionId">An optional partner session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<GetServiceAuthorisationResponse>>

            GetServiceAuthorisation(this ICPOClient            ICPOClient,
                                    Partner_Id                 PartnerId,
                                    Operator_Id                OperatorId,
                                    EVSE_Id                    EVSEId,
                                    User_Id                    UserId,
                                    Service_Id                 RequestedServiceId,
                                    Transaction_Id?            TransactionId             = null,
                                    PartnerServiceSession_Id?  PartnerServiceSessionId   = null,

                                    DateTime?                  Timestamp                 = null,
                                    CancellationToken?         CancellationToken         = null,
                                    EventTracking_Id           EventTrackingId           = null,
                                    TimeSpan?                  RequestTimeout            = null)


                => ICPOClient.GetServiceAuthorisation(new GetServiceAuthorisationRequest(PartnerId,
                                                                                         OperatorId,
                                                                                         EVSEId,
                                                                                         UserId,
                                                                                         RequestedServiceId,
                                                                                         TransactionId,
                                                                                         PartnerServiceSessionId,

                                                                                         Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion

        #region SetChargeDetailRecord(PartnerId, OperatorId, ChargeDetailRecord, ...)

        /// <summary>
        /// Upload the given charge detail record.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="ChargeDetailRecord">The charge detail record.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<SetChargeDetailRecordResponse>>

            SetChargeDetailRecord(this ICPOClient     ICPOClient,
                                  Partner_Id          PartnerId,
                                  Operator_Id         OperatorId,
                                  ChargeDetailRecord  ChargeDetailRecord,
                                  Transaction_Id?     TransactionId       = null,

                                  DateTime?           Timestamp           = null,
                                  CancellationToken?  CancellationToken   = null,
                                  EventTracking_Id    EventTrackingId     = null,
                                  TimeSpan?           RequestTimeout      = null)


                => ICPOClient.SetChargeDetailRecord(new SetChargeDetailRecordRequest(PartnerId,
                                                                                     OperatorId,
                                                                                     ChargeDetailRecord,
                                                                                     TransactionId,

                                                                                     Timestamp,
                                                                                     CancellationToken,
                                                                                     EventTrackingId,
                                                                                     RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion


    }

}
