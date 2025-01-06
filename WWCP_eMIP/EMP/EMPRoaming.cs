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
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.EMP
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
        public EMPClient  EMPClient    { get; }

        #region IEMPClient

        /// <summary>
        /// The remote URL of the OICP HTTP endpoint to connect to.
        /// </summary>
        URL                                  IHTTPClient.RemoteURL
            => EMPClient.RemoteURL;

        /// <summary>
        /// The virtual HTTP hostname to connect to.
        /// </summary>
        HTTPHostname?                        IHTTPClient.VirtualHostname
            => EMPClient.VirtualHostname;

        /// <summary>
        /// An optional description of this CPO client.
        /// </summary>
        I18NString                           IHTTPClient.Description
        {

            get
            {
                return EMPClient.Description;
            }

            set
            {
                EMPClient.Description = value;
            }

        }

        /// <summary>
        /// The remote TLS certificate validator.
        /// </summary>
        RemoteTLSServerCertificateValidationHandler<IHTTPClient>?  IHTTPClient.RemoteCertificateValidator
            => EMPClient.RemoteCertificateValidator;

        /// <summary>
        /// The TLS client certificate to use of HTTP authentication.
        /// </summary>
        X509Certificate?                     IHTTPClient.ClientCert
            => EMPClient.ClientCert;

        /// <summary>
        /// The TLS protocol to use.
        /// </summary>
        SslProtocols                         IHTTPClient.TLSProtocol
            => EMPClient.TLSProtocol;

        /// <summary>
        /// Prefer IPv4 instead of IPv6.
        /// </summary>
        Boolean                              IHTTPClient.PreferIPv4
            => EMPClient.PreferIPv4;

        /// <summary>
        /// The optional HTTP connection type.
        /// </summary>
        ConnectionType?                      IHTTPClient.Connection
            => EMPClient.Connection;

        /// <summary>
        /// The optional HTTP content type.
        /// </summary>
        HTTPContentType?                     IHTTPClient.ContentType
            => EMPClient.ContentType;

        /// <summary>
        /// The optional HTTP accept header.
        /// </summary>
        AcceptTypes?                         IHTTPClient.Accept
            => EMPClient.Accept;

        /// <summary>
        /// The optional HTTP authentication to use.
        /// </summary>
        IHTTPAuthentication?                 IHTTPClient.Authentication
            => EMPClient.Authentication;

        /// <summary>
        /// The HTTP user agent identification.
        /// </summary>
        String                               IHTTPClient.HTTPUserAgent
            => EMPClient.HTTPUserAgent;

        /// <summary>
        /// The timeout for upstream requests.
        /// </summary>
        TimeSpan                             IHTTPClient.RequestTimeout
        {

            get
            {
                return EMPClient.RequestTimeout;
            }

            set
            {
                EMPClient.RequestTimeout = value;
            }

        }

        /// <summary>
        /// The delay between transmission retries.
        /// </summary>
        TransmissionRetryDelayDelegate       IHTTPClient.TransmissionRetryDelay
            => EMPClient.TransmissionRetryDelay;

        /// <summary>
        /// The maximum number of retries when communicationg with the remote OICP service.
        /// </summary>
        UInt16                               IHTTPClient.MaxNumberOfRetries
            => EMPClient.MaxNumberOfRetries;

        /// <summary>
        /// Make use of HTTP pipelining.
        /// </summary>
        Boolean                              IHTTPClient.UseHTTPPipelining
            => EMPClient.UseHTTPPipelining;

        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        HTTPClientLogger                     IHTTPClient.HTTPLogger
            => EMPClient.HTTPLogger;

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        DNSClient                            IHTTPClient.DNSClient
            => EMPClient.DNSClient;

        Boolean IHTTPClient.Connected
            => EMPClient.Connected;

        IIPAddress? IHTTPClient.RemoteIPAddress
            => EMPClient.RemoteIPAddress;

        #endregion


        /// <summary>
        /// The EMP server.
        /// </summary>
        public EMPServer  EMPServer    { get; }

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

        /// <summary>
        /// Create a new eMIP roaming client for EMPs.
        /// </summary>
        /// <param name="EMPClient">A EMP client.</param>
        /// <param name="EMPServer">A EMP sever.</param>
        public EMPRoaming(EMPClient  EMPClient,
                          EMPServer  EMPServer)
        {

            this.EMPClient        = EMPClient;
            this.EMPServer        = EMPServer;

            // Link HTTP events...
            EMPServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            EMPServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            EMPServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

        }

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


        #region Start    (EventTrackingId = null)

        /// <summary>
        /// Start this HTTP API.
        /// </summary>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        public async Task<Boolean> Start(EventTracking_Id? EventTrackingId = null)
        {

            var result = await EMPServer.Start(
                                   EventTrackingId ?? EventTracking_Id.New
                               );

            //SendStarted(this, CurrentTimestamp);

            return result;

        }

        #endregion

        #region Shutdown (EventTrackingId = null, Message = null, Wait = true)

        /// <summary>
        /// Shutdown this HTTP API.
        /// </summary>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="Message">An optional shutdown message.</param>
        /// <param name="Wait">Whether to wait for the shutdown to complete.</param>
        public async virtual Task<Boolean> Shutdown(EventTracking_Id?  EventTrackingId   = null,
                                                    String?            Message           = null,
                                                    Boolean            Wait              = true)
        {

            var result = await EMPServer.Shutdown(
                                   EventTrackingId ?? EventTracking_Id.New,
                                   Message,
                                   Wait
                               );

            //SendShutdown(this, CurrentTimestamp);

            return result;

        }

        #endregion

        #region Dispose()

        public virtual void Dispose()
        {
            EMPServer.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

    }

}
