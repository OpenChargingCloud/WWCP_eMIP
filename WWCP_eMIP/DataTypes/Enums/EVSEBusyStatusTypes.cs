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

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// The current busy status of an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public enum EVSEBusyStatusTypes
    {

        /// <summary>
        /// The Charging Point busy status is unknown.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The Charging Point is ready to charge any authorised end-user.
        /// </summary>
        Free,

        /// <summary>
        /// The Charging Point is currently in use and cannot be used by any other end-user.
        /// </summary>
        Busy,

        /// <summary>
        /// The Charging Point has been reserved and is waiting to charge the end-user who reserved the point and only him.
        /// </summary>
        Reserved

    }

}
