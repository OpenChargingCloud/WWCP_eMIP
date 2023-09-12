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
    /// Session action natures.
    /// </summary>
    public readonly struct SessionActionNatures : IId,
                                                  IEquatable <SessionActionNatures>,
                                                  IComparable<SessionActionNatures>
    {

        #region Data

        private static readonly ConcurrentDictionary<Int32, SessionActionNatures> Lookup = new();

        #endregion

        #region Properties

        /// <summary>
        /// The internal identification.
        /// </summary>
        public Int32    Code          { get; }

        /// <summary>
        /// The description of the session action nature.
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
            => true;

        /// <summary>
        /// The length of the tag identification.
        /// </summary>
        public UInt64   Length
            => 0;

        #endregion

        #region Constructor(s)

        #region (static)  SessionActionNatures() <- Does reflection!

        static SessionActionNatures()
        {

            foreach (var methodInfo in typeof(SessionActionNatures).GetMethods())
            {
                if (methodInfo.IsStatic &&
                    methodInfo.GetParameters().Length == 0)
                {

                    var sessionActionNatureObject = methodInfo.Invoke(Activator.CreateInstance(typeof(SessionActionNatures)), null);

                    if (sessionActionNatureObject is SessionActionNatures sessionActionNature &&
                        !Lookup.ContainsKey(sessionActionNature.Code))
                    {
                        Lookup.TryAdd(sessionActionNature.Code, sessionActionNature);
                    }

                }
            }

        }

        #endregion

        #region (private) SessionActionNatures(Code, Description = null)

        /// <summary>
        /// Create a new session action nature.
        /// </summary>
        /// <param name="Code">The numeric code of the status.</param>
        /// <param name="Description">The description of the session action nature.</param>
        private SessionActionNatures(Int32    Code,
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
        /// Parse the given string as a session action nature.
        /// </summary>
        /// <param name="Code">The numeric code of the session action nature.</param>
        /// <param name="Description">The description of the session action nature.</param>
        public static SessionActionNatures Register(Int32    Code,
                                                    String?  Description   = null)

            => new (Code,
                    Description);

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a session action nature.
        /// </summary>
        /// <param name="Text">A text representation of a session action nature.</param>
        public static SessionActionNatures Parse(String Text)
        {

            #region Initial checks

            Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a session action nature must not be null or empty!");

            #endregion

            if (Int32.TryParse(Text, out var number))
                return Parse(number);

            throw new ArgumentException($"Invalid text representation of a session action nature: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse   (Code)

        /// <summary>
        /// Parse the given number as a session action nature.
        /// </summary>
        /// <param name="Code">A numeric representation of a session action nature.</param>
        public static SessionActionNatures Parse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out var sessionActionNature))
                return sessionActionNature;

            return new SessionActionNatures(Code);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a session action nature.
        /// </summary>
        /// <param name="Text">A text representation of a session action nature.</param>
        public static SessionActionNatures? TryParse(String Text)
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
        /// Try to parse the given number as a session action nature.
        /// </summary>
        /// <param name="Code">A numeric representation of a session action nature.</param>
        public static SessionActionNatures? TryParse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out var sessionActionNature))
                return sessionActionNature;

            return new SessionActionNatures(Code);

        }

        #endregion

        #region TryParse(Text, out SessionActionNature)

        /// <summary>
        /// Try to parse the given string as a session action nature.
        /// </summary>
        /// <param name="Text">A text representation of a session action nature.</param>
        /// <param name="SessionActionNatures">The parsed session action nature.</param>
        public static Boolean TryParse(String Text, out SessionActionNatures SessionActionNature)
        {

            #region Initial checks

            Text = Text.Trim();

            #endregion

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out var number))
            {
                SessionActionNature = default;
                return false;
            }

            SessionActionNature = new SessionActionNatures(number);
            return true;

        }

        #endregion

        #region TryParse(Code, out SessionActionNature)

        /// <summary>
        /// Try to parse the given number as a session action nature.
        /// </summary>
        /// <param name="Code">A numeric representation of a session action nature.</param>
        /// <param name="SessionActionNatures">The parsed session action nature.</param>
        public static Boolean TryParse(Int32 Code, out SessionActionNatures SessionActionNature)
        {

            if (Lookup.TryGetValue(Code, out SessionActionNature))
                return true;

            SessionActionNature = new SessionActionNatures(Code);
            return true;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this session action nature.
        /// </summary>
        public SessionActionNatures Clone

            => new (Code,
                    Description is not null && Description.IsNotNullOrEmpty()
                        ? new String(Description.ToCharArray())
                        : null);

        #endregion


        #region Static definitions

        // Source: GIREVE Gireve_Tech_eMIP-V0.7.4_ProtocolDescription_1.0.6-en.pdf
        // All those will be reflected and added to the lookup within the static constructor!

        /// <summary>
        /// Emergency Stop.
        /// </summary>
        public static SessionActionNatures EmergencyStop
            => new (0, "Emergency Stop");

        /// <summary>
        /// Stop and terminate current operation.
        /// </summary>
        public static SessionActionNatures Stop
            => new (1, "Stop and terminate current operation");

        /// <summary>
        /// Suspend current operation.
        /// </summary>
        public static SessionActionNatures Suspend
            => new (2, "Suspend current operation");

        /// <summary>
        /// Restart current operation.
        /// </summary>
        public static SessionActionNatures Restart
            => new (3, "Restart current operation");

        #endregion


        #region Operator overloading

        #region Operator == (SessionActionNature1, SessionActionNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNature1">A session action nature.</param>
        /// <param name="SessionActionNature2">Another session action nature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SessionActionNatures SessionActionNature1,
                                           SessionActionNatures SessionActionNature2)

            => SessionActionNature1.Equals(SessionActionNature2);

        #endregion

        #region Operator != (SessionActionNature1, SessionActionNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNature1">A session action nature.</param>
        /// <param name="SessionActionNature2">Another session action nature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SessionActionNatures SessionActionNature1,
                                           SessionActionNatures SessionActionNature2)

            => !SessionActionNature1.Equals(SessionActionNature2);

        #endregion

        #region Operator <  (SessionActionNature1, SessionActionNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNature1">A session action nature.</param>
        /// <param name="SessionActionNature2">Another session action nature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SessionActionNatures SessionActionNature1,
                                          SessionActionNatures SessionActionNature2)

            => SessionActionNature1.CompareTo(SessionActionNature2) < 0;

        #endregion

        #region Operator <= (SessionActionNature1, SessionActionNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNature1">A session action nature.</param>
        /// <param name="SessionActionNature2">Another session action nature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SessionActionNatures SessionActionNature1,
                                           SessionActionNatures SessionActionNature2)

            => SessionActionNature1.CompareTo(SessionActionNature2) <= 0;

        #endregion

        #region Operator >  (SessionActionNature1, SessionActionNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNature1">A session action nature.</param>
        /// <param name="SessionActionNature2">Another session action nature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SessionActionNatures SessionActionNature1,
                                          SessionActionNatures SessionActionNature2)

            => SessionActionNature1.CompareTo(SessionActionNature2) > 0;

        #endregion

        #region Operator >= (SessionActionNature1, SessionActionNature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionActionNature1">A session action nature.</param>
        /// <param name="SessionActionNature2">Another session action nature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SessionActionNatures SessionActionNature1,
                                           SessionActionNatures SessionActionNature2)

            => SessionActionNature1.CompareTo(SessionActionNature2) >= 0;

        #endregion

        #endregion

        #region IComparable<SessionActionNatures> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two session action natures.
        /// </summary>
        /// <param name="Object">A session action nature to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is SessionActionNatures sessionActionNature
                   ? CompareTo(sessionActionNature)
                   : throw new ArgumentException("The given object is not a session action nature!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(SessionActionNature)

        /// <summary>
        /// Compares two session action natures.
        /// </summary>
        /// <param name="SessionActionNature">A session action nature to compare with.</param>
        public Int32 CompareTo(SessionActionNatures SessionActionNature)

            => Code.CompareTo(SessionActionNature.Code);

        #endregion

        #endregion

        #region IEquatable<SessionActionNatures> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two session action natures for equality.
        /// </summary>
        /// <param name="Object">A session action nature to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SessionActionNatures sessionActionNature &&
                   Equals(sessionActionNature);

        #endregion

        #region Equals(SessionActionNature)

        /// <summary>
        /// Compares two session action natures for equality.
        /// </summary>
        /// <param name="SessionActionNature">A session action nature to compare with.</param>
        public Boolean Equals(SessionActionNatures SessionActionNature)

            => Code.Equals(SessionActionNature.Code);

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
