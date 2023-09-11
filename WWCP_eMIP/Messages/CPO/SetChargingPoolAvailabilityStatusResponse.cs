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

using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A SetChargingPoolAvailabilityStatus response.
    /// </summary>
    public class SetChargingPoolAvailabilityStatusResponse : AResponse<SetChargingPoolAvailabilityStatusRequest,
                                                                       SetChargingPoolAvailabilityStatusResponse>
    {

        #region Constructor(s)

        /// <summary>
        /// Create a new SetChargingPoolAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingPoolAvailabilityStatus request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// 
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public SetChargingPoolAvailabilityStatusResponse(SetChargingPoolAvailabilityStatusRequest  Request,
                                                         Transaction_Id                            TransactionId,
                                                         RequestStatus                             RequestStatus,

                                                         HTTPResponse?                             HTTPResponse   = null,
                                                         JObject?                                  CustomData     = null,
                                                         UserDefinedDictionary?                    InternalData   = null)

            : base(Request,
                   TransactionId,
                   RequestStatus,
                   HTTPResponse,
                   CustomData,
                   InternalData)

        { }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/EVCIDynamicV1/">
        //
        //    <soap:Header/>
        //
        //    <soap:Body>
        //       <eMIP:eMIP_ToIOP_SetChargingPoolAvailabilityStatusResponse>
        //          <transactionId>TRANSACTION_46151</transactionId>
        //          <requestStatus>1</requestStatus>
        //       </eMIP:eMIP_ToIOP_SetChargingPoolAvailabilityStatusResponse>
        //    </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, SetChargingPoolAvailabilityStatusResponseXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a SetChargingPoolAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingPoolAvailabilityStatus request leading to this response.</param>
        /// <param name="SetChargingPoolAvailabilityStatusResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargingPoolAvailabilityStatusResponseParser">An optional delegate to parse custom SetChargingPoolAvailabilityStatusResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetChargingPoolAvailabilityStatusResponse Parse(SetChargingPoolAvailabilityStatusRequest                            Request,
                                                                      XElement                                                            SetChargingPoolAvailabilityStatusResponseXML,
                                                                      CustomXMLParserDelegate<SetChargingPoolAvailabilityStatusResponse>  CustomSendSetChargingPoolAvailabilityStatusResponseParser   = null,
                                                                      HTTPResponse                                                        HTTPResponse                                                = null,
                                                                      OnExceptionDelegate                                                 OnException                                                 = null)
        {

            if (TryParse(Request,
                         SetChargingPoolAvailabilityStatusResponseXML,
                         out SetChargingPoolAvailabilityStatusResponse SetChargingPoolAvailabilityStatusResponse,
                         CustomSendSetChargingPoolAvailabilityStatusResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return SetChargingPoolAvailabilityStatusResponse;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (Request, SetChargingPoolAvailabilityStatusResponseText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a SetChargingPoolAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingPoolAvailabilityStatus request leading to this response.</param>
        /// <param name="SetChargingPoolAvailabilityStatusResponseText">The text to parse.</param>
        /// <param name="CustomSendSetChargingPoolAvailabilityStatusResponseParser">An optional delegate to parse custom SetChargingPoolAvailabilityStatusResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetChargingPoolAvailabilityStatusResponse Parse(SetChargingPoolAvailabilityStatusRequest                            Request,
                                                                      String                                                              SetChargingPoolAvailabilityStatusResponseText,
                                                                      CustomXMLParserDelegate<SetChargingPoolAvailabilityStatusResponse>  CustomSendSetChargingPoolAvailabilityStatusResponseParser   = null,
                                                                      HTTPResponse                                                        HTTPResponse                                                = null,
                                                                      OnExceptionDelegate                                                 OnException                                                 = null)
        {

            if (TryParse(Request,
                         SetChargingPoolAvailabilityStatusResponseText,
                         out SetChargingPoolAvailabilityStatusResponse SetChargingPoolAvailabilityStatusResponse,
                         CustomSendSetChargingPoolAvailabilityStatusResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return SetChargingPoolAvailabilityStatusResponse;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Request, SetChargingPoolAvailabilityStatusResponseXML,  ..., out SetChargingPoolAvailabilityStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a SetChargingPoolAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingPoolAvailabilityStatus request leading to this response.</param>
        /// <param name="SetChargingPoolAvailabilityStatusResponseXML">The XML to parse.</param>
        /// <param name="SetChargingPoolAvailabilityStatusResponse">The parsed SetChargingPoolAvailabilityStatus response.</param>
        /// <param name="CustomSendSetChargingPoolAvailabilityStatusResponseParser">An optional delegate to parse custom SetChargingPoolAvailabilityStatusResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetChargingPoolAvailabilityStatusRequest                            Request,
                                       XElement                                                            SetChargingPoolAvailabilityStatusResponseXML,
                                       out SetChargingPoolAvailabilityStatusResponse                       SetChargingPoolAvailabilityStatusResponse,
                                       CustomXMLParserDelegate<SetChargingPoolAvailabilityStatusResponse>  CustomSendSetChargingPoolAvailabilityStatusResponseParser   = null,
                                       HTTPResponse                                                        HTTPResponse                                                = null,
                                       OnExceptionDelegate                                                 OnException                                                 = null)
        {

            try
            {

                SetChargingPoolAvailabilityStatusResponse = new SetChargingPoolAvailabilityStatusResponse(

                                                                Request,

                                                                SetChargingPoolAvailabilityStatusResponseXML.MapValueOrFail(eMIPNS.EVCIDynamic + "transactionId",
                                                                                                                            Transaction_Id.Parse),

                                                                SetChargingPoolAvailabilityStatusResponseXML.MapValueOrFail(eMIPNS.EVCIDynamic + "requestStatus",
                                                                                                                            RequestStatus.Parse),

                                                                HTTPResponse

                                                            );


                if (CustomSendSetChargingPoolAvailabilityStatusResponseParser != null)
                    SetChargingPoolAvailabilityStatusResponse = CustomSendSetChargingPoolAvailabilityStatusResponseParser(SetChargingPoolAvailabilityStatusResponseXML,
                                                                  SetChargingPoolAvailabilityStatusResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, SetChargingPoolAvailabilityStatusResponseXML, e);

                SetChargingPoolAvailabilityStatusResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, SetChargingPoolAvailabilityStatusResponseText, ..., out SetChargingPoolAvailabilityStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a SetChargingPoolAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingPoolAvailabilityStatus request leading to this response.</param>
        /// <param name="SetChargingPoolAvailabilityStatusResponseText">The text to parse.</param>
        /// <param name="SetChargingPoolAvailabilityStatusResponse">The parsed SetChargingPoolAvailabilityStatus response.</param>
        /// <param name="CustomSendSetChargingPoolAvailabilityStatusResponseParser">An optional delegate to parse custom SetChargingPoolAvailabilityStatusResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetChargingPoolAvailabilityStatusRequest                            Request,
                                       String                                                              SetChargingPoolAvailabilityStatusResponseText,
                                       out SetChargingPoolAvailabilityStatusResponse                       SetChargingPoolAvailabilityStatusResponse,
                                       CustomXMLParserDelegate<SetChargingPoolAvailabilityStatusResponse>  CustomSendSetChargingPoolAvailabilityStatusResponseParser   = null,
                                       HTTPResponse                                                        HTTPResponse                                                = null,
                                       OnExceptionDelegate                                                 OnException                                                 = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(SetChargingPoolAvailabilityStatusResponseText).Root,
                             out SetChargingPoolAvailabilityStatusResponse,
                             CustomSendSetChargingPoolAvailabilityStatusResponseParser,
                             HTTPResponse,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, SetChargingPoolAvailabilityStatusResponseText, e);
            }

            SetChargingPoolAvailabilityStatusResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetChargingPoolAvailabilityStatusResponseSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetChargingPoolAvailabilityStatusResponseSerializer">A delegate to serialize custom SetChargingPoolAvailabilityStatus response XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetChargingPoolAvailabilityStatusResponse> CustomSetChargingPoolAvailabilityStatusResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.EVCIDynamic + "eMIP_ToIOP_SetChargingPoolAvailabilityStatusResponse",

                          new XElement(eMIPNS.EVCIDynamic + "transactionId",  TransactionId.ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "requestStatus",  RequestStatus.ToString())

                      );


            return CustomSetChargingPoolAvailabilityStatusResponseSerializer != null
                       ? CustomSetChargingPoolAvailabilityStatusResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetChargingPoolAvailabilityStatusResponse1, SetChargingPoolAvailabilityStatusResponse2)

        /// <summary>
        /// Compares two SetChargingPoolAvailabilityStatus responses for equality.
        /// </summary>
        /// <param name="SetChargingPoolAvailabilityStatusResponse1">A SetChargingPoolAvailabilityStatus response.</param>
        /// <param name="SetChargingPoolAvailabilityStatusResponse2">Another SetChargingPoolAvailabilityStatus response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetChargingPoolAvailabilityStatusResponse SetChargingPoolAvailabilityStatusResponse1, SetChargingPoolAvailabilityStatusResponse SetChargingPoolAvailabilityStatusResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetChargingPoolAvailabilityStatusResponse1, SetChargingPoolAvailabilityStatusResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SetChargingPoolAvailabilityStatusResponse1 == null) || ((Object) SetChargingPoolAvailabilityStatusResponse2 == null))
                return false;

            return SetChargingPoolAvailabilityStatusResponse1.Equals(SetChargingPoolAvailabilityStatusResponse2);

        }

        #endregion

        #region Operator != (SetChargingPoolAvailabilityStatusResponse1, SetChargingPoolAvailabilityStatusResponse2)

        /// <summary>
        /// Compares two SetChargingPoolAvailabilityStatus responses for inequality.
        /// </summary>
        /// <param name="SetChargingPoolAvailabilityStatusResponse1">A SetChargingPoolAvailabilityStatus response.</param>
        /// <param name="SetChargingPoolAvailabilityStatusResponse2">Another SetChargingPoolAvailabilityStatus response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetChargingPoolAvailabilityStatusResponse SetChargingPoolAvailabilityStatusResponse1, SetChargingPoolAvailabilityStatusResponse SetChargingPoolAvailabilityStatusResponse2)
            => !(SetChargingPoolAvailabilityStatusResponse1 == SetChargingPoolAvailabilityStatusResponse2);

        #endregion

        #endregion

        #region IEquatable<SetChargingPoolAvailabilityStatusResponse> Members

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

            var SetChargingPoolAvailabilityStatusResponse = Object as SetChargingPoolAvailabilityStatusResponse;
            if ((Object) SetChargingPoolAvailabilityStatusResponse == null)
                return false;

            return Equals(SetChargingPoolAvailabilityStatusResponse);

        }

        #endregion

        #region Equals(SetChargingPoolAvailabilityStatusResponse)

        /// <summary>
        /// Compares two SetChargingPoolAvailabilityStatus responses for equality.
        /// </summary>
        /// <param name="SetChargingPoolAvailabilityStatusResponse">A SetChargingPoolAvailabilityStatus response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetChargingPoolAvailabilityStatusResponse SetChargingPoolAvailabilityStatusResponse)
        {

            if ((Object) SetChargingPoolAvailabilityStatusResponse == null)
                return false;

            return TransactionId.Equals(SetChargingPoolAvailabilityStatusResponse.TransactionId) &&
                   RequestStatus.Equals(SetChargingPoolAvailabilityStatusResponse.RequestStatus);

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

                return TransactionId.GetHashCode() * 3 ^
                       RequestStatus.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(TransactionId, ", ",
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
        /// A SetChargingPoolAvailabilityStatus response builder.
        /// </summary>
        public new class Builder : AResponseBuilder<SetChargingPoolAvailabilityStatusRequest,
                                                    SetChargingPoolAvailabilityStatusResponse>
        {

            #region Properties

            /// <summary>
            /// The transaction identification.
            /// </summary>
            public Transaction_Id  TransactionId    { get; set; }

            /// <summary>
            /// The status of the request.
            /// </summary>
            public RequestStatus   RequestStatus    { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,                                   CustomData = null)

            /// <summary>
            /// Create a new SetChargingPoolAvailabilityStatus response builder.
            /// </summary>
            /// <param name="Request">A SetChargingPoolAvailabilityStatus request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetChargingPoolAvailabilityStatusRequest  Request,
                           JObject?                                  CustomData     = null,
                           UserDefinedDictionary?                    InternalData   = null)

                : base(Request,
                       CustomData,
                       InternalData)

            { }

            #endregion

            #region Builder(SetChargingPoolAvailabilityStatusResponse, CustomData = null)

            /// <summary>
            /// Create a new SetChargingPoolAvailabilityStatus response builder.
            /// </summary>
            /// <param name="SetChargingPoolAvailabilityStatusResponse">A SetChargingPoolAvailabilityStatus response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetChargingPoolAvailabilityStatusResponse?  SetChargingPoolAvailabilityStatusResponse   = null,
                           JObject?                                    CustomData                                  = null,
                           UserDefinedDictionary?                      InternalData                                = null)

                : base(SetChargingPoolAvailabilityStatusResponse?.Request,
                       CustomData,
                       SetChargingPoolAvailabilityStatusResponse.IsNotEmpty
                           ? InternalData.IsNotEmpty
                                 ? new UserDefinedDictionary(SetChargingPoolAvailabilityStatusResponse.InternalData.Concat(InternalData))
                                 : new UserDefinedDictionary(SetChargingPoolAvailabilityStatusResponse.InternalData)
                           : InternalData)

            {

                if (SetChargingPoolAvailabilityStatusResponse is not null)
                {
                    this.TransactionId  = SetChargingPoolAvailabilityStatusResponse.TransactionId;
                    this.RequestStatus  = SetChargingPoolAvailabilityStatusResponse.RequestStatus;
                }

            }

            #endregion

            #endregion


            #region Equals(SetChargingPoolAvailabilityStatusResponse)

            /// <summary>
            /// Compares two SetChargingPoolAvailabilityStatus responses for equality.
            /// </summary>
            /// <param name="SetChargingPoolAvailabilityStatusResponse">A SetChargingPoolAvailabilityStatus response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(SetChargingPoolAvailabilityStatusResponse SetChargingPoolAvailabilityStatusResponse)
            {

                if ((Object) SetChargingPoolAvailabilityStatusResponse == null)
                    return false;

                return TransactionId.Equals(SetChargingPoolAvailabilityStatusResponse.TransactionId) &&
                       RequestStatus.Equals(SetChargingPoolAvailabilityStatusResponse.RequestStatus);

            }

            #endregion

            #region (implicit) "ToImmutable()"

            /// <summary>
            /// Return an immutable SetChargingPoolAvailabilityStatusResponse response.
            /// </summary>
            /// <param name="Builder">A SetChargingPoolAvailabilityStatusResponse response builder.</param>
            public static implicit operator SetChargingPoolAvailabilityStatusResponse(Builder Builder)

                => new SetChargingPoolAvailabilityStatusResponse(Builder.Request,
                                                                 Builder.TransactionId,
                                                                 Builder.RequestStatus);

            #endregion

        }

        #endregion

    }

}
