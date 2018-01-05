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

        #region AsMeterTypeId(Number)

        /// <summary>
        /// Parse the given numeric representation of an meter type identification result.
        /// </summary>
        /// <param name="Number">A numeric-representation of an meter type identification result.</param>
        public static MeterTypeIds AsMeterTypeId(Byte Number)
        {

            switch (Number)
            {

                case 1:
                    return MeterTypeIds.Minutes;

                case 2:
                    return MeterTypeIds.kWh;

                default:
                    return MeterTypeIds.Undefined;

            }

        }

        #endregion

        #region AsMeterTypeId(Text)

        /// <summary>
        /// Parse the given text representation of an meter type identification result.
        /// </summary>
        /// <param name="Text">A text representation of an meter type identification result.</param>
        public static MeterTypeIds AsMeterTypeId(String Text)
        {

            switch (Text)
            {

                case "minutes":
                    return MeterTypeIds.Minutes;

                case "kWh":
                    return MeterTypeIds.kWh;

                default:
                    return MeterTypeIds.Undefined;

            }

        }

        #endregion


        #region AsText  (this MeterTypeId)

        /// <summary>
        /// Return a text representation of the given meter type identification result.
        /// </summary>
        /// <param name="MeterTypeId">A meter type identification result.</param>
        public static String AsText(this MeterTypeIds MeterTypeId)
        {

            switch (MeterTypeId)
            {

                case MeterTypeIds.Minutes:
                    return "minutes";

                case MeterTypeIds.kWh:
                    return "kWh";

                default:
                    return "undefined";

            }

        }

        #endregion

        #region AsNumber(this MeterTypeId)

        /// <summary>
        /// Return a numeric representation of the given meter type identification result.
        /// </summary>
        /// <param name="MeterTypeId">A meter type identification result.</param>
        public static Byte AsNumber(this MeterTypeIds MeterTypeId)
        {

            switch (MeterTypeId)
            {

                case MeterTypeIds.Minutes:
                    return 1;

                case MeterTypeIds.kWh:
                    return 2;

                default:
                    return 0;

            }

        }

        #endregion

    }

    /// <summary>
    /// The type of a meter report value.
    /// </summary>
    public enum MeterTypeIds
    {

        /// <summary>
        /// Undefined.
        /// </summary>
        Undefined,

        /// <summary>
        /// Duration in minutes.
        /// </summary>
        Minutes,

        /// <summary>
        /// Transferred energy equivalent.
        /// </summary>
        kWh

    }

}
