﻿/*
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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A SetSessionAction response.
    /// </summary>
    public class SetSessionActionResponse : AResponse<SetSessionActionRequest,
                                                      SetSessionActionResponse>
    {

        #region Constructor(s)

        /// <summary>
        /// Create a new SetSessionAction response.
        /// </summary>
        /// <param name="Request">The SetSessionAction request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// 
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public SetSessionActionResponse(SetSessionActionRequest              Request,
                                        Transaction_Id                       TransactionId,
                                        RequestStatus                        RequestStatus,

                                        HTTPResponse                         HTTPResponse   = null,
                                        IReadOnlyDictionary<String, Object>  CustomData     = null)

            : base(Request,
                   TransactionId,
                   RequestStatus,
                   HTTPResponse,
                   CustomData)

        { }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:aut  = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //    <soap:Header/>
        //    <soap:Body>
        //       <aut:eMIP_FromIOP_SetSessionActionResponse>
        //
        //          <transactionId>?</transactionId>
        //
        //          <!--       1: OK-Normal:  Normal successful completion! -->
        //          <!--     205: OK-Warning: The autorisation request is rejected by CPO: The requested service is not available on this EVSE! -->
        //          <!--     206: OK-Warning: The autorisation request is rejected by CPO: The EVSE is not technically reachable (communication)! -->
        //          <!--   10201: Ko-Error:   The autorisation request is rejected: Unknown error! -->
        //          <!--  <10000: OK:         Reserved for future use! -->
        //          <!-- >=10000: Ko-Error:   Reserved for future use! -->
        //          <requestStatus>?</requestStatus>
        //
        //       </aut:eMIP_FromIOP_SetSessionActionResponse>
        //    </soap:Body>
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, SetSessionActionResponseXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a SetSessionAction response.
        /// </summary>
        /// <param name="Request">The SetSessionAction request leading to this response.</param>
        /// <param name="SetSessionActionResponseXML">The XML to parse.</param>
        /// <param name="CustomSendSetSessionActionResponseParser">An optional delegate to parse custom SetSessionActionResponse XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetSessionActionResponse Parse(SetSessionActionRequest                            Request,
                                                     XElement                                           SetSessionActionResponseXML,
                                                     CustomXMLParserDelegate<SetSessionActionResponse>  CustomSendSetSessionActionResponseParser  = null,
                                                     HTTPResponse                                       HTTPResponse                              = null,
                                                     OnExceptionDelegate                                OnException                               = null)
        {

            if (TryParse(Request,
                         SetSessionActionResponseXML,
                         out SetSessionActionResponse SetSessionActionResponse,
                         CustomSendSetSessionActionResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return SetSessionActionResponse;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (Request, SetSessionActionResponseText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a SetSessionAction response.
        /// </summary>
        /// <param name="Request">The SetSessionAction request leading to this response.</param>
        /// <param name="SetSessionActionResponseText">The text to parse.</param>
        /// <param name="CustomSendSetSessionActionResponseParser">An optional delegate to parse custom SetSessionActionResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetSessionActionResponse Parse(SetSessionActionRequest                            Request,
                                                     String                                             SetSessionActionResponseText,
                                                     CustomXMLParserDelegate<SetSessionActionResponse>  CustomSendSetSessionActionResponseParser  = null,
                                                     HTTPResponse                                       HTTPResponse                              = null,
                                                     OnExceptionDelegate                                OnException                               = null)
        {

            if (TryParse(Request,
                         SetSessionActionResponseText,
                         out SetSessionActionResponse SetSessionActionResponse,
                         CustomSendSetSessionActionResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return SetSessionActionResponse;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Request, SetSessionActionResponseXML,  ..., out SetSessionActionResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a SetSessionAction response.
        /// </summary>
        /// <param name="Request">The SetSessionAction request leading to this response.</param>
        /// <param name="SetSessionActionResponseXML">The XML to parse.</param>
        /// <param name="SetSessionActionResponse">The parsed SetSessionAction response.</param>
        /// <param name="CustomSendSetSessionActionResponseParser">An optional delegate to parse custom SetSessionActionResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetSessionActionRequest                            Request,
                                       XElement                                           SetSessionActionResponseXML,
                                       out SetSessionActionResponse                       SetSessionActionResponse,
                                       CustomXMLParserDelegate<SetSessionActionResponse>  CustomSendSetSessionActionResponseParser  = null,
                                       HTTPResponse                                       HTTPResponse                              = null,
                                       OnExceptionDelegate                                OnException                               = null)
        {

            try
            {

                SetSessionActionResponse = new SetSessionActionResponse(

                                                      Request,

                                                      SetSessionActionResponseXML.MapValueOrFail("transactionId",  Transaction_Id.Parse),
                                                      SetSessionActionResponseXML.MapValueOrFail("requestStatus",  RequestStatus. Parse),

                                                      HTTPResponse

                                                  );


                if (CustomSendSetSessionActionResponseParser != null)
                    SetSessionActionResponse = CustomSendSetSessionActionResponseParser(SetSessionActionResponseXML,
                                                                                                      SetSessionActionResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetSessionActionResponseXML, e);

                SetSessionActionResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, SetSessionActionResponseText, ..., out SetSessionActionResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a SetSessionAction response.
        /// </summary>
        /// <param name="Request">The SetSessionAction request leading to this response.</param>
        /// <param name="SetSessionActionResponseText">The text to parse.</param>
        /// <param name="SetSessionActionResponse">The parsed SetSessionAction response.</param>
        /// <param name="CustomSendSetSessionActionResponseParser">An optional delegate to parse custom SetSessionActionResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetSessionActionRequest                            Request,
                                       String                                             SetSessionActionResponseText,
                                       out SetSessionActionResponse                       SetSessionActionResponse,
                                       CustomXMLParserDelegate<SetSessionActionResponse>  CustomSendSetSessionActionResponseParser  = null,
                                       HTTPResponse                                       HTTPResponse                              = null,
                                       OnExceptionDelegate                                OnException                               = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(SetSessionActionResponseText).Root,
                             out SetSessionActionResponse,
                             CustomSendSetSessionActionResponseParser,
                             HTTPResponse,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SetSessionActionResponseText, e);
            }

            SetSessionActionResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetSessionActionResponseSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetSessionActionResponseSerializer">A delegate to serialize custom Heartbeat response XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetSessionActionResponse> CustomSetSessionActionResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_ToIOP_SetSessionActionResponse",

                          new XElement("transactionId",  TransactionId.ToString()),
                          new XElement("requestStatus",  RequestStatus.ToString())

                      );


            return CustomSetSessionActionResponseSerializer != null
                       ? CustomSetSessionActionResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region (static) SystemError(Request, TransactionId, HTTPResponse = null, CustomData = null)

        /// <summary>
        /// Signal a system error.
        /// </summary>
        /// <param name="Request">The SetServiceAuthorisation request.</param>
        /// <param name="TransactionId">The transaction identification.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static SetSessionActionResponse SystemError(SetSessionActionRequest              Request,
                                                           Transaction_Id                       TransactionId,
                                                           HTTPResponse                         HTTPResponse   = null,
                                                           IReadOnlyDictionary<String, Object>  CustomData     = null)

            => new SetSessionActionResponse(Request,
                                            TransactionId,
                                            RequestStatus.SystemError,
                                            HTTPResponse,
                                            CustomData);

        #endregion


        #region Operator overloading

        #region Operator == (SetSessionActionResponse1, SetSessionActionResponse2)

        /// <summary>
        /// Compares two SetSessionAction responses for equality.
        /// </summary>
        /// <param name="SetSessionActionResponse1">A SetSessionAction response.</param>
        /// <param name="SetSessionActionResponse2">Another SetSessionAction response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetSessionActionResponse SetSessionActionResponse1, SetSessionActionResponse SetSessionActionResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SetSessionActionResponse1, SetSessionActionResponse2))
                return true;

            // If one is null, but not both, return false.
            if ((SetSessionActionResponse1 is null) || (SetSessionActionResponse2 is null))
                return false;

            return SetSessionActionResponse1.Equals(SetSessionActionResponse2);

        }

        #endregion

        #region Operator != (SetSessionActionResponse1, SetSessionActionResponse2)

        /// <summary>
        /// Compares two SetSessionAction responses for inequality.
        /// </summary>
        /// <param name="SetSessionActionResponse1">A SetSessionAction response.</param>
        /// <param name="SetSessionActionResponse2">Another SetSessionAction response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetSessionActionResponse SetSessionActionResponse1, SetSessionActionResponse SetSessionActionResponse2)
            => !(SetSessionActionResponse1 == SetSessionActionResponse2);

        #endregion

        #endregion

        #region IEquatable<SetSessionActionResponse> Members

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

            if (!(Object is SetSessionActionResponse SetSessionActionResponse))
                return false;

            return Equals(SetSessionActionResponse);

        }

        #endregion

        #region Equals(SetSessionActionResponse)

        /// <summary>
        /// Compares two SetSessionAction responses for equality.
        /// </summary>
        /// <param name="SetSessionActionResponse">A SetSessionAction response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetSessionActionResponse SetSessionActionResponse)
        {

            if (SetSessionActionResponse is null)
                return false;

            return TransactionId.Equals(SetSessionActionResponse.TransactionId) &&
                   RequestStatus.Equals(SetSessionActionResponse.RequestStatus);

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
        /// A SetSessionAction response builder.
        /// </summary>
        public class Builder : AResponseBuilder<SetSessionActionRequest,
                                                SetSessionActionResponse>
        {

            #region Constructor(s)

            #region Builder(Request,                  CustomData = null)

            /// <summary>
            /// Create a new SetSessionAction response builder.
            /// </summary>
            /// <param name="Request">A SetSessionAction request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetSessionActionRequest              Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(SetSessionActionResponse, CustomData = null)

            /// <summary>
            /// Create a new SetSessionAction response builder.
            /// </summary>
            /// <param name="SetSessionActionResponse">A SetSessionAction response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetSessionActionResponse             SetSessionActionResponse  = null,
                           IReadOnlyDictionary<String, Object>  CustomData                = null)

                : base(SetSessionActionResponse?.Request,
                       SetSessionActionResponse.HasCustomData
                           ? CustomData?.Count > 0
                                 ? SetSessionActionResponse.CustomData.Concat(CustomData)
                                 : SetSessionActionResponse.CustomData
                           : CustomData)

            {

                if (SetSessionActionResponse != null)
                {
                    this.TransactionId  = SetSessionActionResponse.TransactionId;
                    this.RequestStatus  = SetSessionActionResponse.RequestStatus;
                }

            }

            #endregion

            #endregion


            #region Equals(SetSessionActionResponse)

            /// <summary>
            /// Compares two SetSessionAction responses for equality.
            /// </summary>
            /// <param name="SetSessionActionResponse">A SetSessionAction response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(SetSessionActionResponse SetSessionActionResponse)
            {

                if (SetSessionActionResponse is null)
                    return false;

                return TransactionId.Equals(SetSessionActionResponse.TransactionId) &&
                       RequestStatus.Equals(SetSessionActionResponse.RequestStatus);

            }

            #endregion

            #region (implicit) "ToImmutable()"

            /// <summary>
            /// Return an immutable SetSessionActionResponse response.
            /// </summary>
            /// <param name="Builder">A SetSessionActionResponse response builder.</param>
            public static implicit operator SetSessionActionResponse(Builder Builder)

                => new SetSessionActionResponse(Builder.Request,
                                                Builder.TransactionId,
                                                Builder.RequestStatus);

            #endregion

        }

        #endregion

    }

}
