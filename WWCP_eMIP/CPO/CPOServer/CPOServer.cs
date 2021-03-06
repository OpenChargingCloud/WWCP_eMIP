﻿/*
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
using System.Linq;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// An eMIP CPO HTTP/SOAP/XML Server API.
    /// </summary>
    public class CPOServer : ASOAPServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public new const           String           DefaultHTTPServerName      = "GraphDefined eMIP " + Version.Number + " HTTP/SOAP/XML CPO API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort      = IPPort.Parse(2002);

        /// <summary>
        /// The default HTTP/SOAP/XML server URI prefix.
        /// </summary>
        public new static readonly HTTPPath         DefaultURLPathPrefix           = HTTPPath.Parse("/");

        /// <summary>
        /// The default HTTP/SOAP/XML URI for eMIP authorization requests.
        /// </summary>
        public     const           String           DefaultAuthorisationURL    = "";

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
        public String  ServiceName           { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for eMIP authorization requests.
        /// </summary>
        public String  AuthorisationURL    { get; }

        #endregion

        #region Custom request/response mappers

        public CustomXMLParserDelegate<SetServiceAuthorisationRequest>       CustomSetServiceAuthorisationRequestParser        { get; set; }

        public CustomXMLParserDelegate<MeterReport>                          CustomMeterReportParser                           { get; set; }

        public CustomXMLSerializerDelegate<SetServiceAuthorisationResponse>  CustomSetServiceAuthorisationResponseSerializer   { get; set; }


        public CustomXMLParserDelegate<SetSessionActionRequestRequest>              CustomSetSessionActionRequestParser               { get; set; }

        public CustomXMLParserDelegate<SessionAction>                        CustomSessionActionParser                         { get; set; }

        public CustomXMLSerializerDelegate<SetSessionActionRequestResponse>         CustomSetSessionActionResponseSerializer          { get; set; }


        public OnExceptionDelegate                                           OnException                                       { get; set; }

        #endregion

        #region Events

        #region OnSetServiceAuthorisation

        /// <summary>
        /// An event sent whenever a SetServiceAuthorisation SOAP request was received.
        /// </summary>
        public event RequestLogHandler                          OnSetServiceAuthorisationSOAPRequest;

        /// <summary>
        /// An event sent whenever a SetServiceAuthorisation request was received.
        /// </summary>
        public event OnSetServiceAuthorisationRequestDelegate   OnSetServiceAuthorisationRequest;

        /// <summary>
        /// An event sent whenever a SetServiceAuthorisation request was received.
        /// </summary>
        public event OnSetServiceAuthorisationDelegate          OnSetServiceAuthorisation;

        /// <summary>
        /// An event sent whenever a response to a SetServiceAuthorisation request was sent.
        /// </summary>
        public event OnSetServiceAuthorisationResponseDelegate  OnSetServiceAuthorisationResponse;

        /// <summary>
        /// An event sent whenever a response to a SetServiceAuthorisation SOAP request was sent.
        /// </summary>
        public event AccessLogHandler                           OnSetServiceAuthorisationSOAPResponse;

        #endregion

        #region OnSetSessionAction

        /// <summary>
        /// An event sent whenever a SetSessionAction SOAP request was received.
        /// </summary>
        public event RequestLogHandler                   OnSetSessionActionSOAPRequest;

        /// <summary>
        /// An event sent whenever a SetSessionAction request was received.
        /// </summary>
        public event OnSetSessionActionRequestDelegate   OnSetSessionActionRequest;

        /// <summary>
        /// An event sent whenever a SetSessionAction request was received.
        /// </summary>
        public event OnSetSessionActionDelegate          OnSetSessionAction;

        /// <summary>
        /// An event sent whenever a response to a SetSessionAction request was sent.
        /// </summary>
        public event OnSetSessionActionResponseDelegate  OnSetSessionActionResponse;

        /// <summary>
        /// An event sent whenever a response to a SetSessionAction SOAP request was sent.
        /// </summary>
        public event AccessLogHandler                    OnSetSessionActionSOAPResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region CPOServer(HTTPServerName, ServiceName = null, TCPPort = default, URLPrefix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the eMIP HTTP/SOAP/XML CPO API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceName">An optional identification for this SOAP service.</param>
        /// <param name="HTTPServerPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URLPathPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="AuthorisationURL">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public CPOServer(String           HTTPServerName            = DefaultHTTPServerName,
                         IPPort?          HTTPServerPort            = null,
                         String           ServiceName               = null,
                         HTTPPath?        URLPathPrefix             = null,
                         String           AuthorisationURL          = DefaultAuthorisationURL,
                         HTTPContentType  ContentType               = null,
                         Boolean          RegisterHTTPRootService   = true,
                         DNSClient        DNSClient                 = null,
                         Boolean          AutoStart                 = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   HTTPServerPort ?? DefaultHTTPServerPort,
                   ServiceName    ?? "eMIP " + Version.Number + " " + nameof(CPOServer),
                   URLPathPrefix  ?? DefaultURLPathPrefix,
                   ContentType    ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            this.ServiceName       = ServiceName      ?? "eMIP " + Version.Number + " " + nameof(CPOServer);
            this.AuthorisationURL  = AuthorisationURL ?? DefaultAuthorisationURL;

            RegisterURITemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region CPOServer(SOAPServer, ServiceName = null, URLPathPrefix = default)

        /// <summary>
        /// Use the given SOAP server for the eMIP HTTP/SOAP/XML CPO API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="ServiceName">An optional identification for this SOAP service.</param>
        /// <param name="URLPathPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="AuthorisationURL">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
        public CPOServer(SOAPServer  SOAPServer,
                         String      ServiceName       = null,
                         HTTPPath?   URLPathPrefix     = null,
                         String      AuthorisationURL  = DefaultAuthorisationURL)

            : base(SOAPServer,
                   URLPathPrefix ?? DefaultURLPathPrefix)

        {

            this.ServiceName       = ServiceName      ?? "eMIP " + Version.Number + " " + nameof(CPOServer);
            this.AuthorisationURL  = AuthorisationURL ?? DefaultAuthorisationURL;

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

            #region ~/ - SetServiceAuthorisation

            // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST  -H "Content-Type: application/soap+xml" -H "Accept: application/soap+xml" --data-binary "@Tests/SetServiceAuthorisationRequest001.xml" http://127.0.0.1:3004/RNs/Prod/IO/Gireve
            // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URLPrefix + AuthorisationURL,
                                            "SetServiceAuthorisationRequest",
                                            XML => XML.Descendants(eMIPNS.Authorisation + "eMIP_FromIOP_SetServiceAuthorisationRequest").FirstOrDefault(),
                                            async (HTTPRequest, SetServiceAuthorisationXML) => {


                SetServiceAuthorisationResponse Response  = null;

                #region Send OnSetServiceAuthorisationSOAPRequest event

                var StartTime = DateTime.UtcNow;

                try
                {

                    if (OnSetServiceAuthorisationSOAPRequest != null)
                        await Task.WhenAll(OnSetServiceAuthorisationSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnSetServiceAuthorisationSOAPRequest));
                }

                #endregion


                if (SetServiceAuthorisationRequest.TryParse(SetServiceAuthorisationXML,
                                                            CustomSetServiceAuthorisationRequestParser,
                                                            CustomMeterReportParser,
                                                            out SetServiceAuthorisationRequest _SetServiceAuthorisationRequest,
                                                            OnException,

                                                            HTTPRequest,
                                                            HTTPRequest.Timestamp,
                                                            HTTPRequest.CancellationToken,
                                                            HTTPRequest.EventTrackingId,
                                                            HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnSetServiceAuthorisationRequest event

                    try
                    {

                        if (OnSetServiceAuthorisationRequest != null)
                            await Task.WhenAll(OnSetServiceAuthorisationRequest.GetInvocationList().
                                               Cast<OnSetServiceAuthorisationRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             _SetServiceAuthorisationRequest.Timestamp.Value,
                                                             this,
                                                             ServiceName,
                                                             _SetServiceAuthorisationRequest.EventTrackingId,
                                                             _SetServiceAuthorisationRequest.PartnerId,
                                                             _SetServiceAuthorisationRequest.OperatorId,
                                                             _SetServiceAuthorisationRequest.TargetOperatorId,
                                                             _SetServiceAuthorisationRequest.EVSEId,
                                                             _SetServiceAuthorisationRequest.UserId,
                                                             _SetServiceAuthorisationRequest.RequestedServiceId,
                                                             _SetServiceAuthorisationRequest.ServiceSessionId,
                                                             _SetServiceAuthorisationRequest.AuthorisationValue,
                                                             _SetServiceAuthorisationRequest.IntermediateCDRRequested,
                                                             _SetServiceAuthorisationRequest.TransactionId,
                                                             _SetServiceAuthorisationRequest.UserContractIdAlias,
                                                             _SetServiceAuthorisationRequest.MeterLimits,
                                                             _SetServiceAuthorisationRequest.Parameter,
                                                             _SetServiceAuthorisationRequest.BookingId,
                                                             _SetServiceAuthorisationRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnSetServiceAuthorisationRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnSetServiceAuthorisation != null)
                    {

                        var results = await Task.WhenAll(OnSetServiceAuthorisation.GetInvocationList().
                                                             Cast<OnSetServiceAuthorisationDelegate>().
                                                             Select(e => e(DateTime.UtcNow,
                                                                           this,
                                                                           _SetServiceAuthorisationRequest))).
                                                             ConfigureAwait(false);

                        Response = results.FirstOrDefault();

                    }

                    //"Could not forward the incoming SetServiceAuthorisation request!",
                    if (Response == null)
                        Response = SetServiceAuthorisationResponse.SystemError(
                                       _SetServiceAuthorisationRequest,
                                       _SetServiceAuthorisationRequest.TransactionId ?? Transaction_Id.Zero
                                   );

                    #endregion

                    #region Send OnSetServiceAuthorisationResponse event

                    var EndTime = DateTime.UtcNow;

                    try
                    {

                        if (OnSetServiceAuthorisationResponse != null)
                            await Task.WhenAll(OnSetServiceAuthorisationResponse.GetInvocationList().
                                               Cast<OnSetServiceAuthorisationResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceName,
                                                             _SetServiceAuthorisationRequest.EventTrackingId,
                                                             _SetServiceAuthorisationRequest.PartnerId,
                                                             _SetServiceAuthorisationRequest.OperatorId,
                                                             _SetServiceAuthorisationRequest.TargetOperatorId,
                                                             _SetServiceAuthorisationRequest.EVSEId,
                                                             _SetServiceAuthorisationRequest.UserId,
                                                             _SetServiceAuthorisationRequest.RequestedServiceId,
                                                             _SetServiceAuthorisationRequest.ServiceSessionId,
                                                             _SetServiceAuthorisationRequest.AuthorisationValue,
                                                             _SetServiceAuthorisationRequest.IntermediateCDRRequested,
                                                             _SetServiceAuthorisationRequest.TransactionId,
                                                             _SetServiceAuthorisationRequest.UserContractIdAlias,
                                                             _SetServiceAuthorisationRequest.MeterLimits,
                                                             _SetServiceAuthorisationRequest.Parameter,
                                                             _SetServiceAuthorisationRequest.BookingId,
                                                             _SetServiceAuthorisationRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Response,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnSetServiceAuthorisationResponse));
                    }

                    #endregion

                }

                // "Could not process the incoming SetServiceAuthorisation request!"
                else
                    Response = SetServiceAuthorisationResponse.SystemError(
                                   _SetServiceAuthorisationRequest,
                                   _SetServiceAuthorisationRequest.TransactionId ?? Transaction_Id.Zero
                               );


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.SOAPXML_UTF8,
                    Content         = SOAP.Encapsulation(Response.ToXML(CustomSetServiceAuthorisationResponseSerializer)).ToUTF8Bytes(),
                    Connection      = "close"
                };

                #endregion

                #region Send OnSetServiceAuthorisationSOAPResponse event

                try
                {

                    if (OnSetServiceAuthorisationSOAPResponse != null)
                        await Task.WhenAll(OnSetServiceAuthorisationSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnSetServiceAuthorisationSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region ~/ - SetSessionAction

            // ActionNature
            //  0  Emergency Stop                        (mandatory)
            //  1  Stop and terminate current operation  (mandatory)
            //  2  Suspend current operation
            //  3  Restart current operation

            // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST  -H "Content-Type: application/soap+xml" -H "Accept: application/soap+xml" --data-binary "@Tests/SetSessionActionRequestRequest001.xml" http://127.0.0.1:3004/RNs/Prod/IO/Gireve
            // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URLPrefix + AuthorisationURL,
                                            "SetSessionActionRequest",
                                            XML => XML.Descendants(eMIPNS.Authorisation + "eMIP_FromIOP_SetSessionActionRequestRequest").FirstOrDefault(),
                                            async (HTTPRequest, SetSessionActionXML) => {


                SetSessionActionRequestResponse Response  = null;

                #region Send OnSetSessionActionSOAPRequest event

                var StartTime = DateTime.UtcNow;

                try
                {

                    if (OnSetSessionActionSOAPRequest != null)
                        await Task.WhenAll(OnSetSessionActionSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnSetSessionActionSOAPRequest));
                }

                #endregion


                if (SetSessionActionRequestRequest.TryParse(SetSessionActionXML,
                                                            out SetSessionActionRequestRequest setSessionActionRequest,
                                                            CustomSetSessionActionRequestParser,
                                                            CustomSessionActionParser,
                                                            OnException,

                                                            HTTPRequest,
                                                            HTTPRequest.Timestamp,
                                                            HTTPRequest.CancellationToken,
                                                            HTTPRequest.EventTrackingId,
                                                            HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnSetSessionActionRequest event

                    try
                    {

                        if (OnSetSessionActionRequest != null)
                            await Task.WhenAll(OnSetSessionActionRequest.GetInvocationList().
                                               Cast<OnSetSessionActionRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             setSessionActionRequest.Timestamp.Value,
                                                             this,
                                                             ServiceName,
                                                             setSessionActionRequest.EventTrackingId,

                                                             setSessionActionRequest.PartnerId,
                                                             setSessionActionRequest.OperatorId,
                                                             setSessionActionRequest.TargetOperatorId,
                                                             setSessionActionRequest.ServiceSessionId,
                                                             setSessionActionRequest.SessionAction,
                                                             setSessionActionRequest.TransactionId,
                                                             setSessionActionRequest.ExecPartnerSessionId,

                                                             setSessionActionRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnSetSessionActionRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnSetSessionAction != null)
                    {

                        var results = await Task.WhenAll(OnSetSessionAction.GetInvocationList().
                                                             Cast<OnSetSessionActionDelegate>().
                                                             Select(e => e(DateTime.UtcNow,
                                                                           this,
                                                                           setSessionActionRequest))).
                                                             ConfigureAwait(false);

                        Response = results.FirstOrDefault();

                    }

                    // "Could not process the incoming SetSessionAction request!",
                    if (Response == null)
                        Response = SetSessionActionRequestResponse.SystemError(
                                       setSessionActionRequest,
                                       setSessionActionRequest.TransactionId ?? Transaction_Id.Zero
                                   );

                    #endregion

                    #region Send OnSetSessionActionResponse event

                    var EndTime = DateTime.UtcNow;

                    try
                    {

                        if (OnSetSessionActionResponse != null)
                            await Task.WhenAll(OnSetSessionActionResponse.GetInvocationList().
                                               Cast<OnSetSessionActionResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceName,
                                                             setSessionActionRequest.EventTrackingId,

                                                             setSessionActionRequest.PartnerId,
                                                             setSessionActionRequest.OperatorId,
                                                             setSessionActionRequest.TargetOperatorId,
                                                             setSessionActionRequest.ServiceSessionId,
                                                             setSessionActionRequest.SessionAction,
                                                             setSessionActionRequest.TransactionId,
                                                             setSessionActionRequest.ExecPartnerSessionId,

                                                             setSessionActionRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Response,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnSetSessionActionResponse));
                    }

                    #endregion

                }

                // "Could not process the incoming SetSessionAction request!"
                else
                    Response = SetSessionActionRequestResponse.SystemError(
                                   setSessionActionRequest,
                                   setSessionActionRequest.TransactionId ?? Transaction_Id.Zero
                               );


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.SOAPXML_UTF8,
                    Content         = SOAP.Encapsulation(Response.ToXML(CustomSetSessionActionResponseSerializer)).ToUTF8Bytes(),
                    Connection      = "close"
                };

                #endregion

                #region Send OnSetSessionActionSOAPResponse event

                try
                {

                    if (OnSetSessionActionSOAPResponse != null)
                        await Task.WhenAll(OnSetSessionActionSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnSetSessionActionSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

        }

        #endregion


    }

}
