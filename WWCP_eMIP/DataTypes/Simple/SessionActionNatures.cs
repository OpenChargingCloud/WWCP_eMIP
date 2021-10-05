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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// Session action natures.
    /// </summary>
    public struct SessionActionNatures : IId,
                                         IEquatable <SessionActionNatures>,
                                         IComparable<SessionActionNatures>
    {

        #region Data

        private static readonly Dictionary<Int32, SessionActionNatures> Lookup = new Dictionary<Int32, SessionActionNatures>();

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

        #region (static)  SessionActionNatures() <- Does reflection!

        static SessionActionNatures()
        {

            SessionActionNatures sessionActionNature;

            foreach (var _MethodInfo in typeof(SessionActionNatures).GetMethods())
            {
                if (_MethodInfo.IsStatic &&
                    _MethodInfo.GetParameters().Length == 0)
                {

                    sessionActionNature = (SessionActionNatures) _MethodInfo.Invoke(Activator.CreateInstance(typeof(SessionActionNatures)), null);

                    if (!Lookup.ContainsKey(sessionActionNature.Code))
                        Lookup.Add(sessionActionNature.Code, sessionActionNature);

                }
            }

        }

        #endregion

        #region (private) SessionActionNatures(Code, Description = null)

        /// <summary>
        /// Create a new meter type.
        /// </summary>
        /// <param name="Code">The numeric code of the status.</param>
        /// <param name="Description">The description of the meter type.</param>
        private SessionActionNatures(Int32   Code,
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
        public static SessionActionNatures Register(Int32   Code,
                                          String  Description = null)

            => new SessionActionNatures(Code,
                              Description);

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a meter type.
        /// </summary>
        /// <param name="Text">A text representation of a meter type.</param>
        public static SessionActionNatures Parse(String Text)
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
        public static SessionActionNatures Parse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out SessionActionNatures Status))
                return Status;

            return new SessionActionNatures(Code);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a meter type.
        /// </summary>
        /// <param name="Text">A text representation of a meter type.</param>
        public static SessionActionNatures? TryParse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out Int32 Code))
                return new SessionActionNatures?();

            #endregion

            return TryParse(Code);

        }

        #endregion

        #region TryParse(Code)

        /// <summary>
        /// Try to parse the given number as a meter type.
        /// </summary>
        /// <param name="Code">A numeric representation of a meter type.</param>
        public static SessionActionNatures? TryParse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out SessionActionNatures Status))
                return Status;

            return new SessionActionNatures(Code);

        }

        #endregion

        #region TryParse(Text, out SessionActionNatures)

        /// <summary>
        /// Try to parse the given string as a meter type.
        /// </summary>
        /// <param name="Text">A text representation of a meter type.</param>
        /// <param name="SessionActionNatures">The parsed meter type.</param>
        public static Boolean TryParse(String Text, out SessionActionNatures SessionActionNatures)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            #endregion

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out Int32 Value))
            {
                SessionActionNatures = default(SessionActionNatures);
                return false;
            }

            SessionActionNatures = new SessionActionNatures(Value);
            return true;

        }

        #endregion

        #region TryParse(Code, out SessionActionNatures)

        /// <summary>
        /// Try to parse the given number as a meter type.
        /// </summary>
        /// <param name="Code">A numeric representation of a meter type.</param>
        /// <param name="SessionActionNatures">The parsed meter type.</param>
        public static Boolean TryParse(Int32 Code, out SessionActionNatures SessionActionNatures)
        {

            if (Lookup.TryGetValue(Code, out SessionActionNatures))
                return true;

            SessionActionNatures = new SessionActionNatures(Code);
            return true;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this meter type.
        /// </summary>
        public SessionActionNatures Clone

            => new SessionActionNatures(Code,
                              new String(Description.ToCharArray()));

        #endregion


        #region Static definitions

        // Source: GIREVE Gireve_Tech_eMIP-V0.7.4_ProtocolDescription_1.0.6-en.pdf
        // All those will be reflected and added to the lookup within the static constructor!

        /// <summary>
        /// Emergency Stop.
        /// </summary>
        public static SessionActionNatures EmergencyStop
            => new SessionActionNatures(0, "Emergency Stop");

        /// <summary>
        /// Stop and terminate current operation.
        /// </summary>
        public static SessionActionNatures Stop
            => new SessionActionNatures(1, "Stop and terminate current operation");

        /// <summary>
        /// Suspend current operation.
        /// </summary>
        public static SessionActionNatures Suspend
            => new SessionActionNatures(2, "Suspend current operation");

        /// <summary>
        /// Restart current operation.
        /// </summary>
        public static SessionActionNatures Restart
            => new SessionActionNatures(3, "Restart current operation");

        #endregion


        #region Operator overloading

        #region Operator == (SessionActionNature1, SessionActionNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNature1">A meter type.</param>
        /// <param name="SessionActionNature2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SessionActionNatures SessionActionNature1, SessionActionNatures SessionActionNature2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SessionActionNature1, SessionActionNature2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SessionActionNature1 == null) || ((Object) SessionActionNature2 == null))
                return false;

            return SessionActionNature1.Equals(SessionActionNature2);

        }

        #endregion

        #region Operator != (SessionActionNature1, SessionActionNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNature1">A meter type.</param>
        /// <param name="SessionActionNature2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SessionActionNatures SessionActionNature1, SessionActionNatures SessionActionNature2)
            => !(SessionActionNature1 == SessionActionNature2);

        #endregion

        #region Operator <  (SessionActionNature1, SessionActionNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNature1">A meter type.</param>
        /// <param name="SessionActionNature2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SessionActionNatures SessionActionNature1, SessionActionNatures SessionActionNature2)
        {

            if ((Object) SessionActionNature1 == null)
                throw new ArgumentNullException(nameof(SessionActionNature1), "The given SessionActionNature1 must not be null!");

            return SessionActionNature1.CompareTo(SessionActionNature2) < 0;

        }

        #endregion

        #region Operator <= (SessionActionNature1, SessionActionNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNature1">A meter type.</param>
        /// <param name="SessionActionNature2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SessionActionNatures SessionActionNature1, SessionActionNatures SessionActionNature2)
            => !(SessionActionNature1 > SessionActionNature2);

        #endregion

        #region Operator >  (SessionActionNature1, SessionActionNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNature1">A meter type.</param>
        /// <param name="SessionActionNature2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SessionActionNatures SessionActionNature1, SessionActionNatures SessionActionNature2)
        {

            if ((Object) SessionActionNature1 == null)
                throw new ArgumentNullException(nameof(SessionActionNature1), "The given SessionActionNature1 must not be null!");

            return SessionActionNature1.CompareTo(SessionActionNature2) > 0;

        }

        #endregion

        #region Operator >= (SessionActionNature1, SessionActionNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNature1">A meter type.</param>
        /// <param name="SessionActionNature2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SessionActionNatures SessionActionNature1, SessionActionNatures SessionActionNature2)
            => !(SessionActionNature1 < SessionActionNature2);

        #endregion

        #endregion

        #region IComparable<SessionActionNatures> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is SessionActionNatures SessionActionNature))
                throw new ArgumentException("The given object is not a meter type!",
                                            nameof(Object));

            return CompareTo(SessionActionNature);

        }

        #endregion

        #region CompareTo(SessionActionNatures)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNatures">An object to compare with.</param>
        public Int32 CompareTo(SessionActionNatures SessionActionNatures)
        {

            if ((Object) SessionActionNatures == null)
                throw new ArgumentNullException(nameof(SessionActionNatures),  "The given meter type must not be null!");

            return Code.CompareTo(SessionActionNatures.Code);

        }

        #endregion

        #endregion

        #region IEquatable<SessionActionNatures> Members

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

            if (!(Object is SessionActionNatures SessionActionNature))
                return false;

            return Equals(SessionActionNature);

        }

        #endregion

        #region Equals(SessionActionNatures)

        /// <summary>
        /// Compares two SessionActionNaturess for equality.
        /// </summary>
        /// <param name="SessionActionNatures">A meter type to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SessionActionNatures SessionActionNatures)
        {

            if ((Object) SessionActionNatures == null)
                return false;

            return Code.Equals(SessionActionNatures.Code);

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
