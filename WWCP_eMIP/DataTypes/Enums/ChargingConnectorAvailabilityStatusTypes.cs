/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

        #region AsChargingConnectorAvailabilityStatusTypes(Number)

        /// <summary>
        /// Parse the given numeric representation of a charging connector availability status.
        /// </summary>
        /// <param name="Number">A numeric-representation of a charging connector availability status</param>
        public static ChargingConnectorAvailabilityStatusTypes AsChargingConnectorAvailabilityStatusTypes(Byte Number)
        {

            switch (Number)
            {

                case 1:
                    return ChargingConnectorAvailabilityStatusTypes.OutOfOrder;

                case 2:
                    return ChargingConnectorAvailabilityStatusTypes.InService;

                case 3:
                    return ChargingConnectorAvailabilityStatusTypes.Future;

                case 4:
                    return ChargingConnectorAvailabilityStatusTypes.Deleted;

                default:
                    return ChargingConnectorAvailabilityStatusTypes.Unspecified;

            }

        }

        #endregion

        #region AsChargingConnectorAvailabilityStatusTypes(Text)

        /// <summary>
        /// Parse the given text representation of a charging connector availability status.
        /// </summary>
        /// <param name="Text">A text representation of a charging connector availability status</param>
        public static ChargingConnectorAvailabilityStatusTypes AsChargingConnectorAvailabilityStatusTypes(String Text)
        {

            switch (Text)
            {

                case "out-of-order":
                    return ChargingConnectorAvailabilityStatusTypes.OutOfOrder;

                case "in-service":
                    return ChargingConnectorAvailabilityStatusTypes.InService;

                case "future":
                    return ChargingConnectorAvailabilityStatusTypes.Future;

                case "deleted":
                    return ChargingConnectorAvailabilityStatusTypes.Deleted;

                default:
                    return ChargingConnectorAvailabilityStatusTypes.Unspecified;

            }

        }

        #endregion


        #region AsText  (this AvailabilityStatus)

        /// <summary>
        /// Return a text representation of the given charging connector availability status.
        /// </summary>
        /// <param name="AvailabilityStatus">A charging connector availability status.</param>
        public static String AsText(this ChargingConnectorAvailabilityStatusTypes AvailabilityStatus)
        {

            switch (AvailabilityStatus)
            {

                case ChargingConnectorAvailabilityStatusTypes.OutOfOrder:
                    return "out-of-order";

                case ChargingConnectorAvailabilityStatusTypes.InService:
                    return "in-service";

                case ChargingConnectorAvailabilityStatusTypes.Future:
                    return "future";

                case ChargingConnectorAvailabilityStatusTypes.Deleted:
                    return "deleted";

                default:
                    return "unspecified";

            }

        }

        #endregion

        #region AsNumber(this AvailabilityStatus)

        /// <summary>
        /// Return a numeric representation of the given charging connector availability status.
        /// </summary>
        /// <param name="AvailabilityStatus">A charging connector availability status.</param>
        public static Byte AsNumber(this ChargingConnectorAvailabilityStatusTypes AvailabilityStatus)
        {

            switch (AvailabilityStatus)
            {

                case ChargingConnectorAvailabilityStatusTypes.OutOfOrder:
                    return 1;

                case ChargingConnectorAvailabilityStatusTypes.InService:
                    return 2;

                case ChargingConnectorAvailabilityStatusTypes.Future:
                    return 3;

                case ChargingConnectorAvailabilityStatusTypes.Deleted:
                    return 4;

                default:
                    return 0;

            }

        }

        #endregion

    }

    /// <summary>
    /// The current availability status of a charging connector.
    /// </summary>
    public enum ChargingConnectorAvailabilityStatusTypes
    {

        /// <summary>
        /// The charging connector status is unknown.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The charging connector is not available to the end-user.
        /// </summary>
        OutOfOrder,

        /// <summary>
        /// The charging connector is available to the end-user.
        /// </summary>
        InService,

        /// <summary>
        /// The charging connector is not available to the end-user because it does not exist yet.
        /// The installation of this element is scheduled.
        /// </summary>
        Future,

        /// <summary>
        /// The charging connector is not available to the end-user because it does not exist anymore.
        /// </summary>
        Deleted

    }

}
