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

using System;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
{

    /// <summary>
    /// An EVSE availability status.
    /// </summary>
    public class EVSEAvailabilityStatus
    {

        #region Properties

        /// <summary>
        /// The EVSE identification.
        /// </summary>
        public EVSE_Id                      EVSEId                       { get; }

        /// <summary>
        /// The timestamp of the status change.
        /// </summary>
        public DateTimeOffset               StatusEventDate              { get; }

        /// <summary>
        /// The EVSE availability status.
        /// </summary>
        public EVSEAvailabilityStatusTypes  AvailabilityStatus           { get; }

        /// <summary>
        /// The optional timestamp until which the given availability status is valid.
        /// </summary>
        public DateTimeOffset?              AvailabilityStatusUntil      { get; }

        /// <summary>
        /// The optional comment about the availability status.
        /// </summary>
        public String                       AvailabilityStatusComment    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE availability status.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="StatusEventDate">The timestamp of the status update.</param>
        /// <param name="AvailabilityStatus">The EVSE availability status.</param>
        /// <param name="AvailabilityStatusUntil">An optional timestamp until which the given availability status is valid.</param>
        /// <param name="AvailabilityStatusComment">An optional comment about the availability status.</param>
        public EVSEAvailabilityStatus(EVSE_Id                      EVSEId,
                                      DateTimeOffset               StatusEventDate,
                                      EVSEAvailabilityStatusTypes  AvailabilityStatus,
                                      DateTimeOffset?              AvailabilityStatusUntil     = null,
                                      String?                      AvailabilityStatusComment   = null)
        {

            this.EVSEId                     = EVSEId;
            this.StatusEventDate            = StatusEventDate;
            this.AvailabilityStatus         = AvailabilityStatus;
            this.AvailabilityStatusUntil    = AvailabilityStatusUntil;
            this.AvailabilityStatusComment  = AvailabilityStatusComment?.Trim();

        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSEAvailabilityStatus1, EVSEAvailabilityStatus2)

        /// <summary>
        /// Compares two EVSEAvailabilityStatus for equality.
        /// </summary>
        /// <param name="EVSEAvailabilityStatus1">An EVSEAvailabilityStatus.</param>
        /// <param name="EVSEAvailabilityStatus2">Another EVSEAvailabilityStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EVSEAvailabilityStatus EVSEAvailabilityStatus1, EVSEAvailabilityStatus EVSEAvailabilityStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EVSEAvailabilityStatus1, EVSEAvailabilityStatus2))
                return true;

            // If one is null, but not both, return false.
            if (EVSEAvailabilityStatus1 is null || EVSEAvailabilityStatus2 is null)
                return false;

            return EVSEAvailabilityStatus1.Equals(EVSEAvailabilityStatus2);

        }

        #endregion

        #region Operator != (EVSEAvailabilityStatus1, EVSEAvailabilityStatus2)

        /// <summary>
        /// Compares two EVSEAvailabilityStatus for inequality.
        /// </summary>
        /// <param name="EVSEAvailabilityStatus1">An EVSEAvailabilityStatus.</param>
        /// <param name="EVSEAvailabilityStatus2">Another EVSEAvailabilityStatus.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVSEAvailabilityStatus EVSEAvailabilityStatus1, EVSEAvailabilityStatus EVSEAvailabilityStatus2)

            => !(EVSEAvailabilityStatus1 == EVSEAvailabilityStatus2);

        #endregion

        #endregion

        #region IEquatable<EVSEAvailabilityStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            var EVSEAvailabilityStatus = Object as EVSEAvailabilityStatus;
            if ((Object) EVSEAvailabilityStatus is null)
                return false;

            return Equals(EVSEAvailabilityStatus);

        }

        #endregion

        #region Equals(EVSEAvailabilityStatus)

        /// <summary>
        /// Compares two EVSEAvailabilityStatus for equality.
        /// </summary>
        /// <param name="EVSEAvailabilityStatus">An EVSEAvailabilityStatus to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEAvailabilityStatus EVSEAvailabilityStatus)
        {

            if ((Object) EVSEAvailabilityStatus is null)
                return false;

            return EVSEId.         Equals(EVSEAvailabilityStatus.EVSEId)          &&
                   StatusEventDate.Equals(EVSEAvailabilityStatus.StatusEventDate) &&
                   AvailabilityStatus.     Equals(EVSEAvailabilityStatus.AvailabilityStatus)      &&

                   ((!AvailabilityStatusUntil.HasValue && !EVSEAvailabilityStatus.AvailabilityStatusUntil.HasValue) ||
                     (AvailabilityStatusUntil.HasValue &&  EVSEAvailabilityStatus.AvailabilityStatusUntil.HasValue && AvailabilityStatusUntil.Value.Equals(EVSEAvailabilityStatus.AvailabilityStatusUntil.Value))) &&

                   ((!AvailabilityStatusComment.IsNeitherNullNorEmpty() && !EVSEAvailabilityStatus.AvailabilityStatusComment.IsNeitherNullNorEmpty()) ||
                     (AvailabilityStatusComment.IsNeitherNullNorEmpty() &&  EVSEAvailabilityStatus.AvailabilityStatusComment.IsNeitherNullNorEmpty() && AvailabilityStatusComment.Equals(EVSEAvailabilityStatus.AvailabilityStatusComment)));

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return EVSEId.         GetHashCode() * 11 ^
                       StatusEventDate.GetHashCode() *  7 ^
                       AvailabilityStatus.     GetHashCode() *  5 ^

                       (AvailabilityStatusUntil.HasValue
                            ? AvailabilityStatusUntil.  GetHashCode() * 3
                            : 0) ^

                       (AvailabilityStatusComment.IsNeitherNullNorEmpty()
                            ? AvailabilityStatusComment.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEId, " -> ", AvailabilityStatus.AsText());

        #endregion

    }

}
