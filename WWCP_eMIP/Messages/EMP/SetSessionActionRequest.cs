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
using System.Xml.Linq;
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.EMP
{

    /// <summary>
    /// A SetSessionActionRequest request.
    /// </summary>
    public class SetSessionActionRequest : ARequest<SetSessionActionRequest>
    {

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id                OperatorId              { get; }

        /// <summary>
        /// The service session identification.
        /// </summary>
        public ServiceSession_Id          ServiceSessionId        { get; }

        /// <summary>
        /// An optional partner service session identification.
        /// </summary>
        public PartnerServiceSession_Id?  SalePartnerSessionId    { get; }

        /// <summary>
        /// The session action.
        /// </summary>
        public SessionAction              SessionAction           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a SetSessionActionRequest XML/SOAP request.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="ServiceSessionId">The service session identification.</param>
        /// <param name="SessionAction">The session action.</param>
        /// 
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="SalePartnerSessionId">An optional partner service session identification.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetSessionActionRequest(Partner_Id                 PartnerId,
                                       Operator_Id                OperatorId,
                                       ServiceSession_Id          ServiceSessionId,
                                       SessionAction              SessionAction,

                                       Transaction_Id?            TransactionId          = null,
                                       PartnerServiceSession_Id?  SalePartnerSessionId   = null,

                                       HTTPRequest                HTTPRequest            = null,
                                       DateTime?                  Timestamp              = null,
                                       CancellationToken?         CancellationToken      = null,
                                       EventTracking_Id           EventTrackingId        = null,
                                       TimeSpan?                  RequestTimeout         = null)

            : base(HTTPRequest,
                   PartnerId,
                   TransactionId,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.OperatorId            = OperatorId;
            this.ServiceSessionId      = ServiceSessionId;
            this.SessionAction         = SessionAction;
            this.SalePartnerSessionId  = SalePartnerSessionId;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:aut  = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //
        //    <soap:Header/>
        //    <soap:Body>
        //       <aut:eMIP_ToIOP_SetSessionActionRequestRequest>
        //
        //          <!--Optional:-->
        //          <transactionId>?</transactionId>
        //
        //          <partnerIdType>?</partnerIdType>
        //          <partnerId>?</partnerId>
        //
        //          <operatorIdType>?</operatorIdType>
        //          <operatorId>?</operatorId>
        //
        //          <serviceSessionId>?</serviceSessionId>
        //
        //          <!--Optional:-->
        //          <salePartnerSessionId>eMSP_Id_001</salePartnerSessionId>
        //
        //          <sessionAction>
        //
        //             <sessionActionNature>?</sessionActionNature>
        //
        //             <!--Optional:-->
        //             <sessionActionId>?</sessionActionId>
        //
        //             <sessionActionDateTime>?</sessionActionDateTime>
        //
        //             <!--Optional:-->
        //             <sessionActionParameter>?</sessionActionParameter>
        //
        //             <!--Optional:-->
        //             <relatedSessionEventId>?</relatedSessionEventId>
        //
        //          </sessionAction>
        //
        //       </aut:eMIP_ToIOP_SetSessionActionRequestRequest>
        //    </soap:Body>
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (SetSessionActionRequestXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetSessionActionRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetSessionActionRequestParser">An optional delegate to parse custom SetSessionActionRequest XML elements.</param>
        /// <param name="CustomSessionActionParser">An optional delegate to parse custom SessionAction XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetSessionActionRequest Parse(XElement                                          SetSessionActionRequestXML,
                                                    CustomXMLParserDelegate<SetSessionActionRequest>  CustomSendSetSessionActionRequestParser,
                                                    CustomXMLParserDelegate<SessionAction>            CustomSessionActionParser,
                                                    OnExceptionDelegate                               OnException         = null,

                                                    HTTPRequest                                       HTTPRequest         = null,
                                                    DateTime?                                         Timestamp           = null,
                                                    CancellationToken?                                CancellationToken   = null,
                                                    EventTracking_Id                                  EventTrackingId     = null,
                                                    TimeSpan?                                         RequestTimeout      = null)
        {

            if (TryParse(SetSessionActionRequestXML,
                         CustomSendSetSessionActionRequestParser,
                         CustomSessionActionParser,
                         out SetSessionActionRequest _SetSessionActionRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetSessionActionRequest;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (SetSessionActionRequestText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetSessionActionRequestText">The text to parse.</param>
        /// <param name="CustomSendSetSessionActionRequestParser">An optional delegate to parse custom SetSessionActionRequest XML elements.</param>
        /// <param name="CustomSessionActionParser">An optional delegate to parse custom SessionAction XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetSessionActionRequest Parse(String                                            SetSessionActionRequestText,
                                                    CustomXMLParserDelegate<SetSessionActionRequest>  CustomSendSetSessionActionRequestParser,
                                                    CustomXMLParserDelegate<SessionAction>            CustomSessionActionParser,
                                                    OnExceptionDelegate                               OnException         = null,

                                                    HTTPRequest                                       HTTPRequest         = null,
                                                    DateTime?                                         Timestamp           = null,
                                                    CancellationToken?                                CancellationToken   = null,
                                                    EventTracking_Id                                  EventTrackingId     = null,
                                                    TimeSpan?                                         RequestTimeout      = null)
        {

            if (TryParse(SetSessionActionRequestText,
                         CustomSendSetSessionActionRequestParser,
                         CustomSessionActionParser,
                         out SetSessionActionRequest _SetSessionActionRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetSessionActionRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SetSessionActionRequestXML,  ..., out SetSessionActionRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetSessionActionRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetSessionActionRequestParser">An optional delegate to parse custom SetSessionActionRequest XML elements.</param>
        /// <param name="CustomSessionActionParser">An optional delegate to parse custom SessionAction XML elements.</param>
        /// <param name="SetSessionActionRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                          SetSessionActionRequestXML,
                                       CustomXMLParserDelegate<SetSessionActionRequest>  CustomSendSetSessionActionRequestParser,
                                       CustomXMLParserDelegate<SessionAction>            CustomSessionActionParser,
                                       out SetSessionActionRequest                       SetSessionActionRequest,
                                       OnExceptionDelegate                               OnException         = null,

                                       HTTPRequest                                       HTTPRequest         = null,
                                       DateTime?                                         Timestamp           = null,
                                       CancellationToken?                                CancellationToken   = null,
                                       EventTracking_Id                                  EventTrackingId     = null,
                                       TimeSpan?                                         RequestTimeout      = null)
        {

            try
            {

                var SessionActionXML = SetSessionActionRequestXML.ElementOrFail("sessionAction");


                SetSessionActionRequest = new SetSessionActionRequest(

                                                     //ToDo: What to do with: <partnerIdType>eMI3</partnerIdType>?
                                                     SetSessionActionRequestXML.MapValueOrFail    ("partnerId",             Partner_Id.              Parse),

                                                     //ToDo: What to do with: <operatorIdType>eMI3</operatorIdType>
                                                     SetSessionActionRequestXML.MapValueOrFail    ("operatorId",            Operator_Id.             Parse),

                                                     SetSessionActionRequestXML.MapValueOrFail    ("serviceSessionId",      ServiceSession_Id.       Parse),

                                                     SessionActionXML.          MapElementOrFail  ("sessionAction",
                                                                                                   (s, e) => SessionAction.Parse(s,
                                                                                                                                 CustomSessionActionParser,
                                                                                                                                 e),
                                                                                                   OnException),


                                                     SetSessionActionRequestXML.MapValueOrNullable("transactionId",         Transaction_Id.          Parse),
                                                     SetSessionActionRequestXML.MapValueOrFail    ("salePartnerSessionId",  PartnerServiceSession_Id.Parse),

                                                     HTTPRequest,
                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout

                                                 );


                if (CustomSendSetSessionActionRequestParser != null)
                    SetSessionActionRequest = CustomSendSetSessionActionRequestParser(SetSessionActionRequestXML,
                                                                                      SetSessionActionRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetSessionActionRequestXML, e);

                SetSessionActionRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetSessionActionRequestText, ..., out SetSessionActionRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetSessionActionRequestText">The text to parse.</param>
        /// <param name="CustomSendSetSessionActionRequestParser">An optional delegate to parse custom SetSessionActionRequest XML elements.</param>
        /// <param name="CustomSessionActionParser">An optional delegate to parse custom SessionAction XML elements.</param>
        /// <param name="SetSessionActionRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                            SetSessionActionRequestText,
                                       CustomXMLParserDelegate<SetSessionActionRequest>  CustomSendSetSessionActionRequestParser,
                                       CustomXMLParserDelegate<SessionAction>            CustomSessionActionParser,
                                       out SetSessionActionRequest                       SetSessionActionRequest,
                                       OnExceptionDelegate                               OnException         = null,

                                       HTTPRequest                                       HTTPRequest         = null,
                                       DateTime?                                         Timestamp           = null,
                                       CancellationToken?                                CancellationToken   = null,
                                       EventTracking_Id                                  EventTrackingId     = null,
                                       TimeSpan?                                         RequestTimeout      = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetSessionActionRequestText).Root,
                             CustomSendSetSessionActionRequestParser,
                             CustomSessionActionParser,
                             out SetSessionActionRequest,
                             OnException,

                             HTTPRequest,
                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SetSessionActionRequestText, e);
            }

            SetSessionActionRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetSessionActionRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetSessionActionRequestSerializer">A delegate to serialize custom set EVSE busy status request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetSessionActionRequest> CustomSetSessionActionRequestSerializer = null)
        {

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_ToIOP_SetSessionActionRequest",

                          TransactionId.HasValue
                              ? new XElement("transactionId",         TransactionId.       ToString())
                              : null,

                          new XElement("partnerIdType",       PartnerId.Format. AsText()),
                          new XElement("partnerId",           PartnerId.        ToString()),

                          new XElement("operatorIdType",      OperatorId.Format.AsText()),
                          new XElement("operatorId",          OperatorId.       ToString()),

                          new XElement("serviceSessionId",    ServiceSessionId. ToString()),

                          SessionAction.ToXML(),

                          SalePartnerSessionId.HasValue
                              ? new XElement("salePartnerSessionId",  SalePartnerSessionId.ToString())
                              : null

                      );


            return CustomSetSessionActionRequestSerializer != null
                       ? CustomSetSessionActionRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetSessionActionRequest1, SetSessionActionRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetSessionActionRequest1">A heartbeat request.</param>
        /// <param name="SetSessionActionRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetSessionActionRequest SetSessionActionRequest1, SetSessionActionRequest SetSessionActionRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetSessionActionRequest1, SetSessionActionRequest2))
                return true;

            // If one is null, but not both, return false.
            if ((SetSessionActionRequest1 is null) || (SetSessionActionRequest2 is null))
                return false;

            return SetSessionActionRequest1.Equals(SetSessionActionRequest2);

        }

        #endregion

        #region Operator != (SetSessionActionRequest1, SetSessionActionRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="SetSessionActionRequest1">A heartbeat request.</param>
        /// <param name="SetSessionActionRequest2">Another heartbeat request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetSessionActionRequest SetSessionActionRequest1, SetSessionActionRequest SetSessionActionRequest2)

            => !(SetSessionActionRequest1 == SetSessionActionRequest2);

        #endregion

        #endregion

        #region IEquatable<SetSessionActionRequest> Members

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

            if (!(Object is SetSessionActionRequest SetSessionActionRequest))
                return false;

            return Equals(SetSessionActionRequest);

        }

        #endregion

        #region Equals(SetSessionActionRequest)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetSessionActionRequest">A heartbeat request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetSessionActionRequest SetSessionActionRequest)
        {

            if (SetSessionActionRequest is null)
                return false;

            return ((!TransactionId.HasValue && !SetSessionActionRequest.TransactionId.HasValue) ||
                     (TransactionId.HasValue && SetSessionActionRequest.TransactionId.HasValue && TransactionId.Value.Equals(SetSessionActionRequest.TransactionId.Value))) &&

                   PartnerId.             Equals(SetSessionActionRequest.PartnerId)              &&
                   OperatorId.            Equals(SetSessionActionRequest.OperatorId)             &&
                   ServiceSessionId.      Equals(SetSessionActionRequest.ServiceSessionId)       &&
                   SessionAction.         Equals(SetSessionActionRequest.SessionAction)  &&

                   ((!SalePartnerSessionId.HasValue && !SetSessionActionRequest.SalePartnerSessionId.HasValue) ||
                     (SalePartnerSessionId.HasValue &&  SetSessionActionRequest.SalePartnerSessionId.HasValue && SalePartnerSessionId.Equals(SetSessionActionRequest.SalePartnerSessionId)));

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

                return (TransactionId.HasValue
                            ? TransactionId.GetHashCode() * 13
                            : 0) ^

                       PartnerId.       GetHashCode() * 11 ^
                       OperatorId.      GetHashCode() *  7 ^
                       ServiceSessionId.GetHashCode() *  5 ^
                       SessionAction.   GetHashCode() *  3 ^

                       (SalePartnerSessionId.HasValue
                            ? SalePartnerSessionId.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(PartnerId,  " / ",
                             OperatorId, ": ",
                             SessionAction.Nature,
                             SessionAction.Parameter.IsNotNullOrEmpty() ? ", (" + SessionAction.Parameter + ")" : "");

        #endregion

    }

}
