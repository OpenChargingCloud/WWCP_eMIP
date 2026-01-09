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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// Extensions methods for user identification formats.
    /// </summary>
    public static class UserIdFormatsExtensions
    {

        #region Parse (Text)

        /// <summary>
        /// Parse the given text representation of an user identification format.
        /// </summary>
        /// <param name="Text">A text representation of an user identification format.</param>
        public static UserIdFormats Parse(String Text)

            => Text switch {
                   "RFID-UID"  => UserIdFormats.RFID_UID,
                   "eMI3"      => UserIdFormats.eMI3,
                   "eMA"       => UserIdFormats.eMA,
                   "EVCO"      => UserIdFormats.EVCO,
                   _           => UserIdFormats.EMP_SPEC
               };

        #endregion

        #region AsText(this UserIdFormat)

        /// <summary>
        /// Return a text representation of the given user identification format.
        /// </summary>
        /// <param name="UserIdFormat">A user identification format.</param>
        public static String AsText(this UserIdFormats UserIdFormat)

            => UserIdFormat switch {
                   UserIdFormats.RFID_UID  => "RFID-UID",
                   UserIdFormats.eMI3      => "eMI3",
                   UserIdFormats.eMA       => "eMA",
                   UserIdFormats.EVCO      => "EVCO",
                   _                       => "EMP-SPEC"
               };

        #endregion

    }

    /// <summary>
    /// The different formats of user identifications.
    /// </summary>
    public enum UserIdFormats
    {

        /// <summary>
        /// The unique identification of a RFID card/tag.
        /// </summary>
        RFID_UID,

        /// <summary>
        /// An eMI3 token.
        /// </summary>
        eMI3,

        /// <summary>
        /// An eMI3 e-mobility account identification.
        /// </summary>
        eMA,

        /// <summary>
        /// A electric vehicle contract identification (see also ISO/IEC 15118).
        /// </summary>
        EVCO,

        /// <summary>
        /// An EMP-specific identification.
        /// </summary>
        EMP_SPEC

    }


    /// <summary>
    /// The unique identification of an user.
    /// </summary>
    public readonly struct User_Id : IId<User_Id>

    {

        #region Properties

        /// <summary>
        /// The user identification.
        /// </summary>
        public String         Value     { get; }

        /// <summary>
        /// The format of the user identification.
        /// </summary>
        public UserIdFormats  Format    { get; }

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean        IsNullOrEmpty
            => Value.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean        IsNotNullOrEmpty
            => Value.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64         Length
            => (UInt64) (Value?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new user identification.
        /// </summary>
        /// <param name="Value">The value of the user identification.</param>
        /// <param name="Format">The format of the user identification.</param>
        public User_Id(String         Value,
                       UserIdFormats  Format = UserIdFormats.RFID_UID)
        {

            this.Value   = Value;
            this.Format  = Format;

        }

        #endregion


        #region (static) Parse   (Text, Format = RFID_UID)

        /// <summary>
        /// Parse the given text representation of an user identification.
        /// </summary>
        /// <param name="Text">A text representation of an user identification.</param>
        /// <param name="Format">The format of the user identification.</param>
        public static User_Id Parse(String         Text,
                                    UserIdFormats  Format = UserIdFormats.RFID_UID)
        {

            if (TryParse(Text, out var userId, Format))
                return userId;

            throw new ArgumentException($"Invalid text representation of an user identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text, Format = RFID_UID)

        /// <summary>
        /// Try to parse the given text representation of an user identification.
        /// </summary>
        /// <param name="Text">A text representation of an user identification.</param>
        /// <param name="Format">The format of the user identification.</param>
        public static User_Id? TryParse(String         Text,
                                        UserIdFormats  Format = UserIdFormats.RFID_UID)
        {

            if (TryParse(Text, out var userId, Format))
                return userId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out UserId, Format = RFID_UID)

        /// <summary>
        /// Try to parse the given text representation of an user identification.
        /// </summary>
        /// <param name="Text">A text representation of an user identification.</param>
        /// <param name="UserId">The parsed user identification.</param>
        /// <param name="Format">The format of the user identification.</param>
        public static Boolean TryParse(String         Text,
                                       out User_Id    UserId,
                                       UserIdFormats  Format  = UserIdFormats.RFID_UID)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    UserId = new User_Id(Text, Format);
                    return true;
                }
                catch
                { }
            }

            UserId = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this user identification.
        /// </summary>
        public User_Id Clone()

            => new (
                   Value.CloneString(),
                   Format
               );

        #endregion


        #region Operator overloading

        #region Operator == (UserId1, UserId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId1">An user identification.</param>
        /// <param name="UserId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (User_Id UserId1,
                                           User_Id UserId2)

            => UserId1.Equals(UserId2);

        #endregion

        #region Operator != (UserId1, UserId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId1">An user identification.</param>
        /// <param name="UserId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (User_Id UserId1,
                                           User_Id UserId2)

            => !UserId1.Equals(UserId2);

        #endregion

        #region Operator <  (UserId1, UserId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId1">An user identification.</param>
        /// <param name="UserId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (User_Id UserId1,
                                          User_Id UserId2)

            => UserId1.CompareTo(UserId2) < 0;

        #endregion

        #region Operator <= (UserId1, UserId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId1">An user identification.</param>
        /// <param name="UserId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (User_Id UserId1,
                                           User_Id UserId2)

            => UserId1.CompareTo(UserId2) <= 0;

        #endregion

        #region Operator >  (UserId1, UserId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId1">An user identification.</param>
        /// <param name="UserId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (User_Id UserId1,
                                          User_Id UserId2)

            => UserId1.CompareTo(UserId2) > 0;

        #endregion

        #region Operator >= (UserId1, UserId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId1">An user identification.</param>
        /// <param name="UserId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (User_Id UserId1,
                                           User_Id UserId2)

            => UserId1.CompareTo(UserId2) >= 0;

        #endregion

        #endregion

        #region IComparable<UserId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two user identifications.
        /// </summary>
        /// <param name="Object">An user identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is User_Id userId
                   ? CompareTo(userId)
                   : throw new ArgumentException("The given object is not an user identification!", nameof(Object));

        #endregion

        #region CompareTo(UserId)

        /// <summary>
        /// Compares two user identifications.
        /// </summary>
        /// <param name="UserId">An user identification to compare with.</param>
        public Int32 CompareTo(User_Id UserId)
        {

            var c = String.Compare(Value,
                                   UserId.Value,
                                   StringComparison.OrdinalIgnoreCase);

            if (c == 0)
                return Format.CompareTo(UserId.Format);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<UserId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two user identifications for equality.
        /// </summary>
        /// <param name="Object">An user identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is User_Id userId &&
               Equals(userId);

        #endregion

        #region Equals(UserId)

        /// <summary>
        /// Compares two user identifications for equality.
        /// </summary>
        /// <param name="UserId">An user identification to compare with.</param>
        public Boolean Equals(User_Id UserId)

            => String.Equals(Value,
                             UserId.Value,
                             StringComparison.OrdinalIgnoreCase) &&

               Format.Equals(UserId.Format);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => Value.GetHashCode() ^
               Format.    GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Value;
            //=> $"{Value} ({Format})";

        #endregion

    }

}
