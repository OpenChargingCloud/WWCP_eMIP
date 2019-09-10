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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using System;
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// An abstract generic eMIP response builder.
    /// </summary>
    /// <typeparam name="TRequest">The type of the eMIP request.</typeparam>
    /// <typeparam name="TResponse">The type of the eMIP response.</typeparam>
    public abstract class AResponseBuilder<TRequest, TResponse> : ACustomDataBuilder,
                                                                  IResponse,
                                                                  IEquatable<TResponse>

        where TRequest  : class, IRequest
        where TResponse : class, IResponse

    {

        #region Properties

        /// <summary>
        /// The request leading to this response.
        /// </summary>
        public TRequest        Request             { get; }

        /// <summary>
        /// The correlated HTTP response of this eMIP response.
        /// </summary>
        public HTTPResponse    HTTPResponse        { get; set; }

        /// <summary>
        /// The timestamp of the response message creation.
        /// </summary>
        public DateTime        ResponseTimestamp   { get; set; }

        /// <summary>
        /// The transaction identification.
        /// </summary>
        public Transaction_Id  TransactionId       { get; set; }

        /// <summary>
        /// The status of the request.
        /// </summary>
        public RequestStatus   RequestStatus       { get; set; }

        #endregion

        #region Constructor(s)

        #region AResponse(Request, CustomData = null)

        /// <summary>
        /// Create a new generic eMIP response.
        /// </summary>
        /// <param name="Request">The eMIP request leading to this result.</param>
        /// <param name="CustomData">Optional customer-specific data of the response.</param>
        protected AResponseBuilder(TRequest                             Request,
                                   IReadOnlyDictionary<String, Object>  CustomData   = null)

            : this(Request,
                   DateTime.UtcNow,
                   CustomData)

        { }

        #endregion

        #region AResponse(Request, ResponseTimestamp = null, CustomData = null)

        /// <summary>
        /// Create a new generic eMIP response.
        /// </summary>
        /// <param name="Request">The eMIP request leading to this result.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="CustomData">Optional customer-specific data of the response.</param>
        protected AResponseBuilder(TRequest                             Request,
                                   DateTime?                            ResponseTimestamp   = null,
                                   IReadOnlyDictionary<String, Object>  CustomData          = null)

            : base(CustomData)

        {

            this.Request            = Request;
            this.ResponseTimestamp  = ResponseTimestamp ?? DateTime.UtcNow;

        }

        #endregion

        #region AResponse(Request, CustomData = null)

        /// <summary>
        /// Create a new generic eMIP response.
        /// </summary>
        /// <param name="Request">The eMIP request leading to this result.</param>
        /// <param name="CustomData">Optional customer-specific data of the response.</param>
        protected AResponseBuilder(TRequest                                   Request,
                                   IEnumerable<KeyValuePair<String, Object>>  CustomData   = null)

            : this(Request,
                   DateTime.UtcNow,
                   CustomData)

        { }

        #endregion

        #region AResponse(Request, ResponseTimestamp = null, CustomData = null)

        /// <summary>
        /// Create a new generic eMIP response.
        /// </summary>
        /// <param name="Request">The eMIP request leading to this result.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="CustomData">Optional customer-specific data of the response.</param>
        protected AResponseBuilder(TRequest                                   Request,
                                   DateTime?                                  ResponseTimestamp   = null,
                                   IEnumerable<KeyValuePair<String, Object>>  CustomData          = null)

            : base(CustomData)

        {

            this.Request            = Request;
            this.ResponseTimestamp  = ResponseTimestamp ?? DateTime.UtcNow;

        }

        #endregion

        #endregion


        #region IEquatable<AResponse> Members

        /// <summary>
        /// Compare two abstract responses for equality.
        /// </summary>
        /// <param name="AResponse">Another abstract eMIP response.</param>
        public abstract Boolean Equals(TResponse AResponse);

        #endregion

        //#region (abstract) ToImmutable

        ///// <summary>
        ///// Return an immutable response.
        ///// </summary>
        //public abstract TResponse ToImmutable { get; }

        //#endregion

    }


}
