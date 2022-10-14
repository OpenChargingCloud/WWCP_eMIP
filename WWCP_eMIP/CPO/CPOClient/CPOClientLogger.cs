/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
{

    /// <summary>
    /// The eMIP CPO client.
    /// </summary>
    public partial class CPOClient : ASOAPClient
    {

        /// <summary>
        /// The eMIP CPO HTTP/SOAP client logger.
        /// </summary>
        public class Logger : HTTPClientLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "eMIP_CPOClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached eMIP CPO client.
            /// </summary>
            public ICPOClient CPOClient { get; }

            #endregion

            #region Constructor(s)

            #region Logger(CPOClient, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new eMIP CPO client logger using the default logging delegates.
            /// </summary>
            /// <param name="CPOClient">An eMIP CPO client.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public Logger(CPOClient                CPOClient,
                          String                   LoggingPath,
                          String                   Context         = DefaultContext,
                          LogfileCreatorDelegate?  LogfileCreator  = null)

                : this(CPOClient,
                       LoggingPath,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region Logger(CPOClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new eMIP CPO client logger using the given logging delegates.
            /// </summary>
            /// <param name="CPOClient">An eMIP CPO client.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// 
            /// <param name="LogHTTPRequest_toConsole">A delegate to log incoming HTTP requests to console.</param>
            /// <param name="LogHTTPResponse_toConsole">A delegate to log HTTP requests/responses to console.</param>
            /// <param name="LogHTTPRequest_toDisc">A delegate to log incoming HTTP requests to disc.</param>
            /// <param name="LogHTTPResponse_toDisc">A delegate to log HTTP requests/responses to disc.</param>
            /// 
            /// <param name="LogHTTPRequest_toNetwork">A delegate to log incoming HTTP requests to a network target.</param>
            /// <param name="LogHTTPResponse_toNetwork">A delegate to log HTTP requests/responses to a network target.</param>
            /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP client sent events source.</param>
            /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP client sent events source.</param>
            /// 
            /// <param name="LogHTTPError_toConsole">A delegate to log HTTP errors to console.</param>
            /// <param name="LogHTTPError_toDisc">A delegate to log HTTP errors to disc.</param>
            /// <param name="LogHTTPError_toNetwork">A delegate to log HTTP errors to a network target.</param>
            /// <param name="LogHTTPError_toHTTPSSE">A delegate to log HTTP errors to a HTTP client sent events source.</param>
            /// 
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public Logger(ICPOClient                   CPOClient,
                          String                       LoggingPath,
                          String                       Context,

                          HTTPRequestLoggerDelegate?   LogHTTPRequest_toConsole    = null,
                          HTTPResponseLoggerDelegate?  LogHTTPResponse_toConsole   = null,
                          HTTPRequestLoggerDelegate?   LogHTTPRequest_toDisc       = null,
                          HTTPResponseLoggerDelegate?  LogHTTPResponse_toDisc      = null,

                          HTTPRequestLoggerDelegate?   LogHTTPRequest_toNetwork    = null,
                          HTTPResponseLoggerDelegate?  LogHTTPResponse_toNetwork   = null,
                          HTTPRequestLoggerDelegate?   LogHTTPRequest_toHTTPSSE    = null,
                          HTTPResponseLoggerDelegate?  LogHTTPResponse_toHTTPSSE   = null,

                          HTTPResponseLoggerDelegate?  LogHTTPError_toConsole      = null,
                          HTTPResponseLoggerDelegate?  LogHTTPError_toDisc         = null,
                          HTTPResponseLoggerDelegate?  LogHTTPError_toNetwork      = null,
                          HTTPResponseLoggerDelegate?  LogHTTPError_toHTTPSSE      = null,

                          LogfileCreatorDelegate?      LogfileCreator              = null)

                : base(CPOClient,
                       LoggingPath,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,

                       LogHTTPRequest_toConsole,
                       LogHTTPResponse_toConsole,
                       LogHTTPRequest_toDisc,
                       LogHTTPResponse_toDisc,

                       LogHTTPRequest_toNetwork,
                       LogHTTPResponse_toNetwork,
                       LogHTTPRequest_toHTTPSSE,
                       LogHTTPResponse_toHTTPSSE,

                       LogHTTPError_toConsole,
                       LogHTTPError_toDisc,
                       LogHTTPError_toNetwork,
                       LogHTTPError_toHTTPSSE,

                       LogfileCreator)

            {

                #region Initial checks

                this.CPOClient = CPOClient ?? throw new ArgumentNullException(nameof(CPOClient), "The given CPO client must not be null!");

                #endregion

                #region Register log events

                RegisterEvent("SendHeartbeatRequest",
                              handler => CPOClient.OnSendHeartbeatSOAPRequest  += handler,
                              handler => CPOClient.OnSendHeartbeatSOAPRequest  -= handler,
                              "SendHeartbeat", "Heartbeat", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SendHeartbeatResponse",
                              handler => CPOClient.OnSendHeartbeatSOAPResponse += handler,
                              handler => CPOClient.OnSendHeartbeatSOAPResponse -= handler,
                              "SendHeartbeat", "Heartbeat", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);



                RegisterEvent("SetChargingPoolAvailabilityStatusRequest",
                              handler => CPOClient.OnSetChargingPoolAvailabilityStatusSOAPRequest += handler,
                              handler => CPOClient.OnSetChargingPoolAvailabilityStatusSOAPRequest -= handler,
                              "SetChargingPoolAvailabilityStatus", "SetChargingPoolStatus", "ChargingPool", "AvailabilityStatus", "Status", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SetChargingPoolAvailabilityStatusResponse",
                              handler => CPOClient.OnSetChargingPoolAvailabilityStatusSOAPResponse += handler,
                              handler => CPOClient.OnSetChargingPoolAvailabilityStatusSOAPResponse -= handler,
                              "SetChargingPoolAvailabilityStatus", "SetChargingPoolStatus", "ChargingPool", "AvailabilityStatus", "Status", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("SetChargingStationAvailabilityStatusRequest",
                              handler => CPOClient.OnSetChargingStationAvailabilityStatusSOAPRequest += handler,
                              handler => CPOClient.OnSetChargingStationAvailabilityStatusSOAPRequest -= handler,
                              "SetChargingStationAvailabilityStatus", "SetChargingStationStatus", "ChargingStation", "AvailabilityStatus", "Status", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SetChargingStationAvailabilityStatusResponse",
                              handler => CPOClient.OnSetChargingStationAvailabilityStatusSOAPResponse += handler,
                              handler => CPOClient.OnSetChargingStationAvailabilityStatusSOAPResponse -= handler,
                              "SetChargingStationAvailabilityStatus", "SetChargingStationStatus", "ChargingStation", "AvailabilityStatus", "Status", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("SetEVSEAvailabilityStatusRequest",
                              handler => CPOClient.OnSetEVSEAvailabilityStatusSOAPRequest += handler,
                              handler => CPOClient.OnSetEVSEAvailabilityStatusSOAPRequest -= handler,
                              "SetEVSEAvailabilityStatus", "SetEVSEStatus", "EVSE", "AvailabilityStatus", "Status", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SetEVSEAvailabilityStatusResponse",
                              handler => CPOClient.OnSetEVSEAvailabilityStatusSOAPResponse += handler,
                              handler => CPOClient.OnSetEVSEAvailabilityStatusSOAPResponse -= handler,
                              "SetEVSEAvailabilityStatus", "SetEVSEStatus", "EVSE", "AvailabilityStatus", "Status", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("SetChargingConnectorAvailabilityStatusRequest",
                              handler => CPOClient.OnSetChargingConnectorAvailabilityStatusSOAPRequest += handler,
                              handler => CPOClient.OnSetChargingConnectorAvailabilityStatusSOAPRequest -= handler,
                              "SetChargingConnectorAvailabilityStatus", "SetChargingConnectorStatus", "ChargingConnector", "AvailabilityStatus", "Status", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SetChargingConnectorAvailabilityStatusResponse",
                              handler => CPOClient.OnSetChargingConnectorAvailabilityStatusSOAPResponse += handler,
                              handler => CPOClient.OnSetChargingConnectorAvailabilityStatusSOAPResponse -= handler,
                              "SetChargingConnectorAvailabilityStatus", "SetChargingConnectorStatus", "ChargingConnector", "AvailabilityStatus", "Status", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);



                RegisterEvent("SetEVSEBusyStatusRequest",
                              handler => CPOClient.OnSetEVSEBusyStatusSOAPRequest += handler,
                              handler => CPOClient.OnSetEVSEBusyStatusSOAPRequest -= handler,
                              "SetEVSEBusyStatus", "SetEVSEStatus", "EVSE", "BusyStatus", "Status", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SetEVSEBusyStatusResponse",
                              handler => CPOClient.OnSetEVSEBusyStatusSOAPResponse += handler,
                              handler => CPOClient.OnSetEVSEBusyStatusSOAPResponse -= handler,
                              "SetEVSEBusyStatus", "SetEVSEStatus", "EVSE", "BusyStatus", "Status", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("SetEVSESyntheticStatusRequest",
                              handler => CPOClient.OnSetEVSESyntheticStatusSOAPRequest += handler,
                              handler => CPOClient.OnSetEVSESyntheticStatusSOAPRequest -= handler,
                              "SetEVSESyntheticStatus", "SetEVSEStatus", "EVSE", "SyntheticStatus", "Status", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SetEVSESyntheticStatusResponse",
                              handler => CPOClient.OnSetEVSESyntheticStatusSOAPResponse += handler,
                              handler => CPOClient.OnSetEVSESyntheticStatusSOAPResponse -= handler,
                              "SetEVSESyntheticStatus", "SetEVSEStatus", "EVSE", "SyntheticStatus", "Status", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);




                RegisterEvent("GetServiceAuthorisationRequest",
                              handler => CPOClient.OnGetServiceAuthorisationSOAPRequest += handler,
                              handler => CPOClient.OnGetServiceAuthorisationSOAPRequest -= handler,
                              "GetServiceAuthorisation", "Authorisation", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetServiceAuthorisationResponse",
                              handler => CPOClient.OnGetServiceAuthorisationSOAPResponse += handler,
                              handler => CPOClient.OnGetServiceAuthorisationSOAPResponse -= handler,
                              "GetServiceAuthorisation", "Authorisation", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);



                RegisterEvent("SetSessionEventReportRequest",
                              handler => CPOClient.OnSetSessionEventReportSOAPRequest += handler,
                              handler => CPOClient.OnSetSessionEventReportSOAPRequest -= handler,
                              "SetSessionEventReport", "Authorisation", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SetSessionEventReportResponse",
                              handler => CPOClient.OnSetSessionEventReportSOAPResponse += handler,
                              handler => CPOClient.OnSetSessionEventReportSOAPResponse -= handler,
                              "SetSessionEventReport", "Authorisation", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);



                RegisterEvent("SetChargeDetailRecordRequest",
                              handler => CPOClient.OnSetChargeDetailRecordSOAPRequest += handler,
                              handler => CPOClient.OnSetChargeDetailRecordSOAPRequest -= handler,
                              "SetChargeDetailRecord", "ChargeDetailRecord", "CDR", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SetChargeDetailRecordResponse",
                              handler => CPOClient.OnSetChargeDetailRecordSOAPResponse += handler,
                              handler => CPOClient.OnSetChargeDetailRecordSOAPResponse -= handler,
                              "SetChargeDetailRecord", "ChargeDetailRecord", "CDR", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

     }

}
