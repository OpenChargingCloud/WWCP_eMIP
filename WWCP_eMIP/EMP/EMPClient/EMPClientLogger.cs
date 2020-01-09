/*
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.EMP
{

    /// <summary>
    /// An eMIP EMP Client.
    /// </summary>
    public partial class EMPClient : ASOAPClient
    {

        /// <summary>
        /// An eMIP EMP client (HTTP/SOAP client) logger.
        /// </summary>
        public class EMPClientLogger : HTTPClientLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "eMIP_EMPClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached eMIP EMP client.
            /// </summary>
            public IEMPClient EMPClient { get; }

            #endregion

            #region Constructor(s)

            #region EMPClientLogger(EMPClient, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new eMIP EMP client logger using the default logging delegates.
            /// </summary>
            /// <param name="EMPClient">A eMIP EMP client.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public EMPClientLogger(EMPClient               EMPClient,
                                   String                  Context         = DefaultContext,
                                   LogfileCreatorDelegate  LogfileCreator  = null)

                : this(EMPClient,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region EMPClientLogger(EMPClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new eMIP EMP client logger using the given logging delegates.
            /// </summary>
            /// <param name="EMPClient">A eMIP EMP client.</param>
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
            public EMPClientLogger(IEMPClient                  EMPClient,
                                   String                      Context,

                                   HTTPRequestLoggerDelegate   LogHTTPRequest_toConsole,
                                   HTTPResponseLoggerDelegate  LogHTTPResponse_toConsole,
                                   HTTPRequestLoggerDelegate   LogHTTPRequest_toDisc,
                                   HTTPResponseLoggerDelegate  LogHTTPResponse_toDisc,

                                   HTTPRequestLoggerDelegate   LogHTTPRequest_toNetwork    = null,
                                   HTTPResponseLoggerDelegate  LogHTTPResponse_toNetwork   = null,
                                   HTTPRequestLoggerDelegate   LogHTTPRequest_toHTTPSSE    = null,
                                   HTTPResponseLoggerDelegate  LogHTTPResponse_toHTTPSSE   = null,

                                   HTTPResponseLoggerDelegate  LogHTTPError_toConsole      = null,
                                   HTTPResponseLoggerDelegate  LogHTTPError_toDisc         = null,
                                   HTTPResponseLoggerDelegate  LogHTTPError_toNetwork      = null,
                                   HTTPResponseLoggerDelegate  LogHTTPError_toHTTPSSE      = null,

                                   LogfileCreatorDelegate      LogfileCreator              = null)

                : base(EMPClient,
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

                this.EMPClient = EMPClient ?? throw new ArgumentNullException(nameof(EMPClient), "The given EMP client must not be null!");

                #endregion

                #region Register log events

                RegisterEvent("SendHeartbeatRequest",
                              handler => EMPClient.OnSendHeartbeatSOAPRequest  += handler,
                              handler => EMPClient.OnSendHeartbeatSOAPRequest  -= handler,
                              "SendHeartbeat", "Heartbeat", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SendHeartbeatResponse",
                              handler => EMPClient.OnSendHeartbeatSOAPResponse += handler,
                              handler => EMPClient.OnSendHeartbeatSOAPResponse -= handler,
                              "SendHeartbeat", "Heartbeat", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);



                RegisterEvent("SetServiceAuthorisationRequest",
                              handler => EMPClient.OnSetServiceAuthorisationSOAPRequest  += handler,
                              handler => EMPClient.OnSetServiceAuthorisationSOAPRequest -= handler,
                              "SetServiceAuthorisation", "ServiceAuthorisation", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SetServiceAuthorisationResponse",
                              handler => EMPClient.OnSetServiceAuthorisationSOAPResponse += handler,
                              handler => EMPClient.OnSetServiceAuthorisationSOAPResponse -= handler,
                              "SetServiceAuthorisation", "ServiceAuthorisation", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("SetSessionActionRequest",
                              handler => EMPClient.OnSetSessionActionSOAPRequest += handler,
                              handler => EMPClient.OnSetSessionActionSOAPRequest -= handler,
                              "SetSessionAction", "SessionAction", "Request", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SetSessionActionResponse",
                              handler => EMPClient.OnSetSessionActionSOAPResponse += handler,
                              handler => EMPClient.OnSetSessionActionSOAPResponse -= handler,
                              "SetSessionAction", "SessionAction", "Response", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

     }

}
