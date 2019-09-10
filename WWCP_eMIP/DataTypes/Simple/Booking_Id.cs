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
    /// The unique identification of a booking.
    /// </summary>
    public struct Booking_Id : IId,
                               IEquatable <Booking_Id>,
                               IComparable<Booking_Id>

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
        /// The length of the booking identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new booking identification.
        /// based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of a booking identification.</param>
        private Booking_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a booking identification.
        /// </summary>
        /// <param name="Text">A text representation of a booking identification.</param>
        public static Booking_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a booking identification must not be null or empty!");

            #endregion

            if (TryParse(Text, out Booking_Id BookingId))
                return BookingId;

            throw new ArgumentNullException(nameof(Text), "The given text representation of a booking identification is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a booking identification.
        /// </summary>
        /// <param name="Text">A text representation of a booking identification.</param>
        public static Booking_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Booking_Id BookingId))
                return BookingId;

            return new Booking_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out BookingId)

        /// <summary>
        /// Try to parse the given string as a booking identification.
        /// </summary>
        /// <param name="Text">A text representation of a booking identification.</param>
        /// <param name="BookingId">The parsed booking identification.</param>
        public static Boolean TryParse(String Text, out Booking_Id BookingId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                BookingId = default;
                return false;
            }

            #endregion

            try
            {
                BookingId = new Booking_Id(Text);
                return true;
            }
            catch (Exception)
            { }

            BookingId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this booking identification.
        /// </summary>
        public Booking_Id Clone

            => new Booking_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (BookingId1, BookingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BookingId1">A booking identification.</param>
        /// <param name="BookingId2">Another booking identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Booking_Id BookingId1, Booking_Id BookingId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(BookingId1, BookingId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) BookingId1 == null) || ((Object) BookingId2 == null))
                return false;

            return BookingId1.Equals(BookingId2);

        }

        #endregion

        #region Operator != (BookingId1, BookingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BookingId1">A booking identification.</param>
        /// <param name="BookingId2">Another booking identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Booking_Id BookingId1, Booking_Id BookingId2)
            => !(BookingId1 == BookingId2);

        #endregion

        #region Operator <  (BookingId1, BookingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BookingId1">A booking identification.</param>
        /// <param name="BookingId2">Another booking identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Booking_Id BookingId1, Booking_Id BookingId2)
        {

            if ((Object) BookingId1 == null)
                throw new ArgumentNullException(nameof(BookingId1), "The given BookingId1 must not be null!");

            return BookingId1.CompareTo(BookingId2) < 0;

        }

        #endregion

        #region Operator <= (BookingId1, BookingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BookingId1">A booking identification.</param>
        /// <param name="BookingId2">Another booking identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Booking_Id BookingId1, Booking_Id BookingId2)
            => !(BookingId1 > BookingId2);

        #endregion

        #region Operator >  (BookingId1, BookingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BookingId1">A booking identification.</param>
        /// <param name="BookingId2">Another booking identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Booking_Id BookingId1, Booking_Id BookingId2)
        {

            if ((Object) BookingId1 == null)
                throw new ArgumentNullException(nameof(BookingId1), "The given BookingId1 must not be null!");

            return BookingId1.CompareTo(BookingId2) > 0;

        }

        #endregion

        #region Operator >= (BookingId1, BookingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BookingId1">A booking identification.</param>
        /// <param name="BookingId2">Another booking identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Booking_Id BookingId1, Booking_Id BookingId2)
            => !(BookingId1 < BookingId2);

        #endregion

        #endregion

        #region IComparable<BookingId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Booking_Id BookingId))
                throw new ArgumentException("The given object is not a booking identification!",
                                            nameof(Object));

            return CompareTo(BookingId);

        }

        #endregion

        #region CompareTo(BookingId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BookingId">An object to compare with.</param>
        public Int32 CompareTo(Booking_Id BookingId)
        {

            if ((Object) BookingId == null)
                throw new ArgumentNullException(nameof(BookingId),  "The given booking identification must not be null!");

            return String.Compare(InternalId, BookingId.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<BookingId> Members

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

            if (!(Object is Booking_Id BookingId))
                return false;

            return Equals(BookingId);

        }

        #endregion

        #region Equals(BookingId)

        /// <summary>
        /// Compares two BookingIds for equality.
        /// </summary>
        /// <param name="BookingId">A booking identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Booking_Id BookingId)
        {

            if ((Object) BookingId == null)
                return false;

            return InternalId.ToLower().Equals(BookingId.InternalId.ToLower());

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
