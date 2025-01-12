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

using System;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// eMIP data conversion methods.
    /// </summary>
    public static partial class ConversionMethods
    {

        #region AsEVSEAvailabilityStatusTypes(Number)

        /// <summary>
        /// Parse the given numeric representation of an EVSE availability status.
        /// </summary>
        /// <param name="Number">A numeric-representation of an EVSE availability status</param>
        public static EVSEAvailabilityStatusTypes AsEVSEAvailabilityStatusTypes(Byte Number)
        {

            switch (Number)
            {

                case 1:
                    return EVSEAvailabilityStatusTypes.OutOfOrder;

                case 2:
                    return EVSEAvailabilityStatusTypes.InService;

                case 3:
                    return EVSEAvailabilityStatusTypes.Future;

                case 4:
                    return EVSEAvailabilityStatusTypes.Deleted;

                default:
                    return EVSEAvailabilityStatusTypes.Unspecified;

            }

        }

        #endregion

        #region AsEVSEAvailabilityStatusTypes(Text)

        /// <summary>
        /// Parse the given text representation of an EVSE availability status.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE availability status</param>
        public static EVSEAvailabilityStatusTypes AsEVSEAvailabilityStatusTypes(String Text)
        {

            switch (Text)
            {

                case "out-of-order":
                    return EVSEAvailabilityStatusTypes.OutOfOrder;

                case "in-service":
                    return EVSEAvailabilityStatusTypes.InService;

                case "future":
                    return EVSEAvailabilityStatusTypes.Future;

                case "deleted":
                    return EVSEAvailabilityStatusTypes.Deleted;

                default:
                    return EVSEAvailabilityStatusTypes.Unspecified;

            }

        }

        #endregion


        #region AsText  (this AvailabilityStatus)

        /// <summary>
        /// Return a text representation of the given EVSE availability status.
        /// </summary>
        /// <param name="AvailabilityStatus">An EVSE availability status.</param>
        public static String AsText(this EVSEAvailabilityStatusTypes AvailabilityStatus)
        {

            switch (AvailabilityStatus)
            {

                case EVSEAvailabilityStatusTypes.OutOfOrder:
                    return "out-of-order";

                case EVSEAvailabilityStatusTypes.InService:
                    return "in-service";

                case EVSEAvailabilityStatusTypes.Future:
                    return "future";

                case EVSEAvailabilityStatusTypes.Deleted:
                    return "deleted";

                default:
                    return "unspecified";

            }

        }

        #endregion

        #region AsNumber(this AvailabilityStatus)

        /// <summary>
        /// Return a numeric representation of the given EVSE availability status.
        /// </summary>
        /// <param name="AvailabilityStatus">An EVSE availability status.</param>
        public static Byte AsNumber(this EVSEAvailabilityStatusTypes AvailabilityStatus)
        {

            switch (AvailabilityStatus)
            {

                case EVSEAvailabilityStatusTypes.OutOfOrder:
                    return 1;

                case EVSEAvailabilityStatusTypes.InService:
                    return 2;

                case EVSEAvailabilityStatusTypes.Future:
                    return 3;

                case EVSEAvailabilityStatusTypes.Deleted:
                    return 4;

                default:
                    return 0;

            }

        }

        #endregion

    }

    /// <summary>
    /// The current availability status of an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public enum EVSEAvailabilityStatusTypes
    {

        /// <summary>
        /// The EVSE status is unknown.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The EVSE is not available to the end-user.
        /// </summary>
        OutOfOrder,

        /// <summary>
        /// The EVSE is available to the end-user.
        /// </summary>
        InService,

        /// <summary>
        /// The EVSE is not available to the end-user because it does not exist yet.
        /// The installation of this element is scheduled.
        /// </summary>
        Future,

        /// <summary>
        /// The EVSE is not available to the end-user because it does not exist anymore.
        /// </summary>
        Deleted

    }

}
