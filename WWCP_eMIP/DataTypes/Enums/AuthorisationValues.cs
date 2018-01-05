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

        #region AsAuthorisationValue(Number)

        /// <summary>
        /// Parse the given numeric representation of an authorisation result.
        /// </summary>
        /// <param name="Number">A numeric-representation of an authorisation result</param>
        public static AuthorisationValues AsAuthorisationValue(Byte Number)
        {

            switch (Number)
            {

                case 1:
                    return AuthorisationValues.OK;

                case 2:
                    return AuthorisationValues.OK;

                default:
                    return AuthorisationValues.Undefined;

            }

        }

        #endregion

        #region AsAuthorisationValue(Text)

        /// <summary>
        /// Parse the given text representation of an authorisation result.
        /// </summary>
        /// <param name="Text">A text representation of an authorisation result</param>
        public static AuthorisationValues AsAuthorisationValue(String Text)
        {

            switch (Text)
            {

                case "OK":
                    return AuthorisationValues.OK;

                case "KO":
                    return AuthorisationValues.KO;

                default:
                    return AuthorisationValues.Undefined;

            }

        }

        #endregion


        #region AsText  (this AvailabilityStatus)

        /// <summary>
        /// Return a text representation of the given authorisation result.
        /// </summary>
        /// <param name="AvailabilityStatus">An authorisation result.</param>
        public static String AsText(this AuthorisationValues AvailabilityStatus)
        {

            switch (AvailabilityStatus)
            {

                case AuthorisationValues.OK:
                    return "OK";

                case AuthorisationValues.KO:
                    return "KO";

                default:
                    return "undefined";

            }

        }

        #endregion

        #region AsNumber(this AvailabilityStatus)

        /// <summary>
        /// Return a numeric representation of the given authorisation result.
        /// </summary>
        /// <param name="AvailabilityStatus">An authorisation result.</param>
        public static Byte AsNumber(this AuthorisationValues AvailabilityStatus)
        {

            switch (AvailabilityStatus)
            {

                case AuthorisationValues.OK:
                    return 1;

                case AuthorisationValues.KO:
                    return 2;

                default:
                    return 0;

            }

        }

        #endregion

    }

    /// <summary>
    /// The result of an authorisation request.
    /// </summary>
    public enum AuthorisationValues
    {

        /// <summary>
        /// Undefined.
        /// </summary>
        Undefined,

        /// <summary>
        /// An eMSP has authorised the service or request a service.
        /// </summary>
        OK,

        /// <summary>
        /// An eMSP has forbidden the service.
        /// </summary>
        KO

    }

}
