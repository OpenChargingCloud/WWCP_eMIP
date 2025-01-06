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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
{

    /// <summary>
    /// An eMIP CPO server logger.
    /// </summary>
    public class CPOServerLogger : HTTPServerLogger
    {

        #region Data

        /// <summary>
        /// The default context for this logger.
        /// </summary>
        public const String DefaultContext = "eMIP_CPOServer";

        #endregion

        #region Properties

        /// <summary>
        /// The linked eMIP CPO server.
        /// </summary>
        public CPOServer  CPOServer    { get; }

        #endregion

        #region Constructor(s)

        #region CPOServerLogger(CPOServer, Context = DefaultContext, LogfileCreator = null)

        /// <summary>
        /// Create a new eMIP CPO server logger using the default logging delegates.
        /// </summary>
        /// <param name="CPOServer">A eMIP CPO server.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="Context">A context of this API.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPOServerLogger(CPOServer                CPOServer,
                               String                   LoggingPath,
                               String?                  Context         = null,
                               LogfileCreatorDelegate?  LogfileCreator  = null)

            : this(CPOServer,
                   LoggingPath,
                   Context,
                   null,
                   null,
                   null,
                   null,

                   LogfileCreator: LogfileCreator)

        { }

        #endregion

        #region CPOServerLogger(CPOServer, Context, ... Logging delegates ...)

        /// <summary>
        /// Create a new eMIP CPO server logger using the given logging delegates.
        /// </summary>
        /// <param name="CPOServer">A eMIP CPO server.</param>
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
        /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP server sent events source.</param>
        /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogHTTPError_toConsole">A delegate to log HTTP errors to console.</param>
        /// <param name="LogHTTPError_toDisc">A delegate to log HTTP errors to disc.</param>
        /// <param name="LogHTTPError_toNetwork">A delegate to log HTTP errors to a network target.</param>
        /// <param name="LogHTTPError_toHTTPSSE">A delegate to log HTTP errors to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPOServerLogger(CPOServer                    CPOServer,
                               String                       LoggingPath,
                               String?                      Context                     = null,

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

            : base(CPOServer.SOAPServer.HTTPServer,
                   LoggingPath,
                   Context ?? DefaultContext,

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

            this.CPOServer = CPOServer ?? throw new ArgumentNullException(nameof(CPOServer), "The given CPO server must not be null!");

            #region Register SetServiceAuthorisation log events

            RegisterEvent("SetServiceAuthorisationRequest",
                          handler => CPOServer.OnSetServiceAuthorisationSOAPRequest += handler,
                          handler => CPOServer.OnSetServiceAuthorisationSOAPRequest -= handler,
                          "SetServiceAuthorisation", "Request", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("SetServiceAuthorisationResponse",
                          handler => CPOServer.OnSetServiceAuthorisationSOAPResponse += handler,
                          handler => CPOServer.OnSetServiceAuthorisationSOAPResponse -= handler,
                          "SetServiceAuthorisation", "Response", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region Register SetSessionAction log events

            RegisterEvent("SetSessionActionRequest",
                          handler => CPOServer.OnSetSessionActionSOAPRequest += handler,
                          handler => CPOServer.OnSetSessionActionSOAPRequest -= handler,
                          "SetSessionAction", "Request", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("SetSessionActionResponse",
                          handler => CPOServer.OnSetSessionActionSOAPResponse += handler,
                          handler => CPOServer.OnSetSessionActionSOAPResponse -= handler,
                          "SetSessionAction", "Response", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

        }

        #endregion

        #endregion

    }

}
