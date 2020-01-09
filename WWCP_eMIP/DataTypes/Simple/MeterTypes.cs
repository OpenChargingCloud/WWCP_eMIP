/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// Meter types.
    /// </summary>
    public struct MeterTypes : IId,
                               IEquatable <MeterTypes>,
                               IComparable<MeterTypes>
    {

        #region Data

        private static readonly Dictionary<Int32, MeterTypes> Lookup = new Dictionary<Int32, MeterTypes>();

        #endregion

        #region Properties

        /// <summary>
        /// The internal identification.
        /// </summary>
        public Int32   Code          { get; }

        /// <summary>
        /// The description of the meter type.
        /// </summary>
        public String  Description   { get; }

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => false;

        /// <summary>
        /// The length of the tag identification.
        /// </summary>
        public UInt64 Length
            => 0;

        #endregion

        #region Constructor(s)

        #region (static)  MeterTypes() <- Does reflection!

        static MeterTypes()
        {

            MeterTypes meterType;

            foreach (var _MethodInfo in typeof(MeterTypes).GetMethods())
            {
                if (_MethodInfo.IsStatic &&
                    _MethodInfo.GetParameters().Length == 0)
                {

                    meterType = (MeterTypes) _MethodInfo.Invoke(Activator.CreateInstance(typeof(MeterTypes)), null);

                    if (!Lookup.ContainsKey(meterType.Code))
                        Lookup.Add(meterType.Code, meterType);

                }
            }

        }

        #endregion

        #region (private) MeterTypes(Code, Description = null)

        /// <summary>
        /// Create a new meter type.
        /// </summary>
        /// <param name="Code">The numeric code of the status.</param>
        /// <param name="Description">The description of the meter type.</param>
        private MeterTypes(Int32   Code,
                           String  Description = null)
        {

            this.Code         = Code;
            this.Description  = Description;

            lock (Lookup)
            {
                if (!Lookup.ContainsKey(Code))
                    Lookup.Add(Code, this);
            }

        }

        #endregion

        #endregion


        #region Register(Code, Description = null)

        /// <summary>
        /// Parse the given string as a meter type.
        /// </summary>
        /// <param name="Code">The numeric code of the meter type.</param>
        /// <param name="Description">The description of the meter type.</param>
        public static MeterTypes Register(Int32   Code,
                                          String  Description = null)

            => new MeterTypes(Code,
                              Description);

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a meter type.
        /// </summary>
        /// <param name="Text">A text representation of a meter type.</param>
        public static MeterTypes Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a meter type must not be null or empty!");

            #endregion

            return Parse(Int32.Parse(Text));

        }

        #endregion

        #region Parse   (Code)

        /// <summary>
        /// Parse the given number as a meter type.
        /// </summary>
        /// <param name="Code">A numeric representation of a meter type.</param>
        public static MeterTypes Parse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out MeterTypes Status))
                return Status;

            return new MeterTypes(Code);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a meter type.
        /// </summary>
        /// <param name="Text">A text representation of a meter type.</param>
        public static MeterTypes? TryParse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out Int32 Code))
                return new MeterTypes?();

            #endregion

            return TryParse(Code);

        }

        #endregion

        #region TryParse(Code)

        /// <summary>
        /// Try to parse the given number as a meter type.
        /// </summary>
        /// <param name="Code">A numeric representation of a meter type.</param>
        public static MeterTypes? TryParse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out MeterTypes Status))
                return Status;

            return new MeterTypes(Code);

        }

        #endregion

        #region TryParse(Text, out MeterTypes)

        /// <summary>
        /// Try to parse the given string as a meter type.
        /// </summary>
        /// <param name="Text">A text representation of a meter type.</param>
        /// <param name="MeterTypes">The parsed meter type.</param>
        public static Boolean TryParse(String Text, out MeterTypes MeterTypes)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            #endregion

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out Int32 Value))
            {
                MeterTypes = default(MeterTypes);
                return false;
            }

            MeterTypes = new MeterTypes(Value);
            return true;

        }

        #endregion

        #region TryParse(Code, out MeterTypes)

        /// <summary>
        /// Try to parse the given number as a meter type.
        /// </summary>
        /// <param name="Code">A numeric representation of a meter type.</param>
        /// <param name="MeterTypes">The parsed meter type.</param>
        public static Boolean TryParse(Int32 Code, out MeterTypes MeterTypes)
        {

            if (Lookup.TryGetValue(Code, out MeterTypes))
                return true;

            MeterTypes = new MeterTypes(Code);
            return true;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this meter type.
        /// </summary>
        public MeterTypes Clone

            => new MeterTypes(Code,
                              new String(Description.ToCharArray()));

        #endregion


        #region Static definitions

        // Source: GIREVE eMIPv0.7.4-WebServices-Description V27-Diff
        // All those will be reflected and added to the lookup within the static constructor!

        /// <summary>
        /// Total duration (e.g. in minutes).
        /// </summary>
        public static MeterTypes TotalDuration
            => new MeterTypes(1,  "Total duration (e.g. in minutes)");

        /// <summary>
        /// Total energy (e.g. in Wh).
        /// </summary>
        public static MeterTypes TotalEnergy
            => new MeterTypes(2,  "Total energy (e.g. in Wh)");

        /// <summary>
        /// B2B Service Costs.
        /// </summary>
        public static MeterTypes SystemError
            => new MeterTypes(3,  "B2B Service Costs");

        #endregion


        #region Operator overloading

        #region Operator == (MeterType1, MeterType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterType1">A meter type.</param>
        /// <param name="MeterType2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (MeterTypes MeterType1, MeterTypes MeterType2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(MeterType1, MeterType2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) MeterType1 == null) || ((Object) MeterType2 == null))
                return false;

            return MeterType1.Equals(MeterType2);

        }

        #endregion

        #region Operator != (MeterType1, MeterType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterType1">A meter type.</param>
        /// <param name="MeterType2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (MeterTypes MeterType1, MeterTypes MeterType2)
            => !(MeterType1 == MeterType2);

        #endregion

        #region Operator <  (MeterType1, MeterType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterType1">A meter type.</param>
        /// <param name="MeterType2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (MeterTypes MeterType1, MeterTypes MeterType2)
        {

            if ((Object) MeterType1 == null)
                throw new ArgumentNullException(nameof(MeterType1), "The given MeterType1 must not be null!");

            return MeterType1.CompareTo(MeterType2) < 0;

        }

        #endregion

        #region Operator <= (MeterType1, MeterType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterType1">A meter type.</param>
        /// <param name="MeterType2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (MeterTypes MeterType1, MeterTypes MeterType2)
            => !(MeterType1 > MeterType2);

        #endregion

        #region Operator >  (MeterType1, MeterType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterType1">A meter type.</param>
        /// <param name="MeterType2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (MeterTypes MeterType1, MeterTypes MeterType2)
        {

            if ((Object) MeterType1 == null)
                throw new ArgumentNullException(nameof(MeterType1), "The given MeterType1 must not be null!");

            return MeterType1.CompareTo(MeterType2) > 0;

        }

        #endregion

        #region Operator >= (MeterType1, MeterType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterType1">A meter type.</param>
        /// <param name="MeterType2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (MeterTypes MeterType1, MeterTypes MeterType2)
            => !(MeterType1 < MeterType2);

        #endregion

        #endregion

        #region IComparable<MeterTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is MeterTypes))
                throw new ArgumentException("The given object is not a meter type!",
                                            nameof(Object));

            return CompareTo((MeterTypes) Object);

        }

        #endregion

        #region CompareTo(MeterTypes)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterTypes">An object to compare with.</param>
        public Int32 CompareTo(MeterTypes MeterTypes)
        {

            if ((Object) MeterTypes == null)
                throw new ArgumentNullException(nameof(MeterTypes),  "The given meter type must not be null!");

            return Code.CompareTo(MeterTypes.Code);

        }

        #endregion

        #endregion

        #region IEquatable<MeterTypes> Members

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

            if (!(Object is MeterTypes))
                return false;

            return Equals((MeterTypes) Object);

        }

        #endregion

        #region Equals(MeterTypes)

        /// <summary>
        /// Compares two MeterTypess for equality.
        /// </summary>
        /// <param name="MeterTypes">A meter type to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(MeterTypes MeterTypes)
        {

            if ((Object) MeterTypes == null)
                return false;

            return Code.Equals(MeterTypes.Code);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => Code.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Code,
                             Description.IsNotNullOrEmpty() ? ": " + Description : "");

        #endregion

    }

}
