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
    /// A SetServiceAuthorisation response.
    /// </summary>
    public class SetServiceAuthorisationResponse : AResponse<SetServiceAuthorisationRequest,
                                                   SetServiceAuthorisationResponse>
    {

        #region Properties

        /// <summary>
        /// The GIREVE session id for this service session.
        /// </summary>
        public ServiceSession_Id  ServiceSessionId         { get; }

        /// <summary>
        /// The operator identification of the executing operator.
        /// </summary>
        public Operator_Id?       ExecPartnerOperatorId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new SetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The SetServiceAuthorisation request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// <param name="ServiceSessionId">The GIREVE session id for this service session.</param>
        /// 
        /// <param name="ExecPartnerOperatorId">The operator identification of the executing operator.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public SetServiceAuthorisationResponse(SetServiceAuthorisationRequest  Request,
                                               Transaction_Id                  TransactionId,
                                               RequestStatus                   RequestStatus,
                                               ServiceSession_Id               ServiceSessionId,

                                               Operator_Id?                    ExecPartnerOperatorId   = null,
                                               HTTPResponse?                   HTTPResponse            = null,
                                               JObject?                        CustomData              = null,
                                               UserDefinedDictionary?          InternalData            = null)

            : base(Request,
                   TransactionId,
                   RequestStatus,
                   HTTPResponse,
                   CustomData,
                   InternalData)

        {

            this.ServiceSessionId       = ServiceSessionId;
            this.ExecPartnerOperatorId  = ExecPartnerOperatorId;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:aut  = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //    <soap:Header/>
        //    <soap:Body>
        //       <aut:eMIP_ToIOP_SetServiceAuthorisationResponse>
        //
        //          <transactionId>TRANSACTION_46151</transactionId>
        //
        //          <!--       1: OK-Normal:  Normal successful completion! -->
        //          <!--     205: OK-Warning: The authorisation request is rejected by CPO: The requested service is not available on this EVSE! -->
        //          <!--     206: OK-Warning: The authorisation request is rejected by CPO: The EVSE is not technically reachable (communication)! -->
        //          <!--   10201: Ko-Error:   The authorisation request is rejected: Unknown error! -->
        //          <!--  <10000: OK:         Reserved for future use! -->
        //          <!-- >=10000: Ko-Error:   Reserved for future use! -->
        //          <requestStatus>?</requestStatus>
        //
        //          <serviceSessionId>IOP-SID-GIR-V-IOPFT01-0dc6fc3...153e</serviceSessionId>
        //
        //          <!--Optional:-->
        //          <execPartnerOperatorIdType>eMI3</execPartnerOperatorIdType>
        //          <!--Optional:-->
        //          <execPartnerOperatorId>FR* CPO</execPartnerOperatorId>
        //
        //       </aut:eMIP_ToIOP_SetServiceAuthorisationResponse>
        //    </soap:Body>
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, SetServiceAuthorisationResponseXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a SetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The SetServiceAuthorisation request leading to this response.</param>
        /// <param name="SetServiceAuthorisationResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetServiceAuthorisationResponseParser">An optional delegate to parse custom SetServiceAuthorisationResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static SetServiceAuthorisationResponse Parse(SetServiceAuthorisationRequest                            Request,
                                                            XElement                                                  SetServiceAuthorisationResponseXML,
                                                            CustomXMLParserDelegate<SetServiceAuthorisationResponse>  CustomSendSetServiceAuthorisationResponseParser   = null,
                                                            HTTPResponse                                              HTTPResponse                                      = null,
                                                            OnExceptionDelegate                                       OnException                                       = null)
        {

            if (TryParse(Request,
                         SetServiceAuthorisationResponseXML,
                         out SetServiceAuthorisationResponse SetServiceAuthorisationResponse,
                         CustomSendSetServiceAuthorisationResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return SetServiceAuthorisationResponse;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (Request, SetServiceAuthorisationResponseText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a SetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The SetServiceAuthorisation request leading to this response.</param>
        /// <param name="SetServiceAuthorisationResponseText">The text to parse.</param>
        /// <param name="CustomSendSetServiceAuthorisationResponseParser">An optional delegate to parse custom SetServiceAuthorisationResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static SetServiceAuthorisationResponse Parse(SetServiceAuthorisationRequest                            Request,
                                                            String                                                    SetServiceAuthorisationResponseText,
                                                            CustomXMLParserDelegate<SetServiceAuthorisationResponse>  CustomSendSetServiceAuthorisationResponseParser   = null,
                                                            HTTPResponse                                              HTTPResponse                                      = null,
                                                            OnExceptionDelegate                                       OnException                                       = null)
        {

            if (TryParse(Request,
                         SetServiceAuthorisationResponseText,
                         out SetServiceAuthorisationResponse SetServiceAuthorisationResponse,
                         CustomSendSetServiceAuthorisationResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return SetServiceAuthorisationResponse;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Request, SetServiceAuthorisationResponseXML,  ..., out SetServiceAuthorisationResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a SetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The SetServiceAuthorisation request leading to this response.</param>
        /// <param name="SetServiceAuthorisationResponseXML">The XML to parse.</param>
        /// <param name="SetServiceAuthorisationResponse">The parsed SetServiceAuthorisation response.</param>
        /// <param name="CustomSendSetServiceAuthorisationResponseParser">An optional delegate to parse custom SetServiceAuthorisationResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static Boolean TryParse(SetServiceAuthorisationRequest                            Request,
                                       XElement                                                  SetServiceAuthorisationResponseXML,
                                       out SetServiceAuthorisationResponse                       SetServiceAuthorisationResponse,
                                       CustomXMLParserDelegate<SetServiceAuthorisationResponse>  CustomSendSetServiceAuthorisationResponseParser   = null,
                                       HTTPResponse                                              HTTPResponse                                      = null,
                                       OnExceptionDelegate                                       OnException                                       = null)
        {

            try
            {

                SetServiceAuthorisationResponse = new SetServiceAuthorisationResponse(

                                                      Request,

                                                      SetServiceAuthorisationResponseXML.MapValueOrFail    ("transactionId",          Transaction_Id.   Parse),
                                                      SetServiceAuthorisationResponseXML.MapValueOrFail    ("requestStatus",          RequestStatus.    Parse),
                                                      SetServiceAuthorisationResponseXML.MapValueOrFail    ("serviceSessionId",       ServiceSession_Id.Parse),
                                                      SetServiceAuthorisationResponseXML.MapValueOrNullable("execPartnerOperatorId",  Operator_Id.      Parse),

                                                      HTTPResponse

                                                  );


                if (CustomSendSetServiceAuthorisationResponseParser is not null)
                    SetServiceAuthorisationResponse = CustomSendSetServiceAuthorisationResponseParser(SetServiceAuthorisationResponseXML,
                                                                                                      SetServiceAuthorisationResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, SetServiceAuthorisationResponseXML, e);

                SetServiceAuthorisationResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, SetServiceAuthorisationResponseText, ..., out SetServiceAuthorisationResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a SetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The SetServiceAuthorisation request leading to this response.</param>
        /// <param name="SetServiceAuthorisationResponseText">The text to parse.</param>
        /// <param name="CustomSendSetServiceAuthorisationResponseParser">An optional delegate to parse custom SetServiceAuthorisationResponse XML elements.</param>
        /// <param name="SetServiceAuthorisationResponse">The parsed SetServiceAuthorisation response.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static Boolean TryParse(SetServiceAuthorisationRequest                            Request,
                                       String                                                    SetServiceAuthorisationResponseText,
                                       out SetServiceAuthorisationResponse                       SetServiceAuthorisationResponse,
                                       CustomXMLParserDelegate<SetServiceAuthorisationResponse>  CustomSendSetServiceAuthorisationResponseParser   = null,
                                       HTTPResponse                                              HTTPResponse                                      = null,
                                       OnExceptionDelegate                                       OnException                                       = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(SetServiceAuthorisationResponseText).Root,
                             out SetServiceAuthorisationResponse,
                             CustomSendSetServiceAuthorisationResponseParser,
                             HTTPResponse,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, SetServiceAuthorisationResponseText, e);
            }

            SetServiceAuthorisationResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetServiceAuthorisationResponseSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetServiceAuthorisationResponseSerializer">A delegate to serialize custom Heartbeat response XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetServiceAuthorisationResponse> CustomSetServiceAuthorisationResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_ToIOP_SetServiceAuthorisationResponse",

                          new XElement("transactionId",                TransactionId.     ToString()),
                          new XElement("requestStatus",                RequestStatus.Code.ToString()),
                          new XElement("serviceSessionId",             ServiceSessionId.  ToString()),

                          ExecPartnerOperatorId.HasValue
                              ? new XElement("execPartnerOperatorIdType",  ExecPartnerOperatorId.Value.Format.AsText())
                              : null,

                          ExecPartnerOperatorId.HasValue
                              ? new XElement("execPartnerOperatorId",      ExecPartnerOperatorId.             ToString())
                              : null

                      );


            return CustomSetServiceAuthorisationResponseSerializer is not null
                       ? CustomSetServiceAuthorisationResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetServiceAuthorisationResponse1, SetServiceAuthorisationResponse2)

        /// <summary>
        /// Compares two SetServiceAuthorisation responses for equality.
        /// </summary>
        /// <param name="SetServiceAuthorisationResponse1">A SetServiceAuthorisation response.</param>
        /// <param name="SetServiceAuthorisationResponse2">Another SetServiceAuthorisation response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetServiceAuthorisationResponse SetServiceAuthorisationResponse1, SetServiceAuthorisationResponse SetServiceAuthorisationResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetServiceAuthorisationResponse1, SetServiceAuthorisationResponse2))
                return true;

            // If one is null, but not both, return false.
            if (SetServiceAuthorisationResponse1 is null || SetServiceAuthorisationResponse2 is null)
                return false;

            return SetServiceAuthorisationResponse1.Equals(SetServiceAuthorisationResponse2);

        }

        #endregion

        #region Operator != (SetServiceAuthorisationResponse1, SetServiceAuthorisationResponse2)

        /// <summary>
        /// Compares two SetServiceAuthorisation responses for inequality.
        /// </summary>
        /// <param name="SetServiceAuthorisationResponse1">A SetServiceAuthorisation response.</param>
        /// <param name="SetServiceAuthorisationResponse2">Another SetServiceAuthorisation response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetServiceAuthorisationResponse SetServiceAuthorisationResponse1, SetServiceAuthorisationResponse SetServiceAuthorisationResponse2)
            => !(SetServiceAuthorisationResponse1 == SetServiceAuthorisationResponse2);

        #endregion

        #endregion

        #region IEquatable<SetServiceAuthorisationResponse> Members

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

            if (!(Object is SetServiceAuthorisationResponse))
                return false;

            return Equals(Object as SetServiceAuthorisationResponse);

        }

        #endregion

        #region Equals(SetServiceAuthorisationResponse)

        /// <summary>
        /// Compares two SetServiceAuthorisation responses for equality.
        /// </summary>
        /// <param name="SetServiceAuthorisationResponse">A SetServiceAuthorisation response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetServiceAuthorisationResponse SetServiceAuthorisationResponse)
        {

            if (SetServiceAuthorisationResponse is null)
                return false;

            return TransactionId.   Equals(SetServiceAuthorisationResponse.TransactionId)    &&
                   RequestStatus.   Equals(SetServiceAuthorisationResponse.RequestStatus)    &&
                   ServiceSessionId.Equals(SetServiceAuthorisationResponse.ServiceSessionId) &&

                   ((!ExecPartnerOperatorId.HasValue && !SetServiceAuthorisationResponse.ExecPartnerOperatorId.HasValue) ||
                     (ExecPartnerOperatorId.HasValue &&  SetServiceAuthorisationResponse.ExecPartnerOperatorId.HasValue && ExecPartnerOperatorId.Value.Equals(SetServiceAuthorisationResponse.ExecPartnerOperatorId.Value)));

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

                return TransactionId.   GetHashCode() * 7 ^
                       RequestStatus.   GetHashCode() * 5 ^
                       ServiceSessionId.GetHashCode() * 3 ^

                       (ExecPartnerOperatorId.HasValue
                            ? ExecPartnerOperatorId.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(TransactionId,
                             " -> ",
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
        /// A SetServiceAuthorisation response builder.
        /// </summary>
        public new class Builder : AResponseBuilder<SetServiceAuthorisationRequest,
                                                    SetServiceAuthorisationResponse>
        {

            #region Properties

            /// <summary>
            /// The GIREVE session id for this service session.
            /// </summary>
            public ServiceSession_Id  ServiceSessionId         { get; set; }

            /// <summary>
            /// The operator identification of the executing operator.
            /// </summary>
            public Operator_Id?       ExecPartnerOperatorId    { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,                         CustomData = null)

            /// <summary>
            /// Create a new SetServiceAuthorisation response builder.
            /// </summary>
            /// <param name="Request">A SetServiceAuthorisation request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetServiceAuthorisationRequest  Request,
                           JObject?                        CustomData     = null,
                           UserDefinedDictionary?          InternalData   = null)

                : base(Request,
                       CustomData,
                       InternalData)

            { }

            #endregion

            #region Builder(SetServiceAuthorisationResponse, CustomData = null)

            /// <summary>
            /// Create a new SetServiceAuthorisation response builder.
            /// </summary>
            /// <param name="SetServiceAuthorisationResponse">A SetServiceAuthorisation response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetServiceAuthorisationResponse?  SetServiceAuthorisationResponse   = null,
                           JObject?                          CustomData                        = null,
                           UserDefinedDictionary?            InternalData                      = null)

                : base(SetServiceAuthorisationResponse?.Request,
                       CustomData,
                       SetServiceAuthorisationResponse.IsNotEmpty
                           ? InternalData.IsNotEmpty
                                 ? new UserDefinedDictionary(SetServiceAuthorisationResponse.InternalData.Concat(InternalData))
                                 : new UserDefinedDictionary(SetServiceAuthorisationResponse.InternalData)
                           : InternalData)

            {

                if (SetServiceAuthorisationResponse is not null)
                {
                    this.TransactionId          = SetServiceAuthorisationResponse.TransactionId;
                    this.RequestStatus          = SetServiceAuthorisationResponse.RequestStatus;
                    this.ServiceSessionId       = SetServiceAuthorisationResponse.ServiceSessionId;
                    this.ExecPartnerOperatorId  = SetServiceAuthorisationResponse.ExecPartnerOperatorId;
                }

            }

            #endregion

            #endregion


            #region Equals(SetServiceAuthorisationResponse)

            /// <summary>
            /// Compares two SetServiceAuthorisation responses for equality.
            /// </summary>
            /// <param name="SetServiceAuthorisationResponse">A SetServiceAuthorisation response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(SetServiceAuthorisationResponse SetServiceAuthorisationResponse)
            {

                if (SetServiceAuthorisationResponse is null)
                    return false;

                return TransactionId.   Equals(SetServiceAuthorisationResponse.TransactionId)    &&
                       RequestStatus.   Equals(SetServiceAuthorisationResponse.RequestStatus)    &&
                       ServiceSessionId.Equals(SetServiceAuthorisationResponse.ServiceSessionId) &&

                       ((!ExecPartnerOperatorId.HasValue && !SetServiceAuthorisationResponse.ExecPartnerOperatorId.HasValue) ||
                         (ExecPartnerOperatorId.HasValue &&  SetServiceAuthorisationResponse.ExecPartnerOperatorId.HasValue && ExecPartnerOperatorId.Value.Equals(SetServiceAuthorisationResponse.ExecPartnerOperatorId.Value)));

            }

            #endregion

            #region (implicit) "ToImmutable()"

            /// <summary>
            /// Return an immutable SetServiceAuthorisationResponse response.
            /// </summary>
            /// <param name="Builder">A SetServiceAuthorisationResponse response builder.</param>
            public static implicit operator SetServiceAuthorisationResponse(Builder Builder)

                => new SetServiceAuthorisationResponse(Builder.Request,
                                                       Builder.TransactionId,
                                                       Builder.RequestStatus,
                                                       Builder.ServiceSessionId,
                                                       Builder.ExecPartnerOperatorId);

            #endregion

        }

        #endregion

    }

}
