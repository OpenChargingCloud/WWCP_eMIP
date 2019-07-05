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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A SetEVSEBusyStatus response.
    /// </summary>
    public class SetEVSEBusyStatusResponse : AResponse<SetEVSEBusyStatusRequest,
                                                       SetEVSEBusyStatusResponse>
    {

        #region Constructor(s)

        /// <summary>
        /// Create a new SetEVSEBusyStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSEBusyStatus request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public SetEVSEBusyStatusResponse(SetEVSEBusyStatusRequest             Request,
                                         Transaction_Id                       TransactionId,
                                         RequestStatus                        RequestStatus,
                                         IReadOnlyDictionary<String, Object>  CustomData  = null)

            : base(Request,
                   TransactionId,
                   RequestStatus,
                   CustomData)

        { }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/EVCIDynamicV1/">
        //
        //    <soap:Header/>
        //
        //    <soap:Body>
        //       <eMIP:eMIP_ToIOP_SetEVSEBusyStatusResponse>
        //          <transactionId>TRANSACTION_46151</transactionId>
        //          <requestStatus>1</requestStatus>
        //       </eMIP:eMIP_ToIOP_SetEVSEBusyStatusResponse>
        //    </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, SetEVSEBusyStatusResponseXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a SetEVSEBusyStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSEBusyStatus request leading to this response.</param>
        /// <param name="SetEVSEBusyStatusResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSEBusyStatusResponseParser">An optional delegate to parse custom SetEVSEBusyStatusResponse XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetEVSEBusyStatusResponse Parse(SetEVSEBusyStatusRequest                            Request,
                                                      XElement                                            SetEVSEBusyStatusResponseXML,
                                                      CustomXMLParserDelegate<SetEVSEBusyStatusResponse>  CustomSendSetEVSEBusyStatusResponseParser,
                                                      OnExceptionDelegate                                 OnException = null)
        {

            if (TryParse(Request,
                         SetEVSEBusyStatusResponseXML,
                         CustomSendSetEVSEBusyStatusResponseParser,
                         out SetEVSEBusyStatusResponse SetEVSEBusyStatusResponse,
                         OnException))
            {
                return SetEVSEBusyStatusResponse;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (Request, SetEVSEBusyStatusResponseText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a SetEVSEBusyStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSEBusyStatus request leading to this response.</param>
        /// <param name="SetEVSEBusyStatusResponseText">The text to parse.</param>
        /// <param name="CustomSendSetEVSEBusyStatusResponseParser">An optional delegate to parse custom SetEVSEBusyStatusResponse XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetEVSEBusyStatusResponse Parse(SetEVSEBusyStatusRequest                            Request,
                                                      String                                              SetEVSEBusyStatusResponseText,
                                                      CustomXMLParserDelegate<SetEVSEBusyStatusResponse>  CustomSendSetEVSEBusyStatusResponseParser,
                                                      OnExceptionDelegate                                 OnException = null)
        {

            if (TryParse(Request,
                         SetEVSEBusyStatusResponseText,
                         CustomSendSetEVSEBusyStatusResponseParser,
                         out SetEVSEBusyStatusResponse SetEVSEBusyStatusResponse,
                         OnException))
            {
                return SetEVSEBusyStatusResponse;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Request, SetEVSEBusyStatusResponseXML,  ..., out SetEVSEBusyStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a SetEVSEBusyStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSEBusyStatus request leading to this response.</param>
        /// <param name="SetEVSEBusyStatusResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSEBusyStatusResponseParser">An optional delegate to parse custom SetEVSEBusyStatusResponse XML elements.</param>
        /// <param name="SetEVSEBusyStatusResponse">The parsed SetEVSEBusyStatus response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetEVSEBusyStatusRequest                            Request,
                                       XElement                                            SetEVSEBusyStatusResponseXML,
                                       CustomXMLParserDelegate<SetEVSEBusyStatusResponse>  CustomSendSetEVSEBusyStatusResponseParser,
                                       out SetEVSEBusyStatusResponse                       SetEVSEBusyStatusResponse,
                                       OnExceptionDelegate                                 OnException  = null)
        {

            try
            {

                SetEVSEBusyStatusResponse = new SetEVSEBusyStatusResponse(
                                                Request,
                                                SetEVSEBusyStatusResponseXML.MapValueOrFail("transactionId",  Transaction_Id.Parse),
                                                SetEVSEBusyStatusResponseXML.MapValueOrFail("requestStatus",  RequestStatus.Parse)
                                            );


                if (CustomSendSetEVSEBusyStatusResponseParser != null)
                    SetEVSEBusyStatusResponse = CustomSendSetEVSEBusyStatusResponseParser(SetEVSEBusyStatusResponseXML,
                                                                  SetEVSEBusyStatusResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetEVSEBusyStatusResponseXML, e);

                SetEVSEBusyStatusResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, SetEVSEBusyStatusResponseText, ..., out SetEVSEBusyStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a SetEVSEBusyStatus response.
        /// </summary>
        /// <param name="Request">The SetEVSEBusyStatus request leading to this response.</param>
        /// <param name="SetEVSEBusyStatusResponseText">The text to parse.</param>
        /// <param name="CustomSendSetEVSEBusyStatusResponseParser">An optional delegate to parse custom SetEVSEBusyStatusResponse XML elements.</param>
        /// <param name="SetEVSEBusyStatusResponse">The parsed SetEVSEBusyStatus response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetEVSEBusyStatusRequest                            Request,
                                       String                                              SetEVSEBusyStatusResponseText,
                                       CustomXMLParserDelegate<SetEVSEBusyStatusResponse>  CustomSendSetEVSEBusyStatusResponseParser,
                                       out SetEVSEBusyStatusResponse                       SetEVSEBusyStatusResponse,
                                       OnExceptionDelegate                                 OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(SetEVSEBusyStatusResponseText).Root,
                             CustomSendSetEVSEBusyStatusResponseParser,
                             out SetEVSEBusyStatusResponse,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SetEVSEBusyStatusResponseText, e);
            }

            SetEVSEBusyStatusResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetEVSEBusyStatusResponseSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetEVSEBusyStatusResponseSerializer">A delegate to serialize custom Heartbeat response XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetEVSEBusyStatusResponse> CustomSetEVSEBusyStatusResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.EVCIDynamic + "eMIP_ToIOP_SetEVSEBusyStatusResponse",

                          new XElement("transactionId",  TransactionId.ToString()),
                          new XElement("requestStatus",  RequestStatus.ToString())

                      );


            return CustomSetEVSEBusyStatusResponseSerializer != null
                       ? CustomSetEVSEBusyStatusResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetEVSEBusyStatusResponse1, SetEVSEBusyStatusResponse2)

        /// <summary>
        /// Compares two SetEVSEBusyStatus responses for equality.
        /// </summary>
        /// <param name="SetEVSEBusyStatusResponse1">A SetEVSEBusyStatus response.</param>
        /// <param name="SetEVSEBusyStatusResponse2">Another SetEVSEBusyStatus response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetEVSEBusyStatusResponse SetEVSEBusyStatusResponse1, SetEVSEBusyStatusResponse SetEVSEBusyStatusResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SetEVSEBusyStatusResponse1, SetEVSEBusyStatusResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SetEVSEBusyStatusResponse1 == null) || ((Object) SetEVSEBusyStatusResponse2 == null))
                return false;

            return SetEVSEBusyStatusResponse1.Equals(SetEVSEBusyStatusResponse2);

        }

        #endregion

        #region Operator != (SetEVSEBusyStatusResponse1, SetEVSEBusyStatusResponse2)

        /// <summary>
        /// Compares two SetEVSEBusyStatus responses for inequality.
        /// </summary>
        /// <param name="SetEVSEBusyStatusResponse1">A SetEVSEBusyStatus response.</param>
        /// <param name="SetEVSEBusyStatusResponse2">Another SetEVSEBusyStatus response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetEVSEBusyStatusResponse SetEVSEBusyStatusResponse1, SetEVSEBusyStatusResponse SetEVSEBusyStatusResponse2)
            => !(SetEVSEBusyStatusResponse1 == SetEVSEBusyStatusResponse2);

        #endregion

        #endregion

        #region IEquatable<SetEVSEBusyStatusResponse> Members

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

            var SetEVSEBusyStatusResponse = Object as SetEVSEBusyStatusResponse;
            if ((Object) SetEVSEBusyStatusResponse == null)
                return false;

            return Equals(SetEVSEBusyStatusResponse);

        }

        #endregion

        #region Equals(SetEVSEBusyStatusResponse)

        /// <summary>
        /// Compares two SetEVSEBusyStatus responses for equality.
        /// </summary>
        /// <param name="SetEVSEBusyStatusResponse">A SetEVSEBusyStatus response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetEVSEBusyStatusResponse SetEVSEBusyStatusResponse)
        {

            if ((Object) SetEVSEBusyStatusResponse == null)
                return false;

            return TransactionId.Equals(SetEVSEBusyStatusResponse.TransactionId) &&
                   RequestStatus.Equals(SetEVSEBusyStatusResponse.RequestStatus);

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
        /// A SetEVSEBusyStatus response builder.
        /// </summary>
        public class Builder : AResponseBuilder<SetEVSEBusyStatusRequest,
                                                SetEVSEBusyStatusResponse>
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

            #region Builder(Request,                   CustomData = null)

            /// <summary>
            /// Create a new SetEVSEBusyStatus response builder.
            /// </summary>
            /// <param name="Request">A SetEVSEBusyStatus request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetEVSEBusyStatusRequest             Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(SetEVSEBusyStatusResponse, CustomData = null)

            /// <summary>
            /// Create a new SetEVSEBusyStatus response builder.
            /// </summary>
            /// <param name="SetEVSEBusyStatusResponse">A SetEVSEBusyStatus response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetEVSEBusyStatusResponse            SetEVSEBusyStatusResponse  = null,
                           IReadOnlyDictionary<String, Object>  CustomData                 = null)

                : base(SetEVSEBusyStatusResponse?.Request,
                       SetEVSEBusyStatusResponse.HasCustomData
                           ? CustomData?.Count > 0
                                 ? SetEVSEBusyStatusResponse.CustomData.Concat(CustomData)
                                 : SetEVSEBusyStatusResponse.CustomData
                           : CustomData)

            {

                if (SetEVSEBusyStatusResponse != null)
                {
                    this.TransactionId  = SetEVSEBusyStatusResponse.TransactionId;
                    this.RequestStatus  = SetEVSEBusyStatusResponse.RequestStatus;
                }

            }

            #endregion

            #endregion


            #region Equals(SetEVSEBusyStatusResponse)

            /// <summary>
            /// Compares two SetEVSEBusyStatus responses for equality.
            /// </summary>
            /// <param name="SetEVSEBusyStatusResponse">A SetEVSEBusyStatus response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(SetEVSEBusyStatusResponse SetEVSEBusyStatusResponse)
            {

                if ((Object) SetEVSEBusyStatusResponse == null)
                    return false;

                return TransactionId.Equals(SetEVSEBusyStatusResponse.TransactionId) &&
                       RequestStatus.Equals(SetEVSEBusyStatusResponse.RequestStatus);

            }

            #endregion

            #region ToImmutable

            /// <summary>
            /// Return an immutable representation.
            /// </summary>
            public override SetEVSEBusyStatusResponse ToImmutable

                => new SetEVSEBusyStatusResponse(Request,
                                                 TransactionId,
                                                 RequestStatus);

            #endregion

        }

        #endregion


    }

}
