/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Xml.Linq;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
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
        public CPOClient  CPOClient    { get; }

        #region ICPOClient

        /// <summary>
        /// The remote URL of the OICP HTTP endpoint to connect to.
        /// </summary>
        URL                                  IHTTPClient.RemoteURL
            => CPOClient.RemoteURL;

        /// <summary>
        /// The virtual HTTP hostname to connect to.
        /// </summary>
        HTTPHostname?                        IHTTPClient.VirtualHostname
            => CPOClient.VirtualHostname;

        /// <summary>
        /// An optional description of this CPO client.
        /// </summary>
        I18NString                           IHTTPClient.Description
        {

            get
            {
                return CPOClient.Description;
            }

            //set
            //{
            //    CPOClient.Description = value;
            //}

        }

        /// <summary>
        /// The remote TLS certificate validator.
        /// </summary>
        RemoteTLSServerCertificateValidationHandler<IHTTPClient>?  IHTTPClient.RemoteCertificateValidator
            => CPOClient.RemoteCertificateValidator;

        /// <summary>
        /// Multiple optional TLS client certificates to use for HTTP authentication (not a chain of certificates!).
        /// </summary>
        IEnumerable<X509Certificate2>                              IHTTPClient.ClientCertificates
            => CPOClient.ClientCertificates;

        /// <summary>
        /// The optionalTLS client certificate context to use for HTTP authentication.
        /// </summary>
        SslStreamCertificateContext?                               IHTTPClient.ClientCertificateContext
            => CPOClient.ClientCertificateContext;

        /// <summary>
        /// The optional TLS client certificate chain to use for HTTP authentication.
        /// </summary>
        IEnumerable<X509Certificate2>                              IHTTPClient.ClientCertificateChain
            => CPOClient.ClientCertificateChain;

        /// <summary>
        /// The TLS protocol to use.
        /// </summary>
        SslProtocols                         IHTTPClient.TLSProtocols
            => CPOClient.TLSProtocols;

        /// <summary>
        /// Prefer IPv4 instead of IPv6.
        /// </summary>
        Boolean                              IHTTPClient.PreferIPv4
            => CPOClient.PreferIPv4;

        /// <summary>
        /// The optional HTTP connection type.
        /// </summary>
        ConnectionType?                      IHTTPClient.Connection
            => CPOClient.Connection;

        /// <summary>
        /// The optional HTTP content type.
        /// </summary>
        HTTPContentType?                     IHTTPClient.ContentType
            => CPOClient.ContentType;

        /// <summary>
        /// The optional HTTP accept header.
        /// </summary>
        AcceptTypes?                         IHTTPClient.Accept
            => CPOClient.Accept;

        /// <summary>
        /// The optional HTTP authentication to use.
        /// </summary>
        IHTTPAuthentication?                 IHTTPClient.HTTPAuthentication
        {
            get
            {
                return CPOClient.HTTPAuthentication;
            }
            set
            {
                CPOClient.HTTPAuthentication = value;
            }
        }

        /// <summary>
        /// The HTTP user agent identification.
        /// </summary>
        String                               IHTTPClient.HTTPUserAgent
            => CPOClient.HTTPUserAgent;

        /// <summary>
        /// The timeout for upstream requests.
        /// </summary>
        TimeSpan                             IHTTPClient.RequestTimeout
        {

            get
            {
                return CPOClient.RequestTimeout;
            }

            set
            {
                CPOClient.RequestTimeout = value;
            }

        }

        /// <summary>
        /// The delay between transmission retries.
        /// </summary>
        TransmissionRetryDelayDelegate       IHTTPClient.TransmissionRetryDelay
            => CPOClient.TransmissionRetryDelay;

        /// <summary>
        /// The maximum number of retries when communicating with the remote OICP service.
        /// </summary>
        UInt16                               IHTTPClient.MaxNumberOfRetries
            => CPOClient.MaxNumberOfRetries;

        /// <summary>
        /// Make use of HTTP pipelining.
        /// </summary>
        Boolean                              IHTTPClient.UseHTTPPipelining
            => CPOClient.UseHTTPPipelining;

        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        HTTPClientLogger                     IHTTPClient.HTTPLogger
            => CPOClient.HTTPLogger;

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        IDNSClient                           IHTTPClient.DNSClient
            => CPOClient.DNSClient;

        Boolean                              IHTTPClient.Connected
            => CPOClient.Connected;

        IIPAddress?                          IHTTPClient.RemoteIPAddress
            => CPOClient.RemoteIPAddress;

        UInt64                               IHTTPClient.KeepAliveMessageCount
            => CPOClient.KeepAliveMessageCount;

        #endregion


        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServer  CPOServer    { get; }

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
        public event HTTPRequestLogHandlerX OnSetServiceAuthorisationSOAPRequest
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
        public event HTTPResponseLogHandlerX OnSetServiceAuthorisationSOAPResponse
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
        public event HTTPRequestLogHandlerX OnSetSessionActionSOAPRequest
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
        public event HTTPResponseLogHandlerX OnSetSessionActionSOAPResponse
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
                if (value is not null)
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
                if (value is not null)
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
                if (value is not null)
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
                if (value is not null)
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
                if (value is not null)
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
                if (value is not null)
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
                if (value is not null)
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
                if (value is not null)
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
                if (value is not null)
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
                if (value is not null)
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
                if (value is not null)
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
                if (value is not null)
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
                if (value is not null)
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
                if (value is not null)
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

        public X509Certificate2? ClientCertificate => throw new NotImplementedException();

        public IHTTPAuthentication? HTTPAuthentication { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TOTPConfig? TOTPConfig { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new eMIP roaming client for CPOs.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// <param name="CPOServer">A CPO server.</param>
        public CPORoaming(CPOClient  CPOClient,
                          CPOServer  CPOServer)
        {

            this.CPOClient  = CPOClient;
            this.CPOServer  = CPOServer;

            // Link HTTP events...
            CPOServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            CPOServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            CPOServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

        }

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


        #region Start    (EventTrackingId = null)

        ///// <summary>
        ///// Start this HTTP API.
        ///// </summary>
        ///// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        //public async Task<Boolean> Start(EventTracking_Id? EventTrackingId = null)
        //{

        //    var result = await CPOServer.Start(
        //                           EventTrackingId ?? EventTracking_Id.New
        //                       );

        //    //SendStarted(this, CurrentTimestamp);

        //    return result;

        //}

        #endregion

        #region Shutdown (EventTrackingId = null, Message = null, Wait = true)

        ///// <summary>
        ///// Shutdown this HTTP API.
        ///// </summary>
        ///// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        ///// <param name="Message">An optional shutdown message.</param>
        ///// <param name="Wait">Whether to wait for the shutdown to complete.</param>
        //public async virtual Task<Boolean> Shutdown(EventTracking_Id?  EventTrackingId   = null,
        //                                            String?            Message           = null,
        //                                            Boolean            Wait              = true)
        //{

        //    var result = await CPOServer.Shutdown(
        //                           EventTrackingId ?? EventTracking_Id.New,
        //                           Message,
        //                           Wait
        //                       );

        //    //SendShutdown(this, CurrentTimestamp);

        //    return result;

        //}

        #endregion

        #region Dispose()

        public virtual void Dispose()
        {
            CPOServer.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

    }

}
