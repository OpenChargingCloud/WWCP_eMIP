﻿/*
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// Extension methods for charging station identifications.
    /// </summary>
    public static class ChargingStationIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging station identification is null or empty.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingStation_Id? ChargingStationId)
            => !ChargingStationId.HasValue || ChargingStationId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station identification is NOT null or empty.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStation_Id? ChargingStationId)
            => ChargingStationId.HasValue && ChargingStationId.Value.IsNotNullOrEmpty;


        /// <summary>
        /// Create a new EVSE identification
        /// based on the given charging station identification.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification.</param>
        /// <param name="AdditionalSuffix">An additional EVSE suffix.</param>
        public static EVSE_Id CreateEVSEId(this ChargingStation_Id  ChargingStationId,
                                           String                   AdditionalSuffix)
        {

            var Suffix = ChargingStationId.Suffix;

            if (Suffix.StartsWith("TATION", StringComparison.Ordinal))
                Suffix = "VSE" + Suffix.Substring(6);

            return EVSE_Id.Parse(ChargingStationId.OperatorId, Suffix + AdditionalSuffix ?? "");

        }

    }


    /// <summary>
    /// The unique identification of a charging station.
    /// </summary>
    public readonly struct ChargingStation_Id : IId,
                                                IEquatable<ChargingStation_Id>,
                                                IComparable<ChargingStation_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging station identification.
        /// All '*' are optional!
        /// </summary>
        public  static readonly Regex  ChargingStationId_RegEx  = new (@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?S([A-Z0-9][A-Z0-9\*]{0,50})$",
                                                                       RegexOptions.IgnorePatternWhitespace);

        private static readonly Char[] StarSplitter             = new Char[] { '*' };

        private        readonly String MinSuffix;

        #endregion

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id  OperatorId   { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String       Suffix       { get; }

        /// <summary>
        /// The format of the charging station identification.
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
        /// Create a new charging station identification based on the given
        /// operator identification and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Suffix">The suffix of the charging station identification.</param>
        private ChargingStation_Id(Operator_Id  OperatorId,
                                   String       Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The charging station identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;
            this.MinSuffix   = Suffix.Replace("*", "");

        }

        #endregion


        #region Create(EVSEId,  RemoveLastStar = true)

        /// <summary>
        /// Create a ChargingStationId based on the given EVSE identification.
        /// </summary>
        /// <param name="EVSEId">An EVSEId.</param>
        /// <param name="RemoveLastStar">Generate a charging station identification by removing the last star part.</param>
        public static ChargingStation_Id Create(EVSE_Id  EVSEId,
                                                Boolean  RemoveLastStar = true)
        {

            var _Suffix = EVSEId.Suffix;

            if (RemoveLastStar)
            {

                var _HasAStar = EVSEId.Suffix.LastIndexOf("*");

                if (_HasAStar > 0)
                    _Suffix = _Suffix.Substring(0, _HasAStar);

            }

            return Parse(EVSEId.OperatorId, _Suffix.ToUpper());

            //var _Array = new String[] {
            //                 EVSEId.OperatorId.CountryCode.Alpha2Code,
            //                 EVSEId.OperatorId.Suffix
            //             }.Concat(EVSEId.ToString().Substring(2 + EVSEId.OperatorId.Suffix.Length).ToUpper().Split('*', '-')).ToArray();

            //if (EVSEId.Format == OperatorIdFormats.eMI3 || EVSEId.Format == OperatorIdFormats.eMI3_STAR)
            //{
            //    if (_Array[2].StartsWith("E", StringComparison.Ordinal))
            //        _Array[2] = "S" + _Array[2].Substring(1);
            //}
            //else
            //{
            //    if (!_Array[2].StartsWith("S", StringComparison.Ordinal))
            //         _Array[2] = "S" + _Array[2];
            //}


            //// e.g. "DE*822*E123456"
            //if (_Array.Length == 3)
            //{

            //    if (EVSEId.ToString().Contains('-'))
            //        return Parse(_Array.AggregateWith("-"));

            //    return Parse(_Array.AggregateWith("*"));

            //}

            //// e.g. "DE*822*E123456*1" => "DE*822*S123456"
            //if (EVSEId.ToString().Contains('-'))
            //    return Parse(_Array.Take(_Array.Length - 1).AggregateWith("-"));

            //if (_Array[0].StartsWith("+"))
            //    return Parse(_Array.Take(1).Select(item => Country.ParseTelefonCode(item.Substring(1)).Alpha2Code).Concat(_Array.Skip(1).Take(_Array.Length - 1)).AggregateWith("*"));

            //else
            //    return Parse(_Array.Take(_Array.Length - 1).AggregateWith("*"));

        }

        #endregion

        #region Create(EVSEIds, HelperId = "", Length = 15, Mapper = null)

        /// <summary>
        /// Create a ChargingStationId based on the given EVSEIds.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSEIds.</param>
        public static ChargingStation_Id? Create(IEnumerable<EVSE_Id>  EVSEIds,
                                                 String                HelperId  = "",
                                                 Byte                  Length    = 15,
                                                 Func<String, String>  Mapper    = null)
        {

            #region Initial checks

            if (EVSEIds == null)
                return null;

            var _EVSEIds = EVSEIds.ToArray();

            if (_EVSEIds.Length == 0)
                return null;

            #endregion

            #region A single EVSE Id...

            // It is just a single EVSE Id...
            if (_EVSEIds.Length == 1)
                return Create(_EVSEIds[0]);

            #endregion


            // Multiple OperatorIds which do not match!
            if (_EVSEIds.Select(evse => evse.OperatorId.ToString()).Distinct().Skip(1).Any())
                return null;

            #region EVSEIdSuffix contains '*'...

            if (_EVSEIds.Any(EVSEId => EVSEId.Suffix.Contains('*') ||
                                       EVSEId.Suffix.Contains('-')))
            {

                var EVSEIdPrefixStrings = _EVSEIds.
                                              Select(EVSEId         => EVSEId.Suffix.Split(StarSplitter, StringSplitOptions.RemoveEmptyEntries)).
                                              Select(SuffixElements => SuffixElements.Take(SuffixElements.Length - 1).AggregateWith("*", "")).
                                              Where (NewSuffix      => NewSuffix != "").
                                              Distinct().
                                              ToArray();

                if (EVSEIdPrefixStrings.Length != 1)
                    return null;

                return Parse(_EVSEIds[0].OperatorId, EVSEIdPrefixStrings[0]);

            }

            #endregion

            #region ...or use longest prefix...

            else
            {

                var _Array      = _EVSEIds.Select(evse => evse.ToString()).ToArray();
                var _MinLength  = (Int32) _Array.Select(v => v.Length).Min();

                var _Prefix     = "";

                for (var i = 0; i < _MinLength; i++)
                {
                    if (_Array.All(v => v[i] == _Array[0][i]))
                        _Prefix += _Array[0][i];
                }

                if (((UInt64) _Prefix.Length) > _EVSEIds[0].OperatorId.Length + 1)
                {

                    var TmpEVSEId = EVSE_Id.Parse(_Prefix);

                    if (TmpEVSEId.Format == OperatorIdFormats.eMI3)
                    {
                        if (((UInt64) _Prefix.Length) > _EVSEIds[0].OperatorId.Length + 2)
                            _Prefix = TmpEVSEId.Suffix; //TmpEVSEId.OperatorId + "*S" + TmpEVSEId.Suffix;
                        else
                            return null;
                    }

                    return Parse(_EVSEIds[0].OperatorId, _Prefix);

                }

            }

            #endregion

            #region ...or generate a hash of the EVSEIds!

            if (Length < 12)
                Length = 12;

            if (Length > 50)
                Length = 50;

            var Suffíx = new SHA1CryptoServiceProvider().
                             ComputeHash(Encoding.UTF8.GetBytes(EVSEIds.Select(evseid => evseid.ToString()).
                                                                        AggregateWith(HelperId ?? ","))).
                             ToHexString().
                             SubstringMax(Length).
                             ToUpper();

            return Parse(_EVSEIds[0].OperatorId,
                         Mapper != null
                            ? Mapper(Suffíx)
                            : Suffíx);

            #endregion

        }

        #endregion

        #region Random(OperatorId, Length = 8, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging station identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Length">The desired length of the identification suffix.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        public static ChargingStation_Id Random(Operator_Id            OperatorId,
                                                Byte                   Length  = 8,
                                                Func<String, String>?  Mapper  = null)

            => new (OperatorId,
                    (Mapper ?? (_ => _)) (RandomExtensions.RandomString((UInt16)(Length < 8 ? 8 : Length > 50 ? 50 : Length))));

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        public static ChargingStation_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging station identification must not be null or empty!");

            #endregion

            var matchCollection = ChargingStationId_RegEx.Matches(Text);

            if (matchCollection.Count != 1)
                throw new ArgumentException($"Illegal text representation of a charging station identification: '{Text}'!",
                                            nameof(Text));


            if (Operator_Id.TryParse(matchCollection[0].Groups[1].Value, out var operatorId))
                return new ChargingStation_Id(
                           operatorId,
                           matchCollection[0].Groups[2].Value
                       );

            throw new ArgumentException($"Invalid text representation of a charging station identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an operator.</param>
        /// <param name="Suffix">The suffix of the charging station identification.</param>
        public static ChargingStation_Id Parse(Operator_Id  OperatorId,
                                               String       Suffix)

            => OperatorId.Format switch {
                   OperatorIdFormats.eMI3  => Parse(OperatorId.ToString() +  "S" + Suffix),
                   _                       => Parse(OperatorId.ToString() + "*S" + Suffix)
               };

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        public static ChargingStation_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingStationId))
                return chargingStationId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ChargingStationId)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        /// <param name="ChargingStationId">The parsed charging station identification.</param>
        public static Boolean TryParse(String Text, out ChargingStation_Id ChargingStationId)
        {

            #region Initial checks

            Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                ChargingStationId = default;
                return false;
            }

            #endregion

            try
            {

                var matchCollection = ChargingStationId_RegEx.Matches(Text);

                if (matchCollection.Count != 1)
                {
                    ChargingStationId = default;
                    return false;
                }

                if (Operator_Id.TryParse(matchCollection[0].Groups[1].Value, out var operatorId))
                {

                    ChargingStationId = new ChargingStation_Id(
                                            operatorId,
                                            matchCollection[0].Groups[2].Value
                                        );

                    return true;

                }

            }
            catch
            { }

            ChargingStationId = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Return a clone of this charging station identification.
        /// </summary>
        public ChargingStation_Id Clone()

            => new (
                   OperatorId.Clone(),
                   Suffix.    CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStation_Id ChargingStationId1,
                                           ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.Equals(ChargingStationId2);

        #endregion

        #region Operator != (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStation_Id ChargingStationId1,
                                           ChargingStation_Id ChargingStationId2)

            => !ChargingStationId1.Equals(ChargingStationId2);

        #endregion

        #region Operator <  (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStation_Id ChargingStationId1,
                                          ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.CompareTo(ChargingStationId2) < 0;

        #endregion

        #region Operator <= (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStation_Id ChargingStationId1,
                                           ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.CompareTo(ChargingStationId2) <= 0;

        #endregion

        #region Operator >  (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStation_Id ChargingStationId1,
                                          ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.CompareTo(ChargingStationId2) > 0;

        #endregion

        #region Operator >= (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStation_Id ChargingStationId1,
                                           ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.CompareTo(ChargingStationId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStation_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station identifications.
        /// </summary>
        /// <param name="Object">A charging station identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStation_Id chargingStationId
                   ? CompareTo(chargingStationId)
                   : throw new ArgumentException("The given object is not a charging station identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationId)

        /// <summary>
        /// Compares two charging station identifications.
        /// </summary>
        /// <param name="Object">A charging station identification to compare with.</param>
        public Int32 CompareTo(ChargingStation_Id ChargingStationId)
        {

            var c = OperatorId.CompareTo(ChargingStationId.OperatorId);

            if (c == 0)
                c = String.Compare(MinSuffix,
                                   ChargingStationId.MinSuffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStation_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station identifications for equality.
        /// </summary>
        /// <param name="Object">A charging station identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStation_Id chargingStationId &&
                   Equals(chargingStationId);

        #endregion

        #region Equals(ChargingStationId)

        /// <summary>
        /// Compares two charging station identifications for equality.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification to compare with.</param>
        public Boolean Equals(ChargingStation_Id ChargingStationId)

            => OperatorId.Equals(ChargingStationId.OperatorId) &&

               String.Equals(MinSuffix,
                             ChargingStationId.MinSuffix,
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
                   OperatorIdFormats.eMI3  => String.Concat(OperatorId,  "S", Suffix),
                   _                       => String.Concat(OperatorId, "*S", Suffix)
               };

        #endregion

    }

}
