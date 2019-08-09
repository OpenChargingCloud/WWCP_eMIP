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
    /// The unique identification of a contract.
    /// </summary>
    public struct Contract_Id : IId,
                                IEquatable <Contract_Id>,
                                IComparable<Contract_Id>

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
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// The length of the contract identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new contract identification.
        /// based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of a contract.</param>
        private Contract_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a contract identification.
        /// </summary>
        /// <param name="Text">A text representation of a contract identification.</param>
        public static Contract_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a contract identification must not be null or empty!");

            #endregion

            return new Contract_Id(Text);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a contract identification.
        /// </summary>
        /// <param name="Text">A text representation of a contract identification.</param>
        public static Contract_Id? TryParse(String Text)
        {

            if (Text != null)
                Text = Text.Trim();

            return Text.IsNullOrEmpty()
                       ? new Contract_Id?()
                       : new Contract_Id(Text);

        }

        #endregion

        #region TryParse(Text, out ContractId)

        /// <summary>
        /// Try to parse the given string as a contract identification.
        /// </summary>
        /// <param name="Text">A text representation of a contract identification.</param>
        /// <param name="ContractId">The parsed contract identification.</param>
        public static Boolean TryParse(String Text, out Contract_Id ContractId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                ContractId = default(Contract_Id);
                return false;
            }

            #endregion

            try
            {

                ContractId = new Contract_Id(Text);

                return true;

            }
            catch (Exception)
            { }

            ContractId = default(Contract_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this contract identification.
        /// </summary>
        public Contract_Id Clone

            => new Contract_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        public static Contract_Id Random(Byte Length = 20)
            => new Contract_Id(_Random.RandomString(Length));


        #region Operator overloading

        #region Operator == (ContractId1, ContractId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId1">A contract identification.</param>
        /// <param name="ContractId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Contract_Id ContractId1, Contract_Id ContractId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ContractId1, ContractId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ContractId1 == null) || ((Object) ContractId2 == null))
                return false;

            return ContractId1.Equals(ContractId2);

        }

        #endregion

        #region Operator != (ContractId1, ContractId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId1">A contract identification.</param>
        /// <param name="ContractId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Contract_Id ContractId1, Contract_Id ContractId2)
            => !(ContractId1 == ContractId2);

        #endregion

        #region Operator <  (ContractId1, ContractId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId1">A contract identification.</param>
        /// <param name="ContractId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Contract_Id ContractId1, Contract_Id ContractId2)
        {

            if ((Object) ContractId1 == null)
                throw new ArgumentNullException(nameof(ContractId1), "The given ContractId1 must not be null!");

            return ContractId1.CompareTo(ContractId2) < 0;

        }

        #endregion

        #region Operator <= (ContractId1, ContractId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId1">A contract identification.</param>
        /// <param name="ContractId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Contract_Id ContractId1, Contract_Id ContractId2)
            => !(ContractId1 > ContractId2);

        #endregion

        #region Operator >  (ContractId1, ContractId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId1">A contract identification.</param>
        /// <param name="ContractId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Contract_Id ContractId1, Contract_Id ContractId2)
        {

            if ((Object) ContractId1 == null)
                throw new ArgumentNullException(nameof(ContractId1), "The given ContractId1 must not be null!");

            return ContractId1.CompareTo(ContractId2) > 0;

        }

        #endregion

        #region Operator >= (ContractId1, ContractId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId1">A contract identification.</param>
        /// <param name="ContractId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Contract_Id ContractId1, Contract_Id ContractId2)
            => !(ContractId1 < ContractId2);

        #endregion

        #endregion

        #region IComparable<ContractId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Contract_Id ContractId))
                throw new ArgumentException("The given object is not a contract identification!",
                                            nameof(Object));

            return CompareTo(ContractId);

        }

        #endregion

        #region CompareTo(ContractId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId">An object to compare with.</param>
        public Int32 CompareTo(Contract_Id ContractId)
        {

            if ((Object) ContractId == null)
                throw new ArgumentNullException(nameof(ContractId),  "The given contract identification must not be null!");

            return String.Compare(InternalId, ContractId.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<ContractId> Members

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

            if (!(Object is Contract_Id ContractId))
                return false;

            return Equals(ContractId);

        }

        #endregion

        #region Equals(ContractId)

        /// <summary>
        /// Compares two ContractIds for equality.
        /// </summary>
        /// <param name="ContractId">A contract identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Contract_Id ContractId)
        {

            if ((Object) ContractId == null)
                return false;

            return InternalId.ToLower().Equals(ContractId.InternalId.ToLower());

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
