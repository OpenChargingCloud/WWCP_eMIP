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

namespace org.GraphDefined.WWCP.eMIPv0_7_4.EMP
{

    /// <summary>
    /// An eMIP roaming client for EMPs which combines the EMP client
    /// and server and adds additional logging for both.
    /// </summary>
    public class EMPRoaming : IEMPClient
    {

        #region Properties

        /// <summary>
        /// The EMP client.
        /// </summary>
        public EMPClient        EMPClient         { get; }

        public HTTPHostname Hostname
            => EMPClient.Hostname;

        public HTTPHostname? VirtualHostname
            => EMPClient.VirtualHostname;

        public IPPort RemotePort
            => EMPClient.RemotePort;

        public RemoteCertificateValidationCallback RemoteCertificateValidator
            => EMPClient?.RemoteCertificateValidator;

        /// <summary>
        /// The EMP server.
        /// </summary>
        public EMPServer        EMPServer         { get; }

        /// <summary>
        /// The EMP server logger.
        /// </summary>
        public EMPServerLogger  EMPServerLogger   { get; }

        /// <summary>
        /// The default request timeout for this client.
        /// </summary>
        public TimeSpan?        RequestTimeout    { get; }


        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient DNSClient
            => EMPClient.DNSClient;

        #endregion

        #region Events

        // EMPClient logging methods

