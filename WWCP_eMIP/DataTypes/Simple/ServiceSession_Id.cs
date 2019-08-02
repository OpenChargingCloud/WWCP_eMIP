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
    /// The unique identification of a service session.
    /// </summary>
    public struct ServiceSession_Id : IId,
                                      IEquatable <ServiceSession_Id>,
                                      IComparable<ServiceSession_Id>

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
        /// The length of the service session identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new service session identification based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of a service session identification.</param>
        private ServiceSession_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Random(Length = 20)

        public static ServiceSession_Id Random(Byte Length = 20)
            => new ServiceSession_Id(_Random.RandomString(Length));

        #endregion

        #region (static) Zero

        public static ServiceSession_Id Zero
            => new ServiceSession_Id("0");

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a service session identification.
        /// </summary>
        /// <param name="Text">A text representation of a service session identification.</param>
        public static ServiceSession_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a service session identification must not be null or empty!");

            #endregion

            if (TryParse(Text, out ServiceSession_Id ServiceSessionId))
                return ServiceSessionId;

            throw new ArgumentNullException(nameof(Text), "The given text representation of a service session identification is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a service session identification.
        /// </summary>
        /// <param name="Text">A text representation of a service session identification.</param>
        public static ServiceSession_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ServiceSession_Id ServiceSessionId))
                return ServiceSessionId;

            return new ServiceSession_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out ServiceSessionId)

        /// <summary>
        /// Try to parse the given string as a service session identification.
        /// </summary>
        /// <param name="Text">A text representation of a service session identification.</param>
        /// <param name="ServiceSessionId">The parsed service session identification.</param>
        public static Boolean TryParse(String Text, out ServiceSession_Id ServiceSessionId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                ServiceSessionId = default;
                return false;
            }

            #endregion

            try
            {
                ServiceSessionId = new ServiceSession_Id(Text);
                return true;
            }
            catch (Exception)
            { }

            ServiceSessionId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this service session identification.
        /// </summary>
        public ServiceSession_Id Clone

            => new ServiceSession_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Provider == (ServiceSessionId1, ServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceSessionId1">A service session identification.</param>
        /// <param name="ServiceSessionId2">Another service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ServiceSession_Id ServiceSessionId1, ServiceSession_Id ServiceSessionId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ServiceSessionId1, ServiceSessionId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ServiceSessionId1 == null) || ((Object) ServiceSessionId2 == null))
                return false;

            return ServiceSessionId1.Equals(ServiceSessionId2);

        }

        #endregion

        #region Provider != (ServiceSessionId1, ServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceSessionId1">A service session identification.</param>
        /// <param name="ServiceSessionId2">Another service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ServiceSession_Id ServiceSessionId1, ServiceSession_Id ServiceSessionId2)
            => !(ServiceSessionId1 == ServiceSessionId2);

        #endregion

        #region Provider <  (ServiceSessionId1, ServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceSessionId1">A service session identification.</param>
        /// <param name="ServiceSessionId2">Another service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ServiceSession_Id ServiceSessionId1, ServiceSession_Id ServiceSessionId2)
        {

            if ((Object) ServiceSessionId1 == null)
                throw new ArgumentNullException(nameof(ServiceSessionId1), "The given ServiceSessionId1 must not be null!");

            return ServiceSessionId1.CompareTo(ServiceSessionId2) < 0;

        }

        #endregion

        #region Provider <= (ServiceSessionId1, ServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceSessionId1">A service session identification.</param>
        /// <param name="ServiceSessionId2">Another service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ServiceSession_Id ServiceSessionId1, ServiceSession_Id ServiceSessionId2)
            => !(ServiceSessionId1 > ServiceSessionId2);

        #endregion

        #region Provider >  (ServiceSessionId1, ServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceSessionId1">A service session identification.</param>
        /// <param name="ServiceSessionId2">Another service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ServiceSession_Id ServiceSessionId1, ServiceSession_Id ServiceSessionId2)
        {

            if ((Object) ServiceSessionId1 == null)
                throw new ArgumentNullException(nameof(ServiceSessionId1), "The given ServiceSessionId1 must not be null!");

            return ServiceSessionId1.CompareTo(ServiceSessionId2) > 0;

        }

        #endregion

        #region Provider >= (ServiceSessionId1, ServiceSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceSessionId1">A service session identification.</param>
        /// <param name="ServiceSessionId2">Another service session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ServiceSession_Id ServiceSessionId1, ServiceSession_Id ServiceSessionId2)
            => !(ServiceSessionId1 < ServiceSessionId2);

        #endregion

        #endregion

        #region IComparable<ServiceSessionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ServiceSession_Id ServiceSessionId))
                throw new ArgumentException("The given object is not a service session identification!",
                                            nameof(Object));

            return CompareTo(ServiceSessionId);

        }

        #endregion

        #region CompareTo(ServiceSessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ServiceSessionId">An object to compare with.</param>
        public Int32 CompareTo(ServiceSession_Id ServiceSessionId)
        {

            if ((Object) ServiceSessionId == null)
                throw new ArgumentNullException(nameof(ServiceSessionId),  "The given service session identification must not be null!");

            return String.Compare(InternalId, ServiceSessionId.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<ServiceSessionId> Members

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

            if (!(Object is ServiceSession_Id ServiceSessionId))
                return false;

            return Equals(ServiceSessionId);

        }

        #endregion

        #region Equals(ServiceSessionId)

        /// <summary>
        /// Compares two ServiceSessionIds for equality.
        /// </summary>
        /// <param name="ServiceSessionId">A service session identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ServiceSession_Id ServiceSessionId)
        {

            if ((Object) ServiceSessionId == null)
                return false;

            return InternalId.ToLower().Equals(ServiceSessionId.InternalId.ToLower());

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
