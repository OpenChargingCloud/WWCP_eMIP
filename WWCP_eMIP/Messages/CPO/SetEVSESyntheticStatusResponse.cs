﻿/*
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

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A SetEVSESyntheticStatus response.
    /// </summary>
    public class SetEVSESyntheticStatusResponse : AResponse<SetEVSESyntheticStatusRequest,
                                                            SetEVSESyntheticStatusResponse>
    {

        #region Constructor(s)

        /// <summary>
        /// Create a new SetEVSESyntheticStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSESyntheticStatus request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// 
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public SetEVSESyntheticStatusResponse(SetEVSESyntheticStatusRequest  Request,
                                              Transaction_Id                 TransactionId,
                                              RequestStatus                  RequestStatus,

                                              HTTPResponse?                  HTTPResponse   = null,
                                              JObject?                       CustomData     = null,
                                              UserDefinedDictionary?         InternalData   = null)

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
        //       <eMIP:eMIP_ToIOP_SetEVSESyntheticStatusResponse>
        //          <transactionId>TRANSACTION_46151</transactionId>
        //          <requestStatus>1</requestStatus>
        //       </eMIP:eMIP_ToIOP_SetEVSESyntheticStatusResponse>
        //    </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, SetEVSESyntheticStatusResponseXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a SetEVSESyntheticStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSESyntheticStatus request leading to this response.</param>
        /// <param name="SetEVSESyntheticStatusResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSESyntheticStatusResponseParser">An optional delegate to parse custom SetEVSESyntheticStatusResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetEVSESyntheticStatusResponse Parse(SetEVSESyntheticStatusRequest                            Request,
                                                           XElement                                                 SetEVSESyntheticStatusResponseXML,
                                                           CustomXMLParserDelegate<SetEVSESyntheticStatusResponse>  CustomSendSetEVSESyntheticStatusResponseParser   = null,
                                                           HTTPResponse                                             HTTPResponse                                     = null,
                                                           OnExceptionDelegate                                      OnException                                      = null)
        {

            if (TryParse(Request,
                         SetEVSESyntheticStatusResponseXML,
                         out SetEVSESyntheticStatusResponse SetEVSESyntheticStatusResponse,
                         CustomSendSetEVSESyntheticStatusResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return SetEVSESyntheticStatusResponse;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (Request, SetEVSESyntheticStatusResponseText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a SetEVSESyntheticStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSESyntheticStatus request leading to this response.</param>
        /// <param name="SetEVSESyntheticStatusResponseText">The text to parse.</param>
        /// <param name="CustomSendSetEVSESyntheticStatusResponseParser">An optional delegate to parse custom SetEVSESyntheticStatusResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetEVSESyntheticStatusResponse Parse(SetEVSESyntheticStatusRequest                            Request,
                                                           String                                                   SetEVSESyntheticStatusResponseText,
                                                           CustomXMLParserDelegate<SetEVSESyntheticStatusResponse>  CustomSendSetEVSESyntheticStatusResponseParser   = null,
                                                           HTTPResponse                                             HTTPResponse                                     = null,
                                                           OnExceptionDelegate                                      OnException                                      = null)
        {

            if (TryParse(Request,
                         SetEVSESyntheticStatusResponseText,
                         out SetEVSESyntheticStatusResponse SetEVSESyntheticStatusResponse,
                         CustomSendSetEVSESyntheticStatusResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return SetEVSESyntheticStatusResponse;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Request, SetEVSESyntheticStatusResponseXML,  ..., out SetEVSESyntheticStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a SetEVSESyntheticStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSESyntheticStatus request leading to this response.</param>
        /// <param name="SetEVSESyntheticStatusResponseXML">The XML to parse.</param>
        /// <param name="SetEVSESyntheticStatusResponse">The parsed SetEVSESyntheticStatus response.</param>
        /// <param name="CustomSendSetEVSESyntheticStatusResponseParser">An optional delegate to parse custom SetEVSESyntheticStatusResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetEVSESyntheticStatusRequest                            Request,
                                       XElement                                                 SetEVSESyntheticStatusResponseXML,
                                       out SetEVSESyntheticStatusResponse                       SetEVSESyntheticStatusResponse,
                                       CustomXMLParserDelegate<SetEVSESyntheticStatusResponse>  CustomSendSetEVSESyntheticStatusResponseParser   = null,
                                       HTTPResponse                                             HTTPResponse                                     = null,
                                       OnExceptionDelegate                                      OnException                                      = null)
        {

            try
            {

                SetEVSESyntheticStatusResponse = new SetEVSESyntheticStatusResponse(

                                                     Request,

                                                     SetEVSESyntheticStatusResponseXML.MapValueOrFail(eMIPNS.EVCIDynamic + "transactionId",
                                                                                                      Transaction_Id.Parse),

                                                     SetEVSESyntheticStatusResponseXML.MapValueOrFail(eMIPNS.EVCIDynamic + "requestStatus",
                                                                                                      RequestStatus.Parse),

                                                     HTTPResponse

                                                 );


                if (CustomSendSetEVSESyntheticStatusResponseParser is not null)
                    SetEVSESyntheticStatusResponse = CustomSendSetEVSESyntheticStatusResponseParser(SetEVSESyntheticStatusResponseXML,
                                                                  SetEVSESyntheticStatusResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, SetEVSESyntheticStatusResponseXML, e);

                SetEVSESyntheticStatusResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, SetEVSESyntheticStatusResponseText, ..., out SetEVSESyntheticStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a SetEVSESyntheticStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSESyntheticStatus request leading to this response.</param>
        /// <param name="SetEVSESyntheticStatusResponseText">The text to parse.</param>
        /// <param name="SetEVSESyntheticStatusResponse">The parsed SetEVSESyntheticStatus response.</param>
        /// <param name="CustomSendSetEVSESyntheticStatusResponseParser">An optional delegate to parse custom SetEVSESyntheticStatusResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetEVSESyntheticStatusRequest                            Request,
                                       String                                                   SetEVSESyntheticStatusResponseText,
                                       out SetEVSESyntheticStatusResponse                       SetEVSESyntheticStatusResponse,
                                       CustomXMLParserDelegate<SetEVSESyntheticStatusResponse>  CustomSendSetEVSESyntheticStatusResponseParser   = null,
                                       HTTPResponse                                             HTTPResponse                                     = null,
                                       OnExceptionDelegate                                      OnException                                      = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(SetEVSESyntheticStatusResponseText).Root,
                             out SetEVSESyntheticStatusResponse,
                             CustomSendSetEVSESyntheticStatusResponseParser,
                             HTTPResponse,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, SetEVSESyntheticStatusResponseText, e);
            }

            SetEVSESyntheticStatusResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetEVSESyntheticStatusResponseSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetEVSESyntheticStatusResponseSerializer">A delegate to serialize custom Heartbeat response XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetEVSESyntheticStatusResponse> CustomSetEVSESyntheticStatusResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.EVCIDynamic + "eMIP_ToIOP_SetEVSESyntheticStatusResponse",

                          new XElement(eMIPNS.EVCIDynamic + "transactionId",  TransactionId.ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "requestStatus",  RequestStatus.ToString())

                      );


            return CustomSetEVSESyntheticStatusResponseSerializer is not null
                       ? CustomSetEVSESyntheticStatusResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetEVSESyntheticStatusResponse1, SetEVSESyntheticStatusResponse2)

        /// <summary>
        /// Compares two SetEVSESyntheticStatus responses for equality.
        /// </summary>
        /// <param name="SetEVSESyntheticStatusResponse1">A SetEVSESyntheticStatus response.</param>
        /// <param name="SetEVSESyntheticStatusResponse2">Another SetEVSESyntheticStatus response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetEVSESyntheticStatusResponse SetEVSESyntheticStatusResponse1, SetEVSESyntheticStatusResponse SetEVSESyntheticStatusResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetEVSESyntheticStatusResponse1, SetEVSESyntheticStatusResponse2))
                return true;

            // If one is null, but not both, return false.
            if (SetEVSESyntheticStatusResponse1 is null || SetEVSESyntheticStatusResponse2 is null)
                return false;

            return SetEVSESyntheticStatusResponse1.Equals(SetEVSESyntheticStatusResponse2);

        }

        #endregion

        #region Operator != (SetEVSESyntheticStatusResponse1, SetEVSESyntheticStatusResponse2)

        /// <summary>
        /// Compares two SetEVSESyntheticStatus responses for inequality.
        /// </summary>
        /// <param name="SetEVSESyntheticStatusResponse1">A SetEVSESyntheticStatus response.</param>
        /// <param name="SetEVSESyntheticStatusResponse2">Another SetEVSESyntheticStatus response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetEVSESyntheticStatusResponse SetEVSESyntheticStatusResponse1, SetEVSESyntheticStatusResponse SetEVSESyntheticStatusResponse2)
            => !(SetEVSESyntheticStatusResponse1 == SetEVSESyntheticStatusResponse2);

        #endregion

        #endregion

        #region IEquatable<SetEVSESyntheticStatusResponse> Members

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

            var SetEVSESyntheticStatusResponse = Object as SetEVSESyntheticStatusResponse;
            if ((Object) SetEVSESyntheticStatusResponse == null)
                return false;

            return Equals(SetEVSESyntheticStatusResponse);

        }

        #endregion

        #region Equals(SetEVSESyntheticStatusResponse)

        /// <summary>
        /// Compares two SetEVSESyntheticStatus responses for equality.
        /// </summary>
        /// <param name="SetEVSESyntheticStatusResponse">A SetEVSESyntheticStatus response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetEVSESyntheticStatusResponse SetEVSESyntheticStatusResponse)
        {

            if ((Object) SetEVSESyntheticStatusResponse == null)
                return false;

            return TransactionId.Equals(SetEVSESyntheticStatusResponse.TransactionId) &&
                   RequestStatus.Equals(SetEVSESyntheticStatusResponse.RequestStatus);

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
        /// A SetEVSESyntheticStatus response builder.
        /// </summary>
        public new class Builder : AResponseBuilder<SetEVSESyntheticStatusRequest,
                                                    SetEVSESyntheticStatusResponse>
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

            #region Builder(Request,                        CustomData = null)

            /// <summary>
            /// Create a new SetEVSESyntheticStatus response builder.
            /// </summary>
            /// <param name="Request">A SetEVSESyntheticStatus request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetEVSESyntheticStatusRequest  Request,
                           JObject?                       CustomData     = null,
                           UserDefinedDictionary?         InternalData   = null)

                : base(Request,
                       CustomData,
                       InternalData)

            { }

            #endregion

            #region Builder(SetEVSESyntheticStatusResponse, CustomData = null)

            /// <summary>
            /// Create a new SetEVSESyntheticStatus response builder.
            /// </summary>
            /// <param name="SetEVSESyntheticStatusResponse">A SetEVSESyntheticStatus response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetEVSESyntheticStatusResponse?  SetEVSESyntheticStatusResponse   = null,
                           JObject?                         CustomData                       = null,
                           UserDefinedDictionary?           InternalData                     = null)

                : base(SetEVSESyntheticStatusResponse?.Request,
                       CustomData,
                       SetEVSESyntheticStatusResponse.IsEmpty
                           ? InternalData.IsNotEmpty
                                 ? new UserDefinedDictionary(SetEVSESyntheticStatusResponse.InternalData.Concat(InternalData))
                                 : new UserDefinedDictionary(SetEVSESyntheticStatusResponse.InternalData)
                           : InternalData)

            {

                if (SetEVSESyntheticStatusResponse is not null)
                {
                    this.TransactionId  = SetEVSESyntheticStatusResponse.TransactionId;
                    this.RequestStatus  = SetEVSESyntheticStatusResponse.RequestStatus;
                }

            }

            #endregion

            #endregion


            #region Equals(SetEVSESyntheticStatusResponse)

            /// <summary>
            /// Compares two SetEVSESyntheticStatus responses for equality.
            /// </summary>
            /// <param name="SetEVSESyntheticStatusResponse">A SetEVSESyntheticStatus response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(SetEVSESyntheticStatusResponse SetEVSESyntheticStatusResponse)
            {

                if ((Object) SetEVSESyntheticStatusResponse == null)
                    return false;

                return TransactionId.Equals(SetEVSESyntheticStatusResponse.TransactionId) &&
                       RequestStatus.Equals(SetEVSESyntheticStatusResponse.RequestStatus);

            }

            #endregion

            #region (implicit) "ToImmutable()"

            /// <summary>
            /// Return an immutable SetEVSESyntheticStatusResponse response.
            /// </summary>
            /// <param name="Builder">A SetEVSESyntheticStatusResponse response builder.</param>
            public static implicit operator SetEVSESyntheticStatusResponse(Builder Builder)

                => new SetEVSESyntheticStatusResponse(Builder.Request,
                                                      Builder.TransactionId,
                                                      Builder.RequestStatus);

            #endregion

        }

        #endregion

    }

}
