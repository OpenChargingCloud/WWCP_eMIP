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
    /// Extension methods for charging connector identifications.
    /// </summary>
    public static class ChargingConnectorIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging connector identification is null or empty.
        /// </summary>
        /// <param name="ChargingConnectorId">A charging connector identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingConnector_Id? ChargingConnectorId)
            => !ChargingConnectorId.HasValue || ChargingConnectorId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging connector identification is NOT null or empty.
        /// </summary>
        /// <param name="ChargingConnectorId">A charging connector identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingConnector_Id? ChargingConnectorId)
            => ChargingConnectorId.HasValue && ChargingConnectorId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging connector.
    /// </summary>
    public readonly struct ChargingConnector_Id : IId,
                                                  IEquatable<ChargingConnector_Id>,
                                                  IComparable<ChargingConnector_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging connector identification.
        /// </summary>
        public  static readonly Regex  ChargingConnectorId_RegEx  = new (@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?X([A-Z0-9][A-Z0-9\*]{0,50})$",
                                                                         RegexOptions.IgnorePatternWhitespace);

        private        readonly String MinSuffix;

        #endregion

        #region Properties

        /// <summary>
        /// The charging connector operator identification.
        /// </summary>
        public Operator_Id  OperatorId   { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String       Suffix       { get; }

        /// <summary>
        /// The format of the charging connector identification.
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
        /// Create a new charging connector identification based on the given
        /// operator identification and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Suffix">The suffix of the charging station identification.</param>
        private ChargingConnector_Id(Operator_Id  OperatorId,
                                     String       Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The charging connector identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;
            this.MinSuffix   = Suffix.Replace("*", "");

        }

        #endregion


        #region (static) Random(OperatorId, Length = 12, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging connector identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Length">The desired length of the identification suffix.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging connector identification.</param>
        public static ChargingConnector_Id Random(Operator_Id            OperatorId,
                                                  Byte                   Length  = 12,
                                                  Func<String, String>?  Mapper  = null)

            => new (OperatorId,
                    (Mapper ?? (_ => _)) (RandomExtensions.RandomString((UInt16) (Length < 12 ? 12 : Length > 50 ? 50 : Length))));

        #endregion

        #region(static) Parse(Text)

        /// <summary>
        /// Parse the given string as a charging connector identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging connector identification.</param>
        public static ChargingConnector_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging connector identification must not be null or empty!");

            #endregion

            var matchCollection = ChargingConnectorId_RegEx.Matches(Text);

            if (matchCollection.Count != 1)
                throw new ArgumentException($"Illegal text representation of a charging connector identification: '{Text}'!",
                                            nameof(Text));


            if (Operator_Id.TryParse(matchCollection[0].Groups[1].Value, out var operatorId))
                return new ChargingConnector_Id(
                           operatorId,
                           matchCollection[0].Groups[2].Value
                       );

            throw new ArgumentException($"Invalid text representation of a charging connector identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging connector identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Suffix">The suffix of the charging connector identification.</param>
        public static ChargingConnector_Id Parse(Operator_Id  OperatorId,
                                                 String       Suffix)

            => OperatorId.Format switch {
                OperatorIdFormats.eMI3  => Parse(OperatorId.ToString() +  "X" + Suffix),
                _                       => Parse(OperatorId.ToString() + "*X" + Suffix),
            };

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Parse the given string as a charging connector identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging connector identification.</param>
        public static ChargingConnector_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingConnectorId))
                return chargingConnectorId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingConnectorId)

        /// <summary>
        /// Parse the given string as a charging connector identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging connector identification.</param>
        /// <param name="ChargingConnectorId">The parsed charging connector identification.</param>
        public static Boolean TryParse(String Text, out ChargingConnector_Id ChargingConnectorId)
        {

            #region Initial checks

            Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                ChargingConnectorId = default;
                return false;
            }

            #endregion

            try
            {

                ChargingConnectorId = default;

                var matchCollection = ChargingConnectorId_RegEx.Matches(Text);

                if (matchCollection.Count != 1)
                    return false;

                if (Operator_Id.TryParse(matchCollection[0].Groups[1].Value, out var operatorId))
                {

                    ChargingConnectorId = new ChargingConnector_Id(
                                              operatorId,
                                              matchCollection[0].Groups[2].Value
                                          );

                    return true;

                }

            }
            catch
            { }

            ChargingConnectorId = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Return a clone of this charging connector identification.
        /// </summary>
        public ChargingConnector_Id Clone()

            => new (
                   OperatorId.Clone(),
                   Suffix.    CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingConnectorId1, ChargingConnectorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnectorId1">A charging connector identification.</param>
        /// <param name="ChargingConnectorId2">Another charging connector identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingConnector_Id ChargingConnectorId1,
                                           ChargingConnector_Id ChargingConnectorId2)

            => ChargingConnectorId1.Equals(ChargingConnectorId2);

        #endregion

        #region Operator != (ChargingConnectorId1, ChargingConnectorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnectorId1">A charging connector identification.</param>
        /// <param name="ChargingConnectorId2">Another charging connector identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingConnector_Id ChargingConnectorId1,
                                           ChargingConnector_Id ChargingConnectorId2)

            => !ChargingConnectorId1.Equals(ChargingConnectorId2);

        #endregion

        #region Operator <  (ChargingConnectorId1, ChargingConnectorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnectorId1">A charging connector identification.</param>
        /// <param name="ChargingConnectorId2">Another charging connector identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingConnector_Id ChargingConnectorId1,
                                          ChargingConnector_Id ChargingConnectorId2)

            => ChargingConnectorId1.CompareTo(ChargingConnectorId2) < 0;

        #endregion

        #region Operator <= (ChargingConnectorId1, ChargingConnectorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnectorId1">A charging connector identification.</param>
        /// <param name="ChargingConnectorId2">Another charging connector identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingConnector_Id ChargingConnectorId1,
                                           ChargingConnector_Id ChargingConnectorId2)

            => ChargingConnectorId1.CompareTo(ChargingConnectorId2) <= 0;

        #endregion

        #region Operator >  (ChargingConnectorId1, ChargingConnectorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnectorId1">A charging connector identification.</param>
        /// <param name="ChargingConnectorId2">Another charging connector identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingConnector_Id ChargingConnectorId1,
                                          ChargingConnector_Id ChargingConnectorId2)

            => ChargingConnectorId1.CompareTo(ChargingConnectorId2) > 0;

        #endregion

        #region Operator >= (ChargingConnectorId1, ChargingConnectorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnectorId1">A charging connector identification.</param>
        /// <param name="ChargingConnectorId2">Another charging connector identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingConnector_Id ChargingConnectorId1,
                                           ChargingConnector_Id ChargingConnectorId2)

            => ChargingConnectorId1.CompareTo(ChargingConnectorId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingConnector_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging connector identifications.
        /// </summary>
        /// <param name="Object">A charging connector identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingConnector_Id chargingConnectorId
                   ? CompareTo(chargingConnectorId)
                   : throw new ArgumentException("The given object is not a charging connector identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingConnector_Id)

        /// <summary>
        /// Compares two charging connector identifications.
        /// </summary>
        /// <param name="ChargingConnectorId">A charging connector identification to compare with.</param>
        public Int32 CompareTo(ChargingConnector_Id ChargingConnectorId)
        {

            var c = OperatorId.CompareTo(ChargingConnectorId.OperatorId);

            if (c == 0)
                c = String.Compare(MinSuffix,
                                   ChargingConnectorId.MinSuffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingConnector_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging connector identifications for equality.
        /// </summary>
        /// <param name="Object">A charging connector identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingConnector_Id chargingConnectorId &&
                   Equals(chargingConnectorId);

        #endregion

        #region Equals(ChargingConnectorId)

        /// <summary>
        /// Compares two charging connector identifications for equality.
        /// </summary>
        /// <param name="ChargingConnectorId">A charging connector identification to compare with.</param>
        public Boolean Equals(ChargingConnector_Id ChargingConnectorId)

            => OperatorId.Equals(ChargingConnectorId.OperatorId) &&

               String.Equals(MinSuffix,
                             ChargingConnectorId.MinSuffix,
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
                   OperatorIdFormats.eMI3  => String.Concat(OperatorId,  "X", Suffix),
                   _                       => String.Concat(OperatorId, "*X", Suffix)
               };

        #endregion

    }

}
