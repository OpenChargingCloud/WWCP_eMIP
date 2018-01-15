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

        public IPPort RemotePort
            => CPOClient?.RemotePort;

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
        public event OnSendHeartbeatRequestDelegate   OnSendHeartbeatRequest
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
        public event ClientRequestLogHandler          OnSendHeartbeatSOAPRequest
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
        public event ClientResponseLogHandler         OnSendHeartbeatSOAPResponse
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
        public event OnSendHeartbeatResponseDelegate  OnSendHeartbeatResponse
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
        public event OnSetEVSEAvailabilityStatusRequestDelegate   OnSetEVSEAvailabilityStatusRequest
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
        public event ClientRequestLogHandler              OnSetEVSEAvailabilityStatusSOAPRequest
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
        public event ClientResponseLogHandler             OnSetEVSEAvailabilityStatusSOAPResponse
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
        public event OnSetEVSEAvailabilityStatusResponseDelegate  OnSetEVSEAvailabilityStatusResponse
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

        // CPOServer methods


        // Generic HTTP/SOAP server logging

        #region RequestLog

        /// <summary>
        /// An event called whenever a request came in.
        /// </summary>
        public event RequestLogHandler RequestLog
        {

            add
            {
                CPOServer.RequestLog += value;
            }

            remove
            {
                CPOServer.RequestLog -= value;
            }

        }

        #endregion

        #region AccessLog

        /// <summary>
        /// An event called whenever a request could successfully be processed.
        /// </summary>
        public event AccessLogHandler AccessLog
        {

            add
            {
                CPOServer.AccessLog += value;
            }

            remove
            {
                CPOServer.AccessLog -= value;
            }

        }

        #endregion

        #region ErrorLog

        /// <summary>
        /// An event called whenever a request resulted in an error.
        /// </summary>
        public event ErrorLogHandler ErrorLog
        {

            add
            {
                CPOServer.ErrorLog += value;
            }

            remove
            {
                CPOServer.ErrorLog -= value;
            }

        }

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
                          String                               RemoteHostname,
                          IPPort                               RemoteTCPPort                   = null,
                          RemoteCertificateValidationCallback  RemoteCertificateValidator      = null,
                          LocalCertificateSelectionCallback    ClientCertificateSelector       = null,
                          String                               RemoteHTTPVirtualHost           = null,
                          String                               URIPrefix                       = CPOClient.DefaultURIPrefix,
                          String                               HTTPUserAgent                   = CPOClient.DefaultHTTPUserAgent,
                          TimeSpan?                            RequestTimeout                  = null,
                          Byte?                                MaxNumberOfRetries              = CPOClient.DefaultMaxNumberOfRetries,

                          String                               ServerName                      = CPOServer.DefaultHTTPServerName,
                          String                               ServiceId                       = null,
                          IPPort                               ServerTCPPort                   = null,
                          String                               ServerURIPrefix                 = CPOServer.DefaultURIPrefix,
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
                                 URIPrefix,
                                 HTTPUserAgent,
                                 RequestTimeout,
                                 MaxNumberOfRetries,
                                 DNSClient,
                                 ClientLoggingContext,
                                 LogfileCreator),

                   new CPOServer(ServerName,
                                 ServiceId,
                                 ServerTCPPort,
                                 ServerURIPrefix,
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


        #region SendHeartbeat  (Request)

        /// <summary>
        /// Send the given heartbeat.
        /// </summary>
        /// <param name="Request">A SendHeartbeat request.</param>
        public Task<HTTPResponse<HeartbeatResponse>>

            SendHeartbeat(HeartbeatRequest Request)

                => CPOClient.SendHeartbeat(Request);

        #endregion


        #region SetEVSEAvailabilityStatus(Request)

        /// <summary>
        /// Send the given EVSE Availability status.
        /// </summary>
        /// <param name="Request">A SetEVSEAvailabilityStatus request.</param>
        public Task<HTTPResponse<SetEVSEAvailabilityStatusResponse>>

            SetEVSEAvailabilityStatus(SetEVSEAvailabilityStatusRequest Request)

                => CPOClient.SetEVSEAvailabilityStatus(Request);

        #endregion

        #region SetEVSEBusyStatus(Request)

        /// <summary>
        /// Send the given EVSE busy status.
        /// </summary>
        /// <param name="Request">A SetEVSEBusyStatus request.</param>
        public Task<HTTPResponse<SetEVSEBusyStatusResponse>>

            SetEVSEBusyStatus(SetEVSEBusyStatusRequest Request)

                => CPOClient.SetEVSEBusyStatus(Request);

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
