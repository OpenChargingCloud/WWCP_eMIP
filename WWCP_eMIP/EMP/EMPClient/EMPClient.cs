/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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
using org.GraphDefined.Vanaheimr.Hermod.SOAP.v1_2;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.EMP
{

    /// <summary>
    /// The eMIP EMP client.
    /// </summary>
    public partial class EMPClient : ASOAPClient,
                                     IEMPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String    DefaultHTTPUserAgent        = "GraphDefined eMIP " + Version.Number + " EMP Client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort    DefaultRemotePort           = IPPort.Parse(443);

        /// <summary>
        /// The default URI prefix.
        /// </summary>
        public     static readonly HTTPPath  DefaultURLPrefix            = HTTPPath.Parse("/api/emip");


        /// <summary>
        /// The default SOAP action prefix.
        /// </summary>
        public     const           String    DefaultSOAPActionPrefix     = "https://api-iop.gireve.com/services/";

        #endregion

        #region Properties

        /// <summary>
        /// The attached HTTP client logger.
        /// </summary>
        public new Logger HTTPLogger
        {
            get
            {
                return base.HTTPLogger as Logger;
            }
            set
            {
                base.HTTPLogger = value;
            }
        }

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


        #region CustomSetServiceAuthorisationRequestMapper

        #region CustomSetServiceAuthorisationRequestMapper

        private Func<SetServiceAuthorisationRequest, SetServiceAuthorisationRequest> _CustomSetServiceAuthorisationRequestMapper = _ => _;

        public Func<SetServiceAuthorisationRequest, SetServiceAuthorisationRequest> CustomSetServiceAuthorisationRequestMapper
        {

            get
            {
                return _CustomSetServiceAuthorisationRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetServiceAuthorisationRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetServiceAuthorisationSOAPRequestMapper

        private Func<SetServiceAuthorisationRequest, XElement, XElement> _CustomSetServiceAuthorisationSOAPRequestMapper = (request, xml) => xml;

        public Func<SetServiceAuthorisationRequest, XElement, XElement> CustomSetServiceAuthorisationSOAPRequestMapper
        {

            get
            {
                return _CustomSetServiceAuthorisationSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetServiceAuthorisationSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetServiceAuthorisationResponse> CustomSetServiceAuthorisationParser   { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<SetServiceAuthorisationRequest>    CustomSetServiceAuthorisationRequestSerializer      { get; set; }


        #region CustomSetSessionActionMapper

        #region CustomSetSessionActionRequestMapper

        private Func<SetSessionActionRequest, SetSessionActionRequest> _CustomSetSessionActionRequestMapper = _ => _;

        public Func<SetSessionActionRequest, SetSessionActionRequest> CustomSetSessionActionRequestMapper
        {

            get
            {
                return _CustomSetSessionActionRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetSessionActionRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetSessionActionSOAPRequestMapper

        private Func<SetSessionActionRequest, XElement, XElement> _CustomSetSessionActionSOAPRequestMapper = (request, xml) => xml;

        public Func<SetSessionActionRequest, XElement, XElement> CustomSetSessionActionSOAPRequestMapper
        {

            get
            {
                return _CustomSetSessionActionSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetSessionActionSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetSessionActionResponse> CustomSetSessionActionParser   { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<SetSessionActionRequest>    CustomSetSessionActionRequestSerializer      { get; set; }


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


        #region OnSetServiceAuthorisationRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a SetServiceAuthorisation will be send.
        /// </summary>
        public event OnSetServiceAuthorisationRequestDelegate   OnSetServiceAuthorisationRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a SetServiceAuthorisation will be send.
        /// </summary>
        public event ClientRequestLogHandler                    OnSetServiceAuthorisationSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a SetServiceAuthorisation SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                   OnSetServiceAuthorisationSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a SetServiceAuthorisation request had been received.
        /// </summary>
        public event OnSetServiceAuthorisationResponseDelegate  OnSetServiceAuthorisationResponse;

        #endregion

        #region OnSetSessionActionRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a SetSessionAction will be send.
        /// </summary>
        public event OnSetSessionActionRequestDelegate   OnSetSessionActionRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a SetSessionAction will be send.
        /// </summary>
        public event ClientRequestLogHandler             OnSetSessionActionSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a SetSessionAction SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler            OnSetSessionActionSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a SetSessionAction request had been received.
        /// </summary>
        public event OnSetSessionActionResponseDelegate  OnSetSessionActionResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMP client.
        /// </summary>
        /// <param name="RemoteURL">The remote URL of the OICP HTTP endpoint to connect to.</param>
        /// <param name="VirtualHostname">An optional HTTP virtual hostname.</param>
        /// <param name="Description">An optional description of this CPO client.</param>
        /// <param name="RemoteCertificateValidator">The remote SSL/TLS certificate validator.</param>
        /// <param name="ClientCert">The SSL/TLS client certificate to use of HTTP authentication.</param>
        /// <param name="HTTPUserAgent">The HTTP user agent identification.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries for HTTP request.</param>
        /// <param name="DisableLogging">Disable all logging.</param>
        /// <param name="LoggingContext">An optional context for logging.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// <param name="DNSClient">The DNS client to use.</param>
        public EMPClient(URL?                                  RemoteURL                    = null,
                         HTTPHostname?                         VirtualHostname              = null,
                         String?                               Description                  = null,
                         Boolean?                              PreferIPv4                   = null,
                         RemoteCertificateValidationCallback?  RemoteCertificateValidator   = null,
                         LocalCertificateSelectionCallback?    ClientCertificateSelector    = null,
                         X509Certificate?                      ClientCert                   = null,
                         SslProtocols?                         TLSProtocol                  = null,
                         String                                HTTPUserAgent                = DefaultHTTPUserAgent,
                         TimeSpan?                             RequestTimeout               = null,
                         TransmissionRetryDelayDelegate?       TransmissionRetryDelay       = null,
                         UInt16?                               MaxNumberOfRetries           = null,
                         UInt32?                               InternalBufferSize           = null,
                         Boolean?                              DisableLogging               = false,
                         String?                               LoggingPath                  = null,
                         String?                               LoggingContext               = Logger.DefaultContext,
                         LogfileCreatorDelegate?               LogfileCreator               = null,
                         DNSClient?                            DNSClient                    = null)

            : base(RemoteURL     ?? URL.Parse("???"),
                   VirtualHostname,
                   Description,
                   PreferIPv4,
                   RemoteCertificateValidator,
                   ClientCertificateSelector,
                   ClientCert,
                   TLSProtocol,
                   HTTPUserAgent ?? DefaultHTTPUserAgent,
                   null,
                   null,
                   HTTPContentType.XMLTEXT_UTF8,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries,
                   InternalBufferSize,
                   false,
                   DisableLogging,
                   null,
                   DNSClient)

        {

            base.HTTPLogger  = this.DisableLogging == false
                                   ? new Logger(this,
                                                LoggingPath,
                                                LoggingContext,
                                                LogfileCreator)
                                   : null;

        }

        #endregion


        #region SendHeartbeat              (Request)

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
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.TransactionId,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnSendHeartbeatRequest));
            }

            #endregion


            do
            {

                using (var _eMIPClient = new SOAPClient(RemoteURL,
                                                        VirtualHostname,
                                                        false,
                                                        Description,
                                                        PreferIPv4,
                                                        RemoteCertificateValidator,
                                                        ClientCertificateSelector,
                                                        ClientCert,
                                                        TLSProtocol,
                                                        HTTPUserAgent,
                                                        URLPathPrefix,
                                                        null,
                                                        null,
                                                        RequestTimeout,
                                                        TransmissionRetryDelay,
                                                        MaxNumberOfRetries,
                                                        InternalBufferSize,
                                                        false,
                                                        false,
                                                        null,
                                                        DNSClient))
                {

                    result = await _eMIPClient.Query(_CustomHeartbeatSOAPRequestMapper(Request,
                                                                                       SOAP.Encapsulation(Request.ToXML(CustomHeartbeatRequestSerializer))),
                                                     DefaultSOAPActionPrefix + "eMIP_ToIOP_HeartBeatV1/",
                                                     RequestLogDelegate:   OnSendHeartbeatSOAPRequest,
                                                     ResponseLogDelegate:  OnSendHeartbeatSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, httpResponse, onexception) =>
                                                                                                              HeartbeatResponse.Parse(request,
                                                                                                                                      xml,
                                                                                                                                      CustomHeartbeatParser,
                                                                                                                                      httpResponse,
                                                                                                                                      onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return HTTPResponse<HeartbeatResponse>.IsFault(
                                                                    httpresponse,
                                                                    new HeartbeatResponse(
                                                                        Request,
                                                                        Request.TransactionId ?? Transaction_Id.Zero,
                                                                        RequestStatus.DataError,
                                                                        httpresponse
                                                                    )
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

                                                             return HTTPResponse<HeartbeatResponse>.IsFault(
                                                                        httpresponse,
                                                                        new HeartbeatResponse(
                                                                            Request,
                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                            RequestStatus.HTTPError,
                                                                            httpresponse
                                                                        )
                                                                    );


                                                         return HTTPResponse<HeartbeatResponse>.IsFault(
                                                                    httpresponse,
                                                                    new HeartbeatResponse(
                                                                        Request,
                                                                        Request.TransactionId ?? Transaction_Id.Zero,
                                                                        RequestStatus.SystemError,
                                                                        httpresponse
                                                                    )
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
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.TransactionId,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnSendHeartbeatResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SetServiceAuthorisation    (Request)

        /// <summary>
        /// Send the given SetServiceAuthorisation request.
        /// </summary>
        /// <param name="Request">A SetServiceAuthorisation request.</param>
        public async Task<HTTPResponse<SetServiceAuthorisationResponse>>

            SetServiceAuthorisation(SetServiceAuthorisationRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SetServiceAuthorisation request must not be null!");

            Request = _CustomSetServiceAuthorisationRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SetServiceAuthorisation request must not be null!");


            Byte                            TransmissionRetry  = 0;
            HTTPResponse<SetServiceAuthorisationResponse> result             = null;

            #endregion

            #region Send OnSetServiceAuthorisationRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnSetServiceAuthorisationRequest != null)
                    await Task.WhenAll(OnSetServiceAuthorisationRequest.GetInvocationList().
                                       Cast<OnSetServiceAuthorisationRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.EVSEId,
                                                     Request.UserId,
                                                     Request.RequestedServiceId,
                                                     Request.AuthorisationValue,
                                                     Request.IntermediateCDRRequested,

                                                     Request.TransactionId,
                                                     Request.PartnerServiceSessionId,
                                                     Request.UserContractIdAlias,
                                                     Request.MeterLimits,
                                                     Request.Parameter,
                                                     Request.BookingId,
                                                     Request.SalePartnerBookingId,

                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnSetServiceAuthorisationRequest));
            }

            #endregion


            do
            {

                using (var _eMIPClient = new SOAPClient(RemoteURL,
                                                        VirtualHostname,
                                                        false,
                                                        Description,
                                                        PreferIPv4,
                                                        RemoteCertificateValidator,
                                                        ClientCertificateSelector,
                                                        ClientCert,
                                                        TLSProtocol,
                                                        HTTPUserAgent,
                                                        URLPathPrefix,
                                                        null,
                                                        null,
                                                        RequestTimeout,
                                                        TransmissionRetryDelay,
                                                        MaxNumberOfRetries,
                                                        InternalBufferSize,
                                                        false,
                                                        false,
                                                        null,
                                                        DNSClient))
                {

                    result = await _eMIPClient.Query(_CustomSetServiceAuthorisationSOAPRequestMapper(Request,
                                                                                                     SOAP.Encapsulation(Request.ToXML(CustomSetServiceAuthorisationRequestSerializer))),
                                                     DefaultSOAPActionPrefix + "eMIP_ToIOP_SetServiceAuthorisationV1/",
                                                     RequestLogDelegate:   OnSetServiceAuthorisationSOAPRequest,
                                                     ResponseLogDelegate:  OnSetServiceAuthorisationSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, httpResonse, onexception) =>
                                                                                                              SetServiceAuthorisationResponse.Parse(request,
                                                                                                                                                    xml,
                                                                                                                                                    CustomSetServiceAuthorisationParser,
                                                                                                                                                    httpResonse,
                                                                                                                                                    onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return HTTPResponse<SetServiceAuthorisationResponse>.IsFault(
                                                                    httpresponse,
                                                                    new SetServiceAuthorisationResponse(
                                                                        Request,
                                                                        Request.TransactionId ?? Transaction_Id.Zero,
                                                                        RequestStatus.DataError,
                                                                        ServiceSession_Id.Zero,
                                                                        null,
                                                                        httpresponse
                                                                    )
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

                                                             return HTTPResponse<SetServiceAuthorisationResponse>.IsFault(
                                                                        httpresponse,
                                                                        new SetServiceAuthorisationResponse(
                                                                            Request,
                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                            RequestStatus.HTTPError,
                                                                            ServiceSession_Id.Zero,
                                                                            null,
                                                                            httpresponse
                                                                        )
                                                                    );


                                                         return HTTPResponse<SetServiceAuthorisationResponse>.IsFault(
                                                                    httpresponse,
                                                                    new SetServiceAuthorisationResponse(
                                                                        Request,
                                                                        Request.TransactionId ?? Transaction_Id.Zero,
                                                                        RequestStatus.SystemError,
                                                                        ServiceSession_Id.Zero,
                                                                        null,
                                                                        httpresponse
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<SetServiceAuthorisationResponse>.ExceptionThrown(

                                                                new SetServiceAuthorisationResponse(
                                                                    Request,
                                                                    Request.TransactionId ?? Transaction_Id.Zero,
                                                                    RequestStatus.ServiceNotAvailable,
                                                                    ServiceSession_Id.Zero
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
                    result = HTTPResponse<SetServiceAuthorisationResponse>.OK(
                                 new SetServiceAuthorisationResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.SystemError,
                                     ServiceSession_Id.Zero
                                 //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnSetServiceAuthorisationResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnSetServiceAuthorisationResponse != null)
                    await Task.WhenAll(OnSetServiceAuthorisationResponse.GetInvocationList().
                                       Cast<OnSetServiceAuthorisationResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.EVSEId,
                                                     Request.UserId,
                                                     Request.RequestedServiceId,
                                                     Request.AuthorisationValue,
                                                     Request.IntermediateCDRRequested,

                                                     Request.TransactionId,
                                                     Request.PartnerServiceSessionId,
                                                     Request.UserContractIdAlias,
                                                     Request.MeterLimits,
                                                     Request.Parameter,
                                                     Request.BookingId,
                                                     Request.SalePartnerBookingId,

                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnSetServiceAuthorisationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        // ToIOP_SetAuthenticationData

        #region SetSessionActionRequest    (Request)

        /// <summary>
        /// Send the given SetSessionActionRequest request.
        /// </summary>
        /// <param name="Request">A SetSessionActionRequest request.</param>
        public async Task<HTTPResponse<SetSessionActionResponse>>

            SetSessionAction(SetSessionActionRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SetSessionActionRequest request must not be null!");

            Request = _CustomSetSessionActionRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SetSessionActionRequest request must not be null!");


            Byte                                   TransmissionRetry  = 0;
            HTTPResponse<SetSessionActionResponse> result             = null;

            #endregion

            #region Send OnSetSessionActionRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnSetSessionActionRequest != null)
                    await Task.WhenAll(OnSetSessionActionRequest.GetInvocationList().
                                       Cast<OnSetSessionActionRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.ServiceSessionId,
                                                     Request.SessionAction,

                                                     Request.TransactionId,
                                                     Request.SalePartnerSessionId,

                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnSetSessionActionRequest));
            }

            #endregion


            do
            {

                using (var _eMIPClient = new SOAPClient(RemoteURL,
                                                        VirtualHostname,
                                                        false,
                                                        Description,
                                                        PreferIPv4,
                                                        RemoteCertificateValidator,
                                                        ClientCertificateSelector,
                                                        ClientCert,
                                                        TLSProtocol,
                                                        HTTPUserAgent,
                                                        URLPathPrefix,
                                                        null,
                                                        null,
                                                        RequestTimeout,
                                                        TransmissionRetryDelay,
                                                        MaxNumberOfRetries,
                                                        InternalBufferSize,
                                                        false,
                                                        false,
                                                        null,
                                                        DNSClient))
                {

                    result = await _eMIPClient.Query(_CustomSetSessionActionSOAPRequestMapper(Request,
                                                                                              SOAP.Encapsulation(Request.ToXML(CustomSetSessionActionRequestSerializer))),
                                                     DefaultSOAPActionPrefix + "eMIP_ToIOP_SetSessionActionRequestV1/",
                                                     RequestLogDelegate:   OnSetSessionActionSOAPRequest,
                                                     ResponseLogDelegate:  OnSetSessionActionSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, httpResonse, onexception) =>
                                                                                                              SetSessionActionResponse.Parse(request,
                                                                                                                                             xml,
                                                                                                                                             CustomSetSessionActionParser,
                                                                                                                                             httpResonse,
                                                                                                                                             onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return HTTPResponse<SetSessionActionResponse>.IsFault(
                                                                    httpresponse,
                                                                    new SetSessionActionResponse(
                                                                        Request,
                                                                        Request.TransactionId ?? Transaction_Id.Zero,
                                                                        RequestStatus.DataError,
                                                                        ServiceSession_Id.Zero,
                                                                        SessionAction_Id.Zero,
                                                                        httpresponse
                                                                    )
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

                                                             return HTTPResponse<SetSessionActionResponse>.IsFault(
                                                                        httpresponse,
                                                                        new SetSessionActionResponse(
                                                                            Request,
                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                            RequestStatus.HTTPError,
                                                                            ServiceSession_Id.Zero,
                                                                            SessionAction_Id.Zero,
                                                                            httpresponse
                                                                        )
                                                                    );


                                                         return HTTPResponse<SetSessionActionResponse>.IsFault(
                                                                    httpresponse,
                                                                    new SetSessionActionResponse(
                                                                        Request,
                                                                        Request.TransactionId ?? Transaction_Id.Zero,
                                                                        RequestStatus.SystemError,
                                                                        ServiceSession_Id.Zero,
                                                                        SessionAction_Id.Zero,
                                                                        httpresponse
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<SetSessionActionResponse>.ExceptionThrown(

                                                                new SetSessionActionResponse(
                                                                    Request,
                                                                    Request.TransactionId ?? Transaction_Id.Zero,
                                                                    RequestStatus.ServiceNotAvailable,
                                                                    ServiceSession_Id.Zero,
                                                                    SessionAction_Id.Zero
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
                    result = HTTPResponse<SetSessionActionResponse>.OK(
                                 new SetSessionActionResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.SystemError,
                                     ServiceSession_Id.Zero,
                                     SessionAction_Id.Zero
                                 //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnSetSessionActionResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnSetSessionActionResponse != null)
                    await Task.WhenAll(OnSetSessionActionResponse.GetInvocationList().
                                       Cast<OnSetSessionActionResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.ServiceSessionId,
                                                     Request.SessionAction,

                                                     Request.TransactionId,
                                                     Request.SalePartnerSessionId,

                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnSetSessionActionResponse));
            }

            #endregion

            return result;

        }

        #endregion


    }

}
