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

        #region AsText  (this OperatorIdFormat)

        /// <summary>
        /// Return a text representation of the given operator identification format.
        /// </summary>
        /// <param name="OperatorIdFormat">A operator identification format.</param>
        public static String AsText(this OperatorIdFormats OperatorIdFormat)

            => OperatorIdFormat switch {
                   OperatorIdFormats.eMI3 or OperatorIdFormats.eMI3_STAR  => "eMI3",
                   _                                                      => "Gireve"
               };

        #endregion

    }

    /// <summary>
    /// The different formats of operator identifications.
    /// </summary>
    public enum OperatorIdFormats
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
    /// Extension methods for operator identifications.
    /// </summary>
    public static class OperatorIdExtensions
    {

        /// <summary>
        /// Indicates whether this operator identification is null or empty.
        /// </summary>
        /// <param name="OperatorId">An operator identification.</param>
        public static Boolean IsNullOrEmpty(this Operator_Id? OperatorId)
            => !OperatorId.HasValue || OperatorId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this operator identification is NOT null or empty.
        /// </summary>
        /// <param name="OperatorId">An operator identification.</param>
        public static Boolean IsNotNullOrEmpty(this Operator_Id? OperatorId)
            => OperatorId.HasValue && OperatorId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of an operator.
    /// </summary>
    public readonly struct Operator_Id : IId,
                                         IEquatable<Operator_Id>,
                                         IComparable<Operator_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an eMIP charging operator identification.
        /// </summary>
        public static readonly Regex  OperatorId_RegEx  = new Regex(@"^([A-Z]{2})(\*?)([A-Z0-9]{3})$",
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
        /// The format of the charging operator identification.
        /// </summary>
        public OperatorIdFormats  Format        { get; }

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
                        return (UInt64) (CountryCode.Alpha2Code.Length + 1 + Suffix.Length);

                    default:
                        return (UInt64) (CountryCode.Alpha2Code.Length     + Suffix.Length);

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
        private Operator_Id(Country            CountryCode,
                            String             Suffix,
                            OperatorIdFormats  Format = OperatorIdFormats.eMI3_STAR)
        {

            this.CountryCode  = CountryCode;
            this.Suffix       = Suffix;
            this.Format       = Format;

        }

        #endregion


        #region (static) Parse(Text)

        /// <summary>
        /// Parse the given text representation of an operator identification.
        /// </summary>
        /// <param name="Text">A text representation of an operator identification.</param>
        public static Operator_Id Parse(String Text)
        {

            #region Initial checks

            Text = Text.Trim().ToUpper();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an operator identification must not be null or empty!");

            #endregion

            var matchCollection = OperatorId_RegEx.Matches(Text);

            if (matchCollection.Count == 1 &&
                Country.TryParseAlpha2Code(matchCollection[0].Groups[1].Value, out var countryCode))
            {

                return new Operator_Id(countryCode,
                                       matchCollection[0].Groups[3].Value,
                                       matchCollection[0].Groups[2].Value == "*" ? OperatorIdFormats.eMI3_STAR : OperatorIdFormats.eMI3);

            }

            throw new ArgumentException($"Invalid text representation of an operator identification: '{Text}'!",
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
        public static Operator_Id Parse(Country            CountryCode,
                                        String             Suffix,
                                        OperatorIdFormats  IdFormat = OperatorIdFormats.eMI3_STAR)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),  "The given charging operator identification suffix must not be null or empty!");

            #endregion

            return IdFormat switch {
                OperatorIdFormats.eMI3  => Parse(CountryCode.Alpha2Code +       Suffix),
                _                       => Parse(CountryCode.Alpha2Code + "*" + Suffix)
            };

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of an operator identification.
        /// </summary>
        /// <param name="Text">A text representation of an operator identification.</param>
        public static Operator_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var operatorId))
                return operatorId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out OperatorId)

        /// <summary>
        /// Try to parse the given text representation of an operator identification.
        /// </summary>
        /// <param name="Text">A text representation of an operator identification.</param>
        /// <param name="OperatorId">The parsed charging operator identification.</param>
        public static Boolean TryParse(String           Text,
                                       out Operator_Id  OperatorId)
        {

            #region Initial checks

            Text = Text.Trim().ToUpper();

            if (Text.IsNullOrEmpty())
            {
                OperatorId = default;
                return false;
            }

            #endregion

            try
            {

                var matchCollection = OperatorId_RegEx.Matches(Text);

                if (matchCollection.Count == 1 &&
                    Country.TryParseAlpha2Code(matchCollection[0].Groups[1].Value, out var countryCode))
                {

                    OperatorId = new Operator_Id(countryCode,
                                                 matchCollection[0].Groups[3].Value,
                                                 matchCollection[0].Groups[2].Value == "*" ? OperatorIdFormats.eMI3_STAR : OperatorIdFormats.eMI3);

                    return true;

                }

            }
            catch
            { }

            OperatorId = default;
            return false;

        }

        #endregion

        #region (static) TryParse(CountryCode, Suffix, IdFormat = OperatorIdFormats.eMI3_STAR)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility operator identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility operator identification.</param>
        public static Operator_Id? TryParse(Country            CountryCode,
                                            String             Suffix,
                                            OperatorIdFormats  IdFormat = OperatorIdFormats.eMI3_STAR)
        {

            if (TryParse(CountryCode, Suffix, out var operatorId, IdFormat))
                return operatorId;

            return null;

        }

        #endregion

        #region (static) TryParse(CountryCode, Suffix, out OperatorId, IdFormat = OperatorIdFormats.eMI3_STAR)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility operator identification.</param>
        /// <param name="OperatorId">The parsed e-mobility operator identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility operator identification.</param>
        public static Boolean TryParse(Country            CountryCode,
                                       String             Suffix,
                                       out Operator_Id    OperatorId,
                                       OperatorIdFormats  IdFormat = OperatorIdFormats.eMI3_STAR)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty() || Suffix.Trim().IsNullOrEmpty())
            {
                OperatorId = default;
                return false;
            }

            #endregion

            return IdFormat switch {
                OperatorIdFormats.eMI3  => TryParse(CountryCode.Alpha2Code +       Suffix, out OperatorId),
                _                       => TryParse(CountryCode.Alpha2Code + "*" + Suffix, out OperatorId)
            };

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this charging operator identification.
        /// </summary>
        public Operator_Id Clone()

            => new (
                   CountryCode.Clone(),
                   Suffix.     CloneString(),
                   Format
               );

        #endregion


        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging operator identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging operator identification format.</param>
        public Operator_Id ChangeFormat(OperatorIdFormats NewFormat)

            => new (CountryCode,
                    Suffix,
                    NewFormat);

        #endregion


        #region Operator overloading

        #region Operator == (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging operator identification.</param>
        /// <param name="OperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Operator_Id OperatorId1,
                                           Operator_Id OperatorId2)

            => OperatorId1.Equals(OperatorId2);

        #endregion

        #region Operator != (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging operator identification.</param>
        /// <param name="OperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Operator_Id OperatorId1,
                                           Operator_Id OperatorId2)

            => !OperatorId1.Equals(OperatorId2);

        #endregion

        #region Operator <  (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging operator identification.</param>
        /// <param name="OperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Operator_Id OperatorId1,
                                          Operator_Id OperatorId2)

            => OperatorId1.CompareTo(OperatorId2) < 0;

        #endregion

        #region Operator <= (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging operator identification.</param>
        /// <param name="OperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Operator_Id OperatorId1,
                                           Operator_Id OperatorId2)

            => OperatorId1.CompareTo(OperatorId2) <= 0;

        #endregion

        #region Operator >  (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging operator identification.</param>
        /// <param name="OperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Operator_Id OperatorId1,
                                          Operator_Id OperatorId2)

            => OperatorId1.CompareTo(OperatorId2) > 0;

        #endregion

        #region Operator >= (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging operator identification.</param>
        /// <param name="OperatorId2">Another charging operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Operator_Id OperatorId1,
                                           Operator_Id OperatorId2)

            => OperatorId1.CompareTo(OperatorId2) >= 0;

        #endregion

        #endregion

        #region IComparable<Operator_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two operator identifications.
        /// </summary>
        /// <param name="Object">An operator identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Operator_Id operatorId
                   ? CompareTo(operatorId)
                   : throw new ArgumentException("The given object is not an operator identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(OperatorId)

        /// <summary>
        /// Compares two operator identifications.
        /// </summary>
        /// <param name="OperatorId">An operator identification to compare with.</param>
        public Int32 CompareTo(Operator_Id OperatorId)
        {

            var c = CountryCode.CompareTo(OperatorId.CountryCode);

            if (c == 0)
                c = String.Compare(Suffix,
                                   OperatorId.Suffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<Operator_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two operator identifications for equality.
        /// </summary>
        /// <param name="Object">An operator identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Operator_Id operatorId &&
                   Equals(operatorId);

        #endregion

        #region Equals(OperatorId)

        /// <summary>
        /// Compares two operator identifications for equality.
        /// </summary>
        /// <param name="OperatorId">An operator identification to compare with.</param>
        public Boolean Equals(Operator_Id OperatorId)

            => CountryCode.Equals(OperatorId.CountryCode) &&

               String.Equals(Suffix,
                             OperatorId.Suffix,
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
                   OperatorIdFormats.eMI3_STAR  => CountryCode.Alpha2Code + "*" + Suffix,
                   _                            => CountryCode.Alpha2Code +       Suffix,
               };

        #endregion

    }

}
