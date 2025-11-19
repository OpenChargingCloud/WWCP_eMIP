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

using System.Xml.Linq;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A SetServiceAuthorisation response.
    /// </summary>
    public class SetServiceAuthorisationResponse : AResponse<SetServiceAuthorisationRequest,
                                                             SetServiceAuthorisationResponse>
    {

        #region Properties

        /// <summary>
        /// The GIREVE partner session id for this service session.
        /// </summary>
        public PartnerServiceSession_Id?  PartnerServiceSessionId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new SetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The SetServiceAuthorisation request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// 
        /// <param name="PartnerServiceSessionId">The GIREVE session id for this service session.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public SetServiceAuthorisationResponse(SetServiceAuthorisationRequest  Request,
                                               Transaction_Id                  TransactionId,
                                               RequestStatus                   RequestStatus,

                                               PartnerServiceSession_Id?       PartnerServiceSessionId   = null,
                                               HTTPResponse?                   HTTPResponse              = null,
                                               JObject?                        CustomData                = null,
                                               UserDefinedDictionary?          InternalData              = null)

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


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:aut  = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //    <soap:Header/>
        //    <soap:Body>
        //       <aut:eMIP_FromIOP_SetServiceAuthorisationResponse>
        //
        //          <transactionId>?</transactionId>
        //
        //          <!--Optional:-->
        //          <partnerServiceSessionId>?</partnerServiceSessionId>
        //
        //          <!--       1: OK-Normal:  Normal successful completion! -->
        //          <!--     205: OK-Warning: The authorisation request is rejected by CPO: The requested service is not available on this EVSE! -->
        //          <!--     206: OK-Warning: The authorisation request is rejected by CPO: The EVSE is not technically reachable (communication)! -->
        //          <!--   10201: Ko-Error:   The authorisation request is rejected: Unknown error! -->
        //          <!--  <10000: OK:         Reserved for future use! -->
        //          <!-- >=10000: Ko-Error:   Reserved for future use! -->
        //          <requestStatus>?</requestStatus>
        //
        //       </aut:eMIP_FromIOP_SetServiceAuthorisationResponse>
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
        public static SetServiceAuthorisationResponse Parse(SetServiceAuthorisationRequest                             Request,
                                                            XElement                                                   SetServiceAuthorisationResponseXML,
                                                            CustomXMLParserDelegate<SetServiceAuthorisationResponse>?  CustomSendSetServiceAuthorisationResponseParser   = null,
                                                            HTTPResponse?                                              HTTPResponse                                      = null,
                                                            OnExceptionDelegate?                                       OnException                                       = null)
        {

            if (TryParse(Request,
                         SetServiceAuthorisationResponseXML,
                         out var setServiceAuthorisationResponse,
                         CustomSendSetServiceAuthorisationResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return setServiceAuthorisationResponse!;
            }

            throw new ArgumentException($"Invalid text representation of a SetServiceAuthorisation response: '{SetServiceAuthorisationResponseXML}'!",
                                        nameof(SetServiceAuthorisationResponseXML));

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
        public static Boolean TryParse(SetServiceAuthorisationRequest                             Request,
                                       XElement                                                   SetServiceAuthorisationResponseXML,
                                       out SetServiceAuthorisationResponse?                       SetServiceAuthorisationResponse,
                                       CustomXMLParserDelegate<SetServiceAuthorisationResponse>?  CustomSendSetServiceAuthorisationResponseParser   = null,
                                       HTTPResponse?                                              HTTPResponse                                      = null,
                                       OnExceptionDelegate?                                       OnException                                       = null)
        {

            try
            {

                SetServiceAuthorisationResponse = new SetServiceAuthorisationResponse(

                                                      Request,

                                                      SetServiceAuthorisationResponseXML.MapValueOrFail    ("transactionId",            Transaction_Id.          Parse),
                                                      SetServiceAuthorisationResponseXML.MapValueOrFail    ("requestStatus",            RequestStatus.           Parse),
                                                      SetServiceAuthorisationResponseXML.MapValueOrNullable("partnerServiceSessionId",  PartnerServiceSession_Id.Parse),

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

        #region ToXML(CustomSetServiceAuthorisationResponseSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetServiceAuthorisationResponseSerializer">A delegate to serialize custom Heartbeat response XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetServiceAuthorisationResponse>? CustomSetServiceAuthorisationResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_FromIOP_SetServiceAuthorisationResponse",

                          new XElement("transactionId",                   TransactionId.          ToString()),
                          new XElement("requestStatus",                   RequestStatus.Code.     ToString()),

                          PartnerServiceSessionId.HasValue
                              ? new XElement("partnerServiceSessionId",   PartnerServiceSessionId.ToString())
                              : null

                      );


            return CustomSetServiceAuthorisationResponseSerializer is not null
                       ? CustomSetServiceAuthorisationResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region (static) SystemError(Request, TransactionId, PartnerServiceSessionId = null, HTTPResponse = null, CustomData = null)

        /// <summary>
        /// Signal a system error.
        /// </summary>
        /// <param name="Request">The SetServiceAuthorisation request.</param>
        /// <param name="TransactionId">The transaction identification.</param>
        /// <param name="PartnerServiceSessionId">An optional partner service session identification.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static SetServiceAuthorisationResponse SystemError(SetServiceAuthorisationRequest  Request,
                                                                  Transaction_Id                  TransactionId,
                                                                  PartnerServiceSession_Id?       PartnerServiceSessionId   = null,
                                                                  HTTPResponse?                   HTTPResponse              = null,
                                                                  JObject?                        CustomData                = null,
                                                                  UserDefinedDictionary?          InternalData              = null)

            => new (Request,
                    TransactionId,
                    RequestStatus.SystemError,
                    PartnerServiceSessionId,
                    HTTPResponse,
                    CustomData,
                    InternalData);

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

            var SetServiceAuthorisationResponse = Object as SetServiceAuthorisationResponse;
            if ((Object) SetServiceAuthorisationResponse is null)
                return false;

            return Equals(SetServiceAuthorisationResponse);

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

            if ((Object) SetServiceAuthorisationResponse is null)
                return false;

            return TransactionId.Equals(SetServiceAuthorisationResponse.TransactionId) &&
                   RequestStatus.Equals(SetServiceAuthorisationResponse.RequestStatus) &&

                   ((!PartnerServiceSessionId.HasValue && !SetServiceAuthorisationResponse.PartnerServiceSessionId.HasValue) ||
                     (PartnerServiceSessionId.HasValue &&  SetServiceAuthorisationResponse.PartnerServiceSessionId.HasValue && PartnerServiceSessionId.Value.Equals(SetServiceAuthorisationResponse.PartnerServiceSessionId.Value)));

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

                return TransactionId.           GetHashCode() * 5 ^
                       RequestStatus.           GetHashCode() * 3 ^
                      (PartnerServiceSessionId?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{TransactionId} -> {RequestStatus}";

        #endregion


        #region ToBuilder

        /// <summary>
        /// Return a response builder.
        /// </summary>
        public Builder ToBuilder

            => new (this);

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
            /// The GIREVE partner session id for this service session.
            /// </summary>
            public PartnerServiceSession_Id?  PartnerServiceSessionId    { get; set; }

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
                    this.TransactionId            = SetServiceAuthorisationResponse.TransactionId;
                    this.RequestStatus            = SetServiceAuthorisationResponse.RequestStatus;
                    this.PartnerServiceSessionId  = SetServiceAuthorisationResponse.PartnerServiceSessionId;
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

                => SetServiceAuthorisationResponse is not null &&

                   TransactionId.Equals(SetServiceAuthorisationResponse.TransactionId) &&
                   RequestStatus.Equals(SetServiceAuthorisationResponse.RequestStatus) &&

                ((!PartnerServiceSessionId.HasValue && !SetServiceAuthorisationResponse.PartnerServiceSessionId.HasValue) ||
                  (PartnerServiceSessionId.HasValue &&  SetServiceAuthorisationResponse.PartnerServiceSessionId.HasValue && PartnerServiceSessionId.Value.Equals(SetServiceAuthorisationResponse.PartnerServiceSessionId.Value)));

            #endregion

            #region (implicit) "ToImmutable()"

            /// <summary>
            /// Return an immutable SetServiceAuthorisationResponse response.
            /// </summary>
            /// <param name="Builder">A SetServiceAuthorisationResponse response builder.</param>
            public static implicit operator SetServiceAuthorisationResponse(Builder Builder)

                => new (Builder.Request,
                        Builder.TransactionId,
                        Builder.RequestStatus,
                        Builder.PartnerServiceSessionId);

            #endregion

        }

        #endregion

    }

}
