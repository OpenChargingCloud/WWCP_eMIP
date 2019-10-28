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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// A session action.
    /// </summary>
    public class SessionAction: IEquatable<SessionAction>
    {

        #region Properties

        /// <summary>
        /// The nature of the session action.
        /// </summary>
        public SessionActionNatures  Nature                   { get; }

        /// <summary>
        /// The timestamp of the session action.
        /// </summary>
        public DateTime              DateTime                 { get; }

        /// <summary>
        /// The unique identification of the session action.
        /// </summary>
        public SessionAction_Id?     Id                       { get; }

        /// <summary>
        /// Optional parameters of the session action.
        /// </summary>
        public String                Parameter                { get; }

        /// <summary>
        /// An optional related session event identification.
        /// </summary>
        public SessionEvent_Id?      RelatedSessionEventId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new session action.
        /// </summary>
        /// <param name="Nature">The nature of the session action.</param>
        /// <param name="DateTime">The timestamp of the session action.</param>
        /// <param name="Id">The unique identification of the session action.</param>
        /// <param name="Parameter">Optional parameters of the session action.</param>
        /// <param name="RelatedSessionEventId">An optional related session event identification.</param>
        public SessionAction(SessionActionNatures  Nature,
                             DateTime              DateTime,
                             SessionAction_Id?     Id                      = null,
                             String                Parameter               = null,
                             SessionEvent_Id?      RelatedSessionEventId   = null)


        {

            this.Nature                  = Nature;
            this.DateTime                = DateTime;
            this.Id                      = Id;
            this.Parameter               = Parameter?.Trim();
            this.RelatedSessionEventId   = RelatedSessionEventId;

        }

        #endregion


        #region Documentation

        // <sessionAction>
        //
        //     <sessionActionNature>0</sessionActionNature>
        //
        //     <!--Optional:-->
        //     <sessionActionId>00969e30-78a0-435e-a368-ea50ef20e878</sessionActionId>
        //
        //     <sessionActionDateTime>2015-11-18T22:47:35.123Z</sessionActionDateTime>
        //
        //     <!--Optional:-->
        //     <sessionActionParameter>Parameter1</sessionActionParameter>
        //
        //     <!--Optional:-->
        //     <relatedSessionEventId></relatedSessionEventId>
        //
        // </sessionAction>

        #endregion

        #region (static) Parse   (SessionActionXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP SessionAction object.
        /// </summary>
        /// <param name="SessionActionXML">The XML to parse.</param>
        /// <param name="CustomSessionActionParser">An optional delegate to parse custom SessionAction XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SessionAction Parse(XElement                                SessionActionXML,
                                          CustomXMLParserDelegate<SessionAction>  CustomSessionActionParser,
                                          OnExceptionDelegate                     OnException = null)
        {

            if (TryParse(SessionActionXML,
                         CustomSessionActionParser,
                         out SessionAction _SessionAction,
                         OnException))
            {
                return _SessionAction;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (SessionActionText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP SessionAction object.
        /// </summary>
        /// <param name="SessionActionText">The text to parse.</param>
        /// <param name="CustomSessionActionParser">An optional delegate to parse custom SessionAction XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SessionAction Parse(String                                  SessionActionText,
                                          CustomXMLParserDelegate<SessionAction>  CustomSessionActionParser,
                                          OnExceptionDelegate                     OnException = null)
        {

            if (TryParse(SessionActionText,
                         CustomSessionActionParser,
                         out SessionAction _SessionAction,
                         OnException))
            {
                return _SessionAction;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SessionActionXML,  ..., out SessionAction, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP SessionAction object.
        /// </summary>
        /// <param name="SessionActionXML">The XML to parse.</param>
        /// <param name="CustomSessionActionParser">An optional delegate to parse custom SessionAction XML elements.</param>
        /// <param name="SessionAction">The parsed SessionAction object.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                SessionActionXML,
                                       CustomXMLParserDelegate<SessionAction>  CustomSessionActionParser,
                                       out SessionAction                       SessionAction,
                                       OnExceptionDelegate                     OnException  = null)
        {

            try
            {

                SessionAction = new SessionAction(SessionActionXML.MapValueOrFail       ("sessionActionNature",    SessionActionNatures.Parse),
                                                  SessionActionXML.MapValueOrFail       ("sessionActionDateTime",  DateTime.            Parse),

                                                  SessionActionXML.MapValueOrNullable   ("sessionActionId",        SessionAction_Id.    Parse),
                                                  SessionActionXML.ElementValueOrDefault("sessionActionParameter"),
                                                  SessionActionXML.MapValueOrNullable   ("relatedSessionEventId",  SessionEvent_Id.     Parse));


                if (CustomSessionActionParser != null)
                    SessionAction = CustomSessionActionParser(SessionActionXML,
                                                              SessionAction);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SessionActionXML, e);

                SessionAction = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SessionActionText, ..., out SessionAction, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP SessionAction object.
        /// </summary>
        /// <param name="SessionActionText">The text to parse.</param>
        /// <param name="CustomSessionActionParser">An optional delegate to parse custom SessionAction XML elements.</param>
        /// <param name="SessionAction">The parsed SessionAction object.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                  SessionActionText,
                                       CustomXMLParserDelegate<SessionAction>  CustomSessionActionParser,
                                       out SessionAction                       SessionAction,
                                       OnExceptionDelegate                     OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SessionActionText).Root,
                             CustomSessionActionParser,
                             out SessionAction,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SessionActionText, e);
            }

            SessionAction = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSessionActionSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSessionActionSerializer">A delegate to serialize custom set EVSE busy status request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SessionAction> CustomSessionActionSerializer = null)
        {

            var XML = new XElement("sessionAction",

                              new XElement("sessionActionNature",           Nature.                     ToString()),

                              Id.HasValue
                                  ? new XElement("sessionActionId",         Id.Value.                   ToString())
                                  : null,

                              new XElement("sessionActionDateTime",         DateTime.                   ToIso8601()),

                              Parameter.IsNotNullOrEmpty()
                                  ? new XElement("sessionActionParameter",  Parameter)
                                  : null,

                              RelatedSessionEventId.HasValue
                                  ? new XElement("relatedSessionEventId",   RelatedSessionEventId.Value.ToString())
                                  : null

                      );


            return CustomSessionActionSerializer != null
                       ? CustomSessionActionSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SessionAction1, SessionAction2)

        /// <summary>
        /// Compares two SessionAction for equality.
        /// </summary>
        /// <param name="SessionAction1">An SessionAction.</param>
        /// <param name="SessionAction2">Another SessionAction.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SessionAction SessionAction1, SessionAction SessionAction2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SessionAction1, SessionAction2))
                return true;

            // If one is null, but not both, return false.
            if ((SessionAction1 is null) || (SessionAction2 is null))
                return false;

            return SessionAction1.Equals(SessionAction2);

        }

        #endregion

        #region Operator != (SessionAction1, SessionAction2)

        /// <summary>
        /// Compares two SessionAction for inequality.
        /// </summary>
        /// <param name="SessionAction1">An SessionAction.</param>
        /// <param name="SessionAction2">Another SessionAction.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SessionAction SessionAction1, SessionAction SessionAction2)

            => !(SessionAction1 == SessionAction2);

        #endregion

        #endregion

        #region IEquatable<SessionAction> Members

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

            if (!(Object is SessionAction SessionAction))
                return false;

            return Equals(SessionAction);

        }

        #endregion

        #region Equals(SessionAction)

        /// <summary>
        /// Compares two SessionAction for equality.
        /// </summary>
        /// <param name="SessionAction">An SessionAction to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SessionAction SessionAction)
        {

            if (SessionAction is null)
                return false;

            return Nature.  Equals(SessionAction.Nature)   &&
                   DateTime.Equals(SessionAction.DateTime) &&

                   ((!Id.                   HasValue           && !SessionAction.Id.                   HasValue) ||
                     (Id.                   HasValue           &&  SessionAction.Id.                   HasValue           && Id.Value.                   Equals(SessionAction.Id.Value))) &&

                   ((!Parameter.            IsNotNullOrEmpty() && !SessionAction.Parameter.            IsNotNullOrEmpty()) ||
                     (Parameter.            IsNotNullOrEmpty() &&  SessionAction.Parameter.            IsNotNullOrEmpty() && Parameter.                  Equals(SessionAction.Parameter))) &&

                   ((!RelatedSessionEventId.HasValue           && !SessionAction.RelatedSessionEventId.HasValue) ||
                     (RelatedSessionEventId.HasValue           &&  SessionAction.RelatedSessionEventId.HasValue           && RelatedSessionEventId.Value.Equals(SessionAction.RelatedSessionEventId.Value)));

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

                       (RelatedSessionEventId.HasValue
                            ? RelatedSessionEventId.GetHashCode()
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
