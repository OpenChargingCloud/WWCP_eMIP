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

using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// Session event natures.
    /// </summary>
    public readonly struct SessionEventNatures : IId,
                                                 IEquatable <SessionEventNatures>,
                                                 IComparable<SessionEventNatures>
    {

        #region Data

        private static readonly ConcurrentDictionary<Int32, SessionEventNatures> Lookup = new ();

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => false;

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => false;

        /// <summary>
        /// The length of the tag identification.
        /// </summary>
        public UInt64 Length
            => 0;

        /// <summary>
        /// The internal identification.
        /// </summary>
        public Int32    Code           { get; }

        /// <summary>
        /// The description of the session event nature.
        /// </summary>
        public String?  Description    { get; }

        #endregion

        #region Constructor(s)

        #region (static)  SessionEventNatures() <- Does reflection!

        static SessionEventNatures()
        {

            foreach (var methodInfo in typeof(SessionEventNatures).GetMethods())
            {
                if (methodInfo.IsStatic &&
                    methodInfo.GetParameters().Length == 0)
                {

                    var sessionEventNatureObject = methodInfo.Invoke(Activator.CreateInstance(typeof(SessionEventNatures)), null);

                    if (sessionEventNatureObject is SessionEventNatures sessionEventNature &&
                        !Lookup.ContainsKey(sessionEventNature.Code))
                    {
                        Lookup.TryAdd(sessionEventNature.Code, sessionEventNature);
                    }

                }
            }

        }

        #endregion

        #region (private) SessionEventNatures(Code, Description = null)

        /// <summary>
        /// Create a new session event nature.
        /// </summary>
        /// <param name="Code">The numeric code of the status.</param>
        /// <param name="Description">The description of the session event nature.</param>
        private SessionEventNatures(Int32    Code,
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
        /// Parse the given string as a session event nature.
        /// </summary>
        /// <param name="Code">The numeric code of the session event nature.</param>
        /// <param name="Description">The description of the session event nature.</param>
        public static SessionEventNatures Register(Int32    Code,
                                                   String?  Description   = null)

            => new (Code,
                    Description);

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a session event nature.
        /// </summary>
        /// <param name="Text">A text representation of a session event nature.</param>
        public static SessionEventNatures Parse(String Text)
        {

            #region Initial checks

            Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a session event nature must not be null or empty!");

            #endregion

            if (Int32.TryParse(Text, out var number))
                return Parse(number);

            throw new ArgumentException($"Invalid text representation of a session event nature: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse   (Code)

        /// <summary>
        /// Parse the given number as a session event nature.
        /// </summary>
        /// <param name="Code">A numeric representation of a session event nature.</param>
        public static SessionEventNatures Parse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out var sessionEventNature))
                return sessionEventNature;

            return new SessionEventNatures(Code);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a session event nature.
        /// </summary>
        /// <param name="Text">A text representation of a session event nature.</param>
        public static SessionEventNatures? TryParse(String Text)
        {

            #region Initial checks

            Text = Text.Trim();

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out var number))
                return null;

            #endregion

            return TryParse(number);

        }

        #endregion

        #region TryParse(Code)

        /// <summary>
        /// Try to parse the given number as a session event nature.
        /// </summary>
        /// <param name="Code">A numeric representation of a session event nature.</param>
        public static SessionEventNatures? TryParse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out var sessionEventNature))
                return sessionEventNature;

            return null;

        }

        #endregion

        #region TryParse(Text, out SessionEventNature)

        /// <summary>
        /// Try to parse the given string as a session event nature.
        /// </summary>
        /// <param name="Text">A text representation of a session event nature.</param>
        /// <param name="SessionEventNature">The parsed session event nature.</param>
        public static Boolean TryParse(String Text, out SessionEventNatures SessionEventNature)
        {

            #region Initial checks

            Text = Text.Trim();

            #endregion

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out var number))
            {
                SessionEventNature = default;
                return false;
            }

            SessionEventNature = new SessionEventNatures(number);
            return true;

        }

        #endregion

        #region TryParse(Code, out SessionEventNature)

        /// <summary>
        /// Try to parse the given number as a session event nature.
        /// </summary>
        /// <param name="Code">A numeric representation of a session event nature.</param>
        /// <param name="SessionEventNatures">The parsed session event nature.</param>
        public static Boolean TryParse(Int32 Code, out SessionEventNatures SessionEventNature)
        {

            if (Lookup.TryGetValue(Code, out SessionEventNature))
                return true;

            SessionEventNature = new SessionEventNatures(Code);
            return true;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this session event nature.
        /// </summary>
        public SessionEventNatures Clone()

            => new(
                   Code,
                   Description is not null && Description.IsNotNullOrEmpty()
                       ? Description.CloneString()
                       : null
               );

        #endregion


        #region Static definitions

        // Source: GIREVE Gireve_Tech_eMIP-V0.7.4_ProtocolDescription_1.0.6-en.pdf
        // All those will be reflected and added to the lookup within the static constructor!

        /// <summary>
        /// Emergency Stop.
        /// </summary>
        public static SessionEventNatures EmergencyStop
            => new (0, "Emergency Stop");

        /// <summary>
        /// Operation terminated.
        /// </summary>
        public static SessionEventNatures Terminated
            => new (1, "Operation terminated");

        /// <summary>
        /// Operation suspendedn.
        /// </summary>
        public static SessionEventNatures Suspended
            => new (2, "Operation suspended");

        /// <summary>
        /// Operation started.
        /// </summary>
        public static SessionEventNatures Started
            => new (3, "Operation started");

        #endregion


        #region Operator overloading

        #region Operator == (SessionEventNature1, SessionEventNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNature1">A session event nature.</param>
        /// <param name="SessionEventNature2">Another session event nature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SessionEventNatures SessionEventNature1,
                                           SessionEventNatures SessionEventNature2)

            => SessionEventNature1.Equals(SessionEventNature2);

        #endregion

        #region Operator != (SessionEventNature1, SessionEventNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNature1">A session event nature.</param>
        /// <param name="SessionEventNature2">Another session event nature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SessionEventNatures SessionEventNature1,
                                           SessionEventNatures SessionEventNature2)

            => !SessionEventNature1.Equals(SessionEventNature2);

        #endregion

        #region Operator <  (SessionEventNature1, SessionEventNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNature1">A session event nature.</param>
        /// <param name="SessionEventNature2">Another session event nature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SessionEventNatures SessionEventNature1,
                                          SessionEventNatures SessionEventNature2)

            => SessionEventNature1.CompareTo(SessionEventNature2) < 0;

        #endregion

        #region Operator <= (SessionEventNature1, SessionEventNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNature1">A session event nature.</param>
        /// <param name="SessionEventNature2">Another session event nature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SessionEventNatures SessionEventNature1,
                                           SessionEventNatures SessionEventNature2)

            => SessionEventNature1.CompareTo(SessionEventNature2) <= 0;

        #endregion

        #region Operator >  (SessionEventNature1, SessionEventNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNature1">A session event nature.</param>
        /// <param name="SessionEventNature2">Another session event nature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SessionEventNatures SessionEventNature1,
                                          SessionEventNatures SessionEventNature2)

            => SessionEventNature1.CompareTo(SessionEventNature2) > 0;

        #endregion

        #region Operator >= (SessionEventNature1, SessionEventNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionEventNature1">A session event nature.</param>
        /// <param name="SessionEventNature2">Another session event nature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SessionEventNatures SessionEventNature1,
                                           SessionEventNatures SessionEventNature2)

            => SessionEventNature1.CompareTo(SessionEventNature2) >= 0;

        #endregion

        #endregion

        #region IComparable<SessionEventNatures> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two session event natures.
        /// </summary>
        /// <param name="Object">A session event nature to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is SessionEventNatures sessionEventNature
                   ? CompareTo(sessionEventNature)
                   : throw new ArgumentException("The given object is not a session event nature!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(SessionEventNatures)

        /// <summary>
        /// Compares two session event natures.
        /// </summary>
        /// <param name="SessionEventNatures">A session event nature to compare with.</param>
        public Int32 CompareTo(SessionEventNatures SessionEventNatures)

            => Code.CompareTo(SessionEventNatures.Code);

        #endregion

        #endregion

        #region IEquatable<SessionEventNatures> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two session event natures for equality.
        /// </summary>
        /// <param name="Object">A session event nature to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SessionEventNatures sessionEventNature &&
                   Equals(sessionEventNature);

        #endregion

        #region Equals(SessionEventNatures)

        /// <summary>
        /// Compares two session event natures for equality.
        /// </summary>
        /// <param name="SessionEventNatures">A session event nature to compare with.</param>
        public Boolean Equals(SessionEventNatures SessionEventNatures)

            => Code.Equals(SessionEventNatures.Code);

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
