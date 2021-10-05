/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
    /// The unique identification of a service.
    /// </summary>
    public struct Service_Id : IId,
                               IEquatable <Service_Id>,
                               IComparable<Service_Id>

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
        /// The length of the service identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

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


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a service identification.
        /// </summary>
        /// <param name="Text">A text representation of a service identification.</param>
        public static Service_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a service identification must not be null or empty!");

            #endregion

            if (TryParse(Text, out Service_Id ServiceId))
                return ServiceId;

            throw new ArgumentNullException(nameof(Text), "The given text representation of a service identification is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a service identification.
        /// </summary>
        /// <param name="Text">A text representation of a service identification.</param>
        public static Service_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Service_Id ServiceId))
                return ServiceId;

            return new Service_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out ServiceId)

        /// <summary>
        /// Try to parse the given string as a service identification.
        /// </summary>
        /// <param name="Text">A text representation of a service identification.</param>
        /// <param name="ServiceId">The parsed service identification.</param>
        public static Boolean TryParse(String Text, out Service_Id ServiceId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                ServiceId = default;
                return false;
            }

            #endregion

            try
            {
                ServiceId = new Service_Id(Text);
                return true;
            }
            catch (Exception)
            { }

            ServiceId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this service identification.
        /// </summary>
        public Service_Id Clone

            => new Service_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ServiceId1, ServiceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId1">A service identification.</param>
        /// <param name="ServiceId2">Another service identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Service_Id ServiceId1, Service_Id ServiceId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ServiceId1, ServiceId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ServiceId1 == null) || ((Object) ServiceId2 == null))
                return false;

            return ServiceId1.Equals(ServiceId2);

        }

        #endregion

        #region Operator != (ServiceId1, ServiceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId1">A service identification.</param>
        /// <param name="ServiceId2">Another service identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Service_Id ServiceId1, Service_Id ServiceId2)
            => !(ServiceId1 == ServiceId2);

        #endregion

        #region Operator <  (ServiceId1, ServiceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId1">A service identification.</param>
        /// <param name="ServiceId2">Another service identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Service_Id ServiceId1, Service_Id ServiceId2)
        {

            if ((Object) ServiceId1 == null)
                throw new ArgumentNullException(nameof(ServiceId1), "The given ServiceId1 must not be null!");

            return ServiceId1.CompareTo(ServiceId2) < 0;

        }

        #endregion

        #region Operator <= (ServiceId1, ServiceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId1">A service identification.</param>
        /// <param name="ServiceId2">Another service identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Service_Id ServiceId1, Service_Id ServiceId2)
            => !(ServiceId1 > ServiceId2);

        #endregion

        #region Operator >  (ServiceId1, ServiceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId1">A service identification.</param>
        /// <param name="ServiceId2">Another service identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Service_Id ServiceId1, Service_Id ServiceId2)
        {

            if ((Object) ServiceId1 == null)
                throw new ArgumentNullException(nameof(ServiceId1), "The given ServiceId1 must not be null!");

            return ServiceId1.CompareTo(ServiceId2) > 0;

        }

        #endregion

        #region Operator >= (ServiceId1, ServiceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId1">A service identification.</param>
        /// <param name="ServiceId2">Another service identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Service_Id ServiceId1, Service_Id ServiceId2)
            => !(ServiceId1 < ServiceId2);

        #endregion

        #endregion

        #region IComparable<ServiceId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Service_Id ServiceId))
                throw new ArgumentException("The given object is not a service identification!",
                                            nameof(Object));

            return CompareTo(ServiceId);

        }

        #endregion

        #region CompareTo(ServiceId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceId">An object to compare with.</param>
        public Int32 CompareTo(Service_Id ServiceId)
        {

            if ((Object) ServiceId == null)
                throw new ArgumentNullException(nameof(ServiceId),  "The given service identification must not be null!");

            return String.Compare(InternalId, ServiceId.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<ServiceId> Members

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

            if (!(Object is Service_Id ServiceId))
                return false;

            return Equals(ServiceId);

        }

        #endregion

        #region Equals(ServiceId)

        /// <summary>
        /// Compares two ServiceIds for equality.
        /// </summary>
        /// <param name="ServiceId">A service identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Service_Id ServiceId)
        {

            if ((Object) ServiceId == null)
                return false;

            return InternalId.ToLower().Equals(ServiceId.InternalId.ToLower());

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
