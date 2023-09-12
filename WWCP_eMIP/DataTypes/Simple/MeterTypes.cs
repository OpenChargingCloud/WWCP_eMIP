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

using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// Meter types.
    /// </summary>
    public readonly struct MeterTypes : IId,
                                        IEquatable <MeterTypes>,
                                        IComparable<MeterTypes>
    {

        #region Data

        private static readonly ConcurrentDictionary<Int32, MeterTypes> Lookup = new();

        #endregion

        #region Properties

        /// <summary>
        /// The internal identification.
        /// </summary>
        public Int32    Code          { get; }

        /// <summary>
        /// The description of the meter type.
        /// </summary>
        public String?  Description   { get; }

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean  IsNullOrEmpty
            => false;

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean  IsNotNullOrEmpty
            => false;

        /// <summary>
        /// The length of the tag identification.
        /// </summary>
        public UInt64   Length
            => 0;

        #endregion

        #region Constructor(s)

        #region (static)  MeterTypes() <- Does reflection!

        static MeterTypes()
        {

            foreach (var methodInfo in typeof(MeterTypes).GetMethods())
            {
                if (methodInfo.IsStatic &&
                    methodInfo.GetParameters().Length == 0)
                {

                    var meterTypeObject = methodInfo.Invoke(Activator.CreateInstance(typeof(MeterTypes)), null);

                    if (meterTypeObject is MeterTypes meterType &&
                        !Lookup.ContainsKey(meterType.Code))
                    {
                        Lookup.TryAdd(meterType.Code, meterType);
                    }

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
        private MeterTypes(Int32    Code,
                           String?  Description   = null)
        {

            this.Code         = Code;
            this.Description  = Description;

            if (!Lookup.ContainsKey(Code))
                 Lookup.TryAdd(Code, this);

        }

        #endregion

        #endregion


        #region Register(Code, Description = null)

        /// <summary>
        /// Parse the given string as a meter type.
        /// </summary>
        /// <param name="Code">The numeric code of the meter type.</param>
        /// <param name="Description">The description of the meter type.</param>
        public static MeterTypes Register(Int32    Code,
                                          String?  Description = null)

            => new (Code,
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

            Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a meter type must not be null or empty!");

            #endregion

            if (Int32.TryParse(Text, out var number))
                return Parse(number);

            throw new ArgumentException($"Invalid text representation of a meter type: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse   (Code)

        /// <summary>
        /// Parse the given number as a meter type.
        /// </summary>
        /// <param name="Code">A numeric representation of a meter type.</param>
        public static MeterTypes Parse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out var meterType))
                return meterType;

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

            Text = Text.Trim();

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out var number))
                return new MeterTypes?();

            #endregion

            return TryParse(number);

        }

        #endregion

        #region TryParse(Code)

        /// <summary>
        /// Try to parse the given number as a meter type.
        /// </summary>
        /// <param name="Code">A numeric representation of a meter type.</param>
        public static MeterTypes? TryParse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out var meterType))
                return meterType;

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

            Text = Text.Trim();

            #endregion

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out var number))
            {
                MeterTypes = default;
                return false;
            }

            MeterTypes = new MeterTypes(number);
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

            => new (Code,
                    Description is not null && Description.IsNotNullOrEmpty()
                        ? new String(Description.ToCharArray())
                        : null);

        #endregion


        #region Static definitions

        // Source: GIREVE eMIPv0.7.4-WebServices-Description V27-Diff
        // All those will be reflected and added to the lookup within the static constructor!

        /// <summary>
        /// Total duration (e.g. in minutes).
        /// </summary>
        public static MeterTypes TotalDuration
            => new (1,  "Total duration (e.g. in minutes)");

        /// <summary>
        /// Total energy (e.g. in Wh).
        /// </summary>
        public static MeterTypes TotalEnergy
            => new (2,  "Total energy (e.g. in Wh)");

        /// <summary>
        /// B2B Service Costs.
        /// </summary>
        public static MeterTypes SystemError
            => new (3,  "B2B Service Costs");

        #endregion


        #region Operator overloading

        #region Operator == (MeterType1, MeterType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterType1">A meter type.</param>
        /// <param name="MeterType2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (MeterTypes MeterType1,
                                           MeterTypes MeterType2)

            => MeterType1.Equals(MeterType2);

        #endregion

        #region Operator != (MeterType1, MeterType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterType1">A meter type.</param>
        /// <param name="MeterType2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (MeterTypes MeterType1,
                                           MeterTypes MeterType2)

            => !MeterType1.Equals(MeterType2);

        #endregion

        #region Operator <  (MeterType1, MeterType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterType1">A meter type.</param>
        /// <param name="MeterType2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (MeterTypes MeterType1,
                                          MeterTypes MeterType2)

            => MeterType1.CompareTo(MeterType2) < 0;

        #endregion

        #region Operator <= (MeterType1, MeterType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterType1">A meter type.</param>
        /// <param name="MeterType2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (MeterTypes MeterType1,
                                           MeterTypes MeterType2)

            => MeterType1.CompareTo(MeterType2) <= 0;

        #endregion

        #region Operator >  (MeterType1, MeterType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterType1">A meter type.</param>
        /// <param name="MeterType2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (MeterTypes MeterType1,
                                          MeterTypes MeterType2)

            => MeterType1.CompareTo(MeterType2) > 0;

        #endregion

        #region Operator >= (MeterType1, MeterType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterType1">A meter type.</param>
        /// <param name="MeterType2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (MeterTypes MeterType1,
                                           MeterTypes MeterType2)

            => MeterType1.CompareTo(MeterType2) >= 0;

        #endregion

        #endregion

        #region IComparable<MeterTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two meter types.
        /// </summary>
        /// <param name="Object">A meter type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is MeterTypes meterType
                   ? CompareTo(meterType)
                   : throw new ArgumentException("The given object is not a meter type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(MeterType)

        /// <summary>
        /// Compares two meter types.
        /// </summary>
        /// <param name="MeterType">A meter type to compare with.</param>
        public Int32 CompareTo(MeterTypes MeterType)

            => Code.CompareTo(MeterType.Code);

        #endregion

        #endregion

        #region IEquatable<MeterTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two meter types for equality.
        /// </summary>
        /// <param name="Object">A meter type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is MeterTypes meterType &&
                   Equals(meterType);

        #endregion

        #region Equals(MeterType)

        /// <summary>
        /// Compares two meter types for equality.
        /// </summary>
        /// <param name="MeterType">A meter type to compare with.</param>
        public Boolean Equals(MeterTypes MeterType)

            => Code.Equals(MeterType.Code);

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

            => $"{Code}{(Description.IsNotNullOrEmpty() ? ": " + Description : "")}";

        #endregion

    }

}
