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
        public new static readonly HTTPURI          DefaultURIPrefix           = HTTPURI.Parse("/");

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

        #endregion

        #region Custom request/response mappers

        public OnExceptionDelegate                                                                       OnException                                                      { get; set; }

        #endregion

        #region Events

        #endregion

        #region Constructor(s)

        #region CPOServer(HTTPServerName, ServiceId = null, TCPPort = default, URIPrefix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the eMIP HTTP/SOAP/XML CPO API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public CPOServer(String           HTTPServerName            = DefaultHTTPServerName,
                         String           ServiceId                 = null,
                         IPPort?          TCPPort                   = null,
                         HTTPURI?         URIPrefix                 = null,
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

            this.ServiceId  = ServiceId ?? nameof(CPOServer);

            RegisterURITemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region CPOServer(SOAPServer, ServiceId = null, URIPrefix = default)

        /// <summary>
        /// Use the given SOAP server for the eMIP HTTP/SOAP/XML CPO API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public CPOServer(SOAPServer  SOAPServer,
                         String      ServiceId   = null,
                         HTTPURI?    URIPrefix   = null)

            : base(SOAPServer,
                   URIPrefix ?? DefaultURIPrefix)

        {

            this.ServiceId  = ServiceId ?? nameof(CPOServer);

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

            #region /Authorization - AuthorizeRemoteStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix,// + AuthorizationURI,
                                            "AuthorizeRemoteStart",
                                            XML => XML.Descendants(eMIPNS.Authorisation + "eRoamingAuthorizeRemoteStart").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeRemoteStartXML) => {


// <soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope" xmlns:aut="https://api-iop.gireve.com/schemas/AuthorisationV1/">
//   <soap:Header/>
//   <soap:Body>
//      <aut:eMIP_FromIOP_SetServiceAuthorisationRequest>
//
//         <!--Optional:-->
//         <transactionId>?</transactionId>
//
//         <partnerIdType>?</partnerIdType>
//         <partnerId>?</partnerId>
//         <operatorIdType>?</operatorIdType>
//         <operatorId>?</operatorId>
//         <targetOperatorIdType>?</targetOperatorIdType>
//         <targetOperatorId>?</targetOperatorId>
//         <EVSEIdType>?</EVSEIdType>
//         <EVSEId>?</EVSEId>
//         <userIdType>?</userIdType>
//         <userId>?</userId>
//         <requestedServiceId>?</requestedServiceId>
//         <serviceSessionId>?</serviceSessionId>
//         <authorisationValue>?</authorisationValue>
//         <intermediateCDRRequested>?</intermediateCDRRequested>
//
//         <!--Optional:-->
//         <userContractIdAlias>?</userContractIdAlias>
//
//         <!--Optional:-->
//         <meterLimitList>
//
//            <!--Zero or more repetitions:-->
//            <meterReport>
//               <meterTypeId>?</meterTypeId>
//               <meterValue>?</meterValue>
//               <meterUnit>?</meterUnit>
//            </meterReport>
//
//         </meterLimitList>
//
//         <!--Optional:-->
//         <parameter>?</parameter>
//
//         <!--Optional:-->
//         <bookingId>?</bookingId>
//
//      </aut:eMIP_FromIOP_SetServiceAuthorisationRequest>
//   </soap:Body>
// </soap:Envelope>


             //   Acknowledgement<EMP.AuthorizeRemoteStartRequest> Acknowledgement  = null;

                #region Send OnAuthorizeRemoteStartSOAPRequest event

                //var StartTime = DateTime.Now;

                //try
                //{

                //    if (OnAuthorizeRemoteStartSOAPRequest != null)
                //        await Task.WhenAll(OnAuthorizeRemoteStartSOAPRequest.GetInvocationList().
                //                           Cast<RequestLogHandler>().
                //                           Select(e => e(StartTime,
                //                                         SOAPServer,
                //                                         HTTPRequest))).
                //                           ConfigureAwait(false);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStartSOAPRequest));
                //}

                #endregion


                //if (EMP.AuthorizeRemoteStartRequest.TryParse(AuthorizeRemoteStartXML,
                //                                             out EMP.AuthorizeRemoteStartRequest AuthorizeRemoteStartRequest,
                //                                             CustomAuthorizeRemoteStartRequestParser,
                //                                             CustomIdentificationParser,
                //                                             OnException,

                //                                             HTTPRequest.Timestamp,
                //                                             HTTPRequest.CancellationToken,
                //                                             HTTPRequest.EventTrackingId,
                //                                             HTTPRequest.Timeout ?? DefaultRequestTimeout))
                //{

                //    #region Send OnAuthorizeRemoteStartRequest event

                //    try
                //    {

                //        if (OnAuthorizeRemoteStartRequest != null)
                //            await Task.WhenAll(OnAuthorizeRemoteStartRequest.GetInvocationList().
                //                               Cast<OnAuthorizeRemoteStartRequestDelegate>().
                //                               Select(e => e(StartTime,
                //                                              AuthorizeRemoteStartRequest.Timestamp.Value,
                //                                              this,
                //                                              ServiceId,
                //                                              AuthorizeRemoteStartRequest.EventTrackingId,
                //                                              AuthorizeRemoteStartRequest.EVSEId,
                //                                              AuthorizeRemoteStartRequest.PartnerProductId,
                //                                              AuthorizeRemoteStartRequest.SessionId,
                //                                              AuthorizeRemoteStartRequest.PartnerSessionId,
                //                                              AuthorizeRemoteStartRequest.ProviderId,
                //                                              AuthorizeRemoteStartRequest.EVCOId,
                //                                              AuthorizeRemoteStartRequest.RequestTimeout ?? DefaultRequestTimeout))).
                //                               ConfigureAwait(false);

                //    }
                //    catch (Exception e)
                //    {
                //        e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStartRequest));
                //    }

                //    #endregion

                //    #region Call async subscribers

                //    if (OnAuthorizeRemoteStart != null)
                //    {

                //        var results = await Task.WhenAll(OnAuthorizeRemoteStart.GetInvocationList().
                //                                             Cast<OnAuthorizeRemoteStartDelegate>().
                //                                             Select(e => e(DateTime.Now,
                //                                                           this,
                //                                                           AuthorizeRemoteStartRequest))).
                //                                             ConfigureAwait(false);

                //        Acknowledgement = results.FirstOrDefault();

                //    }

                //    if (Acknowledgement == null)
                //        Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteStartRequest>.SystemError(
                //                             AuthorizeRemoteStartRequest,
                //                             "Could not process the incoming AuthorizeRemoteStart request!",
                //                             null,
                //                             AuthorizeRemoteStartRequest.SessionId,
                //                             AuthorizeRemoteStartRequest.PartnerSessionId
                //                         );

                //    #endregion

                //    #region Send OnAuthorizeRemoteStartResponse event

                //    var EndTime = DateTime.Now;

                //    try
                //    {

                //        if (OnAuthorizeRemoteStartResponse != null)
                //            await Task.WhenAll(OnAuthorizeRemoteStartResponse.GetInvocationList().
                //                               Cast<OnAuthorizeRemoteStartResponseDelegate>().
                //                               Select(e => e(EndTime,
                //                                             this,
                //                                             ServiceId,
                //                                             AuthorizeRemoteStartRequest.EventTrackingId,
                //                                             AuthorizeRemoteStartRequest.EVSEId,
                //                                             AuthorizeRemoteStartRequest.PartnerProductId,
                //                                             AuthorizeRemoteStartRequest.SessionId,
                //                                             AuthorizeRemoteStartRequest.PartnerSessionId,
                //                                             AuthorizeRemoteStartRequest.ProviderId,
                //                                             AuthorizeRemoteStartRequest.EVCOId,
                //                                             AuthorizeRemoteStartRequest.RequestTimeout ?? DefaultRequestTimeout,
                //                                             Acknowledgement,
                //                                             EndTime - StartTime))).
                //                               ConfigureAwait(false);

                //    }
                //    catch (Exception e)
                //    {
                //        e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStartResponse));
                //    }

                //    #endregion

                //}

                //else
                //    Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteStartRequest>.DataError(
                //                          AuthorizeRemoteStartRequest,
                //                          "Could not process the incoming AuthorizeRemoteStart request!"
                //                      );


                #region Create SOAPResponse

                //var HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                //    HTTPStatusCode  = HTTPStatusCode.OK,
                //    Server          = SOAPServer.DefaultServerName,
                //    Date            = DateTime.Now,
                //    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                //    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomAuthorizeRemoteStartAcknowledgementSerializer,
                //                                                               CustomStatusCodeSerializer)).ToUTF8Bytes(),
                //    Connection      = "close"
                //};

                #endregion

                #region Send OnAuthorizeRemoteStartSOAPResponse event

                //try
                //{

                //    if (OnAuthorizeRemoteStartSOAPResponse != null)
                //        await Task.WhenAll(OnAuthorizeRemoteStartSOAPResponse.GetInvocationList().
                //                           Cast<AccessLogHandler>().
                //                           Select(e => e(HTTPResponse.Timestamp,
                //                                         SOAPServer,
                //                                         HTTPRequest,
                //                                         HTTPResponse))).
                //                           ConfigureAwait(false);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStartSOAPResponse));
                //}

                #endregion

                //return HTTPResponse;
                return null;

            });

            #endregion

        }

        #endregion


    }

}
