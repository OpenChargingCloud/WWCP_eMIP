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

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// eMIP data conversion methods.
    /// </summary>
    public static partial class ConversionMethods
    {

        #region AsEVSEBusyStatusTypes(Number)

        /// <summary>
        /// Parse the given numeric representation of an EVSE busy status.
        /// </summary>
        /// <param name="Number">A numeric-representation of an EVSE busy status</param>
        public static EVSEBusyStatusTypes AsEVSEBusyStatusTypes(Byte Number)
        {

            switch (Number)
            {

                case 1:
                    return EVSEBusyStatusTypes.Free;

                case 2:
                    return EVSEBusyStatusTypes.Busy;

                case 3:
                    return EVSEBusyStatusTypes.Reserved;

                default:
                    return EVSEBusyStatusTypes.Unspecified;

            }

        }

        #endregion

        #region AsEVSEBusyStatusTypes(Text)

        /// <summary>
        /// Parse the given text representation of an EVSE busy status.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE busy status</param>
        public static EVSEBusyStatusTypes AsEVSEBusyStatusTypes(String Text)
        {

            switch (Text)
            {

                case "free":
                    return EVSEBusyStatusTypes.Free;

                case "busy":
                    return EVSEBusyStatusTypes.Busy;

                case "reserved":
                    return EVSEBusyStatusTypes.Reserved;

                default:
                    return EVSEBusyStatusTypes.Unspecified;

            }

        }

        #endregion


        #region AsText  (this BusyStatus)

        /// <summary>
        /// Return a text representation of the given EVSE busy status.
        /// </summary>
        /// <param name="BusyStatus">An EVSE busy status.</param>
        public static String AsText(this EVSEBusyStatusTypes BusyStatus)
        {

            switch (BusyStatus)
            {

                case EVSEBusyStatusTypes.Free:
                    return "free";

                case EVSEBusyStatusTypes.Busy:
                    return "busy";

                case EVSEBusyStatusTypes.Reserved:
                    return "reserved";

                default:
                    return "unspecified";

            }

        }

        #endregion

        #region AsNumber(this BusyStatus)

        /// <summary>
        /// Return a numeric representation of the given EVSE busy status.
        /// </summary>
        /// <param name="BusyStatus">An EVSE busy status.</param>
        public static Byte AsNumber(this EVSEBusyStatusTypes BusyStatus)
        {

            switch (BusyStatus)
            {

                case EVSEBusyStatusTypes.Free:
                    return 1;

                case EVSEBusyStatusTypes.Busy:
                    return 2;

                case EVSEBusyStatusTypes.Reserved:
                    return 3;

                default:
                    return 0;

            }

        }

        #endregion

    }

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
