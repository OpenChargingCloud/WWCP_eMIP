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
        public new const           String    DefaultHTTPUserAgent        = "GraphDefined eMIP " + Version.Number + " CPO Client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort    DefaultRemotePort           = IPPort.Parse(443);

        /// <summary>
        /// The default URI prefix.
        /// </summary>
        public new static readonly HTTPPath  DefaultURIPrefix            = HTTPPath.Parse("/api/emip");


        /// <summary>
        /// The default SOAP action prefix.
        /// </summary>
        public     const           String    DefaultSOAPActionPrefix     = "https://api-iop.gireve.com/services/";

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

        public CustomXMLSerializerDelegate<HeartbeatRequest>                              CustomHeartbeatRequestSerializer                               { get; set; }



        #region CustomSetChargingPoolAvailabilityStatusRequestMapper

        #region CustomSetChargingPoolAvailabilityStatusRequestMapper

        private Func<SetChargingPoolAvailabilityStatusRequest, SetChargingPoolAvailabilityStatusRequest> _CustomSetChargingPoolAvailabilityStatusRequestMapper = _ => _;

        public Func<SetChargingPoolAvailabilityStatusRequest, SetChargingPoolAvailabilityStatusRequest> CustomSetChargingPoolAvailabilityStatusRequestMapper
        {

            get
            {
                return _CustomSetChargingPoolAvailabilityStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetChargingPoolAvailabilityStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetChargingPoolAvailabilityStatusSOAPRequestMapper

        private Func<SetChargingPoolAvailabilityStatusRequest, XElement, XElement> _CustomSetChargingPoolAvailabilityStatusSOAPRequestMapper = (request, xml) => xml;

        public Func<SetChargingPoolAvailabilityStatusRequest, XElement, XElement> CustomSetChargingPoolAvailabilityStatusSOAPRequestMapper
        {

            get
            {
                return _CustomSetChargingPoolAvailabilityStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetChargingPoolAvailabilityStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetChargingPoolAvailabilityStatusResponse> CustomSetChargingPoolAvailabilityStatusParser   { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<SetChargingPoolAvailabilityStatusRequest>      CustomSetChargingPoolAvailabilityStatusRequestSerializer       { get; set; }


        #region CustomSetChargingStationAvailabilityStatusRequestMapper

        #region CustomSetChargingStationAvailabilityStatusRequestMapper

        private Func<SetChargingStationAvailabilityStatusRequest, SetChargingStationAvailabilityStatusRequest> _CustomSetChargingStationAvailabilityStatusRequestMapper = _ => _;

        public Func<SetChargingStationAvailabilityStatusRequest, SetChargingStationAvailabilityStatusRequest> CustomSetChargingStationAvailabilityStatusRequestMapper
        {

            get
            {
                return _CustomSetChargingStationAvailabilityStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetChargingStationAvailabilityStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetChargingStationAvailabilityStatusSOAPRequestMapper

        private Func<SetChargingStationAvailabilityStatusRequest, XElement, XElement> _CustomSetChargingStationAvailabilityStatusSOAPRequestMapper = (request, xml) => xml;

        public Func<SetChargingStationAvailabilityStatusRequest, XElement, XElement> CustomSetChargingStationAvailabilityStatusSOAPRequestMapper
        {

            get
            {
                return _CustomSetChargingStationAvailabilityStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetChargingStationAvailabilityStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetChargingStationAvailabilityStatusResponse> CustomSetChargingStationAvailabilityStatusParser   { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<SetChargingStationAvailabilityStatusRequest>   CustomSetChargingStationAvailabilityStatusRequestSerializer    { get; set; }


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

        public CustomXMLSerializerDelegate<SetEVSEAvailabilityStatusRequest>              CustomSetEVSEAvailabilityStatusRequestSerializer               { get; set; }


        #region CustomSetChargingConnectorAvailabilityStatusRequestMapper

        #region CustomSetChargingConnectorAvailabilityStatusRequestMapper

        private Func<SetChargingConnectorAvailabilityStatusRequest, SetChargingConnectorAvailabilityStatusRequest> _CustomSetChargingConnectorAvailabilityStatusRequestMapper = _ => _;

        public Func<SetChargingConnectorAvailabilityStatusRequest, SetChargingConnectorAvailabilityStatusRequest> CustomSetChargingConnectorAvailabilityStatusRequestMapper
        {

            get
            {
                return _CustomSetChargingConnectorAvailabilityStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetChargingConnectorAvailabilityStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetChargingConnectorAvailabilityStatusSOAPRequestMapper

        private Func<SetChargingConnectorAvailabilityStatusRequest, XElement, XElement> _CustomSetChargingConnectorAvailabilityStatusSOAPRequestMapper = (request, xml) => xml;

        public Func<SetChargingConnectorAvailabilityStatusRequest, XElement, XElement> CustomSetChargingConnectorAvailabilityStatusSOAPRequestMapper
        {

            get
            {
                return _CustomSetChargingConnectorAvailabilityStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetChargingConnectorAvailabilityStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetChargingConnectorAvailabilityStatusResponse> CustomSetChargingConnectorAvailabilityStatusParser { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<SetChargingConnectorAvailabilityStatusRequest> CustomSetChargingConnectorAvailabilityStatusRequestSerializer  { get; set; }



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

        public CustomXMLSerializerDelegate<SetEVSEBusyStatusRequest>                      CustomSetEVSEBusyStatusRequestSerializer                       { get; set; }


        #region CustomSetEVSESyntheticStatusRequestMapper

        #region CustomSetEVSESyntheticStatusRequestMapper

        private Func<SetEVSESyntheticStatusRequest, SetEVSESyntheticStatusRequest> _CustomSetEVSESyntheticStatusRequestMapper = _ => _;

        public Func<SetEVSESyntheticStatusRequest, SetEVSESyntheticStatusRequest> CustomSetEVSESyntheticStatusRequestMapper
        {

            get
            {
                return _CustomSetEVSESyntheticStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetEVSESyntheticStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetEVSESyntheticStatusSOAPRequestMapper

        private Func<SetEVSESyntheticStatusRequest, XElement, XElement> _CustomSetEVSESyntheticStatusSOAPRequestMapper = (request, xml) => xml;

        public Func<SetEVSESyntheticStatusRequest, XElement, XElement> CustomSetEVSESyntheticStatusSOAPRequestMapper
        {

            get
            {
                return _CustomSetEVSESyntheticStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetEVSESyntheticStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetEVSESyntheticStatusResponse> CustomSetEVSESyntheticStatusParser   { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<SetEVSESyntheticStatusRequest>                 CustomSetEVSESyntheticStatusRequestSerializer                  { get; set; }



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

        public CustomXMLSerializerDelegate<GetServiceAuthorisationRequest>                CustomGetServiceAuthorisationRequestSerializer                 { get; set; }



        #region CustomSetSessionEventReportRequestMapper

        #region CustomSetSessionEventReportRequestMapper

        private Func<SetSessionEventReportRequest, SetSessionEventReportRequest> _CustomSetSessionEventReportRequestMapper = _ => _;

        public Func<SetSessionEventReportRequest, SetSessionEventReportRequest> CustomSetSessionEventReportRequestMapper
        {

            get
            {
                return _CustomSetSessionEventReportRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetSessionEventReportRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetSessionEventReportSOAPRequestMapper

        private Func<SetSessionEventReportRequest, XElement, XElement> _CustomSetSessionEventReportSOAPRequestMapper = (request, xml) => xml;

        public Func<SetSessionEventReportRequest, XElement, XElement> CustomSetSessionEventReportSOAPRequestMapper
        {

            get
            {
                return _CustomSetSessionEventReportSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetSessionEventReportSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetSessionEventReportResponse> CustomSetSessionEventReportParser { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<SetSessionEventReportRequest>                  CustomSetSessionEventReportRequestSerializer                   { get; set; }



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

        public CustomXMLSerializerDelegate<SetChargeDetailRecordRequest>                  CustomSetChargeDetailRecordRequestSerializer                   { get; set; }



        public CustomXMLSerializerDelegate<ChargeDetailRecord>                            CustomChargeDetailRecordSerializer                             { get; set; }
        public CustomXMLParserDelegate<ChargeDetailRecord>                                CustomChargeDetailRecordParser                                 { get; set; }

        public CustomXMLSerializerDelegate<MeterReport>                                   CustomMeterReportSerializer                                    { get; set; }
        public CustomXMLParserDelegate<MeterReport>                                       CustomMeterReportParser                                        { get; set; }

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


        #region OnSetChargingPoolAvailabilityStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a charging pool availability status will be send.
        /// </summary>
        public event OnSetChargingPoolAvailabilityStatusRequestDelegate   OnSetChargingPoolAvailabilityStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a charging pool availability status will be send.
        /// </summary>
        public event ClientRequestLogHandler                              OnSetChargingPoolAvailabilityStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a charging pool availability status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                             OnSetChargingPoolAvailabilityStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a charging pool availability status request had been received.
        /// </summary>
        public event OnSetChargingPoolAvailabilityStatusResponseDelegate  OnSetChargingPoolAvailabilityStatusResponse;

        #endregion

        #region OnSetChargingStationAvailabilityStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a charging station availability status will be send.
        /// </summary>
        public event OnSetChargingStationAvailabilityStatusRequestDelegate   OnSetChargingStationAvailabilityStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a charging station availability status will be send.
        /// </summary>
        public event ClientRequestLogHandler                                 OnSetChargingStationAvailabilityStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a charging station availability status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                                OnSetChargingStationAvailabilityStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a charging station availability status request had been received.
        /// </summary>
        public event OnSetChargingStationAvailabilityStatusResponseDelegate  OnSetChargingStationAvailabilityStatusResponse;

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

        #region OnSetChargingConnectorAvailabilityStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a charging connector availability status will be send.
        /// </summary>
        public event OnSetChargingConnectorAvailabilityStatusRequestDelegate   OnSetChargingConnectorAvailabilityStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a charging connector availability status will be send.
        /// </summary>
        public event ClientRequestLogHandler                                   OnSetChargingConnectorAvailabilityStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a charging connector availability status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                                  OnSetChargingConnectorAvailabilityStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a charging connector availability status request had been received.
        /// </summary>
        public event OnSetChargingConnectorAvailabilityStatusResponseDelegate  OnSetChargingConnectorAvailabilityStatusResponse;

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

        #region OnSetEVSESyntheticStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending an EVSE synthetic status will be send.
        /// </summary>
        public event OnSetEVSESyntheticStatusRequestDelegate   OnSetEVSESyntheticStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending an EVSE synthetic status will be send.
        /// </summary>
        public event ClientRequestLogHandler                   OnSetEVSESyntheticStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an EVSE synthetic status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                  OnSetEVSESyntheticStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an EVSE synthetic status request had been received.
        /// </summary>
        public event OnSetEVSESyntheticStatusResponseDelegate  OnSetEVSESyntheticStatusResponse;

        #endregion


        #region OnGetServiceAuthorisationRequest/-Response

        /// <summary>
        /// An event fired whenever a GetServiceAuthorisation request will be send.
        /// </summary>
        public event OnGetServiceAuthorisationRequestDelegate   OnGetServiceAuthorisationRequest;

        /// <summary>
        /// An event fired whenever a SOAP GetServiceAuthorisation request will be send.
        /// </summary>
        public event ClientRequestLogHandler                    OnGetServiceAuthorisationSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a GetServiceAuthorisationn SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                   OnGetServiceAuthorisationSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a GetServiceAuthorisation request had been received.
        /// </summary>
        public event OnGetServiceAuthorisationResponseDelegate  OnGetServiceAuthorisationResponse;

        #endregion

        #region OnSetSessionEventReportRequest/-Response

        /// <summary>
        /// An event fired whenever a SetSessionEventReport request will be send.
        /// </summary>
        public event OnSetSessionEventReportRequestDelegate   OnSetSessionEventReportRequest;

        /// <summary>
        /// An event fired whenever a SOAP SetSessionEventReport request will be send.
        /// </summary>
        public event ClientRequestLogHandler                  OnSetSessionEventReportSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a SetSessionEventReport SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnSetSessionEventReportSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a SetSessionEventReport request had been received.
        /// </summary>
        public event OnSetSessionEventReportResponseDelegate  OnSetSessionEventReportResponse;

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
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
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
                         HTTPPath?                            URIPrefix                    = null,
                         String                               HTTPUserAgent                = DefaultHTTPUserAgent,
                         TimeSpan?                            RequestTimeout               = null,
                         TransmissionRetryDelayDelegate       TransmissionRetryDelay       = null,
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
                   TransmissionRetryDelay,
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
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPOClient(String                               ClientId,
                         CPOClientLogger                      Logger,
                         HTTPHostname                         Hostname,
                         IPPort?                              RemotePort                   = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                         LocalCertificateSelectionCallback    ClientCertificateSelector    = null,
                         HTTPHostname?                        HTTPVirtualHost              = null,
                         HTTPPath?                            URIPrefix                    = null,
                         String                               HTTPUserAgent                = DefaultHTTPUserAgent,
                         TimeSpan?                            RequestTimeout               = null,
                         TransmissionRetryDelayDelegate       TransmissionRetryDelay       = null,
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
                   TransmissionRetryDelay,
                   MaxNumberOfRetries,
                   DNSClient)

        {

            this.Logger  = Logger ?? throw new ArgumentNullException(nameof(Logger), "The given mobile client logger must not be null!");

        }

        #endregion

        #endregion


        #region SendHeartbeat                         (Request)

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


        #region SetChargingPoolAvailabilityStatus     (Request)

        /// <summary>
        /// Send the given ChargingPool busy status.
        /// </summary>
        /// <param name="Request">A SetChargingPoolAvailabilityStatus request.</param>
        public async Task<HTTPResponse<SetChargingPoolAvailabilityStatusResponse>>

            SetChargingPoolAvailabilityStatus(SetChargingPoolAvailabilityStatusRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SetChargingPoolAvailabilityStatus request must not be null!");

            Request = _CustomSetChargingPoolAvailabilityStatusRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SetChargingPoolAvailabilityStatus request must not be null!");


            Byte                                                    TransmissionRetry  = 0;
            HTTPResponse<SetChargingPoolAvailabilityStatusResponse> result             = null;

            #endregion

            #region Send OnSetChargingPoolAvailabilityStatusRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnSetChargingPoolAvailabilityStatusRequest != null)
                    await Task.WhenAll(OnSetChargingPoolAvailabilityStatusRequest.GetInvocationList().
                                       Cast<OnSetChargingPoolAvailabilityStatusRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.ChargingPoolId,
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
                e.Log(nameof(CPOClient) + "." + nameof(OnSetChargingPoolAvailabilityStatusRequest));
            }

            #endregion



            if (!Request.ChargingPoolId.ToString().StartsWith("DE*BDO*E666181358*") &&
                !Request.ChargingPoolId.ToString().StartsWith("DE*BDO*ChargingPool*CI*TESTS"))
                    result = HTTPResponse<SetChargingPoolAvailabilityStatusResponse>.OK(
                                 new SetChargingPoolAvailabilityStatusResponse(
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

                    result = await _eMIPClient.Query(_CustomSetChargingPoolAvailabilityStatusSOAPRequestMapper(Request,
                                                                                                       SOAP.Encapsulation(Request.ToXML(CustomSetChargingPoolAvailabilityStatusRequestSerializer))),
                                                     DefaultSOAPActionPrefix + "eMIP_ToIOP_SetChargingPoolAvailabilityStatusV1/",
                                                     RequestLogDelegate:   OnSetChargingPoolAvailabilityStatusSOAPRequest,
                                                     ResponseLogDelegate:  OnSetChargingPoolAvailabilityStatusSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              SetChargingPoolAvailabilityStatusResponse.Parse(request,
                                                                                                                                      xml,
                                                                                                                                      CustomSetChargingPoolAvailabilityStatusParser,
                                                                                                                                      onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<SetChargingPoolAvailabilityStatusResponse>(

                                                                    httpresponse,

                                                                    new SetChargingPoolAvailabilityStatusResponse(
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

                                                             return new HTTPResponse<SetChargingPoolAvailabilityStatusResponse>(httpresponse,
                                                                                                        new SetChargingPoolAvailabilityStatusResponse(
                                                                                                            Request,
                                                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                                                            RequestStatus.HTTPError
                                                                                                            //httpresponse.HTTPStatusCode.ToString(),
                                                                                                            //httpresponse.HTTPBody.      ToUTF8String()
                                                                                                        ),
                                                                                                        IsFault: true);


                                                         return new HTTPResponse<SetChargingPoolAvailabilityStatusResponse>(

                                                                    httpresponse,

                                                                    new SetChargingPoolAvailabilityStatusResponse(
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

                                                         return HTTPResponse<SetChargingPoolAvailabilityStatusResponse>.ExceptionThrown(

                                                                new SetChargingPoolAvailabilityStatusResponse(
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
                    result = HTTPResponse<SetChargingPoolAvailabilityStatusResponse>.OK(
                                 new SetChargingPoolAvailabilityStatusResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.SystemError
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnSendSetChargingPoolAvailabilityStatusResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnSetChargingPoolAvailabilityStatusResponse != null)
                    await Task.WhenAll(OnSetChargingPoolAvailabilityStatusResponse.GetInvocationList().
                                       Cast<OnSetChargingPoolAvailabilityStatusResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.ChargingPoolId,
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
                e.Log(nameof(CPOClient) + "." + nameof(OnSetChargingPoolAvailabilityStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SetChargingStationAvailabilityStatus  (Request)

        /// <summary>
        /// Send the given ChargingStation busy status.
        /// </summary>
        /// <param name="Request">A SetChargingStationAvailabilityStatus request.</param>
        public async Task<HTTPResponse<SetChargingStationAvailabilityStatusResponse>>

            SetChargingStationAvailabilityStatus(SetChargingStationAvailabilityStatusRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SetChargingStationAvailabilityStatus request must not be null!");

            Request = _CustomSetChargingStationAvailabilityStatusRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SetChargingStationAvailabilityStatus request must not be null!");


            Byte                                                       TransmissionRetry  = 0;
            HTTPResponse<SetChargingStationAvailabilityStatusResponse> result             = null;

            #endregion

            #region Send OnSetChargingStationAvailabilityStatusRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnSetChargingStationAvailabilityStatusRequest != null)
                    await Task.WhenAll(OnSetChargingStationAvailabilityStatusRequest.GetInvocationList().
                                       Cast<OnSetChargingStationAvailabilityStatusRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.ChargingStationId,
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
                e.Log(nameof(CPOClient) + "." + nameof(OnSetChargingStationAvailabilityStatusRequest));
            }

            #endregion



            if (!Request.ChargingStationId.ToString().StartsWith("DE*BDO*E666181358*") &&
                !Request.ChargingStationId.ToString().StartsWith("DE*BDO*ChargingStation*CI*TESTS"))
                    result = HTTPResponse<SetChargingStationAvailabilityStatusResponse>.OK(
                                 new SetChargingStationAvailabilityStatusResponse(
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

                    result = await _eMIPClient.Query(_CustomSetChargingStationAvailabilityStatusSOAPRequestMapper(Request,
                                                                                                       SOAP.Encapsulation(Request.ToXML(CustomSetChargingStationAvailabilityStatusRequestSerializer))),
                                                     DefaultSOAPActionPrefix + "eMIP_ToIOP_SetChargingStationAvailabilityStatusV1/",
                                                     RequestLogDelegate:   OnSetChargingStationAvailabilityStatusSOAPRequest,
                                                     ResponseLogDelegate:  OnSetChargingStationAvailabilityStatusSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              SetChargingStationAvailabilityStatusResponse.Parse(request,
                                                                                                                                      xml,
                                                                                                                                      CustomSetChargingStationAvailabilityStatusParser,
                                                                                                                                      onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<SetChargingStationAvailabilityStatusResponse>(

                                                                    httpresponse,

                                                                    new SetChargingStationAvailabilityStatusResponse(
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

                                                             return new HTTPResponse<SetChargingStationAvailabilityStatusResponse>(httpresponse,
                                                                                                        new SetChargingStationAvailabilityStatusResponse(
                                                                                                            Request,
                                                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                                                            RequestStatus.HTTPError
                                                                                                            //httpresponse.HTTPStatusCode.ToString(),
                                                                                                            //httpresponse.HTTPBody.      ToUTF8String()
                                                                                                        ),
                                                                                                        IsFault: true);


                                                         return new HTTPResponse<SetChargingStationAvailabilityStatusResponse>(

                                                                    httpresponse,

                                                                    new SetChargingStationAvailabilityStatusResponse(
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

                                                         return HTTPResponse<SetChargingStationAvailabilityStatusResponse>.ExceptionThrown(

                                                                new SetChargingStationAvailabilityStatusResponse(
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
                    result = HTTPResponse<SetChargingStationAvailabilityStatusResponse>.OK(
                                 new SetChargingStationAvailabilityStatusResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.SystemError
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnSendSetChargingStationAvailabilityStatusResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnSetChargingStationAvailabilityStatusResponse != null)
                    await Task.WhenAll(OnSetChargingStationAvailabilityStatusResponse.GetInvocationList().
                                       Cast<OnSetChargingStationAvailabilityStatusResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.ChargingStationId,
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
                e.Log(nameof(CPOClient) + "." + nameof(OnSetChargingStationAvailabilityStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SetEVSEAvailabilityStatus             (Request)

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


            Byte                                            TransmissionRetry  = 0;
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

        #region SetChargingConnectorAvailabilityStatus(Request)

        /// <summary>
        /// Send the given ChargingConnector busy status.
        /// </summary>
        /// <param name="Request">A SetChargingConnectorAvailabilityStatus request.</param>
        public async Task<HTTPResponse<SetChargingConnectorAvailabilityStatusResponse>>

            SetChargingConnectorAvailabilityStatus(SetChargingConnectorAvailabilityStatusRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SetChargingConnectorAvailabilityStatus request must not be null!");

            Request = _CustomSetChargingConnectorAvailabilityStatusRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SetChargingConnectorAvailabilityStatus request must not be null!");


            Byte                                                         TransmissionRetry  = 0;
            HTTPResponse<SetChargingConnectorAvailabilityStatusResponse> result             = null;

            #endregion

            #region Send OnSetChargingConnectorAvailabilityStatusRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnSetChargingConnectorAvailabilityStatusRequest != null)
                    await Task.WhenAll(OnSetChargingConnectorAvailabilityStatusRequest.GetInvocationList().
                                       Cast<OnSetChargingConnectorAvailabilityStatusRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.ChargingConnectorId,
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
                e.Log(nameof(CPOClient) + "." + nameof(OnSetChargingConnectorAvailabilityStatusRequest));
            }

            #endregion



            if (!Request.ChargingConnectorId.ToString().StartsWith("DE*BDO*E666181358*") &&
                !Request.ChargingConnectorId.ToString().StartsWith("DE*BDO*ChargingConnector*CI*TESTS"))
                    result = HTTPResponse<SetChargingConnectorAvailabilityStatusResponse>.OK(
                                 new SetChargingConnectorAvailabilityStatusResponse(
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

                    result = await _eMIPClient.Query(_CustomSetChargingConnectorAvailabilityStatusSOAPRequestMapper(Request,
                                                                                                       SOAP.Encapsulation(Request.ToXML(CustomSetChargingConnectorAvailabilityStatusRequestSerializer))),
                                                     DefaultSOAPActionPrefix + "eMIP_ToIOP_SetChargingConnectorAvailabilityStatusV1/",
                                                     RequestLogDelegate:   OnSetChargingConnectorAvailabilityStatusSOAPRequest,
                                                     ResponseLogDelegate:  OnSetChargingConnectorAvailabilityStatusSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              SetChargingConnectorAvailabilityStatusResponse.Parse(request,
                                                                                                                                      xml,
                                                                                                                                      CustomSetChargingConnectorAvailabilityStatusParser,
                                                                                                                                      onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<SetChargingConnectorAvailabilityStatusResponse>(

                                                                    httpresponse,

                                                                    new SetChargingConnectorAvailabilityStatusResponse(
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

                                                             return new HTTPResponse<SetChargingConnectorAvailabilityStatusResponse>(httpresponse,
                                                                                                        new SetChargingConnectorAvailabilityStatusResponse(
                                                                                                            Request,
                                                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                                                            RequestStatus.HTTPError
                                                                                                            //httpresponse.HTTPStatusCode.ToString(),
                                                                                                            //httpresponse.HTTPBody.      ToUTF8String()
                                                                                                        ),
                                                                                                        IsFault: true);


                                                         return new HTTPResponse<SetChargingConnectorAvailabilityStatusResponse>(

                                                                    httpresponse,

                                                                    new SetChargingConnectorAvailabilityStatusResponse(
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

                                                         return HTTPResponse<SetChargingConnectorAvailabilityStatusResponse>.ExceptionThrown(

                                                                new SetChargingConnectorAvailabilityStatusResponse(
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
                    result = HTTPResponse<SetChargingConnectorAvailabilityStatusResponse>.OK(
                                 new SetChargingConnectorAvailabilityStatusResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.SystemError
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnSendSetChargingConnectorAvailabilityStatusResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnSetChargingConnectorAvailabilityStatusResponse != null)
                    await Task.WhenAll(OnSetChargingConnectorAvailabilityStatusResponse.GetInvocationList().
                                       Cast<OnSetChargingConnectorAvailabilityStatusResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.ChargingConnectorId,
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
                e.Log(nameof(CPOClient) + "." + nameof(OnSetChargingConnectorAvailabilityStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SetEVSEBusyStatus                     (Request)

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

        #region SetEVSESyntheticStatus                (Request)

        /// <summary>
        /// Send the given EVSE busy status.
        /// </summary>
        /// <param name="Request">A SetEVSESyntheticStatus request.</param>
        public async Task<HTTPResponse<SetEVSESyntheticStatusResponse>>

            SetEVSESyntheticStatus(SetEVSESyntheticStatusRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SetEVSESyntheticStatus request must not be null!");

            Request = _CustomSetEVSESyntheticStatusRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SetEVSESyntheticStatus request must not be null!");


            Byte                                         TransmissionRetry  = 0;
            HTTPResponse<SetEVSESyntheticStatusResponse> result             = null;

            #endregion

            #region Send OnSetEVSESyntheticStatusRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnSetEVSESyntheticStatusRequest != null)
                    await Task.WhenAll(OnSetEVSESyntheticStatusRequest.GetInvocationList().
                                       Cast<OnSetEVSESyntheticStatusRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.EVSEId,
                                                     Request.TransactionId,
                                                     Request.AvailabilityStatusEventDate,
                                                     Request.AvailabilityStatus,
                                                     Request.AvailabilityStatusUntil,
                                                     Request.AvailabilityStatusComment,
                                                     Request.BusyStatusEventDate,
                                                     Request.BusyStatus,
                                                     Request.BusyStatusUntil,
                                                     Request.BusyStatusComment,

                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetEVSESyntheticStatusRequest));
            }

            #endregion


            if (!Request.EVSEId.ToString().StartsWith("DE*BDO*E666181358*") &&
                !Request.EVSEId.ToString().StartsWith("DE*BDO*EVSE*CI*TESTS"))
                    result = HTTPResponse<SetEVSESyntheticStatusResponse>.OK(
                                 new SetEVSESyntheticStatusResponse(
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

                    result = await _eMIPClient.Query(_CustomSetEVSESyntheticStatusSOAPRequestMapper(Request,
                                                                                               SOAP.Encapsulation(Request.ToXML(CustomSetEVSESyntheticStatusRequestSerializer))),
                                                     "eMIP_ToIOP_SetEVSESyntheticStatusV1/",
                                                     RequestLogDelegate:   OnSetEVSESyntheticStatusSOAPRequest,
                                                     ResponseLogDelegate:  OnSetEVSESyntheticStatusSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              SetEVSESyntheticStatusResponse.Parse(request,
                                                                                                                                      xml,
                                                                                                                                      CustomSetEVSESyntheticStatusParser,
                                                                                                                                      onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<SetEVSESyntheticStatusResponse>(

                                                                    httpresponse,

                                                                    new SetEVSESyntheticStatusResponse(
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

                                                             return new HTTPResponse<SetEVSESyntheticStatusResponse>(httpresponse,
                                                                                                        new SetEVSESyntheticStatusResponse(
                                                                                                            Request,
                                                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                                                            RequestStatus.HTTPError
                                                                                                            //httpresponse.HTTPStatusCode.ToString(),
                                                                                                            //httpresponse.HTTPBody.      ToUTF8String()
                                                                                                        ),
                                                                                                        IsFault: true);


                                                         return new HTTPResponse<SetEVSESyntheticStatusResponse>(

                                                                    httpresponse,

                                                                    new SetEVSESyntheticStatusResponse(
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

                                                         return HTTPResponse<SetEVSESyntheticStatusResponse>.ExceptionThrown(

                                                                new SetEVSESyntheticStatusResponse(
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
                    result = HTTPResponse<SetEVSESyntheticStatusResponse>.OK(
                                 new SetEVSESyntheticStatusResponse(
                                     Request,
                                     Request.TransactionId ?? Transaction_Id.Zero,
                                     RequestStatus.SystemError
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnSendSetEVSESyntheticStatusResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnSetEVSESyntheticStatusResponse != null)
                    await Task.WhenAll(OnSetEVSESyntheticStatusResponse.GetInvocationList().
                                       Cast<OnSetEVSESyntheticStatusResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.EVSEId,
                                                     Request.TransactionId,
                                                     Request.AvailabilityStatusEventDate,
                                                     Request.AvailabilityStatus,
                                                     Request.AvailabilityStatusUntil,
                                                     Request.AvailabilityStatusComment,
                                                     Request.BusyStatusEventDate,
                                                     Request.BusyStatus,
                                                     Request.BusyStatusUntil,
                                                     Request.BusyStatusComment,

                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetEVSESyntheticStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region GetServiceAuthorisation               (Request)

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

        // eMIP_ToIOP_GetAuthenticationDataRequest

        #region SetSessionEventReport               (Request)

        /// <summary>
        /// Send a session event report.
        /// </summary>
        /// <param name="Request">A SetSessionEventReport request.</param>
        public async Task<HTTPResponse<SetSessionEventReportResponse>>

            SetSessionEventReport(SetSessionEventReportRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SetSessionEventReport request must not be null!");

            Request = _CustomSetSessionEventReportRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SetSessionEventReport request must not be null!");


            Byte                                        TransmissionRetry  = 0;
            HTTPResponse<SetSessionEventReportResponse> result             = null;

            #endregion

            #region Send OnSetSessionEventReportRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnSetSessionEventReportRequest != null)
                    await Task.WhenAll(OnSetSessionEventReportRequest.GetInvocationList().
                                       Cast<OnSetSessionEventReportRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.ServiceSessionId,
                                                     Request.SessionEvent,

                                                     Request.TransactionId,
                                                     Request.ExecPartnerSessionId,

                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetSessionEventReportRequest));
            }

            #endregion


            //if (!Request.EVSEId.ToString().StartsWith("DE*BDO*E666181358*") &&
            //    !Request.EVSEId.ToString().StartsWith("DE*BDO*EVSE*CI*TESTS"))
            //        result = HTTPResponse<SetSessionEventReportResponse>.OK(
            //                     new SetSessionEventReportResponse(
            //                         Request,
            //                         Request.TransactionId ?? Transaction_Id.Zero,
            //                         RequestStatus.ServiceNotAvailable,
            //                         ServiceSession_Id.Zero,
            //                         SessionAction_Id.Zero
            //                     //"HTTP request failed!"
            //                     )
            //                 );

            //else

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

                    result = await _eMIPClient.Query(_CustomSetSessionEventReportSOAPRequestMapper(Request,
                                                                                                   SOAP.Encapsulation(Request.ToXML(CustomSetSessionEventReportRequestSerializer))),
                                                     DefaultSOAPActionPrefix + "eMIP_ToIOP_SetSessionEventReportV1/",
                                                     RequestLogDelegate:   OnSetSessionEventReportSOAPRequest,
                                                     ResponseLogDelegate:  OnSetSessionEventReportSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              SetSessionEventReportResponse.Parse(request,
                                                                                                                                                  xml,
                                                                                                                                                  CustomSetSessionEventReportParser,
                                                                                                                                                  onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<SetSessionEventReportResponse>(

                                                                    httpresponse,

                                                                    new SetSessionEventReportResponse(
                                                                        Request,
                                                                        Request.TransactionId ?? Transaction_Id.Zero,
                                                                        RequestStatus.DataError,
                                                                        ServiceSession_Id.Zero,
                                                                        SessionAction_Id.Zero
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

                                                             return new HTTPResponse<SetSessionEventReportResponse>(httpresponse,
                                                                                                        new SetSessionEventReportResponse(
                                                                                                            Request,
                                                                                                            Request.TransactionId ?? Transaction_Id.Zero,
                                                                                                            RequestStatus.HTTPError,
                                                                                                            ServiceSession_Id.Zero,
                                                                                                            SessionAction_Id.Zero
                                                                                                        //httpresponse.HTTPStatusCode.ToString(),
                                                                                                        //httpresponse.HTTPBody.      ToUTF8String()
                                                                                                        ),
                                                                                                        IsFault: true);


                                                         return new HTTPResponse<SetSessionEventReportResponse>(

                                                                    httpresponse,

                                                                    new SetSessionEventReportResponse(
                                                                        Request,
                                                                        Request.TransactionId ?? Transaction_Id.Zero,
                                                                        RequestStatus.SystemError,
                                                                        ServiceSession_Id.Zero,
                                                                        SessionAction_Id.Zero
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

                                                         return HTTPResponse<SetSessionEventReportResponse>.ExceptionThrown(

                                                                new SetSessionEventReportResponse(
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
                    result = HTTPResponse<SetSessionEventReportResponse>.OK(
                                 new SetSessionEventReportResponse(
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


            #region Send OnSendSetSessionEventReportResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnSetSessionEventReportResponse != null)
                    await Task.WhenAll(OnSetSessionEventReportResponse.GetInvocationList().
                                       Cast<OnSetSessionEventReportResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,

                                                     Request.PartnerId,
                                                     Request.OperatorId,
                                                     Request.ServiceSessionId,
                                                     Request.SessionEvent,

                                                     Request.TransactionId,
                                                     Request.ExecPartnerSessionId,

                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetSessionEventReportResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SetChargeDetailRecord                 (Request)

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
