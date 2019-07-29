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
    /// The unique identification of a transaction.
    /// </summary>
    public struct Transaction_Id : IId,
                                   IEquatable <Transaction_Id>,
                                   IComparable<Transaction_Id>

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
        /// The length of the transaction identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new transaction identification.
        /// based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of a transaction.</param>
        private Transaction_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a transaction identification.
        /// </summary>
        /// <param name="Text">A text representation of a transaction identification.</param>
        public static Transaction_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a transaction identification must not be null or empty!");

            #endregion

            return new Transaction_Id(Text);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a transaction identification.
        /// </summary>
        /// <param name="Text">A text representation of a transaction identification.</param>
        public static Transaction_Id? TryParse(String Text)
        {

            if (Text != null)
                Text = Text.Trim();

            return Text.IsNullOrEmpty()
                       ? new Transaction_Id?()
                       : new Transaction_Id(Text);

        }

        #endregion

        #region TryParse(Text, out TransactionId)

        /// <summary>
        /// Try to parse the given string as a transaction identification.
        /// </summary>
        /// <param name="Text">A text representation of a transaction identification.</param>
        /// <param name="TransactionId">The parsed transaction identification.</param>
        public static Boolean TryParse(String Text, out Transaction_Id TransactionId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                TransactionId = default(Transaction_Id);
                return false;
            }

            #endregion

            try
            {

                TransactionId = new Transaction_Id(Text);

                return true;

            }
            catch (Exception)
            { }

            TransactionId = default(Transaction_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this transaction identification.
        /// </summary>
        public Transaction_Id Clone

            => new Transaction_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        public static Transaction_Id Random(Byte Length = 20)
            => new Transaction_Id(_Random.RandomString(Length));

        public static Transaction_Id Zero
            => new Transaction_Id("0");


        #region Operator overloading

        #region Provider == (TransactionId1, TransactionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TransactionId1">A transaction identification.</param>
        /// <param name="TransactionId2">Another transaction identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Transaction_Id TransactionId1, Transaction_Id TransactionId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(TransactionId1, TransactionId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) TransactionId1 == null) || ((Object) TransactionId2 == null))
                return false;

            return TransactionId1.Equals(TransactionId2);

        }

        #endregion

        #region Provider != (TransactionId1, TransactionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TransactionId1">A transaction identification.</param>
        /// <param name="TransactionId2">Another transaction identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Transaction_Id TransactionId1, Transaction_Id TransactionId2)
            => !(TransactionId1 == TransactionId2);

        #endregion

        #region Provider <  (TransactionId1, TransactionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TransactionId1">A transaction identification.</param>
        /// <param name="TransactionId2">Another transaction identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Transaction_Id TransactionId1, Transaction_Id TransactionId2)
        {

            if ((Object) TransactionId1 == null)
                throw new ArgumentNullException(nameof(TransactionId1), "The given TransactionId1 must not be null!");

            return TransactionId1.CompareTo(TransactionId2) < 0;

        }

        #endregion

        #region Provider <= (TransactionId1, TransactionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TransactionId1">A transaction identification.</param>
        /// <param name="TransactionId2">Another transaction identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Transaction_Id TransactionId1, Transaction_Id TransactionId2)
            => !(TransactionId1 > TransactionId2);

        #endregion

        #region Provider >  (TransactionId1, TransactionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TransactionId1">A transaction identification.</param>
        /// <param name="TransactionId2">Another transaction identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Transaction_Id TransactionId1, Transaction_Id TransactionId2)
        {

            if ((Object) TransactionId1 == null)
                throw new ArgumentNullException(nameof(TransactionId1), "The given TransactionId1 must not be null!");

            return TransactionId1.CompareTo(TransactionId2) > 0;

        }

        #endregion

        #region Provider >= (TransactionId1, TransactionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TransactionId1">A transaction identification.</param>
        /// <param name="TransactionId2">Another transaction identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Transaction_Id TransactionId1, Transaction_Id TransactionId2)
            => !(TransactionId1 < TransactionId2);

        #endregion

        #endregion

        #region IComparable<TransactionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Transaction_Id TransactionId))
                throw new ArgumentException("The given object is not a transaction identification!",
                                            nameof(Object));

            return CompareTo(TransactionId);

        }

        #endregion

        #region CompareTo(TransactionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TransactionId">An object to compare with.</param>
        public Int32 CompareTo(Transaction_Id TransactionId)
        {

            if ((Object) TransactionId == null)
                throw new ArgumentNullException(nameof(TransactionId),  "The given transaction identification must not be null!");

            return String.Compare(InternalId, TransactionId.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<TransactionId> Members

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

            if (!(Object is Transaction_Id TransactionId))
                return false;

            return Equals(TransactionId);

        }

        #endregion

        #region Equals(TransactionId)

        /// <summary>
        /// Compares two TransactionIds for equality.
        /// </summary>
        /// <param name="TransactionId">A transaction identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Transaction_Id TransactionId)
        {

            if ((Object) TransactionId == null)
                return false;

            return InternalId.ToLower().Equals(TransactionId.InternalId.ToLower());

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
