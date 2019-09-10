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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public struct EVSE_Id : IId,
                            IEquatable<EVSE_Id>,
                            IComparable<EVSE_Id>

    {

        #region Data

        private static readonly Random _Random       = new Random(DateTime.UtcNow.Millisecond);

        /// <summary>
        /// The regular expression for parsing an EVSE identification.
        /// All '*' are optional!
        /// </summary>
        public  static readonly Regex  EVSEId_RegEx  = new Regex(@"^([A-Za-z]{2}\*?[A-Za-z0-9]{3})\*?E([A-Za-z0-9\*]{1,30})$",
                                                                 RegexOptions.IgnorePatternWhitespace);

        private        readonly String MinSuffix;

        #endregion

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id        OperatorId   { get; }

        /// <summary>
        /// The suffix of the EVSE identification.
        /// </summary>
        public String             Suffix       { get; }

        /// <summary>
        /// The format of the EVSE identification.
        /// </summary>
        public OperatorIdFormats  Format
            => OperatorId.Format;

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
        {
            get
            {

                switch (Format)
                {

                    case OperatorIdFormats.eMI3_STAR:
                        return OperatorId.Length + 2 + (UInt64) Suffix.Length;

                    default:
                        return OperatorId.Length + 1 + (UInt64) Suffix.Length;

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE) identification based on the given
        /// operator identification and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Suffix">The suffix of the EVSE identification.</param>
        private EVSE_Id(Operator_Id  OperatorId,
                        String       Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),  "The EVSE identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;
            this.MinSuffix   = Suffix.Replace("*", "");

        }

        #endregion


        #region Random(OperatorId, Length = 10, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of an EVSE identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Length">The desired length of the identification suffix.</param>
        /// <param name="Mapper">A delegate to modify the newly generated EVSE identification.</param>
        public static EVSE_Id Random(Operator_Id           OperatorId,
                                     Byte                  Length  = 10,
                                     Func<String, String>  Mapper  = null)

            => new EVSE_Id(OperatorId,
                           (Mapper ?? (_ => _)) (_Random.RandomString((UInt16) (Length < 10 ? 10 : Length > 50 ? 50 : Length))));

        #endregion

        #region (static) Parse(Text)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE identification.</param>
        public static EVSE_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text),  "The given text representation of an EVSE identification must not be null or empty!");

            #endregion

            var MatchCollection = EVSEId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE identification '" + Text + "'!",
                                            nameof(Text));


            if (Operator_Id.TryParse(MatchCollection[0].Groups[1].Value, out Operator_Id _OperatorId))
                return new EVSE_Id(_OperatorId,
                                   MatchCollection[0].Groups[2].Value);

            throw new ArgumentException("Illegal EVSE identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Suffix">The suffix of the EVSE identification.</param>
        public static EVSE_Id Parse(Operator_Id  OperatorId,
                                    String       Suffix)
        {

            switch (OperatorId.Format)
            {

                case OperatorIdFormats.eMI3:
                    return Parse(OperatorId +  "E" + Suffix);

                default:
                    return Parse(OperatorId + "*E" + Suffix);

            }

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an EVSE identification.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE identification.</param>
        public static EVSE_Id? TryParse(String Text)
        {

            if (TryParse(Text, out EVSE_Id _EVSEId))
                return _EVSEId;

            return new EVSE_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out EVSEId)

        /// <summary>
        /// Try to parse the given string as an EVSE identification.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE identification.</param>
        /// <param name="EVSEId">The parsed EVSE identification.</param>
        public static Boolean TryParse(String Text, out EVSE_Id EVSEId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                EVSEId = default;
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = EVSEId_RegEx.Matches(Text);

                if (MatchCollection.Count != 1)
                {
                    EVSEId = default;
                    return false;
                }

                if (Operator_Id.TryParse(MatchCollection[0].Groups[1].Value, out Operator_Id OperatorId))
                {

                    EVSEId = new EVSE_Id(OperatorId,
                                         MatchCollection[0].Groups[2].Value);

                    return true;

                }

            }
            catch (Exception)
            { }

            EVSEId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Return a clone of this EVSE identification.
        /// </summary>
        public EVSE_Id Clone

            => new EVSE_Id(OperatorId.Clone,
                           new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEId1, EVSEId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEId1 == null) || ((Object) EVSEId2 == null))
                return false;

            if ((Object) EVSEId1 == null)
                throw new ArgumentNullException(nameof(EVSEId1),  "The given EVSE identification must not be null!");

            return EVSEId1.Equals(EVSEId2);

        }

        #endregion

        #region Operator != (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => !(EVSEId1 == EVSEId2);

        #endregion

        #region Operator <  (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
        {

            if ((Object) EVSEId1 == null)
                throw new ArgumentNullException(nameof(EVSEId1),  "The given EVSE identification must not be null!");

            return EVSEId1.CompareTo(EVSEId2) < 0;

        }

        #endregion

        #region Operator <= (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => !(EVSEId1 > EVSEId2);

        #endregion

        #region Operator >  (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
        {

            if ((Object) EVSEId1 == null)
                throw new ArgumentNullException(nameof(EVSEId1),  "The given EVSE identification must not be null!");

            return EVSEId1.CompareTo(EVSEId2) > 0;

        }

        #endregion

        #region Operator >= (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => !(EVSEId1 < EVSEId2);

        #endregion

        #endregion

        #region IComparable<EVSEId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            if (!(Object is EVSE_Id EVSEId))
                throw new ArgumentException("The given object is not a EVSE identification!", nameof(Object));

            return CompareTo(EVSEId);

        }

        #endregion

        #region CompareTo(EVSEId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId">An object to compare with.</param>
        public Int32 CompareTo(EVSE_Id EVSEId)
        {

            if ((Object) EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),  "The given EVSE identification must not be null!");

            var _Result = OperatorId.CompareTo(EVSEId.OperatorId);

            if (_Result == 0)
                _Result = String.Compare(MinSuffix, EVSEId.MinSuffix, StringComparison.OrdinalIgnoreCase);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            if (!(Object is EVSE_Id EVSEId))
                return false;

            return Equals(EVSEId);

        }

        #endregion

        #region Equals(EVSEId)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSE_Id EVSEId)
        {

            if ((Object) EVSEId == null)
                return false;

            return OperatorId.         Equals(EVSEId.OperatorId) &&
                   MinSuffix.ToLower().Equals(EVSEId.MinSuffix.ToLower());

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => OperatorId.GetHashCode() ^
               MinSuffix. GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            switch (Format)
            {

                case OperatorIdFormats.eMI3:
                    return String.Concat(OperatorId,  "E", Suffix);

                default:
                    return String.Concat(OperatorId, "*E", Suffix);

            }

        }

        #endregion

    }

}
