/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A heartbeat response.
    /// </summary>
    public class ToIOP_HeartbeatResponse : AResponse<ToIOP_HeartbeatRequest,
                                                     ToIOP_HeartbeatResponse>
    {

        #region Properties

        /// <summary>
        /// The heartbeat period.
        /// </summary>
        public TimeSpan        HeartbeatPeriod   { get; }

        /// <summary>
        /// The current time.
        /// </summary>
        public DateTime        CurrentTime       { get; }

        /// <summary>
        /// The transaction identification.
        /// </summary>
        public Transaction_Id  TransactionId     { get; }

        /// <summary>
        /// The status of the request.
        /// </summary>
        public RequestStatus   RequestStatus     { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new heartbeat response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="HeartbeatPeriod">A heartbeat period.</param>
        /// <param name="CurrentTime">The current time.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public ToIOP_HeartbeatResponse(ToIOP_HeartbeatRequest               Request,
                                       TimeSpan                             HeartbeatPeriod,
                                       DateTime                             CurrentTime,
                                       Transaction_Id                       TransactionId,
                                       RequestStatus                        RequestStatus,
                                       IReadOnlyDictionary<String, Object>  CustomData  = null)

            : base(Request,
                   CustomData)

        {

            this.HeartbeatPeriod  = HeartbeatPeriod;
            this.CurrentTime      = CurrentTime;
            this.TransactionId    = TransactionId;
            this.RequestStatus    = RequestStatus;

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

        #region (static) Parse   (Request, HeartbeatResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="HeartbeatResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ToIOP_HeartbeatResponse Parse(ToIOP_HeartbeatRequest  Request,
                                                    XElement                HeartbeatResponseXML,
                                                    OnExceptionDelegate     OnException = null)
        {

            if (TryParse(Request, HeartbeatResponseXML, out ToIOP_HeartbeatResponse HeartbeatResponse, OnException))
                return HeartbeatResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, HeartbeatResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="HeartbeatResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ToIOP_HeartbeatResponse Parse(ToIOP_HeartbeatRequest  Request,
                                                    String                  HeartbeatResponseText,
                                                    OnExceptionDelegate     OnException = null)
        {

            if (TryParse(Request, HeartbeatResponseText, out ToIOP_HeartbeatResponse HeartbeatResponse, OnException))
                return HeartbeatResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, HeartbeatResponseXML,  out HeartbeatResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="HeartbeatResponseXML">The XML to parse.</param>
        /// <param name="HeartbeatResponse">The parsed add charge details records response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(ToIOP_HeartbeatRequest       Request,
                                       XElement                     HeartbeatResponseXML,
                                       out ToIOP_HeartbeatResponse  HeartbeatResponse,
                                       OnExceptionDelegate          OnException  = null)
        {

            try
            {

                HeartbeatResponse = new ToIOP_HeartbeatResponse(

                                        Request,

                                        HeartbeatResponseXML.MapValueOrFail(eMIPNS.Default + "heartBeatPeriod",
                                                                            s => TimeSpan.FromSeconds(UInt32.Parse(s))),

                                        HeartbeatResponseXML.MapValueOrFail(eMIPNS.Default + "currentTime",
                                                                            s => DateTime.Parse(s)),

                                        HeartbeatResponseXML.MapValueOrFail(eMIPNS.Default + "transactionId",
                                                                            s => Transaction_Id.Parse(s)),

                                        HeartbeatResponseXML.MapValueOrFail(eMIPNS.Default + "requestStatus",
                                                                            s => RequestStatus.Parse(s))

                                    );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, HeartbeatResponseXML, e);

                HeartbeatResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, HeartbeatResponseText, out HeartbeatResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="HeartbeatResponseText">The text to parse.</param>
        /// <param name="HeartbeatResponse">The parsed add charge details records response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(ToIOP_HeartbeatRequest       Request,
                                       String                       HeartbeatResponseText,
                                       out ToIOP_HeartbeatResponse  HeartbeatResponse,
                                       OnExceptionDelegate          OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(HeartbeatResponseText).Root,
                             out HeartbeatResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, HeartbeatResponseText, e);
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
        public XElement ToXML(CustomXMLSerializerDelegate<ToIOP_HeartbeatResponse> CustomHeartbeatResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.Default + "eMIP_ToIOP_HeartbeatResponse",

                          new XElement(eMIPNS.Default + "heartBeatPeriod",  HeartbeatPeriod.TotalSeconds.ToString()),
                          new XElement(eMIPNS.Default + "currentTime",      CurrentTime.                 ToIso8601()),
                          new XElement(eMIPNS.Default + "transactionId",    TransactionId.               ToString()),
                          new XElement(eMIPNS.Default + "requestStatus",    RequestStatus.               ToString())

                      );


            return CustomHeartbeatResponseSerializer != null
                       ? CustomHeartbeatResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (HeartbeatResponse1, HeartbeatResponse2)

        /// <summary>
        /// Compares two add charge details records responses for equality.
        /// </summary>
        /// <param name="HeartbeatResponse1">A add charge details records response.</param>
        /// <param name="HeartbeatResponse2">Another add charge details records response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ToIOP_HeartbeatResponse HeartbeatResponse1, ToIOP_HeartbeatResponse HeartbeatResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(HeartbeatResponse1, HeartbeatResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) HeartbeatResponse1 == null) || ((Object) HeartbeatResponse2 == null))
                return false;

            return HeartbeatResponse1.Equals(HeartbeatResponse2);

        }

        #endregion

        #region Operator != (HeartbeatResponse1, HeartbeatResponse2)

        /// <summary>
        /// Compares two add charge details records responses for inequality.
        /// </summary>
        /// <param name="HeartbeatResponse1">A add charge details records response.</param>
        /// <param name="HeartbeatResponse2">Another add charge details records response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ToIOP_HeartbeatResponse HeartbeatResponse1, ToIOP_HeartbeatResponse HeartbeatResponse2)
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

            // Check if the given object is a add charge details records response.
            var HeartbeatResponse = Object as ToIOP_HeartbeatResponse;
            if ((Object) HeartbeatResponse == null)
                return false;

            return Equals(HeartbeatResponse);

        }

        #endregion

        #region Equals(HeartbeatResponse)

        /// <summary>
        /// Compares two add charge details records responses for equality.
        /// </summary>
        /// <param name="HeartbeatResponse">A add charge details records response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ToIOP_HeartbeatResponse HeartbeatResponse)
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
        /// Return a string representation of this object.
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
        /// An AuthorizationStartById response builder.
        /// </summary>
        public class Builder : AResponseBuilder<ToIOP_HeartbeatRequest,
                                                ToIOP_HeartbeatResponse>
        {

            #region Properties

            /// <summary>
            /// The heartbeat period.
            /// </summary>
            public TimeSpan        HeartbeatPeriod    { get; set; }

            /// <summary>
            /// The current time.
            /// </summary>
            public DateTime        CurrentTime        { get; set; }

            /// <summary>
            /// The transaction identification.
            /// </summary>
            public Transaction_Id  TransactionId      { get; set; }

            /// <summary>
            /// The status of the request.
            /// </summary>
            public RequestStatus   RequestStatus      { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,           CustomData = null)

            /// <summary>
            /// Create a new HeartbeatResponse response builder.
            /// </summary>
            /// <param name="Request">A Heartbeat request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(ToIOP_HeartbeatRequest               Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(HeartbeatResponse, CustomData = null)

            /// <summary>
            /// Create a new Heartbeat response builder.
            /// </summary>
            /// <param name="HeartbeatResponse">A Heartbeat response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(ToIOP_HeartbeatResponse              HeartbeatResponse  = null,
                           IReadOnlyDictionary<String, Object>  CustomData         = null)

                : base(HeartbeatResponse?.Request,
                       HeartbeatResponse.HasCustomData
                           ? CustomData != null && CustomData.Any()
                                 ? HeartbeatResponse.CustomData.Concat(CustomData)
                                 : HeartbeatResponse.CustomData
                           : CustomData)

            {

                if (HeartbeatResponse != null)
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
            public override Boolean Equals(ToIOP_HeartbeatResponse HeartbeatResponse)
            {

                if ((Object) HeartbeatResponse == null)
                    return false;

                return HeartbeatPeriod.Equals(HeartbeatResponse.HeartbeatPeriod) &&
                       CurrentTime.    Equals(HeartbeatResponse.CurrentTime)     &&
                       TransactionId.  Equals(HeartbeatResponse.TransactionId)   &&
                       RequestStatus.  Equals(HeartbeatResponse.RequestStatus);

            }

            #endregion

            public override ToIOP_HeartbeatResponse ToImmutable

                => new ToIOP_HeartbeatResponse(Request,
                                               HeartbeatPeriod,
                                               CurrentTime,
                                               TransactionId,
                                               RequestStatus);

        }

        #endregion

    }

}
