/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Diagnostics;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// eMIP data conversion methods.
    /// </summary>
    public static partial class ConversionMethods
    {

        #region AsText        (this UserIdFormat)

        /// <summary>
        /// Return a text representation of the given user identification format.
        /// </summary>
        /// <param name="UserIdFormat">A user identification format.</param>
        public static String AsText(this UserIdFormats UserIdFormat)
        {

            switch (UserIdFormat)
            {

                case UserIdFormats.RFID_UID:
                    return "RFID-UID";

                case UserIdFormats.eMI3:
                    return "eMI3";

                case UserIdFormats.eMA:
                    return "eMA";

                case UserIdFormats.EVCO:
                    return "EVCO";

                default:
                    return "EMP-SPEC";

            }

        }

        #endregion

        #region AsUserIdFormat(this Text)

        /// <summary>
        /// Parse the given text representation of an user identification format.
        /// </summary>
        /// <param name="Text">A text-representation of an user identification format.</param>
        public static UserIdFormats AsUserIdFormat(String Text)
        {

            switch (Text)
            {

                case "RFID-UID":
                    return UserIdFormats.RFID_UID;

                case "eMI3":
                    return UserIdFormats.eMI3;

                case "eMA":
                    return UserIdFormats.eMA;

                case "EVCO":
                    return UserIdFormats.EVCO;

                default:
                    return UserIdFormats.EMP_SPEC;

            }

        }

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
    [DebuggerDisplay("{InternalId} ({Format})")]
    public struct User_Id : IId,
                            IEquatable<User_Id>,
                            IComparable<User_Id>

    {

        #region Data

        /// <summary>
        /// The identificator.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// The format of the user identification.
        /// </summary>
        public UserIdFormats Format   { get; }

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
        {
            get
            {

                switch (Format)
                {

                    default:
                        return (UInt64) InternalId.Length;

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new user identification.
        /// </summary>
        /// <param name="Value">The value of the user identification.</param>
        /// <param name="Format">The format of the user identification.</param>
        private User_Id(String         Value,
                        UserIdFormats  Format = UserIdFormats.RFID_UID)
        {

            this.InternalId  = Value;
            this.Format      = Format;

        }

        #endregion


        #region Parse   (Text, Format = RFID_UID)

        /// <summary>
        /// Parse the given text representation of an user identification.
        /// </summary>
        /// <param name="Text">A text representation of an user identification.</param>
        /// <param name="Format">The format of the user identification.</param>
        public static User_Id Parse(String         Text,
                                    UserIdFormats  Format = UserIdFormats.RFID_UID)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an user identification must not be null or empty!");

            #endregion

            return new User_Id(Text,
                               Format);

        }

        #endregion

        #region TryParse(Text, Format = RFID_UID)

        /// <summary>
        /// Try to parse the given text representation of an user identification.
        /// </summary>
        /// <param name="Text">A text representation of an user identification.</param>
        /// <param name="Format">The format of the user identification.</param>
        public static User_Id? TryParse(String         Text,
                                        UserIdFormats  Format = UserIdFormats.RFID_UID)
        {

            if (TryParse(Text, out User_Id UserId, Format))
                return UserId;

            return new User_Id?();

        }

        #endregion

        #region TryParse(Text, out UserId, Format = RFID_UID)

        /// <summary>
        /// Try to parse the given text representation of an user identification.
        /// </summary>
        /// <param name="Text">A text representation of an user identification.</param>
        /// <param name="UserId">The parsed user identification.</param>
        /// <param name="Format">The format of the user identification.</param>
        public static Boolean TryParse(String         Text,
                                       out User_Id    UserId,
                                       UserIdFormats  Format = UserIdFormats.RFID_UID)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                UserId = default(User_Id);
                return false;
            }

            #endregion

            try
            {

                UserId = new User_Id(Text,
                                     Format);

                return true;

            }
            catch (Exception)
            { }

            UserId = default(User_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this user identification.
        /// </summary>
        public User_Id Clone

            => new User_Id(new String(InternalId.ToCharArray()),
                           Format);

        #endregion


        #region Operator overloading

        #region Operator == (UserId1, UserId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId1">An user identification.</param>
        /// <param name="UserId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (User_Id UserId1, User_Id UserId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(UserId1, UserId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) UserId1 == null) || ((Object) UserId2 == null))
                return false;

            return UserId1.Equals(UserId2);

        }

        #endregion

        #region Operator != (UserId1, UserId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId1">An user identification.</param>
        /// <param name="UserId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (User_Id UserId1, User_Id UserId2)
            => !(UserId1 == UserId2);

        #endregion

        #region Operator <  (UserId1, UserId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId1">An user identification.</param>
        /// <param name="UserId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (User_Id UserId1, User_Id UserId2)
        {

            if ((Object) UserId1 == null)
                throw new ArgumentNullException(nameof(UserId1), "The given UserId1 must not be null!");

            return UserId1.CompareTo(UserId2) < 0;

        }

        #endregion

        #region Operator <= (UserId1, UserId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId1">An user identification.</param>
        /// <param name="UserId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (User_Id UserId1, User_Id UserId2)
            => !(UserId1 > UserId2);

        #endregion

        #region Operator >  (UserId1, UserId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId1">An user identification.</param>
        /// <param name="UserId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (User_Id UserId1, User_Id UserId2)
        {

            if ((Object) UserId1 == null)
                throw new ArgumentNullException(nameof(UserId1), "The given UserId1 must not be null!");

            return UserId1.CompareTo(UserId2) > 0;

        }

        #endregion

        #region Operator >= (UserId1, UserId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId1">An user identification.</param>
        /// <param name="UserId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (User_Id UserId1, User_Id UserId2)
            => !(UserId1 < UserId2);

        #endregion

        #endregion

        #region IComparable<UserId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is User_Id))
                throw new ArgumentException("The given object is not an user identification!", nameof(Object));

            return CompareTo((User_Id) Object);

        }

        #endregion

        #region CompareTo(UserId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UserId">An object to compare with.</param>
        public Int32 CompareTo(User_Id UserId)
        {

            if ((Object) UserId == null)
                throw new ArgumentNullException(nameof(UserId), "The given user identification must not be null!");

            var _Result = String.Compare(InternalId, UserId.InternalId, StringComparison.Ordinal);

            if (_Result == 0)
                return Format.CompareTo(UserId.Format);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<UserId> Members

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

            if (!(Object is User_Id))
                return false;

            return Equals((User_Id) Object);

        }

        #endregion

        #region Equals(UserId)

        /// <summary>
        /// Compares two UserIds for equality.
        /// </summary>
        /// <param name="UserId">A UserId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(User_Id UserId)
        {

            if ((Object) UserId == null)
                return false;

            return InternalId.Equals(UserId.InternalId) &&
                   Format.    Equals(UserId.Format);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId.GetHashCode() ^
               Format.    GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            return InternalId;

            switch (Format)
            {

                default:
                    return String.Concat(InternalId, " (", Format, ")");

            }

        }

        #endregion

    }

}
