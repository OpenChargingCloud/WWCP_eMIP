/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.EMP
{

    /// <summary>
    /// Extention methods for the eMIP EMP client interface.
    /// </summary>
    public static class IEMPClientExtentions
    {

        #region SendHeartbeat          (this EMPClient, PartnerId, OperatorId, TransactionId = null, ...)

        /// <summary>
        /// Send a heartbeat.
        /// </summary>
        /// <param name="EMPClient">An eMIP EMP client.</param>
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

            SendHeartbeat(this IEMPClient     EMPClient,
                          Partner_Id          PartnerId,
                          Operator_Id         OperatorId,
                          Transaction_Id?     TransactionId       = null,

                          HTTPRequest         HTTPRequest         = null,
                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id    EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null)


                => EMPClient.SendHeartbeat(new HeartbeatRequest(PartnerId,
                                                                OperatorId,
                                                                TransactionId,

                                                                HTTPRequest,
                                                                Timestamp,
                                                                CancellationToken,
                                                                EventTrackingId,
                                                                RequestTimeout ?? EMPClient.RequestTimeout));

        #endregion


        #region SetServiceAuthorisation(this EMPClient, PartnerId, OperatorId, EVSEId, UserId, RequestedServiceId, AuthorisationValue, IntermediateCDRRequested, ...)

        /// <summary>
        /// Send a SetServiceAuthorisation.
        /// </summary>
        /// <param name="EMPClient">An eMIP EMP client.</param>
        /// 
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="UserId">The user identification.</param>
        /// <param name="RequestedServiceId">The service identification for which an authorisation is requested.</param>
        /// <param name="PartnerServiceSessionId">The partner service session identification.</param>
        /// <param name="AuthorisationValue">Whether to start or stop the charging process.</param>
        /// <param name="IntermediateCDRRequested">Whether the eMSP wishes to receive intermediate charging session records.</param>
        /// 
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="UserContractIdAlias">Anonymized alias of the contract id between the end-user and the eMSP.</param>
        /// <param name="MeterLimits">Meter limits for this authorisation: The eMSP can authorise the charge but for less than x Wh or y minutes, or z euros.</param>
        /// <param name="Parameter">eMSP parameter string (reserved for future use).</param>
        /// <param name="BookingId"></param>
        /// <param name="SalePartnerBookingId"></param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<SetServiceAuthorisationResponse>>

            SetServiceAuthorisation(this IEMPClient            EMPClient,
                                    Partner_Id                 PartnerId,
                                    Operator_Id                OperatorId,
                                    EVSE_Id                    EVSEId,
                                    User_Id                    UserId,
                                    Service_Id                 RequestedServiceId,
                                    RemoteStartStopValues      AuthorisationValue,
                                    Boolean                    IntermediateCDRRequested,

                                    Transaction_Id?            TransactionId             = null,
                                    PartnerServiceSession_Id?  PartnerServiceSessionId   = null,
                                    Contract_Id?               UserContractIdAlias       = null,
                                    IEnumerable<MeterReport>   MeterLimits               = null,
                                    String                     Parameter                 = null,
                                    Booking_Id?                BookingId                 = null,
                                    Booking_Id?                SalePartnerBookingId      = null,

                                    HTTPRequest                HTTPRequest               = null,
                                    DateTime?                  Timestamp                 = null,
                                    CancellationToken?         CancellationToken         = null,
                                    EventTracking_Id           EventTrackingId           = null,
                                    TimeSpan?                  RequestTimeout            = null)


                => EMPClient.SetServiceAuthorisation(new SetServiceAuthorisationRequest(PartnerId,
                                                                                        OperatorId,
                                                                                        EVSEId,
                                                                                        UserId,
                                                                                        RequestedServiceId,
                                                                                        AuthorisationValue,
                                                                                        IntermediateCDRRequested,

                                                                                        TransactionId,
                                                                                        PartnerServiceSessionId,
                                                                                        UserContractIdAlias,
                                                                                        MeterLimits,
                                                                                        Parameter,
                                                                                        BookingId,
                                                                                        SalePartnerBookingId,

                                                                                        HTTPRequest,
                                                                                        Timestamp,
                                                                                        CancellationToken,
                                                                                        EventTrackingId,
                                                                                        RequestTimeout ?? EMPClient.RequestTimeout));

        #endregion

        #region SetSessionAction       (this EMPClient, PartnerId, OperatorId, ServiceSessionId, SessionAction, ...)

        /// <summary>
        /// Create a SetSessionActionRequest XML/SOAP request.
        /// </summary>
        /// <param name="EMPClient">An eMIP EMP client.</param>
        /// 
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="ServiceSessionId">The service session identification.</param>
        /// <param name="SessionAction">The session action.</param>
        /// 
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="SalePartnerSessionId">An optional partner service session identification.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<SetSessionActionResponse>>

            SetSessionAction(this IEMPClient            EMPClient,
                             Partner_Id                 PartnerId,
                             Operator_Id                OperatorId,
                             ServiceSession_Id          ServiceSessionId,
                             SessionAction              SessionAction,

                             Transaction_Id?            TransactionId          = null,
                             PartnerServiceSession_Id?  SalePartnerSessionId   = null,

                             HTTPRequest                HTTPRequest            = null,
                             DateTime?                  Timestamp              = null,
                             CancellationToken?         CancellationToken      = null,
                             EventTracking_Id           EventTrackingId        = null,
                             TimeSpan?                  RequestTimeout         = null)


                => EMPClient.SetSessionAction(new SetSessionActionRequest(PartnerId,
                                                                          OperatorId,
                                                                          ServiceSessionId,
                                                                          SessionAction,

                                                                          TransactionId,
                                                                          SalePartnerSessionId,

                                                                          HTTPRequest,
                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout ?? EMPClient.RequestTimeout));


        #endregion


    }

}
