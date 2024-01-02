/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for session action identifications.
    /// </summary>
    public static class SessionActionIdExtensions
    {

        /// <summary>
        /// Indicates whether this session action identification is null or empty.
        /// </summary>
        /// <param name="SessionActionId">A session action identification.</param>
        public static Boolean IsNullOrEmpty(this SessionAction_Id? SessionActionId)
            => !SessionActionId.HasValue || SessionActionId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this session action identification is NOT null or empty.
        /// </summary>
        /// <param name="SessionActionId">A session action identification.</param>
        public static Boolean IsNotNullOrEmpty(this SessionAction_Id? SessionActionId)
            => SessionActionId.HasValue && SessionActionId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a session action.
    /// </summary>
    public readonly struct SessionAction_Id : IId,
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
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the session action identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

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
            => new ("0");

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a session action identification.
        /// </summary>
        /// <param name="Text">A text representation of a session action identification.</param>
        public static SessionAction_Id Parse(String Text)
        {

            if (TryParse(Text, out var serviceSessionId))
                return serviceSessionId;

            throw new ArgumentException($"Invalid text representation of a session action identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a session action identification.
        /// </summary>
        /// <param name="Text">A text representation of a session action identification.</param>
        public static SessionAction_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var serviceSessionId))
                return serviceSessionId;

            return null;

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
            catch
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

            => new (
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
        public static Boolean operator == (SessionAction_Id SessionActionId1,
                                           SessionAction_Id SessionActionId2)

            => SessionActionId1.Equals(SessionActionId2);

        #endregion

        #region Operator != (SessionActionId1, SessionActionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionId1">A session action identification.</param>
        /// <param name="SessionActionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SessionAction_Id SessionActionId1,
                                           SessionAction_Id SessionActionId2)

            => !SessionActionId1.Equals(SessionActionId2);

        #endregion

        #region Operator <  (SessionActionId1, SessionActionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionId1">A session action identification.</param>
        /// <param name="SessionActionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SessionAction_Id SessionActionId1,
                                          SessionAction_Id SessionActionId2)

            => SessionActionId1.CompareTo(SessionActionId2) < 0;

        #endregion

        #region Operator <= (SessionActionId1, SessionActionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionId1">A session action identification.</param>
        /// <param name="SessionActionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SessionAction_Id SessionActionId1,
                                           SessionAction_Id SessionActionId2)

            => SessionActionId1.CompareTo(SessionActionId2) <= 0;

        #endregion

        #region Operator >  (SessionActionId1, SessionActionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionId1">A session action identification.</param>
        /// <param name="SessionActionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SessionAction_Id SessionActionId1,
                                          SessionAction_Id SessionActionId2)

            => SessionActionId1.CompareTo(SessionActionId2) > 0;

        #endregion

        #region Operator >= (SessionActionId1, SessionActionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionId1">A session action identification.</param>
        /// <param name="SessionActionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SessionAction_Id SessionActionId1,
                                           SessionAction_Id SessionActionId2)

            => SessionActionId1.CompareTo(SessionActionId2) >= 0;

        #endregion

        #endregion

        #region IComparable<SessionAction_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two session action identifications.
        /// </summary>
        /// <param name="Object">A session action identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is SessionAction_Id sessionActionId
                   ? CompareTo(sessionActionId)
                   : throw new ArgumentException("The given object is not a session action identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(SessionActionId)

        /// <summary>
        /// Compares two session action identifications.
        /// </summary>
        /// <param name="SessionActionId">A session action identification to compare with.</param>
        public Int32 CompareTo(SessionAction_Id SessionActionId)

            => String.Compare(InternalId,
                              SessionActionId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<SessionAction_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two session action identifications for equality.
        /// </summary>
        /// <param name="Object">A session action identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SessionAction_Id sessionActionId &&
                   Equals(sessionActionId);

        #endregion

        #region Equals(SessionActionId)

        /// <summary>
        /// Compares two session action identifications for equality.
        /// </summary>
        /// <param name="SessionActionId">A session action identification to compare with.</param>
        public Boolean Equals(SessionAction_Id SessionActionId)

            => String.Equals(InternalId,
                             SessionActionId.InternalId,
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
