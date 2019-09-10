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
using System.Xml.Linq;
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A GetServiceAuthorisation request.
    /// </summary>
    public class GetServiceAuthorisationRequest : ARequest<GetServiceAuthorisationRequest>
    {

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id                OperatorId                 { get; }

        /// <summary>
        /// The EVSE identification.
        /// </summary>
        public EVSE_Id                    EVSEId                     { get; }

        /// <summary>
        /// The user identification.
        /// </summary>
        public User_Id                    UserId                     { get; }

        /// <summary>
        /// The service identification for which an authorisation is requested.
        /// </summary>
        public Service_Id                 RequestedServiceId         { get; }

        /// <summary>
        /// The optional partner session identification.
        /// </summary>
        public PartnerServiceSession_Id?  PartnerServiceSessionId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a GetServiceAuthorisation XML/SOAP request.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="UserId">The user identification.</param>
        /// <param name="RequestedServiceId">The service identification for which an authorisation is requested.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="PartnerServiceSessionId">An optional partner session identification.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public GetServiceAuthorisationRequest(Partner_Id                 PartnerId,
                                              Operator_Id                OperatorId,
                                              EVSE_Id                    EVSEId,
                                              User_Id                    UserId,
                                              Service_Id                 RequestedServiceId,
                                              Transaction_Id?            TransactionId             = null,
                                              PartnerServiceSession_Id?  PartnerServiceSessionId   = null,

                                              HTTPRequest                HTTPRequest               = null,
                                              DateTime?                  Timestamp                 = null,
                                              CancellationToken?         CancellationToken         = null,
                                              EventTracking_Id           EventTrackingId           = null,
                                              TimeSpan?                  RequestTimeout            = null)

            : base(HTTPRequest,
                   PartnerId,
                   TransactionId,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.OperatorId               = OperatorId;
            this.EVSEId                   = EVSEId;
            this.UserId                   = UserId;
            this.RequestedServiceId       = RequestedServiceId;
            this.PartnerServiceSessionId  = PartnerServiceSessionId;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //
        //   <soap:Header />
        //
        //   <soap:Body>
        //     <eMIP:eMIP_ToIOP_GetServiceAuthorisationRequest>
        //
        //       <!--Optional:-->
        //       <transactionId>TRANSACTION_46151</transactionId>
        //
        //       <partnerIdType>eMI3</partnerIdType>
        //       <partnerId>FR*MSP</partnerId>
        //
        //       <operatorIdType>eMI3</operatorIdType>
        //       <operatorId>FR*798</operatorId>
        //
        //       <EVSEIdType>eMI3</EVSEIdType>
        //       <EVSEId>?</EVSEId>
        //
        //       <userIdType>RFID-UID</userIdType>
        //       <userId>?</userId>
        //
        //       <!-- 1: charge -->
        //       <requestedServiceId>?</requestedServiceId>
        //
        //       <!--Optional:-->
        //       <partnerServiceSessionId>?</partnerServiceSessionId>
        //
        //     </eMIP:eMIP_ToIOP_GetServiceAuthorisationRequest>
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
                                                           CancellationToken?                                       CancellationToken   = null,
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
                                                           CancellationToken?                                       CancellationToken   = null,
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
                                       CancellationToken?                                       CancellationToken   = null,
                                       EventTracking_Id                                         EventTrackingId     = null,
                                       TimeSpan?                                                RequestTimeout      = null)
        {

            try
            {

                GetServiceAuthorisationRequest = new GetServiceAuthorisationRequest(

                                                     //ToDo: What to do with: <partnerIdType>eMI3</partnerIdType>?
                                                     GetServiceAuthorisationRequestXML.MapValueOrFail    ("partnerId",                Partner_Id.Parse),

                                                     //ToDo: What to do with: <operatorIdType>eMI3</operatorIdType>
                                                     GetServiceAuthorisationRequestXML.MapValueOrFail    ("operatorId",               Operator_Id.Parse),

                                                     //ToDo: What to do with: <EVSEIdType>eMI3</EVSEIdType>
                                                     GetServiceAuthorisationRequestXML.MapValueOrFail    ("EVSEId",                   EVSE_Id.Parse),

                                                     GetServiceAuthorisationRequestXML.MapValueOrFail    ("userId",                   s => User_Id.Parse(s,
                                                         GetServiceAuthorisationRequestXML.MapValueOrFail("userIdType", ConversionMethods.AsUserIdFormat))),

                                                     GetServiceAuthorisationRequestXML.MapValueOrFail    ("requestedServiceId",       Service_Id.Parse),
                                                     GetServiceAuthorisationRequestXML.MapValueOrNullable("transactionId",            Transaction_Id.Parse),
                                                     GetServiceAuthorisationRequestXML.MapValueOrNullable("partnerServiceSessionId",  PartnerServiceSession_Id.Parse),

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

                OnException?.Invoke(DateTime.UtcNow, GetServiceAuthorisationRequestXML, e);

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
                                       CancellationToken?                                       CancellationToken   = null,
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
                OnException?.Invoke(DateTime.UtcNow, GetServiceAuthorisationRequestText, e);
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

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_ToIOP_GetServiceAuthorisationRequest",

                          TransactionId.HasValue
                              ? new XElement("transactionId",            TransactionId.                ToString())
                              : null,

                          new XElement("partnerIdType",                  PartnerId.Format.             AsText()),
                          new XElement("partnerId",                      PartnerId.                    ToString()),

                          new XElement("operatorIdType",                 OperatorId.Format.            AsText()),
                          new XElement("operatorId",                     OperatorId.                   ToString()),

                          new XElement("EVSEIdType",                     EVSEId.Format.                AsText()),
                          new XElement("EVSEId",                         EVSEId.                       ToString()),

                          new XElement("userIdType",                     UserId.Format.                AsText()),
                          new XElement("userId",                         UserId.                       ToString()),

                          new XElement("requestedServiceId",             RequestedServiceId.           ToString()),

                          PartnerServiceSessionId.HasValue
                              ? new XElement("partnerServiceSessionId",  PartnerServiceSessionId.Value.ToString())
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
            if (Object.ReferenceEquals(GetServiceAuthorisationRequest1, GetServiceAuthorisationRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetServiceAuthorisationRequest1 == null) || ((Object) GetServiceAuthorisationRequest2 == null))
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

            if (Object == null)
                return false;

            var GetServiceAuthorisationRequest = Object as GetServiceAuthorisationRequest;
            if ((Object) GetServiceAuthorisationRequest == null)
                return false;

            return Equals(GetServiceAuthorisationRequest);

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

            if ((Object) GetServiceAuthorisationRequest == null)
                return false;

            return ((!TransactionId.HasValue && !GetServiceAuthorisationRequest.TransactionId.HasValue) ||
                     (TransactionId.HasValue && GetServiceAuthorisationRequest.TransactionId.HasValue && TransactionId.Value.Equals(GetServiceAuthorisationRequest.TransactionId.Value))) &&

                   PartnerId.         Equals(GetServiceAuthorisationRequest.PartnerId)          &&
                   OperatorId.        Equals(GetServiceAuthorisationRequest.OperatorId)         &&
                   EVSEId.            Equals(GetServiceAuthorisationRequest.EVSEId)             &&
                   UserId.            Equals(GetServiceAuthorisationRequest.UserId)             &&
                   RequestedServiceId.Equals(GetServiceAuthorisationRequest.RequestedServiceId) &&

                   ((!PartnerServiceSessionId.HasValue && !GetServiceAuthorisationRequest.PartnerServiceSessionId.HasValue) ||
                     (PartnerServiceSessionId.HasValue &&  GetServiceAuthorisationRequest.PartnerServiceSessionId.HasValue && PartnerServiceSessionId.Value.Equals(GetServiceAuthorisationRequest.PartnerServiceSessionId.Value)));

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
                            ? TransactionId.GetHashCode() * 19
                            : 0) ^

                       PartnerId.         GetHashCode() * 17 ^
                       OperatorId.        GetHashCode() * 13 ^
                       EVSEId.            GetHashCode() * 11 ^
                       UserId.            GetHashCode() *  7 ^
                       RequestedServiceId.GetHashCode() *  3 ^

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

            => String.Concat(PartnerId,  " / ",
                             OperatorId, ": ",
                             UserId,
                             " (" + RequestedServiceId + ") @ ",
                             EVSEId);

        #endregion

    }

}
