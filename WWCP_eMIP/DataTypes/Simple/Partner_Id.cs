/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
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
    /// The unique identification of a partner.
    /// </summary>
    public struct Partner_Id : IId,
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
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
        {
            get
            {

                switch (Format)
                {

                    case PartnerIdFormats.eMI3_STAR:
                        return (UInt64) (CountryCode.Alpha2Code.Length             + 1 + Suffix.Length);

                    default:  // eMI3
                        return (UInt64) (CountryCode.Alpha2Code.Length                 + Suffix.Length);

                }

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
        private Partner_Id(Country            CountryCode,
                           String             Suffix,
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

            var MatchCollection = PartnerId_RegEx.Matches(Text.Trim().ToUpper());

            if (MatchCollection.Count == 1 &&
                Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out Country _CountryCode))
            {

                return new Partner_Id(_CountryCode,
                                       MatchCollection[0].Groups[3].Value,
                                       MatchCollection[0].Groups[2].Value == "*" ? PartnerIdFormats.eMI3_STAR : PartnerIdFormats.eMI3);

            }

            throw new ArgumentException("Illegal text representation of a charging operator identification: '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region Parse(CountryCode, Suffix, IdFormat = IdFormatType.eMI3)

        /// <summary>
        /// Parse the given string as an charging operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an charging operator identification.</param>
        /// <param name="IdFormat">The format of the charging operator identification [old|new].</param>
        public static Partner_Id Parse(Country            CountryCode,
                                        String             Suffix,
                                        PartnerIdFormats  IdFormat = PartnerIdFormats.eMI3)
        {

            #region Initial checks

            if (CountryCode == null)
                throw new ArgumentNullException(nameof(CountryCode),  "The given country must not be null!");

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),       "The given charging operator identification suffix must not be null or empty!");

            #endregion

            switch (IdFormat)
            {

                case PartnerIdFormats.eMI3:
                    return Parse(CountryCode.Alpha2Code +       Suffix);

                default:
                    return Parse(CountryCode.Alpha2Code + "*" + Suffix);

            }

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of a charging operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging operator identification.</param>
        public static Partner_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Partner_Id _PartnerId))
                return _PartnerId;

            return new Partner_Id?();

        }

        #endregion

        #region TryParse(Text, out PartnerId)

        /// <summary>
        /// Try to parse the given text representation of a charging operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging operator identification.</param>
        /// <param name="PartnerId">The parsed charging operator identification.</param>
        public static Boolean TryParse(String           Text,
                                       out Partner_Id  PartnerId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                PartnerId = default(Partner_Id);
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = PartnerId_RegEx.Matches(Text.Trim().ToUpper());

                if (MatchCollection.Count == 1 &&
                    Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out Country _CountryCode))
                {

                    PartnerId = new Partner_Id(_CountryCode,
                                                 MatchCollection[0].Groups[3].Value,
                                                 MatchCollection[0].Groups[2].Value == "*" ? PartnerIdFormats.eMI3_STAR : PartnerIdFormats.eMI3);

                    return true;

                }

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            PartnerId = default(Partner_Id);
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

            if (TryParse(CountryCode, Suffix, out Partner_Id _PartnerId, IdFormat))
                return _PartnerId;

            return new Partner_Id?();

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
        public static Boolean TryParse(Country            CountryCode,
                                       String             Suffix,
                                       out Partner_Id    PartnerId,
                                       PartnerIdFormats  IdFormat = PartnerIdFormats.eMI3_STAR)
        {

            #region Initial checks

            if (CountryCode == null || Suffix.IsNullOrEmpty() || Suffix.Trim().IsNullOrEmpty())
            {
                PartnerId = default(Partner_Id);
                return false;
            }

            #endregion

            switch (IdFormat)
            {

                case PartnerIdFormats.eMI3:
                    return TryParse(CountryCode.Alpha2Code +       Suffix,
                                    out PartnerId);

                default:_STAR:
                    return TryParse(CountryCode.Alpha2Code + "*" + Suffix,
                                    out PartnerId);

            }

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging operator identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging operator identification format.</param>
        public Partner_Id ChangeFormat(PartnerIdFormats NewFormat)

            => new Partner_Id(CountryCode,
                               Suffix,
                               NewFormat);

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging operator identification.
        /// </summary>
        public Partner_Id Clone

            => new Partner_Id(CountryCode,
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
        public static Boolean operator == (Partner_Id PartnerId1, Partner_Id PartnerId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PartnerId1, PartnerId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PartnerId1 == null) || ((Object) PartnerId2 == null))
                return false;

            return PartnerId1.Equals(PartnerId2);

        }

        #endregion

        #region Operator != (PartnerId1, PartnerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerId1">An charging operator identification.</param>
        /// <param name="PartnerId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Partner_Id PartnerId1, Partner_Id PartnerId2)
            => !(PartnerId1 == PartnerId2);

        #endregion

        #region Operator <  (PartnerId1, PartnerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerId1">An charging operator identification.</param>
        /// <param name="PartnerId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Partner_Id PartnerId1, Partner_Id PartnerId2)
        {

            if ((Object) PartnerId1 == null)
                throw new ArgumentNullException(nameof(PartnerId1), "The given PartnerId1 must not be null!");

            return PartnerId1.CompareTo(PartnerId2) < 0;

        }

        #endregion

        #region Operator <= (PartnerId1, PartnerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerId1">An charging operator identification.</param>
        /// <param name="PartnerId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Partner_Id PartnerId1, Partner_Id PartnerId2)
            => !(PartnerId1 > PartnerId2);

        #endregion

        #region Operator >  (PartnerId1, PartnerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerId1">An charging operator identification.</param>
        /// <param name="PartnerId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Partner_Id PartnerId1, Partner_Id PartnerId2)
        {

            if ((Object) PartnerId1 == null)
                throw new ArgumentNullException(nameof(PartnerId1), "The given PartnerId1 must not be null!");

            return PartnerId1.CompareTo(PartnerId2) > 0;

        }

        #endregion

        #region Operator >= (PartnerId1, PartnerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerId1">An charging operator identification.</param>
        /// <param name="PartnerId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Partner_Id PartnerId1, Partner_Id PartnerId2)
            => !(PartnerId1 < PartnerId2);

        #endregion

        #endregion

        #region IComparable<PartnerId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Partner_Id PartnerId))
                throw new ArgumentException("The given object is not a charging operator identification!", nameof(Object));

            return CompareTo(PartnerId);

        }

        #endregion

        #region CompareTo(PartnerId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerId">An object to compare with.</param>
        public Int32 CompareTo(Partner_Id PartnerId)
        {

            if ((Object) PartnerId == null)
                throw new ArgumentNullException(nameof(PartnerId), "The given charging operator identification must not be null!");

            var _Result = CountryCode.CompareTo(PartnerId.CountryCode);

            if (_Result == 0)
                _Result = String.Compare(Suffix, PartnerId.Suffix, StringComparison.OrdinalIgnoreCase);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<PartnerId> Members

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

            if (!(Object is Partner_Id PartnerId))
                return false;

            return Equals(PartnerId);

        }

        #endregion

        #region Equals(PartnerId)

        /// <summary>
        /// Compares two PartnerIds for equality.
        /// </summary>
        /// <param name="PartnerId">A PartnerId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Partner_Id PartnerId)
        {

            if ((Object) PartnerId == null)
                return false;

            return CountryCode.     Equals(PartnerId.CountryCode) &&
                   Suffix.ToLower().Equals(PartnerId.Suffix.ToLower());

        }

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
        {

            switch (Format)
            {

                case PartnerIdFormats.eMI3_STAR:
                    return CountryCode.Alpha2Code + "*" + Suffix;

                default:
                    return CountryCode.Alpha2Code       + Suffix;

            }

        }

        #endregion

        #region ToString(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToString(PartnerIdFormats Format)
        {

            switch (Format)
            {

                case PartnerIdFormats.eMI3:
                    return String.Concat(CountryCode.Alpha2Code,
                                         Suffix);

                case PartnerIdFormats.eMI3_STAR:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "*",
                                         Suffix);

                default: // DIN
                    return String.Concat("+",
                                         CountryCode.TelefonCode,
                                         "*",
                                         Suffix);

            }

        }

        #endregion

    }

}
