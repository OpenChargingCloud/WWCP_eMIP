/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
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
    /// eMIP data conversion methods.
    /// </summary>
    public static partial class ConversionMethods
    {

        #region AsText  (this PartnerOperatorIdFormat)

        /// <summary>
        /// Return a text representation of the given operator identification format.
        /// </summary>
        /// <param name="PartnerOperatorIdFormat">A operator identification format.</param>
        public static String AsText(this PartnerOperatorIdFormats PartnerOperatorIdFormat)
        {

            switch (PartnerOperatorIdFormat)
            {

                case PartnerOperatorIdFormats.eMI3:
                case PartnerOperatorIdFormats.eMI3_STAR:
                    return "eMI3";

                default:
                    return "Gireve";

            }

        }

        #endregion

    }

    /// <summary>
    /// The different formats of operator identifications.
    /// </summary>
    public enum PartnerOperatorIdFormats
    {

        /// <summary>
        /// The eMI3 format.
        /// </summary>
        eMI3,

        /// <summary>
        /// The eMI3 format with a '*' as separator.
        /// </summary>
        eMI3_STAR,

        /// <summary>
        /// Proprietary Gireve format.
        /// </summary>
        Gireve

    }

    /// <summary>
    /// Extension methods for partner operator identifications.
    /// </summary>
    public static class PartnerOperatorIdExtensions
    {

        /// <summary>
        /// Indicates whether this partner operator identification is null or empty.
        /// </summary>
        /// <param name="PartnerOperatorId">A partner operator identification.</param>
        public static Boolean IsNullOrEmpty(this PartnerOperator_Id? PartnerOperatorId)
            => !PartnerOperatorId.HasValue || PartnerOperatorId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this partner operator identification is NOT null or empty.
        /// </summary>
        /// <param name="PartnerOperatorId">A partner operator identification.</param>
        public static Boolean IsNotNullOrEmpty(this PartnerOperator_Id? PartnerOperatorId)
            => PartnerOperatorId.HasValue && PartnerOperatorId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a partner operator.
    /// Gireve sometimes uses AA-BBB and sometimes AA*BBB!
    /// </summary>
    public readonly struct PartnerOperator_Id : IId,
                                                IEquatable<PartnerOperator_Id>,
                                                IComparable<PartnerOperator_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an eMIP charging partner operator identification.
        /// </summary>
        public static readonly Regex  PartnerOperatorId_RegEx  = new Regex(@"^([A-Z]{2})([\*|\-]?)([A-Z0-9]{3})$",
                                                                           RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country                   CountryCode    { get; }

        /// <summary>
        /// The identificator suffix.
        /// </summary>
        public String                    Suffix         { get; }

        /// <summary>
        /// The format of the charging partner operator identification.
        /// </summary>
        public PartnerOperatorIdFormats  Format         { get; }

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
                return Format switch {
                    PartnerOperatorIdFormats.eMI3_STAR  => (UInt64) (CountryCode.Alpha2Code.Length + 1 + Suffix.Length),
                    _                                   => (UInt64) (CountryCode.Alpha2Code.Length +     Suffix.Length)
                };
            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging partner operator identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="Suffix">The suffix of the charging partner operator identification.</param>
        /// <param name="Format">The format of the charging partner operator identification.</param>
        private PartnerOperator_Id(Country                   CountryCode,
                                   String                    Suffix,
                                   PartnerOperatorIdFormats  Format   = PartnerOperatorIdFormats.eMI3_STAR)
        {

            this.CountryCode  = CountryCode;
            this.Suffix       = Suffix;
            this.Format       = Format;

        }

        #endregion


        #region (static) Parse(Text)

        /// <summary>
        /// Parse the given text representation of a partner operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner operator identification.</param>
        public static PartnerOperator_Id Parse(String Text)
        {

            #region Initial checks

            Text = Text.Trim().ToUpper();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a partner operator identification must not be null or empty!");

            #endregion

            var matchCollection = PartnerOperatorId_RegEx.Matches(Text);

            if (matchCollection.Count == 1 &&
                Country.TryParseAlpha2Code(matchCollection[0].Groups[1].Value, out var countryCode))
            {

                return new PartnerOperator_Id(
                           countryCode,
                           matchCollection[0].Groups[3].Value,
                           matchCollection[0].Groups[2].Value == "*"
                               ? PartnerOperatorIdFormats.eMI3_STAR
                               : PartnerOperatorIdFormats.eMI3
                       );

            }

            throw new ArgumentException($"Invalid text representation of a partner operator identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse(CountryCode, Suffix, IdFormat = IdFormatType.eMI3_STAR)

        /// <summary>
        /// Parse the given string as an charging operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an charging operator identification.</param>
        /// <param name="IdFormat">The format of the charging operator identification [old|new].</param>
        public static PartnerOperator_Id Parse(Country                   CountryCode,
                                               String                    Suffix,
                                               PartnerOperatorIdFormats  IdFormat   = PartnerOperatorIdFormats.eMI3_STAR)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),  "The given charging operator identification suffix must not be null or empty!");

            #endregion

            return IdFormat switch {
                PartnerOperatorIdFormats.eMI3  => Parse(CountryCode.Alpha2Code +       Suffix),
                _                              => Parse(CountryCode.Alpha2Code + "*" + Suffix)
            };

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of a partner operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner operator identification.</param>
        public static PartnerOperator_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var partnerOperatorId))
                return partnerOperatorId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out PartnerOperatorId)

        /// <summary>
        /// Try to parse the given text representation of a partner operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner operator identification.</param>
        /// <param name="PartnerOperatorId">The parsed charging operator identification.</param>
        public static Boolean TryParse(String                  Text,
                                       out PartnerOperator_Id  PartnerOperatorId)
        {

            #region Initial checks

            Text = Text.Trim().ToUpper();

            if (Text.IsNullOrEmpty())
            {
                PartnerOperatorId = default;
                return false;
            }

            #endregion

            try
            {

                var matchCollection = PartnerOperatorId_RegEx.Matches(Text);

                if (matchCollection.Count == 1 &&
                    Country.TryParseAlpha2Code(matchCollection[0].Groups[1].Value, out var countryCode))
                {

                    PartnerOperatorId = new PartnerOperator_Id(
                                            countryCode,
                                            matchCollection[0].Groups[3].Value,
                                            matchCollection[0].Groups[2].Value == "*"
                                                ? PartnerOperatorIdFormats.eMI3_STAR
                                                : PartnerOperatorIdFormats.eMI3
                                        );

                    return true;

                }

            }
            catch
            { }

            PartnerOperatorId = default;
            return false;

        }

        #endregion

        #region (static) TryParse(CountryCode, Suffix, IdFormat = PartnerOperatorIdFormats.eMI3_STAR)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility operator identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility operator identification.</param>
        public static PartnerOperator_Id? TryParse(Country                   CountryCode,
                                                   String                    Suffix,
                                                   PartnerOperatorIdFormats  IdFormat   = PartnerOperatorIdFormats.eMI3_STAR)
        {

            if (TryParse(CountryCode, Suffix, out var partnerOperatorId, IdFormat))
                return partnerOperatorId;

            return null;

        }

        #endregion

        #region (static) TryParse(CountryCode, Suffix, out PartnerOperatorId, IdFormat = PartnerOperatorIdFormats.eMI3_STAR)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility operator identification.</param>
        /// <param name="PartnerOperatorId">The parsed e-mobility operator identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility operator identification.</param>
        public static Boolean TryParse(Country                   CountryCode,
                                       String                    Suffix,
                                       out PartnerOperator_Id    PartnerOperatorId,
                                       PartnerOperatorIdFormats  IdFormat   = PartnerOperatorIdFormats.eMI3_STAR)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty() || Suffix.Trim().IsNullOrEmpty())
            {
                PartnerOperatorId = default;
                return false;
            }

            #endregion

            return IdFormat switch {
                PartnerOperatorIdFormats.eMI3  => TryParse(CountryCode.Alpha2Code +       Suffix, out PartnerOperatorId),
                _                              => TryParse(CountryCode.Alpha2Code + "*" + Suffix, out PartnerOperatorId)
            };

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging operator identification.
        /// </summary>
        public PartnerOperator_Id Clone

            => new (CountryCode,
                    new String(Suffix.ToCharArray()),
                    Format);

        #endregion


        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging operator identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging operator identification format.</param>
        public PartnerOperator_Id ChangeFormat(PartnerOperatorIdFormats NewFormat)

            => new (CountryCode,
                    Suffix,
                    NewFormat);

        #endregion


        #region PartnerOperator overloading

        #region PartnerOperator == (PartnerOperatorId1, PartnerOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerOperatorId1">An charging operator identification.</param>
        /// <param name="PartnerOperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (PartnerOperator_Id PartnerOperatorId1,
                                           PartnerOperator_Id PartnerOperatorId2)

            => PartnerOperatorId1.Equals(PartnerOperatorId2);

        #endregion

        #region PartnerOperator != (PartnerOperatorId1, PartnerOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerOperatorId1">An charging operator identification.</param>
        /// <param name="PartnerOperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PartnerOperator_Id PartnerOperatorId1,
                                           PartnerOperator_Id PartnerOperatorId2)

            => !PartnerOperatorId1.Equals(PartnerOperatorId2);

        #endregion

        #region PartnerOperator <  (PartnerOperatorId1, PartnerOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerOperatorId1">An charging operator identification.</param>
        /// <param name="PartnerOperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (PartnerOperator_Id PartnerOperatorId1,
                                          PartnerOperator_Id PartnerOperatorId2)

            => PartnerOperatorId1.CompareTo(PartnerOperatorId2) < 0;

        #endregion

        #region PartnerOperator <= (PartnerOperatorId1, PartnerOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerOperatorId1">An charging operator identification.</param>
        /// <param name="PartnerOperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (PartnerOperator_Id PartnerOperatorId1,
                                           PartnerOperator_Id PartnerOperatorId2)

            => PartnerOperatorId1.CompareTo(PartnerOperatorId2) <= 0;

        #endregion

        #region PartnerOperator >  (PartnerOperatorId1, PartnerOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerOperatorId1">An charging operator identification.</param>
        /// <param name="PartnerOperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (PartnerOperator_Id PartnerOperatorId1,
                                          PartnerOperator_Id PartnerOperatorId2)

            => PartnerOperatorId1.CompareTo(PartnerOperatorId2) > 0;

        #endregion

        #region PartnerOperator >= (PartnerOperatorId1, PartnerOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerOperatorId1">An charging operator identification.</param>
        /// <param name="PartnerOperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (PartnerOperator_Id PartnerOperatorId1,
                                           PartnerOperator_Id PartnerOperatorId2)

            => PartnerOperatorId1.CompareTo(PartnerOperatorId2) >= 0;

        #endregion

        #endregion

        #region IComparable<PartnerOperator_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two partner operator identifications.
        /// </summary>
        /// <param name="Object">A partner operator identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Booking_Id partnerOperatorId
                   ? CompareTo(partnerOperatorId)
                   : throw new ArgumentException("The given object is not a partner operator identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(PartnerOperatorId)

        /// <summary>
        /// Compares two partner operator identifications.
        /// </summary>
        /// <param name="PartnerOperatorId">A partner operator identification to compare with.</param>
        public Int32 CompareTo(PartnerOperator_Id PartnerOperatorId)
        {

            var c = CountryCode.CompareTo(PartnerOperatorId.CountryCode);

            if (c == 0)
                c = String.Compare(Suffix,
                                   PartnerOperatorId.Suffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<PartnerOperator_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two partner operator identifications for equality.
        /// </summary>
        /// <param name="Object">A partner operator identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Booking_Id partnerOperatorId &&
                   Equals(partnerOperatorId);

        #endregion

        #region Equals(PartnerOperatorId)

        /// <summary>
        /// Compares two partner operator identifications for equality.
        /// </summary>
        /// <param name="PartnerOperatorId">A partner operator identification to compare with.</param>
        public Boolean Equals(PartnerOperator_Id PartnerOperatorId)

            => CountryCode.Equals(PartnerOperatorId.CountryCode) &&

               String.Equals(Suffix,
                             PartnerOperatorId.Suffix,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => CountryCode.GetHashCode() ^
               Suffix.     GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Format switch {
                   PartnerOperatorIdFormats.eMI3_STAR  => CountryCode.Alpha2Code + "*" + Suffix,
                   _                                   => CountryCode.Alpha2Code +       Suffix
               };

        #endregion

    }

}
