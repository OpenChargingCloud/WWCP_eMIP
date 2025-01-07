/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for session event identifications.
    /// </summary>
    public static class SessionEventIdExtensions
    {

        /// <summary>
        /// Indicates whether this session event identification is null or empty.
        /// </summary>
        /// <param name="SessionEventId">A session event identification.</param>
        public static Boolean IsNullOrEmpty(this SessionEvent_Id? SessionEventId)
            => !SessionEventId.HasValue || SessionEventId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this session event identification is NOT null or empty.
        /// </summary>
        /// <param name="SessionEventId">A session event identification.</param>
        public static Boolean IsNotNullOrEmpty(this SessionEvent_Id? SessionEventId)
            => SessionEventId.HasValue && SessionEventId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a session event.
    /// </summary>
    public readonly struct SessionEvent_Id : IId,
                                             IEquatable <SessionEvent_Id>,
                                             IComparable<SessionEvent_Id>
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
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the session event identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

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

            => new (RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Zero

        public static SessionEvent_Id Zero
            => new ("0");

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a session event identification.
        /// </summary>
        /// <param name="Text">A text representation of a session event identification.</param>
        public static SessionEvent_Id Parse(String Text)
        {

            if (TryParse(Text, out var sessionEventId))
                return sessionEventId;

            throw new ArgumentException($"Invalid text representation of a session event identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a session event identification.
        /// </summary>
        /// <param name="Text">A text representation of a session event identification.</param>
        public static SessionEvent_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var serviceEventId))
                return serviceEventId;

            return null;

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
            catch
            { }

            SessionEventId = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this session event identification.
        /// </summary>
        public SessionEvent_Id Clone()

            => new (
                   InternalId.CloneString()
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
        public static Boolean operator == (SessionEvent_Id SessionEventId1,
                                           SessionEvent_Id SessionEventId2)

            => SessionEventId1.Equals(SessionEventId2);

        #endregion

        #region Operator != (SessionEventId1, SessionEventId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventId1">A session event identification.</param>
        /// <param name="SessionEventId2">Another session event identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SessionEvent_Id SessionEventId1,
                                           SessionEvent_Id SessionEventId2)

            => !SessionEventId1.Equals(SessionEventId2);

        #endregion

        #region Operator <  (SessionEventId1, SessionEventId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventId1">A session event identification.</param>
        /// <param name="SessionEventId2">Another session event identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SessionEvent_Id SessionEventId1,
                                          SessionEvent_Id SessionEventId2)

            => SessionEventId1.CompareTo(SessionEventId2) < 0;

        #endregion

        #region Operator <= (SessionEventId1, SessionEventId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventId1">A session event identification.</param>
        /// <param name="SessionEventId2">Another session event identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SessionEvent_Id SessionEventId1,
                                           SessionEvent_Id SessionEventId2)

            => SessionEventId1.CompareTo(SessionEventId2) <= 0;

        #endregion

        #region Operator >  (SessionEventId1, SessionEventId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventId1">A session event identification.</param>
        /// <param name="SessionEventId2">Another session event identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SessionEvent_Id SessionEventId1,
                                          SessionEvent_Id SessionEventId2)

            => SessionEventId1.CompareTo(SessionEventId2) > 0;

        #endregion

        #region Operator >= (SessionEventId1, SessionEventId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventId1">A session event identification.</param>
        /// <param name="SessionEventId2">Another session event identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SessionEvent_Id SessionEventId1,
                                           SessionEvent_Id SessionEventId2)

            => SessionEventId1.CompareTo(SessionEventId2) >= 0;

        #endregion

        #endregion

        #region IComparable<SessionEvent_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two session event identifications.
        /// </summary>
        /// <param name="Object">A session event identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is SessionEvent_Id sessionEventId
                   ? CompareTo(sessionEventId)
                   : throw new ArgumentException("The given object is not a session event identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(SessionEventId)

        /// <summary>
        /// Compares two session event identifications.
        /// </summary>
        /// <param name="SessionEventId">A session event identification to compare with.</param>
        public Int32 CompareTo(SessionEvent_Id SessionEventId)

            => String.Compare(InternalId,
                              SessionEventId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<SessionEvent_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two session event identifications for equality.
        /// </summary>
        /// <param name="Object">A session event identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SessionEvent_Id sessionEventId &&
                   Equals(sessionEventId);

        #endregion

        #region Equals(SessionEventId)

        /// <summary>
        /// Compares two session event identifications for equality.
        /// </summary>
        /// <param name="SessionEventId">A session event identification to compare with.</param>
        public Boolean Equals(SessionEvent_Id SessionEventId)

            => String.Equals(InternalId,
                             SessionEventId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.ToLower().GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "-";

        #endregion

    }

}
