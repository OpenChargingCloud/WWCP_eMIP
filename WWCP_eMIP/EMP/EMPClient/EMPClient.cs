﻿/*
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
using System.Linq;
using System.Xml.Linq;
using System.Net.Security;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP.v1_2;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.EMP
{

    /// <summary>
    /// An eMIP EMP client.
    /// </summary>
    public partial class EMPClient : ASOAPClient,
                                     IEMPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String   DefaultHTTPUserAgent        = "GraphDefined eMIP " + Version.Number + " EMP Client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort   DefaultRemotePort           = IPPort.Parse(443);

        /// <summary>
        /// The default URI prefix.
        /// </summary>
        public     static readonly HTTPPath  DefaultURIPrefix            = HTTPPath.Parse("/api/emip");


        /// <summary>
        /// The default SOAP action prefix.
        /// </summary>
        public     const           String   DefaultSOAPActionPrefix     = "https://api-iop.gireve.com/services/";

        #endregion

        #region Properties

        /// <summary>
        /// The attached eMIP EMP client (HTTP/SOAP client) logger.
        /// </summary>
        public EMPClientLogger  Logger   { get; }

        #endregion

        #region Custom request mappers

        #region CustomHeartbeatRequestMapper

        #region CustomHeartbeatRequestMapper

        private Func<HeartbeatRequest, HeartbeatRequest> _CustomHeartbeatRequestMapper = _ => _;

        public Func<HeartbeatRequest, HeartbeatRequest> CustomHeartbeatRequestMapper
        {

            get
            {
                return _CustomHeartbeatRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomHeartbeatRequestMapper = value;
            }

        }

        #endregion

        #region CustomHeartbeatSOAPRequestMapper

        private Func<HeartbeatRequest, XElement, XElement> _CustomHeartbeatSOAPRequestMapper = (request, xml) => xml;

        public Func<HeartbeatRequest, XElement, XElement> CustomHeartbeatSOAPRequestMapper
        {

            get
            {
                return _CustomHeartbeatSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomHeartbeatSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<HeartbeatResponse> CustomHeartbeatParser   { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<HeartbeatRequest>                  CustomHeartbeatRequestSerializer                    { get; set; }

        #endregion

        #region Events

        #region OnHeartbeatRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a heartbeat will be send.
        /// </summary>
        public event OnSendHeartbeatRequestDelegate   OnSendHeartbeatRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a heartbeat will be send.
        /// </summary>
        public event ClientRequestLogHandler          OnSendHeartbeatSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a heartbeat SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler         OnSendHeartbeatSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a heartbeat request had been received.
        /// </summary>
        public event OnSendHeartbeatResponseDelegate  OnSendHeartbeatResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPClient(ClientId, Hostname, ..., LoggingContext = EMPClientLogger.DefaultContext, ...)

        /// <summary>
        /// Create a new eMIP EMP Client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The hostname of the remote eMIP service.</param>
        /// <param name="RemotePort">An optional TCP port of the remote eMIP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote eMIP service.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="LoggingContext">An optional context for logging client methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public EMPClient(String                               ClientId,
                         HTTPHostname                         Hostname,
                         IPPort?                              RemotePort                   = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                         LocalCertificateSelectionCallback    ClientCertificateSelector    = null,
                         HTTPHostname?                        HTTPVirtualHost              = null,
                         HTTPPath?                             URIPrefix                    = null,
                         String                               HTTPUserAgent                = DefaultHTTPUserAgent,
                         TimeSpan?                            RequestTimeout               = null,
                         Byte?                                MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                         DNSClient                            DNSClient                    = null,
                         String                               LoggingContext               = EMPClientLogger.DefaultContext,
                         LogfileCreatorDelegate               LogfileCreator               = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCertificateSelector,
                   HTTPVirtualHost,
                   URIPrefix ?? DefaultURIPrefix,
                   null,
                   HTTPUserAgent,
                   RequestTimeout,
                   MaxNumberOfRetries,
                   DNSClient)

        {

            this.Logger  = new EMPClientLogger(this,
                                               LoggingContext,
                                               LogfileCreator);

        }

        #endregion

        #region EMPClient(ClientId, Logger, Hostname, ...)

        /// <summary>
        /// Create a new eMIP EMP Client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Logger">A EMP client logger.</param>
        /// <param name="Hostname">The hostname of the remote eMIP service.</param>
        /// <param name="RemotePort">An optional TCP port of the remote eMIP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote eMIP service.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public EMPClient(String                               ClientId,
                         EMPClientLogger                      Logger,
                         HTTPHostname                         Hostname,
                         IPPort?                              RemotePort                   = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                         LocalCertificateSelectionCallback    ClientCertificateSelector    = null,
                         HTTPHostname?                        HTTPVirtualHost              = null,
                         HTTPPath?                             URIPrefix                    = null,
                         String                               HTTPUserAgent                = DefaultHTTPUserAgent,
                         TimeSpan?                            RequestTimeout               = null,
                         Byte?                                MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                         DNSClient                            DNSClient                    = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCertificateSelector,
                   HTTPVirtualHost,
                   URIPrefix ?? DefaultURIPrefix,
                   null,
                   HTTPUserAgent,
                   RequestTimeout,
                   MaxNumberOfRetries,
                   DNSClient)

        {

            this.Logger  = Logger ?? throw new ArgumentNullException(nameof(Logger), "The given mobile client logger must not be null!");

        }

        #endregion

        #endregion


        #region SendHeartbeat            (Request)

        /// <summary>
        /// Send the given heartbeat.
        /// </summary>
        /// <param name="Request">A SendHeartbeat request.</param>
        public async Task<HTTPResponse<HeartbeatResponse>>

            SendHeartbeat(HeartbeatRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SendHeartbeat request must not be null!");

            Request = _CustomHeartbeatRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SendHeartbeat request must not be null!");


            Byte                            TransmissionRetry  = 0;
            HTTPResponse<HeartbeatResponse> result             = null;

            #endregion

            #region Send OnHeartbeatRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnSendHeartbeatRequest != null)
                    await Task.WhenAll(OnSendHeartbeatRequest.GetInvocationList().
                                       Cast<OnSendHeartbeatRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.TransactionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnSendHeartbeatRequest));
            }

            #endregion


            do
            {

                using (var _eMIPClient = new SOAPClient(Hostname,
                                                        URIPrefix,
                                                        VirtualHostname,
                                                        RemotePort,
                                                        RemoteCertificateValidator,
                                                        ClientCertificateSelector,
                                                        UserAgent,
                                                        RequestTimeout,
                                                        DNSClient))
                {

                    result = await _eMIPClient.Query(_CustomHeartbeatSOAPRequestMapper(Request,
                                                                                       SOAP.Encapsulation(Request.ToXML(CustomHeartbeatRequestSerializer))),
                                                     DefaultSOAPActionPrefix + "eMIP_ToIOP_HeartBeatV1/",
                                                     RequestLogDelegate:   OnSendHeartbeatSOAPRequest,
                                                     ResponseLogDelegate:  OnSendHeartbeatSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              HeartbeatResponse.Parse(request,
                                                                                                                                      xml,
                                                                                                                                      CustomHeartbeatParser,
                                                                                                                                      onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<HeartbeatResponse>(

                                                                    httpresponse,

                                                                    new HeartbeatResponse(
                                                                        Request,
                                                                        Request.TransactionId ?? Transaction_Id.Zero,
                                                                        RequestStatus.DataError
                                                                        //httpresponse.Content.ToString()
                                                                    ),

                                                                    IsFault: true

                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);


                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)

                                                             return new HTTPResponse<HeartbeatResponse>(httpresponse,
                                                                                                        new HeartbeatResponse(
                                                                                                            Request,
                                                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                                                            RequestStatus.HTTPError
                                                                                                            //httpresponse.HTTPStatusCode.ToString(),
                                                                                                            //httpresponse.HTTPBody.      ToUTF8String()
                                                                                                        ),
                                                                                                        IsFault: true);


                                                         return new HTTPResponse<HeartbeatResponse>(

                                                                    httpresponse,

                                                                    new HeartbeatResponse(
                                                                        Request,
                                                                        Request.TransactionId ?? Transaction_Id.Zero,
                                                                        RequestStatus.SystemError
                                                                        //httpresponse.HTTPStatusCode.ToString(),
                                                                        //httpresponse.HTTPBody.      ToUTF8String()
                                                                    ),

                                                                    IsFault: true

                                                                );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<HeartbeatResponse>.ExceptionThrown(

                                                                new HeartbeatResponse(
                                                                    Request,
                                                                    Request.TransactionId ?? Transaction_Id.Zero,
                                                                    RequestStatus.ServiceNotAvailable
                                                                    //httpresponse.HTTPStatusCode.ToString(),
                                                                    //httpresponse.HTTPBody.      ToUTF8String()
                                                                ),

                                                                Exception: exception

                                                            );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<HeartbeatResponse>.OK(
                                 new HeartbeatResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.SystemError
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnSendHeartbeatResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnSendHeartbeatResponse != null)
                    await Task.WhenAll(OnSendHeartbeatResponse.GetInvocationList().
                                       Cast<OnSendHeartbeatResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.TransactionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnSendHeartbeatResponse));
            }

            #endregion

            return result;

        }

        #endregion


    }

}
