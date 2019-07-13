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


        #region OnGetServiceAuthorisationRequest/-Response

        /// <summary>
        /// An event fired whenever a service authorisation request will be send.
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
        /// An event fired whenever a SOAP service authorisation request will be send.
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
        /// An event fired whenever a response to a service authorisation SOAP request had been received.
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
        /// An event fired whenever a response to a service authorisation request had been received.
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
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerAuthorisationURI">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
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
                          IPPort?                              RemoteTCPPort                   = null,
                          RemoteCertificateValidationCallback  RemoteCertificateValidator      = null,
                          LocalCertificateSelectionCallback    ClientCertificateSelector       = null,
                          HTTPHostname?                        RemoteHTTPVirtualHost           = null,
                          HTTPPath?                            URIPrefix                       = null,
                          String                               HTTPUserAgent                   = CPOClient.DefaultHTTPUserAgent,
                          TimeSpan?                            RequestTimeout                  = null,
                          Byte?                                MaxNumberOfRetries              = CPOClient.DefaultMaxNumberOfRetries,

                          String                               ServerName                      = CPOServer.DefaultHTTPServerName,
                          String                               ServiceId                       = null,
                          IPPort?                              ServerTCPPort                   = null,
                          HTTPPath?                            ServerURIPrefix                 = null,
                          String                               ServerAuthorisationURI          = CPOServer.DefaultAuthorisationURI,
                          HTTPContentType                      ServerContentType               = null,
                          Boolean                              ServerRegisterHTTPRootService   = true,
                          Boolean                              ServerAutoStart                 = false,

                          String                               ClientLoggingContext            = CPOClient.CPOClientLogger.DefaultContext,
                          String                               ServerLoggingContext            = CPOServerLogger.DefaultContext,
                          LogfileCreatorDelegate               LogfileCreator                  = null,

                          DNSClient                            DNSClient                       = null)

            : this(new CPOClient(ClientId,
                                 RemoteHostname,
                                 RemoteTCPPort,
                                 RemoteCertificateValidator,
                                 ClientCertificateSelector,
                                 RemoteHTTPVirtualHost,
                                 URIPrefix ?? CPOClient.DefaultURIPrefix,
                                 HTTPUserAgent,
                                 RequestTimeout,
                                 MaxNumberOfRetries,
                                 DNSClient,
                                 ClientLoggingContext,
                                 LogfileCreator),

                   new CPOServer(ServerName,
                                 ServiceId,
                                 ServerTCPPort,
                                 ServerURIPrefix        ?? CPOServer.DefaultURIPrefix,
                                 ServerAuthorisationURI ?? CPOServer.DefaultAuthorisationURI,
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
