/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// eMIP data conversion methods.
    /// </summary>
    public static partial class ConversionMethods
    {

        #region AsRemoteStartStopValue(Number)

        /// <summary>
        /// Parse the given numeric representation of a remote start/stop request.
        /// </summary>
        /// <param name="Number">A numeric-representation of a remote start/stop request.</param>
        public static RemoteStartStopValues AsRemoteStartStopValue(Byte Number)
        {

            switch (Number)
            {

                case 1:
                    return RemoteStartStopValues.Start;

                case 2:
                    return RemoteStartStopValues.Stop;

                default:
                    return RemoteStartStopValues.Undefined;

            }

        }

        #endregion

        #region AsRemoteStartStopValue(Text)

        /// <summary>
        /// Parse the given text representation of a remote start/stop request.
        /// </summary>
        /// <param name="Text">A text representation of a remote start/stop request.</param>
        public static RemoteStartStopValues AsRemoteStartStopValue(String Text)
        {

            switch (Text)
            {

                case "1":
                case "OK":
                    return RemoteStartStopValues.Start;

                case "2":
                case "KO":
                    return RemoteStartStopValues.Stop;

                default:
                    return RemoteStartStopValues.Undefined;

            }

        }

        #endregion


        #region AsText  (this RemoteStartStopValue)

        /// <summary>
        /// Return a text representation of the given remote start/stop value.
        /// </summary>
        /// <param name="RemoteStartStopValue">An remote start/stop value.</param>
        public static String AsText(this RemoteStartStopValues RemoteStartStopValue)

            => RemoteStartStopValue switch {
                   RemoteStartStopValues.Start  => "1",
                   RemoteStartStopValues.Stop   => "2",
                   _                            => "undefined",
               };

        #endregion

        #region AsNumber(this RemoteStartStopValue)

        /// <summary>
        /// Return a numeric representation of the given remote start/stop value.
        /// </summary>
        /// <param name="RemoteStartStopValue">An remote start/stop value.</param>
        public static Byte AsNumber(this RemoteStartStopValues RemoteStartStopValue)

            => RemoteStartStopValue switch {
                   RemoteStartStopValues.Start  => 1,
                   RemoteStartStopValues.Stop   => 2,
                   _                            => 0,
               };

        #endregion

    }

    /// <summary>
    /// The commands of a remote start/stop request.
    /// </summary>
    public enum RemoteStartStopValues
    {

        /// <summary>
        /// Undefined.
        /// </summary>
        Undefined,

        /// <summary>
        /// Start a charging process.
        /// </summary>
        Start,

        /// <summary>
        /// Stop a charging process.
        /// </summary>
        Stop

    }

}