        #region OnHeartbeatRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a heartbeat will be send.
        /// </summary>
        public event OnSendHeartbeatRequestDelegate OnSendHeartbeatRequest
        {

            add
            {
                EMPClient.OnSendHeartbeatRequest += value;
            }

            remove
            {
                EMPClient.OnSendHeartbeatRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request sending a heartbeat will be send.
        /// </summary>
        public event ClientRequestLogHandler OnSendHeartbeatSOAPRequest
        {

            add
            {
                EMPClient.OnSendHeartbeatSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnSendHeartbeatSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a heartbeat SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSendHeartbeatSOAPResponse
        {

            add
            {
                EMPClient.OnSendHeartbeatSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnSendHeartbeatSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a heartbeat request had been received.
        /// </summary>
        public event OnSendHeartbeatResponseDelegate OnSendHeartbeatResponse
        {

            add
            {
                EMPClient.OnSendHeartbeatResponse += value;
            }

            remove
            {
                EMPClient.OnSendHeartbeatResponse -= value;
            }

        }

        #endregion


        #region OnSetServiceAuthorisationRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a SetServiceAuthorisation will be send.
        /// </summary>
        public event OnSetServiceAuthorisationRequestDelegate   OnSetServiceAuthorisationRequest
        {

            add
            {
                EMPClient.OnSetServiceAuthorisationRequest += value;
            }

            remove
            {
                EMPClient.OnSetServiceAuthorisationRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request sending a SetServiceAuthorisation will be send.
        /// </summary>
        public event ClientRequestLogHandler                    OnSetServiceAuthorisationSOAPRequest
        {

            add
            {
                EMPClient.OnSetServiceAuthorisationSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnSetServiceAuthorisationSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a SetServiceAuthorisation SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                   OnSetServiceAuthorisationSOAPResponse
        {

            add
            {
                EMPClient.OnSetServiceAuthorisationSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnSetServiceAuthorisationSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a SetServiceAuthorisation request had been received.
        /// </summary>
        public event OnSetServiceAuthorisationResponseDelegate  OnSetServiceAuthorisationResponse
        {

            add
            {
                EMPClient.OnSetServiceAuthorisationResponse += value;
            }

            remove
            {
                EMPClient.OnSetServiceAuthorisationResponse -= value;
            }

        }

        #endregion

        #region OnSetSessionActionRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a SetSessionAction will be send.
        /// </summary>
        public event OnSetSessionActionRequestDelegate   OnSetSessionActionRequest
        {

            add
            {
                EMPClient.OnSetSessionActionRequest += value;
            }

            remove
            {
                EMPClient.OnSetSessionActionRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request sending a SetSessionAction will be send.
        /// </summary>
        public event ClientRequestLogHandler             OnSetSessionActionSOAPRequest
        {

            add
            {
                EMPClient.OnSetSessionActionSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnSetSessionActionSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a SetSessionAction SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler            OnSetSessionActionSOAPResponse
        {

            add
            {
                EMPClient.OnSetSessionActionSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnSetSessionActionSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a SetSessionAction request had been received.
        /// </summary>
        public event OnSetSessionActionResponseDelegate  OnSetSessionActionResponse
        {

            add
            {
                EMPClient.OnSetSessionActionResponse += value;
            }

            remove
            {
                EMPClient.OnSetSessionActionResponse -= value;
            }

        }

        #endregion



        // EMPServer methods


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
                return EMPClient.CustomHeartbeatRequestMapper;
            }

            set
            {
                EMPClient.CustomHeartbeatRequestMapper = value;
            }

        }

        #endregion

        #region CustomHeartbeatSOAPRequestMapper

        public Func<HeartbeatRequest, XElement, XElement> CustomHeartbeatSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomHeartbeatSOAPRequestMapper;
            }

            set
            {
                EMPClient.CustomHeartbeatSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<HeartbeatResponse> CustomHeartbeatParser
        {

            get
            {
                return EMPClient.CustomHeartbeatParser;
            }

            set
            {
                EMPClient.CustomHeartbeatParser = value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPRoaming(EMPClient, EMPServer, ServerLoggingContext = EMPServerLogger.DefaultContext, LogfileCreator = null)

        /// <summary>
        /// Create a new eMIP roaming client for EMPs.
        /// </summary>
        /// <param name="EMPClient">A EMP client.</param>
        /// <param name="EMPServer">A EMP sever.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public EMPRoaming(EMPClient               EMPClient,
                          EMPServer               EMPServer,
                          String                  ServerLoggingContext  = EMPServerLogger.DefaultContext,
                          LogfileCreatorDelegate  LogfileCreator        = null)
        {

            this.EMPClient        = EMPClient;
            this.EMPServer        = EMPServer;

            this.EMPServerLogger  = new EMPServerLogger(EMPServer,
                                                        ServerLoggingContext,
                                                        LogfileCreator);

            // Link HTTP events...
            EMPServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            EMPServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            EMPServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

        }

        #endregion

        #region EMPRoaming(ClientId, RemoteHostname, RemoteTCPPort = null, RemoteHTTPVirtualHost = null, ... )

        /// <summary>
        /// Create a new eMIP roaming client for EMPs.
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
        /// <param name="ServiceName">An optional identification for this SOAP service.</param>
        /// <param name="HTTPServerPort">An optional TCP port for the HTTP server.</param>
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
        public EMPRoaming(String                               ClientId,
                          HTTPHostname                         RemoteHostname,
                          IPPort?                              RemoteTCPPort                   = null,
                          RemoteCertificateValidationCallback  RemoteCertificateValidator      = null,
                          LocalCertificateSelectionCallback    ClientCertificateSelector       = null,
                          HTTPHostname?                        RemoteHTTPVirtualHost           = null,
                          HTTPPath?                            URLPrefix                       = null,
                          String                               HTTPUserAgent                   = EMPClient.DefaultHTTPUserAgent,
                          TimeSpan?                            RequestTimeout                  = null,
                          TransmissionRetryDelayDelegate       TransmissionRetryDelay          = null,
                          Byte?                                MaxNumberOfRetries              = EMPClient.DefaultMaxNumberOfRetries,

                          String                               ServerName                      = EMPServer.DefaultHTTPServerName,
                          IPPort?                              HTTPServerPort                  = null,
                          String                               ServiceName                     = null,
                          HTTPPath?                            ServerURLPrefix                 = null,
                          String                               ServerAuthorisationURL          = EMPServer.DefaultAuthorisationURL,
                          HTTPContentType                      ServerContentType               = null,
                          Boolean                              ServerRegisterHTTPRootService   = true,
                          Boolean                              ServerAutoStart                 = false,

                          String                               ClientLoggingContext            = EMPClient.EMPClientLogger.DefaultContext,
                          String                               ServerLoggingContext            = EMPServerLogger.DefaultContext,
                          LogfileCreatorDelegate               LogfileCreator                  = null,

                          DNSClient                            DNSClient                       = null)

            : this(new EMPClient(ClientId,
                                 RemoteHostname,
                                 RemoteTCPPort,
                                 RemoteCertificateValidator,
                                 ClientCertificateSelector,
                                 RemoteHTTPVirtualHost,
                                 URLPrefix              ?? EMPClient.DefaultURLPrefix,
                                 HTTPUserAgent,
                                 RequestTimeout,
                                 TransmissionRetryDelay,
                                 MaxNumberOfRetries,
                                 DNSClient,
                                 ClientLoggingContext,
                                 LogfileCreator),

                   new EMPServer(ServerName,
                                 HTTPServerPort,
                                 ServiceName,
                                 ServerURLPrefix        ?? EMPServer.DefaultURLPrefix,
                                 ServerAuthorisationURL ?? EMPServer.DefaultAuthorisationURL,
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

                => EMPClient.SendHeartbeat(Request);

        #endregion


        #region SetServiceAuthorisation    (Request)

        /// <summary>
        /// Send the given SetServiceAuthorisation request.
        /// </summary>
        /// <param name="Request">A SetServiceAuthorisation request.</param>
        public Task<HTTPResponse<SetServiceAuthorisationResponse>>

            SetServiceAuthorisation(SetServiceAuthorisationRequest Request)

                => EMPClient.SetServiceAuthorisation(Request);

        #endregion

        // ToIOP_SetAuthenticationData

        #region SetSessionActionRequest    (Request)

        /// <summary>
        /// Send the given SetSessionActionRequest request.
        /// </summary>
        /// <param name="Request">A SetSessionActionRequest request.</param>
        public Task<HTTPResponse<SetSessionActionResponse>>

            SetSessionAction(SetSessionActionRequest Request)

                => EMPClient.SetSessionAction(Request);

        #endregion



        #region Start()

        public void Start()
        {
            EMPServer.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String Message = null, Boolean Wait = true)
        {
            EMPServer.Shutdown(Message, Wait);
        }

        #endregion

        public void Dispose()
        { }

    }

}
