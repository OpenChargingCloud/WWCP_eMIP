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

        #region AsCDRNature(Number)

        /// <summary>
        /// Parse the given numeric representation of a charge detail record nature.
        /// </summary>
        /// <param name="Number">A numeric-representation of a charge detail record nature.</param>
        public static CDRNatures AsCDRNature(Byte Number)
        {

            switch (Number)
            {

                case 1:
                    return CDRNatures.Final;

                case 2:
                    return CDRNatures.Intermediate;

                default:
                    return CDRNatures.Undefined;

            }

        }

        #endregion

        #region AsCDRNature(Text)

        /// <summary>
        /// Parse the given text representation of a charge detail record nature.
        /// </summary>
        /// <param name="Text">A text representation of a charge detail record nature.</param>
        public static CDRNatures AsCDRNature(String Text)
        {

            switch (Text)
            {

                case "1":
                case "final":
                    return CDRNatures.Final;

                case "2":
                case "intermediate":
                    return CDRNatures.Intermediate;

                default:
                    return CDRNatures.Undefined;

            }

        }

        #endregion


        #region AsText  (this CDRNature)

        /// <summary>
        /// Return a text representation of the given charge detail record nature.
        /// </summary>
        /// <param name="CDRNature">A charge detail record nature.</param>
        public static String AsText(this CDRNatures CDRNature)
        {

            switch (CDRNature)
            {

                case CDRNatures.Final:
                    return "1";

                case CDRNatures.Intermediate:
                    return "2";

                default:
                    return "0";

            }

        }

        #endregion

        #region AsNumber(this CDRNature)

        /// <summary>
        /// Return a numeric representation of the given charge detail record nature.
        /// </summary>
        /// <param name="CDRNature">A charge detail record nature.</param>
        public static Byte AsNumber(this CDRNatures CDRNature)
        {

            switch (CDRNature)
            {

                case CDRNatures.Final:
                    return 1;

                case CDRNatures.Intermediate:
                    return 2;

                default:
                    return 0;

            }

        }

        #endregion

    }

    /// <summary>
    /// The nature of a charge detail record.
    /// </summary>
    public enum CDRNatures
    {

        /// <summary>
        /// Undefined.
        /// </summary>
        Undefined,

        /// <summary>
        /// A final charge detail record.
        /// </summary>
        Final,

        /// <summary>
        /// An intermediate charge detail record.
        /// </summary>
        Intermediate

    }

}
