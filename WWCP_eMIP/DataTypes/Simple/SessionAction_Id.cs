/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
 * This file is part of WWCP eMIP <https://github.com/OpenChargingCloud/WWCP_eMIP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
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
    /// The unique identification of a session action.
    /// </summary>
    public struct SessionAction_Id : IId,
                                     IEquatable <SessionAction_Id>,
                                     IComparable<SessionAction_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// The length of the session action identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new session action identification.
        /// based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of a session action identification.</param>
        private SessionAction_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Random(Length = 20)

        public static SessionAction_Id Random(Byte Length = 20)

            => new (RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Zero

        public static SessionAction_Id Zero
            => new SessionAction_Id("0");

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a session action identification.
        /// </summary>
        /// <param name="Text">A text representation of a session action identification.</param>
        public static SessionAction_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a session action identification must not be null or empty!");

            #endregion

            if (TryParse(Text, out SessionAction_Id ServiceSessionId))
                return ServiceSessionId;

            throw new ArgumentNullException(nameof(Text), "The given text representation of a session action identification is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a session action identification.
        /// </summary>
        /// <param name="Text">A text representation of a session action identification.</param>
        public static SessionAction_Id? TryParse(String Text)
        {

            if (TryParse(Text, out SessionAction_Id ServiceSessionId))
                return ServiceSessionId;

            return new SessionAction_Id?();

        }

        #endregion

        #region TryParse(Text, out SessionActionId)

        /// <summary>
        /// Try to parse the given string as a session action identification.
        /// </summary>
        /// <param name="Text">A text representation of a session action identification.</param>
        /// <param name="SessionActionId">The parsed session action identification.</param>
        public static Boolean TryParse(String Text, out SessionAction_Id SessionActionId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                SessionActionId = default;
                return false;
            }

            #endregion

            try
            {
                SessionActionId = new SessionAction_Id(Text);
                return true;
            }
            catch (Exception)
            { }

            SessionActionId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this session action identification.
        /// </summary>
        public SessionAction_Id Clone

            => new SessionAction_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (SessionActionId1, SessionActionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionId1">A session action identification.</param>
        /// <param name="SessionActionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SessionAction_Id SessionActionId1, SessionAction_Id SessionActionId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SessionActionId1, SessionActionId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SessionActionId1 == null) || ((Object) SessionActionId2 == null))
                return false;

            return SessionActionId1.Equals(SessionActionId2);

        }

        #endregion

        #region Operator != (SessionActionId1, SessionActionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionId1">A session action identification.</param>
        /// <param name="SessionActionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SessionAction_Id SessionActionId1, SessionAction_Id SessionActionId2)
            => !(SessionActionId1 == SessionActionId2);

        #endregion

        #region Operator <  (SessionActionId1, SessionActionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionId1">A session action identification.</param>
        /// <param name="SessionActionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SessionAction_Id SessionActionId1, SessionAction_Id SessionActionId2)
        {

            if ((Object) SessionActionId1 == null)
                throw new ArgumentNullException(nameof(SessionActionId1), "The given SessionActionId1 must not be null!");

            return SessionActionId1.CompareTo(SessionActionId2) < 0;

        }

        #endregion

        #region Operator <= (SessionActionId1, SessionActionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionId1">A session action identification.</param>
        /// <param name="SessionActionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SessionAction_Id SessionActionId1, SessionAction_Id SessionActionId2)
            => !(SessionActionId1 > SessionActionId2);

        #endregion

        #region Operator >  (SessionActionId1, SessionActionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionId1">A session action identification.</param>
        /// <param name="SessionActionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SessionAction_Id SessionActionId1, SessionAction_Id SessionActionId2)
        {

            if ((Object) SessionActionId1 == null)
                throw new ArgumentNullException(nameof(SessionActionId1), "The given SessionActionId1 must not be null!");

            return SessionActionId1.CompareTo(SessionActionId2) > 0;

        }

        #endregion

        #region Operator >= (SessionActionId1, SessionActionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionId1">A session action identification.</param>
        /// <param name="SessionActionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SessionAction_Id SessionActionId1, SessionAction_Id SessionActionId2)
            => !(SessionActionId1 < SessionActionId2);

        #endregion

        #endregion

        #region IComparable<SessionActionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is SessionAction_Id SessionActionId))
                throw new ArgumentException("The given object is not a session action identification!",
                                            nameof(Object));

            return CompareTo(SessionActionId);

        }

        #endregion

        #region CompareTo(SessionActionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionId">An object to compare with.</param>
        public Int32 CompareTo(SessionAction_Id SessionActionId)
        {

            if ((Object) SessionActionId == null)
                throw new ArgumentNullException(nameof(SessionActionId),  "The given session action identification must not be null!");

            return String.Compare(InternalId, SessionActionId.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<SessionActionId> Members

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

            if (!(Object is SessionAction_Id SessionActionId))
                return false;

            return Equals(SessionActionId);

        }

        #endregion

        #region Equals(SessionActionId)

        /// <summary>
        /// Compares two SessionActionIds for equality.
        /// </summary>
        /// <param name="SessionActionId">A session action identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SessionAction_Id SessionActionId)
        {

            if ((Object) SessionActionId == null)
                return false;

            return InternalId.ToLower().Equals(SessionActionId.InternalId.ToLower());

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => InternalId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
