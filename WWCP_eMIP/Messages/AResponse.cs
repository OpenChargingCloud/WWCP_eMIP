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
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// An abstract generic eMIP response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the eMIP request.</typeparam>
    /// <typeparam name="TResponse">The type of the eMIP response.</typeparam>
    public abstract class AResponse<TRequest, TResponse> : ACustomData,
                                                           IResponse<TRequest>,
                                                           IEquatable<TResponse>

        where TRequest  : class, IRequest
        where TResponse : class, IResponse

    {

        #region Properties

        /// <summary>
        /// The request leading to this response.
        /// </summary>
        public TRequest        Request              { get; }

        /// <summary>
        /// The timestamp of the response message creation.
        /// </summary>
        public DateTime        ResponseTimestamp    { get; }


        /// <summary>
        /// The eMIP transaction identification.
        /// </summary>
        public Transaction_Id  TransactionId        { get; }

        /// <summary>
        /// The eMIP status of the request.
        /// </summary>
        public RequestStatus   RequestStatus        { get; }

        #endregion

        #region Constructor(s)

        #region AResponse(Request, TransactionId, RequestStatus,                           CustomData = null)

        /// <summary>
        /// Create a new generic eMIP response.
        /// </summary>
        /// <param name="Request">The eMIP request leading to this result.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        protected AResponse(TRequest                             Request,
                            Transaction_Id                       TransactionId,
                            RequestStatus                        RequestStatus,
                            IReadOnlyDictionary<String, Object>  CustomData  = null)

            : this(Request,
                   TransactionId,
                   RequestStatus,
                   DateTime.UtcNow,
                   CustomData)

        { }

        #endregion

        #region AResponse(Request, TransactionId, RequestStatus, ResponseTimestamp = null, CustomData = null)

        /// <summary>
        /// Create a new generic eMIP response.
        /// </summary>
        /// <param name="Request">The eMIP request leading to this result.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        protected AResponse(TRequest                             Request,
                            Transaction_Id                       TransactionId,
                            RequestStatus                        RequestStatus,
                            DateTime?                            ResponseTimestamp  = null,
                            IReadOnlyDictionary<String, Object>  CustomData         = null)

            : base(CustomData)

        {

            this.Request            = Request;
            this.TransactionId      = TransactionId;
            this.RequestStatus      = RequestStatus;
            this.ResponseTimestamp  = ResponseTimestamp ?? DateTime.UtcNow;

        }

        #endregion

        #endregion


        #region IEquatable<TResponse> Members

        /// <summary>
        /// Compare two abstract responses for equality.
        /// </summary>
        /// <param name="TResponse">Another abstract eMIP response.</param>
        public abstract Boolean Equals(TResponse TResponse);

        #endregion


    }

}
