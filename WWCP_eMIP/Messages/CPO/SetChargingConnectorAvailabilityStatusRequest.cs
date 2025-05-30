﻿/*
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
using System.Xml.Linq;
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A SetChargingConnectorAvailabilityStatus request.
    /// </summary>
    public class SetChargingConnectorAvailabilityStatusRequest : ARequest<SetChargingConnectorAvailabilityStatusRequest>
    {

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id                               OperatorId                   { get; }

        /// <summary>
        /// The charging connector identification.
        /// </summary>
        public ChargingConnector_Id                      ChargingConnectorId          { get; }

        /// <summary>
        /// The timestamp of the status change.
        /// </summary>
        public DateTime                                  StatusEventDate              { get; }

        /// <summary>
        /// The charging connector availability status.
        /// </summary>
        public ChargingConnectorAvailabilityStatusTypes  AvailabilityStatus           { get; }

        /// <summary>
        /// The optional timestamp until which the given availability status is valid.
        /// </summary>
        public DateTime?                                 AvailabilityStatusUntil      { get; }

        /// <summary>
        /// The optional comment about the availability status.
        /// </summary>
        public String                                    AvailabilityStatusComment    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a SetChargingConnectorAvailabilityStatus XML/SOAP request.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="ChargingConnectorId">The charging connector identification.</param>
        /// <param name="StatusEventDate">The timestamp of the status update.</param>
        /// <param name="AvailabilityStatus">The charging connector availability status.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="AvailabilityStatusUntil">An optional timestamp until which the given availability status is valid.</param>
        /// <param name="AvailabilityStatusComment">An optional comment about the availability status.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetChargingConnectorAvailabilityStatusRequest(Partner_Id                                PartnerId,
                                                             Operator_Id                               OperatorId,
                                                             ChargingConnector_Id                      ChargingConnectorId,
                                                             DateTime                                  StatusEventDate,
                                                             ChargingConnectorAvailabilityStatusTypes  AvailabilityStatus,
                                                             Transaction_Id?                           TransactionId               = null,
                                                             DateTime?                                 AvailabilityStatusUntil     = null,
                                                             String?                                   AvailabilityStatusComment   = null,

                                                             HTTPRequest?                              HTTPRequest                 = null,
                                                             DateTime?                                 Timestamp                   = null,
                                                             CancellationToken                         CancellationToken           = default,
                                                             EventTracking_Id?                         EventTrackingId             = null,
                                                             TimeSpan?                                 RequestTimeout              = null)

            : base(HTTPRequest,
                   PartnerId,
                   TransactionId,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.OperatorId                 = OperatorId;
            this.ChargingConnectorId        = ChargingConnectorId;
            this.StatusEventDate            = StatusEventDate;
            this.AvailabilityStatus         = AvailabilityStatus;
            this.AvailabilityStatusUntil    = AvailabilityStatusUntil;
            this.AvailabilityStatusComment  = AvailabilityStatusComment?.Trim() ?? String.Empty;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/EVCIDynamicV1/">
        //
        //   <soap:Header />
        //
        //   <soap:Body>
        //     <eMIP:eMIP_ToIOP_SetChargingConnectorAvailabilityStatusRequest>
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
        //       <ChargingConnectorIdType>eMI3</ChargingConnectorIdType>
        //       <ChargingConnectorId>?</ChargingConnectorId>
        //
        //       <statusEventDate>?</statusEventDate>
        //       <availabilityStatus>?</availabilityStatus>
        //
        //       <!--Optional:-->
        //       <availabilityStatusUntil>?</availabilityStatusUntil>
        //
        //       <!--Optional:-->
        //       <availabilityStatusComment>?</availabilityStatusComment>
        //
        //     </eMIP:eMIP_ToIOP_SetChargingConnectorAvailabilityStatusRequest>
        //   </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (SetChargingConnectorAvailabilityStatusRequestXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargingConnectorAvailabilityStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargingConnectorAvailabilityStatusRequestParser">An optional delegate to parse custom SetChargingConnectorAvailabilityStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetChargingConnectorAvailabilityStatusRequest Parse(XElement                                                                SetChargingConnectorAvailabilityStatusRequestXML,
                                                                          CustomXMLParserDelegate<SetChargingConnectorAvailabilityStatusRequest>  CustomSendSetChargingConnectorAvailabilityStatusRequestParser,
                                                                          OnExceptionDelegate                                                     OnException         = null,

                                                                          HTTPRequest                                                             HTTPRequest         = null,
                                                                          DateTime?                                                               Timestamp           = null,
                                                                          CancellationToken                                                       CancellationToken   = default,
                                                                          EventTracking_Id                                                        EventTrackingId     = null,
                                                                          TimeSpan?                                                               RequestTimeout      = null)
        {

            if (TryParse(SetChargingConnectorAvailabilityStatusRequestXML,
                         CustomSendSetChargingConnectorAvailabilityStatusRequestParser,
                         out SetChargingConnectorAvailabilityStatusRequest _SetChargingConnectorAvailabilityStatusRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetChargingConnectorAvailabilityStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (SetChargingConnectorAvailabilityStatusRequestText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargingConnectorAvailabilityStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetChargingConnectorAvailabilityStatusRequestParser">An optional delegate to parse custom SetChargingConnectorAvailabilityStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetChargingConnectorAvailabilityStatusRequest Parse(String                                                                  SetChargingConnectorAvailabilityStatusRequestText,
                                                                          CustomXMLParserDelegate<SetChargingConnectorAvailabilityStatusRequest>  CustomSendSetChargingConnectorAvailabilityStatusRequestParser,
                                                                          OnExceptionDelegate                                                     OnException         = null,

                                                                          HTTPRequest                                                             HTTPRequest         = null,
                                                                          DateTime?                                                               Timestamp           = null,
                                                                          CancellationToken                                                       CancellationToken   = default,
                                                                          EventTracking_Id                                                        EventTrackingId     = null,
                                                                          TimeSpan?                                                               RequestTimeout      = null)
        {

            if (TryParse(SetChargingConnectorAvailabilityStatusRequestText,
                         CustomSendSetChargingConnectorAvailabilityStatusRequestParser,
                         out SetChargingConnectorAvailabilityStatusRequest _SetChargingConnectorAvailabilityStatusRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetChargingConnectorAvailabilityStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SetChargingConnectorAvailabilityStatusRequestXML,  ..., out SetChargingConnectorAvailabilityStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargingConnectorAvailabilityStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargingConnectorAvailabilityStatusRequestParser">An optional delegate to parse custom SetChargingConnectorAvailabilityStatusRequest XML elements.</param>
        /// <param name="SetChargingConnectorAvailabilityStatusRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                                                SetChargingConnectorAvailabilityStatusRequestXML,
                                       CustomXMLParserDelegate<SetChargingConnectorAvailabilityStatusRequest>  CustomSendSetChargingConnectorAvailabilityStatusRequestParser,
                                       out SetChargingConnectorAvailabilityStatusRequest                       SetChargingConnectorAvailabilityStatusRequest,
                                       OnExceptionDelegate                                                     OnException         = null,

                                       HTTPRequest                                                             HTTPRequest         = null,
                                       DateTime?                                                               Timestamp           = null,
                                       CancellationToken                                                       CancellationToken   = default,
                                       EventTracking_Id                                                        EventTrackingId     = null,
                                       TimeSpan?                                                               RequestTimeout      = null)
        {

            try
            {

                SetChargingConnectorAvailabilityStatusRequest = new SetChargingConnectorAvailabilityStatusRequest(

                                                                    //ToDo: What to do with: <partnerIdType>eMI3</partnerIdType>?

                                                                    SetChargingConnectorAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "partnerId",
                                                                                                                                        Partner_Id.Parse),

                                                                    //ToDo: What to do with: <operatorIdType>eMI3</operatorIdType>

                                                                    SetChargingConnectorAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "operatorId",
                                                                                                                                        Operator_Id.Parse),

                                                                    //ToDo: What to do with: <ChargingConnectorIdType>eMI3</ChargingConnectorIdType>

                                                                    SetChargingConnectorAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "ChargingConnectorId",
                                                                                                                                        ChargingConnector_Id.Parse),

                                                                    SetChargingConnectorAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "statusEventDate",
                                                                                                                                        DateTime.Parse),

                                                                    SetChargingConnectorAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "availabilityStatus",
                                                                                                                                        ConversionMethods.AsChargingConnectorAvailabilityStatusTypes),

                                                                    SetChargingConnectorAvailabilityStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "transactionId",
                                                                                                                                        Transaction_Id.Parse),

                                                                    SetChargingConnectorAvailabilityStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "availabilityStatusUntil",
                                                                                                                                        DateTime.Parse),

                                                                    SetChargingConnectorAvailabilityStatusRequestXML.MapValueOrNull    (eMIPNS.EVCIDynamic + "availabilityStatusComment"),

                                                                    HTTPRequest,
                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout

                                                                );


                if (CustomSendSetChargingConnectorAvailabilityStatusRequestParser is not null)
                    SetChargingConnectorAvailabilityStatusRequest = CustomSendSetChargingConnectorAvailabilityStatusRequestParser(SetChargingConnectorAvailabilityStatusRequestXML,
                                                                                                                                  SetChargingConnectorAvailabilityStatusRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetChargingConnectorAvailabilityStatusRequestXML, e);

                SetChargingConnectorAvailabilityStatusRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetChargingConnectorAvailabilityStatusRequestText, ..., out SetChargingConnectorAvailabilityStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargingConnectorAvailabilityStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetChargingConnectorAvailabilityStatusRequestParser">An optional delegate to parse custom SetChargingConnectorAvailabilityStatusRequest XML elements.</param>
        /// <param name="SetChargingConnectorAvailabilityStatusRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                                  SetChargingConnectorAvailabilityStatusRequestText,
                                       CustomXMLParserDelegate<SetChargingConnectorAvailabilityStatusRequest>  CustomSendSetChargingConnectorAvailabilityStatusRequestParser,
                                       out SetChargingConnectorAvailabilityStatusRequest                       SetChargingConnectorAvailabilityStatusRequest,
                                       OnExceptionDelegate                                                     OnException         = null,

                                       HTTPRequest                                                             HTTPRequest         = null,
                                       DateTime?                                                               Timestamp           = null,
                                       CancellationToken                                                       CancellationToken   = default,
                                       EventTracking_Id                                                        EventTrackingId     = null,
                                       TimeSpan?                                                               RequestTimeout      = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetChargingConnectorAvailabilityStatusRequestText).Root,
                             CustomSendSetChargingConnectorAvailabilityStatusRequestParser,
                             out SetChargingConnectorAvailabilityStatusRequest,
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
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetChargingConnectorAvailabilityStatusRequestText, e);
            }

            SetChargingConnectorAvailabilityStatusRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetChargingConnectorAvailabilityStatusRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetChargingConnectorAvailabilityStatusRequestSerializer">A delegate to serialize custom set ChargingConnector availability status request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetChargingConnectorAvailabilityStatusRequest> CustomSetChargingConnectorAvailabilityStatusRequestSerializer = null)
        {

            var XML = new XElement(eMIPNS.EVCIDynamic + "eMIP_ToIOP_SetChargingConnectorAvailabilityStatusRequest",

                          TransactionId.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "transactionId",              TransactionId.                ToString())
                              : null,

                          new XElement(eMIPNS.EVCIDynamic + "partnerIdType",                    PartnerId.Format.             ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "partnerId",                        PartnerId.                    ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "operatorIdType",                   OperatorId.Format.            ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "operatorId",                       OperatorId.                   ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "ChargingConnectorIdType",          ChargingConnectorId.Format.   ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "ChargingConnectorId",              ChargingConnectorId.          ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "statusEventDate",                  StatusEventDate.              ToISO8601(false)),
                          new XElement(eMIPNS.EVCIDynamic + "availabilityStatus",               AvailabilityStatus.           AsNumber()),

                          AvailabilityStatusUntil.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "availabilityStatusUntil",    AvailabilityStatusUntil.Value.ToISO8601(false))
                              : null,

                          AvailabilityStatusComment.IsNeitherNullNorEmpty()
                              ? new XElement(eMIPNS.EVCIDynamic + "availabilityStatusComment",  AvailabilityStatusComment)
                              : null

                      );


            return CustomSetChargingConnectorAvailabilityStatusRequestSerializer is not null
                       ? CustomSetChargingConnectorAvailabilityStatusRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetChargingConnectorAvailabilityStatusRequest1, SetChargingConnectorAvailabilityStatusRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetChargingConnectorAvailabilityStatusRequest1">A heartbeat request.</param>
        /// <param name="SetChargingConnectorAvailabilityStatusRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetChargingConnectorAvailabilityStatusRequest SetChargingConnectorAvailabilityStatusRequest1, SetChargingConnectorAvailabilityStatusRequest SetChargingConnectorAvailabilityStatusRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetChargingConnectorAvailabilityStatusRequest1, SetChargingConnectorAvailabilityStatusRequest2))
                return true;

            // If one is null, but not both, return false.
            if (SetChargingConnectorAvailabilityStatusRequest1 is null || SetChargingConnectorAvailabilityStatusRequest2 is null)
                return false;

            return SetChargingConnectorAvailabilityStatusRequest1.Equals(SetChargingConnectorAvailabilityStatusRequest2);

        }

        #endregion

        #region Operator != (SetChargingConnectorAvailabilityStatusRequest1, SetChargingConnectorAvailabilityStatusRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="SetChargingConnectorAvailabilityStatusRequest1">A heartbeat request.</param>
        /// <param name="SetChargingConnectorAvailabilityStatusRequest2">Another heartbeat request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetChargingConnectorAvailabilityStatusRequest SetChargingConnectorAvailabilityStatusRequest1, SetChargingConnectorAvailabilityStatusRequest SetChargingConnectorAvailabilityStatusRequest2)

            => !(SetChargingConnectorAvailabilityStatusRequest1 == SetChargingConnectorAvailabilityStatusRequest2);

        #endregion

        #endregion

        #region IEquatable<SetChargingConnectorAvailabilityStatusRequest> Members

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

            var SetChargingConnectorAvailabilityStatusRequest = Object as SetChargingConnectorAvailabilityStatusRequest;
            if ((Object) SetChargingConnectorAvailabilityStatusRequest == null)
                return false;

            return Equals(SetChargingConnectorAvailabilityStatusRequest);

        }

        #endregion

        #region Equals(SetChargingConnectorAvailabilityStatusRequest)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetChargingConnectorAvailabilityStatusRequest">A heartbeat request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetChargingConnectorAvailabilityStatusRequest SetChargingConnectorAvailabilityStatusRequest)
        {

            if ((Object) SetChargingConnectorAvailabilityStatusRequest == null)
                return false;

            return ((!TransactionId.HasValue && !SetChargingConnectorAvailabilityStatusRequest.TransactionId.HasValue) ||
                     (TransactionId.HasValue && SetChargingConnectorAvailabilityStatusRequest.TransactionId.HasValue && TransactionId.Value.Equals(SetChargingConnectorAvailabilityStatusRequest.TransactionId.Value))) &&

                   PartnerId.          Equals(SetChargingConnectorAvailabilityStatusRequest.PartnerId)           &&
                   OperatorId.         Equals(SetChargingConnectorAvailabilityStatusRequest.OperatorId)          &&
                   ChargingConnectorId.Equals(SetChargingConnectorAvailabilityStatusRequest.ChargingConnectorId) &&
                   StatusEventDate.    Equals(SetChargingConnectorAvailabilityStatusRequest.StatusEventDate)     &&
                   AvailabilityStatus. Equals(SetChargingConnectorAvailabilityStatusRequest.AvailabilityStatus)  &&

                   ((!AvailabilityStatusUntil.HasValue && !SetChargingConnectorAvailabilityStatusRequest.AvailabilityStatusUntil.HasValue) ||
                     (AvailabilityStatusUntil.HasValue &&  SetChargingConnectorAvailabilityStatusRequest.AvailabilityStatusUntil.HasValue && AvailabilityStatusUntil.Value.Equals(SetChargingConnectorAvailabilityStatusRequest.AvailabilityStatusUntil.Value))) &&

                   ((!AvailabilityStatusComment.IsNeitherNullNorEmpty() && !SetChargingConnectorAvailabilityStatusRequest.AvailabilityStatusComment.IsNeitherNullNorEmpty()) ||
                     (AvailabilityStatusComment.IsNeitherNullNorEmpty() &&  SetChargingConnectorAvailabilityStatusRequest.AvailabilityStatusComment.IsNeitherNullNorEmpty() && AvailabilityStatusComment.Equals(SetChargingConnectorAvailabilityStatusRequest.AvailabilityStatusComment)));

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
                            ? TransactionId.GetHashCode() * 23
                            : 0) ^

                       PartnerId.          GetHashCode() * 21 ^
                       OperatorId.         GetHashCode() * 17 ^
                       ChargingConnectorId.GetHashCode() * 13 ^
                       StatusEventDate.    GetHashCode() * 11 ^
                       AvailabilityStatus. GetHashCode() *  7 ^

                       (AvailabilityStatusUntil.HasValue
                            ? AvailabilityStatusUntil.  GetHashCode() * 5
                            : 0) ^

                       (AvailabilityStatusComment.IsNeitherNullNorEmpty()
                            ? AvailabilityStatusComment.GetHashCode() * 3
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
                             ChargingConnectorId, " -> ", AvailabilityStatus.AsText());

        #endregion

    }

}
