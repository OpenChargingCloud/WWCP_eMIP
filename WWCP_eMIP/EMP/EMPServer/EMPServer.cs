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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.EMP
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
        public new static readonly HTTPPath         DefaultURLPrefix           = HTTPPath.Parse("/");

        /// <summary>
        /// The default HTTP/SOAP/XML URI for eMIP authorization requests.
        /// </summary>
        public     const           String           DefaultAuthorisationURL    = "";

        /// <summary>
        /// The default HTTP/SOAP/XML content type.
        /// </summary>
        public new static readonly HTTPContentType  DefaultContentType         = HTTPContentType.Text.XML_UTF8;

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

        public CustomXMLParserDelegate<GetServiceAuthorisationRequest>       CustomGetServiceAuthorisationRequestParser         { get; set; }

        public CustomXMLSerializerDelegate<GetServiceAuthorisationResponse>  CustomGetServiceAuthorisationResponseSerializer    { get; set; }


        public CustomXMLParserDelegate<SetSessionEventReportRequest>         CustomSetSessionEventReportRequestParser           { get; set; }

        public CustomXMLParserDelegate<SessionEvent>                         CustomSessionEventParser                           { get; set; }

        public CustomXMLSerializerDelegate<SetSessionEventReportResponse>    CustomSetSessionEventReportResponseSerializer      { get; set; }


        public CustomXMLParserDelegate<SetChargeDetailRecordRequest>         CustomSetChargeDetailRecordRequestParser           { get; set; }

        public CustomXMLParserDelegate<ChargeDetailRecord>                   CustomChargeDetailRecordParser                     { get; set; }

        public CustomXMLParserDelegate<MeterReport>                          CustomMeterReportParser                            { get; set; }

        public CustomXMLSerializerDelegate<SetChargeDetailRecordResponse>    CustomSetChargeDetailRecordResponseSerializer      { get; set; }


        public OnExceptionDelegate                                           OnException                                        { get; set; }

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

        #region OnSetSessionEventReport

        /// <summary>
        /// An event sent whenever a SetSessionEventReport SOAP request was received.
        /// </summary>
        public event RequestLogHandler                           OnSetSessionEventReportSOAPRequest;

        /// <summary>
        /// An event sent whenever a SetSessionEventReport request was received.
        /// </summary>
        public event OnSetSessionEventReportRequestDelegate      OnSetSessionEventReportRequest;

        /// <summary>
        /// An event sent whenever a SetSessionEventReport request was received.
        /// </summary>
        public event OnSetSessionEventReportDelegate             OnSetSessionEventReport;

        /// <summary>
        /// An event sent whenever a response to a SetSessionEventReport request was sent.
        /// </summary>
        public event OnSetSessionEventReportResponseDelegate     OnSetSessionEventReportResponse;

        /// <summary>
        /// An event sent whenever a response to a SetSessionEventReport SOAP request was sent.
        /// </summary>
        public event AccessLogHandler                            OnSetSessionEventReportSOAPResponse;

        #endregion

        #region OnSetChargeDetailRecord

        /// <summary>
        /// An event sent whenever a SetChargeDetailRecord SOAP request was received.
        /// </summary>
        public event RequestLogHandler                           OnSetChargeDetailRecordSOAPRequest;

        /// <summary>
        /// An event sent whenever a SetChargeDetailRecord request was received.
        /// </summary>
        public event OnSetChargeDetailRecordRequestDelegate      OnSetChargeDetailRecordRequest;

        /// <summary>
        /// An event sent whenever a SetChargeDetailRecord request was received.
        /// </summary>
        public event OnSetChargeDetailRecordDelegate             OnSetChargeDetailRecord;

        /// <summary>
        /// An event sent whenever a response to a SetChargeDetailRecord request was sent.
        /// </summary>
        public event OnSetChargeDetailRecordResponseDelegate     OnSetChargeDetailRecordResponse;

        /// <summary>
        /// An event sent whenever a response to a SetChargeDetailRecord SOAP request was sent.
        /// </summary>
        public event AccessLogHandler                            OnSetChargeDetailRecordSOAPResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPServer(HTTPServerName, ServiceName = null, TCPPort = default, URLPrefix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the eMIP HTTP/SOAP/XML EMP API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServiceName">An optional identification for this SOAP service.</param>
        /// <param name="URLPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="AuthorisationURL">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public EMPServer(String           HTTPServerName            = DefaultHTTPServerName,
                         IPPort?          TCPPort                   = null,
                         String           ServiceName               = null,
                         HTTPPath?        URLPrefix                 = null,
                         String           AuthorisationURL          = DefaultAuthorisationURL,
                         HTTPContentType  ContentType               = null,
                         Boolean          RegisterHTTPRootService   = true,
                         DNSClient        DNSClient                 = null,
                         Boolean          AutoStart                 = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort     ?? DefaultHTTPServerPort,
                   ServiceName ?? "eMIP " + Version.Number + " " + nameof(EMPServer),
                   URLPrefix   ?? DefaultURLPrefix,
                   ContentType ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            this.ServiceName       = ServiceName      ?? "eMIP " + Version.Number + " " + nameof(EMPServer);
            this.AuthorisationURL  = AuthorisationURL ?? DefaultAuthorisationURL;

            RegisterURITemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region EMPServer(SOAPServer, ServiceName = null, URLPrefix = default)

        /// <summary>
        /// Use the given SOAP server for the eMIP HTTP/SOAP/XML EMP API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="ServiceName">An optional identification for this SOAP service.</param>
        /// <param name="URLPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="AuthorisationURL">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
        public EMPServer(SOAPServer  SOAPServer,
                         String      ServiceName       = null,
                         HTTPPath?   URLPrefix         = null,
                         String      AuthorisationURL  = DefaultAuthorisationURL)

            : base(SOAPServer,
                   URLPrefix ?? DefaultURLPrefix)

        {

            this.ServiceName       = ServiceName      ?? "eMIP " + Version.Number + " " + nameof(EMPServer);
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

            #region ~/ - GetServiceAuthorisation

            SOAPServer.RegisterSOAPDelegate(null,
                                            HTTPHostname.Any,
                                            URLPrefix + AuthorisationURL,
                                            "GetServiceAuthorisationRequest",
                                            XML => XML.Descendants(eMIPNS.Authorisation + "eMIP_FromIOP_GetServiceAuthorisationRequest").FirstOrDefault(),
                                            async (HTTPRequest, GetServiceAuthorisationXML) => {


                GetServiceAuthorisationResponse Response  = null;

                #region Send OnGetServiceAuthorisationSOAPRequest event

                var StartTime = Timestamp.Now;

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
                    DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnGetServiceAuthorisationSOAPRequest));
                }

                #endregion


                if (GetServiceAuthorisationRequest.TryParse(GetServiceAuthorisationXML,
                                                            CustomGetServiceAuthorisationRequestParser,
                                                            out GetServiceAuthorisationRequest _GetServiceAuthorisationRequest,
                                                            OnException,

                                                            HTTPRequest,
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
                                                              _GetServiceAuthorisationRequest.Timestamp,
                                                              this,
                                                              ServiceName,
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
                        DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnGetServiceAuthorisationRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnGetServiceAuthorisation != null)
                    {

                        var results = await Task.WhenAll(OnGetServiceAuthorisation.GetInvocationList().
                                                             Cast<OnGetServiceAuthorisationDelegate>().
                                                             Select(e => e(Timestamp.Now,
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

                    var EndTime = Timestamp.Now;

                    try
                    {

                        if (OnGetServiceAuthorisationResponse != null)
                            await Task.WhenAll(OnGetServiceAuthorisationResponse.GetInvocationList().
                                               Cast<OnGetServiceAuthorisationResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceName,
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
                        DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnGetServiceAuthorisationResponse));
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
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Text.XML_UTF8,
                    Content         = SOAP.Encapsulation(Response.ToXML(CustomGetServiceAuthorisationResponseSerializer)).ToUTF8Bytes(),
                    Connection      = ConnectionType.Close
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
                    DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnGetServiceAuthorisationSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region ~/ - SetSessionEventReport

            SOAPServer.RegisterSOAPDelegate(null,
                                            HTTPHostname.Any,
                                            URLPrefix + AuthorisationURL,
                                            "SetSessionEventReportRequest",
                                            XML => XML.Descendants(eMIPNS.Authorisation + "eMIP_FromIOP_SetSessionEventReportRequest").FirstOrDefault(),
                                            async (HTTPRequest, SetSessionEventReportXML) => {


                SetSessionEventReportResponse Response  = null;

                #region Send OnSetSessionEventReportSOAPRequest event

                var StartTime = Timestamp.Now;

                try
                {

                    if (OnSetSessionEventReportSOAPRequest != null)
                        await Task.WhenAll(OnSetSessionEventReportSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnSetSessionEventReportSOAPRequest));
                }

                #endregion


                if (SetSessionEventReportRequest.TryParse(SetSessionEventReportXML,
                                                          out SetSessionEventReportRequest _SetSessionEventReportRequest,
                                                          CustomSetSessionEventReportRequestParser,
                                                          CustomSessionEventParser,
                                                          OnException,

                                                          HTTPRequest,
                                                          HTTPRequest.Timestamp,
                                                          HTTPRequest.CancellationToken,
                                                          HTTPRequest.EventTrackingId,
                                                          HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnSetSessionEventReportRequest event

                    try
                    {

                        if (OnSetSessionEventReportRequest != null)
                            await Task.WhenAll(OnSetSessionEventReportRequest.GetInvocationList().
                                               Cast<OnSetSessionEventReportRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             _SetSessionEventReportRequest.Timestamp,
                                                             this,
                                                             ServiceName,
                                                             _SetSessionEventReportRequest.EventTrackingId,

                                                             _SetSessionEventReportRequest.PartnerId,
                                                             _SetSessionEventReportRequest.OperatorId,
                                                             _SetSessionEventReportRequest.TargetOperatorId,
                                                             _SetSessionEventReportRequest.ServiceSessionId,
                                                             _SetSessionEventReportRequest.SessionEvent,

                                                             _SetSessionEventReportRequest.TransactionId,
                                                             _SetSessionEventReportRequest.SalePartnerSessionId,

                                                             _SetSessionEventReportRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnSetSessionEventReportRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnSetSessionEventReport != null)
                    {

                        var results = await Task.WhenAll(OnSetSessionEventReport.GetInvocationList().
                                                             Cast<OnSetSessionEventReportDelegate>().
                                                             Select(e => e(Timestamp.Now,
                                                                           this,
                                                                           _SetSessionEventReportRequest))).
                                                             ConfigureAwait(false);

                        Response = results.FirstOrDefault();

                    }

                    //if (Response == null)
                    //    Response = Response<EMP.SetSessionEventReportRequest>.SystemError(
                    //                         _SetSessionEventReportRequest,
                    //                         "Could not process the incoming SetSessionEventReport request!",
                    //                         null,
                    //                         _SetSessionEventReportRequest.SessionId,
                    //                         _SetSessionEventReportRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnSetSessionEventReportResponse event

                    var EndTime = Timestamp.Now;

                    try
                    {

                        if (OnSetSessionEventReportResponse != null)
                            await Task.WhenAll(OnSetSessionEventReportResponse.GetInvocationList().
                                               Cast<OnSetSessionEventReportResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceName,
                                                             _SetSessionEventReportRequest.EventTrackingId,

                                                             _SetSessionEventReportRequest.PartnerId,
                                                             _SetSessionEventReportRequest.OperatorId,
                                                             _SetSessionEventReportRequest.TargetOperatorId,
                                                             _SetSessionEventReportRequest.ServiceSessionId,
                                                             _SetSessionEventReportRequest.SessionEvent,

                                                             _SetSessionEventReportRequest.TransactionId,
                                                             _SetSessionEventReportRequest.SalePartnerSessionId,

                                                             _SetSessionEventReportRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Response,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnSetSessionEventReportResponse));
                    }

                    #endregion

                }

                //else
                //    Response = Response<EMP.SetSessionEventReportRequest>.DataError(
                //                          _SetSessionEventReportRequest,
                //                          "Could not process the incoming SetSessionEventReport request!"
                //                      );


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Text.XML_UTF8,
                    Content         = SOAP.Encapsulation(Response.ToXML(CustomSetSessionEventReportResponseSerializer)).ToUTF8Bytes(),
                    Connection      = ConnectionType.Close
                };

                #endregion

                #region Send OnSetSessionEventReportSOAPResponse event

                try
                {

                    if (OnSetSessionEventReportSOAPResponse != null)
                        await Task.WhenAll(OnSetSessionEventReportSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnSetSessionEventReportSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region ~/ - SetChargeDetailRecord

            SOAPServer.RegisterSOAPDelegate(null,
                                            HTTPHostname.Any,
                                            URLPrefix + AuthorisationURL,
                                            "SetChargeDetailRecordRequest",
                                            XML => XML.Descendants(eMIPNS.Authorisation + "eMIP_FromIOP_SetChargeDetailRecordRequest").FirstOrDefault(),
                                            async (HTTPRequest, SetChargeDetailRecordXML) => {


                SetChargeDetailRecordResponse Response  = null;

                #region Send OnSetChargeDetailRecordSOAPRequest event

                var StartTime = Timestamp.Now;

                try
                {

                    if (OnSetChargeDetailRecordSOAPRequest != null)
                        await Task.WhenAll(OnSetChargeDetailRecordSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnSetChargeDetailRecordSOAPRequest));
                }

                #endregion


                if (SetChargeDetailRecordRequest.TryParse(SetChargeDetailRecordXML,
                                                          out SetChargeDetailRecordRequest _SetChargeDetailRecordRequest,
                                                          CustomSetChargeDetailRecordRequestParser,
                                                          CustomChargeDetailRecordParser,
                                                          CustomMeterReportParser,
                                                          OnException,

                                                          HTTPRequest,
                                                          HTTPRequest.Timestamp,
                                                          HTTPRequest.CancellationToken,
                                                          HTTPRequest.EventTrackingId,
                                                          HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnSetChargeDetailRecordRequest event

                    try
                    {

                        if (OnSetChargeDetailRecordRequest != null)
                            await Task.WhenAll(OnSetChargeDetailRecordRequest.GetInvocationList().
                                               Cast<OnSetChargeDetailRecordRequestDelegate>().
                                               Select(e => e(StartTime,
                                                              _SetChargeDetailRecordRequest.Timestamp,
                                                              this,
                                                              ServiceName,
                                                              _SetChargeDetailRecordRequest.EventTrackingId,

                                                              _SetChargeDetailRecordRequest.PartnerId,
                                                              _SetChargeDetailRecordRequest.OperatorId,
                                                              _SetChargeDetailRecordRequest.ChargeDetailRecord,
                                                              _SetChargeDetailRecordRequest.TransactionId,

                                                              _SetChargeDetailRecordRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnSetChargeDetailRecordRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnSetChargeDetailRecord != null)
                    {

                        var results = await Task.WhenAll(OnSetChargeDetailRecord.GetInvocationList().
                                                             Cast<OnSetChargeDetailRecordDelegate>().
                                                             Select(e => e(Timestamp.Now,
                                                                           this,
                                                                           _SetChargeDetailRecordRequest))).
                                                             ConfigureAwait(false);

                        Response = results.FirstOrDefault();

                    }

                    //if (Response == null)
                    //    Response = Response<EMP.SetChargeDetailRecordRequest>.SystemError(
                    //                         _SetChargeDetailRecordRequest,
                    //                         "Could not process the incoming SetChargeDetailRecord request!",
                    //                         null,
                    //                         _SetChargeDetailRecordRequest.SessionId,
                    //                         _SetChargeDetailRecordRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnSetChargeDetailRecordResponse event

                    var EndTime = Timestamp.Now;

                    try
                    {

                        if (OnSetChargeDetailRecordResponse != null)
                            await Task.WhenAll(OnSetChargeDetailRecordResponse.GetInvocationList().
                                               Cast<OnSetChargeDetailRecordResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceName,
                                                             _SetChargeDetailRecordRequest.EventTrackingId,

                                                             _SetChargeDetailRecordRequest.PartnerId,
                                                             _SetChargeDetailRecordRequest.OperatorId,
                                                             _SetChargeDetailRecordRequest.ChargeDetailRecord,
                                                             _SetChargeDetailRecordRequest.TransactionId,

                                                             _SetChargeDetailRecordRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Response,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnSetChargeDetailRecordResponse));
                    }

                    #endregion

                }

                //else
                //    Response = Response<EMP.SetChargeDetailRecordRequest>.DataError(
                //                          _SetChargeDetailRecordRequest,
                //                          "Could not process the incoming SetChargeDetailRecord request!"
                //                      );


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Text.XML_UTF8,
                    Content         = SOAP.Encapsulation(Response.ToXML(CustomSetChargeDetailRecordResponseSerializer)).ToUTF8Bytes(),
                    Connection      = ConnectionType.Close
                };

                #endregion

                #region Send OnSetChargeDetailRecordSOAPResponse event

                try
                {

                    if (OnSetChargeDetailRecordSOAPResponse != null)
                        await Task.WhenAll(OnSetChargeDetailRecordSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnSetChargeDetailRecordSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

        }

        #endregion


    }

}
