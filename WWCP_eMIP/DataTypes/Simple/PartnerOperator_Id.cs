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
    /// The unique identification of a partner operator.
    /// Gireve sometimes uses AA-BBB and sometimes AA*BBB!
    /// </summary>
    public struct PartnerOperator_Id : IId,
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
        public Country            CountryCode   { get; }

        /// <summary>
        /// The identificator suffix.
        /// </summary>
        public String             Suffix        { get; }

        /// <summary>
        /// The format of the charging partner operator identification.
        /// </summary>
        public PartnerOperatorIdFormats  Format        { get; }

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

                    case PartnerOperatorIdFormats.eMI3_STAR:
                        return (UInt64) (CountryCode.Alpha2Code.Length + 1 + Suffix.Length);

                    default:
                        return (UInt64) (CountryCode.Alpha2Code.Length     + Suffix.Length);

                }

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
        private PartnerOperator_Id(Country            CountryCode,
                            String             Suffix,
                            PartnerOperatorIdFormats  Format = PartnerOperatorIdFormats.eMI3_STAR)
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

            if (Text != null)
                Text = Text.Trim().ToUpper();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a partner operator identification must not be null or empty!");

            #endregion

            var MatchCollection = PartnerOperatorId_RegEx.Matches(Text);

            if (MatchCollection.Count == 1 &&
                Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out Country _CountryCode))
            {

                return new PartnerOperator_Id(_CountryCode,
                                       MatchCollection[0].Groups[3].Value,
                                       MatchCollection[0].Groups[2].Value == "*" ? PartnerOperatorIdFormats.eMI3_STAR : PartnerOperatorIdFormats.eMI3);

            }

            throw new ArgumentException("Illegal text representation of a partner operator identification: '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region (static) Parse(CountryCode, Suffix, IdFormat = IdFormatType.eMI3_STAR)

        /// <summary>
        /// Parse the given string as an charging operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an charging operator identification.</param>
        /// <param name="IdFormat">The format of the charging operator identification [old|new].</param>
        public static PartnerOperator_Id Parse(Country            CountryCode,
                                        String             Suffix,
                                        PartnerOperatorIdFormats  IdFormat = PartnerOperatorIdFormats.eMI3_STAR)
        {

            #region Initial checks

            if (CountryCode == null)
                throw new ArgumentNullException(nameof(CountryCode),  "The given country must not be null!");

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),       "The given charging operator identification suffix must not be null or empty!");

            #endregion

            switch (IdFormat)
            {

                case PartnerOperatorIdFormats.eMI3:
                    return Parse(CountryCode.Alpha2Code +       Suffix);

                default:
                    return Parse(CountryCode.Alpha2Code + "*" + Suffix);

            }

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of a partner operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner operator identification.</param>
        public static PartnerOperator_Id? TryParse(String Text)
        {

            if (TryParse(Text, out PartnerOperator_Id _PartnerOperatorId))
                return _PartnerOperatorId;

            return new PartnerOperator_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out PartnerOperatorId)

        /// <summary>
        /// Try to parse the given text representation of a partner operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner operator identification.</param>
        /// <param name="PartnerOperatorId">The parsed charging operator identification.</param>
        public static Boolean TryParse(String           Text,
                                       out PartnerOperator_Id  PartnerOperatorId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim().ToUpper();

            if (Text.IsNullOrEmpty())
            {
                PartnerOperatorId = default;
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = PartnerOperatorId_RegEx.Matches(Text);

                if (MatchCollection.Count == 1 &&
                    Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out Country _CountryCode))
                {

                    PartnerOperatorId = new PartnerOperator_Id(_CountryCode,
                                                 MatchCollection[0].Groups[3].Value,
                                                 MatchCollection[0].Groups[2].Value == "*" ? PartnerOperatorIdFormats.eMI3_STAR : PartnerOperatorIdFormats.eMI3);

                    return true;

                }

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
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
        public static PartnerOperator_Id? TryParse(Country            CountryCode,
                                            String             Suffix,
                                            PartnerOperatorIdFormats  IdFormat = PartnerOperatorIdFormats.eMI3_STAR)
        {

            if (TryParse(CountryCode, Suffix, out PartnerOperator_Id _PartnerOperatorId, IdFormat))
                return _PartnerOperatorId;

            return new PartnerOperator_Id?();

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
        public static Boolean TryParse(Country            CountryCode,
                                       String             Suffix,
                                       out PartnerOperator_Id    PartnerOperatorId,
                                       PartnerOperatorIdFormats  IdFormat = PartnerOperatorIdFormats.eMI3_STAR)
        {

            #region Initial checks

            if (CountryCode == null || Suffix.IsNullOrEmpty() || Suffix.Trim().IsNullOrEmpty())
            {
                PartnerOperatorId = default;
                return false;
            }

            #endregion

            switch (IdFormat)
            {

                case PartnerOperatorIdFormats.eMI3:
                    return TryParse(CountryCode.Alpha2Code +       Suffix,
                                    out PartnerOperatorId);

                default:
                    return TryParse(CountryCode.Alpha2Code + "*" + Suffix,
                                    out PartnerOperatorId);

            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging operator identification.
        /// </summary>
        public PartnerOperator_Id Clone

            => new PartnerOperator_Id(CountryCode,
                               new String(Suffix.ToCharArray()),
                               Format);

        #endregion


        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging operator identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging operator identification format.</param>
        public PartnerOperator_Id ChangeFormat(PartnerOperatorIdFormats NewFormat)

            => new PartnerOperator_Id(CountryCode,
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
        public static Boolean operator == (PartnerOperator_Id PartnerOperatorId1, PartnerOperator_Id PartnerOperatorId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PartnerOperatorId1, PartnerOperatorId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PartnerOperatorId1 == null) || ((Object) PartnerOperatorId2 == null))
                return false;

            return PartnerOperatorId1.Equals(PartnerOperatorId2);

        }

        #endregion

        #region PartnerOperator != (PartnerOperatorId1, PartnerOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerOperatorId1">An charging operator identification.</param>
        /// <param name="PartnerOperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PartnerOperator_Id PartnerOperatorId1, PartnerOperator_Id PartnerOperatorId2)
            => !(PartnerOperatorId1 == PartnerOperatorId2);

        #endregion

        #region PartnerOperator <  (PartnerOperatorId1, PartnerOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerOperatorId1">An charging operator identification.</param>
        /// <param name="PartnerOperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (PartnerOperator_Id PartnerOperatorId1, PartnerOperator_Id PartnerOperatorId2)
        {

            if ((Object) PartnerOperatorId1 == null)
                throw new ArgumentNullException(nameof(PartnerOperatorId1), "The given PartnerOperatorId1 must not be null!");

            return PartnerOperatorId1.CompareTo(PartnerOperatorId2) < 0;

        }

        #endregion

        #region PartnerOperator <= (PartnerOperatorId1, PartnerOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerOperatorId1">An charging operator identification.</param>
        /// <param name="PartnerOperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (PartnerOperator_Id PartnerOperatorId1, PartnerOperator_Id PartnerOperatorId2)
            => !(PartnerOperatorId1 > PartnerOperatorId2);

        #endregion

        #region PartnerOperator >  (PartnerOperatorId1, PartnerOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerOperatorId1">An charging operator identification.</param>
        /// <param name="PartnerOperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (PartnerOperator_Id PartnerOperatorId1, PartnerOperator_Id PartnerOperatorId2)
        {

            if ((Object) PartnerOperatorId1 == null)
                throw new ArgumentNullException(nameof(PartnerOperatorId1), "The given PartnerOperatorId1 must not be null!");

            return PartnerOperatorId1.CompareTo(PartnerOperatorId2) > 0;

        }

        #endregion

        #region PartnerOperator >= (PartnerOperatorId1, PartnerOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerOperatorId1">An charging operator identification.</param>
        /// <param name="PartnerOperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (PartnerOperator_Id PartnerOperatorId1, PartnerOperator_Id PartnerOperatorId2)
            => !(PartnerOperatorId1 < PartnerOperatorId2);

        #endregion

        #endregion

        #region IComparable<PartnerOperatorId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is PartnerOperator_Id PartnerOperatorId))
                throw new ArgumentException("The given object is not a partner operator identification!", nameof(Object));

            return CompareTo(PartnerOperatorId);

        }

        #endregion

        #region CompareTo(PartnerOperatorId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerOperatorId">An object to compare with.</param>
        public Int32 CompareTo(PartnerOperator_Id PartnerOperatorId)
        {

            if ((Object) PartnerOperatorId == null)
                throw new ArgumentNullException(nameof(PartnerOperatorId), "The given charging operator identification must not be null!");

            var _Result = CountryCode.CompareTo(PartnerOperatorId.CountryCode);

            if (_Result == 0)
                _Result = String.Compare(Suffix, PartnerOperatorId.Suffix, StringComparison.OrdinalIgnoreCase);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<PartnerOperatorId> Members

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

            if (!(Object is PartnerOperator_Id PartnerOperatorId))
                return false;

            return Equals(PartnerOperatorId);

        }

        #endregion

        #region Equals(PartnerOperatorId)

        /// <summary>
        /// Compares two PartnerOperatorIds for equality.
        /// </summary>
        /// <param name="PartnerOperatorId">A PartnerOperatorId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(PartnerOperator_Id PartnerOperatorId)
        {

            if ((Object) PartnerOperatorId == null)
                return false;

            return CountryCode.     Equals(PartnerOperatorId.CountryCode) &&
                   Suffix.ToLower().Equals(PartnerOperatorId.Suffix.ToLower());

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

                case PartnerOperatorIdFormats.eMI3_STAR:
                    return CountryCode.Alpha2Code + "*" + Suffix;

                default:
                    return CountryCode.Alpha2Code       + Suffix;

            }

        }

        #endregion

    }

}
