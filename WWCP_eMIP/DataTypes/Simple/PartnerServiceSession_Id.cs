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
    /// The unique identification of a partner service session.
    /// </summary>
    public struct PartnerServiceSession_Id : IId,
                                             IEquatable <PartnerServiceSession_Id>,
                                             IComparable<PartnerServiceSession_Id>

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
        /// The length of the partner service session identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new partner service session identification.
        /// based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of a partner service session identification.</param>
        private PartnerServiceSession_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Random(Length = 20)

        public static PartnerServiceSession_Id Random(Byte Length = 20)
            => new PartnerServiceSession_Id(_Random.RandomString(Length));

        #endregion

        #region (static) Zero

        public static PartnerServiceSession_Id Zero
            => new PartnerServiceSession_Id("0");

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a partner service session identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner service session identification.</param>
        public static PartnerServiceSession_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a partner service session identification must not be null or empty!");

            #endregion

            if (TryParse(Text, out PartnerServiceSession_Id PartnerServiceSessionId))
                return PartnerServiceSessionId;

            throw new ArgumentNullException(nameof(Text), "The given text representation of a partner service session identification is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a partner service session identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner service session identification.</param>
        public static PartnerServiceSession_Id? TryParse(String Text)
        {

            if (TryParse(Text, out PartnerServiceSession_Id PartnerServiceSessionId))
                return PartnerServiceSessionId;

            return new PartnerServiceSession_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out PartnerServiceSessionId)

        /// <summary>
        /// Try to parse the given string as a partner service session identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner service session identification.</param>
        /// <param name="PartnerServiceSessionId">The parsed partner service session identification.</param>
        public static Boolean TryParse(String Text, out PartnerServiceSession_Id PartnerServiceSessionId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                PartnerServiceSessionId = default;
                return false;
            }

            #endregion

            try
            {
                PartnerServiceSessionId = new PartnerServiceSession_Id(Text);
                return true;
            }
            catch (Exception)
            { }

            PartnerServiceSessionId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this partner service session identification.
        /// </summary>
        public PartnerServiceSession_Id Clone

            => new PartnerServiceSession_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A partner service session identification.</param>
        /// <param name="PartnerServiceSessionId2">Another partner service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (PartnerServiceSession_Id PartnerServiceSessionId1, PartnerServiceSession_Id PartnerServiceSessionId2)
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

        #region Operator != (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A partner service session identification.</param>
        /// <param name="PartnerServiceSessionId2">Another partner service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PartnerServiceSession_Id PartnerServiceSessionId1, PartnerServiceSession_Id PartnerServiceSessionId2)
            => !(PartnerServiceSessionId1 == PartnerServiceSessionId2);

        #endregion

        #region Operator <  (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A partner service session identification.</param>
        /// <param name="PartnerServiceSessionId2">Another partner service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (PartnerServiceSession_Id PartnerServiceSessionId1, PartnerServiceSession_Id PartnerServiceSessionId2)
        {

            if ((Object) PartnerServiceSessionId1 == null)
                throw new ArgumentNullException(nameof(PartnerServiceSessionId1), "The given PartnerServiceSessionId1 must not be null!");

            return PartnerServiceSessionId1.CompareTo(PartnerServiceSessionId2) < 0;

        }

        #endregion

        #region Operator <= (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A partner service session identification.</param>
        /// <param name="PartnerServiceSessionId2">Another partner service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (PartnerServiceSession_Id PartnerServiceSessionId1, PartnerServiceSession_Id PartnerServiceSessionId2)
            => !(PartnerServiceSessionId1 > PartnerServiceSessionId2);

        #endregion

        #region Operator >  (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A partner service session identification.</param>
        /// <param name="PartnerServiceSessionId2">Another partner service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (PartnerServiceSession_Id PartnerServiceSessionId1, PartnerServiceSession_Id PartnerServiceSessionId2)
        {

            if ((Object) PartnerServiceSessionId1 == null)
                throw new ArgumentNullException(nameof(PartnerServiceSessionId1), "The given PartnerServiceSessionId1 must not be null!");

            return PartnerServiceSessionId1.CompareTo(PartnerServiceSessionId2) > 0;

        }

        #endregion

        #region Operator >= (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A partner service session identification.</param>
        /// <param name="PartnerServiceSessionId2">Another partner service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (PartnerServiceSession_Id PartnerServiceSessionId1, PartnerServiceSession_Id PartnerServiceSessionId2)
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

            if (!(Object is PartnerServiceSession_Id))
                throw new ArgumentException("The given object is not a partner service session identification!",
                                            nameof(Object));

            return CompareTo((PartnerServiceSession_Id) Object);

        }

        #endregion

        #region CompareTo(PartnerServiceSessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId">An object to compare with.</param>
        public Int32 CompareTo(PartnerServiceSession_Id PartnerServiceSessionId)
        {

            if ((Object) PartnerServiceSessionId == null)
                throw new ArgumentNullException(nameof(PartnerServiceSessionId),  "The given partner service session identification must not be null!");

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

            if (!(Object is PartnerServiceSession_Id))
                return false;

            return Equals((PartnerServiceSession_Id) Object);

        }

        #endregion

        #region Equals(PartnerServiceSessionId)

        /// <summary>
        /// Compares two PartnerServiceSessionIds for equality.
        /// </summary>
        /// <param name="PartnerServiceSessionId">A partner service session identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(PartnerServiceSession_Id PartnerServiceSessionId)
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
