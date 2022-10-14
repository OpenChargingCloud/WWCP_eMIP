/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// A session event.
    /// </summary>
    public class SessionEvent: IEquatable<SessionEvent>
    {

        #region Properties

        /// <summary>
        /// The nature of the session event.
        /// </summary>
        public SessionEventNatures  Nature                    { get; }

        /// <summary>
        /// The timestamp of the session event.
        /// </summary>
        public DateTime             DateTime                  { get; }

        /// <summary>
        /// The unique identification of the session event.
        /// </summary>
        public SessionEvent_Id?     Id                        { get; }

        /// <summary>
        /// Optional parameters of the session event.
        /// </summary>
        public String               Parameter                 { get; }

        /// <summary>
        /// An optional related session action identification.
        /// </summary>
        public SessionAction_Id?    RelatedSessionActionId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new session event.
        /// </summary>
        /// <param name="Nature">The nature of the session event.</param>
        /// <param name="DateTime">The timestamp of the session event.</param>
        /// <param name="Id">The unique identification of the session event.</param>
        /// <param name="Parameter">Optional parameters of the session event.</param>
        /// <param name="RelatedSessionActionId">An optional related session action identification.</param>
        public SessionEvent(SessionEventNatures  Nature,
                            DateTime             DateTime,
                            SessionEvent_Id?     Id                       = null,
                            String               Parameter                = null,
                            SessionAction_Id?    RelatedSessionActionId   = null)


        {

            this.Nature                  = Nature;
            this.DateTime                = DateTime;
            this.Id                      = Id;
            this.Parameter               = Parameter?.Trim();
            this.RelatedSessionActionId  = RelatedSessionActionId;

        }

        #endregion


        #region Documentation

        // <sessionEvent>
        //
        //     <sessionEventNature>0</sessionEventNature>
        //
        //     <!--Optional:-->
        //     <sessionEventId>00969e30-78a0-435e-a368-ea50ef20e878</sessionEventId>
        //
        //     <sessionEventDateTime>2015-11-18T22:47:35.123Z</sessionEventDateTime>
        //
        //     <!--Optional:-->
        //     <sessionEventParameter>Parameter1</sessionEventParameter>
        //
        //     <!--Optional:-->
        //     <relatedSessionActionId></relatedSessionActionId>
        //
        // </sessionEvent>

        #endregion

        #region (static) Parse   (SessionEventXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP SessionEvent object.
        /// </summary>
        /// <param name="SessionEventXML">The XML to parse.</param>
        /// <param name="CustomSessionEventParser">An optional delegate to parse custom SessionEvent XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SessionEvent Parse(XElement                               SessionEventXML,
                                         CustomXMLParserDelegate<SessionEvent>  CustomSessionEventParser,
                                         OnExceptionDelegate                    OnException = null)
        {

            if (TryParse(SessionEventXML,
                         CustomSessionEventParser,
                         out SessionEvent _SessionEvent,
                         OnException))
            {
                return _SessionEvent;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (SessionEventText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP SessionEvent object.
        /// </summary>
        /// <param name="SessionEventText">The text to parse.</param>
        /// <param name="CustomSessionEventParser">An optional delegate to parse custom SessionEvent XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SessionEvent Parse(String                                 SessionEventText,
                                         CustomXMLParserDelegate<SessionEvent>  CustomSessionEventParser,
                                         OnExceptionDelegate                    OnException = null)
        {

            if (TryParse(SessionEventText,
                         CustomSessionEventParser,
                         out SessionEvent _SessionEvent,
                         OnException))
            {
                return _SessionEvent;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SessionEventXML,  ..., out SessionEvent, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP SessionEvent object.
        /// </summary>
        /// <param name="SessionEventXML">The XML to parse.</param>
        /// <param name="CustomSessionEventParser">An optional delegate to parse custom SessionEvent XML elements.</param>
        /// <param name="SessionEvent">The parsed SessionEvent object.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                               SessionEventXML,
                                       CustomXMLParserDelegate<SessionEvent>  CustomSessionEventParser,
                                       out SessionEvent                       SessionEvent,
                                       OnExceptionDelegate                    OnException  = null)
        {

            try
            {

                SessionEvent = new SessionEvent(SessionEventXML.MapValueOrFail       ("sessionEventNature",     SessionEventNatures.Parse),
                                                SessionEventXML.MapValueOrFail       ("sessionEventDateTime",   DateTime.           Parse),

                                                SessionEventXML.MapValueOrNullable   ("sessionEventId",         SessionEvent_Id.    Parse),
                                                SessionEventXML.ElementValueOrDefault("sessionEventParameter"),
                                                SessionEventXML.MapValueOrNullable   ("relatedSessionEventId",  SessionAction_Id.   Parse));


                if (CustomSessionEventParser != null)
                    SessionEvent = CustomSessionEventParser(SessionEventXML,
                                                            SessionEvent);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SessionEventXML, e);

                SessionEvent = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SessionEventText, ..., out SessionEvent, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP SessionEvent object.
        /// </summary>
        /// <param name="SessionEventText">The text to parse.</param>
        /// <param name="CustomSessionEventParser">An optional delegate to parse custom SessionEvent XML elements.</param>
        /// <param name="SessionEvent">The parsed SessionEvent object.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                 SessionEventText,
                                       CustomXMLParserDelegate<SessionEvent>  CustomSessionEventParser,
                                       out SessionEvent                       SessionEvent,
                                       OnExceptionDelegate                    OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SessionEventText).Root,
                             CustomSessionEventParser,
                             out SessionEvent,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SessionEventText, e);
            }

            SessionEvent = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSessionEventSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSessionEventSerializer">A delegate to serialize custom set EVSE busy status request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SessionEvent> CustomSessionEventSerializer = null)
        {

            var XML = new XElement("sessionEvent",

                              new XElement("sessionEventNature",            Nature.                      ToString()),

                              Id.HasValue
                                  ? new XElement("sessionEventId",          Id.Value.                    ToString())
                                  : null,

                              new XElement("sessionEventDateTime",          DateTime.                    ToIso8601()),

                              Parameter.IsNotNullOrEmpty()
                                  ? new XElement("sessionEventParameter",   Parameter)
                                  : null,

                              RelatedSessionActionId.HasValue
                                  ? new XElement("relatedSessionActionId",  RelatedSessionActionId.Value.ToString())
                                  : null

                      );


            return CustomSessionEventSerializer != null
                       ? CustomSessionEventSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SessionEvent1, SessionEvent2)

        /// <summary>
        /// Compares two SessionEvent for equality.
        /// </summary>
        /// <param name="SessionEvent1">An SessionEvent.</param>
        /// <param name="SessionEvent2">Another SessionEvent.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SessionEvent SessionEvent1, SessionEvent SessionEvent2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SessionEvent1, SessionEvent2))
                return true;

            // If one is null, but not both, return false.
            if ((SessionEvent1 is null) || (SessionEvent2 is null))
                return false;

            return SessionEvent1.Equals(SessionEvent2);

        }

        #endregion

        #region Operator != (SessionEvent1, SessionEvent2)

        /// <summary>
        /// Compares two SessionEvent for inequality.
        /// </summary>
        /// <param name="SessionEvent1">An SessionEvent.</param>
        /// <param name="SessionEvent2">Another SessionEvent.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SessionEvent SessionEvent1, SessionEvent SessionEvent2)

            => !(SessionEvent1 == SessionEvent2);

        #endregion

        #endregion

        #region IEquatable<SessionEvent> Members

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

            if (!(Object is SessionEvent SessionEvent))
                return false;

            return Equals(SessionEvent);

        }

        #endregion

        #region Equals(SessionEvent)

        /// <summary>
        /// Compares two SessionEvent for equality.
        /// </summary>
        /// <param name="SessionEvent">An SessionEvent to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SessionEvent SessionEvent)
        {

            if (SessionEvent is null)
                return false;

            return Nature.  Equals(SessionEvent.Nature)   &&
                   DateTime.Equals(SessionEvent.DateTime) &&

                   ((!Id.                    HasValue           && !SessionEvent.Id.                    HasValue) ||
                     (Id.                    HasValue           &&  SessionEvent.Id.                    HasValue           && Id.Value.                    Equals(SessionEvent.Id.Value))) &&

                   ((!Parameter.             IsNotNullOrEmpty() && !SessionEvent.Parameter.             IsNotNullOrEmpty()) ||
                     (Parameter.             IsNotNullOrEmpty() &&  SessionEvent.Parameter.             IsNotNullOrEmpty() && Parameter.                   Equals(SessionEvent.Parameter))) &&

                   ((!RelatedSessionActionId.HasValue           && !SessionEvent.RelatedSessionActionId.HasValue) ||
                     (RelatedSessionActionId.HasValue           &&  SessionEvent.RelatedSessionActionId.HasValue           && RelatedSessionActionId.Value.Equals(SessionEvent.RelatedSessionActionId.Value)));

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Nature.  GetHashCode() * 11 ^
                       DateTime.GetHashCode() *  7 ^

                       (Id.HasValue
                            ? Id.GetHashCode() * 5
                            : 0) ^

                       (Parameter.IsNotNullOrEmpty()
                            ? Parameter.GetHashCode() * 3
                            : 0) ^

                       (RelatedSessionActionId.HasValue
                            ? RelatedSessionActionId.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Nature, " @ ", DateTime.ToIso8601());

        #endregion

    }

}
