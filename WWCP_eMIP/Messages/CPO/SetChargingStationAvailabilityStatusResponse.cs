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
    /// A SetChargingStationAvailabilityStatus response.
    /// </summary>
    public class SetChargingStationAvailabilityStatusResponse : AResponse<SetChargingStationAvailabilityStatusRequest,
                                                                          SetChargingStationAvailabilityStatusResponse>
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
        /// Create a new SetChargingStationAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingStationAvailabilityStatus request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public SetChargingStationAvailabilityStatusResponse(SetChargingStationAvailabilityStatusRequest  Request,
                                                            Transaction_Id                               TransactionId,
                                                            RequestStatus                                RequestStatus,
                                                            IReadOnlyDictionary<String, Object>          CustomData  = null)

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
        //       <eMIP:eMIP_ToIOP_SetChargingStationAvailabilityStatusResponse>
        //          <transactionId>TRANSACTION_46151</transactionId>
        //          <requestStatus>1</requestStatus>
        //       </eMIP:eMIP_ToIOP_SetChargingStationAvailabilityStatusResponse>
        //    </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, SetChargingStationAvailabilityStatusResponseXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a SetChargingStationAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingStationAvailabilityStatus request leading to this response.</param>
        /// <param name="SetChargingStationAvailabilityStatusResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargingStationAvailabilityStatusResponseParser">An optional delegate to parse custom SetChargingStationAvailabilityStatusResponse XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetChargingStationAvailabilityStatusResponse Parse(SetChargingStationAvailabilityStatusRequest                            Request,
                                                                         XElement                                                               SetChargingStationAvailabilityStatusResponseXML,
                                                                         CustomXMLParserDelegate<SetChargingStationAvailabilityStatusResponse>  CustomSendSetChargingStationAvailabilityStatusResponseParser,
                                                                         OnExceptionDelegate                                                    OnException = null)
        {

            if (TryParse(Request,
                         SetChargingStationAvailabilityStatusResponseXML,
                         CustomSendSetChargingStationAvailabilityStatusResponseParser,
                         out SetChargingStationAvailabilityStatusResponse SetChargingStationAvailabilityStatusResponse,
                         OnException))
            {
                return SetChargingStationAvailabilityStatusResponse;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (Request, SetChargingStationAvailabilityStatusResponseText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a SetChargingStationAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingStationAvailabilityStatus request leading to this response.</param>
        /// <param name="SetChargingStationAvailabilityStatusResponseText">The text to parse.</param>
        /// <param name="CustomSendSetChargingStationAvailabilityStatusResponseParser">An optional delegate to parse custom SetChargingStationAvailabilityStatusResponse XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetChargingStationAvailabilityStatusResponse Parse(SetChargingStationAvailabilityStatusRequest                            Request,
                                                                         String                                                                 SetChargingStationAvailabilityStatusResponseText,
                                                                         CustomXMLParserDelegate<SetChargingStationAvailabilityStatusResponse>  CustomSendSetChargingStationAvailabilityStatusResponseParser,
                                                                         OnExceptionDelegate                                                    OnException = null)
        {

            if (TryParse(Request,
                         SetChargingStationAvailabilityStatusResponseText,
                         CustomSendSetChargingStationAvailabilityStatusResponseParser,
                         out SetChargingStationAvailabilityStatusResponse SetChargingStationAvailabilityStatusResponse,
                         OnException))
            {
                return SetChargingStationAvailabilityStatusResponse;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Request, SetChargingStationAvailabilityStatusResponseXML,  ..., out SetChargingStationAvailabilityStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a SetChargingStationAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingStationAvailabilityStatus request leading to this response.</param>
        /// <param name="SetChargingStationAvailabilityStatusResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargingStationAvailabilityStatusResponseParser">An optional delegate to parse custom SetChargingStationAvailabilityStatusResponse XML elements.</param>
        /// <param name="SetChargingStationAvailabilityStatusResponse">The parsed SetChargingStationAvailabilityStatus response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetChargingStationAvailabilityStatusRequest                            Request,
                                       XElement                                                               SetChargingStationAvailabilityStatusResponseXML,
                                       CustomXMLParserDelegate<SetChargingStationAvailabilityStatusResponse>  CustomSendSetChargingStationAvailabilityStatusResponseParser,
                                       out SetChargingStationAvailabilityStatusResponse                       SetChargingStationAvailabilityStatusResponse,
                                       OnExceptionDelegate                                                    OnException  = null)
        {

            try
            {

                SetChargingStationAvailabilityStatusResponse = new SetChargingStationAvailabilityStatusResponse(

                                                                   Request,

                                                                   SetChargingStationAvailabilityStatusResponseXML.MapValueOrFail(eMIPNS.EVCIDynamic + "transactionId",
                                                                                                                                  Transaction_Id.Parse),

                                                                   SetChargingStationAvailabilityStatusResponseXML.MapValueOrFail(eMIPNS.EVCIDynamic + "requestStatus",
                                                                                                                                  RequestStatus.Parse)

                                                               );


                if (CustomSendSetChargingStationAvailabilityStatusResponseParser != null)
                    SetChargingStationAvailabilityStatusResponse = CustomSendSetChargingStationAvailabilityStatusResponseParser(SetChargingStationAvailabilityStatusResponseXML,
                                                                  SetChargingStationAvailabilityStatusResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetChargingStationAvailabilityStatusResponseXML, e);

                SetChargingStationAvailabilityStatusResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, SetChargingStationAvailabilityStatusResponseText, ..., out SetChargingStationAvailabilityStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a SetChargingStationAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetChargingStationAvailabilityStatus request leading to this response.</param>
        /// <param name="SetChargingStationAvailabilityStatusResponseText">The text to parse.</param>
        /// <param name="CustomSendSetChargingStationAvailabilityStatusResponseParser">An optional delegate to parse custom SetChargingStationAvailabilityStatusResponse XML elements.</param>
        /// <param name="SetChargingStationAvailabilityStatusResponse">The parsed SetChargingStationAvailabilityStatus response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetChargingStationAvailabilityStatusRequest                            Request,
                                       String                                                                 SetChargingStationAvailabilityStatusResponseText,
                                       CustomXMLParserDelegate<SetChargingStationAvailabilityStatusResponse>  CustomSendSetChargingStationAvailabilityStatusResponseParser,
                                       out SetChargingStationAvailabilityStatusResponse                       SetChargingStationAvailabilityStatusResponse,
                                       OnExceptionDelegate                                                    OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(SetChargingStationAvailabilityStatusResponseText).Root,
                             CustomSendSetChargingStationAvailabilityStatusResponseParser,
                             out SetChargingStationAvailabilityStatusResponse,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SetChargingStationAvailabilityStatusResponseText, e);
            }

            SetChargingStationAvailabilityStatusResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetChargingStationAvailabilityStatusResponseSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetChargingStationAvailabilityStatusResponseSerializer">A delegate to serialize custom SetChargingStationAvailabilityStatus response XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetChargingStationAvailabilityStatusResponse> CustomSetChargingStationAvailabilityStatusResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.EVCIDynamic + "eMIP_ToIOP_SetChargingStationAvailabilityStatusResponse",

                          new XElement(eMIPNS.EVCIDynamic + "transactionId",  TransactionId.ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "requestStatus",  RequestStatus.ToString())

                      );


            return CustomSetChargingStationAvailabilityStatusResponseSerializer != null
                       ? CustomSetChargingStationAvailabilityStatusResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetChargingStationAvailabilityStatusResponse1, SetChargingStationAvailabilityStatusResponse2)

        /// <summary>
        /// Compares two SetChargingStationAvailabilityStatus responses for equality.
        /// </summary>
        /// <param name="SetChargingStationAvailabilityStatusResponse1">A SetChargingStationAvailabilityStatus response.</param>
        /// <param name="SetChargingStationAvailabilityStatusResponse2">Another SetChargingStationAvailabilityStatus response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetChargingStationAvailabilityStatusResponse SetChargingStationAvailabilityStatusResponse1, SetChargingStationAvailabilityStatusResponse SetChargingStationAvailabilityStatusResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SetChargingStationAvailabilityStatusResponse1, SetChargingStationAvailabilityStatusResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SetChargingStationAvailabilityStatusResponse1 == null) || ((Object) SetChargingStationAvailabilityStatusResponse2 == null))
                return false;

            return SetChargingStationAvailabilityStatusResponse1.Equals(SetChargingStationAvailabilityStatusResponse2);

        }

        #endregion

        #region Operator != (SetChargingStationAvailabilityStatusResponse1, SetChargingStationAvailabilityStatusResponse2)

        /// <summary>
        /// Compares two SetChargingStationAvailabilityStatus responses for inequality.
        /// </summary>
        /// <param name="SetChargingStationAvailabilityStatusResponse1">A SetChargingStationAvailabilityStatus response.</param>
        /// <param name="SetChargingStationAvailabilityStatusResponse2">Another SetChargingStationAvailabilityStatus response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetChargingStationAvailabilityStatusResponse SetChargingStationAvailabilityStatusResponse1, SetChargingStationAvailabilityStatusResponse SetChargingStationAvailabilityStatusResponse2)
            => !(SetChargingStationAvailabilityStatusResponse1 == SetChargingStationAvailabilityStatusResponse2);

        #endregion

        #endregion

        #region IEquatable<SetChargingStationAvailabilityStatusResponse> Members

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

            var SetChargingStationAvailabilityStatusResponse = Object as SetChargingStationAvailabilityStatusResponse;
            if ((Object) SetChargingStationAvailabilityStatusResponse == null)
                return false;

            return Equals(SetChargingStationAvailabilityStatusResponse);

        }

        #endregion

        #region Equals(SetChargingStationAvailabilityStatusResponse)

        /// <summary>
        /// Compares two SetChargingStationAvailabilityStatus responses for equality.
        /// </summary>
        /// <param name="SetChargingStationAvailabilityStatusResponse">A SetChargingStationAvailabilityStatus response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetChargingStationAvailabilityStatusResponse SetChargingStationAvailabilityStatusResponse)
        {

            if ((Object) SetChargingStationAvailabilityStatusResponse == null)
                return false;

            return TransactionId.Equals(SetChargingStationAvailabilityStatusResponse.TransactionId) &&
                   RequestStatus.Equals(SetChargingStationAvailabilityStatusResponse.RequestStatus);

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
        /// A SetChargingStationAvailabilityStatus response builder.
        /// </summary>
        public class Builder : AResponseBuilder<SetChargingStationAvailabilityStatusRequest,
                                                SetChargingStationAvailabilityStatusResponse>
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

            #region Builder(Request,                                      CustomData = null)

            /// <summary>
            /// Create a new SetChargingStationAvailabilityStatus response builder.
            /// </summary>
            /// <param name="Request">A SetChargingStationAvailabilityStatus request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetChargingStationAvailabilityStatusRequest  Request,
                           IReadOnlyDictionary<String, Object>          CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(SetChargingStationAvailabilityStatusResponse, CustomData = null)

            /// <summary>
            /// Create a new SetChargingStationAvailabilityStatus response builder.
            /// </summary>
            /// <param name="SetChargingStationAvailabilityStatusResponse">A SetChargingStationAvailabilityStatus response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetChargingStationAvailabilityStatusResponse  SetChargingStationAvailabilityStatusResponse  = null,
                           IReadOnlyDictionary<String, Object>           CustomData                                    = null)

                : base(SetChargingStationAvailabilityStatusResponse?.Request,
                       SetChargingStationAvailabilityStatusResponse.HasCustomData
                           ? CustomData?.Count > 0
                                 ? SetChargingStationAvailabilityStatusResponse.CustomData.Concat(CustomData)
                                 : SetChargingStationAvailabilityStatusResponse.CustomData
                           : CustomData)

            {

                if (SetChargingStationAvailabilityStatusResponse != null)
                {
                    this.TransactionId  = SetChargingStationAvailabilityStatusResponse.TransactionId;
                    this.RequestStatus  = SetChargingStationAvailabilityStatusResponse.RequestStatus;
                }

            }

            #endregion

            #endregion


            #region Equals(SetChargingStationAvailabilityStatusResponse)

            /// <summary>
            /// Compares two SetChargingStationAvailabilityStatus responses for equality.
            /// </summary>
            /// <param name="SetChargingStationAvailabilityStatusResponse">A SetChargingStationAvailabilityStatus response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(SetChargingStationAvailabilityStatusResponse SetChargingStationAvailabilityStatusResponse)
            {

                if ((Object) SetChargingStationAvailabilityStatusResponse == null)
                    return false;

                return TransactionId.Equals(SetChargingStationAvailabilityStatusResponse.TransactionId) &&
                       RequestStatus.Equals(SetChargingStationAvailabilityStatusResponse.RequestStatus);

            }

            #endregion

            #region ToImmutable

            /// <summary>
            /// Return an immutable representation.
            /// </summary>
            public override SetChargingStationAvailabilityStatusResponse ToImmutable

                => new SetChargingStationAvailabilityStatusResponse(Request,
                                                                    TransactionId,
                                                                    RequestStatus);

            #endregion

        }

        #endregion


    }

}
