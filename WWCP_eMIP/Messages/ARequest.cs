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
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
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
        /// The partner identification.
        /// </summary>
        public Partner_Id               PartnerId                  { get; }

        /// <summary>
        /// The optional eMIP transaction identification.
        /// </summary>
        public Transaction_Id?          TransactionId              { get; }


        /// <summary>
        /// The optional timestamp of the request.
        /// </summary>
        public DateTime?                Timestamp                  { get; }

        /// <summary>
        /// An optional token source to cancel this request.
        /// </summary>
        public CancellationTokenSource  CancellationTokenSource    { get; }

        /// <summary>
        /// An optional token to cancel this request.
        /// </summary>
        public CancellationToken?       CancellationToken          { get; }

        /// <summary>
        /// An optional event tracking identification for correlating this request with other events.
        /// </summary>
        public EventTracking_Id         EventTrackingId            { get; }

        /// <summary>
        /// An optional timeout for this request.
        /// </summary>
        public TimeSpan?                RequestTimeout             { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new generic eMIP request message.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="TransactionId">An optional eMIP transaction identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public ARequest(Partner_Id          PartnerId,
                        Transaction_Id?     TransactionId       = null,

                        DateTime?           Timestamp           = null,
                        CancellationToken?  CancellationToken   = null,
                        EventTracking_Id    EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null)
        {

            this.PartnerId                = PartnerId;
            this.TransactionId            = TransactionId;

            this.Timestamp                = Timestamp                 ?? DateTime.UtcNow;
            this.CancellationTokenSource  = CancellationToken == null  ? new CancellationTokenSource() : null;
            this.CancellationToken        = CancellationToken         ?? CancellationTokenSource.Token;
            this.EventTrackingId          = EventTrackingId           ?? EventTracking_Id.New;
            this.RequestTimeout           = RequestTimeout            ?? DefaultRequestTimeout;

        }

        #endregion


        #region IEquatable<ARequest> Members

        /// <summary>
        /// Compare two abstract requests for equality.
        /// </summary>
        /// <param name="ARequest">Another abstract eMIP request.</param>
        public abstract Boolean Equals(TRequest ARequest);

        #endregion


    }

}
