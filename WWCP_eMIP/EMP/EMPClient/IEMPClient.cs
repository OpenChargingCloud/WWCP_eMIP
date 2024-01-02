/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.EMP
{

    /// <summary>
    /// The common interface of all eMIP EMP clients.
    /// </summary>
    public interface IEMPClient : IHTTPClient
    {

        #region Events

        #region OnHeartbeatRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a heartbeat will be send.
        /// </summary>
        event OnSendHeartbeatRequestDelegate   OnSendHeartbeatRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a heartbeat will be send.
        /// </summary>
        event ClientRequestLogHandler          OnSendHeartbeatSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a heartbeat SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler         OnSendHeartbeatSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a heartbeat request had been received.
        /// </summary>
        event OnSendHeartbeatResponseDelegate  OnSendHeartbeatResponse;

        #endregion


        #region OnSetServiceAuthorisationRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a SetServiceAuthorisation will be send.
        /// </summary>
        event OnSetServiceAuthorisationRequestDelegate   OnSetServiceAuthorisationRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a SetServiceAuthorisation will be send.
        /// </summary>
        event ClientRequestLogHandler                    OnSetServiceAuthorisationSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a SetServiceAuthorisation SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                   OnSetServiceAuthorisationSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a SetServiceAuthorisation request had been received.
        /// </summary>
        event OnSetServiceAuthorisationResponseDelegate  OnSetServiceAuthorisationResponse;

        #endregion

        #region OnSetSessionActionRequest/-Response

        /// <summary>
        /// An event fired whenever a request sending a heartbeat will be send.
        /// </summary>
        event OnSetSessionActionRequestDelegate   OnSetSessionActionRequest;

        /// <summary>
        /// An event fired whenever a SOAP request sending a heartbeat will be send.
        /// </summary>
        event ClientRequestLogHandler             OnSetSessionActionSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a heartbeat SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler            OnSetSessionActionSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a heartbeat request had been received.
        /// </summary>
        event OnSetSessionActionResponseDelegate  OnSetSessionActionResponse;

        #endregion

        #endregion


        /// <summary>
        /// Send the given heartbeat.
        /// </summary>
        /// <param name="Request">A SendHeartbeat request.</param>
        Task<HTTPResponse<HeartbeatResponse>>
            SendHeartbeat(HeartbeatRequest Request);


        /// <summary>
        /// Send the given SetServiceAuthorisation request.
        /// </summary>
        /// <param name="Request">A SetServiceAuthorisation request.</param>
        Task<HTTPResponse<SetServiceAuthorisationResponse>>
            SetServiceAuthorisation(SetServiceAuthorisationRequest Request);

        /// <summary>
        /// Send the given SetSessionActionRequest request.
        /// </summary>
        /// <param name="Request">A SetSessionActionRequest request.</param>
        Task<HTTPResponse<SetSessionActionResponse>>
            SetSessionAction(SetSessionActionRequest Request);

    }

}
