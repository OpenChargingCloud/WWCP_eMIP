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

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// The common interface of all eMIP response messages.
    /// </summary>
    public interface IResponse
    {

        /// <summary>
        /// The timestamp of the response message creation.
        /// </summary>
        DateTime        ResponseTimestamp    { get; }

        /// <summary>
        /// The eMIP transaction identification.
        /// </summary>
        Transaction_Id  TransactionId        { get; }

        /// <summary>
        /// The eMIP status of the request.
        /// </summary>
        RequestStatus   RequestStatus        { get; }

    }


    /// <summary>
    /// The common interface of an eMIP response message.
    /// </summary>
    /// <typeparam name="TRequest">The type of the eMIP request.</typeparam>
    public interface IResponse<TRequest> : IResponse

        where TRequest : class, IRequest

    {

        /// <summary>
        /// The request leading to this response.
        /// </summary>
        TRequest        Request              { get; }

    }

}
