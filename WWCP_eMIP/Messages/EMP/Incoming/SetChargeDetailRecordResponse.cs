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
    /// A SetChargeDetailRecord response.
    /// </summary>
    public class SetChargeDetailRecordResponse : AResponse<SetChargeDetailRecordRequest,
                                                           SetChargeDetailRecordResponse>
    {

        #region Properties

        /// <summary>
        /// An optional partner service session identification.
        /// </summary>
        public PartnerServiceSession_Id?  PartnerServiceSessionId    { get; }

        #endregion

        #region Constructor(s)

        #region SetChargeDetailRecordResponse(Request, TransactionId, RequestStatus, ...)

        /// <summary>
        /// Create a new SetChargeDetailRecord response.
        /// </summary>
        /// <param name="Request">The SetChargeDetailRecord request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// 
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public SetChargeDetailRecordResponse(SetChargeDetailRecordRequest  Request,
                                             Transaction_Id                TransactionId,
                                             RequestStatus                 RequestStatus,

                                             HTTPResponse?                 HTTPResponse   = null,
                                             JObject?                      CustomData     = null,
                                             UserDefinedDictionary?        InternalData   = null)

            : this(Request,
                   TransactionId,
                   RequestStatus,
                   null,
                   HTTPResponse,
                   CustomData,
                   InternalData)

        { }

        #endregion

        #region SetChargeDetailRecordResponse(Request, TransactionId, RequestStatus, ...)

        /// <summary>
        /// Create a new SetChargeDetailRecord response.
        /// </summary>
        /// <param name="Request">The SetChargeDetailRecord request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// 
        /// <param name="PartnerServiceSessionId">An optional partner service session identification.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public SetChargeDetailRecordResponse(SetChargeDetailRecordRequest  Request,
                                             Transaction_Id                TransactionId,
                                             RequestStatus                 RequestStatus,

                                             PartnerServiceSession_Id?     PartnerServiceSessionId   = null,
                                             HTTPResponse?                 HTTPResponse              = null,
                                             JObject?                      CustomData                = null,
                                             UserDefinedDictionary?        InternalData              = null)

            : base(Request,
                   TransactionId,
                   RequestStatus,
                   HTTPResponse,
                   CustomData,
                   InternalData)

        {

            this.PartnerServiceSessionId  = PartnerServiceSessionId;

        }

        #endregion

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //
        //    <soap:Header/>
        //
        //    <soap:Body>
        //       <eMIP:eMIP_FromIOP_SetChargeDetailRecordResponse>
        //
        //          <transactionId>IOP-TID-GIR-V-IOPPPFT02-8531144d-fb53-43ef-9daa-671bb9447a9d</transactionId>
        //          <requestStatus>1</requestStatus>
        //
        //          <!--Optional:-->
        //          <partnerServiceSessionId>?</partnerServiceSessionId>
        //
        //       </eMIP:eMIP_FromIOP_SetChargeDetailRecordResponse>
        //    </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, SetChargeDetailRecordResponseXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a SetChargeDetailRecord response.
        /// </summary>
        /// <param name="Request">The SetChargeDetailRecord request leading to this response.</param>
        /// <param name="SetChargeDetailRecordResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargeDetailRecordResponseParser">An optional delegate to parse custom SetChargeDetailRecordResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetChargeDetailRecordResponse Parse(SetChargeDetailRecordRequest                            Request,
                                                          XElement                                                SetChargeDetailRecordResponseXML,
                                                          CustomXMLParserDelegate<SetChargeDetailRecordResponse>  CustomSendSetChargeDetailRecordResponseParser   = null,
                                                          HTTPResponse                                            HTTPResponse                                    = null,
                                                          OnExceptionDelegate                                     OnException                                     = null)
        {

            if (TryParse(Request,
                         SetChargeDetailRecordResponseXML,
                         out SetChargeDetailRecordResponse SetChargeDetailRecordResponse,
                         CustomSendSetChargeDetailRecordResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return SetChargeDetailRecordResponse;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (Request, SetChargeDetailRecordResponseText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a SetChargeDetailRecord response.
        /// </summary>
        /// <param name="Request">The SetChargeDetailRecord request leading to this response.</param>
        /// <param name="SetChargeDetailRecordResponseText">The text to parse.</param>
        /// <param name="CustomSendSetChargeDetailRecordResponseParser">An optional delegate to parse custom SetChargeDetailRecordResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetChargeDetailRecordResponse Parse(SetChargeDetailRecordRequest                            Request,
                                                          String                                                  SetChargeDetailRecordResponseText,
                                                          CustomXMLParserDelegate<SetChargeDetailRecordResponse>  CustomSendSetChargeDetailRecordResponseParser   = null,
                                                          HTTPResponse                                            HTTPResponse                                    = null,
                                                          OnExceptionDelegate                                     OnException                                     = null)
        {

            if (TryParse(Request,
                         SetChargeDetailRecordResponseText,
                         out SetChargeDetailRecordResponse SetChargeDetailRecordResponse,
                         CustomSendSetChargeDetailRecordResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return SetChargeDetailRecordResponse;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Request, SetChargeDetailRecordResponseXML,  ..., out SetChargeDetailRecordResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a SetChargeDetailRecord response.
        /// </summary>
        /// <param name="Request">The SetChargeDetailRecord request leading to this response.</param>
        /// <param name="SetChargeDetailRecordResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargeDetailRecordResponseParser">An optional delegate to parse custom SetChargeDetailRecordResponse XML elements.</param>
        /// <param name="SetChargeDetailRecordResponse">The parsed SetChargeDetailRecord response.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetChargeDetailRecordRequest                            Request,
                                       XElement                                                SetChargeDetailRecordResponseXML,
                                       out SetChargeDetailRecordResponse                       SetChargeDetailRecordResponse,
                                       CustomXMLParserDelegate<SetChargeDetailRecordResponse>  CustomSendSetChargeDetailRecordResponseParser   = null,
                                       HTTPResponse                                            HTTPResponse                                    = null,
                                       OnExceptionDelegate                                     OnException                                     = null)
        {

            try
            {

                SetChargeDetailRecordResponse = new SetChargeDetailRecordResponse(

                                                        Request,

                                                        SetChargeDetailRecordResponseXML.MapValueOrFail    ("transactionId",            Transaction_Id.Parse),
                                                        SetChargeDetailRecordResponseXML.MapValueOrFail    ("requestStatus",            RequestStatus.Parse),
                                                        SetChargeDetailRecordResponseXML.MapValueOrNullable("partnerServiceSessionId",  PartnerServiceSession_Id.Parse),

                                                        HTTPResponse

                                                    );


                if (CustomSendSetChargeDetailRecordResponseParser != null)
                    SetChargeDetailRecordResponse = CustomSendSetChargeDetailRecordResponseParser(SetChargeDetailRecordResponseXML,
                                                                                                  SetChargeDetailRecordResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetChargeDetailRecordResponseXML, e);

                SetChargeDetailRecordResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, SetChargeDetailRecordResponseText, ..., out SetChargeDetailRecordResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a SetChargeDetailRecord response.
        /// </summary>
        /// <param name="Request">The SetChargeDetailRecord request leading to this response.</param>
        /// <param name="SetChargeDetailRecordResponseText">The text to parse.</param>
        /// <param name="SetChargeDetailRecordResponse">The parsed SetChargeDetailRecord response.</param>
        /// <param name="CustomSendSetChargeDetailRecordResponseParser">An optional delegate to parse custom SetChargeDetailRecordResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetChargeDetailRecordRequest                            Request,
                                       String                                                  SetChargeDetailRecordResponseText,
                                       out SetChargeDetailRecordResponse                       SetChargeDetailRecordResponse,
                                       CustomXMLParserDelegate<SetChargeDetailRecordResponse>  CustomSendSetChargeDetailRecordResponseParser   = null,
                                       HTTPResponse                                            HTTPResponse                                    = null,
                                       OnExceptionDelegate                                     OnException                                     = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(SetChargeDetailRecordResponseText).Root,
                             out SetChargeDetailRecordResponse,
                             CustomSendSetChargeDetailRecordResponseParser,
                             HTTPResponse,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SetChargeDetailRecordResponseText, e);
            }

            SetChargeDetailRecordResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetChargeDetailRecordResponseSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetChargeDetailRecordResponseSerializer">A delegate to serialize custom SetChargeDetailRecord response XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetChargeDetailRecordResponse> CustomSetChargeDetailRecordResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_FromIOP_SetChargeDetailRecordResponse",

                          new XElement("transactionId",  TransactionId.ToString()),
                          new XElement("requestStatus",  RequestStatus.ToString()),

                          PartnerServiceSessionId.HasValue
                              ? new XElement("partnerServiceSessionId",  PartnerServiceSessionId.Value.ToString())
                              : null

                      );


            return CustomSetChargeDetailRecordResponseSerializer != null
                       ? CustomSetChargeDetailRecordResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetChargeDetailRecordResponse1, SetChargeDetailRecordResponse2)

        /// <summary>
        /// Compares two SetChargeDetailRecord responses for equality.
        /// </summary>
        /// <param name="SetChargeDetailRecordResponse1">A SetChargeDetailRecord response.</param>
        /// <param name="SetChargeDetailRecordResponse2">Another SetChargeDetailRecord response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetChargeDetailRecordResponse SetChargeDetailRecordResponse1, SetChargeDetailRecordResponse SetChargeDetailRecordResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetChargeDetailRecordResponse1, SetChargeDetailRecordResponse2))
                return true;

            // If one is null, but not both, return false.
            if ((SetChargeDetailRecordResponse1 is null) || (SetChargeDetailRecordResponse2 is null))
                return false;

            return SetChargeDetailRecordResponse1.Equals(SetChargeDetailRecordResponse2);

        }

        #endregion

        #region Operator != (SetChargeDetailRecordResponse1, SetChargeDetailRecordResponse2)

        /// <summary>
        /// Compares two SetChargeDetailRecord responses for inequality.
        /// </summary>
        /// <param name="SetChargeDetailRecordResponse1">A SetChargeDetailRecord response.</param>
        /// <param name="SetChargeDetailRecordResponse2">Another SetChargeDetailRecord response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetChargeDetailRecordResponse SetChargeDetailRecordResponse1, SetChargeDetailRecordResponse SetChargeDetailRecordResponse2)

            => !(SetChargeDetailRecordResponse1 == SetChargeDetailRecordResponse2);

        #endregion

        #endregion

        #region IEquatable<SetChargeDetailRecordResponse> Members

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

            if (!(Object is SetChargeDetailRecordResponse SetChargeDetailRecordResponse))
                return false;

            return Equals(SetChargeDetailRecordResponse);

        }

        #endregion

        #region Equals(SetChargeDetailRecordResponse)

        /// <summary>
        /// Compares two SetChargeDetailRecord responses for equality.
        /// </summary>
        /// <param name="SetChargeDetailRecordResponse">A SetChargeDetailRecord response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetChargeDetailRecordResponse SetChargeDetailRecordResponse)
        {

            if ((Object) SetChargeDetailRecordResponse == null)
                return false;

            return TransactionId.Equals(SetChargeDetailRecordResponse.TransactionId) &&
                   RequestStatus.Equals(SetChargeDetailRecordResponse.RequestStatus) &&

                   ((!PartnerServiceSessionId.HasValue && !SetChargeDetailRecordResponse.PartnerServiceSessionId.HasValue) ||
                     (PartnerServiceSessionId.HasValue &&  SetChargeDetailRecordResponse.PartnerServiceSessionId.HasValue && PartnerServiceSessionId.Value.Equals(SetChargeDetailRecordResponse.PartnerServiceSessionId.Value)));

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

                return TransactionId.GetHashCode() * 5 ^
                       RequestStatus.GetHashCode() * 3 ^

                       (PartnerServiceSessionId.HasValue
                            ? PartnerServiceSessionId.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(TransactionId,    ", ",
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
        /// A SetChargeDetailRecord response builder.
        /// </summary>
        public new class Builder : AResponseBuilder<SetChargeDetailRecordRequest,
                                                    SetChargeDetailRecordResponse>
        {

            #region Properties

            /// <summary>
            /// An optional partner service session identification.
            /// </summary>
            public PartnerServiceSession_Id?  PartnerServiceSessionId    { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,                       CustomData = null)

            /// <summary>
            /// Create a new SetChargeDetailRecord response builder.
            /// </summary>
            /// <param name="Request">A SetChargeDetailRecord request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetChargeDetailRecordRequest  Request,
                           JObject?                      CustomData     = null,
                           UserDefinedDictionary?        InternalData   = null)

                : base(Request,
                       CustomData,
                       InternalData)

            { }

            #endregion

            #region Builder(SetChargeDetailRecordResponse, CustomData = null)

            /// <summary>
            /// Create a new SetChargeDetailRecord response builder.
            /// </summary>
            /// <param name="SetChargeDetailRecordResponse">A SetChargeDetailRecord response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetChargeDetailRecordResponse?  SetChargeDetailRecordResponse   = null,
                           JObject?                        CustomData                      = null,
                           UserDefinedDictionary?          InternalData                    = null)

                : base(SetChargeDetailRecordResponse?.Request,
                       CustomData,
                       SetChargeDetailRecordResponse.IsNotEmpty
                           ? InternalData.IsNotEmpty
                                 ? new UserDefinedDictionary(SetChargeDetailRecordResponse.InternalData.Concat(InternalData))
                                 : new UserDefinedDictionary(SetChargeDetailRecordResponse.InternalData)
                           : InternalData)

            {

                if (SetChargeDetailRecordResponse is not null)
                {

                    this.TransactionId            = SetChargeDetailRecordResponse.TransactionId;
                    this.RequestStatus            = SetChargeDetailRecordResponse.RequestStatus;
                    this.PartnerServiceSessionId  = SetChargeDetailRecordResponse.PartnerServiceSessionId;

                }

            }

            #endregion

            #endregion


            #region Equals(SetChargeDetailRecordResponse)

            /// <summary>
            /// Compares two SetChargeDetailRecord responses for equality.
            /// </summary>
            /// <param name="SetChargeDetailRecordResponse">A SetChargeDetailRecord response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(SetChargeDetailRecordResponse SetChargeDetailRecordResponse)
            {

                if (SetChargeDetailRecordResponse is null)
                    return false;

                return TransactionId.   Equals(SetChargeDetailRecordResponse.TransactionId)    &&
                       RequestStatus.   Equals(SetChargeDetailRecordResponse.RequestStatus)    &&

                       ((!PartnerServiceSessionId.HasValue && !SetChargeDetailRecordResponse.PartnerServiceSessionId.HasValue) ||
                         (PartnerServiceSessionId.HasValue &&  SetChargeDetailRecordResponse.PartnerServiceSessionId.HasValue && PartnerServiceSessionId.Value.Equals(SetChargeDetailRecordResponse.PartnerServiceSessionId.Value)));

            }

            #endregion

            #region (implicit) "ToImmutable()"

            /// <summary>
            /// Return an immutable SetChargeDetailRecordResponse response.
            /// </summary>
            /// <param name="Builder">A SetChargeDetailRecordResponse response builder.</param>
            public static implicit operator SetChargeDetailRecordResponse(Builder Builder)

                => new SetChargeDetailRecordResponse(Builder.Request,
                                                     Builder.TransactionId,
                                                     Builder.RequestStatus,
                                                     Builder.PartnerServiceSessionId);

            #endregion

        }

        #endregion

    }

}
