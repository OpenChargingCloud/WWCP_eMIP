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
    /// Extension methods for booking identifications.
    /// </summary>
    public static class BookingIdExtensions
    {

        /// <summary>
        /// Indicates whether this booking identification is null or empty.
        /// </summary>
        /// <param name="BookingId">A booking identification.</param>
        public static Boolean IsNullOrEmpty(this Booking_Id? BookingId)
            => !BookingId.HasValue || BookingId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this booking identification is NOT null or empty.
        /// </summary>
        /// <param name="BookingId">A booking identification.</param>
        public static Boolean IsNotNullOrEmpty(this Booking_Id? BookingId)
            => BookingId.HasValue && BookingId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a booking.
    /// </summary>
    public readonly struct Booking_Id : IId,
                                        IEquatable <Booking_Id>,
                                        IComparable<Booking_Id>

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
        /// The length of the booking identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

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


        #region (static) Random(Length = 20)

        public static Booking_Id Random(Byte Length = 20)
            => new (RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Zero

        public static Booking_Id Zero
            => new ("0");

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a booking identification.
        /// </summary>
        /// <param name="Text">A text representation of a booking identification.</param>
        public static Booking_Id Parse(String Text)
        {

            if (TryParse(Text, out var bookingId))
                return bookingId;

            throw new ArgumentException($"Invalid text representation of a booking identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a booking identification.
        /// </summary>
        /// <param name="Text">A text representation of a booking identification.</param>
        public static Booking_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var bookingId))
                return bookingId;

            return null;

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
            catch
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

            => new (
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
        public static Boolean operator == (Booking_Id BookingId1,
                                           Booking_Id BookingId2)

            => BookingId1.Equals(BookingId2);

        #endregion

        #region Operator != (BookingId1, BookingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BookingId1">A booking identification.</param>
        /// <param name="BookingId2">Another booking identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Booking_Id BookingId1,
                                           Booking_Id BookingId2)

            => !BookingId1.Equals(BookingId2);

        #endregion

        #region Operator <  (BookingId1, BookingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BookingId1">A booking identification.</param>
        /// <param name="BookingId2">Another booking identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Booking_Id BookingId1,
                                          Booking_Id BookingId2)

            => BookingId1.CompareTo(BookingId2) < 0;

        #endregion

        #region Operator <= (BookingId1, BookingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BookingId1">A booking identification.</param>
        /// <param name="BookingId2">Another booking identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Booking_Id BookingId1,
                                           Booking_Id BookingId2)

            => BookingId1.CompareTo(BookingId2) <= 0;

        #endregion

        #region Operator >  (BookingId1, BookingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BookingId1">A booking identification.</param>
        /// <param name="BookingId2">Another booking identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Booking_Id BookingId1,
                                          Booking_Id BookingId2)

            => BookingId1.CompareTo(BookingId2) > 0;

        #endregion

        #region Operator >= (BookingId1, BookingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BookingId1">A booking identification.</param>
        /// <param name="BookingId2">Another booking identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Booking_Id BookingId1,
                                           Booking_Id BookingId2)

            => BookingId1.CompareTo(BookingId2) >= 0;

        #endregion

        #endregion

        #region IComparable<Booking_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two booking identifications.
        /// </summary>
        /// <param name="Object">A booking identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Booking_Id bookingId
                   ? CompareTo(bookingId)
                   : throw new ArgumentException("The given object is not a booking identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(BookingId)

        /// <summary>
        /// Compares two booking identifications.
        /// </summary>
        /// <param name="BookingId">A booking identification to compare with.</param>
        public Int32 CompareTo(Booking_Id BookingId)

            => String.Compare(InternalId,
                              BookingId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<Booking_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two booking identifications for equality.
        /// </summary>
        /// <param name="Object">A booking identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Booking_Id bookingId &&
                   Equals(bookingId);

        #endregion

        #region Equals(BookingId)

        /// <summary>
        /// Compares two booking identifications for equality.
        /// </summary>
        /// <param name="BookingId">A booking identification to compare with.</param>
        public Boolean Equals(Booking_Id BookingId)

            => String.Equals(InternalId,
                             BookingId.InternalId,
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
