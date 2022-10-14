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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
{

    /// <summary>
    /// The common interface of all eMIP CPO clients.
    /// </summary>
    public interface ICPOClient : IHTTPClient
    {

        #region Events

        #region OnHeartbeatRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a heartbeat will be send.
        /// </summary>
        event OnSendHeartbeatRequestDelegate   OnSendHeartbeatRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a heartbeat will be send.
        /// </summary>
        event ClientRequestLogHandler          OnSendHeartbeatSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a heartbeat SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler         OnSendHeartbeatSOAPResponse;

        /// <summary>
        /// An event fired whenever EVSE status records had been sent upstream.
        /// </summary>
        event OnSendHeartbeatResponseDelegate  OnSendHeartbeatResponse;

        #endregion


        #region OnSetChargingPoolAvailabilityStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a charging pool availability status will be send.
        /// </summary>
        event OnSetChargingPoolAvailabilityStatusRequestDelegate   OnSetChargingPoolAvailabilityStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a charging pool availability status will be send.
        /// </summary>
        event ClientRequestLogHandler                              OnSetChargingPoolAvailabilityStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a charging pool availability status SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                             OnSetChargingPoolAvailabilityStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a charging pool availability status request had been received.
        /// </summary>
        event OnSetChargingPoolAvailabilityStatusResponseDelegate  OnSetChargingPoolAvailabilityStatusResponse;

        #endregion

        #region OnSetChargingStationAvailabilityStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a charging station availability status will be send.
        /// </summary>
        event OnSetChargingStationAvailabilityStatusRequestDelegate   OnSetChargingStationAvailabilityStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a charging station availability status will be send.
        /// </summary>
        event ClientRequestLogHandler                                 OnSetChargingStationAvailabilityStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a charging station availability status SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                                OnSetChargingStationAvailabilityStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a charging station availability status request had been received.
        /// </summary>
        event OnSetChargingStationAvailabilityStatusResponseDelegate  OnSetChargingStationAvailabilityStatusResponse;

        #endregion

        #region OnSetEVSEAvailabilityStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending an EVSE availability status will be send.
        /// </summary>
        event OnSetEVSEAvailabilityStatusRequestDelegate   OnSetEVSEAvailabilityStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending an EVSE availability status will be send.
        /// </summary>
        event ClientRequestLogHandler                      OnSetEVSEAvailabilityStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an EVSE availability status SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                     OnSetEVSEAvailabilityStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an EVSE availability status request had been received.
        /// </summary>
        event OnSetEVSEAvailabilityStatusResponseDelegate  OnSetEVSEAvailabilityStatusResponse;

        #endregion

        #region OnSetChargingConnectorAvailabilityStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a charging connector availability status will be send.
        /// </summary>
        event OnSetChargingConnectorAvailabilityStatusRequestDelegate   OnSetChargingConnectorAvailabilityStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a charging connector availability status will be send.
        /// </summary>
        event ClientRequestLogHandler                                   OnSetChargingConnectorAvailabilityStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a charging connector availability status SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                                  OnSetChargingConnectorAvailabilityStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a charging connector availability status request had been received.
        /// </summary>
        event OnSetChargingConnectorAvailabilityStatusResponseDelegate  OnSetChargingConnectorAvailabilityStatusResponse;

        #endregion


        #region OnSetEVSEBusyStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending an EVSE busy status will be send.
        /// </summary>
        event OnSetEVSEBusyStatusRequestDelegate   OnSetEVSEBusyStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending an EVSE busy status will be send.
        /// </summary>
        event ClientRequestLogHandler              OnSetEVSEBusyStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an EVSE busy status SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler             OnSetEVSEBusyStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an EVSE busy status request had been received.
        /// </summary>
        event OnSetEVSEBusyStatusResponseDelegate  OnSetEVSEBusyStatusResponse;

        #endregion

        #region OnSetEVSESyntheticStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending an EVSE synthetic status will be send.
        /// </summary>
        event OnSetEVSESyntheticStatusRequestDelegate   OnSetEVSESyntheticStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending an EVSE synthetic status will be send.
        /// </summary>
        event ClientRequestLogHandler                   OnSetEVSESyntheticStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an EVSE synthetic status SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                  OnSetEVSESyntheticStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an EVSE synthetic status request had been received.
        /// </summary>
        event OnSetEVSESyntheticStatusResponseDelegate  OnSetEVSESyntheticStatusResponse;

        #endregion


        #region OnGetServiceAuthorisationRequest/-Response

        /// <summary>
        /// An event fired whenever a GetServiceAuthorisation request will be send.
        /// </summary>
        event OnGetServiceAuthorisationRequestDelegate   OnGetServiceAuthorisationRequest;

        /// <summary>
        /// An event fired whenever a SOAP GetServiceAuthorisation request will be send.
        /// </summary>
        event ClientRequestLogHandler                    OnGetServiceAuthorisationSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a GetServiceAuthorisation SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                   OnGetServiceAuthorisationSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a GetServiceAuthorisation request had been received.
        /// </summary>
        event OnGetServiceAuthorisationResponseDelegate  OnGetServiceAuthorisationResponse;

        #endregion

        #region OnSetSessionEventReportRequest/-Response

        /// <summary>
        /// An event fired whenever a SetSessionEventReport request will be send.
        /// </summary>
        event OnSetSessionEventReportRequestDelegate   OnSetSessionEventReportRequest;

        /// <summary>
        /// An event fired whenever a SOAP SetSessionEventReport request will be send.
        /// </summary>
        event ClientRequestLogHandler                  OnSetSessionEventReportSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a SetSessionEventReport SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                 OnSetSessionEventReportSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a SetSessionEventReport request had been received.
        /// </summary>
        event OnSetSessionEventReportResponseDelegate  OnSetSessionEventReportResponse;

        #endregion


        #region OnSetChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record will be send.
        /// </summary>
        event OnSetChargeDetailRecordRequestDelegate   OnSetChargeDetailRecordRequest;

        /// <summary>
        /// An event fired whenever a SOAP charge detail record will be send.
        /// </summary>
        event ClientRequestLogHandler                  OnSetChargeDetailRecordSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a SOAP charge detail record had been received.
        /// </summary>
        event ClientResponseLogHandler                 OnSetChargeDetailRecordSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a charge detail record had been received.
        /// </summary>
        event OnSetChargeDetailRecordResponseDelegate  OnSetChargeDetailRecordResponse;

        #endregion

        #endregion


        /// <summary>
        /// Send the given heartbeat.
        /// </summary>
        /// <param name="Request">A SendHeartbeat request.</param>
        Task<HTTPResponse<HeartbeatResponse>>
            SendHeartbeat(HeartbeatRequest Request);


        /// <summary>
        /// Send the given charging pool availability status.
        /// </summary>
        /// <param name="Request">A SetChargingPoolAvailabilityStatus request.</param>
        Task<HTTPResponse<SetChargingPoolAvailabilityStatusResponse>>
            SetChargingPoolAvailabilityStatus(SetChargingPoolAvailabilityStatusRequest Request);

        /// <summary>
        /// Send the given charging station availability status.
        /// </summary>
        /// <param name="Request">A SetChargingStationAvailabilityStatus request.</param>
        Task<HTTPResponse<SetChargingStationAvailabilityStatusResponse>>
            SetChargingStationAvailabilityStatus(SetChargingStationAvailabilityStatusRequest Request);

        /// <summary>
        /// Send the given EVSE availability status.
        /// </summary>
        /// <param name="Request">A SetEVSEAvailabilityStatus request.</param>
        Task<HTTPResponse<SetEVSEAvailabilityStatusResponse>>
            SetEVSEAvailabilityStatus(SetEVSEAvailabilityStatusRequest Request);

        /// <summary>
        /// Send the given charging connector availability status.
        /// </summary>
        /// <param name="Request">A SetChargingConnectorAvailabilityStatus request.</param>
        Task<HTTPResponse<SetChargingConnectorAvailabilityStatusResponse>>
            SetChargingConnectorAvailabilityStatus(SetChargingConnectorAvailabilityStatusRequest Request);


        /// <summary>
        /// Send the given EVSE busy status.
        /// </summary>
        /// <param name="Request">A SetEVSEBusyStatus request.</param>
        Task<HTTPResponse<SetEVSEBusyStatusResponse>>
            SetEVSEBusyStatus(SetEVSEBusyStatusRequest Request);

        /// <summary>
        /// Send the given EVSE synthetic status.
        /// </summary>
        /// <param name="Request">A SetEVSESyntheticStatus request.</param>
        Task<HTTPResponse<SetEVSESyntheticStatusResponse>>
            SetEVSESyntheticStatus(SetEVSESyntheticStatusRequest Request);


        /// <summary>
        /// Request an service authorisation.
        /// </summary>
        /// <param name="Request">A GetServiceAuthorisation request.</param>
        Task<HTTPResponse<GetServiceAuthorisationResponse>>
            GetServiceAuthorisation(GetServiceAuthorisationRequest Request);

        /// <summary>
        /// Send a session event report.
        /// </summary>
        /// <param name="Request">A SetSessionEventReport request.</param>
        Task<HTTPResponse<SetSessionEventReportResponse>>

            SetSessionEventReport(SetSessionEventReportRequest Request);


        /// <summary>
        /// Upload the given charge detail record.
        /// </summary>
        /// <param name="Request">A SetChargeDetailRecord request.</param>
        Task<HTTPResponse<SetChargeDetailRecordResponse>>
            SetChargeDetailRecord(SetChargeDetailRecordRequest Request);

    }

}
