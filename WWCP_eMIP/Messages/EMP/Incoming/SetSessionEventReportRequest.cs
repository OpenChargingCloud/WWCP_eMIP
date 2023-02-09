/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

namespace cloud.charging.open.protocols.eMIPv0_7_4.EMP
{

    /// <summary>
    /// A SetSessionEventReportRequest request,
    /// e.g. report an event from a CPO to an EMP during a charging session.
    /// </summary>
    public class SetSessionEventReportRequest : ARequest<SetSessionEventReportRequest>
    {

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id                OperatorId             { get; }

        /// <summary>
        /// The target operator identification.
        /// </summary>
        public Operator_Id                TargetOperatorId       { get; }

        /// <summary>
        /// The service session identification.
        /// </summary>
        public ServiceSession_Id          ServiceSessionId        { get; }

        /// <summary>
        /// An optional partner service session identification.
        /// </summary>
        public PartnerServiceSession_Id?  SalePartnerSessionId    { get; }

        /// <summary>
        /// The session event.
        /// </summary>
        public SessionEvent               SessionEvent            { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a SetSessionEventReportRequest XML/SOAP request.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="TargetOperatorId">The target operator identification.</param>
        /// <param name="ServiceSessionId">The service session identification.</param>
        /// <param name="SessionEvent">The session event.</param>
        /// 
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="SalePartnerSessionId">An optional partner service session identification.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetSessionEventReportRequest(Partner_Id                 PartnerId,
                                            Operator_Id                OperatorId,
                                            Operator_Id                TargetOperatorId,
                                            ServiceSession_Id          ServiceSessionId,
                                            SessionEvent               SessionEvent,

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
            this.TargetOperatorId      = TargetOperatorId;
            this.ServiceSessionId      = ServiceSessionId;
            this.SessionEvent          = SessionEvent;
            this.SalePartnerSessionId  = SalePartnerSessionId;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:aut  = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //
        //    <soap:Header/>
        //    <soap:Body>
        //       <aut:eMIP_FromIOP_SetSessionEventReportRequest>
        //
        //          <!--Optional:-->
        //          <transactionId>?</transactionId>
        //
        //          <partnerIdType>eMI3</partnerIdType>
        //          <partnerId>FR*IOP</partnerId>
        //
        //          <operatorIdType>eMI3</operatorIdType>
        //          <operatorId>FR*CPO</operatorId>
        //
        //          <targetOperatorIdType>eMI3</targetOperatorIdType>
        //          <targetOperatorId>FR*EMP</targetOperatorId>
        //
        //          <serviceSessionId>IOP-SID-GIR-V-IOPFT01-0dc6fc3...153e</serviceSessionId>
        //
        //          <!--Optional:-->
        //          <salePartnerSessionId>eMSP_Id_001</salePartnerSessionId>
        //
        //          <sessionEvent>
        //
        //             <sessionEventNature>1</sessionEventNature>
        //
        //             <!--Optional:-->
        //             <sessionEventId>Event-Id-2015-11-19T11:14:17.123Z</sessionEventId>
        //
        //             <sessionEventDateTime>2015-11-19T11:14:17.123Z</sessionEventDateTime>
        //
        //             <!--Optional:-->
        //             <sessionEventParameter>?</sessionEventParameter>
        //
        //             <!--Optional:-->
        //             <relatedSessionEventId>?</relatedSessionEventId>
        //
        //          </sessionEvent>
        //
        //       </aut:eMIP_ToIOP_SetSessionEventReportRequestRequest>
        //    </soap:Body>
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (SetSessionEventReportRequestXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetSessionEventReportRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetSessionEventReportRequestParser">An optional delegate to parse custom SetSessionEventReportRequest XML elements.</param>
        /// <param name="CustomSessionEventParser">An optional delegate to parse custom SessionEvent XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetSessionEventReportRequest Parse(XElement                                               SetSessionEventReportRequestXML,
                                                         CustomXMLParserDelegate<SetSessionEventReportRequest>  CustomSendSetSessionEventReportRequestParser   = null,
                                                         CustomXMLParserDelegate<SessionEvent>                  CustomSessionEventParser                       = null,
                                                         OnExceptionDelegate                                    OnException                                    = null,

                                                         HTTPRequest                                            HTTPRequest                                    = null,
                                                         DateTime?                                              Timestamp                                      = null,
                                                         CancellationToken?                                     CancellationToken                              = null,
                                                         EventTracking_Id                                       EventTrackingId                                = null,
                                                         TimeSpan?                                              RequestTimeout                                 = null)
        {

            if (TryParse(SetSessionEventReportRequestXML,
                         out SetSessionEventReportRequest _SetSessionEventReportRequest,
                         CustomSendSetSessionEventReportRequestParser,
                         CustomSessionEventParser,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetSessionEventReportRequest;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (SetSessionEventReportRequestText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetSessionEventReportRequestText">The text to parse.</param>
        /// <param name="CustomSendSetSessionEventReportRequestParser">An optional delegate to parse custom SetSessionEventReportRequest XML elements.</param>
        /// <param name="CustomSessionEventParser">An optional delegate to parse custom SessionEvent XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetSessionEventReportRequest Parse(String                                                 SetSessionEventReportRequestText,
                                                         CustomXMLParserDelegate<SetSessionEventReportRequest>  CustomSendSetSessionEventReportRequestParser   = null,
                                                         CustomXMLParserDelegate<SessionEvent>                  CustomSessionEventParser                       = null,
                                                         OnExceptionDelegate                                    OnException                                    = null,

                                                         HTTPRequest                                            HTTPRequest                                    = null,
                                                         DateTime?                                              Timestamp                                      = null,
                                                         CancellationToken?                                     CancellationToken                              = null,
                                                         EventTracking_Id                                       EventTrackingId                                = null,
                                                         TimeSpan?                                              RequestTimeout                                 = null)
        {

            if (TryParse(SetSessionEventReportRequestText,
                         out SetSessionEventReportRequest _SetSessionEventReportRequest,
                         CustomSendSetSessionEventReportRequestParser,
                         CustomSessionEventParser,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetSessionEventReportRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SetSessionEventReportRequestXML,  ..., out SetSessionEventReportRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetSessionEventReportRequestXML">The XML to parse.</param>
        /// <param name="SetSessionEventReportRequest">The parsed heartbeat request.</param>
        /// <param name="CustomSendSetSessionEventReportRequestParser">An optional delegate to parse custom SetSessionEventReportRequest XML elements.</param>
        /// <param name="CustomSessionEventParser">An optional delegate to parse custom SessionEvent XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                               SetSessionEventReportRequestXML,
                                       out SetSessionEventReportRequest                       SetSessionEventReportRequest,
                                       CustomXMLParserDelegate<SetSessionEventReportRequest>  CustomSendSetSessionEventReportRequestParser   = null,
                                       CustomXMLParserDelegate<SessionEvent>                  CustomSessionEventParser                       = null,
                                       OnExceptionDelegate                                    OnException                                    = null,

                                       HTTPRequest                                            HTTPRequest                                    = null,
                                       DateTime?                                              Timestamp                                      = null,
                                       CancellationToken?                                     CancellationToken                              = null,
                                       EventTracking_Id                                       EventTrackingId                                = null,
                                       TimeSpan?                                              RequestTimeout                                 = null)
        {

            try
            {

                var SessionEventXML = SetSessionEventReportRequestXML.ElementOrFail("sessionAction");


                SetSessionEventReportRequest = new SetSessionEventReportRequest(

                                                     //ToDo: What to do with: <partnerIdType>eMI3</partnerIdType>?
                                                     SetSessionEventReportRequestXML.MapValueOrFail    ("partnerId",             Partner_Id.              Parse),
                                                     SetSessionEventReportRequestXML.MapValueOrFail    ("operatorId",            Operator_Id.             Parse),
                                                     SetSessionEventReportRequestXML.MapValueOrFail    ("targetOperatorId",      Operator_Id.             Parse),

                                                     SetSessionEventReportRequestXML.MapValueOrFail    ("serviceSessionId",      ServiceSession_Id.       Parse),

                                                     SessionEventXML.                MapElementOrFail  ("sessionEvent",
                                                                                                         (s, e) => SessionEvent.Parse(s,
                                                                                                                                      CustomSessionEventParser,
                                                                                                                                      e),
                                                                                                         OnException),


                                                     SetSessionEventReportRequestXML.MapValueOrNullable("transactionId",         Transaction_Id.          Parse),
                                                     SetSessionEventReportRequestXML.MapValueOrFail    ("salePartnerSessionId",  PartnerServiceSession_Id.Parse),

                                                     HTTPRequest,
                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout

                                                 );


                if (CustomSendSetSessionEventReportRequestParser != null)
                    SetSessionEventReportRequest = CustomSendSetSessionEventReportRequestParser(SetSessionEventReportRequestXML,
                                                                                                SetSessionEventReportRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetSessionEventReportRequestXML, e);

                SetSessionEventReportRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetSessionEventReportRequestText, ..., out SetSessionEventReportRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetSessionEventReportRequestText">The text to parse.</param>
        /// <param name="SetSessionEventReportRequest">The parsed heartbeat request.</param>
        /// <param name="CustomSendSetSessionEventReportRequestParser">An optional delegate to parse custom SetSessionEventReportRequest XML elements.</param>
        /// <param name="CustomSessionEventParser">An optional delegate to parse custom SessionEvent XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                 SetSessionEventReportRequestText,
                                       out SetSessionEventReportRequest                       SetSessionEventReportRequest,
                                       CustomXMLParserDelegate<SetSessionEventReportRequest>  CustomSendSetSessionEventReportRequestParser   = null,
                                       CustomXMLParserDelegate<SessionEvent>                  CustomSessionEventParser                       = null,
                                       OnExceptionDelegate                                    OnException                                    = null,

                                       HTTPRequest                                            HTTPRequest                                    = null,
                                       DateTime?                                              Timestamp                                      = null,
                                       CancellationToken?                                     CancellationToken                              = null,
                                       EventTracking_Id                                       EventTrackingId                                = null,
                                       TimeSpan?                                              RequestTimeout                                 = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetSessionEventReportRequestText).Root,
                             out SetSessionEventReportRequest,
                             CustomSendSetSessionEventReportRequestParser,
                             CustomSessionEventParser,
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
                OnException?.Invoke(DateTime.UtcNow, SetSessionEventReportRequestText, e);
            }

            SetSessionEventReportRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetSessionEventReportRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetSessionEventReportRequestSerializer">A delegate to serialize custom set EVSE busy status request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetSessionEventReportRequest> CustomSetSessionEventReportRequestSerializer = null)
        {

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_ToIOP_SetSessionEventReportRequest",

                          TransactionId.HasValue
                              ? new XElement("transactionId",         TransactionId.       ToString())
                              : null,

                          new XElement("partnerIdType",         PartnerId.Format.        AsText()),
                          new XElement("partnerId",             PartnerId.               ToString()),

                          new XElement("operatorIdType",        OperatorId.Format.       AsText()),
                          new XElement("operatorId",            OperatorId.              ToString()),

                          new XElement("targetOperatorIdType",  TargetOperatorId.Format. AsText()),
                          new XElement("targetOperatorId",      TargetOperatorId.        ToString()),

                          new XElement("serviceSessionId",      ServiceSessionId.        ToString()),

                          SessionEvent.ToXML(),

                          SalePartnerSessionId.HasValue
                              ? new XElement("salePartnerSessionId",  SalePartnerSessionId.ToString())
                              : null

                      );


            return CustomSetSessionEventReportRequestSerializer != null
                       ? CustomSetSessionEventReportRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetSessionEventReportRequest1, SetSessionEventReportRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetSessionEventReportRequest1">A heartbeat request.</param>
        /// <param name="SetSessionEventReportRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetSessionEventReportRequest SetSessionEventReportRequest1, SetSessionEventReportRequest SetSessionEventReportRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetSessionEventReportRequest1, SetSessionEventReportRequest2))
                return true;

            // If one is null, but not both, return false.
            if ((SetSessionEventReportRequest1 is null) || (SetSessionEventReportRequest2 is null))
                return false;

            return SetSessionEventReportRequest1.Equals(SetSessionEventReportRequest2);

        }

        #endregion

        #region Operator != (SetSessionEventReportRequest1, SetSessionEventReportRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="SetSessionEventReportRequest1">A heartbeat request.</param>
        /// <param name="SetSessionEventReportRequest2">Another heartbeat request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetSessionEventReportRequest SetSessionEventReportRequest1, SetSessionEventReportRequest SetSessionEventReportRequest2)

            => !(SetSessionEventReportRequest1 == SetSessionEventReportRequest2);

        #endregion

        #endregion

        #region IEquatable<SetSessionEventReportRequest> Members

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

            if (!(Object is SetSessionEventReportRequest SetSessionEventReportRequest))
                return false;

            return Equals(SetSessionEventReportRequest);

        }

        #endregion

        #region Equals(SetSessionEventReportRequest)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetSessionEventReportRequest">A heartbeat request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetSessionEventReportRequest SetSessionEventReportRequest)
        {

            if (SetSessionEventReportRequest is null)
                return false;

            return ((!TransactionId.HasValue && !SetSessionEventReportRequest.TransactionId.HasValue) ||
                     (TransactionId.HasValue && SetSessionEventReportRequest.TransactionId.HasValue && TransactionId.Value.Equals(SetSessionEventReportRequest.TransactionId.Value))) &&

                   PartnerId.       Equals(SetSessionEventReportRequest.PartnerId)        &&
                   OperatorId.      Equals(SetSessionEventReportRequest.OperatorId)       &&
                   TargetOperatorId.Equals(SetSessionEventReportRequest.TargetOperatorId) &&
                   ServiceSessionId.Equals(SetSessionEventReportRequest.ServiceSessionId) &&
                   SessionEvent.    Equals(SetSessionEventReportRequest.SessionEvent)     &&

                   ((!SalePartnerSessionId.HasValue && !SetSessionEventReportRequest.SalePartnerSessionId.HasValue) ||
                     (SalePartnerSessionId.HasValue &&  SetSessionEventReportRequest.SalePartnerSessionId.HasValue && SalePartnerSessionId.Equals(SetSessionEventReportRequest.SalePartnerSessionId)));

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
                            ? TransactionId.GetHashCode() * 17
                            : 0) ^

                       PartnerId.       GetHashCode() * 13 ^
                       OperatorId.      GetHashCode() * 11 ^
                       TargetOperatorId.GetHashCode() *  7 ^
                       ServiceSessionId.GetHashCode() *  5 ^
                       SessionEvent.    GetHashCode() *  3 ^

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
                             SessionEvent.Nature,
                             SessionEvent.Parameter.IsNotNullOrEmpty() ? ", (" + SessionEvent.Parameter + ")" : "");

        #endregion

    }

}
