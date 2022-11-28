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
    /// Extension methods for service identifications.
    /// </summary>
    public static class ServiceIdExtensions
    {

        /// <summary>
        /// Indicates whether this service identification is null or empty.
        /// </summary>
        /// <param name="ServiceId">A service identification.</param>
        public static Boolean IsNullOrEmpty(this Service_Id? ServiceId)
            => !ServiceId.HasValue || ServiceId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this service identification is null or empty.
        /// </summary>
        /// <param name="ServiceId">A service identification.</param>
        public static Boolean IsNotNullOrEmpty(this Service_Id? ServiceId)
            => ServiceId.HasValue && ServiceId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a service.
    /// </summary>
    public readonly struct Service_Id : IId,
                                        IEquatable <Service_Id>,
                                        IComparable<Service_Id>

    {

        #region Data

        /// <summary>
        /// The internal service identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this service identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this service identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the service identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new service identification based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of a service identification.</param>
        private Service_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Random(Length)

        /// <summary>
        /// Create a new random user identification.
        /// </summary>
        /// <param name="Length">The expected length of the random user identification.</param>
        public static Service_Id Random(Byte Length = 15)

            => new (RandomExtensions.RandomString(Length).ToUpper());

        #endregion

        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a service identification.
        /// </summary>
        /// <param name="Text">A text-representation of a service identification.</param>
        public static Service_Id Parse(String Text)
        {

            if (TryParse(Text, out Service_Id serviceId))
                return serviceId;

            throw new ArgumentException("Invalid text representation of a service identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a service identification.
        /// </summary>
        /// <param name="Text">A text-representation of a service identification.</param>
        public static Service_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Service_Id serviceId))
                return serviceId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ServiceId)

        /// <summary>
        /// Try to parse the given string as a service identification.
        /// </summary>
        /// <param name="Text">A text-representation of a service identification.</param>
        /// <param name="ServiceId">The parsed user identification.</param>
        public static Boolean TryParse(String Text, out Service_Id ServiceId)
        {

            Text = Text?.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ServiceId = new Service_Id(Text);
                    return true;
                }
                catch
                { }
            }

            ServiceId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this user identification.
        /// </summary>
        public Service_Id Clone

            => new Service_Id(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region (static) Generic Charge Service

        /// <summary>
        /// Generic Charge Service ("1")
        /// </summary>
        public static Service_Id GenericChargeService
            => new Service_Id("1");

        #endregion


        #region Operator overloading

        #region Operator == (ServiceId1, ServiceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId1">An user identification.</param>
        /// <param name="ServiceId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Service_Id ServiceId1,
                                           Service_Id ServiceId2)

            => ServiceId1.Equals(ServiceId2);

        #endregion

        #region Operator != (ServiceId1, ServiceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId1">An user identification.</param>
        /// <param name="ServiceId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Service_Id ServiceId1,
                                           Service_Id ServiceId2)

            => !ServiceId1.Equals(ServiceId2);

        #endregion

        #region Operator <  (ServiceId1, ServiceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId1">An user identification.</param>
        /// <param name="ServiceId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Service_Id ServiceId1,
                                          Service_Id ServiceId2)

            => ServiceId1.CompareTo(ServiceId2) < 0;

        #endregion

        #region Operator <= (ServiceId1, ServiceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId1">An user identification.</param>
        /// <param name="ServiceId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Service_Id ServiceId1,
                                           Service_Id ServiceId2)

            => ServiceId1.CompareTo(ServiceId2) <= 0;

        #endregion

        #region Operator >  (ServiceId1, ServiceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId1">An user identification.</param>
        /// <param name="ServiceId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Service_Id ServiceId1,
                                          Service_Id ServiceId2)

            => ServiceId1.CompareTo(ServiceId2) > 0;

        #endregion

        #region Operator >= (ServiceId1, ServiceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId1">An user identification.</param>
        /// <param name="ServiceId2">Another user identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Service_Id ServiceId1,
                                           Service_Id ServiceId2)

            => ServiceId1.CompareTo(ServiceId2) >= 0;

        #endregion

        #endregion

        #region IComparable<Service_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is Service_Id serviceId
                   ? CompareTo(serviceId)
                   : throw new ArgumentException("The given object is not a service identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ServiceId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId">An object to compare with.</param>
        public Int32 CompareTo(Service_Id ServiceId)

            => String.Compare(InternalId,
                              ServiceId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<Service_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is Service_Id serviceId &&
                   Equals(serviceId);

        #endregion

        #region Equals(ServiceId)

        /// <summary>
        /// Compares two user identifications for equality.
        /// </summary>
        /// <param name="ServiceId">An user identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Service_Id ServiceId)

            => String.Equals(InternalId,
                             ServiceId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.ToLower().GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
