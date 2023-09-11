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
    /// An incoming GetServiceAuthorisation request.
    /// </summary>
    public class GetServiceAuthorisationRequest : ARequest<GetServiceAuthorisationRequest>
    {

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id         OperatorId             { get; }

        /// <summary>
        /// The target operator identification.
        /// </summary>
        public Operator_Id         TargetOperatorId       { get; }

        /// <summary>
        /// The EVSE identification.
        /// </summary>
        public EVSE_Id             EVSEId                 { get; }

        /// <summary>
        /// The user identification.
        /// </summary>
        public User_Id             UserId                 { get; }

        /// <summary>
        /// The service identification for which an authorisation is requested.
        /// </summary>
        public Service_Id          RequestedServiceId     { get; }

        /// <summary>
        /// An optional session identification.
        /// </summary>
        public ServiceSession_Id?  ServiceSessionId       { get; }

        /// <summary>
        /// An optional booking identification.
        /// </summary>
        public Booking_Id?         BookingId              { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new incoming GetServiceAuthorisation XML/SOAP request.
        /// </summary>
        /// <param name="TransactionId">The transaction identification.</param>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="TargetOperatorId">The target operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="UserId">The user identification.</param>
        /// <param name="RequestedServiceId">The service identification for which an authorisation is requested.</param>
        /// <param name="ServiceSessionId">An optional session identification.</param>
        /// <param name="BookingId">An optional booking identification.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public GetServiceAuthorisationRequest(Transaction_Id      TransactionId,
                                              Partner_Id          PartnerId,
                                              Operator_Id         OperatorId,
                                              Operator_Id         TargetOperatorId,
                                              EVSE_Id             EVSEId,
                                              User_Id             UserId,
                                              Service_Id          RequestedServiceId,
                                              ServiceSession_Id?  ServiceSessionId    = null,
                                              Booking_Id?         BookingId           = null,

                                              HTTPRequest?        HTTPRequest         = null,
                                              DateTime?           Timestamp           = null,
                                              CancellationToken   CancellationToken   = default,
                                              EventTracking_Id?   EventTrackingId     = null,
                                              TimeSpan?           RequestTimeout      = null)

            : base(HTTPRequest,
                   PartnerId,
                   TransactionId,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.OperatorId          = OperatorId;
            this.TargetOperatorId    = TargetOperatorId;
            this.EVSEId              = EVSEId;
            this.UserId              = UserId;
            this.RequestedServiceId  = RequestedServiceId;
            this.ServiceSessionId    = ServiceSessionId;
            this.BookingId           = BookingId;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //
        //   <soap:Header />
        //
        //   <soap:Body>
        //     <eMIP:eMIP_FromIOP_GetServiceAuthorisationRequest>
        //
        //       <transactionId>TRANSACTION_46151</transactionId>
        //
        //       <partnerIdType>eMI3</partnerIdType>
        //       <partnerId>FR*MSP</partnerId>
        //
        //       <operatorIdType>eMI3</operatorIdType>
        //       <operatorId>FR*798</operatorId>
        //
        //       <targetOperatorIdType>eMI3</targetOperatorIdType>
        //       <targetOperatorId>FR*EMP</targetOperatorId>
        //
        //       <EVSEIdType>eMI3</EVSEIdType>
        //       <EVSEId>?</EVSEId>
        //
        //       <userIdType>RFID-UID</userIdType>
        //       <userId>?</userId>
        //
        //       <requestedServiceId>1</requestedServiceId>
        //
        //       <!--Optional:-->
        //       <serviceSessionId>?</serviceSessionId>
        //
        //       <!--Optional:-->
        //       <bookingId>?</bookingId>
        //
        //     </eMIP:eMIP_FromIOP_GetServiceAuthorisationRequest>
        //   </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (GetServiceAuthorisationRequestXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="GetServiceAuthorisationRequestXML">The XML to parse.</param>
        /// <param name="CustomSendGetServiceAuthorisationRequestParser">An optional delegate to parse custom GetServiceAuthorisationRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static GetServiceAuthorisationRequest Parse(XElement                                                 GetServiceAuthorisationRequestXML,
                                                           CustomXMLParserDelegate<GetServiceAuthorisationRequest>  CustomSendGetServiceAuthorisationRequestParser,
                                                           OnExceptionDelegate                                      OnException         = null,

                                                           HTTPRequest                                              HTTPRequest         = null,
                                                           DateTime?                                                Timestamp           = null,
                                                           CancellationToken                                        CancellationToken   = default,
                                                           EventTracking_Id                                         EventTrackingId     = null,
                                                           TimeSpan?                                                RequestTimeout      = null)
        {

            if (TryParse(GetServiceAuthorisationRequestXML,
                         CustomSendGetServiceAuthorisationRequestParser,
                         out GetServiceAuthorisationRequest _GetServiceAuthorisationRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _GetServiceAuthorisationRequest;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (GetServiceAuthorisationRequestText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="GetServiceAuthorisationRequestText">The text to parse.</param>
        /// <param name="CustomSendGetServiceAuthorisationRequestParser">An optional delegate to parse custom GetServiceAuthorisationRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static GetServiceAuthorisationRequest Parse(String                                                   GetServiceAuthorisationRequestText,
                                                           CustomXMLParserDelegate<GetServiceAuthorisationRequest>  CustomSendGetServiceAuthorisationRequestParser,
                                                           OnExceptionDelegate                                      OnException         = null,

                                                           HTTPRequest                                              HTTPRequest         = null,
                                                           DateTime?                                                Timestamp           = null,
                                                           CancellationToken                                        CancellationToken   = default,
                                                           EventTracking_Id                                         EventTrackingId     = null,
                                                           TimeSpan?                                                RequestTimeout      = null)
        {

            if (TryParse(GetServiceAuthorisationRequestText,
                         CustomSendGetServiceAuthorisationRequestParser,
                         out GetServiceAuthorisationRequest _GetServiceAuthorisationRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _GetServiceAuthorisationRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(GetServiceAuthorisationRequestXML,  ..., out GetServiceAuthorisationRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="GetServiceAuthorisationRequestXML">The XML to parse.</param>
        /// <param name="CustomSendGetServiceAuthorisationRequestParser">An optional delegate to parse custom GetServiceAuthorisationRequest XML elements.</param>
        /// <param name="GetServiceAuthorisationRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                                 GetServiceAuthorisationRequestXML,
                                       CustomXMLParserDelegate<GetServiceAuthorisationRequest>  CustomSendGetServiceAuthorisationRequestParser,
                                       out GetServiceAuthorisationRequest                       GetServiceAuthorisationRequest,
                                       OnExceptionDelegate                                      OnException         = null,

                                       HTTPRequest                                              HTTPRequest         = null,
                                       DateTime?                                                Timestamp           = null,
                                       CancellationToken                                        CancellationToken   = default,
                                       EventTracking_Id                                         EventTrackingId     = null,
                                       TimeSpan?                                                RequestTimeout      = null)
        {

            try
            {

                GetServiceAuthorisationRequest = new GetServiceAuthorisationRequest(

                                                     GetServiceAuthorisationRequestXML.MapValueOrFail    ("transactionId",       Transaction_Id.Parse),

                                                     //ToDo: What to do with: <partnerIdType>eMI3</partnerIdType>?
                                                     GetServiceAuthorisationRequestXML.MapValueOrFail    ("partnerId",           Partner_Id.Parse),

                                                     //ToDo: What to do with: <operatorIdType>eMI3</operatorIdType>
                                                     GetServiceAuthorisationRequestXML.MapValueOrFail    ("operatorId",          Operator_Id.Parse),

                                                     //ToDo: What to do with: <targetOperatorIdType>eMI3</targetOperatorIdType>
                                                     GetServiceAuthorisationRequestXML.MapValueOrFail    ("targetOperatorId",    Operator_Id.Parse),

                                                     //ToDo: What to do with: <EVSEIdType>eMI3</EVSEIdType>
                                                     GetServiceAuthorisationRequestXML.MapValueOrFail    ("EVSEId",              EVSE_Id.Parse),

                                                     GetServiceAuthorisationRequestXML.MapValueOrFail    ("userId",              s => User_Id.Parse(s,
                                                         GetServiceAuthorisationRequestXML.MapValueOrFail("userIdType",          UserIdFormatsExtensions.Parse))),

                                                     GetServiceAuthorisationRequestXML.MapValueOrFail    ("requestedServiceId",  Service_Id.Parse),

                                                     GetServiceAuthorisationRequestXML.MapValueOrNullable("serviceSessionId",    ServiceSession_Id.Parse),

                                                     GetServiceAuthorisationRequestXML.MapValueOrNullable("bookingId",           Booking_Id.Parse),

                                                     HTTPRequest,
                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout

                                                 );


                if (CustomSendGetServiceAuthorisationRequestParser != null)
                    GetServiceAuthorisationRequest = CustomSendGetServiceAuthorisationRequestParser(GetServiceAuthorisationRequestXML,
                                                                                                    GetServiceAuthorisationRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, GetServiceAuthorisationRequestXML, e);

                GetServiceAuthorisationRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetServiceAuthorisationRequestText, ..., out GetServiceAuthorisationRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="GetServiceAuthorisationRequestText">The text to parse.</param>
        /// <param name="CustomSendGetServiceAuthorisationRequestParser">An optional delegate to parse custom GetServiceAuthorisationRequest XML elements.</param>
        /// <param name="GetServiceAuthorisationRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                   GetServiceAuthorisationRequestText,
                                       CustomXMLParserDelegate<GetServiceAuthorisationRequest>  CustomSendGetServiceAuthorisationRequestParser,
                                       out GetServiceAuthorisationRequest                       GetServiceAuthorisationRequest,
                                       OnExceptionDelegate                                      OnException         = null,

                                       HTTPRequest                                              HTTPRequest         = null,
                                       DateTime?                                                Timestamp           = null,
                                       CancellationToken                                        CancellationToken   = default,
                                       EventTracking_Id                                         EventTrackingId     = null,
                                       TimeSpan?                                                RequestTimeout      = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetServiceAuthorisationRequestText).Root,
                             CustomSendGetServiceAuthorisationRequestParser,
                             out GetServiceAuthorisationRequest,
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
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, GetServiceAuthorisationRequestText, e);
            }

            GetServiceAuthorisationRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomGetServiceAuthorisationRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomGetServiceAuthorisationRequestSerializer">A delegate to serialize custom set EVSE busy status request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<GetServiceAuthorisationRequest> CustomGetServiceAuthorisationRequestSerializer = null)
        {

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_FromIOP_GetServiceAuthorisationRequest",

                          new XElement("transactionId",         TransactionId.          ToString()),

                          new XElement("partnerIdType",         PartnerId.Format.       AsText()),
                          new XElement("partnerId",             PartnerId.              ToString()),

                          new XElement("operatorIdType",        OperatorId.Format.      AsText()),
                          new XElement("operatorId",            OperatorId.             ToString()),

                          new XElement("targetOperatorIdType",  TargetOperatorId.Format.AsText()),
                          new XElement("targetOperatorId",      TargetOperatorId.       ToString()),

                          new XElement("EVSEIdType",            EVSEId.Format.          AsText()),
                          new XElement("EVSEId",                EVSEId.                 ToString()),

                          new XElement("userIdType",            UserId.Format.          AsText()),
                          new XElement("userId",                UserId.                 ToString()),

                          new XElement("requestedServiceId",    RequestedServiceId.     ToString()),

                          ServiceSessionId.HasValue
                              ? new XElement("serviceSessionId",    ServiceSessionId.Value.ToString())
                              : null,

                          BookingId.HasValue
                              ? new XElement("bookingId",           BookingId.       Value.ToString())
                              : null

                      );


            return CustomGetServiceAuthorisationRequestSerializer != null
                       ? CustomGetServiceAuthorisationRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (GetServiceAuthorisationRequest1, GetServiceAuthorisationRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="GetServiceAuthorisationRequest1">A heartbeat request.</param>
        /// <param name="GetServiceAuthorisationRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetServiceAuthorisationRequest GetServiceAuthorisationRequest1, GetServiceAuthorisationRequest GetServiceAuthorisationRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetServiceAuthorisationRequest1, GetServiceAuthorisationRequest2))
                return true;

            // If one is null, but not both, return false.
            if ((GetServiceAuthorisationRequest1 is null) || (GetServiceAuthorisationRequest2 is null))
                return false;

            return GetServiceAuthorisationRequest1.Equals(GetServiceAuthorisationRequest2);

        }

        #endregion

        #region Operator != (GetServiceAuthorisationRequest1, GetServiceAuthorisationRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="GetServiceAuthorisationRequest1">A heartbeat request.</param>
        /// <param name="GetServiceAuthorisationRequest2">Another heartbeat request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetServiceAuthorisationRequest GetServiceAuthorisationRequest1, GetServiceAuthorisationRequest GetServiceAuthorisationRequest2)

            => !(GetServiceAuthorisationRequest1 == GetServiceAuthorisationRequest2);

        #endregion

        #endregion

        #region IEquatable<GetServiceAuthorisationRequest> Members

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

            if (!(Object is GetServiceAuthorisationRequest))
                return false;

            return Equals(Object as GetServiceAuthorisationRequest);

        }

        #endregion

        #region Equals(GetServiceAuthorisationRequest)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="GetServiceAuthorisationRequest">A heartbeat request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetServiceAuthorisationRequest GetServiceAuthorisationRequest)
        {

            if (GetServiceAuthorisationRequest is null)
                return false;

            return TransactionId.     Equals(GetServiceAuthorisationRequest.TransactionId)      &&
                   PartnerId.         Equals(GetServiceAuthorisationRequest.PartnerId)          &&
                   OperatorId.        Equals(GetServiceAuthorisationRequest.OperatorId)         &&
                   TargetOperatorId.  Equals(GetServiceAuthorisationRequest.TargetOperatorId)   &&
                   EVSEId.            Equals(GetServiceAuthorisationRequest.EVSEId)             &&
                   UserId.            Equals(GetServiceAuthorisationRequest.UserId)             &&
                   RequestedServiceId.Equals(GetServiceAuthorisationRequest.RequestedServiceId) &&

                   ((!ServiceSessionId.HasValue && !GetServiceAuthorisationRequest.ServiceSessionId.HasValue) ||
                     (ServiceSessionId.HasValue &&  GetServiceAuthorisationRequest.ServiceSessionId.HasValue && ServiceSessionId.Value.Equals(GetServiceAuthorisationRequest.ServiceSessionId.Value))) &&

                   ((!BookingId.       HasValue && !GetServiceAuthorisationRequest.BookingId.       HasValue) ||
                     (BookingId.       HasValue &&  GetServiceAuthorisationRequest.BookingId.       HasValue && BookingId.       Value.Equals(GetServiceAuthorisationRequest.BookingId.       Value)));

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

                return TransactionId.     GetHashCode() * 21 ^
                       PartnerId.         GetHashCode() * 19 ^
                       OperatorId.        GetHashCode() * 17 ^
                       TargetOperatorId.  GetHashCode() * 13 ^
                       EVSEId.            GetHashCode() * 11 ^
                       UserId.            GetHashCode() *  7 ^
                       RequestedServiceId.GetHashCode() *  5 ^

                       (ServiceSessionId.HasValue
                            ? ServiceSessionId.GetHashCode() * 3
                            : 0) ^

                       (BookingId.HasValue
                            ? BookingId.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(PartnerId,        " / ",
                             OperatorId,       ": ",
                             TargetOperatorId, ": ",
                             UserId,
                             " (" + RequestedServiceId + ") @ ",
                             EVSEId);

        #endregion

    }

}
