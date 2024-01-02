/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    public class SetChargingPoolAvailabilityStatusRequest : ARequest<SetChargingPoolAvailabilityStatusRequest>
    {

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id                          OperatorId                   { get; }

        /// <summary>
        /// The charging pool identification.
        /// </summary>
        public ChargingPool_Id                      ChargingPoolId               { get; }

        /// <summary>
        /// The timestamp of the status change.
        /// </summary>
        public DateTime                             StatusEventDate              { get; }

        /// <summary>
        /// The charging pool availability status.
        /// </summary>
        public ChargingPoolAvailabilityStatusTypes  AvailabilityStatus           { get; }

        /// <summary>
        /// The optional timestamp until which the given availability status is valid.
        /// </summary>
        public DateTime?                            AvailabilityStatusUntil      { get; }

        /// <summary>
        /// The optional comment about the availability status.
        /// </summary>
        public String                               AvailabilityStatusComment    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a SetChargingPoolAvailabilityStatus XML/SOAP request.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="ChargingPoolId">The charging pool identification.</param>
        /// <param name="StatusEventDate">The timestamp of the status update.</param>
        /// <param name="AvailabilityStatus">The charging pool availability status.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="AvailabilityStatusUntil">An optional timestamp until which the given availability status is valid.</param>
        /// <param name="AvailabilityStatusComment">An optional comment about the availability status.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetChargingPoolAvailabilityStatusRequest(Partner_Id                           PartnerId,
                                                        Operator_Id                          OperatorId,
                                                        ChargingPool_Id                      ChargingPoolId,
                                                        DateTime                             StatusEventDate,
                                                        ChargingPoolAvailabilityStatusTypes  AvailabilityStatus,
                                                        Transaction_Id?                      TransactionId               = null,
                                                        DateTime?                            AvailabilityStatusUntil     = null,
                                                        String?                              AvailabilityStatusComment   = null,

                                                        HTTPRequest?                         HTTPRequest                 = null,
                                                        DateTime?                            Timestamp                   = null,
                                                        CancellationToken                    CancellationToken           = default,
                                                        EventTracking_Id?                    EventTrackingId             = null,
                                                        TimeSpan?                            RequestTimeout              = null)

            : base(HTTPRequest,
                   PartnerId,
                   TransactionId,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.OperatorId                 = OperatorId;
            this.ChargingPoolId             = ChargingPoolId;
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
        //     <eMIP:eMIP_ToIOP_SetChargingPoolAvailabilityStatusRequest>
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
        //       <ChargingPoolIdType>eMI3</ChargingPoolIdType>
        //       <ChargingPoolId>?</ChargingPoolId>
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
        //     </eMIP:eMIP_ToIOP_SetChargingPoolAvailabilityStatusRequest>
        //   </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (SetChargingPoolAvailabilityStatusRequestXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargingPoolAvailabilityStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargingPoolAvailabilityStatusRequestParser">An optional delegate to parse custom SetChargingPoolAvailabilityStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetChargingPoolAvailabilityStatusRequest Parse(XElement                                                           SetChargingPoolAvailabilityStatusRequestXML,
                                                                     CustomXMLParserDelegate<SetChargingPoolAvailabilityStatusRequest>  CustomSendSetChargingPoolAvailabilityStatusRequestParser,
                                                                     OnExceptionDelegate                                                OnException         = null,

                                                                     HTTPRequest                                                        HTTPRequest         = null,
                                                                     DateTime?                                                          Timestamp           = null,
                                                                     CancellationToken                                                  CancellationToken   = default,
                                                                     EventTracking_Id                                                   EventTrackingId     = null,
                                                                     TimeSpan?                                                          RequestTimeout      = null)
        {

            if (TryParse(SetChargingPoolAvailabilityStatusRequestXML,
                         CustomSendSetChargingPoolAvailabilityStatusRequestParser,
                         out SetChargingPoolAvailabilityStatusRequest _SetChargingPoolAvailabilityStatusRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetChargingPoolAvailabilityStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (SetChargingPoolAvailabilityStatusRequestText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargingPoolAvailabilityStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetChargingPoolAvailabilityStatusRequestParser">An optional delegate to parse custom SetChargingPoolAvailabilityStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetChargingPoolAvailabilityStatusRequest Parse(String                                                             SetChargingPoolAvailabilityStatusRequestText,
                                                                     CustomXMLParserDelegate<SetChargingPoolAvailabilityStatusRequest>  CustomSendSetChargingPoolAvailabilityStatusRequestParser,
                                                                     OnExceptionDelegate                                                OnException         = null,

                                                                     HTTPRequest                                                        HTTPRequest         = null,
                                                                     DateTime?                                                          Timestamp           = null,
                                                                     CancellationToken                                                  CancellationToken   = default,
                                                                     EventTracking_Id                                                   EventTrackingId     = null,
                                                                     TimeSpan?                                                          RequestTimeout      = null)
        {

            if (TryParse(SetChargingPoolAvailabilityStatusRequestText,
                         CustomSendSetChargingPoolAvailabilityStatusRequestParser,
                         out SetChargingPoolAvailabilityStatusRequest _SetChargingPoolAvailabilityStatusRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetChargingPoolAvailabilityStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SetChargingPoolAvailabilityStatusRequestXML,  ..., out SetChargingPoolAvailabilityStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargingPoolAvailabilityStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargingPoolAvailabilityStatusRequestParser">An optional delegate to parse custom SetChargingPoolAvailabilityStatusRequest XML elements.</param>
        /// <param name="SetChargingPoolAvailabilityStatusRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                                           SetChargingPoolAvailabilityStatusRequestXML,
                                       CustomXMLParserDelegate<SetChargingPoolAvailabilityStatusRequest>  CustomSendSetChargingPoolAvailabilityStatusRequestParser,
                                       out SetChargingPoolAvailabilityStatusRequest                       SetChargingPoolAvailabilityStatusRequest,
                                       OnExceptionDelegate                                                OnException         = null,

                                       HTTPRequest                                                        HTTPRequest         = null,
                                       DateTime?                                                          Timestamp           = null,
                                       CancellationToken                                                  CancellationToken   = default,
                                       EventTracking_Id                                                   EventTrackingId     = null,
                                       TimeSpan?                                                          RequestTimeout      = null)
        {

            try
            {

                SetChargingPoolAvailabilityStatusRequest = new SetChargingPoolAvailabilityStatusRequest(

                                                                  //ToDo: What to do with: <partnerIdType>eMI3</partnerIdType>?

                                                                  SetChargingPoolAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "partnerId",
                                                                                                                                    Partner_Id.Parse),

                                                                  //ToDo: What to do with: <operatorIdType>eMI3</operatorIdType>

                                                                  SetChargingPoolAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "operatorId",
                                                                                                                                    Operator_Id.Parse),

                                                                  //ToDo: What to do with: <ChargingPoolIdType>eMI3</ChargingPoolIdType>

                                                                  SetChargingPoolAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "ChargingPoolId",
                                                                                                                                    ChargingPool_Id.Parse),

                                                                  SetChargingPoolAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "statusEventDate",
                                                                                                                                    DateTime.Parse),

                                                                  SetChargingPoolAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "availabilityStatus",
                                                                                                                                    ConversionMethods.AsChargingPoolAvailabilityStatusTypes),

                                                                  SetChargingPoolAvailabilityStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "transactionId",
                                                                                                                                    Transaction_Id.Parse),

                                                                  SetChargingPoolAvailabilityStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "availabilityStatusUntil",
                                                                                                                                    DateTime.Parse),

                                                                  SetChargingPoolAvailabilityStatusRequestXML.MapValueOrNull    (eMIPNS.EVCIDynamic + "availabilityStatusComment"),

                                                                  HTTPRequest,
                                                                  Timestamp,
                                                                  CancellationToken,
                                                                  EventTrackingId,
                                                                  RequestTimeout

                                                              );


                if (CustomSendSetChargingPoolAvailabilityStatusRequestParser is not null)
                    SetChargingPoolAvailabilityStatusRequest = CustomSendSetChargingPoolAvailabilityStatusRequestParser(SetChargingPoolAvailabilityStatusRequestXML,
                                                                                                                        SetChargingPoolAvailabilityStatusRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetChargingPoolAvailabilityStatusRequestXML, e);

                SetChargingPoolAvailabilityStatusRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetChargingPoolAvailabilityStatusRequestText, ..., out SetChargingPoolAvailabilityStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargingPoolAvailabilityStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetChargingPoolAvailabilityStatusRequestParser">An optional delegate to parse custom SetChargingPoolAvailabilityStatusRequest XML elements.</param>
        /// <param name="SetChargingPoolAvailabilityStatusRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                             SetChargingPoolAvailabilityStatusRequestText,
                                       CustomXMLParserDelegate<SetChargingPoolAvailabilityStatusRequest>  CustomSendSetChargingPoolAvailabilityStatusRequestParser,
                                       out SetChargingPoolAvailabilityStatusRequest                       SetChargingPoolAvailabilityStatusRequest,
                                       OnExceptionDelegate                                                OnException         = null,

                                       HTTPRequest                                                        HTTPRequest         = null,
                                       DateTime?                                                          Timestamp           = null,
                                       CancellationToken                                                  CancellationToken   = default,
                                       EventTracking_Id                                                   EventTrackingId     = null,
                                       TimeSpan?                                                          RequestTimeout      = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetChargingPoolAvailabilityStatusRequestText).Root,
                             CustomSendSetChargingPoolAvailabilityStatusRequestParser,
                             out SetChargingPoolAvailabilityStatusRequest,
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
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetChargingPoolAvailabilityStatusRequestText, e);
            }

            SetChargingPoolAvailabilityStatusRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetChargingPoolAvailabilityStatusRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetChargingPoolAvailabilityStatusRequestSerializer">A delegate to serialize custom set ChargingPool availability status request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetChargingPoolAvailabilityStatusRequest> CustomSetChargingPoolAvailabilityStatusRequestSerializer = null)
        {

            var XML = new XElement(eMIPNS.EVCIDynamic + "eMIP_ToIOP_SetChargingPoolAvailabilityStatusRequest",

                          TransactionId.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "transactionId",              TransactionId.                ToString())
                              : null,

                          new XElement(eMIPNS.EVCIDynamic + "partnerIdType",                    PartnerId.Format.             ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "partnerId",                        PartnerId.                    ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "operatorIdType",                   OperatorId.Format.            ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "operatorId",                       OperatorId.                   ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "ChargingPoolIdType",               ChargingPoolId.Format.        ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "ChargingPoolId",                   ChargingPoolId.               ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "statusEventDate",                  StatusEventDate.              ToIso8601(false)),
                          new XElement(eMIPNS.EVCIDynamic + "availabilityStatus",               AvailabilityStatus.           AsNumber()),

                          AvailabilityStatusUntil.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "availabilityStatusUntil",    AvailabilityStatusUntil.Value.ToIso8601(false))
                              : null,

                          AvailabilityStatusComment.IsNeitherNullNorEmpty()
                              ? new XElement(eMIPNS.EVCIDynamic + "availabilityStatusComment",  AvailabilityStatusComment)
                              : null

                      );


            return CustomSetChargingPoolAvailabilityStatusRequestSerializer is not null
                       ? CustomSetChargingPoolAvailabilityStatusRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetChargingPoolAvailabilityStatusRequest1, SetChargingPoolAvailabilityStatusRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetChargingPoolAvailabilityStatusRequest1">A heartbeat request.</param>
        /// <param name="SetChargingPoolAvailabilityStatusRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetChargingPoolAvailabilityStatusRequest SetChargingPoolAvailabilityStatusRequest1, SetChargingPoolAvailabilityStatusRequest SetChargingPoolAvailabilityStatusRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetChargingPoolAvailabilityStatusRequest1, SetChargingPoolAvailabilityStatusRequest2))
                return true;

            // If one is null, but not both, return false.
            if (SetChargingPoolAvailabilityStatusRequest1 is null || SetChargingPoolAvailabilityStatusRequest2 is null)
                return false;

            return SetChargingPoolAvailabilityStatusRequest1.Equals(SetChargingPoolAvailabilityStatusRequest2);

        }

        #endregion

        #region Operator != (SetChargingPoolAvailabilityStatusRequest1, SetChargingPoolAvailabilityStatusRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="SetChargingPoolAvailabilityStatusRequest1">A heartbeat request.</param>
        /// <param name="SetChargingPoolAvailabilityStatusRequest2">Another heartbeat request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetChargingPoolAvailabilityStatusRequest SetChargingPoolAvailabilityStatusRequest1, SetChargingPoolAvailabilityStatusRequest SetChargingPoolAvailabilityStatusRequest2)

            => !(SetChargingPoolAvailabilityStatusRequest1 == SetChargingPoolAvailabilityStatusRequest2);

        #endregion

        #endregion

        #region IEquatable<SetChargingPoolAvailabilityStatusRequest> Members

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

            var SetChargingPoolAvailabilityStatusRequest = Object as SetChargingPoolAvailabilityStatusRequest;
            if ((Object) SetChargingPoolAvailabilityStatusRequest == null)
                return false;

            return Equals(SetChargingPoolAvailabilityStatusRequest);

        }

        #endregion

        #region Equals(SetChargingPoolAvailabilityStatusRequest)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetChargingPoolAvailabilityStatusRequest">A heartbeat request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetChargingPoolAvailabilityStatusRequest SetChargingPoolAvailabilityStatusRequest)
        {

            if ((Object) SetChargingPoolAvailabilityStatusRequest == null)
                return false;

            return ((!TransactionId.HasValue && !SetChargingPoolAvailabilityStatusRequest.TransactionId.HasValue) ||
                     (TransactionId.HasValue && SetChargingPoolAvailabilityStatusRequest.TransactionId.HasValue && TransactionId.Value.Equals(SetChargingPoolAvailabilityStatusRequest.TransactionId.Value))) &&

                   PartnerId.         Equals(SetChargingPoolAvailabilityStatusRequest.PartnerId)          &&
                   OperatorId.        Equals(SetChargingPoolAvailabilityStatusRequest.OperatorId)         &&
                   ChargingPoolId.    Equals(SetChargingPoolAvailabilityStatusRequest.ChargingPoolId)     &&
                   StatusEventDate.   Equals(SetChargingPoolAvailabilityStatusRequest.StatusEventDate)    &&
                   AvailabilityStatus.Equals(SetChargingPoolAvailabilityStatusRequest.AvailabilityStatus) &&

                   ((!AvailabilityStatusUntil.HasValue && !SetChargingPoolAvailabilityStatusRequest.AvailabilityStatusUntil.HasValue) ||
                     (AvailabilityStatusUntil.HasValue &&  SetChargingPoolAvailabilityStatusRequest.AvailabilityStatusUntil.HasValue && AvailabilityStatusUntil.Value.Equals(SetChargingPoolAvailabilityStatusRequest.AvailabilityStatusUntil.Value))) &&

                   ((!AvailabilityStatusComment.IsNeitherNullNorEmpty() && !SetChargingPoolAvailabilityStatusRequest.AvailabilityStatusComment.IsNeitherNullNorEmpty()) ||
                     (AvailabilityStatusComment.IsNeitherNullNorEmpty() &&  SetChargingPoolAvailabilityStatusRequest.AvailabilityStatusComment.IsNeitherNullNorEmpty() && AvailabilityStatusComment.Equals(SetChargingPoolAvailabilityStatusRequest.AvailabilityStatusComment)));

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

                       PartnerId.         GetHashCode() * 21 ^
                       OperatorId.        GetHashCode() * 17 ^
                       ChargingPoolId.    GetHashCode() * 13 ^
                       StatusEventDate.   GetHashCode() * 11 ^
                       AvailabilityStatus.GetHashCode() *  7 ^

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
                             ChargingPoolId, " -> ", AvailabilityStatus.AsText());

        #endregion

    }

}
