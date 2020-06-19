/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
using System.Xml.Linq;
using System.Net.Security;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// An eMIP roaming client for CPOs which combines the CPO client
    /// and server and adds additional logging for both.
    /// </summary>
    public class CPORoaming : ICPOClient
    {

        #region Properties

        /// <summary>
        /// The CPO client.
        /// </summary>
        public CPOClient        CPOClient         { get; }

        public HTTPHostname Hostname
            => CPOClient.Hostname;

        public HTTPHostname? VirtualHostname
            => CPOClient.VirtualHostname;

        public IPPort RemotePort
            => CPOClient.RemotePort;

        public RemoteCertificateValidationCallback RemoteCertificateValidator
            => CPOClient?.RemoteCertificateValidator;

        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServer        CPOServer         { get; }

        /// <summary>
        /// The CPO server logger.
        /// </summary>
        public CPOServerLogger  CPOServerLogger   { get; }

        /// <summary>
        /// The default request timeout for this client.
        /// </summary>
        public TimeSpan?        RequestTimeout    { get; }


        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient DNSClient
            => CPOClient.DNSClient;

        #endregion

        #region Events

        // CPOClient logging methods

        #region OnHeartbeatRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a heartbeat will be send.
        /// </summary>
        public event OnSendHeartbeatRequestDelegate OnSendHeartbeatRequest
        {

            add
            {
                CPOClient.OnSendHeartbeatRequest += value;
            }

            remove
            {
                CPOClient.OnSendHeartbeatRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request sending a heartbeat will be send.
        /// </summary>
        public event ClientRequestLogHandler OnSendHeartbeatSOAPRequest
        {

            add
            {
                CPOClient.OnSendHeartbeatSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnSendHeartbeatSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a heartbeat SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSendHeartbeatSOAPResponse
        {

            add
            {
                CPOClient.OnSendHeartbeatSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnSendHeartbeatSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a heartbeat request had been received.
        /// </summary>
        public event OnSendHeartbeatResponseDelegate OnSendHeartbeatResponse
        {

            add
            {
                CPOClient.OnSendHeartbeatResponse += value;
            }

            remove
            {
                CPOClient.OnSendHeartbeatResponse -= value;
            }

        }

        #endregion


        #region OnSetChargingPoolAvailabilityStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a charging pool availability status will be send.
        /// </summary>
        public event OnSetChargingPoolAvailabilityStatusRequestDelegate OnSetChargingPoolAvailabilityStatusRequest
        {

            add
            {
                CPOClient.OnSetChargingPoolAvailabilityStatusRequest += value;
            }

            remove
            {
                CPOClient.OnSetChargingPoolAvailabilityStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request sending a charging pool availability status will be send.
        /// </summary>
        public event ClientRequestLogHandler OnSetChargingPoolAvailabilityStatusSOAPRequest
        {

            add
            {
                CPOClient.OnSetChargingPoolAvailabilityStatusSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnSetChargingPoolAvailabilityStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a charging pool availability status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSetChargingPoolAvailabilityStatusSOAPResponse
        {

            add
            {
                CPOClient.OnSetChargingPoolAvailabilityStatusSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnSetChargingPoolAvailabilityStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a charging pool availability status request had been received.
        /// </summary>
        public event OnSetChargingPoolAvailabilityStatusResponseDelegate OnSetChargingPoolAvailabilityStatusResponse
        {

            add
            {
                CPOClient.OnSetChargingPoolAvailabilityStatusResponse += value;
            }

            remove
            {
                CPOClient.OnSetChargingPoolAvailabilityStatusResponse -= value;
            }

        }

        #endregion

        #region OnSetChargingStationAvailabilityStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a charging station availability status will be send.
        /// </summary>
        public event OnSetChargingStationAvailabilityStatusRequestDelegate OnSetChargingStationAvailabilityStatusRequest
        {

            add
            {
                CPOClient.OnSetChargingStationAvailabilityStatusRequest += value;
            }

            remove
            {
                CPOClient.OnSetChargingStationAvailabilityStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request sending a charging station availability status will be send.
        /// </summary>
        public event ClientRequestLogHandler OnSetChargingStationAvailabilityStatusSOAPRequest
        {

            add
            {
                CPOClient.OnSetChargingStationAvailabilityStatusSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnSetChargingStationAvailabilityStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a charging station availability status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSetChargingStationAvailabilityStatusSOAPResponse
        {

            add
            {
                CPOClient.OnSetChargingStationAvailabilityStatusSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnSetChargingStationAvailabilityStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a charging station availability status request had been received.
        /// </summary>
        public event OnSetChargingStationAvailabilityStatusResponseDelegate OnSetChargingStationAvailabilityStatusResponse
        {

            add
            {
                CPOClient.OnSetChargingStationAvailabilityStatusResponse += value;
            }

            remove
            {
                CPOClient.OnSetChargingStationAvailabilityStatusResponse -= value;
            }

        }

        #endregion

        #region OnSetEVSEAvailabilityStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending an EVSE availability status will be send.
        /// </summary>
        public event OnSetEVSEAvailabilityStatusRequestDelegate OnSetEVSEAvailabilityStatusRequest
        {

            add
            {
                CPOClient.OnSetEVSEAvailabilityStatusRequest += value;
            }

            remove
            {
                CPOClient.OnSetEVSEAvailabilityStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request sending an EVSE availability status will be send.
        /// </summary>
        public event ClientRequestLogHandler OnSetEVSEAvailabilityStatusSOAPRequest
        {

            add
            {
                CPOClient.OnSetEVSEAvailabilityStatusSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnSetEVSEAvailabilityStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an EVSE availability status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSetEVSEAvailabilityStatusSOAPResponse
        {

            add
            {
                CPOClient.OnSetEVSEAvailabilityStatusSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnSetEVSEAvailabilityStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an EVSE availability status request had been received.
        /// </summary>
        public event OnSetEVSEAvailabilityStatusResponseDelegate OnSetEVSEAvailabilityStatusResponse
        {

            add
            {
                CPOClient.OnSetEVSEAvailabilityStatusResponse += value;
            }

            remove
            {
                CPOClient.OnSetEVSEAvailabilityStatusResponse -= value;
            }

        }

        #endregion

        #region OnSetChargingConnectorAvailabilityStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a charging connector availability status will be send.
        /// </summary>
        public event OnSetChargingConnectorAvailabilityStatusRequestDelegate OnSetChargingConnectorAvailabilityStatusRequest
        {

            add
            {
                CPOClient.OnSetChargingConnectorAvailabilityStatusRequest += value;
            }

            remove
            {
                CPOClient.OnSetChargingConnectorAvailabilityStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request sending a charging connector availability status will be send.
        /// </summary>
        public event ClientRequestLogHandler OnSetChargingConnectorAvailabilityStatusSOAPRequest
        {

            add
            {
                CPOClient.OnSetChargingConnectorAvailabilityStatusSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnSetChargingConnectorAvailabilityStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a charging connector availability status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSetChargingConnectorAvailabilityStatusSOAPResponse
        {

            add
            {
                CPOClient.OnSetChargingConnectorAvailabilityStatusSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnSetChargingConnectorAvailabilityStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a charging connector availability status request had been received.
        /// </summary>
        public event OnSetChargingConnectorAvailabilityStatusResponseDelegate OnSetChargingConnectorAvailabilityStatusResponse
        {

            add
            {
                CPOClient.OnSetChargingConnectorAvailabilityStatusResponse += value;
            }

            remove
            {
                CPOClient.OnSetChargingConnectorAvailabilityStatusResponse -= value;
            }

        }

        #endregion


        #region OnSetEVSEBusyStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending an EVSE busy status will be send.
        /// </summary>
        public event OnSetEVSEBusyStatusRequestDelegate OnSetEVSEBusyStatusRequest
        {

            add
            {
                CPOClient.OnSetEVSEBusyStatusRequest += value;
            }

            remove
            {
                CPOClient.OnSetEVSEBusyStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request sending an EVSE busy status will be send.
        /// </summary>
        public event ClientRequestLogHandler OnSetEVSEBusyStatusSOAPRequest
        {

            add
            {
                CPOClient.OnSetEVSEBusyStatusSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnSetEVSEBusyStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an EVSE busy status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSetEVSEBusyStatusSOAPResponse
        {

            add
            {
                CPOClient.OnSetEVSEBusyStatusSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnSetEVSEBusyStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an EVSE busy status request had been received.
        /// </summary>
        public event OnSetEVSEBusyStatusResponseDelegate OnSetEVSEBusyStatusResponse
        {

            add
            {
                CPOClient.OnSetEVSEBusyStatusResponse += value;
            }

            remove
            {
                CPOClient.OnSetEVSEBusyStatusResponse -= value;
            }

        }

        #endregion

        #region OnSetEVSESyntheticStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending an EVSE synthetic status will be send.
        /// </summary>
        public event OnSetEVSESyntheticStatusRequestDelegate OnSetEVSESyntheticStatusRequest
        {

            add
            {
                CPOClient.OnSetEVSESyntheticStatusRequest += value;
            }

            remove
            {
                CPOClient.OnSetEVSESyntheticStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request sending an EVSE synthetic status will be send.
        /// </summary>
        public event ClientRequestLogHandler OnSetEVSESyntheticStatusSOAPRequest
        {

            add
            {
                CPOClient.OnSetEVSESyntheticStatusSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnSetEVSESyntheticStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an EVSE synthetic status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSetEVSESyntheticStatusSOAPResponse
        {

            add
            {
                CPOClient.OnSetEVSESyntheticStatusSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnSetEVSESyntheticStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an EVSE synthetic status request had been received.
        /// </summary>
        public event OnSetEVSESyntheticStatusResponseDelegate OnSetEVSESyntheticStatusResponse
        {

            add
            {
                CPOClient.OnSetEVSESyntheticStatusResponse += value;
            }

            remove
            {
                CPOClient.OnSetEVSESyntheticStatusResponse -= value;
            }

        }

        #endregion


        #region OnGetServiceAuthorisationRequest/-Response

        /// <summary>
        /// An event fired whenever a GetServiceAuthorisation request will be send.
        /// </summary>
        public event OnGetServiceAuthorisationRequestDelegate OnGetServiceAuthorisationRequest
        {

            add
            {
                CPOClient.OnGetServiceAuthorisationRequest += value;
            }

            remove
            {
                CPOClient.OnGetServiceAuthorisationRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP GetServiceAuthorisation request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnGetServiceAuthorisationSOAPRequest
        {

            add
            {
                CPOClient.OnGetServiceAuthorisationSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnGetServiceAuthorisationSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a GetServiceAuthorisation SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnGetServiceAuthorisationSOAPResponse
        {

            add
            {
                CPOClient.OnGetServiceAuthorisationSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnGetServiceAuthorisationSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a GetServiceAuthorisation request had been received.
        /// </summary>
        public event OnGetServiceAuthorisationResponseDelegate OnGetServiceAuthorisationResponse
        {

            add
            {
                CPOClient.OnGetServiceAuthorisationResponse += value;
            }

            remove
            {
                CPOClient.OnGetServiceAuthorisationResponse -= value;
            }

        }

        #endregion

        #region OnSetSessionEventReportRequest/-Response

        /// <summary>
        /// An event fired whenever a SetSessionEventReport request will be send.
        /// </summary>
        public event OnSetSessionEventReportRequestDelegate OnSetSessionEventReportRequest
        {

            add
            {
                CPOClient.OnSetSessionEventReportRequest += value;
            }

            remove
            {
                CPOClient.OnSetSessionEventReportRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP SetSessionEventReport request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnSetSessionEventReportSOAPRequest
        {

            add
            {
                CPOClient.OnSetSessionEventReportSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnSetSessionEventReportSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a SetSessionEventReport SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSetSessionEventReportSOAPResponse
        {

            add
            {
                CPOClient.OnSetSessionEventReportSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnSetSessionEventReportSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a SetSessionEventReport request had been received.
        /// </summary>
        public event OnSetSessionEventReportResponseDelegate OnSetSessionEventReportResponse
        {

            add
            {
                CPOClient.OnSetSessionEventReportResponse += value;
            }

            remove
            {
                CPOClient.OnSetSessionEventReportResponse -= value;
            }

        }

        #endregion


        #region OnSetChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record will be send.
        /// </summary>
        public event OnSetChargeDetailRecordRequestDelegate OnSetChargeDetailRecordRequest
        {

            add
            {
                CPOClient.OnSetChargeDetailRecordRequest += value;
            }

            remove
            {
                CPOClient.OnSetChargeDetailRecordRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP charge detail record will be send.
        /// </summary>
        public event ClientRequestLogHandler OnSetChargeDetailRecordSOAPRequest
        {

            add
            {
                CPOClient.OnSetChargeDetailRecordSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnSetChargeDetailRecordSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a SOAP charge detail record had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSetChargeDetailRecordSOAPResponse
        {

            add
            {
                CPOClient.OnSetChargeDetailRecordSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnSetChargeDetailRecordSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a charge detail record had been received.
        /// </summary>
        public event OnSetChargeDetailRecordResponseDelegate OnSetChargeDetailRecordResponse
        {

            add
            {
                CPOClient.OnSetChargeDetailRecordResponse += value;
            }

            remove
            {
                CPOClient.OnSetChargeDetailRecordResponse -= value;
            }

        }

        #endregion


        // CPOServer methods

        #region OnSetServiceAuthorisation

        /// <summary>
        /// An event sent whenever a SetServiceAuthorisation SOAP request was received.
        /// </summary>
        public event RequestLogHandler OnSetServiceAuthorisationSOAPRequest
        {

            add
            {
                CPOServer.OnSetServiceAuthorisationSOAPRequest += value;
            }

            remove
            {
                CPOServer.OnSetServiceAuthorisationSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a SetServiceAuthorisation request was received.
        /// </summary>
        public event OnSetServiceAuthorisationRequestDelegate OnSetServiceAuthorisationRequest
        {

            add
            {
                CPOServer.OnSetServiceAuthorisationRequest += value;
            }

            remove
            {
                CPOServer.OnSetServiceAuthorisationRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a SetServiceAuthorisation request was received.
        /// </summary>
        public event OnSetServiceAuthorisationDelegate OnSetServiceAuthorisation
        {

            add
            {
                CPOServer.OnSetServiceAuthorisation += value;
            }

            remove
            {
                CPOServer.OnSetServiceAuthorisation -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a response to a SetServiceAuthorisation request was sent.
        /// </summary>
        public event OnSetServiceAuthorisationResponseDelegate OnSetServiceAuthorisationResponse
        {

            add
            {
                CPOServer.OnSetServiceAuthorisationResponse += value;
            }

            remove
            {
                CPOServer.OnSetServiceAuthorisationResponse -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a response to a SetServiceAuthorisation SOAP request was sent.
        /// </summary>
        public event AccessLogHandler OnSetServiceAuthorisationSOAPResponse
        {

            add
            {
                CPOServer.OnSetServiceAuthorisationSOAPResponse += value;
            }

            remove
            {
                CPOServer.OnSetServiceAuthorisationSOAPResponse -= value;
            }

        }

        #endregion

        #region OnSetSessionAction

        /// <summary>
        /// An event sent whenever a SetSessionAction SOAP request was received.
        /// </summary>
        public event RequestLogHandler OnSetSessionActionSOAPRequest
        {

            add
            {
                CPOServer.OnSetSessionActionSOAPRequest += value;
            }

            remove
            {
                CPOServer.OnSetSessionActionSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a SetSessionAction request was received.
        /// </summary>
        public event OnSetSessionActionRequestDelegate OnSetSessionActionRequest
        {

            add
            {
                CPOServer.OnSetSessionActionRequest += value;
            }

            remove
            {
                CPOServer.OnSetSessionActionRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a SetSessionAction request was received.
        /// </summary>
        public event OnSetSessionActionDelegate OnSetSessionAction
        {

            add
            {
                CPOServer.OnSetSessionAction += value;
            }

            remove
            {
                CPOServer.OnSetSessionAction -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a response to a SetSessionAction request was sent.
        /// </summary>
        public event OnSetSessionActionResponseDelegate OnSetSessionActionResponse
        {

            add
            {
                CPOServer.OnSetSessionActionResponse += value;
            }

            remove
            {
                CPOServer.OnSetSessionActionResponse -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a response to a SetSessionAction SOAP request was sent.
        /// </summary>
        public event AccessLogHandler OnSetSessionActionSOAPResponse
        {

            add
            {
                CPOServer.OnSetSessionActionSOAPResponse += value;
            }

            remove
            {
                CPOServer.OnSetSessionActionSOAPResponse -= value;
            }

        }

        #endregion


        #region Generic HTTP/SOAP server logging

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog    = new HTTPRequestLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog   = new HTTPResponseLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog      = new HTTPErrorLogEvent();

        #endregion

        #endregion

        #region Custom request mappers

        #region CustomHeartbeatRequestMapper

        #region CustomHeartbeatRequestMapper

        public Func<HeartbeatRequest, HeartbeatRequest> CustomHeartbeatRequestMapper
        {

            get
            {
                return CPOClient.CustomHeartbeatRequestMapper;
            }

            set
            {
                CPOClient.CustomHeartbeatRequestMapper = value;
            }

        }

        #endregion

        #region CustomHeartbeatSOAPRequestMapper

        public Func<HeartbeatRequest, XElement, XElement> CustomHeartbeatSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomHeartbeatSOAPRequestMapper;
            }

            set
            {
                CPOClient.CustomHeartbeatSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<HeartbeatResponse> CustomHeartbeatParser
        {

            get
            {
                return CPOClient.CustomHeartbeatParser;
            }

            set
            {
                CPOClient.CustomHeartbeatParser = value;
            }

        }

        #endregion


        #region CustomSetChargingPoolAvailabilityStatusRequestMapper

        #region CustomSetChargingPoolAvailabilityStatusRequestMapper

        public Func<SetChargingPoolAvailabilityStatusRequest, SetChargingPoolAvailabilityStatusRequest> CustomSetChargingPoolAvailabilityStatusRequestMapper
        {

            get
            {
                return CPOClient.CustomSetChargingPoolAvailabilityStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomSetChargingPoolAvailabilityStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetChargingPoolAvailabilityStatusSOAPRequestMapper

        public Func<SetChargingPoolAvailabilityStatusRequest, XElement, XElement> CustomSetChargingPoolAvailabilityStatusSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomSetChargingPoolAvailabilityStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomSetChargingPoolAvailabilityStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetChargingPoolAvailabilityStatusResponse> CustomSetChargingPoolAvailabilityStatusParser
        {

            get
            {
                return CPOClient.CustomSetChargingPoolAvailabilityStatusParser;
            }

            set
            {
                CPOClient.CustomSetChargingPoolAvailabilityStatusParser = value;
            }

        }

        #endregion

        #region CustomSetChargingStationAvailabilityStatusRequestMapper

        #region CustomSetChargingStationAvailabilityStatusRequestMapper

        public Func<SetChargingStationAvailabilityStatusRequest, SetChargingStationAvailabilityStatusRequest> CustomSetChargingStationAvailabilityStatusRequestMapper
        {

            get
            {
                return CPOClient.CustomSetChargingStationAvailabilityStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomSetChargingStationAvailabilityStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetChargingStationAvailabilityStatusSOAPRequestMapper

        public Func<SetChargingStationAvailabilityStatusRequest, XElement, XElement> CustomSetChargingStationAvailabilityStatusSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomSetChargingStationAvailabilityStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomSetChargingStationAvailabilityStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetChargingStationAvailabilityStatusResponse> CustomSetChargingStationAvailabilityStatusParser
        {

            get
            {
                return CPOClient.CustomSetChargingStationAvailabilityStatusParser;
            }

            set
            {
                CPOClient.CustomSetChargingStationAvailabilityStatusParser = value;
            }

        }

        #endregion

        #region CustomSetEVSEAvailabilityStatusRequestMapper

        #region CustomSetEVSEAvailabilityStatusRequestMapper

        public Func<SetEVSEAvailabilityStatusRequest, SetEVSEAvailabilityStatusRequest> CustomSetEVSEAvailabilityStatusRequestMapper
        {

            get
            {
                return CPOClient.CustomSetEVSEAvailabilityStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomSetEVSEAvailabilityStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetEVSEAvailabilityStatusSOAPRequestMapper

        public Func<SetEVSEAvailabilityStatusRequest, XElement, XElement> CustomSetEVSEAvailabilityStatusSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomSetEVSEAvailabilityStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomSetEVSEAvailabilityStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetEVSEAvailabilityStatusResponse> CustomSetEVSEAvailabilityStatusParser
        {

            get
            {
                return CPOClient.CustomSetEVSEAvailabilityStatusParser;
            }

            set
            {
                CPOClient.CustomSetEVSEAvailabilityStatusParser = value;
            }

        }

        #endregion

        #region CustomSetChargingConnectorAvailabilityStatusRequestMapper

        #region CustomSetChargingConnectorAvailabilityStatusRequestMapper

        public Func<SetChargingConnectorAvailabilityStatusRequest, SetChargingConnectorAvailabilityStatusRequest> CustomSetChargingConnectorAvailabilityStatusRequestMapper
        {

            get
            {
                return CPOClient.CustomSetChargingConnectorAvailabilityStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomSetChargingConnectorAvailabilityStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetChargingConnectorAvailabilityStatusSOAPRequestMapper

        public Func<SetChargingConnectorAvailabilityStatusRequest, XElement, XElement> CustomSetChargingConnectorAvailabilityStatusSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomSetChargingConnectorAvailabilityStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomSetChargingConnectorAvailabilityStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetChargingConnectorAvailabilityStatusResponse> CustomSetChargingConnectorAvailabilityStatusParser
        {

            get
            {
                return CPOClient.CustomSetChargingConnectorAvailabilityStatusParser;
            }

            set
            {
                CPOClient.CustomSetChargingConnectorAvailabilityStatusParser = value;
            }

        }

        #endregion


        #region CustomSetEVSEBusyStatusRequestMapper

        #region CustomSetEVSEBusyStatusRequestMapper

        public Func<SetEVSEBusyStatusRequest, SetEVSEBusyStatusRequest> CustomSetEVSEBusyStatusRequestMapper
        {

            get
            {
                return CPOClient.CustomSetEVSEBusyStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomSetEVSEBusyStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetEVSEBusyStatusSOAPRequestMapper

        public Func<SetEVSEBusyStatusRequest, XElement, XElement> CustomSetEVSEBusyStatusSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomSetEVSEBusyStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomSetEVSEBusyStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetEVSEBusyStatusResponse> CustomSetEVSEBusyStatusParser
        {

            get
            {
                return CPOClient.CustomSetEVSEBusyStatusParser;
            }

            set
            {
                CPOClient.CustomSetEVSEBusyStatusParser = value;
            }

        }

        #endregion


        #region CustomGetServiceAuthorisationRequestMapper

        #region CustomGetServiceAuthorisationRequestMapper

        public Func<GetServiceAuthorisationRequest, GetServiceAuthorisationRequest> CustomGetServiceAuthorisationRequestMapper
        {

            get
            {
                return CPOClient.CustomGetServiceAuthorisationRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomGetServiceAuthorisationRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetServiceAuthorisationSOAPRequestMapper

        public Func<GetServiceAuthorisationRequest, XElement, XElement> CustomGetServiceAuthorisationSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomGetServiceAuthorisationSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomGetServiceAuthorisationSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<GetServiceAuthorisationResponse> CustomGetServiceAuthorisationParser
        {

            get
            {
                return CPOClient.CustomGetServiceAuthorisationParser;
            }

            set
            {
                CPOClient.CustomGetServiceAuthorisationParser = value;
            }

        }

        #endregion


        #region CustomSetChargeDetailRecordRequestMapper

        #region CustomSetChargeDetailRecordRequestMapper

        public Func<SetChargeDetailRecordRequest, SetChargeDetailRecordRequest> CustomSetChargeDetailRecordRequestMapper
        {

            get
            {
                return CPOClient.CustomSetChargeDetailRecordRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomSetChargeDetailRecordRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetChargeDetailRecordSOAPRequestMapper

        public Func<SetChargeDetailRecordRequest, XElement, XElement> CustomSetChargeDetailRecordSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomSetChargeDetailRecordSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    CPOClient.CustomSetChargeDetailRecordSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetChargeDetailRecordResponse> CustomSetChargeDetailRecordParser
        {

            get
            {
                return CPOClient.CustomSetChargeDetailRecordParser;
            }

            set
            {
                CPOClient.CustomSetChargeDetailRecordParser = value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region CPORoaming(CPOClient, CPOServer, ServerLoggingContext = CPOServerLogger.DefaultContext, LogfileCreator = null)

        /// <summary>
        /// Create a new eMIP roaming client for CPOs.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// <param name="CPOServer">A CPO sever.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPORoaming(CPOClient               CPOClient,
                          CPOServer               CPOServer,
                          String                  ServerLoggingContext  = CPOServerLogger.DefaultContext,
                          LogfileCreatorDelegate  LogfileCreator        = null)
        {

            this.CPOClient        = CPOClient;
            this.CPOServer        = CPOServer;

            this.CPOServerLogger  = new CPOServerLogger(CPOServer,
                                                        ServerLoggingContext,
                                                        LogfileCreator);

            // Link HTTP events...
            CPOServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            CPOServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            CPOServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

        }

        #endregion

        #region CPORoaming(ClientId, RemoteHostname, RemoteTCPPort = null, RemoteHTTPVirtualHost = null, ... )

        /// <summary>
        /// Create a new eMIP roaming client for CPOs.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="RemoteHostname">The hostname of the remote eMIP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote eMIP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote eMIP service.</param>
        /// <param name="URLPrefix">An default URI prefix.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURLPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerAuthorisationURL">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPORoaming(String                               ClientId,
                          HTTPHostname                         RemoteHostname,
                          IPPort?                              RemoteTCPPort                                   = null,
                          RemoteCertificateValidationCallback  RemoteCertificateValidator                      = null,
                          LocalCertificateSelectionCallback    ClientCertificateSelector                       = null,
                          HTTPHostname?                        RemoteHTTPVirtualHost                           = null,
                          HTTPPath?                            URLPrefix                                       = null,
                          String                               HTTPUserAgent                                   = CPOClient.DefaultHTTPUserAgent,
                          TimeSpan?                            RequestTimeout                                  = null,
                          TransmissionRetryDelayDelegate       TransmissionRetryDelay                          = null,
                          Byte?                                MaxNumberOfRetries                              = CPOClient.DefaultMaxNumberOfRetries,

                          String                               ServerName                                      = CPOServer.DefaultHTTPServerName,
                          String                               ServiceId                                       = null,
                          IPPort?                              ServerTCPPort                                   = null,
                          HTTPPath?                            ServerURLPrefix                                 = null,
                          String                               ServerAuthorisationURL                          = CPOServer.DefaultAuthorisationURL,
                          HTTPContentType                      ServerContentType                               = null,
                          Boolean                              ServerRegisterHTTPRootService                   = true,
                          Boolean                              ServerAutoStart                                 = false,

                          String                               ClientLoggingContext                            = CPOClient.CPOClientLogger.DefaultContext,
                          String                               ServerLoggingContext                            = CPOServerLogger.DefaultContext,
                          LogfileCreatorDelegate               LogfileCreator                                  = null,

                          CounterValues?                       SendHeartbeatCounter                            = null,
                          CounterValues?                       SetChargingPoolAvailabilityStatusCounter        = null,
                          CounterValues?                       SetChargingStationAvailabilityStatusCounter     = null,
                          CounterValues?                       SetEVSEAvailabilityStatusCounter                = null,
                          CounterValues?                       SetChargingConnectorAvailabilityStatusCounter   = null,
                          CounterValues?                       SetEVSEBusyStatusCounter                        = null,
                          CounterValues?                       SetEVSESyntheticStatusCounter                   = null,
                          CounterValues?                       GetServiceAuthorisationCounter                  = null,
                          CounterValues?                       SetSessionEventReportCounter                    = null,
                          CounterValues?                       SetChargeDetailRecordCounter                    = null,

                          DNSClient                            DNSClient                                       = null)

            : this(new CPOClient(ClientId,
                                 RemoteHostname,
                                 RemoteTCPPort,
                                 RemoteCertificateValidator,
                                 ClientCertificateSelector,
                                 RemoteHTTPVirtualHost,
                                 URLPrefix ?? CPOClient.DefaultURLPrefix,
                                 HTTPUserAgent,
                                 RequestTimeout,
                                 TransmissionRetryDelay,
                                 MaxNumberOfRetries,

                                 SendHeartbeatCounter,
                                 SetChargingPoolAvailabilityStatusCounter,
                                 SetChargingStationAvailabilityStatusCounter,
                                 SetEVSEAvailabilityStatusCounter,
                                 SetChargingConnectorAvailabilityStatusCounter,
                                 SetEVSEBusyStatusCounter,
                                 SetEVSESyntheticStatusCounter,
                                 GetServiceAuthorisationCounter,
                                 SetSessionEventReportCounter,
                                 SetChargeDetailRecordCounter,

                                 DNSClient,
                                 ClientLoggingContext,
                                 LogfileCreator),

                   new CPOServer(ServerName,
                                 ServiceId,
                                 ServerTCPPort,
                                 ServerURLPrefix        ?? CPOServer.DefaultURLPrefix,
                                 ServerAuthorisationURL ?? CPOServer.DefaultAuthorisationURL,
                                 ServerContentType,
                                 ServerRegisterHTTPRootService,
                                 DNSClient,
                                 false),

                   ServerLoggingContext,
                   LogfileCreator)

        {

            if (ServerAutoStart)
                Start();

        }

        #endregion

        #endregion


        #region SendHeartbeat                         (Request)

        /// <summary>
        /// Send the given heartbeat.
        /// </summary>
        /// <param name="Request">A SendHeartbeat request.</param>
        public Task<HTTPResponse<HeartbeatResponse>>

            SendHeartbeat(HeartbeatRequest Request)

                => CPOClient.SendHeartbeat(Request);

        #endregion


        #region SetChargingPoolAvailabilityStatus     (Request)

        /// <summary>
        /// Send the given charging pool availability status.
        /// </summary>
        /// <param name="Request">A SetChargingPoolAvailabilityStatus request.</param>
        public Task<HTTPResponse<SetChargingPoolAvailabilityStatusResponse>>

            SetChargingPoolAvailabilityStatus(SetChargingPoolAvailabilityStatusRequest Request)

                => CPOClient.SetChargingPoolAvailabilityStatus(Request);

        #endregion

        #region SetChargingStationAvailabilityStatus  (Request)

        /// <summary>
        /// Send the given charging station availability status.
        /// </summary>
        /// <param name="Request">A SetChargingStationAvailabilityStatus request.</param>
        public Task<HTTPResponse<SetChargingStationAvailabilityStatusResponse>>

            SetChargingStationAvailabilityStatus(SetChargingStationAvailabilityStatusRequest Request)

                => CPOClient.SetChargingStationAvailabilityStatus(Request);

        #endregion

        #region SetEVSEAvailabilityStatus             (Request)

        /// <summary>
        /// Send the given EVSE availability status.
        /// </summary>
        /// <param name="Request">A SetEVSEAvailabilityStatus request.</param>
        public Task<HTTPResponse<SetEVSEAvailabilityStatusResponse>>

            SetEVSEAvailabilityStatus(SetEVSEAvailabilityStatusRequest Request)

                => CPOClient.SetEVSEAvailabilityStatus(Request);

        #endregion

        #region SetChargingConnectorAvailabilityStatus(Request)

        /// <summary>
        /// Send the given charging connector availability status.
        /// </summary>
        /// <param name="Request">A SetChargingConnectorAvailabilityStatus request.</param>
        public Task<HTTPResponse<SetChargingConnectorAvailabilityStatusResponse>>

            SetChargingConnectorAvailabilityStatus(SetChargingConnectorAvailabilityStatusRequest Request)

                => CPOClient.SetChargingConnectorAvailabilityStatus(Request);

        #endregion


        #region SetEVSEBusyStatus                     (Request)

        /// <summary>
        /// Send the given EVSE busy status.
        /// </summary>
        /// <param name="Request">A SetEVSEBusyStatus request.</param>
        public Task<HTTPResponse<SetEVSEBusyStatusResponse>>

            SetEVSEBusyStatus(SetEVSEBusyStatusRequest Request)

                => CPOClient.SetEVSEBusyStatus(Request);

        #endregion

        #region SetEVSESyntheticStatus                (Request)

        /// <summary>
        /// Send the given EVSE synthetic status.
        /// </summary>
        /// <param name="Request">A SetEVSESyntheticStatus request.</param>
        public Task<HTTPResponse<SetEVSESyntheticStatusResponse>>

            SetEVSESyntheticStatus(SetEVSESyntheticStatusRequest Request)

                => CPOClient.SetEVSESyntheticStatus(Request);

        #endregion


        #region GetServiceAuthorisation               (Request)

        /// <summary>
        /// Request an service authorisation.
        /// </summary>
        /// <param name="Request">A GetServiceAuthorisation request.</param>
        public Task<HTTPResponse<GetServiceAuthorisationResponse>>

            GetServiceAuthorisation(GetServiceAuthorisationRequest Request)

                => CPOClient.GetServiceAuthorisation(Request);

        #endregion

        #region SetSessionEventReport               (Request)

        /// <summary>
        /// Send a session event report.
        /// </summary>
        /// <param name="Request">A SetSessionEventReport request.</param>
        public Task<HTTPResponse<SetSessionEventReportResponse>>

            SetSessionEventReport(SetSessionEventReportRequest Request)

                => CPOClient.SetSessionEventReport(Request);

        #endregion


        #region SetChargeDetailRecord                 (Request)

        /// <summary>
        /// Upload the given charge detail record.
        /// </summary>
        /// <param name="Request">A SetChargeDetailRecord request.</param>
        public Task<HTTPResponse<SetChargeDetailRecordResponse>>

            SetChargeDetailRecord(SetChargeDetailRecordRequest Request)

                => CPOClient.SetChargeDetailRecord(Request);

        #endregion



        #region Start()

        public void Start()
        {
            CPOServer.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String Message = null, Boolean Wait = true)
        {
            CPOServer.Shutdown(Message, Wait);
        }

        #endregion

        public void Dispose()
        { }

    }

}
