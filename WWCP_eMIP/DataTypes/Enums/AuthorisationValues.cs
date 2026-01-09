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

        #region AsAuthorisationValue(Number)

        /// <summary>
        /// Parse the given numeric representation of an authorisation value.
        /// </summary>
        /// <param name="Number">A numeric-representation of an authorisation value.</param>
        public static AuthorisationValues AsAuthorisationValue(Byte Number)

            => Number switch {
                   1  => AuthorisationValues.OK,
                   2  => AuthorisationValues.KO,
                   _  => AuthorisationValues.Undefined
               };

        #endregion

        #region AsAuthorisationValue(Text)

        /// <summary>
        /// Parse the given text representation of an authorisation value.
        /// </summary>
        /// <param name="Text">A text representation of an authorisation value.</param>
        public static AuthorisationValues AsAuthorisationValue(String Text)

            => Text switch {
                   "1" or "OK"  => AuthorisationValues.OK,
                   "2" or "KO"  => AuthorisationValues.KO,
                   _            => AuthorisationValues.Undefined
               };


        #endregion


        #region AsText  (this AuthorisationValue)

        /// <summary>
        /// Return a text representation of the given authorisation value.
        /// </summary>
        /// <param name="AuthorisationValue">An authorisation value.</param>
        public static String AsText(this AuthorisationValues AuthorisationValue)

            => AuthorisationValue switch {
                   AuthorisationValues.OK  => "1",
                   AuthorisationValues.KO  => "2",
                   _                       => "undefined"
               };

        #endregion

        #region AsNumber(this AuthorisationValue)

        /// <summary>
        /// Return a numeric representation of the given authorisation value.
        /// </summary>
        /// <param name="AuthorisationValue">An authorisation value.</param>
        public static Byte AsNumber(this AuthorisationValues AuthorisationValue)

            => AuthorisationValue switch {
                   AuthorisationValues.OK  => 1,
                   AuthorisationValues.KO  => 2,
                   _                       => 0
               };

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
