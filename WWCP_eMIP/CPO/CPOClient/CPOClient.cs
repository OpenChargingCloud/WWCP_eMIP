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

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// An eMIP CPO client.
    /// </summary>
    public partial class CPOClient : ASOAPClient,
                                     ICPOClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String   DefaultHTTPUserAgent        = "GraphDefined eMIP " + Version.Number + " CPO Client";

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
        /// The attached eMIP CPO client (HTTP/SOAP client) logger.
        /// </summary>
        public CPOClientLogger  Logger   { get; }

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


        #region CustomSetEVSEAvailabilityStatusRequestMapper

        #region CustomSetEVSEAvailabilityStatusRequestMapper

        private Func<SetEVSEAvailabilityStatusRequest, SetEVSEAvailabilityStatusRequest> _CustomSetEVSEAvailabilityStatusRequestMapper = _ => _;

        public Func<SetEVSEAvailabilityStatusRequest, SetEVSEAvailabilityStatusRequest> CustomSetEVSEAvailabilityStatusRequestMapper
        {

            get
            {
                return _CustomSetEVSEAvailabilityStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetEVSEAvailabilityStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetEVSEAvailabilityStatusSOAPRequestMapper

        private Func<SetEVSEAvailabilityStatusRequest, XElement, XElement> _CustomSetEVSEAvailabilityStatusSOAPRequestMapper = (request, xml) => xml;

        public Func<SetEVSEAvailabilityStatusRequest, XElement, XElement> CustomSetEVSEAvailabilityStatusSOAPRequestMapper
        {

            get
            {
                return _CustomSetEVSEAvailabilityStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetEVSEAvailabilityStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetEVSEAvailabilityStatusResponse> CustomSetEVSEAvailabilityStatusParser   { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<SetEVSEAvailabilityStatusRequest>  CustomSetEVSEAvailabilityStatusRequestSerializer    { get; set; }


        #region CustomSetEVSEBusyStatusRequestMapper

        #region CustomSetEVSEBusyStatusRequestMapper

        private Func<SetEVSEBusyStatusRequest, SetEVSEBusyStatusRequest> _CustomSetEVSEBusyStatusRequestMapper = _ => _;

        public Func<SetEVSEBusyStatusRequest, SetEVSEBusyStatusRequest> CustomSetEVSEBusyStatusRequestMapper
        {

            get
            {
                return _CustomSetEVSEBusyStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetEVSEBusyStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetEVSEBusyStatusSOAPRequestMapper

        private Func<SetEVSEBusyStatusRequest, XElement, XElement> _CustomSetEVSEBusyStatusSOAPRequestMapper = (request, xml) => xml;

        public Func<SetEVSEBusyStatusRequest, XElement, XElement> CustomSetEVSEBusyStatusSOAPRequestMapper
        {

            get
            {
                return _CustomSetEVSEBusyStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetEVSEBusyStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetEVSEBusyStatusResponse> CustomSetEVSEBusyStatusParser   { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<SetEVSEBusyStatusRequest>          CustomSetEVSEBusyStatusRequestSerializer            { get; set; }


        #region CustomGetServiceAuthorisationRequestMapper

        #region CustomGetServiceAuthorisationRequestMapper

        private Func<GetServiceAuthorisationRequest, GetServiceAuthorisationRequest> _CustomGetServiceAuthorisationRequestMapper = _ => _;

        public Func<GetServiceAuthorisationRequest, GetServiceAuthorisationRequest> CustomGetServiceAuthorisationRequestMapper
        {

            get
            {
                return _CustomGetServiceAuthorisationRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetServiceAuthorisationRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetServiceAuthorisationSOAPRequestMapper

        private Func<GetServiceAuthorisationRequest, XElement, XElement> _CustomGetServiceAuthorisationSOAPRequestMapper = (request, xml) => xml;

        public Func<GetServiceAuthorisationRequest, XElement, XElement> CustomGetServiceAuthorisationSOAPRequestMapper
        {

            get
            {
                return _CustomGetServiceAuthorisationSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetServiceAuthorisationSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<GetServiceAuthorisationResponse> CustomGetServiceAuthorisationParser { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<GetServiceAuthorisationRequest>    CustomGetServiceAuthorisationRequestSerializer      { get; set; }


        #region CustomSetChargeDetailRecordRequestMapper

        #region CustomSetChargeDetailRecordRequestMapper

        private Func<SetChargeDetailRecordRequest, SetChargeDetailRecordRequest> _CustomSetChargeDetailRecordRequestMapper = _ => _;

        public Func<SetChargeDetailRecordRequest, SetChargeDetailRecordRequest> CustomSetChargeDetailRecordRequestMapper
        {

            get
            {
                return _CustomSetChargeDetailRecordRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetChargeDetailRecordRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetChargeDetailRecordSOAPRequestMapper

        private Func<SetChargeDetailRecordRequest, XElement, XElement> _CustomSetChargeDetailRecordSOAPRequestMapper = (request, xml) => xml;

        public Func<SetChargeDetailRecordRequest, XElement, XElement> CustomSetChargeDetailRecordSOAPRequestMapper
        {

            get
            {
                return _CustomSetChargeDetailRecordSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetChargeDetailRecordSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetChargeDetailRecordResponse> CustomSetChargeDetailRecordParser { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<SetChargeDetailRecordRequest>      CustomSetChargeDetailRecordRequestSerializer        { get; set; }



        public CustomXMLSerializerDelegate<ChargeDetailRecord>                CustomChargeDetailRecordSerializer                  { get; set; }
        public CustomXMLParserDelegate<ChargeDetailRecord>                    CustomChargeDetailRecordParser                      { get; set; }

        public CustomXMLSerializerDelegate<MeterReport>                       CustomMeterReportSerializer                         { get; set; }
        public CustomXMLParserDelegate<MeterReport>                           CustomMeterReportParser                             { get; set; }

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


        #region OnSetEVSEAvailabilityStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending an EVSE availability status will be send.
        /// </summary>
        public event OnSetEVSEAvailabilityStatusRequestDelegate   OnSetEVSEAvailabilityStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending an EVSE availability status will be send.
        /// </summary>
        public event ClientRequestLogHandler                      OnSetEVSEAvailabilityStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an EVSE availability status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                     OnSetEVSEAvailabilityStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an EVSE availability status request had been received.
        /// </summary>
        public event OnSetEVSEAvailabilityStatusResponseDelegate  OnSetEVSEAvailabilityStatusResponse;

        #endregion

        #region OnSetEVSEBusyStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending an EVSE busy status will be send.
        /// </summary>
        public event OnSetEVSEBusyStatusRequestDelegate   OnSetEVSEBusyStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending an EVSE busy status will be send.
        /// </summary>
        public event ClientRequestLogHandler              OnSetEVSEBusyStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an EVSE busy status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler             OnSetEVSEBusyStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an EVSE busy status request had been received.
        /// </summary>
        public event OnSetEVSEBusyStatusResponseDelegate  OnSetEVSEBusyStatusResponse;

        #endregion


        #region OnGetServiceAuthorisationRequest/-Response

        /// <summary>
        /// An event fired whenever a service authorisation request will be send.
        /// </summary>
        public event OnGetServiceAuthorisationRequestDelegate   OnGetServiceAuthorisationRequest;

        /// <summary>
        /// An event fired whenever a SOAP service authorisation request will be send.
        /// </summary>
        public event ClientRequestLogHandler                    OnGetServiceAuthorisationSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a service authorisation SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                   OnGetServiceAuthorisationSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a service authorisation request had been received.
        /// </summary>
        public event OnGetServiceAuthorisationResponseDelegate  OnGetServiceAuthorisationResponse;

        #endregion

        #region OnSetChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record will be send.
        /// </summary>
        public event OnSetChargeDetailRecordRequestDelegate   OnSetChargeDetailRecordRequest;

        /// <summary>
        /// An event fired whenever a SOAP charge detail record will be send.
        /// </summary>
        public event ClientRequestLogHandler                  OnSetChargeDetailRecordSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a SOAP charge detail record had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnSetChargeDetailRecordSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a charge detail record had been received.
        /// </summary>
        public event OnSetChargeDetailRecordResponseDelegate  OnSetChargeDetailRecordResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region CPOClient(ClientId, Hostname, ..., LoggingContext = CPOClientLogger.DefaultContext, ...)

        /// <summary>
        /// Create a new eMIP CPO Client.
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
        public CPOClient(String                               ClientId,
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
                         String                               LoggingContext               = CPOClientLogger.DefaultContext,
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

            this.Logger  = new CPOClientLogger(this,
                                               LoggingContext,
                                               LogfileCreator);

        }

        #endregion

        #region CPOClient(ClientId, Logger, Hostname, ...)

        /// <summary>
        /// Create a new eMIP CPO Client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Logger">A CPO client logger.</param>
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
        public CPOClient(String                               ClientId,
                         CPOClientLogger                      Logger,
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
                e.Log(nameof(CPOClient) + "." + nameof(OnSendHeartbeatRequest));
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
                e.Log(nameof(CPOClient) + "." + nameof(OnSendHeartbeatResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SetEVSEAvailabilityStatus(Request)

        /// <summary>
        /// Send the given EVSE busy status.
        /// </summary>
        /// <param name="Request">A SetEVSEAvailabilityStatus request.</param>
        public async Task<HTTPResponse<SetEVSEAvailabilityStatusResponse>>

            SetEVSEAvailabilityStatus(SetEVSEAvailabilityStatusRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SetEVSEAvailabilityStatus request must not be null!");

            Request = _CustomSetEVSEAvailabilityStatusRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SetEVSEAvailabilityStatus request must not be null!");


            Byte                                    TransmissionRetry  = 0;
            HTTPResponse<SetEVSEAvailabilityStatusResponse> result             = null;

            #endregion

            #region Send OnSetEVSEAvailabilityStatusRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnSetEVSEAvailabilityStatusRequest != null)
                    await Task.WhenAll(OnSetEVSEAvailabilityStatusRequest.GetInvocationList().
                                       Cast<OnSetEVSEAvailabilityStatusRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.EVSEId,
                                                     Request.StatusEventDate,
                                                     Request.AvailabilityStatus,
                                                     Request.TransactionId,
                                                     Request.AvailabilityStatusUntil,
                                                     Request.AvailabilityStatusComment,

                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetEVSEAvailabilityStatusRequest));
            }

            #endregion



            if (!Request.EVSEId.ToString().StartsWith("DE*BDO*E666181358*") &&
                !Request.EVSEId.ToString().StartsWith("DE*BDO*EVSE*CI*TESTS"))
                    result = HTTPResponse<SetEVSEAvailabilityStatusResponse>.OK(
                                 new SetEVSEAvailabilityStatusResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.ServiceNotAvailable
                                     //"HTTP request failed!"
                                 )
                             );

            else

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

                    result = await _eMIPClient.Query(_CustomSetEVSEAvailabilityStatusSOAPRequestMapper(Request,
                                                                                                       SOAP.Encapsulation(Request.ToXML(CustomSetEVSEAvailabilityStatusRequestSerializer))),
                                                     DefaultSOAPActionPrefix + "eMIP_ToIOP_SetEVSEAvailabilityStatusV1/",
                                                     RequestLogDelegate:   OnSetEVSEAvailabilityStatusSOAPRequest,
                                                     ResponseLogDelegate:  OnSetEVSEAvailabilityStatusSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              SetEVSEAvailabilityStatusResponse.Parse(request,
                                                                                                                                      xml,
                                                                                                                                      CustomSetEVSEAvailabilityStatusParser,
                                                                                                                                      onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<SetEVSEAvailabilityStatusResponse>(

                                                                    httpresponse,

                                                                    new SetEVSEAvailabilityStatusResponse(
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

                                                             return new HTTPResponse<SetEVSEAvailabilityStatusResponse>(httpresponse,
                                                                                                        new SetEVSEAvailabilityStatusResponse(
                                                                                                            Request,
                                                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                                                            RequestStatus.HTTPError
                                                                                                            //httpresponse.HTTPStatusCode.ToString(),
                                                                                                            //httpresponse.HTTPBody.      ToUTF8String()
                                                                                                        ),
                                                                                                        IsFault: true);


                                                         return new HTTPResponse<SetEVSEAvailabilityStatusResponse>(

                                                                    httpresponse,

                                                                    new SetEVSEAvailabilityStatusResponse(
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

                                                         return HTTPResponse<SetEVSEAvailabilityStatusResponse>.ExceptionThrown(

                                                                new SetEVSEAvailabilityStatusResponse(
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
                    result = HTTPResponse<SetEVSEAvailabilityStatusResponse>.OK(
                                 new SetEVSEAvailabilityStatusResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.SystemError
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnSendSetEVSEAvailabilityStatusResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnSetEVSEAvailabilityStatusResponse != null)
                    await Task.WhenAll(OnSetEVSEAvailabilityStatusResponse.GetInvocationList().
                                       Cast<OnSetEVSEAvailabilityStatusResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.EVSEId,
                                                     Request.StatusEventDate,
                                                     Request.AvailabilityStatus,
                                                     Request.TransactionId,
                                                     Request.AvailabilityStatusUntil,
                                                     Request.AvailabilityStatusComment,

                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetEVSEAvailabilityStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SetEVSEBusyStatus        (Request)

        /// <summary>
        /// Send the given EVSE busy status.
        /// </summary>
        /// <param name="Request">A SetEVSEBusyStatus request.</param>
        public async Task<HTTPResponse<SetEVSEBusyStatusResponse>>

            SetEVSEBusyStatus(SetEVSEBusyStatusRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SetEVSEBusyStatus request must not be null!");

            Request = _CustomSetEVSEBusyStatusRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SetEVSEBusyStatus request must not be null!");


            Byte                                    TransmissionRetry  = 0;
            HTTPResponse<SetEVSEBusyStatusResponse> result             = null;

            #endregion

            #region Send OnSetEVSEBusyStatusRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnSetEVSEBusyStatusRequest != null)
                    await Task.WhenAll(OnSetEVSEBusyStatusRequest.GetInvocationList().
                                       Cast<OnSetEVSEBusyStatusRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.EVSEId,
                                                     Request.StatusEventDate,
                                                     Request.BusyStatus,
                                                     Request.TransactionId,
                                                     Request.BusyStatusUntil,
                                                     Request.BusyStatusComment,

                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetEVSEBusyStatusRequest));
            }

            #endregion


            if (!Request.EVSEId.ToString().StartsWith("DE*BDO*E666181358*") &&
                !Request.EVSEId.ToString().StartsWith("DE*BDO*EVSE*CI*TESTS"))
                    result = HTTPResponse<SetEVSEBusyStatusResponse>.OK(
                                 new SetEVSEBusyStatusResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.ServiceNotAvailable
                                     //"HTTP request failed!"
                                 )
                             );

            else

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

                    result = await _eMIPClient.Query(_CustomSetEVSEBusyStatusSOAPRequestMapper(Request,
                                                                                               SOAP.Encapsulation(Request.ToXML(CustomSetEVSEBusyStatusRequestSerializer))),
                                                     "eMIP_ToIOP_SetEVSEBusyStatusV1/",
                                                     RequestLogDelegate:   OnSetEVSEBusyStatusSOAPRequest,
                                                     ResponseLogDelegate:  OnSetEVSEBusyStatusSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              SetEVSEBusyStatusResponse.Parse(request,
                                                                                                                                      xml,
                                                                                                                                      CustomSetEVSEBusyStatusParser,
                                                                                                                                      onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<SetEVSEBusyStatusResponse>(

                                                                    httpresponse,

                                                                    new SetEVSEBusyStatusResponse(
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

                                                             return new HTTPResponse<SetEVSEBusyStatusResponse>(httpresponse,
                                                                                                        new SetEVSEBusyStatusResponse(
                                                                                                            Request,
                                                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                                                            RequestStatus.HTTPError
                                                                                                            //httpresponse.HTTPStatusCode.ToString(),
                                                                                                            //httpresponse.HTTPBody.      ToUTF8String()
                                                                                                        ),
                                                                                                        IsFault: true);


                                                         return new HTTPResponse<SetEVSEBusyStatusResponse>(

                                                                    httpresponse,

                                                                    new SetEVSEBusyStatusResponse(
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

                                                         return HTTPResponse<SetEVSEBusyStatusResponse>.ExceptionThrown(

                                                                new SetEVSEBusyStatusResponse(
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
                    result = HTTPResponse<SetEVSEBusyStatusResponse>.OK(
                                 new SetEVSEBusyStatusResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.SystemError
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnSendSetEVSEBusyStatusResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnSetEVSEBusyStatusResponse != null)
                    await Task.WhenAll(OnSetEVSEBusyStatusResponse.GetInvocationList().
                                       Cast<OnSetEVSEBusyStatusResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.EVSEId,
                                                     Request.StatusEventDate,
                                                     Request.BusyStatus,
                                                     Request.TransactionId,
                                                     Request.BusyStatusUntil,
                                                     Request.BusyStatusComment,

                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetEVSEBusyStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region GetServiceAuthorisation  (Request)

        /// <summary>
        /// Request an service authorisation.
        /// </summary>
        /// <param name="Request">A GetServiceAuthorisation request.</param>
        public async Task<HTTPResponse<GetServiceAuthorisationResponse>>

            GetServiceAuthorisation(GetServiceAuthorisationRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given GetServiceAuthorisation request must not be null!");

            Request = _CustomGetServiceAuthorisationRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped GetServiceAuthorisation request must not be null!");


            Byte                                          TransmissionRetry  = 0;
            HTTPResponse<GetServiceAuthorisationResponse> result             = null;

            #endregion

            #region Send OnGetServiceAuthorisationRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnGetServiceAuthorisationRequest != null)
                    await Task.WhenAll(OnGetServiceAuthorisationRequest.GetInvocationList().
                                       Cast<OnGetServiceAuthorisationRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.EVSEId,
                                                     Request.UserId,
                                                     Request.RequestedServiceId,
                                                     Request.TransactionId,
                                                     Request.PartnerServiceSessionId,

                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetServiceAuthorisationRequest));
            }

            #endregion


            if (!Request.EVSEId.ToString().StartsWith("DE*BDO*E666181358*") &&
                !Request.EVSEId.ToString().StartsWith("DE*BDO*EVSE*CI*TESTS"))
                    result = HTTPResponse<GetServiceAuthorisationResponse>.OK(
                                 new GetServiceAuthorisationResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.ServiceNotAvailable
                                     //"HTTP request failed!"
                                 )
                             );

            else

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

                    result = await _eMIPClient.Query(_CustomGetServiceAuthorisationSOAPRequestMapper(Request,
                                                                                                     SOAP.Encapsulation(Request.ToXML(CustomGetServiceAuthorisationRequestSerializer))),
                                                     DefaultSOAPActionPrefix + "eMIP_ToIOP_GetServiceAuthorisationV1/",
                                                     RequestLogDelegate:   OnGetServiceAuthorisationSOAPRequest,
                                                     ResponseLogDelegate:  OnGetServiceAuthorisationSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              GetServiceAuthorisationResponse.Parse(request,
                                                                                                                                                    xml,
                                                                                                                                                    CustomGetServiceAuthorisationParser,
                                                                                                                                                    CustomMeterReportParser,
                                                                                                                                                    onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<GetServiceAuthorisationResponse>(

                                                                    httpresponse,

                                                                    new GetServiceAuthorisationResponse(
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

                                                             return new HTTPResponse<GetServiceAuthorisationResponse>(httpresponse,
                                                                                                        new GetServiceAuthorisationResponse(
                                                                                                            Request,
                                                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                                                            RequestStatus.HTTPError
                                                                                                            //httpresponse.HTTPStatusCode.ToString(),
                                                                                                            //httpresponse.HTTPBody.      ToUTF8String()
                                                                                                        ),
                                                                                                        IsFault: true);


                                                         return new HTTPResponse<GetServiceAuthorisationResponse>(

                                                                    httpresponse,

                                                                    new GetServiceAuthorisationResponse(
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

                                                         return HTTPResponse<GetServiceAuthorisationResponse>.ExceptionThrown(

                                                                new GetServiceAuthorisationResponse(
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
                    result = HTTPResponse<GetServiceAuthorisationResponse>.OK(
                                 new GetServiceAuthorisationResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.SystemError
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnSendGetServiceAuthorisationResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnGetServiceAuthorisationResponse != null)
                    await Task.WhenAll(OnGetServiceAuthorisationResponse.GetInvocationList().
                                       Cast<OnGetServiceAuthorisationResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.EVSEId,
                                                     Request.UserId,
                                                     Request.RequestedServiceId,
                                                     Request.TransactionId,
                                                     Request.PartnerServiceSessionId,

                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetServiceAuthorisationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SetChargeDetailRecord    (Request)

        /// <summary>
        /// Upload the given charge detail record.
        /// </summary>
        /// <param name="Request">A SetChargeDetailRecord request.</param>
        public async Task<HTTPResponse<SetChargeDetailRecordResponse>>

            SetChargeDetailRecord(SetChargeDetailRecordRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SetChargeDetailRecord request must not be null!");

            Request = _CustomSetChargeDetailRecordRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SetChargeDetailRecord request must not be null!");


            Byte                                        TransmissionRetry  = 0;
            HTTPResponse<SetChargeDetailRecordResponse> result             = null;

            #endregion

            #region Send OnSetChargeDetailRecordRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnSetChargeDetailRecordRequest != null)
                    await Task.WhenAll(OnSetChargeDetailRecordRequest.GetInvocationList().
                                       Cast<OnSetChargeDetailRecordRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.ChargeDetailRecord,
                                                     Request.TransactionId,

                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetChargeDetailRecordRequest));
            }

            #endregion


            if (!Request.ChargeDetailRecord.EVSEId.ToString().StartsWith("DE*BDO*E666181358*") &&
                !Request.ChargeDetailRecord.EVSEId.ToString().StartsWith("DE*BDO*EVSE*CI*TESTS"))
                    result = HTTPResponse<SetChargeDetailRecordResponse>.OK(
                                 new SetChargeDetailRecordResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.ServiceNotAvailable
                                     //"HTTP request failed!"
                                 )
                             );

            else

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

                    result = await _eMIPClient.Query(_CustomSetChargeDetailRecordSOAPRequestMapper(Request,
                                                                                                   SOAP.Encapsulation(Request.ToXML(CustomSetChargeDetailRecordRequestSerializer,
                                                                                                                                    CustomChargeDetailRecordSerializer,
                                                                                                                                    CustomMeterReportSerializer))),
                                                     DefaultSOAPActionPrefix + "eMIP_ToIOP_SetChargeDetailRecordV1/",
                                                     RequestLogDelegate:   OnSetChargeDetailRecordSOAPRequest,
                                                     ResponseLogDelegate:  OnSetChargeDetailRecordSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              SetChargeDetailRecordResponse.Parse(request,
                                                                                                                                                  xml,
                                                                                                                                                  CustomSetChargeDetailRecordParser,
                                                                                                                                                  onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<SetChargeDetailRecordResponse>(

                                                                    httpresponse,

                                                                    new SetChargeDetailRecordResponse(
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

                                                             return new HTTPResponse<SetChargeDetailRecordResponse>(httpresponse,
                                                                                                        new SetChargeDetailRecordResponse(
                                                                                                            Request,
                                                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                                                            RequestStatus.HTTPError
                                                                                                            //httpresponse.HTTPStatusCode.ToString(),
                                                                                                            //httpresponse.HTTPBody.      ToUTF8String()
                                                                                                        ),
                                                                                                        IsFault: true);


                                                         return new HTTPResponse<SetChargeDetailRecordResponse>(

                                                                    httpresponse,

                                                                    new SetChargeDetailRecordResponse(
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

                                                         return HTTPResponse<SetChargeDetailRecordResponse>.ExceptionThrown(

                                                                new SetChargeDetailRecordResponse(
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
                    result = HTTPResponse<SetChargeDetailRecordResponse>.OK(
                                 new SetChargeDetailRecordResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.SystemError
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnSendSetChargeDetailRecordResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnSetChargeDetailRecordResponse != null)
                    await Task.WhenAll(OnSetChargeDetailRecordResponse.GetInvocationList().
                                       Cast<OnSetChargeDetailRecordResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.ChargeDetailRecord,
                                                     Request.TransactionId,

                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetChargeDetailRecordResponse));
            }

            #endregion

            return result;

        }

        #endregion


    }

}
