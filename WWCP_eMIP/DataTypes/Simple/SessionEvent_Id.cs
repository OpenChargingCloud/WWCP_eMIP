/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

using System;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// The unique identification of a session event.
    /// </summary>
    public struct SessionEvent_Id : IId,
                                    IEquatable <SessionEvent_Id>,
                                    IComparable<SessionEvent_Id>
    {

        #region Data

        private readonly static Random _Random = new Random(DateTime.Now.Millisecond);

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
        /// The length of the session event identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new session event identification.
        /// based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of a session event identification.</param>
        private SessionEvent_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Random(Length = 20)

        public static SessionEvent_Id Random(Byte Length = 20)
            => new SessionEvent_Id(_Random.RandomString(Length));

        #endregion

        #region (static) Zero

        public static SessionEvent_Id Zero
            => new SessionEvent_Id("0");

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a session event identification.
        /// </summary>
        /// <param name="Text">A text representation of a session event identification.</param>
        public static SessionEvent_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a session event identification must not be null or empty!");

            #endregion

            if (TryParse(Text, out SessionEvent_Id ServiceSessionId))
                return ServiceSessionId;

            throw new ArgumentNullException(nameof(Text), "The given text representation of a session event identification is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a session event identification.
        /// </summary>
        /// <param name="Text">A text representation of a session event identification.</param>
        public static SessionEvent_Id? TryParse(String Text)
        {

            if (TryParse(Text, out SessionEvent_Id ServiceSessionId))
                return ServiceSessionId;

            return new SessionEvent_Id?();

        }

        #endregion

        #region TryParse(Text, out SessionEventId)

        /// <summary>
        /// Try to parse the given string as a session event identification.
        /// </summary>
        /// <param name="Text">A text representation of a session event identification.</param>
        /// <param name="SessionEventId">The parsed session event identification.</param>
        public static Boolean TryParse(String Text, out SessionEvent_Id SessionEventId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                SessionEventId = default;
                return false;
            }

            #endregion

            try
            {
                SessionEventId = new SessionEvent_Id(Text);
                return true;
            }
            catch (Exception)
            { }

            SessionEventId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this session event identification.
        /// </summary>
        public SessionEvent_Id Clone

            => new SessionEvent_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (SessionEventId1, SessionEventId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventId1">A session event identification.</param>
        /// <param name="SessionEventId2">Another session event identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SessionEvent_Id SessionEventId1, SessionEvent_Id SessionEventId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SessionEventId1, SessionEventId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SessionEventId1 == null) || ((Object) SessionEventId2 == null))
                return false;

            return SessionEventId1.Equals(SessionEventId2);

        }

        #endregion

        #region Operator != (SessionEventId1, SessionEventId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventId1">A session event identification.</param>
        /// <param name="SessionEventId2">Another session event identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SessionEvent_Id SessionEventId1, SessionEvent_Id SessionEventId2)
            => !(SessionEventId1 == SessionEventId2);

        #endregion

        #region Operator <  (SessionEventId1, SessionEventId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventId1">A session event identification.</param>
        /// <param name="SessionEventId2">Another session event identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SessionEvent_Id SessionEventId1, SessionEvent_Id SessionEventId2)
        {

            if ((Object) SessionEventId1 == null)
                throw new ArgumentNullException(nameof(SessionEventId1), "The given SessionEventId1 must not be null!");

            return SessionEventId1.CompareTo(SessionEventId2) < 0;

        }

        #endregion

        #region Operator <= (SessionEventId1, SessionEventId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventId1">A session event identification.</param>
        /// <param name="SessionEventId2">Another session event identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SessionEvent_Id SessionEventId1, SessionEvent_Id SessionEventId2)
            => !(SessionEventId1 > SessionEventId2);

        #endregion

        #region Operator >  (SessionEventId1, SessionEventId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventId1">A session event identification.</param>
        /// <param name="SessionEventId2">Another session event identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SessionEvent_Id SessionEventId1, SessionEvent_Id SessionEventId2)
        {

            if ((Object) SessionEventId1 == null)
                throw new ArgumentNullException(nameof(SessionEventId1), "The given SessionEventId1 must not be null!");

            return SessionEventId1.CompareTo(SessionEventId2) > 0;

        }

        #endregion

        #region Operator >= (SessionEventId1, SessionEventId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventId1">A session event identification.</param>
        /// <param name="SessionEventId2">Another session event identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SessionEvent_Id SessionEventId1, SessionEvent_Id SessionEventId2)
            => !(SessionEventId1 < SessionEventId2);

        #endregion

        #endregion

        #region IComparable<SessionEventId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is SessionEvent_Id SessionEventId))
                throw new ArgumentException("The given object is not a session event identification!",
                                            nameof(Object));

            return CompareTo(SessionEventId);

        }

        #endregion

        #region CompareTo(SessionEventId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventId">An object to compare with.</param>
        public Int32 CompareTo(SessionEvent_Id SessionEventId)
        {

            if ((Object) SessionEventId == null)
                throw new ArgumentNullException(nameof(SessionEventId),  "The given session event identification must not be null!");

            return String.Compare(InternalId, SessionEventId.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<SessionEventId> Members

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

            if (!(Object is SessionEvent_Id SessionEventId))
                return false;

            return Equals(SessionEventId);

        }

        #endregion

        #region Equals(SessionEventId)

        /// <summary>
        /// Compares two SessionEventIds for equality.
        /// </summary>
        /// <param name="SessionEventId">A session event identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SessionEvent_Id SessionEventId)
        {

            if ((Object) SessionEventId == null)
                return false;

            return InternalId.ToLower().Equals(SessionEventId.InternalId.ToLower());

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
