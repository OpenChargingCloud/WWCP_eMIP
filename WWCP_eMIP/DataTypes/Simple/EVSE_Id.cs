/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// Extension methods for EVSE identifications.
    /// </summary>
    public static class EVSEIdExtensions
    {

        /// <summary>
        /// Indicates whether this EVSE identification is null or empty.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        public static Boolean IsNullOrEmpty(this EVSE_Id? EVSEId)
            => !EVSEId.HasValue || EVSEId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this EVSE identification is NOT null or empty.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        public static Boolean IsNotNullOrEmpty(this EVSE_Id? EVSEId)
            => EVSEId.HasValue && EVSEId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public readonly struct EVSE_Id : IId,
                                     IEquatable<EVSE_Id>,
                                     IComparable<EVSE_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an EVSE identification.
        /// All '*' are optional!
        /// </summary>
        public  static readonly Regex  EVSEId_RegEx  = new (@"^([A-Za-z]{2}\*?[A-Za-z0-9]{3})\*?E([A-Za-z0-9\*]{1,30})$",
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
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => Suffix.IsNotNullOrEmpty();

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
        public static EVSE_Id Random(Operator_Id            OperatorId,
                                     Byte                   Length  = 10,
                                     Func<String, String>?  Mapper  = null)

            => new (OperatorId,
                    (Mapper ?? (_ => _)) (RandomExtensions.RandomString((UInt16) (Length < 10 ? 10 : Length > 50 ? 50 : Length))));

        #endregion

        #region (static) Parse(Text)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE identification.</param>
        public static EVSE_Id Parse(String Text)
        {

            #region Initial checks

            Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text),  "The given text representation of an EVSE identification must not be null or empty!");

            #endregion

            var matchCollection = EVSEId_RegEx.Matches(Text);

            if (matchCollection.Count != 1)
                throw new ArgumentException($"Illegal EVSE identification '{Text}'!",
                                            nameof(Text));


            if (Operator_Id.TryParse(matchCollection[0].Groups[1].Value, out var operatorId))
                return new EVSE_Id(operatorId,
                                   matchCollection[0].Groups[2].Value);

            throw new ArgumentException($"Invalid text representation of an EVSE identification: '{Text}'!",
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

            => OperatorId.Format switch {
                   OperatorIdFormats.eMI3  => Parse(OperatorId +  "E" + Suffix),
                   _                       => Parse(OperatorId + "*E" + Suffix),
               };

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an EVSE identification.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE identification.</param>
        public static EVSE_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var evseId))
                return evseId;

            return null;

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

                if (Operator_Id.TryParse(MatchCollection[0].Groups[1].Value, out var operatorId))
                {

                    EVSEId = new EVSE_Id(
                                 operatorId,
                                 MatchCollection[0].Groups[2].Value
                             );

                    return true;

                }

            }
            catch
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

            => new (OperatorId.Clone,
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
        public static Boolean operator == (EVSE_Id EVSEId1,
                                           EVSE_Id EVSEId2)

            => EVSEId1.Equals(EVSEId2);

        #endregion

        #region Operator != (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSE_Id EVSEId1,
                                           EVSE_Id EVSEId2)

            => !EVSEId1.Equals(EVSEId2);

        #endregion

        #region Operator <  (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSE_Id EVSEId1,
                                          EVSE_Id EVSEId2)

            => EVSEId1.CompareTo(EVSEId2) < 0;

        #endregion

        #region Operator <= (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSE_Id EVSEId1,
                                           EVSE_Id EVSEId2)

            => EVSEId1.CompareTo(EVSEId2) <= 0;

        #endregion

        #region Operator >  (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSE_Id EVSEId1,
                                          EVSE_Id EVSEId2)

            => EVSEId1.CompareTo(EVSEId2) > 0;

        #endregion

        #region Operator >= (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSE_Id EVSEId1,
                                           EVSE_Id EVSEId2)

            => EVSEId1.CompareTo(EVSEId2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSE_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two EVSE identifications.
        /// </summary>
        /// <param name="Object">An EVSE identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSE_Id EVSEId
                   ? CompareTo(EVSEId)
                   : throw new ArgumentException("The given object is not an EVSE identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVSEId)

        /// <summary>
        /// Compares two EVSE identifications.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification to compare with.</param>
        public Int32 CompareTo(EVSE_Id EVSEId)
        {

            var c = OperatorId.CompareTo(EVSEId.OperatorId);

            if (c == 0)
                c = String.Compare(MinSuffix,
                                   EVSEId.MinSuffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EVSE_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="Object">An EVSE identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EVSE_Id EVSEId &&
                   Equals(EVSEId);

        #endregion

        #region Equals(EVSEId)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification to compare with.</param>
        public Boolean Equals(EVSE_Id EVSEId)

            => OperatorId.Equals(EVSEId.OperatorId) &&

               String.Equals(MinSuffix,
                             EVSEId.MinSuffix,
                             StringComparison.OrdinalIgnoreCase);

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

            => Format switch {
                   OperatorIdFormats.eMI3  => String.Concat(OperatorId,  "E", Suffix),
                   _                       => String.Concat(OperatorId, "*E", Suffix)
               };

        #endregion

    }

}
