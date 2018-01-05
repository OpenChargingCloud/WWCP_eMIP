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
    /// A SetChargingConnectorAvailabilityStatus response.
    /// </summary>
    public class SetChargingConnectorAvailabilityStatusResponse : AResponse<SetChargingConnectorAvailabilityStatusRequest,
                                                                       SetChargingConnectorAvailabilityStatusResponse>
    {

        #region Properties

        /// <summary>
        /// The transaction identification.
        /// </summary>
        public Transaction_Id  TransactionId    { get; }

        /// <summary>
        /// The status of the request.
        /// </summary>
        public RequestStatus   RequestStatus    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new SetChargingConnectorAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingConnectorAvailabilityStatus request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public SetChargingConnectorAvailabilityStatusResponse(SetChargingConnectorAvailabilityStatusRequest  Request,
                                                              Transaction_Id                                 TransactionId,
                                                              RequestStatus                                  RequestStatus,
                                                              IReadOnlyDictionary<String, Object>            CustomData  = null)

            : base(Request,
                   CustomData)

        {

            this.TransactionId  = TransactionId;
            this.RequestStatus  = RequestStatus;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/EVCIDynamicV1/">
        //
        //    <soap:Header/>
        //
        //    <soap:Body>
        //       <eMIP:eMIP_ToIOP_SetChargingConnectorAvailabilityStatusResponse>
        //          <transactionId>TRANSACTION_46151</transactionId>
        //          <requestStatus>1</requestStatus>
        //       </eMIP:eMIP_ToIOP_SetChargingConnectorAvailabilityStatusResponse>
        //    </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, SetChargingConnectorAvailabilityStatusResponseXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a SetChargingConnectorAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingConnectorAvailabilityStatus request leading to this response.</param>
        /// <param name="SetChargingConnectorAvailabilityStatusResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargingConnectorAvailabilityStatusResponseParser">A delegate to parse custom SetChargingConnectorAvailabilityStatusResponse XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetChargingConnectorAvailabilityStatusResponse Parse(SetChargingConnectorAvailabilityStatusRequest                            Request,
                                                                           XElement                                                                 SetChargingConnectorAvailabilityStatusResponseXML,
                                                                           CustomXMLParserDelegate<SetChargingConnectorAvailabilityStatusResponse>  CustomSendSetChargingConnectorAvailabilityStatusResponseParser,
                                                                           OnExceptionDelegate                                                      OnException = null)
        {

            if (TryParse(Request,
                         SetChargingConnectorAvailabilityStatusResponseXML,
                         CustomSendSetChargingConnectorAvailabilityStatusResponseParser,
                         out SetChargingConnectorAvailabilityStatusResponse SetChargingConnectorAvailabilityStatusResponse,
                         OnException))
            {
                return SetChargingConnectorAvailabilityStatusResponse;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (Request, SetChargingConnectorAvailabilityStatusResponseText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a SetChargingConnectorAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingConnectorAvailabilityStatus request leading to this response.</param>
        /// <param name="SetChargingConnectorAvailabilityStatusResponseText">The text to parse.</param>
        /// <param name="CustomSendSetChargingConnectorAvailabilityStatusResponseParser">A delegate to parse custom SetChargingConnectorAvailabilityStatusResponse XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetChargingConnectorAvailabilityStatusResponse Parse(SetChargingConnectorAvailabilityStatusRequest                            Request,
                                                                           String                                                                   SetChargingConnectorAvailabilityStatusResponseText,
                                                                           CustomXMLParserDelegate<SetChargingConnectorAvailabilityStatusResponse>  CustomSendSetChargingConnectorAvailabilityStatusResponseParser,
                                                                           OnExceptionDelegate                                                      OnException = null)
        {

            if (TryParse(Request,
                         SetChargingConnectorAvailabilityStatusResponseText,
                         CustomSendSetChargingConnectorAvailabilityStatusResponseParser,
                         out SetChargingConnectorAvailabilityStatusResponse SetChargingConnectorAvailabilityStatusResponse,
                         OnException))
            {
                return SetChargingConnectorAvailabilityStatusResponse;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Request, SetChargingConnectorAvailabilityStatusResponseXML,  ..., out SetChargingConnectorAvailabilityStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a SetChargingConnectorAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingConnectorAvailabilityStatus request leading to this response.</param>
        /// <param name="SetChargingConnectorAvailabilityStatusResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargingConnectorAvailabilityStatusResponseParser">A delegate to parse custom SetChargingConnectorAvailabilityStatusResponse XML elements.</param>
        /// <param name="SetChargingConnectorAvailabilityStatusResponse">The parsed SetChargingConnectorAvailabilityStatus response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetChargingConnectorAvailabilityStatusRequest                            Request,
                                       XElement                                                                 SetChargingConnectorAvailabilityStatusResponseXML,
                                       CustomXMLParserDelegate<SetChargingConnectorAvailabilityStatusResponse>  CustomSendSetChargingConnectorAvailabilityStatusResponseParser,
                                       out SetChargingConnectorAvailabilityStatusResponse                       SetChargingConnectorAvailabilityStatusResponse,
                                       OnExceptionDelegate                                                      OnException  = null)
        {

            try
            {

                SetChargingConnectorAvailabilityStatusResponse = new SetChargingConnectorAvailabilityStatusResponse(

                                                                Request,

                                                                SetChargingConnectorAvailabilityStatusResponseXML.MapValueOrFail(eMIPNS.EVCIDynamic + "transactionId",
                                                                                                                            Transaction_Id.Parse),

                                                                SetChargingConnectorAvailabilityStatusResponseXML.MapValueOrFail(eMIPNS.EVCIDynamic + "requestStatus",
                                                                                                                            RequestStatus.Parse)

                                                            );


                if (CustomSendSetChargingConnectorAvailabilityStatusResponseParser != null)
                    SetChargingConnectorAvailabilityStatusResponse = CustomSendSetChargingConnectorAvailabilityStatusResponseParser(SetChargingConnectorAvailabilityStatusResponseXML,
                                                                  SetChargingConnectorAvailabilityStatusResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetChargingConnectorAvailabilityStatusResponseXML, e);

                SetChargingConnectorAvailabilityStatusResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, SetChargingConnectorAvailabilityStatusResponseText, ..., out SetChargingConnectorAvailabilityStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a SetChargingConnectorAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingConnectorAvailabilityStatus request leading to this response.</param>
        /// <param name="SetChargingConnectorAvailabilityStatusResponseText">The text to parse.</param>
        /// <param name="CustomSendSetChargingConnectorAvailabilityStatusResponseParser">A delegate to parse custom SetChargingConnectorAvailabilityStatusResponse XML elements.</param>
        /// <param name="SetChargingConnectorAvailabilityStatusResponse">The parsed SetChargingConnectorAvailabilityStatus response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetChargingConnectorAvailabilityStatusRequest                            Request,
                                       String                                                                   SetChargingConnectorAvailabilityStatusResponseText,
                                       CustomXMLParserDelegate<SetChargingConnectorAvailabilityStatusResponse>  CustomSendSetChargingConnectorAvailabilityStatusResponseParser,
                                       out SetChargingConnectorAvailabilityStatusResponse                       SetChargingConnectorAvailabilityStatusResponse,
                                       OnExceptionDelegate                                                      OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(SetChargingConnectorAvailabilityStatusResponseText).Root,
                             CustomSendSetChargingConnectorAvailabilityStatusResponseParser,
                             out SetChargingConnectorAvailabilityStatusResponse,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SetChargingConnectorAvailabilityStatusResponseText, e);
            }

            SetChargingConnectorAvailabilityStatusResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetChargingConnectorAvailabilityStatusResponseSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetChargingConnectorAvailabilityStatusResponseSerializer">A delegate to serialize custom SetChargingConnectorAvailabilityStatus response XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetChargingConnectorAvailabilityStatusResponse> CustomSetChargingConnectorAvailabilityStatusResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.EVCIDynamic + "eMIP_ToIOP_SetChargingConnectorAvailabilityStatusResponse",

                          new XElement(eMIPNS.EVCIDynamic + "transactionId",  TransactionId.ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "requestStatus",  RequestStatus.ToString())

                      );


            return CustomSetChargingConnectorAvailabilityStatusResponseSerializer != null
                       ? CustomSetChargingConnectorAvailabilityStatusResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetChargingConnectorAvailabilityStatusResponse1, SetChargingConnectorAvailabilityStatusResponse2)

        /// <summary>
        /// Compares two SetChargingConnectorAvailabilityStatus responses for equality.
        /// </summary>
        /// <param name="SetChargingConnectorAvailabilityStatusResponse1">A SetChargingConnectorAvailabilityStatus response.</param>
        /// <param name="SetChargingConnectorAvailabilityStatusResponse2">Another SetChargingConnectorAvailabilityStatus response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetChargingConnectorAvailabilityStatusResponse SetChargingConnectorAvailabilityStatusResponse1, SetChargingConnectorAvailabilityStatusResponse SetChargingConnectorAvailabilityStatusResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SetChargingConnectorAvailabilityStatusResponse1, SetChargingConnectorAvailabilityStatusResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SetChargingConnectorAvailabilityStatusResponse1 == null) || ((Object) SetChargingConnectorAvailabilityStatusResponse2 == null))
                return false;

            return SetChargingConnectorAvailabilityStatusResponse1.Equals(SetChargingConnectorAvailabilityStatusResponse2);

        }

        #endregion

        #region Operator != (SetChargingConnectorAvailabilityStatusResponse1, SetChargingConnectorAvailabilityStatusResponse2)

        /// <summary>
        /// Compares two SetChargingConnectorAvailabilityStatus responses for inequality.
        /// </summary>
        /// <param name="SetChargingConnectorAvailabilityStatusResponse1">A SetChargingConnectorAvailabilityStatus response.</param>
        /// <param name="SetChargingConnectorAvailabilityStatusResponse2">Another SetChargingConnectorAvailabilityStatus response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetChargingConnectorAvailabilityStatusResponse SetChargingConnectorAvailabilityStatusResponse1, SetChargingConnectorAvailabilityStatusResponse SetChargingConnectorAvailabilityStatusResponse2)
            => !(SetChargingConnectorAvailabilityStatusResponse1 == SetChargingConnectorAvailabilityStatusResponse2);

        #endregion

        #endregion

        #region IEquatable<SetChargingConnectorAvailabilityStatusResponse> Members

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

            var SetChargingConnectorAvailabilityStatusResponse = Object as SetChargingConnectorAvailabilityStatusResponse;
            if ((Object) SetChargingConnectorAvailabilityStatusResponse == null)
                return false;

            return Equals(SetChargingConnectorAvailabilityStatusResponse);

        }

        #endregion

        #region Equals(SetChargingConnectorAvailabilityStatusResponse)

        /// <summary>
        /// Compares two SetChargingConnectorAvailabilityStatus responses for equality.
        /// </summary>
        /// <param name="SetChargingConnectorAvailabilityStatusResponse">A SetChargingConnectorAvailabilityStatus response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetChargingConnectorAvailabilityStatusResponse SetChargingConnectorAvailabilityStatusResponse)
        {

            if ((Object) SetChargingConnectorAvailabilityStatusResponse == null)
                return false;

            return TransactionId.Equals(SetChargingConnectorAvailabilityStatusResponse.TransactionId) &&
                   RequestStatus.Equals(SetChargingConnectorAvailabilityStatusResponse.RequestStatus);

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
        /// Return a string representation of this object.
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
        /// A SetChargingConnectorAvailabilityStatus response builder.
        /// </summary>
        public class Builder : AResponseBuilder<SetChargingConnectorAvailabilityStatusRequest,
                                                SetChargingConnectorAvailabilityStatusResponse>
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

            #region Builder(Request,                                        CustomData = null)

            /// <summary>
            /// Create a new SetChargingConnectorAvailabilityStatus response builder.
            /// </summary>
            /// <param name="Request">A SetChargingConnectorAvailabilityStatus request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetChargingConnectorAvailabilityStatusRequest  Request,
                           IReadOnlyDictionary<String, Object>            CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(SetChargingConnectorAvailabilityStatusResponse, CustomData = null)

            /// <summary>
            /// Create a new SetChargingConnectorAvailabilityStatus response builder.
            /// </summary>
            /// <param name="SetChargingConnectorAvailabilityStatusResponse">A SetChargingConnectorAvailabilityStatus response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetChargingConnectorAvailabilityStatusResponse  SetChargingConnectorAvailabilityStatusResponse  = null,
                           IReadOnlyDictionary<String, Object>             CustomData                                      = null)

                : base(SetChargingConnectorAvailabilityStatusResponse?.Request,
                       SetChargingConnectorAvailabilityStatusResponse.HasCustomData
                           ? CustomData?.Count > 0
                                 ? SetChargingConnectorAvailabilityStatusResponse.CustomData.Concat(CustomData)
                                 : SetChargingConnectorAvailabilityStatusResponse.CustomData
                           : CustomData)

            {

                if (SetChargingConnectorAvailabilityStatusResponse != null)
                {
                    this.TransactionId    = SetChargingConnectorAvailabilityStatusResponse.TransactionId;
                    this.RequestStatus    = SetChargingConnectorAvailabilityStatusResponse.RequestStatus;
                }

            }

            #endregion

            #endregion


            #region Equals(SetChargingConnectorAvailabilityStatusResponse)

            /// <summary>
            /// Compares two SetChargingConnectorAvailabilityStatus responses for equality.
            /// </summary>
            /// <param name="SetChargingConnectorAvailabilityStatusResponse">A SetChargingConnectorAvailabilityStatus response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(SetChargingConnectorAvailabilityStatusResponse SetChargingConnectorAvailabilityStatusResponse)
            {

                if ((Object) SetChargingConnectorAvailabilityStatusResponse == null)
                    return false;

                return TransactionId.Equals(SetChargingConnectorAvailabilityStatusResponse.TransactionId) &&
                       RequestStatus.Equals(SetChargingConnectorAvailabilityStatusResponse.RequestStatus);

            }

            #endregion

            #region ToImmutable

            /// <summary>
            /// Return an immutable representation.
            /// </summary>
            public override SetChargingConnectorAvailabilityStatusResponse ToImmutable

                => new SetChargingConnectorAvailabilityStatusResponse(Request,
                                                                      TransactionId,
                                                                      RequestStatus);

            #endregion

        }

        #endregion


    }

}
