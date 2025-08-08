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

using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.EMP
{

    /// <summary>
    /// A Heartbeat response.
    /// </summary>
    public class HeartbeatResponse : AResponse<HeartbeatRequest,
                                               HeartbeatResponse>
    {

        #region Properties

        /// <summary>
        /// The heartbeat period.
        /// </summary>
        public TimeSpan        HeartbeatPeriod    { get; }

        /// <summary>
        /// The current time.
        /// </summary>
        public DateTimeOffset  CurrentTime        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Heartbeat response.
        /// </summary>
        /// <param name="Request">The Heartbeat request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// 
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public HeartbeatResponse(HeartbeatRequest        Request,
                                 Transaction_Id          TransactionId,
                                 RequestStatus           RequestStatus,

                                 HTTPResponse?           HTTPResponse   = null,
                                 JObject?                CustomData     = null,
                                 UserDefinedDictionary?  InternalData   = null)

            : this(Request,
                   TimeSpan.FromMinutes(5),
                   Timestamp.Now,
                   TransactionId,
                   RequestStatus,
                   HTTPResponse,
                   CustomData,
                   InternalData)

        { }


        /// <summary>
        /// Create a new heartbeat response.
        /// </summary>
        /// <param name="Request">The Heartbeat request leading to this response.</param>
        /// <param name="HeartbeatPeriod">A heartbeat period.</param>
        /// <param name="CurrentTime">The current time.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// 
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public HeartbeatResponse(HeartbeatRequest        Request,
                                 TimeSpan                HeartbeatPeriod,
                                 DateTimeOffset          CurrentTime,
                                 Transaction_Id          TransactionId,
                                 RequestStatus           RequestStatus,

                                 HTTPResponse?           HTTPResponse   = null,
                                 JObject?                CustomData     = null,
                                 UserDefinedDictionary?  InternalData   = null)

            : base(Request,
                   TransactionId,
                   RequestStatus,
                   HTTPResponse,
                   CustomData,
                   InternalData)

        {

            this.HeartbeatPeriod  = HeartbeatPeriod;
            this.CurrentTime      = CurrentTime;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/PlatformV1/">
        //
        //    <soap:Header/>
        //
        //    <soap:Body>
        //       <eMIP:eMIP_ToIOP_HeartBeatResponse>
        //          <heartBeatPeriod>2000</heartBeatPeriod>
        //          <currentTime>2015-10-26T10:32:52</currentTime>
        //          <transactionId>TRANSACTION_46151</transactionId>
        //          <requestStatus>1</requestStatus>
        //       </eMIP:eMIP_ToIOP_HeartBeatResponse>
        //    </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, HeartbeatResponseXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a Heartbeat response.
        /// </summary>
        /// <param name="Request">The Heartbeat request leading to this response.</param>
        /// <param name="HeartbeatResponseXML">The XML to parse.</param>
        /// <param name="CustomSendHeartbeatResponseParser">An optional delegate to parse custom HeartbeatResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static HeartbeatResponse Parse(HeartbeatRequest                            Request,
                                              XElement                                    HeartbeatResponseXML,
                                              CustomXMLParserDelegate<HeartbeatResponse>  CustomSendHeartbeatResponseParser   = null,
                                              HTTPResponse                                HTTPResponse                        = null,
                                              OnExceptionDelegate                         OnException                         = null)
        {

            if (TryParse(Request,
                         HeartbeatResponseXML,
                         out HeartbeatResponse HeartbeatResponse,
                         CustomSendHeartbeatResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return HeartbeatResponse;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (Request, HeartbeatResponseText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a Heartbeat response.
        /// </summary>
        /// <param name="Request">The Heartbeat request leading to this response.</param>
        /// <param name="HeartbeatResponseText">The text to parse.</param>
        /// <param name="CustomSendHeartbeatResponseParser">An optional delegate to parse custom HeartbeatResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static HeartbeatResponse Parse(HeartbeatRequest                            Request,
                                              String                                      HeartbeatResponseText,
                                              CustomXMLParserDelegate<HeartbeatResponse>  CustomSendHeartbeatResponseParser   = null,
                                              HTTPResponse                                HTTPResponse                        = null,
                                              OnExceptionDelegate                         OnException                         = null)
        {

            if (TryParse(Request,
                         HeartbeatResponseText,
                         out HeartbeatResponse HeartbeatResponse,
                         CustomSendHeartbeatResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return HeartbeatResponse;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Request, HeartbeatResponseXML,  ..., out HeartbeatResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a Heartbeat response.
        /// </summary>
        /// <param name="Request">The Heartbeat request leading to this response.</param>
        /// <param name="HeartbeatResponseXML">The XML to parse.</param>
        /// <param name="HeartbeatResponse">The parsed Heartbeat response.</param>
        /// <param name="CustomSendHeartbeatResponseParser">An optional delegate to parse custom HeartbeatResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(HeartbeatRequest                            Request,
                                       XElement                                    HeartbeatResponseXML,
                                       out HeartbeatResponse                       HeartbeatResponse,
                                       CustomXMLParserDelegate<HeartbeatResponse>  CustomSendHeartbeatResponseParser   = null,
                                       HTTPResponse                                HTTPResponse                        = null,
                                       OnExceptionDelegate                         OnException                         = null)
        {

            try
            {

                HeartbeatResponse = new HeartbeatResponse(

                                        Request,

                                        HeartbeatResponseXML.MapValueOrFail("heartBeatPeriod",
                                                                            s => TimeSpan.FromSeconds(UInt32.Parse(s))),

                                        HeartbeatResponseXML.MapValueOrFail("currentTime",
                                                                            s => DateTime.Parse(s)),

                                        HeartbeatResponseXML.MapValueOrFail("transactionId",
                                                                            Transaction_Id.Parse),

                                        HeartbeatResponseXML.MapValueOrFail("requestStatus",
                                                                            RequestStatus.Parse),

                                        HTTPResponse

                                    );


                if (CustomSendHeartbeatResponseParser is not null)
                    HeartbeatResponse = CustomSendHeartbeatResponseParser(HeartbeatResponseXML,
                                                                  HeartbeatResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, HeartbeatResponseXML, e);

                HeartbeatResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, HeartbeatResponseText, ..., out HeartbeatResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a Heartbeat response.
        /// </summary>
        /// <param name="Request">The Heartbeat request leading to this response.</param>
        /// <param name="HeartbeatResponseText">The text to parse.</param>
        /// <param name="HeartbeatResponse">The parsed Heartbeat response.</param>
        /// <param name="CustomSendHeartbeatResponseParser">An optional delegate to parse custom HeartbeatResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(HeartbeatRequest                            Request,
                                       String                                      HeartbeatResponseText,
                                       out HeartbeatResponse                       HeartbeatResponse,
                                       CustomXMLParserDelegate<HeartbeatResponse>  CustomSendHeartbeatResponseParser   = null,
                                       HTTPResponse                                HTTPResponse                        = null,
                                       OnExceptionDelegate                         OnException                         = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(HeartbeatResponseText).Root,
                             out HeartbeatResponse,
                             CustomSendHeartbeatResponseParser,
                             HTTPResponse,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, HeartbeatResponseText, e);
            }

            HeartbeatResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomHeartbeatResponseSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomHeartbeatResponseSerializer">A delegate to serialize custom Heartbeat response XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<HeartbeatResponse> CustomHeartbeatResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.Default + "eMIP_ToIOP_HeartbeatResponse",

                          new XElement(eMIPNS.Default + "heartBeatPeriod",  HeartbeatPeriod.TotalSeconds.ToString()),
                          new XElement(eMIPNS.Default + "currentTime",      CurrentTime.                 ToISO8601()),
                          new XElement(eMIPNS.Default + "transactionId",    TransactionId.               ToString()),
                          new XElement(eMIPNS.Default + "requestStatus",    RequestStatus.               ToString())

                      );


            return CustomHeartbeatResponseSerializer is not null
                       ? CustomHeartbeatResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (HeartbeatResponse1, HeartbeatResponse2)

        /// <summary>
        /// Compares two Heartbeat responses for equality.
        /// </summary>
        /// <param name="HeartbeatResponse1">A Heartbeat response.</param>
        /// <param name="HeartbeatResponse2">Another Heartbeat response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (HeartbeatResponse HeartbeatResponse1, HeartbeatResponse HeartbeatResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(HeartbeatResponse1, HeartbeatResponse2))
                return true;

            // If one is null, but not both, return false.
            if (HeartbeatResponse1 is null || HeartbeatResponse2 is null)
                return false;

            return HeartbeatResponse1.Equals(HeartbeatResponse2);

        }

        #endregion

        #region Operator != (HeartbeatResponse1, HeartbeatResponse2)

        /// <summary>
        /// Compares two Heartbeat responses for inequality.
        /// </summary>
        /// <param name="HeartbeatResponse1">A Heartbeat response.</param>
        /// <param name="HeartbeatResponse2">Another Heartbeat response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (HeartbeatResponse HeartbeatResponse1, HeartbeatResponse HeartbeatResponse2)
            => !(HeartbeatResponse1 == HeartbeatResponse2);

        #endregion

        #endregion

        #region IEquatable<HeartbeatResponse> Members

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

            var HeartbeatResponse = Object as HeartbeatResponse;
            if ((Object) HeartbeatResponse == null)
                return false;

            return Equals(HeartbeatResponse);

        }

        #endregion

        #region Equals(HeartbeatResponse)

        /// <summary>
        /// Compares two Heartbeat responses for equality.
        /// </summary>
        /// <param name="HeartbeatResponse">A Heartbeat response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(HeartbeatResponse HeartbeatResponse)
        {

            if ((Object) HeartbeatResponse == null)
                return false;

            return HeartbeatPeriod.Equals(HeartbeatResponse.HeartbeatPeriod) &&
                   CurrentTime.    Equals(HeartbeatResponse.CurrentTime)     &&
                   TransactionId.  Equals(HeartbeatResponse.TransactionId)   &&
                   RequestStatus.  Equals(HeartbeatResponse.RequestStatus);

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

                return HeartbeatPeriod.GetHashCode() * 7 ^
                       CurrentTime.    GetHashCode() * 5 ^
                       TransactionId.  GetHashCode() * 3 ^
                       RequestStatus.  GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(HeartbeatPeriod, ", ",
                             CurrentTime,     ", ",
                             TransactionId,   ", ",
                             RequestStatus);

        #endregion


        #region ToBuilder

        /// <summary>
        /// Return a response builder.
        /// </summary>
        public Builder ToBuilder
            => new Builder(this);

        #endregion

        #region (class) Builder

        /// <summary>
        /// A Heartbeat response builder.
        /// </summary>
        public new class Builder : AResponseBuilder<HeartbeatRequest,
                                                    HeartbeatResponse>
        {

            #region Properties

            /// <summary>
            /// The heartbeat period.
            /// </summary>
            public TimeSpan        HeartbeatPeriod    { get; set; }

            /// <summary>
            /// The current time.
            /// </summary>
            public DateTimeOffset  CurrentTime        { get; set; }

            ///// <summary>
            ///// The transaction identification.
            ///// </summary>
            //public Transaction_Id  TransactionId      { get; set; }

            ///// <summary>
            ///// The status of the request.
            ///// </summary>
            //public RequestStatus   RequestStatus      { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,           CustomData = null)

            /// <summary>
            /// Create a new Heartbeat response builder.
            /// </summary>
            /// <param name="Request">A Heartbeat request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(HeartbeatRequest        Request,
                           JObject?                CustomData     = null,
                           UserDefinedDictionary?  InternalData   = null)

                : base(Request,
                       CustomData,
                       InternalData)

            { }

            #endregion

            #region Builder(HeartbeatResponse, CustomData = null)

            /// <summary>
            /// Create a new Heartbeat response builder.
            /// </summary>
            /// <param name="HeartbeatResponse">A Heartbeat response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(HeartbeatResponse?      HeartbeatResponse   = null,
                           JObject?                CustomData          = null,
                           UserDefinedDictionary?  InternalData        = null)

                : base(HeartbeatResponse?.Request,
                       CustomData,
                       HeartbeatResponse.IsNotEmpty
                           ? InternalData.IsNotEmpty
                                 ? new UserDefinedDictionary(HeartbeatResponse.InternalData.Concat(InternalData))
                                 : new UserDefinedDictionary(HeartbeatResponse.InternalData)
                           : InternalData)

            {

                if (HeartbeatResponse is not null)
                {

                    this.HeartbeatPeriod  = HeartbeatResponse.HeartbeatPeriod;
                    this.CurrentTime      = HeartbeatResponse.CurrentTime;
                    this.TransactionId    = HeartbeatResponse.TransactionId;
                    this.RequestStatus    = HeartbeatResponse.RequestStatus;

                }

            }

            #endregion

            #endregion


            #region Equals(HeartbeatResponse)

            /// <summary>
            /// Compares two Heartbeat responses for equality.
            /// </summary>
            /// <param name="HeartbeatResponse">A Heartbeat response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(HeartbeatResponse HeartbeatResponse)
            {

                if ((Object) HeartbeatResponse == null)
                    return false;

                return HeartbeatPeriod.Equals(HeartbeatResponse.HeartbeatPeriod) &&
                       CurrentTime.    Equals(HeartbeatResponse.CurrentTime)     &&
                       TransactionId.  Equals(HeartbeatResponse.TransactionId)   &&
                       RequestStatus.  Equals(HeartbeatResponse.RequestStatus);

            }

            #endregion

            #region (implicit) "ToImmutable()"

            /// <summary>
            /// Return an immutable Heartbeat response.
            /// </summary>
            /// <param name="Builder">A Heartbeat response builder.</param>
            public static implicit operator HeartbeatResponse(Builder Builder)

                => new HeartbeatResponse(Builder.Request,
                                         Builder.HeartbeatPeriod,
                                         Builder.CurrentTime,
                                         Builder.TransactionId,
                                         Builder.RequestStatus);

            #endregion

        }

        #endregion

    }

}
