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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.EMP
{

    /// <summary>
    /// An eMIP EMP HTTP/SOAP/XML Server API.
    /// </summary>
    public class EMPServer : ASOAPServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public new const           String           DefaultHTTPServerName      = "GraphDefined eMIP " + Version.Number + " HTTP/SOAP/XML EMP API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort      = IPPort.Parse(2002);

        /// <summary>
        /// The default HTTP/SOAP/XML server URI prefix.
        /// </summary>
        public new static readonly HTTPPath         DefaultURIPrefix           = HTTPPath.Parse("/");

        /// <summary>
        /// The default HTTP/SOAP/XML URI for eMIP authorization requests.
        /// </summary>
        public     const           String           DefaultAuthorisationURI    = "";

        /// <summary>
        /// The default HTTP/SOAP/XML content type.
        /// </summary>
        public new static readonly HTTPContentType  DefaultContentType         = HTTPContentType.XMLTEXT_UTF8;

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public new static readonly TimeSpan         DefaultRequestTimeout      = TimeSpan.FromMinutes(1);

        #endregion

        #region Properties

        /// <summary>
        /// The identification of this HTTP/SOAP service.
        /// </summary>
        public String  ServiceId           { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for eMIP authorization requests.
        /// </summary>
        public String  AuthorisationURI    { get; }

        #endregion

        #region Custom request/response mappers

        public CustomXMLParserDelegate<GetServiceAuthorisationRequest>       CustomGetServiceAuthorisationRequestParser        { get; set; }

        public CustomXMLSerializerDelegate<GetServiceAuthorisationResponse>  CustomGetServiceAuthorisationResponseSerializer   { get; set; }


        public OnExceptionDelegate                                           OnException                                       { get; set; }

        #endregion

        #region Events

        #region OnGetServiceAuthorisation

        /// <summary>
        /// An event sent whenever a GetServiceAuthorisation SOAP request was received.
        /// </summary>
        public event RequestLogHandler                           OnGetServiceAuthorisationSOAPRequest;

        /// <summary>
        /// An event sent whenever a GetServiceAuthorisation request was received.
        /// </summary>
        public event OnGetServiceAuthorisationRequestDelegate    OnGetServiceAuthorisationRequest;

        /// <summary>
        /// An event sent whenever a GetServiceAuthorisation request was received.
        /// </summary>
        public event OnGetServiceAuthorisationDelegate           OnGetServiceAuthorisation;

        /// <summary>
        /// An event sent whenever a response to a GetServiceAuthorisation request was sent.
        /// </summary>
        public event OnGetServiceAuthorisationResponseDelegate   OnGetServiceAuthorisationResponse;

        /// <summary>
        /// An event sent whenever a response to a GetServiceAuthorisation SOAP request was sent.
        /// </summary>
        public event AccessLogHandler                            OnGetServiceAuthorisationSOAPResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPServer(HTTPServerName, ServiceId = null, TCPPort = default, URIPrefix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the eMIP HTTP/SOAP/XML EMP API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="AuthorisationURI">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public EMPServer(String           HTTPServerName            = DefaultHTTPServerName,
                         String           ServiceId                 = null,
                         IPPort?          TCPPort                   = null,
                         HTTPPath?        URIPrefix                 = null,
                         String           AuthorisationURI          = DefaultAuthorisationURI,
                         HTTPContentType  ContentType               = null,
                         Boolean          RegisterHTTPRootService   = true,
                         DNSClient        DNSClient                 = null,
                         Boolean          AutoStart                 = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort     ?? DefaultHTTPServerPort,
                   URIPrefix   ?? DefaultURIPrefix,
                   ContentType ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            this.ServiceId         = ServiceId        ?? nameof(EMPServer);
            this.AuthorisationURI  = AuthorisationURI ?? DefaultAuthorisationURI;

            RegisterURITemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region EMPServer(SOAPServer, ServiceId = null, URIPrefix = default)

        /// <summary>
        /// Use the given SOAP server for the eMIP HTTP/SOAP/XML EMP API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="AuthorisationURI">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
        public EMPServer(SOAPServer  SOAPServer,
                         String      ServiceId         = null,
                         HTTPPath?   URIPrefix         = null,
                         String      AuthorisationURI  = DefaultAuthorisationURI)

            : base(SOAPServer,
                   URIPrefix ?? DefaultURIPrefix)

        {

            this.ServiceId         = ServiceId        ?? nameof(EMPServer);
            this.AuthorisationURI  = AuthorisationURI ?? DefaultAuthorisationURI;

            RegisterURITemplates();

        }

        #endregion

        #endregion


        #region (override) RegisterURITemplates()

        /// <summary>
        /// Register all URI templates for this SOAP API.
        /// </summary>
        protected void RegisterURITemplates()
        {

            #region ~/ - GetServiceAuthorisation

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + AuthorisationURI,
                                            "GetServiceAuthorisationRequest",
                                            XML => XML.Descendants(eMIPNS.Authorisation + "eMIP_FromIOP_GetServiceAuthorisationRequest").FirstOrDefault(),
                                            async (HTTPRequest, GetServiceAuthorisationXML) => {


                GetServiceAuthorisationResponse Response  = null;

                #region Send OnGetServiceAuthorisationSOAPRequest event

                var StartTime = DateTime.UtcNow;

                try
                {

                    if (OnGetServiceAuthorisationSOAPRequest != null)
                        await Task.WhenAll(OnGetServiceAuthorisationSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnGetServiceAuthorisationSOAPRequest));
                }

                #endregion


                if (GetServiceAuthorisationRequest.TryParse(GetServiceAuthorisationXML,
                                                            CustomGetServiceAuthorisationRequestParser,
                                                            out GetServiceAuthorisationRequest _GetServiceAuthorisationRequest,
                                                            OnException,

                                                            HTTPRequest.Timestamp,
                                                            HTTPRequest.CancellationToken,
                                                            HTTPRequest.EventTrackingId,
                                                            HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnGetServiceAuthorisationRequest event

                    try
                    {

                        if (OnGetServiceAuthorisationRequest != null)
                            await Task.WhenAll(OnGetServiceAuthorisationRequest.GetInvocationList().
                                               Cast<OnGetServiceAuthorisationRequestDelegate>().
                                               Select(e => e(StartTime,
                                                              _GetServiceAuthorisationRequest.Timestamp.Value,
                                                              this,
                                                              ServiceId,
                                                              _GetServiceAuthorisationRequest.EventTrackingId,
                                                              _GetServiceAuthorisationRequest.TransactionId.Value,
                                                              _GetServiceAuthorisationRequest.PartnerId,
                                                              _GetServiceAuthorisationRequest.OperatorId,
                                                              _GetServiceAuthorisationRequest.TargetOperatorId,
                                                              _GetServiceAuthorisationRequest.EVSEId,
                                                              _GetServiceAuthorisationRequest.UserId,
                                                              _GetServiceAuthorisationRequest.RequestedServiceId,
                                                              _GetServiceAuthorisationRequest.ServiceSessionId,
                                                              _GetServiceAuthorisationRequest.BookingId,

                                                              _GetServiceAuthorisationRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(EMPServer) + "." + nameof(OnGetServiceAuthorisationRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnGetServiceAuthorisation != null)
                    {

                        var results = await Task.WhenAll(OnGetServiceAuthorisation.GetInvocationList().
                                                             Cast<OnGetServiceAuthorisationDelegate>().
                                                             Select(e => e(DateTime.UtcNow,
                                                                           this,
                                                                           _GetServiceAuthorisationRequest))).
                                                             ConfigureAwait(false);

                        Response = results.FirstOrDefault();

                    }

                    //if (Response == null)
                    //    Response = Response<EMP.GetServiceAuthorisationRequest>.SystemError(
                    //                         _GetServiceAuthorisationRequest,
                    //                         "Could not process the incoming GetServiceAuthorisation request!",
                    //                         null,
                    //                         _GetServiceAuthorisationRequest.SessionId,
                    //                         _GetServiceAuthorisationRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnGetServiceAuthorisationResponse event

                    var EndTime = DateTime.UtcNow;

                    try
                    {

                        if (OnGetServiceAuthorisationResponse != null)
                            await Task.WhenAll(OnGetServiceAuthorisationResponse.GetInvocationList().
                                               Cast<OnGetServiceAuthorisationResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             _GetServiceAuthorisationRequest.EventTrackingId,
                                                             _GetServiceAuthorisationRequest.TransactionId.Value,
                                                             _GetServiceAuthorisationRequest.PartnerId,
                                                             _GetServiceAuthorisationRequest.OperatorId,
                                                             _GetServiceAuthorisationRequest.TargetOperatorId,
                                                             _GetServiceAuthorisationRequest.EVSEId,
                                                             _GetServiceAuthorisationRequest.UserId,
                                                             _GetServiceAuthorisationRequest.RequestedServiceId,
                                                             _GetServiceAuthorisationRequest.ServiceSessionId,
                                                             _GetServiceAuthorisationRequest.BookingId,
                                                             _GetServiceAuthorisationRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Response,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(EMPServer) + "." + nameof(OnGetServiceAuthorisationResponse));
                    }

                    #endregion

                }

                //else
                //    Response = Response<EMP.GetServiceAuthorisationRequest>.DataError(
                //                          _GetServiceAuthorisationRequest,
                //                          "Could not process the incoming GetServiceAuthorisation request!"
                //                      );


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Response.ToXML(CustomGetServiceAuthorisationResponseSerializer)).ToUTF8Bytes(),
                    Connection      = "close"
                };

                #endregion

                #region Send OnGetServiceAuthorisationSOAPResponse event

                try
                {

                    if (OnGetServiceAuthorisationSOAPResponse != null)
                        await Task.WhenAll(OnGetServiceAuthorisationSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnGetServiceAuthorisationSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

        }

        #endregion


    }

}