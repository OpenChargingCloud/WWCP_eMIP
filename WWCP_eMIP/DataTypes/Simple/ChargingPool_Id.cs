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

using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// Extension methods for charging pool identifications.
    /// </summary>
    public static class ChargingPoolIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging pool identification is null or empty.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingPool_Id? ChargingPoolId)
            => !ChargingPoolId.HasValue || ChargingPoolId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging pool identification is NOT null or empty.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingPool_Id? ChargingPoolId)
            => ChargingPoolId.HasValue && ChargingPoolId.Value.IsNotNullOrEmpty;


        /// <summary>
        /// Create a new charging station identification based
        /// on the given charging pool identification.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification.</param>
        /// <param name="AdditionalSuffix">An additional charging station suffix.</param>
        public static ChargingStation_Id CreateStationId(this ChargingPool_Id  ChargingPoolId,
                                                         String                AdditionalSuffix)
        {

            var Suffix = ChargingPoolId.Suffix;

            if (Suffix.StartsWith("OOL", StringComparison.Ordinal))
                Suffix = "TATION" + Suffix.Substring(3);

            return ChargingStation_Id.Parse(ChargingPoolId.OperatorId, Suffix + AdditionalSuffix ?? "");

        }

    }

    /// <summary>
    /// The unique identification of a charging pool.
    /// </summary>
    public readonly struct ChargingPool_Id : IId,
                                             IEquatable<ChargingPool_Id>,
                                             IComparable<ChargingPool_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging pool identification.
        /// All '*' are optional!
        /// </summary>
        public  static readonly Regex  ChargingPoolId_RegEx  = new (@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?P([A-Z0-9][A-Z0-9\*]{0,50})$",
                                                                    RegexOptions.IgnorePatternWhitespace);

        private        readonly String MinSuffix;

        #endregion

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id       OperatorId   { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String            Suffix       { get; }

        /// <summary>
        /// The format of the charging pool identification.
        /// </summary>
        public OperatorIdFormats Format
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

                switch (OperatorId.Format)
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
        /// Create a new charging pool identification based on the given
        /// operator identification and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Suffix">The suffix of the charging pool identification.</param>
        private ChargingPool_Id(Operator_Id  OperatorId,
                                String       Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The charging pool identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;
            this.MinSuffix   = Suffix.Replace("*", "");

        }

        #endregion


        #region (static) Generate(EVSEOperatorId, Address, GeoLocation, Lenght = 50, Mapper = null)

        /// <summary>
        /// Create a valid charging pool identification based on the given parameters.
        /// </summary>
        /// <param name="OperatorId">The identification of an Charging Station Operator.</param>
        /// <param name="Address">The address of the charging pool.</param>
        /// <param name="GeoLocation">The geo location of the charging pool.</param>
        /// <param name="Length">The maximum size of the generated charging pool identification suffix [12 &lt; n &lt; 50].</param>
        /// <param name="Mapper">A delegate to modify a generated charging pool identification suffix.</param>
        public static ChargingPool_Id Generate(Operator_Id            OperatorId,
                                               Address                Address,
                                               GeoCoordinate?         GeoLocation   = null,
                                               String                 HelperId      = "",
                                               Byte                   Length        = 15,
                                               Func<String, String>?  Mapper        = null)
        {

            if (Length < 12)
                Length = 12;

            if (Length > 50)
                Length = 50;

            var Suffíx = SHA1.HashData(Encoding.UTF8.GetBytes(
                                           String.Concat(
                                               OperatorId.  ToString(),
                                               Address.     ToString(),
                                               GeoLocation?.ToString() ?? "",
                                               HelperId                ?? ""
                                           )
                                       )).
                                       ToHexString().
                                       SubstringMax(Length).
                                       ToUpper();

            return Parse(OperatorId,
                         Mapper is not null
                            ? Mapper(Suffíx)
                            : Suffíx);

        }

        #endregion

        #region (static) Random(OperatorId, Length = 6, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging pool identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Length">The desired length of the identification suffix.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging pool identification.</param>
        public static ChargingPool_Id Random(Operator_Id            OperatorId,
                                             Byte                   Length   = 6,
                                             Func<String, String>?  Mapper   = null)

            => new (OperatorId,
                    (Mapper ?? (_ => _)) (RandomExtensions.RandomString((UInt16)(Length < 6 ? 6 : Length > 50 ? 50 : Length))));

        #endregion


        #region (static) Parse(Text)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        public static ChargingPool_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging pool identification must not be null or empty!");

            #endregion

            var matchCollection = ChargingPoolId_RegEx.Matches(Text);

            if (matchCollection.Count != 1)
                throw new ArgumentException($"Illegal text representation of a charging pool identification: '{Text}'!",
                                            nameof(Text));


            if (Operator_Id.TryParse(matchCollection[0].Groups[1].Value, out var operatorId))
                return new ChargingPool_Id(
                           operatorId,
                           matchCollection[0].Groups[2].Value
                       );

            throw new ArgumentException($"Invalid text representation of a charging pool identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Suffix">The suffix of the charging pool identification.</param>
        public static ChargingPool_Id Parse(Operator_Id  OperatorId,
                                            String       Suffix)

            => OperatorId.Format switch {
                   OperatorIdFormats.eMI3  => Parse(OperatorId.ToString() +  "P" + Suffix),
                   _                       => Parse(OperatorId.ToString() + "*P" + Suffix),
               };

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        public static ChargingPool_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingPoolId))
                return chargingPoolId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingPoolId)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        /// <param name="ChargingPoolId">The parsed charging pool identification.</param>
        public static Boolean TryParse(String Text, out ChargingPool_Id ChargingPoolId)
        {

            #region Initial checks

            Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                ChargingPoolId = default;
                return false;
            }

            #endregion

            try
            {

                var matchCollection = ChargingPoolId_RegEx.Matches(Text);

                if (matchCollection.Count != 1)
                {
                    ChargingPoolId = default;
                    return false;
                }

                if (Operator_Id.TryParse(matchCollection[0].Groups[1].Value, out var operatorId))
                {

                    ChargingPoolId = new ChargingPool_Id(
                                         operatorId,
                                         matchCollection[0].Groups[2].Value
                                     );

                    return true;

                }

            }
            catch
            { }

            ChargingPoolId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Return a clone of this charging pool identification.
        /// </summary>
        public ChargingPool_Id Clone

            => new (OperatorId.Clone,
                    new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPool_Id ChargingPoolId1,
                                           ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.Equals(ChargingPoolId2);

        #endregion

        #region Operator != (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPool_Id ChargingPoolId1,
                                           ChargingPool_Id ChargingPoolId2)

            => !ChargingPoolId1.Equals(ChargingPoolId2);

        #endregion

        #region Operator <  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPool_Id ChargingPoolId1,
                                          ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.CompareTo(ChargingPoolId2) < 0;

        #endregion

        #region Operator <= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPool_Id ChargingPoolId1,
                                           ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.CompareTo(ChargingPoolId2) <= 0;

        #endregion

        #region Operator >  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPool_Id ChargingPoolId1,
                                          ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.CompareTo(ChargingPoolId2) > 0;

        #endregion

        #region Operator >= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPool_Id ChargingPoolId1,
                                           ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.CompareTo(ChargingPoolId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingPool_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging pool identifications.
        /// </summary>
        /// <param name="Object">A charging pool identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPool_Id chargingPoolId
                   ? CompareTo(chargingPoolId)
                   : throw new ArgumentException("The given object is not a charging pool identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolId)

        /// <summary>
        /// Compares two charging pool identifications.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification to compare with.</param>
        public Int32 CompareTo(ChargingPool_Id ChargingPoolId)
        {

            var c = OperatorId.CompareTo(ChargingPoolId.OperatorId);

            if (c == 0)
                c = String.Compare(MinSuffix,
                                   ChargingPoolId.MinSuffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPool_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging pool identifications for equality.
        /// </summary>
        /// <param name="Object">A charging pool identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPool_Id chargingPoolId &&
                   Equals(chargingPoolId);

        #endregion

        #region Equals(ChargingPoolId)

        /// <summary>
        /// Compares two charging pool identifications for equality.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification to compare with.</param>
        public Boolean Equals(ChargingPool_Id ChargingPoolId)

            => OperatorId.Equals(ChargingPoolId.OperatorId) &&

               String.Equals(MinSuffix,
                             ChargingPoolId.MinSuffix,
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
                   OperatorIdFormats.eMI3  => String.Concat(OperatorId,  "P", Suffix),
                   _                       => String.Concat(OperatorId, "*P", Suffix)
               };

        #endregion

    }

}
