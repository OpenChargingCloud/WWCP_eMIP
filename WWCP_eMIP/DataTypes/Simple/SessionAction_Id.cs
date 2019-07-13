/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
    /// The unique identification of a session action.
    /// </summary>
    public struct SessionAction_Id : IId,
                                     IEquatable <SessionAction_Id>,
                                     IComparable<SessionAction_Id>
    {

        #region Data

        private readonly static Random _Random = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

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


        #region Parse   (Text)

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

            return new SessionAction_Id(Text);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a session action identification.
        /// </summary>
        /// <param name="Text">A text representation of a session action identification.</param>
        public static SessionAction_Id? TryParse(String Text)
        {

            if (Text != null)
                Text = Text.Trim();

            return Text.IsNullOrEmpty()
                       ? new SessionAction_Id?()
                       : new SessionAction_Id(Text);

        }

        #endregion

        #region TryParse(Text, out PartnerServiceSessionId)

        /// <summary>
        /// Try to parse the given string as a session action identification.
        /// </summary>
        /// <param name="Text">A text representation of a session action identification.</param>
        /// <param name="PartnerServiceSessionId">The parsed session action identification.</param>
        public static Boolean TryParse(String Text, out SessionAction_Id PartnerServiceSessionId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                PartnerServiceSessionId = default(SessionAction_Id);
                return false;
            }

            #endregion

            try
            {

                PartnerServiceSessionId = new SessionAction_Id(Text);

                return true;

            }
            catch (Exception)
            { }

            PartnerServiceSessionId = default(SessionAction_Id);
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


        public static SessionAction_Id Zero
                    => new SessionAction_Id("0");


        #region Operator overloading

        #region Provider == (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A session action identification.</param>
        /// <param name="PartnerServiceSessionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SessionAction_Id PartnerServiceSessionId1, SessionAction_Id PartnerServiceSessionId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PartnerServiceSessionId1, PartnerServiceSessionId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PartnerServiceSessionId1 == null) || ((Object) PartnerServiceSessionId2 == null))
                return false;

            return PartnerServiceSessionId1.Equals(PartnerServiceSessionId2);

        }

        #endregion

        #region Provider != (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A session action identification.</param>
        /// <param name="PartnerServiceSessionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SessionAction_Id PartnerServiceSessionId1, SessionAction_Id PartnerServiceSessionId2)
            => !(PartnerServiceSessionId1 == PartnerServiceSessionId2);

        #endregion

        #region Provider <  (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A session action identification.</param>
        /// <param name="PartnerServiceSessionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SessionAction_Id PartnerServiceSessionId1, SessionAction_Id PartnerServiceSessionId2)
        {

            if ((Object) PartnerServiceSessionId1 == null)
                throw new ArgumentNullException(nameof(PartnerServiceSessionId1), "The given PartnerServiceSessionId1 must not be null!");

            return PartnerServiceSessionId1.CompareTo(PartnerServiceSessionId2) < 0;

        }

        #endregion

        #region Provider <= (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A session action identification.</param>
        /// <param name="PartnerServiceSessionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SessionAction_Id PartnerServiceSessionId1, SessionAction_Id PartnerServiceSessionId2)
            => !(PartnerServiceSessionId1 > PartnerServiceSessionId2);

        #endregion

        #region Provider >  (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A session action identification.</param>
        /// <param name="PartnerServiceSessionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SessionAction_Id PartnerServiceSessionId1, SessionAction_Id PartnerServiceSessionId2)
        {

            if ((Object) PartnerServiceSessionId1 == null)
                throw new ArgumentNullException(nameof(PartnerServiceSessionId1), "The given PartnerServiceSessionId1 must not be null!");

            return PartnerServiceSessionId1.CompareTo(PartnerServiceSessionId2) > 0;

        }

        #endregion

        #region Provider >= (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A session action identification.</param>
        /// <param name="PartnerServiceSessionId2">Another session action identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SessionAction_Id PartnerServiceSessionId1, SessionAction_Id PartnerServiceSessionId2)
            => !(PartnerServiceSessionId1 < PartnerServiceSessionId2);

        #endregion

        #endregion

        #region IComparable<PartnerServiceSessionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is SessionAction_Id))
                throw new ArgumentException("The given object is not a session action identification!",
                                            nameof(Object));

            return CompareTo((SessionAction_Id) Object);

        }

        #endregion

        #region CompareTo(PartnerServiceSessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId">An object to compare with.</param>
        public Int32 CompareTo(SessionAction_Id PartnerServiceSessionId)
        {

            if ((Object) PartnerServiceSessionId == null)
                throw new ArgumentNullException(nameof(PartnerServiceSessionId),  "The given session action identification must not be null!");

            return String.Compare(InternalId, PartnerServiceSessionId.InternalId, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<PartnerServiceSessionId> Members

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

            if (!(Object is SessionAction_Id))
                return false;

            return Equals((SessionAction_Id) Object);

        }

        #endregion

        #region Equals(PartnerServiceSessionId)

        /// <summary>
        /// Compares two PartnerServiceSessionIds for equality.
        /// </summary>
        /// <param name="PartnerServiceSessionId">A session action identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SessionAction_Id PartnerServiceSessionId)
        {

            if ((Object) PartnerServiceSessionId == null)
                return false;

            return InternalId.Equals(PartnerServiceSessionId.InternalId);

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
