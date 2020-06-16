/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
    /// A SetSessionEventReport response.
    /// </summary>
    public class SetSessionEventReportResponse : AResponse<SetSessionEventReportRequest,
                                                           SetSessionEventReportResponse>
    {

        #region Properties

        /// <summary>
        /// The service session identification.
        /// </summary>
        public ServiceSession_Id  ServiceSessionId    { get; }

        /// <summary>
        /// The unique identification of the session action.
        /// </summary>
        public SessionAction_Id   SessionActionId     { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new SetSessionEventReport response.
        /// </summary>
        /// <param name="Request">The SetSessionEventReport request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// <param name="ServiceSessionId">The service session identification.</param>
        /// <param name="SessionActionId">The unique identification of the session action.</param>
        /// 
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public SetSessionEventReportResponse(SetSessionEventReportRequest         Request,
                                             Transaction_Id                       TransactionId,
                                             RequestStatus                        RequestStatus,
                                             ServiceSession_Id                    ServiceSessionId,
                                             SessionAction_Id                     SessionActionId,

                                             HTTPResponse                         HTTPResponse   = null,
                                             IReadOnlyDictionary<String, Object>  CustomData     = null)

            : base(Request,
                   TransactionId,
                   RequestStatus,
                   HTTPResponse,
                   CustomData)

        {

            this.ServiceSessionId  = ServiceSessionId;
            this.SessionActionId   = SessionActionId;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:aut  = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //    <soap:Header/>
        //    <soap:Body>
        //       <aut:eMIP_ToIOP_SetSessionEventReportResponse>
        //
        //          <transactionId>?</transactionId>
        //
        //          <serviceSessionId>IOP-SID-GIR-V-IOPFT01-0dc6fc3...153e</serviceSessionId>
        //          <sessionActionId>00969e30-78a0-435e-a368-ea50ef20e878</sessionActionId>
        //
        //          <!--       1: Ok-Normal  Normal successful completion! -->
        //          <!--   10501: Ko-Error   session not found
        //          <!--   10502: Ko-Error   CPO/eMSP not found
        //          <!--   10503: Ko-Error   the CPO/eMSP does not accept Action/Event
        //          <!--   10504: Ko-Error   the request cannot be sent to the CPO/eMSP or the CPO/eMSP does not respond
        //          <!--   10505: Ko-Error   the CPO/eMSP returns an IOP Fault 
        //          <!--   10506: Ko-Error   the CPO/eMSP doesn't recognise the actionNature/eventNature: No action on its side
        //          <!--   10507: Ko-Error   the CPO/eMSP returns an error code: an error occured on its side during the action/report treatment 
        //          <!--   10508: Ko-Error   The requestor is neither eMSP nor CPO for this session.
        //          <!--  <10000: OK:         Reserved for future use! -->
        //          <!-- >=10000: Ko-Error:   Reserved for future use! -->
        //          <requestStatus>?</requestStatus>
        //
        //       </aut:eMIP_ToIOP_SetSessionEventReportResponse>
        //    </soap:Body>
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, SetSessionEventReportResponseXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a SetSessionEventReport response.
        /// </summary>
        /// <param name="Request">The SetSessionEventReport request leading to this response.</param>
        /// <param name="SetSessionEventReportResponseXML">The XML to parse.</param>
        /// <param name="CustomSetSessionEventReportResponseParser">An optional delegate to parse custom SetSessionEventReportResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetSessionEventReportResponse Parse(SetSessionEventReportRequest                            Request,
                                                          XElement                                                SetSessionEventReportResponseXML,
                                                          CustomXMLParserDelegate<SetSessionEventReportResponse>  CustomSetSessionEventReportResponseParser  = null,
                                                          HTTPResponse                                            HTTPResponse                               = null,
                                                          OnExceptionDelegate                                     OnException                                = null)
        {

            if (TryParse(Request,
                         SetSessionEventReportResponseXML,
                         out SetSessionEventReportResponse SetSessionEventReportResponse,
                         CustomSetSessionEventReportResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return SetSessionEventReportResponse;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (Request, SetSessionEventReportResponseText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a SetSessionEventReport response.
        /// </summary>
        /// <param name="Request">The SetSessionEventReport request leading to this response.</param>
        /// <param name="SetSessionEventReportResponseText">The text to parse.</param>
        /// <param name="CustomSetSessionEventReportResponseParser">An optional delegate to parse custom SetSessionEventReportResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetSessionEventReportResponse Parse(SetSessionEventReportRequest                            Request,
                                                          String                                                  SetSessionEventReportResponseText,
                                                          CustomXMLParserDelegate<SetSessionEventReportResponse>  CustomSetSessionEventReportResponseParser   = null,
                                                          HTTPResponse                                            HTTPResponse                                = null,
                                                          OnExceptionDelegate                                     OnException                                 = null)
        {

            if (TryParse(Request,
                         SetSessionEventReportResponseText,
                         out SetSessionEventReportResponse SetSessionEventReportResponse,
                         CustomSetSessionEventReportResponseParser,
                         HTTPResponse,
                         OnException))
            {
                return SetSessionEventReportResponse;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Request, SetSessionEventReportResponseXML,  ..., out SetSessionEventReportResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a SetSessionEventReport response.
        /// </summary>
        /// <param name="Request">The SetSessionEventReport request leading to this response.</param>
        /// <param name="SetSessionEventReportResponseXML">The XML to parse.</param>
        /// <param name="SetSessionEventReportResponse">The parsed SetSessionEventReport response.</param>
        /// <param name="CustomSetSessionEventReportResponseParser">An optional delegate to parse custom SetSessionEventReportResponse XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetSessionEventReportRequest                            Request,
                                       XElement                                                SetSessionEventReportResponseXML,
                                       out SetSessionEventReportResponse                       SetSessionEventReportResponse,
                                       CustomXMLParserDelegate<SetSessionEventReportResponse>  CustomSetSessionEventReportResponseParser   = null,
                                       HTTPResponse                                            HTTPResponse                                = null,
                                       OnExceptionDelegate                                     OnException                                 = null)
        {

            try
            {

                SetSessionEventReportResponse = new SetSessionEventReportResponse(

                                               Request,

                                               SetSessionEventReportResponseXML.MapValueOrFail("transactionId",     Transaction_Id.   Parse),

                                               SetSessionEventReportResponseXML.MapValueOrFail("requestStatus",     RequestStatus.    Parse),

                                               SetSessionEventReportResponseXML.MapValueOrFail("serviceSessionId",  ServiceSession_Id.Parse),

                                               SetSessionEventReportResponseXML.MapValueOrFail("sessionActionId",   SessionAction_Id. Parse),

                                               HTTPResponse

                                           );


                if (CustomSetSessionEventReportResponseParser != null)
                    SetSessionEventReportResponse = CustomSetSessionEventReportResponseParser(SetSessionEventReportResponseXML,
                                                                                    SetSessionEventReportResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetSessionEventReportResponseXML, e);

                SetSessionEventReportResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, SetSessionEventReportResponseText, ..., out SetSessionEventReportResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a SetSessionEventReport response.
        /// </summary>
        /// <param name="Request">The SetSessionEventReport request leading to this response.</param>
        /// <param name="SetSessionEventReportResponseText">The text to parse.</param>
        /// <param name="CustomSetSessionEventReportResponseParser">An optional delegate to parse custom SetSessionEventReportResponse XML elements.</param>
        /// <param name="SetSessionEventReportResponse">The parsed SetSessionEventReport response.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetSessionEventReportRequest                            Request,
                                       String                                                  SetSessionEventReportResponseText,
                                       out SetSessionEventReportResponse                       SetSessionEventReportResponse,
                                       CustomXMLParserDelegate<SetSessionEventReportResponse>  CustomSetSessionEventReportResponseParser   = null,
                                       HTTPResponse                                            HTTPResponse                                = null,
                                       OnExceptionDelegate                                     OnException                                 = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(SetSessionEventReportResponseText).Root,
                             out SetSessionEventReportResponse,
                             CustomSetSessionEventReportResponseParser,
                             HTTPResponse,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SetSessionEventReportResponseText, e);
            }

            SetSessionEventReportResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetSessionEventReportResponseSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetSessionEventReportResponseSerializer">A delegate to serialize custom Heartbeat response XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetSessionEventReportResponse> CustomSetSessionEventReportResponseSerializer = null)
        {

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_ToIOP_SetSessionEventReportResponse",

                          new XElement("transactionId",     TransactionId.   ToString()),
                          new XElement("requestStatus",     RequestStatus.   ToString()),
                          new XElement("serviceSessionId",  ServiceSessionId.ToString()),
                          new XElement("sessionActionId",   SessionActionId. ToString())

                      );


            return CustomSetSessionEventReportResponseSerializer != null
                       ? CustomSetSessionEventReportResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetSessionEventReportResponse1, SetSessionEventReportResponse2)

        /// <summary>
        /// Compares two SetSessionEventReport responses for equality.
        /// </summary>
        /// <param name="SetSessionEventReportResponse1">A SetSessionEventReport response.</param>
        /// <param name="SetSessionEventReportResponse2">Another SetSessionEventReport response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetSessionEventReportResponse SetSessionEventReportResponse1, SetSessionEventReportResponse SetSessionEventReportResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetSessionEventReportResponse1, SetSessionEventReportResponse2))
                return true;

            // If one is null, but not both, return false.
            if ((SetSessionEventReportResponse1 is null) || (SetSessionEventReportResponse2 is null))
                return false;

            return SetSessionEventReportResponse1.Equals(SetSessionEventReportResponse2);

        }

        #endregion

        #region Operator != (SetSessionEventReportResponse1, SetSessionEventReportResponse2)

        /// <summary>
        /// Compares two SetSessionEventReport responses for inequality.
        /// </summary>
        /// <param name="SetSessionEventReportResponse1">A SetSessionEventReport response.</param>
        /// <param name="SetSessionEventReportResponse2">Another SetSessionEventReport response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetSessionEventReportResponse SetSessionEventReportResponse1, SetSessionEventReportResponse SetSessionEventReportResponse2)
            => !(SetSessionEventReportResponse1 == SetSessionEventReportResponse2);

        #endregion

        #endregion

        #region IEquatable<SetSessionEventReportResponse> Members

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

            if (!(Object is SetSessionEventReportResponse SetSessionEventReportResponse))
                return false;

            return Equals(SetSessionEventReportResponse);

        }

        #endregion

        #region Equals(SetSessionEventReportResponse)

        /// <summary>
        /// Compares two SetSessionEventReport responses for equality.
        /// </summary>
        /// <param name="SetSessionEventReportResponse">A SetSessionEventReport response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetSessionEventReportResponse SetSessionEventReportResponse)
        {

            if (SetSessionEventReportResponse is null)
                return false;

            return TransactionId.   Equals(SetSessionEventReportResponse.TransactionId)    &&
                   RequestStatus.   Equals(SetSessionEventReportResponse.RequestStatus)    &&
                   ServiceSessionId.Equals(SetSessionEventReportResponse.ServiceSessionId) &&
                   SessionActionId. Equals(SetSessionEventReportResponse.SessionActionId);

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
                       SessionActionId. GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(TransactionId,    " -> ",
                             RequestStatus,    ", ",
                             ServiceSessionId, ", ",
                             SessionActionId);

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
        /// A SetSessionEventReport response builder.
        /// </summary>
        public class Builder : AResponseBuilder<SetSessionEventReportRequest,
                                                SetSessionEventReportResponse>
        {

            #region Properties

            /// <summary>
            /// The service session identification.
            /// </summary>
            public ServiceSession_Id  ServiceSessionId    { get; set; }

            /// <summary>
            /// The unique identification of the session action.
            /// </summary>
            public SessionAction_Id   SessionActionId     { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,                         CustomData = null)

            /// <summary>
            /// Create a new SetSessionEventReport response builder.
            /// </summary>
            /// <param name="Request">A SetSessionEventReport request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetSessionEventReportRequest              Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(SetSessionEventReportResponse, CustomData = null)

            /// <summary>
            /// Create a new SetSessionEventReport response builder.
            /// </summary>
            /// <param name="SetSessionEventReportResponse">A SetSessionEventReport response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(SetSessionEventReportResponse             SetSessionEventReportResponse  = null,
                           IReadOnlyDictionary<String, Object>  CustomData                = null)

                : base(SetSessionEventReportResponse?.Request,
                       SetSessionEventReportResponse.HasCustomData
                           ? CustomData?.Count > 0
                                 ? SetSessionEventReportResponse.CustomData.Concat(CustomData)
                                 : SetSessionEventReportResponse.CustomData
                           : CustomData)

            {

                if (SetSessionEventReportResponse != null)
                {
                    this.TransactionId     = SetSessionEventReportResponse.TransactionId;
                    this.RequestStatus     = SetSessionEventReportResponse.RequestStatus;
                    this.ServiceSessionId  = SetSessionEventReportResponse.ServiceSessionId;
                    this.SessionActionId   = SetSessionEventReportResponse.SessionActionId;
                }

            }

            #endregion

            #endregion


            #region Equals(SetSessionEventReportResponse)

            /// <summary>
            /// Compares two SetSessionEventReport responses for equality.
            /// </summary>
            /// <param name="SetSessionEventReportResponse">A SetSessionEventReport response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(SetSessionEventReportResponse SetSessionEventReportResponse)
            {

                if (SetSessionEventReportResponse is null)
                    return false;

                return TransactionId.   Equals(SetSessionEventReportResponse.TransactionId)    &&
                       RequestStatus.   Equals(SetSessionEventReportResponse.RequestStatus)    &&
                       ServiceSessionId.Equals(SetSessionEventReportResponse.ServiceSessionId) &&
                       SessionActionId. Equals(SetSessionEventReportResponse.SessionActionId);

            }

            #endregion

            #region (implicit) "ToImmutable()"

            /// <summary>
            /// Return an immutable SetSessionEventReportResponse response.
            /// </summary>
            /// <param name="Builder">A SetSessionEventReportResponse response builder.</param>
            public static implicit operator SetSessionEventReportResponse(Builder Builder)

                => new SetSessionEventReportResponse(Builder.Request,
                                                     Builder.TransactionId,
                                                     Builder.RequestStatus,
                                                     Builder.ServiceSessionId,
                                                     Builder.SessionActionId);

            #endregion

        }

        #endregion

    }

}
