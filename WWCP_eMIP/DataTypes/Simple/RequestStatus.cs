/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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
    /// An eMIP request status.
    /// </summary>
    public struct RequestStatus : IId,
                                  IEquatable <RequestStatus>,
                                  IComparable<RequestStatus>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly Int32 InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// The length of the request status.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.ToString().Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new eMIP request status.
        /// </summary>
        private RequestStatus(Int32 Value)
        {
            InternalId = Value;
        }

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a request status.
        /// </summary>
        /// <param name="Text">A text representation of a request status.</param>
        public static RequestStatus Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a request status must not be null or empty!");

            #endregion

            return new RequestStatus(Int32.Parse(Text));

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a request status.
        /// </summary>
        /// <param name="Text">A text representation of a request status.</param>
        public static RequestStatus? TryParse(String Text)
        {

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out Int32 Value))
                return new RequestStatus?();

            return new RequestStatus(Value);

        }

        #endregion

        #region TryParse(Text, out RequestStatus)

        /// <summary>
        /// Try to parse the given string as a request status.
        /// </summary>
        /// <param name="Text">A text representation of a request status.</param>
        /// <param name="RequestStatus">The parsed request status.</param>
        public static Boolean TryParse(String Text, out RequestStatus RequestStatus)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            #endregion

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out Int32 Value))
            {
                RequestStatus = default(RequestStatus);
                return false;
            }

            RequestStatus = new RequestStatus(Value);
            return true;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this request status.
        /// </summary>
        public RequestStatus Clone
            => new RequestStatus(InternalId);

        #endregion


        public static RequestStatus Ok
            => new RequestStatus(0);

        public static RequestStatus SystemError
            => new RequestStatus(-9999990);

        public static RequestStatus HTTPError
            => new RequestStatus(-9999991);

        public static RequestStatus ServiceNotAvailable
            => new RequestStatus(-9999992);

        public static RequestStatus DataError
            => new RequestStatus(-9999993);


        #region Operator overloading

        #region Operator == (RequestStatus1, RequestStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus1">A request status.</param>
        /// <param name="RequestStatus2">Another request status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RequestStatus RequestStatus1, RequestStatus RequestStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(RequestStatus1, RequestStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RequestStatus1 == null) || ((Object) RequestStatus2 == null))
                return false;

            return RequestStatus1.Equals(RequestStatus2);

        }

        #endregion

        #region Operator != (RequestStatus1, RequestStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus1">A request status.</param>
        /// <param name="RequestStatus2">Another request status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RequestStatus RequestStatus1, RequestStatus RequestStatus2)
            => !(RequestStatus1 == RequestStatus2);

        #endregion

        #region Operator <  (RequestStatus1, RequestStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus1">A request status.</param>
        /// <param name="RequestStatus2">Another request status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RequestStatus RequestStatus1, RequestStatus RequestStatus2)
        {

            if ((Object) RequestStatus1 == null)
                throw new ArgumentNullException(nameof(RequestStatus1), "The given RequestStatus1 must not be null!");

            return RequestStatus1.CompareTo(RequestStatus2) < 0;

        }

        #endregion

        #region Operator <= (RequestStatus1, RequestStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus1">A request status.</param>
        /// <param name="RequestStatus2">Another request status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RequestStatus RequestStatus1, RequestStatus RequestStatus2)
            => !(RequestStatus1 > RequestStatus2);

        #endregion

        #region Operator >  (RequestStatus1, RequestStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus1">A request status.</param>
        /// <param name="RequestStatus2">Another request status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RequestStatus RequestStatus1, RequestStatus RequestStatus2)
        {

            if ((Object) RequestStatus1 == null)
                throw new ArgumentNullException(nameof(RequestStatus1), "The given RequestStatus1 must not be null!");

            return RequestStatus1.CompareTo(RequestStatus2) > 0;

        }

        #endregion

        #region Operator >= (RequestStatus1, RequestStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus1">A request status.</param>
        /// <param name="RequestStatus2">Another request status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RequestStatus RequestStatus1, RequestStatus RequestStatus2)
            => !(RequestStatus1 < RequestStatus2);

        #endregion

        #endregion

        #region IComparable<RequestStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is RequestStatus))
                throw new ArgumentException("The given object is not a request status!",
                                            nameof(Object));

            return CompareTo((RequestStatus) Object);

        }

        #endregion

        #region CompareTo(RequestStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus">An object to compare with.</param>
        public Int32 CompareTo(RequestStatus RequestStatus)
        {

            if ((Object) RequestStatus == null)
                throw new ArgumentNullException(nameof(RequestStatus),  "The given request status must not be null!");

            return InternalId.CompareTo(RequestStatus.InternalId);

        }

        #endregion

        #endregion

        #region IEquatable<RequestStatus> Members

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

            if (!(Object is RequestStatus))
                return false;

            return Equals((RequestStatus) Object);

        }

        #endregion

        #region Equals(RequestStatus)

        /// <summary>
        /// Compares two RequestStatuss for equality.
        /// </summary>
        /// <param name="RequestStatus">A request status to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RequestStatus RequestStatus)
        {

            if ((Object) RequestStatus == null)
                return false;

            return InternalId.Equals(RequestStatus.InternalId);

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId.ToString();

        #endregion

    }

}
