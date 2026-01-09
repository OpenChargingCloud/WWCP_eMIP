/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

        #region AsText  (this ProviderIdFormat)

        /// <summary>
        /// Return a text representation of the given provider identification format.
        /// </summary>
        /// <param name="ProviderIdFormat">A provider identification format.</param>
        public static String AsText(this ProviderIdFormats ProviderIdFormat)
        {

            switch (ProviderIdFormat)
            {

                case ProviderIdFormats.eMI3:
                    return "eMI3";

                case ProviderIdFormats.eMI3_HYPHEN:
                    return "eMI3";

                default:
                    return "Gireve";

            }

        }

        #endregion

    }

    /// <summary>
    /// The different formats of e-mobility provider identifications.
    /// </summary>
    public enum ProviderIdFormats
    {

        /// <summary>
        /// The eMI3 format.
        /// </summary>
        eMI3,

        /// <summary>
        /// The eMI3 format with a '-' as separator.
        /// </summary>
        eMI3_HYPHEN,

        /// <summary>
        /// Proprietary Gireve format.
        /// </summary>
        Gireve

    }

    /// <summary>
    /// Extension methods for provider identifications.
    /// </summary>
    public static class ProviderIdExtensions
    {

        /// <summary>
        /// Indicates whether this provider identification is null or empty.
        /// </summary>
        /// <param name="ProviderId">A provider identification.</param>
        public static Boolean IsNullOrEmpty(this Provider_Id? ProviderId)
            => !ProviderId.HasValue || ProviderId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this provider identification is NOT null or empty.
        /// </summary>
        /// <param name="ProviderId">A provider identification.</param>
        public static Boolean IsNotNullOrEmpty(this Provider_Id? ProviderId)
            => ProviderId.HasValue && ProviderId.Value.IsNotNullOrEmpty;

    }



    /// <summary>
    /// The unique identification of an e-mobility provider.
    /// </summary>
    public readonly struct Provider_Id : IId,
                                         IEquatable<Provider_Id>,
                                         IComparable<Provider_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an eMIP charging e-mobility provider identification.
        /// </summary>
        public static readonly Regex  ProviderId_RegEx  = new Regex(@"^([A-Z]{2})(\-?)([A-Z0-9]{3})$",
                                                                    RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country            CountryCode   { get; }

        /// <summary>
        /// The identificator suffix.
        /// </summary>
        public String             Suffix        { get; }

        /// <summary>
        /// The format of the charging e-mobility provider identification.
        /// </summary>
        public ProviderIdFormats  Format        { get; }

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
                    ProviderIdFormats.eMI3_HYPHEN  => (UInt64) (CountryCode.Alpha2Code.Length + 1 + Suffix.Length),
                    _                              => (UInt64) (CountryCode.Alpha2Code.Length +     Suffix.Length),
                };
            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="Suffix">The suffix of the charging e-mobility provider identification.</param>
        /// <param name="Format">The format of the charging e-mobility provider identification.</param>
        private Provider_Id(Country            CountryCode,
                            String             Suffix,
                            ProviderIdFormats  Format = ProviderIdFormats.eMI3_HYPHEN)
        {

            this.CountryCode  = CountryCode;
            this.Suffix       = Suffix;
            this.Format       = Format;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        public static Provider_Id Parse(String Text)
        {

            #region Initial checks

            Text = Text.Trim().ToUpper();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an e-mobility provider identification must not be null or empty!");

            #endregion

            var matchCollection = ProviderId_RegEx.Matches(Text);

            if (matchCollection.Count == 1 &&
                Country.TryParseAlpha2Code(matchCollection[0].Groups[1].Value, out var countryCode))
            {

                return new Provider_Id(
                           countryCode,
                           matchCollection[0].Groups[3].Value,
                           matchCollection[0].Groups[2].Value == "-"
                               ? ProviderIdFormats.eMI3_HYPHEN
                               : ProviderIdFormats.eMI3
                       );

            }

            throw new ArgumentException($"Invalid text representation of a provider identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(CountryCode, Suffix, IdFormat = IdFormatType.eMI3_HYPHEN)

        /// <summary>
        /// Parse the given string as an charging e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an charging e-mobility provider identification.</param>
        /// <param name="IdFormat">The format of the charging e-mobility provider identification [old|new].</param>
        public static Provider_Id Parse(Country            CountryCode,
                                        String             Suffix,
                                        ProviderIdFormats  IdFormat = ProviderIdFormats.eMI3_HYPHEN)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The given charging e-mobility provider identification suffix must not be null or empty!");

            #endregion

            return IdFormat switch {
                ProviderIdFormats.eMI3  => Parse(CountryCode.Alpha2Code +       Suffix),
                _                       => Parse(CountryCode.Alpha2Code + "-" + Suffix)
            };

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        public static Provider_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var providerId))
                return providerId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ProviderId)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        /// <param name="ProviderId">The parsed charging e-mobility provider identification.</param>
        public static Boolean TryParse(String           Text,
                                       out Provider_Id  ProviderId)
        {

            #region Initial checks

            Text = Text.Trim().ToUpper();

            if (Text.IsNullOrEmpty())
            {
                ProviderId = default;
                return false;
            }

            #endregion

            try
            {

                var matchCollection = ProviderId_RegEx.Matches(Text);

                if (matchCollection.Count == 1 &&
                    Country.TryParseAlpha2Code(matchCollection[0].Groups[1].Value, out var countryCode))
                {

                    ProviderId = new Provider_Id(
                                     countryCode,
                                     matchCollection[0].Groups[3].Value,
                                     matchCollection[0].Groups[2].Value == "-"
                                         ? ProviderIdFormats.eMI3_HYPHEN
                                         : ProviderIdFormats.eMI3
                                 );

                    return true;

                }

            }
            catch
            { }

            ProviderId = default;
            return false;

        }

        #endregion

        #region TryParse(CountryCode, Suffix, IdFormat = ProviderIdFormats.eMI3_STAR)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility e-mobility provider identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility e-mobility provider identification.</param>
        public static Provider_Id? TryParse(Country            CountryCode,
                                            String             Suffix,
                                            ProviderIdFormats  IdFormat = ProviderIdFormats.eMI3_HYPHEN)
        {

            if (TryParse(CountryCode, Suffix, out var providerId, IdFormat))
                return providerId;

            return null;

        }

        #endregion

        #region TryParse(CountryCode, Suffix, out ProviderId, IdFormat = ProviderIdFormats.eMI3_STAR)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility e-mobility provider identification.</param>
        /// <param name="ProviderId">The parsed e-mobility e-mobility provider identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility e-mobility provider identification.</param>
        public static Boolean TryParse(Country            CountryCode,
                                       String             Suffix,
                                       out Provider_Id    ProviderId,
                                       ProviderIdFormats  IdFormat = ProviderIdFormats.eMI3_HYPHEN)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty() || Suffix.Trim().IsNullOrEmpty())
            {
                ProviderId = default;
                return false;
            }

            #endregion

            return IdFormat switch {
                ProviderIdFormats.eMI3  => TryParse(CountryCode.Alpha2Code +       Suffix, out ProviderId),
                _                       => TryParse(CountryCode.Alpha2Code + "-" + Suffix, out ProviderId)
            };

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging e-mobility provider identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging e-mobility provider identification format.</param>
        public Provider_Id ChangeFormat(ProviderIdFormats NewFormat)

            => new (CountryCode,
                    Suffix,
                    NewFormat);

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this charging e-mobility provider identification.
        /// </summary>
        public Provider_Id Clone()

            => new (
                   CountryCode.Clone(),
                   Suffix.     CloneString(),
                   Format
               );

        #endregion


        #region Operator overloading

        #region Operator == (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An charging e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another charging e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Provider_Id ProviderId1,
                                           Provider_Id ProviderId2)

            => ProviderId1.Equals(ProviderId2);

        #endregion

        #region Operator != (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An charging e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another charging e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Provider_Id ProviderId1,
                                           Provider_Id ProviderId2)

            => !ProviderId1.Equals(ProviderId2);

        #endregion

        #region Operator <  (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An charging e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another charging e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Provider_Id ProviderId1,
                                          Provider_Id ProviderId2)

            => ProviderId1.CompareTo(ProviderId2) < 0;

        #endregion

        #region Operator <= (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An charging e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another charging e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Provider_Id ProviderId1,
                                           Provider_Id ProviderId2)

            => ProviderId1.CompareTo(ProviderId2) <= 0;

        #endregion

        #region Operator >  (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An charging e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another charging e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Provider_Id ProviderId1,
                                          Provider_Id ProviderId2)

            => ProviderId1.CompareTo(ProviderId2) > 0;

        #endregion

        #region Operator >= (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An charging e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another charging e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Provider_Id ProviderId1,
                                           Provider_Id ProviderId2)

            => ProviderId1.CompareTo(ProviderId2) >= 0;

        #endregion

        #endregion

        #region IComparable<Provider_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two provider identifications.
        /// </summary>
        /// <param name="Object">A provider identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Provider_Id providerId
                   ? CompareTo(providerId)
                   : throw new ArgumentException("The given object is not a provider identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ProviderId)

        /// <summary>
        /// Compares two provider identifications.
        /// </summary>
        /// <param name="ProviderId">A provider identification to compare with.</param>
        public Int32 CompareTo(Provider_Id ProviderId)
        {

            var c = CountryCode.CompareTo(ProviderId.CountryCode);

            if (c == 0)
                c = String.Compare(Suffix,
                                   ProviderId.Suffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<Provider_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two provider identifications for equality.
        /// </summary>
        /// <param name="Object">A provider identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Provider_Id providerId &&
                   Equals(providerId);

        #endregion

        #region Equals(ProviderId)

        /// <summary>
        /// Compares two provider identifications for equality.
        /// </summary>
        /// <param name="ProviderId">A provider identification to compare with.</param>
        public Boolean Equals(Provider_Id ProviderId)

            => CountryCode.Equals(ProviderId.CountryCode) &&

               String.Equals(Suffix,
                             ProviderId.Suffix,
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
                   ProviderIdFormats.eMI3_HYPHEN  => CountryCode.Alpha2Code + "-" + Suffix,
                   _                              => CountryCode.Alpha2Code +       Suffix
               };

        #endregion

    }

}
