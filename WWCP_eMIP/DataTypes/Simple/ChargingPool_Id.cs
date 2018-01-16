/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// Extention methods for charging pool identifications.
    /// </summary>
    public static class ChargingPoolIdExtentions
    {

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
    public struct ChargingPool_Id : IId,
                                    IEquatable<ChargingPool_Id>,
                                    IComparable<ChargingPool_Id>

    {

        #region Data

        private static readonly Random _Random               = new Random(DateTime.UtcNow.Millisecond);

        /// <summary>
        /// The regular expression for parsing a charging pool identification.
        /// All '*' are optional!
        /// </summary>
        public  static readonly Regex  ChargingPoolId_RegEx  = new Regex(@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?P([A-Z0-9][A-Z0-9\*]{0,50})$",
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


        #region Generate(EVSEOperatorId, Address, GeoLocation, Lenght = 50, Mapper = null)

        /// <summary>
        /// Create a valid charging pool identification based on the given parameters.
        /// </summary>
        /// <param name="OperatorId">The identification of an Charging Station Operator.</param>
        /// <param name="Address">The address of the charging pool.</param>
        /// <param name="GeoLocation">The geo location of the charging pool.</param>
        /// <param name="Length">The maximum size of the generated charging pool identification suffix [12 &lt; n &lt; 50].</param>
        /// <param name="Mapper">A delegate to modify a generated charging pool identification suffix.</param>
        public static ChargingPool_Id Generate(Operator_Id   OperatorId,
                                               Address               Address,
                                               GeoCoordinate?        GeoLocation  = null,
                                               String                HelperId     = "",
                                               Byte                  Length       = 15,
                                               Func<String, String>  Mapper       = null)
        {

            if (Length < 12)
                Length = 12;

            if (Length > 50)
                Length = 50;

            var Suffíx = new SHA1CryptoServiceProvider().
                             ComputeHash(Encoding.UTF8.GetBytes(
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
                         Mapper != null
                            ? Mapper(Suffíx)
                            : Suffíx);

        }

        #endregion

        #region Random(OperatorId, Length = 6, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging pool identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Length">The desired length of the identification suffix.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging pool identification.</param>
        public static ChargingPool_Id Random(Operator_Id           OperatorId,
                                             Byte                  Length  = 6,
                                             Func<String, String>  Mapper  = null)

            => new ChargingPool_Id(OperatorId,
                                   (Mapper ?? (_ => _)) (_Random.RandomString((UInt16)(Length < 6 ? 6 : Length > 50 ? 50 : Length))));

        #endregion

        #region Parse(Text)

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

            var MatchCollection = ChargingPoolId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging pool identification: '" + Text + "'!",
                                            nameof(Text));


            if (Operator_Id.TryParse(MatchCollection[0].Groups[1].Value, out Operator_Id OperatorId))
                return new ChargingPool_Id(OperatorId,
                                           MatchCollection[0].Groups[2].Value);

            throw new ArgumentException("Illegal charging pool identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Suffix">The suffix of the charging pool identification.</param>
        public static ChargingPool_Id Parse(Operator_Id  OperatorId,
                                            String       Suffix)
        {

            switch (OperatorId.Format)
            {

                case OperatorIdFormats.eMI3:
                    return Parse(OperatorId.ToString() +  "P" + Suffix);

                default:
                    return Parse(OperatorId.ToString() + "*P" + Suffix);

            }

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        public static ChargingPool_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingPool_Id ChargingPoolId))
                return ChargingPoolId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ChargingPoolId)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        /// <param name="ChargingPoolId">The parsed charging pool identification.</param>
        public static Boolean TryParse(String Text, out ChargingPool_Id ChargingPoolId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                ChargingPoolId = default(ChargingPool_Id);
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = ChargingPoolId_RegEx.Matches(Text);

                if (MatchCollection.Count != 1)
                {
                    ChargingPoolId = default(ChargingPool_Id);
                    return false;
                }

                if (Operator_Id.TryParse(MatchCollection[0].Groups[1].Value, out Operator_Id OperatorId))
                {

                    ChargingPoolId = new ChargingPool_Id(OperatorId,
                                                         MatchCollection[0].Groups[2].Value);

                    return true;

                }

            }
            catch (Exception)
            { }

            ChargingPoolId = default(ChargingPool_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Return a clone of this charging pool identification.
        /// </summary>
        public ChargingPool_Id Clone

            => new ChargingPool_Id(OperatorId.Clone,
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
        public static Boolean operator == (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingPoolId1, ChargingPoolId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingPoolId1 == null) || ((Object) ChargingPoolId2 == null))
                return false;

            return ChargingPoolId1.Equals(ChargingPoolId2);

        }

        #endregion

        #region Operator != (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
            => !(ChargingPoolId1 == ChargingPoolId2);

        #endregion

        #region Operator <  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {

            if ((Object) ChargingPoolId1 == null)
                throw new ArgumentNullException(nameof(ChargingPoolId1), "The given ChargingPoolId1 must not be null!");

            return ChargingPoolId1.CompareTo(ChargingPoolId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
            => !(ChargingPoolId1 > ChargingPoolId2);

        #endregion

        #region Operator >  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {

            if ((Object) ChargingPoolId1 == null)
                throw new ArgumentNullException(nameof(ChargingPoolId1), "The given ChargingPoolId1 must not be null!");

            return ChargingPoolId1.CompareTo(ChargingPoolId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
            => !(ChargingPoolId1 < ChargingPoolId2);

        #endregion

        #endregion

        #region IComparable<ChargingPoolId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingPool_Id))
                throw new ArgumentException("The given object is not a ChargingPoolId!", nameof(Object));

            return CompareTo((ChargingPool_Id) Object);

        }

        #endregion

        #region CompareTo(ChargingPoolId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId">An object to compare with.</param>
        public Int32 CompareTo(ChargingPool_Id ChargingPoolId)
        {

            if ((Object) ChargingPoolId == null)
                throw new ArgumentNullException(nameof(ChargingPoolId), "The given charging pool identification must not be null!");

            var _Result = OperatorId.CompareTo(ChargingPoolId.OperatorId);

            if (_Result == 0)
                _Result = String.Compare(MinSuffix, ChargingPoolId.MinSuffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is ChargingPool_Id))
                return false;

            return Equals((ChargingPool_Id) Object);

        }

        #endregion

        #region Equals(ChargingPoolId)

        /// <summary>
        /// Compares two charging pool identifications for equality.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPool_Id ChargingPoolId)
        {

            if ((Object) ChargingPoolId == null)
                return false;

            return OperatorId.Equals(ChargingPoolId.OperatorId) &&
                   MinSuffix. Equals(ChargingPoolId.MinSuffix);

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
                    return String.Concat(OperatorId,  "P", Suffix);

                default:
                    return String.Concat(OperatorId, "*P", Suffix);

            }

        }

        #endregion

    }

}
