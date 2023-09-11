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

using System;
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

            if (Text != null)
                Text = Text.Trim().ToUpper();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an e-mobility provider identification must not be null or empty!");

            #endregion

            var MatchCollection = ProviderId_RegEx.Matches(Text);

            if (MatchCollection.Count == 1 &&
                Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out Country _CountryCode))
            {

                return new Provider_Id(_CountryCode,
                                       MatchCollection[0].Groups[3].Value,
                                       MatchCollection[0].Groups[2].Value == "-" ? ProviderIdFormats.eMI3_HYPHEN : ProviderIdFormats.eMI3);

            }

            throw new ArgumentException("Illegal text representation of an e-mobility provider identification: '{Text}'!", nameof(Text));

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

            if (CountryCode == null)
                throw new ArgumentNullException(nameof(CountryCode),  "The given country must not be null!");

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),       "The given charging e-mobility provider identification suffix must not be null or empty!");

            #endregion

            switch (IdFormat)
            {

                case ProviderIdFormats.eMI3:
                    return Parse(CountryCode.Alpha2Code +       Suffix);

                default:
                    return Parse(CountryCode.Alpha2Code + "-" + Suffix);

            }

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        public static Provider_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Provider_Id _ProviderId))
                return _ProviderId;

            return new Provider_Id?();

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

            if (Text != null)
                Text = Text.Trim().ToUpper();

            if (Text.IsNullOrEmpty())
            {
                ProviderId = default(Provider_Id);
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = ProviderId_RegEx.Matches(Text);

                if (MatchCollection.Count == 1 &&
                    Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out Country _CountryCode))
                {

                    ProviderId = new Provider_Id(_CountryCode,
                                                 MatchCollection[0].Groups[3].Value,
                                                 MatchCollection[0].Groups[2].Value == "-" ? ProviderIdFormats.eMI3_HYPHEN : ProviderIdFormats.eMI3);

                    return true;

                }

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            ProviderId = default(Provider_Id);
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

            if (TryParse(CountryCode, Suffix, out Provider_Id _ProviderId, IdFormat))
                return _ProviderId;

            return new Provider_Id?();

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

            if (CountryCode == null || Suffix.IsNullOrEmpty() || Suffix.Trim().IsNullOrEmpty())
            {
                ProviderId = default(Provider_Id);
                return false;
            }

            #endregion

            switch (IdFormat)
            {

                case ProviderIdFormats.eMI3:
                    return TryParse(CountryCode.Alpha2Code +       Suffix,
                                    out ProviderId);

                default:
                    return TryParse(CountryCode.Alpha2Code + "-" + Suffix,
                                    out ProviderId);

            }

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging e-mobility provider identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging e-mobility provider identification format.</param>
        public Provider_Id ChangeFormat(ProviderIdFormats NewFormat)

            => new Provider_Id(CountryCode,
                               Suffix,
                               NewFormat);

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging e-mobility provider identification.
        /// </summary>
        public Provider_Id Clone

            => new Provider_Id(CountryCode,
                               new String(Suffix.ToCharArray()),
                               Format);

        #endregion


        #region Operator overloading

        #region Operator == (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An charging e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another charging e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Provider_Id ProviderId1, Provider_Id ProviderId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ProviderId1, ProviderId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ProviderId1 == null) || ((Object) ProviderId2 == null))
                return false;

            return ProviderId1.Equals(ProviderId2);

        }

        #endregion

        #region Operator != (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An charging e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another charging e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Provider_Id ProviderId1, Provider_Id ProviderId2)
            => !(ProviderId1 == ProviderId2);

        #endregion

        #region Operator <  (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An charging e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another charging e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Provider_Id ProviderId1, Provider_Id ProviderId2)
        {

            if ((Object) ProviderId1 == null)
                throw new ArgumentNullException(nameof(ProviderId1), "The given ProviderId1 must not be null!");

            return ProviderId1.CompareTo(ProviderId2) < 0;

        }

        #endregion

        #region Operator <= (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An charging e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another charging e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Provider_Id ProviderId1, Provider_Id ProviderId2)
            => !(ProviderId1 > ProviderId2);

        #endregion

        #region Operator >  (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An charging e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another charging e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Provider_Id ProviderId1, Provider_Id ProviderId2)
        {

            if ((Object) ProviderId1 == null)
                throw new ArgumentNullException(nameof(ProviderId1), "The given ProviderId1 must not be null!");

            return ProviderId1.CompareTo(ProviderId2) > 0;

        }

        #endregion

        #region Operator >= (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An charging e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another charging e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Provider_Id ProviderId1, Provider_Id ProviderId2)
            => !(ProviderId1 < ProviderId2);

        #endregion

        #endregion

        #region IComparable<ProviderId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Provider_Id ProviderId))
                throw new ArgumentException("The given object is not an e-mobility provider identification!", nameof(Object));

            return CompareTo(ProviderId);

        }

        #endregion

        #region CompareTo(ProviderId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId">An object to compare with.</param>
        public Int32 CompareTo(Provider_Id ProviderId)
        {

            if ((Object) ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId), "The given charging e-mobility provider identification must not be null!");

            var _Result = CountryCode.CompareTo(ProviderId.CountryCode);

            if (_Result == 0)
                _Result = String.Compare(Suffix, ProviderId.Suffix, StringComparison.OrdinalIgnoreCase);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ProviderId> Members

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

            if (!(Object is Provider_Id ProviderId))
                return false;

            return Equals(ProviderId);

        }

        #endregion

        #region Equals(ProviderId)

        /// <summary>
        /// Compares two ProviderIds for equality.
        /// </summary>
        /// <param name="ProviderId">A ProviderId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Provider_Id ProviderId)
        {

            if ((Object) ProviderId == null)
                return false;

            return CountryCode.     Equals(ProviderId.CountryCode) &&
                   Suffix.ToLower().Equals(ProviderId.Suffix.ToLower());

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

                case ProviderIdFormats.eMI3_HYPHEN:
                    return CountryCode.Alpha2Code + "-" + Suffix;

                default:
                    return CountryCode.Alpha2Code       + Suffix;

            }

        }

        #endregion

    }

}
