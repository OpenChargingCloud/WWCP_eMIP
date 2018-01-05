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
    /// A SetEVSEAvailabilityStatus response.
    /// </summary>
    public class SetEVSEAvailabilityStatusResponse : AResponse<SetEVSEAvailabilityStatusRequest,
                                                               SetEVSEAvailabilityStatusResponse>
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
        /// Create a new SetEVSEAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSEAvailabilityStatus request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public SetEVSEAvailabilityStatusResponse(SetEVSEAvailabilityStatusRequest     Request,
                                                 Transaction_Id                       TransactionId,
                                                 RequestStatus                        RequestStatus,
                                                 IReadOnlyDictionary<String, Object>  CustomData  = null)

            : base(Request,
                   CustomData)

        {

            this.TransactionId    = TransactionId;
            this.RequestStatus    = RequestStatus;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/EVCIDynamicV1/">
        //
        //    <soap:Header/>
        //
        //    <soap:Body>
        //       <eMIP:eMIP_ToIOP_SetEVSEAvailabilityStatusResponse>
        //          <transactionId>TRANSACTION_46151</transactionId>
        //          <requestStatus>1</requestStatus>
        //       </eMIP:eMIP_ToIOP_SetEVSEAvailabilityStatusResponse>
        //    </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, SetEVSEAvailabilityStatusResponseXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a SetEVSEAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSEAvailabilityStatus request leading to this response.</param>
        /// <param name="SetEVSEAvailabilityStatusResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSEAvailabilityStatusResponseParser">A delegate to parse custom SetEVSEAvailabilityStatusResponse XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetEVSEAvailabilityStatusResponse Parse(SetEVSEAvailabilityStatusRequest                            Request,
                                                              XElement                                                    SetEVSEAvailabilityStatusResponseXML,
                                                              CustomXMLParserDelegate<SetEVSEAvailabilityStatusResponse>  CustomSendSetEVSEAvailabilityStatusResponseParser,
                                                              OnExceptionDelegate                                         OnException = null)
        {

            if (TryParse(Request,
                         SetEVSEAvailabilityStatusResponseXML,
                         CustomSendSetEVSEAvailabilityStatusResponseParser,
                         out SetEVSEAvailabilityStatusResponse SetEVSEAvailabilityStatusResponse,
                         OnException))
            {
                return SetEVSEAvailabilityStatusResponse;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (Request, SetEVSEAvailabilityStatusResponseText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a SetEVSEAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSEAvailabilityStatus request leading to this response.</param>
        /// <param name="SetEVSEAvailabilityStatusResponseText">The text to parse.</param>
        /// <param name="CustomSendSetEVSEAvailabilityStatusResponseParser">A delegate to parse custom SetEVSEAvailabilityStatusResponse XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetEVSEAvailabilityStatusResponse Parse(SetEVSEAvailabilityStatusRequest                            Request,
                                                              String                                                      SetEVSEAvailabilityStatusResponseText,
                                                              CustomXMLParserDelegate<SetEVSEAvailabilityStatusResponse>  CustomSendSetEVSEAvailabilityStatusResponseParser,
                                                              OnExceptionDelegate                                         OnException = null)
        {

            if (TryParse(Request,
                         SetEVSEAvailabilityStatusResponseText,
                         CustomSendSetEVSEAvailabilityStatusResponseParser,
                         out SetEVSEAvailabilityStatusResponse SetEVSEAvailabilityStatusResponse,
                         OnException))
            {
                return SetEVSEAvailabilityStatusResponse;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Request, SetEVSEAvailabilityStatusResponseXML,  ..., out SetEVSEAvailabilityStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a SetEVSEAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSEAvailabilityStatus request leading to this response.</param>
        /// <param name="SetEVSEAvailabilityStatusResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSEAvailabilityStatusResponseParser">A delegate to parse custom SetEVSEAvailabilityStatusResponse XML elements.</param>
        /// <param name="SetEVSEAvailabilityStatusResponse">The parsed SetEVSEAvailabilityStatus response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetEVSEAvailabilityStatusRequest                            Request,
                                       XElement                                                    SetEVSEAvailabilityStatusResponseXML,
                                       CustomXMLParserDelegate<SetEVSEAvailabilityStatusResponse>  CustomSendSetEVSEAvailabilityStatusResponseParser,
                                       out SetEVSEAvailabilityStatusResponse                       SetEVSEAvailabilityStatusResponse,
                                       OnExceptionDelegate                                         OnException  = null)
        {

            try
            {

                SetEVSEAvailabilityStatusResponse = new SetEVSEAvailabilityStatusResponse(

                                                        Request,

                                                        SetEVSEAvailabilityStatusResponseXML.MapValueOrFail(eMIPNS.EVCIDynamic + "transactionId",
                                                                                                            Transaction_Id.Parse),

                                                        SetEVSEAvailabilityStatusResponseXML.MapValueOrFail(eMIPNS.EVCIDynamic + "requestStatus",
                                                                                                            RequestStatus.Parse)

                                                    );


                if (CustomSendSetEVSEAvailabilityStatusResponseParser != null)
                    SetEVSEAvailabilityStatusResponse = CustomSendSetEVSEAvailabilityStatusResponseParser(SetEVSEAvailabilityStatusResponseXML,
                                                                  SetEVSEAvailabilityStatusResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetEVSEAvailabilityStatusResponseXML, e);

                SetEVSEAvailabilityStatusResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, SetEVSEAvailabilityStatusResponseText, ..., out SetEVSEAvailabilityStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a SetEVSEAvailabilityStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSEAvailabilityStatus request leading to this response.</param>
        /// <param name="SetEVSEAvailabilityStatusResponseText">The text to parse.</param>
        /// <param name="CustomSendSetEVSEAvailabilityStatusResponseParser">A delegate to parse custom SetEVSEAvailabilityStatusResponse XML elements.</param>
        /// <param name="SetEVSEAvailabilityStatusResponse">The parsed SetEVSEAvailabilityStatus response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetEVSEAvailabilityStatusRequest                            Request,
                                       String                                                      SetEVSEAvailabilityStatusResponseText,
                                       CustomXMLParserDelegate<SetEVSEAvailabilityStatusResponse>  CustomSendSetEVSEAvailabilityStatusResponseParser,
                                       out SetEVSEAvailabilityStatusResponse                       SetEVSEAvailabilityStatusResponse,
                                       OnExceptionDelegate                                         OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(SetEVSEAvailabilityStatusResponseText).Root,
                             CustomSendSetEVSEAvailabilityStatusResponseParser,
                             out SetEVSEAvailabilityStatusResponse,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SetEVSEAvailabilityStatusResponseText, e);
            }

            SetEVSEAvailabilityStatusResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetEVSEAvailabilityStatusResponseSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetEVSEAvailabilityStatusResponseSerializer">A delegate to serialize custom SetEVSEAvailabilityStatus response XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetEVSEAvailabilityStatusResponse> CustomSetEVSEAvailabilityStatusResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.EVCIDynamic + "eMIP_ToIOP_SetEVSEAvailabilityStatusResponse",

                          new XElement(eMIPNS.EVCIDynamic + "transactionId",  TransactionId.ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "requestStatus",  RequestStatus.ToString())

                      );


            return CustomSetEVSEAvailabilityStatusResponseSerializer != null
                       ? CustomSetEVSEAvailabilityStatusResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetEVSEAvailabilityStatusResponse1, SetEVSEAvailabilityStatusResponse2)

        /// <summary>
        /// Compares two SetEVSEAvailabilityStatus responses for equality.
        /// </summary>
        /// <param name="SetEVSEAvailabilityStatusResponse1">A SetEVSEAvailabilityStatus response.</param>
        /// <param name="SetEVSEAvailabilityStatusResponse2">Another SetEVSEAvailabilityStatus response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetEVSEAvailabilityStatusResponse SetEVSEAvailabilityStatusResponse1, SetEVSEAvailabilityStatusResponse SetEVSEAvailabilityStatusResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SetEVSEAvailabilityStatusResponse1, SetEVSEAvailabilityStatusResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SetEVSEAvailabilityStatusResponse1 == null) || ((Object) SetEVSEAvailabilityStatusResponse2 == null))
                return false;

            return SetEVSEAvailabilityStatusResponse1.Equals(SetEVSEAvailabilityStatusResponse2);

        }

        #endregion

        #region Operator != (SetEVSEAvailabilityStatusResponse1, SetEVSEAvailabilityStatusResponse2)

        /// <summary>
        /// Compares two SetEVSEAvailabilityStatus responses for inequality.
        /// </summary>
        /// <param name="SetEVSEAvailabilityStatusResponse1">A SetEVSEAvailabilityStatus response.</param>
        /// <param name="SetEVSEAvailabilityStatusResponse2">Another SetEVSEAvailabilityStatus response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetEVSEAvailabilityStatusResponse SetEVSEAvailabilityStatusResponse1, SetEVSEAvailabilityStatusResponse SetEVSEAvailabilityStatusResponse2)
            => !(SetEVSEAvailabilityStatusResponse1 == SetEVSEAvailabilityStatusResponse2);

        #endregion

        #endregion

        #region IEquatable<SetEVSEAvailabilityStatusResponse> Members

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

            var SetEVSEAvailabilityStatusResponse = Object as SetEVSEAvailabilityStatusResponse;
            if ((Object) SetEVSEAvailabilityStatusResponse == null)
                return false;

            return Equals(SetEVSEAvailabilityStatusResponse);

        }

        #endregion

        #region Equals(SetEVSEAvailabilityStatusResponse)

        /// <summary>
        /// Compares two SetEVSEAvailabilityStatus responses for equality.
        /// </summary>
        /// <param name="SetEVSEAvailabilityStatusResponse">A SetEVSEAvailabilityStatus response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetEVSEAvailabilityStatusResponse SetEVSEAvailabilityStatusResponse)
        {

            if ((Object) SetEVSEAvailabilityStatusResponse == null)
                return false;

            return TransactionId.Equals(SetEVSEAvailabilityStatusResponse.TransactionId) &&
                   RequestStatus.Equals(SetEVSEAvailabilityStatusResponse.RequestStatus);

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
        /// A SetEVSEAvailabilityStatus response builder.
        /// </summary>
        public class Builder : AResponseBuilder<SetEVSEAvailabilityStatusRequest,
                                                SetEVSEAvailabilityStatusResponse>
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

            #region Builder(Request,                           CustomData = null)

            /// <summary>
            /// Create a new SetEVSEAvailabilityStatus response builder.
            /// </summary>
            /// <param name="Request">A SetEVSEAvailabilityStatus request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetEVSEAvailabilityStatusRequest     Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(SetEVSEAvailabilityStatusResponse, CustomData = null)

            /// <summary>
            /// Create a new SetEVSEAvailabilityStatus response builder.
            /// </summary>
            /// <param name="SetEVSEAvailabilityStatusResponse">A SetEVSEAvailabilityStatus response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetEVSEAvailabilityStatusResponse    SetEVSEAvailabilityStatusResponse  = null,
                           IReadOnlyDictionary<String, Object>  CustomData                         = null)

                : base(SetEVSEAvailabilityStatusResponse?.Request,
                       SetEVSEAvailabilityStatusResponse.HasCustomData
                           ? CustomData?.Count > 0
                                 ? SetEVSEAvailabilityStatusResponse.CustomData.Concat(CustomData)
                                 : SetEVSEAvailabilityStatusResponse.CustomData
                           : CustomData)

            {

                if (SetEVSEAvailabilityStatusResponse != null)
                {
                    this.TransactionId  = SetEVSEAvailabilityStatusResponse.TransactionId;
                    this.RequestStatus  = SetEVSEAvailabilityStatusResponse.RequestStatus;
                }

            }

            #endregion

            #endregion


            #region Equals(SetEVSEAvailabilityStatusResponse)

            /// <summary>
            /// Compares two SetEVSEAvailabilityStatus responses for equality.
            /// </summary>
            /// <param name="SetEVSEAvailabilityStatusResponse">A SetEVSEAvailabilityStatus response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(SetEVSEAvailabilityStatusResponse SetEVSEAvailabilityStatusResponse)
            {

                if ((Object) SetEVSEAvailabilityStatusResponse == null)
                    return false;

                return TransactionId.Equals(SetEVSEAvailabilityStatusResponse.TransactionId) &&
                       RequestStatus.Equals(SetEVSEAvailabilityStatusResponse.RequestStatus);

            }

            #endregion

            #region ToImmutable

            /// <summary>
            /// Return an immutable representation.
            /// </summary>
            public override SetEVSEAvailabilityStatusResponse ToImmutable

                => new SetEVSEAvailabilityStatusResponse(Request,
                                                         TransactionId,
                                                         RequestStatus);

            #endregion

        }

        #endregion


    }

}
