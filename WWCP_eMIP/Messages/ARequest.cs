﻿/*
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// An abstract generic eMIP request.
    /// </summary>
    /// <typeparam name="TRequest">The type of the eMIP request.</typeparam>
    public abstract class ARequest<TRequest> : IRequest,
                                               IEquatable<TRequest>

        where TRequest : class, IRequest

    {

        #region Data

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public static readonly TimeSpan DefaultRequestTimeout = TimeSpan.FromSeconds(60);

        #endregion

        #region Properties

        /// <summary>
        /// The correlated HTTP request of this eMIP request.
        /// </summary>
        public HTTPRequest        HTTPRequest          { get; }

        /// <summary>
        /// The partner identification.
        /// </summary>
        public Partner_Id         PartnerId            { get; }

        /// <summary>
        /// The optional eMIP transaction identification.
        /// </summary>
        public Transaction_Id?    TransactionId        { get; }


        /// <summary>
        /// The timestamp of the request.
        /// </summary>
        public DateTime           Timestamp            { get; }

        /// <summary>
        /// An optional event tracking identification for correlating this request with other events.
        /// </summary>
        public EventTracking_Id   EventTrackingId      { get; }

        /// <summary>
        /// An optional timeout for this request.
        /// </summary>
        public TimeSpan?          RequestTimeout       { get; }

        /// <summary>
        /// An optional token to cancel this request.
        /// </summary>
        public CancellationToken  CancellationToken    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new generic eMIP request message.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="TransactionId">An optional eMIP transaction identification.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public ARequest(HTTPRequest         HTTPRequest,
                        Partner_Id          PartnerId,
                        Transaction_Id?     TransactionId       = null,

                        DateTime?           Timestamp           = null,
                        EventTracking_Id?   EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null,
                        CancellationToken   CancellationToken   = default)
        {

            this.HTTPRequest        = HTTPRequest;
            this.PartnerId          = PartnerId;
            this.TransactionId      = TransactionId;

            this.Timestamp          = Timestamp        ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            this.EventTrackingId    = EventTrackingId  ?? EventTracking_Id.New;
            this.RequestTimeout     = RequestTimeout   ?? DefaultRequestTimeout;
            this.CancellationToken  = CancellationToken;

        }

        #endregion


        #region IEquatable<ARequest> Members

        /// <summary>
        /// Compare two abstract requests for equality.
        /// </summary>
        /// <param name="ARequest">Another abstract eMIP request.</param>
        public abstract Boolean Equals(TRequest? ARequest);

        #endregion


    }

}
