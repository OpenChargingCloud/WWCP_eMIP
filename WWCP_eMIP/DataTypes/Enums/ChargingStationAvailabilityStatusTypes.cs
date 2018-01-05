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

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// eMIP data conversion methods.
    /// </summary>
    public static partial class ConversionMethods
    {

        #region AsChargingStationAvailabilityStatusTypes(Number)

        /// <summary>
        /// Parse the given numeric representation of a charging station availability status.
        /// </summary>
        /// <param name="Number">A numeric-representation of a charging station availability status</param>
        public static ChargingStationAvailabilityStatusTypes AsChargingStationAvailabilityStatusTypes(Byte Number)
        {

            switch (Number)
            {

                case 1:
                    return ChargingStationAvailabilityStatusTypes.OutOfOrder;

                case 2:
                    return ChargingStationAvailabilityStatusTypes.InService;

                case 3:
                    return ChargingStationAvailabilityStatusTypes.Future;

                case 4:
                    return ChargingStationAvailabilityStatusTypes.Deleted;

                default:
                    return ChargingStationAvailabilityStatusTypes.Unspecified;

            }

        }

        #endregion

        #region AsChargingStationAvailabilityStatusTypes(Text)

        /// <summary>
        /// Parse the given text representation of a charging station availability status.
        /// </summary>
        /// <param name="Text">A text representation of a charging station availability status</param>
        public static ChargingStationAvailabilityStatusTypes AsChargingStationAvailabilityStatusTypes(String Text)
        {

            switch (Text)
            {

                case "out-of-order":
                    return ChargingStationAvailabilityStatusTypes.OutOfOrder;

                case "in-service":
                    return ChargingStationAvailabilityStatusTypes.InService;

                case "future":
                    return ChargingStationAvailabilityStatusTypes.Future;

                case "deleted":
                    return ChargingStationAvailabilityStatusTypes.Deleted;

                default:
                    return ChargingStationAvailabilityStatusTypes.Unspecified;

            }

        }

        #endregion


        #region AsText  (this AvailabilityStatus)

        /// <summary>
        /// Return a text representation of the given charging station availability status.
        /// </summary>
        /// <param name="AvailabilityStatus">A charging station availability status.</param>
        public static String AsText(this ChargingStationAvailabilityStatusTypes AvailabilityStatus)
        {

            switch (AvailabilityStatus)
            {

                case ChargingStationAvailabilityStatusTypes.OutOfOrder:
                    return "out-of-order";

                case ChargingStationAvailabilityStatusTypes.InService:
                    return "in-service";

                case ChargingStationAvailabilityStatusTypes.Future:
                    return "future";

                case ChargingStationAvailabilityStatusTypes.Deleted:
                    return "deleted";

                default:
                    return "unspecified";

            }

        }

        #endregion

        #region AsNumber(this AvailabilityStatus)

        /// <summary>
        /// Return a numeric representation of the given charging station availability status.
        /// </summary>
        /// <param name="AvailabilityStatus">A charging station availability status.</param>
        public static Byte AsNumber(this ChargingStationAvailabilityStatusTypes AvailabilityStatus)
        {

            switch (AvailabilityStatus)
            {

                case ChargingStationAvailabilityStatusTypes.OutOfOrder:
                    return 1;

                case ChargingStationAvailabilityStatusTypes.InService:
                    return 2;

                case ChargingStationAvailabilityStatusTypes.Future:
                    return 3;

                case ChargingStationAvailabilityStatusTypes.Deleted:
                    return 4;

                default:
                    return 0;

            }

        }

        #endregion

    }

    /// <summary>
    /// The current availability status of a charging station.
    /// </summary>
    public enum ChargingStationAvailabilityStatusTypes
    {

        /// <summary>
        /// The charging station status is unknown.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The charging station is not available to the end-user.
        /// </summary>
        OutOfOrder,

        /// <summary>
        /// The charging station is available to the end-user.
        /// </summary>
        InService,

        /// <summary>
        /// The charging station is not available to the end-user because it does not exist yet.
        /// The installation of this element is scheduled.
        /// </summary>
        Future,

        /// <summary>
        /// The charging station is not available to the end-user because it does not exist anymore.
        /// </summary>
        Deleted

    }

}
