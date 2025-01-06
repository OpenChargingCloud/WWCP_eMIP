/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

        #region AsText  (this PartnerIdFormat)

        /// <summary>
        /// Return a text representation of the given partner identification format.
        /// </summary>
        /// <param name="PartnerIdFormat">A partner identification format.</param>
        public static String AsText(this PartnerIdFormats PartnerIdFormat)
        {

            switch (PartnerIdFormat)
            {

                case PartnerIdFormats.eMI3:
                    return "eMI3";

                case PartnerIdFormats.eMI3_STAR:
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
    public enum PartnerIdFormats
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
    /// Extension methods for partner identifications.
    /// </summary>
    public static class PartnerIdExtensions
    {

        /// <summary>
        /// Indicates whether this partner identification is null or empty.
        /// </summary>
        /// <param name="PartnerId">A partner identification.</param>
        public static Boolean IsNullOrEmpty(this Partner_Id? PartnerId)
            => !PartnerId.HasValue || PartnerId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this partner identification is NOT null or empty.
        /// </summary>
        /// <param name="PartnerId">A partner identification.</param>
        public static Boolean IsNotNullOrEmpty(this Partner_Id? PartnerId)
            => PartnerId.HasValue && PartnerId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a partner.
    /// </summary>
    public readonly struct Partner_Id : IId,
                                        IEquatable<Partner_Id>,
                                        IComparable<Partner_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an eMIP charging operator identification.
        /// </summary>
        public static readonly Regex  PartnerId_RegEx  = new Regex(@"^([A-Z]{2})(\*?)([A-Z0-9]{3})$",
                                                                    RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country           CountryCode   { get; }

        /// <summary>
        /// The identificator suffix.
        /// </summary>
        public String            Suffix        { get; }

        /// <summary>
        /// The format of the charging operator identification.
        /// </summary>
        public PartnerIdFormats  Format        { get; }

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
                    PartnerIdFormats.eMI3_STAR  => (UInt64) (CountryCode.Alpha2Code.Length + 1 + Suffix.Length),
                    // eMI3
                    _                           => (UInt64) (CountryCode.Alpha2Code.Length +     Suffix.Length),
                };
            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging operator identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="Suffix">The suffix of the charging operator identification.</param>
        /// <param name="Format">The format of the charging operator identification.</param>
        private Partner_Id(Country           CountryCode,
                           String            Suffix,
                           PartnerIdFormats  Format = PartnerIdFormats.eMI3)
        {

            this.CountryCode  = CountryCode;
            this.Suffix       = Suffix;
            this.Format       = Format;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given text representation of a charging operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging operator identification.</param>
        public static Partner_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty() || Text.Trim().IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging operator identification must not be null or empty!");

            #endregion

            var matchCollection = PartnerId_RegEx.Matches(Text.Trim().ToUpper());

            if (matchCollection.Count == 1 &&
                Country.TryParseAlpha2Code(matchCollection[0].Groups[1].Value, out var countryCode))
            {

                return new Partner_Id(
                           countryCode,
                           matchCollection[0].Groups[3].Value,
                           matchCollection[0].Groups[2].Value == "*"
                               ? PartnerIdFormats.eMI3_STAR
                               : PartnerIdFormats.eMI3
                       );

            }

            throw new ArgumentException($"Invalid text representation of a partner identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(CountryCode, Suffix, IdFormat = IdFormatType.eMI3)

        /// <summary>
        /// Parse the given string as an charging operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an charging operator identification.</param>
        /// <param name="IdFormat">The format of the charging operator identification [old|new].</param>
        public static Partner_Id Parse(Country           CountryCode,
                                       String            Suffix,
                                       PartnerIdFormats  IdFormat = PartnerIdFormats.eMI3)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The given charging operator identification suffix must not be null or empty!");

            #endregion

            return IdFormat switch {
                PartnerIdFormats.eMI3  => Parse(CountryCode.Alpha2Code +       Suffix),
                _                      => Parse(CountryCode.Alpha2Code + "*" + Suffix)
            };

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of a charging operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging operator identification.</param>
        public static Partner_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var partnerId))
                return partnerId;

            return null;

        }

        #endregion

        #region TryParse(Text, out PartnerId)

        /// <summary>
        /// Try to parse the given text representation of a charging operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging operator identification.</param>
        /// <param name="PartnerId">The parsed charging operator identification.</param>
        public static Boolean TryParse(String          Text,
                                       out Partner_Id  PartnerId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                PartnerId = default;
                return false;
            }

            #endregion

            try
            {

                var matchCollection = PartnerId_RegEx.Matches(Text.Trim().ToUpper());

                if (matchCollection.Count == 1 &&
                    Country.TryParseAlpha2Code(matchCollection[0].Groups[1].Value, out var countryCode))
                {

                    PartnerId = new Partner_Id(
                                    countryCode,
                                    matchCollection[0].Groups[3].Value,
                                    matchCollection[0].Groups[2].Value == "*"
                                        ? PartnerIdFormats.eMI3_STAR
                                        : PartnerIdFormats.eMI3
                                );

                    return true;

                }

            }
            catch
            { }

            PartnerId = default;
            return false;

        }

        #endregion

        #region TryParse(CountryCode, Suffix, IdFormat = PartnerIdFormats.eMI3_STAR)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility operator identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility operator identification.</param>
        public static Partner_Id? TryParse(Country           CountryCode,
                                           String            Suffix,
                                           PartnerIdFormats  IdFormat = PartnerIdFormats.eMI3_STAR)
        {

            if (TryParse(CountryCode, Suffix, out var partnerId, IdFormat))
                return partnerId;

            return null;

        }

        #endregion

        #region TryParse(CountryCode, Suffix, out PartnerId, IdFormat = PartnerIdFormats.eMI3_STAR)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility operator identification.</param>
        /// <param name="PartnerId">The parsed e-mobility operator identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility operator identification.</param>
        public static Boolean TryParse(Country           CountryCode,
                                       String            Suffix,
                                       out Partner_Id    PartnerId,
                                       PartnerIdFormats  IdFormat = PartnerIdFormats.eMI3_STAR)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty() || Suffix.Trim().IsNullOrEmpty())
            {
                PartnerId = default;
                return false;
            }

            #endregion

            return IdFormat switch {
                PartnerIdFormats.eMI3  => TryParse(CountryCode.Alpha2Code +       Suffix, out PartnerId),
                _                      => TryParse(CountryCode.Alpha2Code + "*" + Suffix, out PartnerId)
            };

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging operator identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging operator identification format.</param>
        public Partner_Id ChangeFormat(PartnerIdFormats NewFormat)

            => new (CountryCode,
                    Suffix,
                    NewFormat);

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging operator identification.
        /// </summary>
        public Partner_Id Clone

            => new (CountryCode,
                    new String(Suffix.ToCharArray()),
                    Format);

        #endregion


        #region Operator overloading

        #region Operator == (PartnerId1, PartnerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerId1">An charging operator identification.</param>
        /// <param name="PartnerId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Partner_Id PartnerId1,
                                           Partner_Id PartnerId2)

            => PartnerId1.Equals(PartnerId2);

        #endregion

        #region Operator != (PartnerId1, PartnerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerId1">An charging operator identification.</param>
        /// <param name="PartnerId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Partner_Id PartnerId1,
                                           Partner_Id PartnerId2)

            => !PartnerId1.Equals(PartnerId2);

        #endregion

        #region Operator <  (PartnerId1, PartnerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerId1">An charging operator identification.</param>
        /// <param name="PartnerId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Partner_Id PartnerId1,
                                          Partner_Id PartnerId2)

            => PartnerId1.CompareTo(PartnerId2) < 0;

        #endregion

        #region Operator <= (PartnerId1, PartnerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerId1">An charging operator identification.</param>
        /// <param name="PartnerId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Partner_Id PartnerId1,
                                           Partner_Id PartnerId2)

            => PartnerId1.CompareTo(PartnerId2) <= 0;

        #endregion

        #region Operator >  (PartnerId1, PartnerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerId1">An charging operator identification.</param>
        /// <param name="PartnerId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Partner_Id PartnerId1,
                                          Partner_Id PartnerId2)

            => PartnerId1.CompareTo(PartnerId2) > 0;

        #endregion

        #region Operator >= (PartnerId1, PartnerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerId1">An charging operator identification.</param>
        /// <param name="PartnerId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Partner_Id PartnerId1,
                                           Partner_Id PartnerId2)

            => PartnerId1.CompareTo(PartnerId2) >= 0;

        #endregion

        #endregion

        #region IComparable<Partner_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two partner identifications.
        /// </summary>
        /// <param name="Object">A partner identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Partner_Id partnerId
                   ? CompareTo(partnerId)
                   : throw new ArgumentException("The given object is not a partner identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(PartnerId)

        /// <summary>
        /// Compares two partner identifications.
        /// </summary>
        /// <param name="PartnerId">A partner identification to compare with.</param>
        public Int32 CompareTo(Partner_Id PartnerId)
        {

            var c = CountryCode.CompareTo(PartnerId.CountryCode);

            if (c == 0)
                c = String.Compare(Suffix,
                                   PartnerId.Suffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<Partner_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two partner identifications for equality.
        /// </summary>
        /// <param name="Object">A partner identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Partner_Id partnerId &&
                   Equals(partnerId);

        #endregion

        #region Equals(PartnerId)

        /// <summary>
        /// Compares two partner identifications for equality.
        /// </summary>
        /// <param name="PartnerId">A partner identification to compare with.</param>
        public Boolean Equals(Partner_Id PartnerId)

            => CountryCode.Equals(PartnerId.CountryCode) &&

               String.Equals(Suffix,
                             PartnerId.Suffix,
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
                PartnerIdFormats.eMI3_STAR  => CountryCode.Alpha2Code + "*" + Suffix,
                _                           => CountryCode.Alpha2Code + Suffix
            };

        #endregion

        #region ToString(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToString(PartnerIdFormats Format)

            => Format switch {
                   PartnerIdFormats.eMI3       => String.Concat(     CountryCode.Alpha2Code,       Suffix),
                   PartnerIdFormats.eMI3_STAR  => String.Concat(     CountryCode.Alpha2Code,  "*", Suffix),
                   // DIN
                   _                           => String.Concat("+", CountryCode.TelefonCode, "*", Suffix)
               };

        #endregion

    }

}
