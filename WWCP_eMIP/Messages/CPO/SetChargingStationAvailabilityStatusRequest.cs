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
using System.Xml.Linq;
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A SetChargingPoolAvailabilityStatus request.
    /// </summary>
    public class SetChargingStationAvailabilityStatusRequest : ARequest<SetChargingStationAvailabilityStatusRequest>
    {

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id                             OperatorId                   { get; }

        /// <summary>
        /// The charging station identification.
        /// </summary>
        public ChargingStation_Id                      ChargingStationId            { get; }

        /// <summary>
        /// The timestamp of the status change.
        /// </summary>
        public DateTime                                StatusEventDate              { get; }

        /// <summary>
        /// The charging station availability status.
        /// </summary>
        public ChargingStationAvailabilityStatusTypes  AvailabilityStatus           { get; }

        /// <summary>
        /// The optional timestamp until which the given availability status is valid.
        /// </summary>
        public DateTime?                               AvailabilityStatusUntil      { get; }

        /// <summary>
        /// The optional comment about the availability status.
        /// </summary>
        public String                                  AvailabilityStatusComment    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a SetChargingStationAvailabilityStatus XML/SOAP request.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="ChargingStationId">The charging station identification.</param>
        /// <param name="StatusEventDate">The timestamp of the status update.</param>
        /// <param name="AvailabilityStatus">The charging station availability status.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="AvailabilityStatusUntil">An optional timestamp until which the given availability status is valid.</param>
        /// <param name="AvailabilityStatusComment">An optional comment about the availability status.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetChargingStationAvailabilityStatusRequest(Partner_Id                              PartnerId,
                                                           Operator_Id                             OperatorId,
                                                           ChargingStation_Id                      ChargingStationId,
                                                           DateTime                                StatusEventDate,
                                                           ChargingStationAvailabilityStatusTypes  AvailabilityStatus,
                                                           Transaction_Id?                         TransactionId               = null,
                                                           DateTime?                               AvailabilityStatusUntil     = null,
                                                           String?                                 AvailabilityStatusComment   = null,

                                                           HTTPRequest?                            HTTPRequest                 = null,
                                                           DateTime?                               Timestamp                   = null,
                                                           CancellationToken                       CancellationToken           = default,
                                                           EventTracking_Id?                       EventTrackingId             = null,
                                                           TimeSpan?                               RequestTimeout              = null)

            : base(HTTPRequest,
                   PartnerId,
                   TransactionId,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.OperatorId                 = OperatorId;
            this.ChargingStationId          = ChargingStationId;
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
        //     <eMIP:eMIP_ToIOP_SetChargingStationAvailabilityStatusRequest>
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
        //       <ChargingStationIdType>eMI3</ChargingStationIdType>
        //       <ChargingStationId>?</ChargingStationId>
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
        //     </eMIP:eMIP_ToIOP_SetChargingStationAvailabilityStatusRequest>
        //   </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (SetChargingStationAvailabilityStatusRequestXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargingStationAvailabilityStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargingStationAvailabilityStatusRequestParser">An optional delegate to parse custom SetChargingStationAvailabilityStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetChargingStationAvailabilityStatusRequest Parse(XElement                                                              SetChargingStationAvailabilityStatusRequestXML,
                                                                        CustomXMLParserDelegate<SetChargingStationAvailabilityStatusRequest>  CustomSendSetChargingStationAvailabilityStatusRequestParser,
                                                                        OnExceptionDelegate                                                   OnException         = null,

                                                                        HTTPRequest                                                           HTTPRequest         = null,
                                                                        DateTime?                                                             Timestamp           = null,
                                                                        CancellationToken                                                     CancellationToken   = default,
                                                                        EventTracking_Id                                                      EventTrackingId     = null,
                                                                        TimeSpan?                                                             RequestTimeout      = null)
        {

            if (TryParse(SetChargingStationAvailabilityStatusRequestXML,
                         CustomSendSetChargingStationAvailabilityStatusRequestParser,
                         out SetChargingStationAvailabilityStatusRequest _SetChargingStationAvailabilityStatusRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetChargingStationAvailabilityStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (SetChargingStationAvailabilityStatusRequestText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargingStationAvailabilityStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetChargingStationAvailabilityStatusRequestParser">An optional delegate to parse custom SetChargingStationAvailabilityStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetChargingStationAvailabilityStatusRequest Parse(String                                                                SetChargingStationAvailabilityStatusRequestText,
                                                                        CustomXMLParserDelegate<SetChargingStationAvailabilityStatusRequest>  CustomSendSetChargingStationAvailabilityStatusRequestParser,
                                                                        OnExceptionDelegate                                                   OnException         = null,

                                                                        HTTPRequest                                                           HTTPRequest         = null,
                                                                        DateTime?                                                             Timestamp           = null,
                                                                        CancellationToken                                                     CancellationToken   = default,
                                                                        EventTracking_Id                                                      EventTrackingId     = null,
                                                                        TimeSpan?                                                             RequestTimeout      = null)
        {

            if (TryParse(SetChargingStationAvailabilityStatusRequestText,
                         CustomSendSetChargingStationAvailabilityStatusRequestParser,
                         out SetChargingStationAvailabilityStatusRequest _SetChargingStationAvailabilityStatusRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetChargingStationAvailabilityStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SetChargingStationAvailabilityStatusRequestXML,  ..., out SetChargingStationAvailabilityStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargingStationAvailabilityStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargingStationAvailabilityStatusRequestParser">An optional delegate to parse custom SetChargingStationAvailabilityStatusRequest XML elements.</param>
        /// <param name="SetChargingStationAvailabilityStatusRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                                              SetChargingStationAvailabilityStatusRequestXML,
                                       CustomXMLParserDelegate<SetChargingStationAvailabilityStatusRequest>  CustomSendSetChargingStationAvailabilityStatusRequestParser,
                                       out SetChargingStationAvailabilityStatusRequest                       SetChargingStationAvailabilityStatusRequest,
                                       OnExceptionDelegate                                                   OnException         = null,

                                       HTTPRequest                                                           HTTPRequest         = null,
                                       DateTime?                                                             Timestamp           = null,
                                       CancellationToken                                                     CancellationToken   = default,
                                       EventTracking_Id                                                      EventTrackingId     = null,
                                       TimeSpan?                                                             RequestTimeout      = null)
        {

            try
            {

                SetChargingStationAvailabilityStatusRequest = new SetChargingStationAvailabilityStatusRequest(

                                                                  //ToDo: What to do with: <partnerIdType>eMI3</partnerIdType>?

                                                                  SetChargingStationAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "partnerId",
                                                                                                                                    Partner_Id.Parse),

                                                                  //ToDo: What to do with: <operatorIdType>eMI3</operatorIdType>

                                                                  SetChargingStationAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "operatorId",
                                                                                                                                    Operator_Id.Parse),

                                                                  //ToDo: What to do with: <ChargingStationIdType>eMI3</ChargingStationIdType>

                                                                  SetChargingStationAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "ChargingStationId",
                                                                                                                                    ChargingStation_Id.Parse),

                                                                  SetChargingStationAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "statusEventDate",
                                                                                                                                    DateTime.Parse),

                                                                  SetChargingStationAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "availabilityStatus",
                                                                                                                                    ConversionMethods.AsChargingStationAvailabilityStatusTypes),

                                                                  SetChargingStationAvailabilityStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "transactionId",
                                                                                                                                    Transaction_Id.Parse),

                                                                  SetChargingStationAvailabilityStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "availabilityStatusUntil",
                                                                                                                                    DateTime.Parse),

                                                                  SetChargingStationAvailabilityStatusRequestXML.MapValueOrNull    (eMIPNS.EVCIDynamic + "availabilityStatusComment"),

                                                                  HTTPRequest,
                                                                  Timestamp,
                                                                  CancellationToken,
                                                                  EventTrackingId,
                                                                  RequestTimeout

                                                              );


                if (CustomSendSetChargingStationAvailabilityStatusRequestParser is not null)
                    SetChargingStationAvailabilityStatusRequest = CustomSendSetChargingStationAvailabilityStatusRequestParser(SetChargingStationAvailabilityStatusRequestXML,
                                                                                                                              SetChargingStationAvailabilityStatusRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetChargingStationAvailabilityStatusRequestXML, e);

                SetChargingStationAvailabilityStatusRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetChargingStationAvailabilityStatusRequestText, ..., out SetChargingStationAvailabilityStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargingStationAvailabilityStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetChargingStationAvailabilityStatusRequestParser">An optional delegate to parse custom SetChargingStationAvailabilityStatusRequest XML elements.</param>
        /// <param name="SetChargingStationAvailabilityStatusRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                                SetChargingStationAvailabilityStatusRequestText,
                                       CustomXMLParserDelegate<SetChargingStationAvailabilityStatusRequest>  CustomSendSetChargingStationAvailabilityStatusRequestParser,
                                       out SetChargingStationAvailabilityStatusRequest                       SetChargingStationAvailabilityStatusRequest,
                                       OnExceptionDelegate                                                   OnException         = null,

                                       HTTPRequest                                                           HTTPRequest         = null,
                                       DateTime?                                                             Timestamp           = null,
                                       CancellationToken                                                     CancellationToken   = default,
                                       EventTracking_Id                                                      EventTrackingId     = null,
                                       TimeSpan?                                                             RequestTimeout      = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetChargingStationAvailabilityStatusRequestText).Root,
                             CustomSendSetChargingStationAvailabilityStatusRequestParser,
                             out SetChargingStationAvailabilityStatusRequest,
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
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetChargingStationAvailabilityStatusRequestText, e);
            }

            SetChargingStationAvailabilityStatusRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetChargingStationAvailabilityStatusRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetChargingStationAvailabilityStatusRequestSerializer">A delegate to serialize custom set ChargingStation availability status request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetChargingStationAvailabilityStatusRequest> CustomSetChargingStationAvailabilityStatusRequestSerializer = null)
        {

            var XML = new XElement(eMIPNS.EVCIDynamic + "eMIP_ToIOP_SetChargingStationAvailabilityStatusRequest",

                          TransactionId.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "transactionId",              TransactionId.                ToString())
                              : null,

                          new XElement(eMIPNS.EVCIDynamic + "partnerIdType",                    PartnerId.Format.             ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "partnerId",                        PartnerId.                    ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "operatorIdType",                   OperatorId.Format.            ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "operatorId",                       OperatorId.                   ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "ChargingStationIdType",            ChargingStationId.Format.     ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "ChargingStationId",                ChargingStationId.            ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "statusEventDate",                  StatusEventDate.              ToISO8601(false)),
                          new XElement(eMIPNS.EVCIDynamic + "availabilityStatus",               AvailabilityStatus.           AsNumber()),

                          AvailabilityStatusUntil.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "availabilityStatusUntil",    AvailabilityStatusUntil.Value.ToISO8601(false))
                              : null,

                          AvailabilityStatusComment.IsNeitherNullNorEmpty()
                              ? new XElement(eMIPNS.EVCIDynamic + "availabilityStatusComment",  AvailabilityStatusComment)
                              : null

                      );


            return CustomSetChargingStationAvailabilityStatusRequestSerializer is not null
                       ? CustomSetChargingStationAvailabilityStatusRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetChargingStationAvailabilityStatusRequest1, SetChargingStationAvailabilityStatusRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetChargingStationAvailabilityStatusRequest1">A heartbeat request.</param>
        /// <param name="SetChargingStationAvailabilityStatusRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetChargingStationAvailabilityStatusRequest SetChargingStationAvailabilityStatusRequest1, SetChargingStationAvailabilityStatusRequest SetChargingStationAvailabilityStatusRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetChargingStationAvailabilityStatusRequest1, SetChargingStationAvailabilityStatusRequest2))
                return true;

            // If one is null, but not both, return false.
            if (SetChargingStationAvailabilityStatusRequest1 is null || SetChargingStationAvailabilityStatusRequest2 is null)
                return false;

            return SetChargingStationAvailabilityStatusRequest1.Equals(SetChargingStationAvailabilityStatusRequest2);

        }

        #endregion

        #region Operator != (SetChargingStationAvailabilityStatusRequest1, SetChargingStationAvailabilityStatusRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="SetChargingStationAvailabilityStatusRequest1">A heartbeat request.</param>
        /// <param name="SetChargingStationAvailabilityStatusRequest2">Another heartbeat request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetChargingStationAvailabilityStatusRequest SetChargingStationAvailabilityStatusRequest1, SetChargingStationAvailabilityStatusRequest SetChargingStationAvailabilityStatusRequest2)

            => !(SetChargingStationAvailabilityStatusRequest1 == SetChargingStationAvailabilityStatusRequest2);

        #endregion

        #endregion

        #region IEquatable<SetChargingStationAvailabilityStatusRequest> Members

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

            var SetChargingStationAvailabilityStatusRequest = Object as SetChargingStationAvailabilityStatusRequest;
            if ((Object) SetChargingStationAvailabilityStatusRequest == null)
                return false;

            return Equals(SetChargingStationAvailabilityStatusRequest);

        }

        #endregion

        #region Equals(SetChargingStationAvailabilityStatusRequest)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetChargingStationAvailabilityStatusRequest">A heartbeat request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetChargingStationAvailabilityStatusRequest SetChargingStationAvailabilityStatusRequest)
        {

            if ((Object) SetChargingStationAvailabilityStatusRequest == null)
                return false;

            return ((!TransactionId.HasValue && !SetChargingStationAvailabilityStatusRequest.TransactionId.HasValue) ||
                     (TransactionId.HasValue && SetChargingStationAvailabilityStatusRequest.TransactionId.HasValue && TransactionId.Value.Equals(SetChargingStationAvailabilityStatusRequest.TransactionId.Value))) &&

                   PartnerId.         Equals(SetChargingStationAvailabilityStatusRequest.PartnerId)          &&
                   OperatorId.        Equals(SetChargingStationAvailabilityStatusRequest.OperatorId)         &&
                   ChargingStationId. Equals(SetChargingStationAvailabilityStatusRequest.ChargingStationId)  &&
                   StatusEventDate.   Equals(SetChargingStationAvailabilityStatusRequest.StatusEventDate)    &&
                   AvailabilityStatus.Equals(SetChargingStationAvailabilityStatusRequest.AvailabilityStatus) &&

                   ((!AvailabilityStatusUntil.HasValue && !SetChargingStationAvailabilityStatusRequest.AvailabilityStatusUntil.HasValue) ||
                     (AvailabilityStatusUntil.HasValue &&  SetChargingStationAvailabilityStatusRequest.AvailabilityStatusUntil.HasValue && AvailabilityStatusUntil.Value.Equals(SetChargingStationAvailabilityStatusRequest.AvailabilityStatusUntil.Value))) &&

                   ((!AvailabilityStatusComment.IsNeitherNullNorEmpty() && !SetChargingStationAvailabilityStatusRequest.AvailabilityStatusComment.IsNeitherNullNorEmpty()) ||
                     (AvailabilityStatusComment.IsNeitherNullNorEmpty() &&  SetChargingStationAvailabilityStatusRequest.AvailabilityStatusComment.IsNeitherNullNorEmpty() && AvailabilityStatusComment.Equals(SetChargingStationAvailabilityStatusRequest.AvailabilityStatusComment)));

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
                            ? TransactionId.            GetHashCode() * 19
                            : 0) ^

                       PartnerId.                       GetHashCode() * 17 ^
                       OperatorId.                      GetHashCode() * 13 ^
                       ChargingStationId.               GetHashCode() * 11 ^
                       StatusEventDate.                 GetHashCode() *  7 ^
                       AvailabilityStatus.              GetHashCode() *  5 ^

                       (AvailabilityStatusUntil.HasValue
                            ? AvailabilityStatusUntil.  GetHashCode() * 3
                            : 0) ^

                       (AvailabilityStatusComment.IsNeitherNullNorEmpty()
                            ? AvailabilityStatusComment.GetHashCode()
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
                             ChargingStationId, " -> ", AvailabilityStatus.AsText());

        #endregion

    }

}
