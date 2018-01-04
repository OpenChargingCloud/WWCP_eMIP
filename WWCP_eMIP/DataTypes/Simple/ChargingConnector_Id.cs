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
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// Extention methods for charging connector identifications.
    /// </summary>
    public static class ChargingConnectorIdExtentions
    {

        ///// <summary>
        ///// Create a new EVSE identification
        ///// based on the given charging connector identification.
        ///// </summary>
        ///// <param name="ChargingStationId">A charging connector identification.</param>
        ///// <param name="AdditionalSuffix">An additional EVSE suffix.</param>
        //public static EVSE_Id CreateEVSEId(this ChargingConnector_Id  ChargingStationId,
        //                                   String                   AdditionalSuffix)
        //{

        //    var Suffix = ChargingStationId.Suffix;

        //    if (Suffix.StartsWith("TATION", StringComparison.Ordinal))
        //        Suffix = "VSE" + Suffix.Substring(6);

        //    return EVSE_Id.Parse(ChargingStationId.OperatorId, Suffix + AdditionalSuffix ?? "");

        //}

    }


    /// <summary>
    /// The unique identification of an electric vehicle charging connector.
    /// </summary>
    public struct ChargingConnector_Id : IId,
                                         IEquatable<ChargingConnector_Id>,
                                         IComparable<ChargingConnector_Id>

    {

        #region Data

        //ToDo: Replace with better randomness!
        private static readonly Random _Random                  = new Random(DateTime.UtcNow.Millisecond);

        /// <summary>
        /// The regular expression for parsing a charging connector identification.
        /// </summary>
        public  static readonly Regex  ChargingStationId_RegEx  = new Regex(@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?X([A-Z0-9][A-Z0-9\*]{0,50})$",
                                                                            RegexOptions.IgnorePatternWhitespace);

        //private static readonly Char[] StarSplitter             = new Char[] { '*' };

        #endregion

        #region Properties

        /// <summary>
        /// The charging connector operator identification.
        /// </summary>
        public Operator_Id  OperatorId   { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String               Suffix       { get; }

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
                        return (UInt64) (OperatorId.CountryCode.Alpha2Code.Length             + 1 + OperatorId.Suffix.Length + 2 + Suffix.Length);

                    default:  // eMI3
                        return (UInt64) (OperatorId.CountryCode.Alpha2Code.Length                 + OperatorId.Suffix.Length + 1 + Suffix.Length);

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new charging connector identification
        /// based on the given charging connector operator and identification suffix.
        /// </summary>
        private ChargingConnector_Id(Operator_Id  OperatorId,
                                     String               Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The charging connector identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region Random(OperatorId, Length = 50, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging connector identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging connector operator.</param>
        /// <param name="Length">The desired length of the identification suffix.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging connector identification.</param>
        public static ChargingConnector_Id Random(Operator_Id   OperatorId,
                                                Byte                  Length  = 50,
                                                Func<String, String>  Mapper  = null)

        {

            if (Length < 12 || Length > 50)
                Length = 50;

            return new ChargingConnector_Id(OperatorId,
                                          Mapper != null
                                              ? Mapper(_Random.RandomString(Length))
                                              : _Random.RandomString(Length));

        }

        #endregion

        #region Parse(Text)

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

            var MatchCollection = ChargingStationId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging connector identification: '" + Text + "'!",
                                            nameof(Text));


            if (Operator_Id.TryParse(MatchCollection[0].Groups[1].Value, out Operator_Id OperatorId))
                return new ChargingConnector_Id(OperatorId,
                                                MatchCollection[0].Groups[2].Value);

            throw new ArgumentException("Illegal charging connector identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging connector identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging connector operator.</param>
        /// <param name="Suffix">The suffix of the charging connector identification.</param>
        public static ChargingConnector_Id Parse(Operator_Id  OperatorId,
                                               String                      Suffix)
        {

            switch (OperatorId.Format)
            {

                case OperatorIdFormats.eMI3_STAR:
                    return Parse(OperatorId.ToString() + "*S" + Suffix);

                case OperatorIdFormats.eMI3:
                    return Parse(OperatorId.ToString() + "S" + Suffix);

                default:
                    return Parse(OperatorId.ToString(OperatorIdFormats.eMI3_STAR) + "*S" + Suffix);

            }

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parse the given string as a charging connector identification.
        /// </summary>
        public static ChargingConnector_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingConnector_Id ChargingStationId))
                return ChargingStationId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ChargingStationId)

        /// <summary>
        /// Parse the given string as a charging connector identification.
        /// </summary>
        public static Boolean TryParse(String Text, out ChargingConnector_Id ChargingStationId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ChargingStationId = default(ChargingConnector_Id);
                return false;
            }

            #endregion

            try
            {

                ChargingStationId = default(ChargingConnector_Id);

                var _MatchCollection = ChargingStationId_RegEx.Matches(Text);

                if (_MatchCollection.Count != 1)
                    return false;


                if (Operator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out Operator_Id _OperatorId))
                {

                    ChargingStationId = new ChargingConnector_Id(_OperatorId,
                                                               _MatchCollection[0].Groups[2].Value);

                    return true;

                }

            }
#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            ChargingStationId = default(ChargingConnector_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging connector identification.
        /// </summary>
        public ChargingConnector_Id Clone

            => new ChargingConnector_Id(OperatorId.Clone,
                                      new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging connector identification.</param>
        /// <param name="ChargingStationId2">Another charging connector identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingConnector_Id ChargingStationId1, ChargingConnector_Id ChargingStationId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingStationId1, ChargingStationId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationId1 == null) || ((Object) ChargingStationId2 == null))
                return false;

            return ChargingStationId1.Equals(ChargingStationId2);

        }

        #endregion

        #region Operator != (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging connector identification.</param>
        /// <param name="ChargingStationId2">Another charging connector identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingConnector_Id ChargingStationId1, ChargingConnector_Id ChargingStationId2)
            => !(ChargingStationId1 == ChargingStationId2);

        #endregion

        #region Operator <  (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging connector identification.</param>
        /// <param name="ChargingStationId2">Another charging connector identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingConnector_Id ChargingStationId1, ChargingConnector_Id ChargingStationId2)
        {

            if ((Object) ChargingStationId1 == null)
                throw new ArgumentNullException(nameof(ChargingStationId1), "The given ChargingStationId1 must not be null!");

            return ChargingStationId1.CompareTo(ChargingStationId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging connector identification.</param>
        /// <param name="ChargingStationId2">Another charging connector identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingConnector_Id ChargingStationId1, ChargingConnector_Id ChargingStationId2)
            => !(ChargingStationId1 > ChargingStationId2);

        #endregion

        #region Operator >  (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging connector identification.</param>
        /// <param name="ChargingStationId2">Another charging connector identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingConnector_Id ChargingStationId1, ChargingConnector_Id ChargingStationId2)
        {

            if ((Object) ChargingStationId1 == null)
                throw new ArgumentNullException(nameof(ChargingStationId1), "The given ChargingStationId1 must not be null!");

            return ChargingStationId1.CompareTo(ChargingStationId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging connector identification.</param>
        /// <param name="ChargingStationId2">Another charging connector identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingConnector_Id ChargingStationId1, ChargingConnector_Id ChargingStationId2)
            => !(ChargingStationId1 < ChargingStationId2);

        #endregion

        #endregion

        #region IComparable<ChargingStationId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingConnector_Id))
                throw new ArgumentException("The given object is not a charging connector identification!", nameof(Object));

            return CompareTo((ChargingConnector_Id) Object);

        }

        #endregion

        #region CompareTo(ChargingStationId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId">An object to compare with.</param>
        public Int32 CompareTo(ChargingConnector_Id ChargingStationId)
        {

            if ((Object) ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId), "The given charging connector identification must not be null!");

            // Compare the length of the ChargingStationIds
            var _Result = Length.CompareTo(ChargingStationId.Length);

            // If equal: Compare charging operator identifications
            if (_Result == 0)
                _Result = OperatorId.CompareTo(ChargingStationId.OperatorId);

            // If equal: Compare ChargingStationId suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, ChargingStationId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingConnector_Id> Members

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

            if (!(Object is ChargingConnector_Id))
                return false;

            return Equals((ChargingConnector_Id) Object);

        }

        #endregion

        #region Equals(ChargingStationId)

        /// <summary>
        /// Compares two charging connector identifications for equality.
        /// </summary>
        /// <param name="ChargingStationId">A charging connector identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingConnector_Id ChargingStationId)
        {

            if ((Object) ChargingStationId == null)
                return false;

            return OperatorId.Equals(ChargingStationId.OperatorId) &&
                   Suffix.    Equals(ChargingStationId.Suffix);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => OperatorId.GetHashCode() ^
               Suffix.    GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            switch (OperatorId.Format)
            {

                case OperatorIdFormats.eMI3_STAR:
                    return OperatorId.CountryCode.Alpha2Code + "*" + OperatorId.Suffix + "*S" + Suffix;

                default: // eMI3
                    return OperatorId.CountryCode.Alpha2Code       + OperatorId.Suffix + "S" + Suffix;

            }

        }

        #endregion


    }

}
