/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// Extention methods for the eMIP CPO client interface.
    /// </summary>
    public static class ICPOClientExtentions
    {

        #region SendHeartbeat(PartnerId, OperatorId, TransactionId = null, ...)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<HeartbeatResponse>>

            SendHeartbeat(this ICPOClient     ICPOClient,
                          Partner_Id          PartnerId,
                          Operator_Id         OperatorId,
                          Transaction_Id?     TransactionId       = null,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id    EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null)


                => ICPOClient.SendHeartbeat(new HeartbeatRequest(PartnerId,
                                                                 OperatorId,
                                                                 TransactionId,

                                                                 Timestamp,
                                                                 CancellationToken,
                                                                 EventTrackingId,
                                                                 RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion

    }

}
