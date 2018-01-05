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

        #region AsChargingPoolAvailabilityStatusTypes(Number)

        /// <summary>
        /// Parse the given numeric representation of a charging pool availability status.
        /// </summary>
        /// <param name="Number">A numeric-representation of a charging pool availability status</param>
        public static ChargingPoolAvailabilityStatusTypes AsChargingPoolAvailabilityStatusTypes(Byte Number)
        {

            switch (Number)
            {

                case 1:
                    return ChargingPoolAvailabilityStatusTypes.OutOfOrder;

                case 2:
                    return ChargingPoolAvailabilityStatusTypes.InService;

                case 3:
                    return ChargingPoolAvailabilityStatusTypes.Future;

                case 4:
                    return ChargingPoolAvailabilityStatusTypes.Deleted;

                default:
                    return ChargingPoolAvailabilityStatusTypes.Unspecified;

            }

        }

        #endregion

        #region AsChargingPoolAvailabilityStatusTypes(Text)

        /// <summary>
        /// Parse the given text representation of a charging pool availability status.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool availability status</param>
        public static ChargingPoolAvailabilityStatusTypes AsChargingPoolAvailabilityStatusTypes(String Text)
        {

            switch (Text)
            {

                case "out-of-order":
                    return ChargingPoolAvailabilityStatusTypes.OutOfOrder;

                case "in-service":
                    return ChargingPoolAvailabilityStatusTypes.InService;

                case "future":
                    return ChargingPoolAvailabilityStatusTypes.Future;

                case "deleted":
                    return ChargingPoolAvailabilityStatusTypes.Deleted;

                default:
                    return ChargingPoolAvailabilityStatusTypes.Unspecified;

            }

        }

        #endregion


        #region AsText  (this AvailabilityStatus)

        /// <summary>
        /// Return a text representation of the given charging pool availability status.
        /// </summary>
        /// <param name="AvailabilityStatus">A charging pool availability status.</param>
        public static String AsText(this ChargingPoolAvailabilityStatusTypes AvailabilityStatus)
        {

            switch (AvailabilityStatus)
            {

                case ChargingPoolAvailabilityStatusTypes.OutOfOrder:
                    return "out-of-order";

                case ChargingPoolAvailabilityStatusTypes.InService:
                    return "in-service";

                case ChargingPoolAvailabilityStatusTypes.Future:
                    return "future";

                case ChargingPoolAvailabilityStatusTypes.Deleted:
                    return "deleted";

                default:
                    return "unspecified";

            }

        }

        #endregion

        #region AsNumber(this AvailabilityStatus)

        /// <summary>
        /// Return a numeric representation of the given charging pool availability status.
        /// </summary>
        /// <param name="AvailabilityStatus">A charging pool availability status.</param>
        public static Byte AsNumber(this ChargingPoolAvailabilityStatusTypes AvailabilityStatus)
        {

            switch (AvailabilityStatus)
            {

                case ChargingPoolAvailabilityStatusTypes.OutOfOrder:
                    return 1;

                case ChargingPoolAvailabilityStatusTypes.InService:
                    return 2;

                case ChargingPoolAvailabilityStatusTypes.Future:
                    return 3;

                case ChargingPoolAvailabilityStatusTypes.Deleted:
                    return 4;

                default:
                    return 0;

            }

        }

        #endregion

    }

    /// <summary>
    /// The current availability status of a charging pool.
    /// </summary>
    public enum ChargingPoolAvailabilityStatusTypes
    {

        /// <summary>
        /// The charging pool status is unknown.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The charging pool is not available to the end-user.
        /// </summary>
        OutOfOrder,

        /// <summary>
        /// The charging pool is available to the end-user.
        /// </summary>
        InService,

        /// <summary>
        /// The charging pool is not available to the end-user because it does not exist yet.
        /// The installation of this element is scheduled.
        /// </summary>
        Future,

        /// <summary>
        /// The charging pool is not available to the end-user because it does not exist anymore.
        /// </summary>
        Deleted

    }

}
