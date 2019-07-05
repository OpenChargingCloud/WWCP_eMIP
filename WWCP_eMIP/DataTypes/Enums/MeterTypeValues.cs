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

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// eMIP data conversion methods.
    /// </summary>
    public static partial class ConversionMethods
    {

        #region AsMeterTypeValue(Number)

        /// <summary>
        /// Parse the given numeric representation of a meter type value.
        /// </summary>
        /// <param name="Number">A numeric-representation of a meter type value.</param>
        public static MeterTypeValues AsMeterTypeValue(Byte Number)
        {

            switch (Number)
            {

                case 1:
                    return MeterTypeValues.CounterMIDCommunicating;

                case 2:
                    return MeterTypeValues.CounterMIDNotCommunicating;

                case 3:
                    return MeterTypeValues.CounterNoMID;

                case 4:
                    return MeterTypeValues.NoMeter;

                default:
                    return MeterTypeValues.NotSpecified;

            }

        }

        #endregion

        #region AsMeterTypeValue(Text)

        /// <summary>
        /// Parse the given text representation of a meter type value.
        /// </summary>
        /// <param name="Text">A text representation of a meter type value.</param>
        public static MeterTypeValues AsMeterTypeValue(String Text)
        {

            switch (Text)
            {

                case "Counter MID not communicating":
                    return MeterTypeValues.CounterMIDCommunicating;

                case "Counter MID communicating":
                    return MeterTypeValues.CounterMIDNotCommunicating;

                case "Counter no MID":
                    return MeterTypeValues.CounterNoMID;

                case "No meter":
                    return MeterTypeValues.NoMeter;

                default:
                    return MeterTypeValues.NotSpecified;

            }

        }

        #endregion


        #region AsText  (this AvailabilityStatus)

        /// <summary>
        /// Return a text representation of the given authorisation result.
        /// </summary>
        /// <param name="AvailabilityStatus">An authorisation result.</param>
        public static String AsText(this MeterTypeValues AvailabilityStatus)
        {

            switch (AvailabilityStatus)
            {

                case MeterTypeValues.CounterMIDNotCommunicating:
                    return "Counter MID not communicating";

                case MeterTypeValues.CounterMIDCommunicating:
                    return "Counter MID communicating";

                case MeterTypeValues.CounterNoMID:
                    return "Counter no MID";

                case MeterTypeValues.NoMeter:
                    return "No meter";

                default:
                    return "Not specified";

            }

        }

        #endregion

        #region AsNumber(this AvailabilityStatus)

        /// <summary>
        /// Return a numeric representation of the given authorisation result.
        /// </summary>
        /// <param name="AvailabilityStatus">An authorisation result.</param>
        public static Byte AsNumber(this MeterTypeValues AvailabilityStatus)
        {

            switch (AvailabilityStatus)
            {

                case MeterTypeValues.CounterMIDNotCommunicating:
                    return 1;

                case MeterTypeValues.CounterMIDCommunicating:
                    return 2;

                case MeterTypeValues.CounterNoMID:
                    return 3;

                case MeterTypeValues.NoMeter:
                    return 4;

                default:
                    return 0;

            }

        }

        #endregion

    }

    /// <summary>
    /// The type of a meter value.
    /// </summary>
    public enum MeterTypeValues
    {

        /// <summary>
        /// Not specified.
        /// </summary>
        NotSpecified,

        /// <summary>
        /// Counter MID not communicating.
        /// </summary>
        CounterMIDNotCommunicating,

        /// <summary>
        /// Counter MID communicating.
        /// </summary>
        CounterMIDCommunicating,

        /// <summary>
        /// Counter non MID.
        /// </summary>
        CounterNoMID,

        /// <summary>
        /// No meter.
        /// </summary>
        NoMeter

    }

}
