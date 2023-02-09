/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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
    /// A EVSE busy status.
    /// </summary>
    public class EVSEBusyStatus
    {

        #region Properties

        /// <summary>
        /// The EVSE identification.
        /// </summary>
        public EVSE_Id              EVSEId               { get; }

        /// <summary>
        /// The timestamp of the status change.
        /// </summary>
        public DateTime             StatusEventDate      { get; }

        /// <summary>
        /// The EVSE busy status.
        /// </summary>
        public EVSEBusyStatusTypes  BusyStatus           { get; }

        /// <summary>
        /// The optional timestamp until which the given busy status is valid.
        /// </summary>
        public DateTime?            BusyStatusUntil      { get; }

        /// <summary>
        /// The optional comment about the busy status.
        /// </summary>
        public String               BusyStatusComment    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE busy status.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="StatusEventDate">The timestamp of the status change.</param>
        /// <param name="BusyStatus">The EVSE busy status.</param>
        /// <param name="BusyStatusUntil">An optional timestamp until which the given busy status is valid.</param>
        /// <param name="BusyStatusComment">An optional comment about the busy status.</param>
        public EVSEBusyStatus(EVSE_Id              EVSEId,
                              DateTime             StatusEventDate,
                              EVSEBusyStatusTypes  BusyStatus,
                              DateTime?            BusyStatusUntil     = null,
                              String               BusyStatusComment   = null)
        {

            this.EVSEId             = EVSEId;
            this.StatusEventDate    = StatusEventDate;
            this.BusyStatus         = BusyStatus;
            this.BusyStatusUntil    = BusyStatusUntil;
            this.BusyStatusComment  = BusyStatusComment?.Trim();

        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSEBusyStatus1, EVSEBusyStatus2)

        /// <summary>
        /// Compares two EVSEBusyStatus for equality.
        /// </summary>
        /// <param name="EVSEBusyStatus1">An EVSEBusyStatus.</param>
        /// <param name="EVSEBusyStatus2">Another EVSEBusyStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EVSEBusyStatus EVSEBusyStatus1, EVSEBusyStatus EVSEBusyStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EVSEBusyStatus1, EVSEBusyStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEBusyStatus1 == null) || ((Object) EVSEBusyStatus2 == null))
                return false;

            return EVSEBusyStatus1.Equals(EVSEBusyStatus2);

        }

        #endregion

        #region Operator != (EVSEBusyStatus1, EVSEBusyStatus2)

        /// <summary>
        /// Compares two EVSEBusyStatus for inequality.
        /// </summary>
        /// <param name="EVSEBusyStatus1">An EVSEBusyStatus.</param>
        /// <param name="EVSEBusyStatus2">Another EVSEBusyStatus.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVSEBusyStatus EVSEBusyStatus1, EVSEBusyStatus EVSEBusyStatus2)

            => !(EVSEBusyStatus1 == EVSEBusyStatus2);

        #endregion

        #endregion

        #region IEquatable<EVSEBusyStatus> Members

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

            var EVSEBusyStatus = Object as EVSEBusyStatus;
            if ((Object) EVSEBusyStatus == null)
                return false;

            return Equals(EVSEBusyStatus);

        }

        #endregion

        #region Equals(EVSEBusyStatus)

        /// <summary>
        /// Compares two EVSEBusyStatus for equality.
        /// </summary>
        /// <param name="EVSEBusyStatus">An EVSEBusyStatus to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEBusyStatus EVSEBusyStatus)
        {

            if ((Object) EVSEBusyStatus == null)
                return false;

            return EVSEId.         Equals(EVSEBusyStatus.EVSEId)          &&
                   StatusEventDate.Equals(EVSEBusyStatus.StatusEventDate) &&
                   BusyStatus.     Equals(EVSEBusyStatus.BusyStatus)      &&

                   ((!BusyStatusUntil.HasValue && !EVSEBusyStatus.BusyStatusUntil.HasValue) ||
                     (BusyStatusUntil.HasValue &&  EVSEBusyStatus.BusyStatusUntil.HasValue && BusyStatusUntil.Value.Equals(EVSEBusyStatus.BusyStatusUntil.Value))) &&

                   ((!BusyStatusComment.IsNeitherNullNorEmpty() && !EVSEBusyStatus.BusyStatusComment.IsNeitherNullNorEmpty()) ||
                     (BusyStatusComment.IsNeitherNullNorEmpty() &&  EVSEBusyStatus.BusyStatusComment.IsNeitherNullNorEmpty() && BusyStatusComment.Equals(EVSEBusyStatus.BusyStatusComment)));

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
                       BusyStatus.     GetHashCode() *  5 ^

                       (BusyStatusUntil.HasValue
                            ? BusyStatusUntil.  GetHashCode() * 3
                            : 0) ^

                       (BusyStatusComment.IsNeitherNullNorEmpty()
                            ? BusyStatusComment.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEId, " -> ", BusyStatus.AsText());

        #endregion

    }

}
