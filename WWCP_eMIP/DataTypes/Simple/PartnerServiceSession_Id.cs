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
    /// Extension methods for partner service session identifications.
    /// </summary>
    public static class PartnerServiceSessionIdExtensions
    {

        /// <summary>
        /// Indicates whether this partner service session identification is null or empty.
        /// </summary>
        /// <param name="PartnerServiceSessionId">A partner service session identification.</param>
        public static Boolean IsNullOrEmpty(this PartnerServiceSession_Id? PartnerServiceSessionId)
            => !PartnerServiceSessionId.HasValue || PartnerServiceSessionId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this partner service session identification is NOT null or empty.
        /// </summary>
        /// <param name="PartnerServiceSessionId">A partner service session identification.</param>
        public static Boolean IsNotNullOrEmpty(this PartnerServiceSession_Id? PartnerServiceSessionId)
            => PartnerServiceSessionId.HasValue && PartnerServiceSessionId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a partner service session.
    /// </summary>
    public readonly struct PartnerServiceSession_Id : IId,
                                                      IEquatable <PartnerServiceSession_Id>,
                                                      IComparable<PartnerServiceSession_Id>
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
        /// The length of the partner service session identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

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

            => new (RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Zero

        public static PartnerServiceSession_Id Zero
            => new ("0");

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a partner service session identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner service session identification.</param>
        public static PartnerServiceSession_Id Parse(String Text)
        {

            if (TryParse(Text, out var partnerServiceSessionId))
                return partnerServiceSessionId;

            throw new ArgumentException($"Invalid text representation of a partner service session identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a partner service session identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner service session identification.</param>
        public static PartnerServiceSession_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var partnerServiceSessionId))
                return partnerServiceSessionId;

            return null;

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
            catch
            { }

            PartnerServiceSessionId = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this partner service session identification.
        /// </summary>
        public PartnerServiceSession_Id Clone()

            => new (
                   InternalId.CloneString()
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
        public static Boolean operator == (PartnerServiceSession_Id PartnerServiceSessionId1,
                                           PartnerServiceSession_Id PartnerServiceSessionId2)

            => PartnerServiceSessionId1.Equals(PartnerServiceSessionId2);

        #endregion

        #region Operator != (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A partner service session identification.</param>
        /// <param name="PartnerServiceSessionId2">Another partner service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PartnerServiceSession_Id PartnerServiceSessionId1,
                                           PartnerServiceSession_Id PartnerServiceSessionId2)

            => !PartnerServiceSessionId1.Equals(PartnerServiceSessionId2);

        #endregion

        #region Operator <  (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A partner service session identification.</param>
        /// <param name="PartnerServiceSessionId2">Another partner service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (PartnerServiceSession_Id PartnerServiceSessionId1,
                                          PartnerServiceSession_Id PartnerServiceSessionId2)

            => PartnerServiceSessionId1.CompareTo(PartnerServiceSessionId2) < 0;

        #endregion

        #region Operator <= (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A partner service session identification.</param>
        /// <param name="PartnerServiceSessionId2">Another partner service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (PartnerServiceSession_Id PartnerServiceSessionId1,
                                           PartnerServiceSession_Id PartnerServiceSessionId2)

            => PartnerServiceSessionId1.CompareTo(PartnerServiceSessionId2) <= 0;

        #endregion

        #region Operator >  (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A partner service session identification.</param>
        /// <param name="PartnerServiceSessionId2">Another partner service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (PartnerServiceSession_Id PartnerServiceSessionId1,
                                          PartnerServiceSession_Id PartnerServiceSessionId2)

            => PartnerServiceSessionId1.CompareTo(PartnerServiceSessionId2) > 0;

        #endregion

        #region Operator >= (PartnerServiceSessionId1, PartnerServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerServiceSessionId1">A partner service session identification.</param>
        /// <param name="PartnerServiceSessionId2">Another partner service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (PartnerServiceSession_Id PartnerServiceSessionId1,
                                           PartnerServiceSession_Id PartnerServiceSessionId2)

            => PartnerServiceSessionId1.CompareTo(PartnerServiceSessionId2) >= 0;

        #endregion

        #endregion

        #region IComparable<PartnerServiceSession_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two partner service session identifications.
        /// </summary>
        /// <param name="Object">A partner service session identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is PartnerServiceSession_Id partnerServiceSessionId
                   ? CompareTo(partnerServiceSessionId)
                   : throw new ArgumentException("The given object is not a partner service session identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(PartnerServiceSessionId)

        /// <summary>
        /// Compares two partner service session identifications.
        /// </summary>
        /// <param name="PartnerServiceSessionId">A partner service session identification to compare with.</param>
        public Int32 CompareTo(PartnerServiceSession_Id PartnerServiceSessionId)

            => String.Compare(InternalId,
                              PartnerServiceSessionId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<PartnerServiceSessionId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two partner service session identifications for equality.
        /// </summary>
        /// <param name="Object">A partner service session identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is PartnerServiceSession_Id partnerServiceSessionId &&
                   Equals(partnerServiceSessionId);

        #endregion

        #region Equals(PartnerServiceSession_Id)

        /// <summary>
        /// Compares two partner service session identifications for equality.
        /// </summary>
        /// <param name="PartnerServiceSessionId">A partner service session identification to compare with.</param>
        public Boolean Equals(PartnerServiceSession_Id PartnerServiceSessionId)

            => String.Equals(InternalId,
                             PartnerServiceSessionId.InternalId,
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
