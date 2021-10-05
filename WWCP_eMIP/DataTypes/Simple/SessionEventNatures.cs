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
    /// Session event natures.
    /// </summary>
    public struct SessionEventNatures : IId,
                                        IEquatable <SessionEventNatures>,
                                        IComparable<SessionEventNatures>
    {

        #region Data

        private static readonly Dictionary<Int32, SessionEventNatures> Lookup = new Dictionary<Int32, SessionEventNatures>();

        #endregion

        #region Properties

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

        /// <summary>
        /// The internal identification.
        /// </summary>
        public Int32   Code          { get; }

        /// <summary>
        /// The description of the meter type.
        /// </summary>
        public String  Description   { get; }

        #endregion

        #region Constructor(s)

        #region (static)  SessionEventNatures() <- Does reflection!

        static SessionEventNatures()
        {

            SessionEventNatures sessionEventNature;

            foreach (var _MethodInfo in typeof(SessionEventNatures).GetMethods())
            {
                if (_MethodInfo.IsStatic &&
                    _MethodInfo.GetParameters().Length == 0)
                {

                    sessionEventNature = (SessionEventNatures) _MethodInfo.Invoke(Activator.CreateInstance(typeof(SessionEventNatures)), null);

                    if (!Lookup.ContainsKey(sessionEventNature.Code))
                        Lookup.Add(sessionEventNature.Code, sessionEventNature);

                }
            }

        }

        #endregion

        #region (private) SessionEventNatures(Code, Description = null)

        /// <summary>
        /// Create a new meter type.
        /// </summary>
        /// <param name="Code">The numeric code of the status.</param>
        /// <param name="Description">The description of the meter type.</param>
        private SessionEventNatures(Int32   Code,
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
        public static SessionEventNatures Register(Int32   Code,
                                          String  Description = null)

            => new SessionEventNatures(Code,
                              Description);

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a meter type.
        /// </summary>
        /// <param name="Text">A text representation of a meter type.</param>
        public static SessionEventNatures Parse(String Text)
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
        public static SessionEventNatures Parse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out SessionEventNatures Status))
                return Status;

            return new SessionEventNatures(Code);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a meter type.
        /// </summary>
        /// <param name="Text">A text representation of a meter type.</param>
        public static SessionEventNatures? TryParse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out Int32 Code))
                return new SessionEventNatures?();

            #endregion

            return TryParse(Code);

        }

        #endregion

        #region TryParse(Code)

        /// <summary>
        /// Try to parse the given number as a meter type.
        /// </summary>
        /// <param name="Code">A numeric representation of a meter type.</param>
        public static SessionEventNatures? TryParse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out SessionEventNatures Status))
                return Status;

            return new SessionEventNatures(Code);

        }

        #endregion

        #region TryParse(Text, out SessionEventNatures)

        /// <summary>
        /// Try to parse the given string as a meter type.
        /// </summary>
        /// <param name="Text">A text representation of a meter type.</param>
        /// <param name="SessionEventNatures">The parsed meter type.</param>
        public static Boolean TryParse(String Text, out SessionEventNatures SessionEventNatures)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            #endregion

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out Int32 Value))
            {
                SessionEventNatures = default(SessionEventNatures);
                return false;
            }

            SessionEventNatures = new SessionEventNatures(Value);
            return true;

        }

        #endregion

        #region TryParse(Code, out SessionEventNatures)

        /// <summary>
        /// Try to parse the given number as a meter type.
        /// </summary>
        /// <param name="Code">A numeric representation of a meter type.</param>
        /// <param name="SessionEventNatures">The parsed meter type.</param>
        public static Boolean TryParse(Int32 Code, out SessionEventNatures SessionEventNatures)
        {

            if (Lookup.TryGetValue(Code, out SessionEventNatures))
                return true;

            SessionEventNatures = new SessionEventNatures(Code);
            return true;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this meter type.
        /// </summary>
        public SessionEventNatures Clone

            => new SessionEventNatures(Code,
                              new String(Description.ToCharArray()));

        #endregion


        #region Static definitions

        // Source: GIREVE Gireve_Tech_eMIP-V0.7.4_ProtocolDescription_1.0.6-en.pdf
        // All those will be reflected and added to the lookup within the static constructor!

        /// <summary>
        /// Emergency Stop.
        /// </summary>
        public static SessionEventNatures EmergencyStop
            => new SessionEventNatures(0, "Emergency Stop");

        /// <summary>
        /// Operation terminated.
        /// </summary>
        public static SessionEventNatures Terminated
            => new SessionEventNatures(1, "Operation terminated");

        /// <summary>
        /// Operation suspendedn.
        /// </summary>
        public static SessionEventNatures Suspended
            => new SessionEventNatures(2, "Operation suspended");

        /// <summary>
        /// Operation started.
        /// </summary>
        public static SessionEventNatures Started
            => new SessionEventNatures(3, "Operation started");

        #endregion


        #region Operator overloading

        #region Operator == (SessionEventNature1, SessionEventNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNature1">A meter type.</param>
        /// <param name="SessionEventNature2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SessionEventNatures SessionEventNature1, SessionEventNatures SessionEventNature2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SessionEventNature1, SessionEventNature2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SessionEventNature1 == null) || ((Object) SessionEventNature2 == null))
                return false;

            return SessionEventNature1.Equals(SessionEventNature2);

        }

        #endregion

        #region Operator != (SessionEventNature1, SessionEventNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNature1">A meter type.</param>
        /// <param name="SessionEventNature2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SessionEventNatures SessionEventNature1, SessionEventNatures SessionEventNature2)
            => !(SessionEventNature1 == SessionEventNature2);

        #endregion

        #region Operator <  (SessionEventNature1, SessionEventNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNature1">A meter type.</param>
        /// <param name="SessionEventNature2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SessionEventNatures SessionEventNature1, SessionEventNatures SessionEventNature2)
        {

            if ((Object) SessionEventNature1 == null)
                throw new ArgumentNullException(nameof(SessionEventNature1), "The given SessionEventNature1 must not be null!");

            return SessionEventNature1.CompareTo(SessionEventNature2) < 0;

        }

        #endregion

        #region Operator <= (SessionEventNature1, SessionEventNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNature1">A meter type.</param>
        /// <param name="SessionEventNature2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SessionEventNatures SessionEventNature1, SessionEventNatures SessionEventNature2)
            => !(SessionEventNature1 > SessionEventNature2);

        #endregion

        #region Operator >  (SessionEventNature1, SessionEventNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNature1">A meter type.</param>
        /// <param name="SessionEventNature2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SessionEventNatures SessionEventNature1, SessionEventNatures SessionEventNature2)
        {

            if ((Object) SessionEventNature1 == null)
                throw new ArgumentNullException(nameof(SessionEventNature1), "The given SessionEventNature1 must not be null!");

            return SessionEventNature1.CompareTo(SessionEventNature2) > 0;

        }

        #endregion

        #region Operator >= (SessionEventNature1, SessionEventNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNature1">A meter type.</param>
        /// <param name="SessionEventNature2">Another meter type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SessionEventNatures SessionEventNature1, SessionEventNatures SessionEventNature2)
            => !(SessionEventNature1 < SessionEventNature2);

        #endregion

        #endregion

        #region IComparable<SessionEventNatures> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is SessionEventNatures SessionEventNature))
                throw new ArgumentException("The given object is not a meter type!",
                                            nameof(Object));

            return CompareTo(SessionEventNature);

        }

        #endregion

        #region CompareTo(SessionEventNatures)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNatures">An object to compare with.</param>
        public Int32 CompareTo(SessionEventNatures SessionEventNatures)
        {

            if ((Object) SessionEventNatures == null)
                throw new ArgumentNullException(nameof(SessionEventNatures),  "The given meter type must not be null!");

            return Code.CompareTo(SessionEventNatures.Code);

        }

        #endregion

        #endregion

        #region IEquatable<SessionEventNatures> Members

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

            if (!(Object is SessionEventNatures SessionEventNature))
                return false;

            return Equals(SessionEventNature);

        }

        #endregion

        #region Equals(SessionEventNatures)

        /// <summary>
        /// Compares two SessionEventNaturess for equality.
        /// </summary>
        /// <param name="SessionEventNatures">A meter type to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SessionEventNatures SessionEventNatures)
        {

            if ((Object) SessionEventNatures == null)
                return false;

            return Code.Equals(SessionEventNatures.Code);

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
