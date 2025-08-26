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
    /// A SetEVSEBusyStatus request.
    /// </summary>
    public class SetEVSEBusyStatusRequest : ARequest<SetEVSEBusyStatusRequest>
    {

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id          OperatorId           { get; }

        /// <summary>
        /// The EVSE identification.
        /// </summary>
        public EVSE_Id              EVSEId               { get; }

        /// <summary>
        /// The timestamp of the status change.
        /// </summary>
        public DateTimeOffset       StatusEventDate      { get; }

        /// <summary>
        /// The EVSE busy status.
        /// </summary>
        public EVSEBusyStatusTypes  BusyStatus           { get; }

        /// <summary>
        /// The optional timestamp until which the given busy status is valid.
        /// </summary>
        public DateTimeOffset?      BusyStatusUntil      { get; }

        /// <summary>
        /// The optional comment about the busy status.
        /// </summary>
        public String               BusyStatusComment    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a SetEVSEBusyStatus XML/SOAP request.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="StatusEventDate">The timestamp of the status update.</param>
        /// <param name="BusyStatus">The EVSE busy status.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="BusyStatusUntil">An optional timestamp until which the given busy status is valid.</param>
        /// <param name="BusyStatusComment">An optional comment about the busy status.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetEVSEBusyStatusRequest(Partner_Id           PartnerId,
                                        Operator_Id          OperatorId,
                                        EVSE_Id              EVSEId,
                                        DateTimeOffset       StatusEventDate,
                                        EVSEBusyStatusTypes  BusyStatus,
                                        Transaction_Id?      TransactionId       = null,
                                        DateTimeOffset?      BusyStatusUntil     = null,
                                        String?              BusyStatusComment   = null,

                                        HTTPRequest?         HTTPRequest         = null,
                                        DateTimeOffset?      Timestamp           = null,
                                        EventTracking_Id?    EventTrackingId     = null,
                                        TimeSpan?            RequestTimeout      = null,
                                        CancellationToken    CancellationToken   = default)

            : base(HTTPRequest,
                   PartnerId,
                   TransactionId,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.OperatorId         = OperatorId;
            this.EVSEId             = EVSEId;
            this.StatusEventDate    = StatusEventDate;
            this.BusyStatus         = BusyStatus;
            this.BusyStatusUntil    = BusyStatusUntil;
            this.BusyStatusComment  = BusyStatusComment?.Trim() ?? String.Empty;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/EVCIDynamicV1/">
        //
        //   <soap:Header />
        //
        //   <soap:Body>
        //     <eMIP:eMIP_ToIOP_SetEVSEBusyStatusRequest>
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
        //       <statusEventDate>?</statusEventDate>
        //       <busyStatus>?</busyStatus>
        //
        //       <!--Optional:-->
        //       <busyStatusUntil>?</busyStatusUntil>
        //
        //       <!--Optional:-->
        //       <busyStatusComment>?</busyStatusComment>
        //
        //     </eMIP:eMIP_ToIOP_SetEVSEBusyStatusRequest>
        //   </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (SetEVSEBusyStatusRequestXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP SetEVSEBusyStatus request.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSEBusyStatusRequestParser">An optional delegate to parse custom SetEVSEBusyStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetEVSEBusyStatusRequest Parse(XElement                                           SetEVSEBusyStatusRequestXML,
                                                     CustomXMLParserDelegate<SetEVSEBusyStatusRequest>  CustomSendSetEVSEBusyStatusRequestParser,
                                                     OnExceptionDelegate                                OnException         = null,

                                                     HTTPRequest                                        HTTPRequest         = null,
                                                     DateTimeOffset?                                    Timestamp           = null,
                                                     CancellationToken                                  CancellationToken   = default,
                                                     EventTracking_Id                                   EventTrackingId     = null,
                                                     TimeSpan?                                          RequestTimeout      = null)
        {

            if (TryParse(SetEVSEBusyStatusRequestXML,
                         CustomSendSetEVSEBusyStatusRequestParser,
                         out SetEVSEBusyStatusRequest _SetEVSEBusyStatusRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetEVSEBusyStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (SetEVSEBusyStatusRequestText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP SetEVSEBusyStatus request.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetEVSEBusyStatusRequestParser">An optional delegate to parse custom SetEVSEBusyStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetEVSEBusyStatusRequest Parse(String                                             SetEVSEBusyStatusRequestText,
                                                     CustomXMLParserDelegate<SetEVSEBusyStatusRequest>  CustomSendSetEVSEBusyStatusRequestParser,
                                                     OnExceptionDelegate                                OnException         = null,

                                                     HTTPRequest                                        HTTPRequest         = null,
                                                     DateTimeOffset?                                    Timestamp           = null,
                                                     CancellationToken                                  CancellationToken   = default,
                                                     EventTracking_Id                                   EventTrackingId     = null,
                                                     TimeSpan?                                          RequestTimeout      = null)
        {

            if (TryParse(SetEVSEBusyStatusRequestText,
                         CustomSendSetEVSEBusyStatusRequestParser,
                         out SetEVSEBusyStatusRequest _SetEVSEBusyStatusRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetEVSEBusyStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SetEVSEBusyStatusRequestXML,  ..., out SetEVSEBusyStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP SetEVSEBusyStatus request.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSEBusyStatusRequestParser">An optional delegate to parse custom SetEVSEBusyStatusRequest XML elements.</param>
        /// <param name="SetEVSEBusyStatusRequest">The parsed SetEVSEBusyStatus request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                           SetEVSEBusyStatusRequestXML,
                                       CustomXMLParserDelegate<SetEVSEBusyStatusRequest>  CustomSendSetEVSEBusyStatusRequestParser,
                                       out SetEVSEBusyStatusRequest                       SetEVSEBusyStatusRequest,
                                       OnExceptionDelegate                                OnException         = null,

                                       HTTPRequest                                        HTTPRequest         = null,
                                       DateTimeOffset?                                    Timestamp           = null,
                                       CancellationToken                                  CancellationToken   = default,
                                       EventTracking_Id                                   EventTrackingId     = null,
                                       TimeSpan?                                          RequestTimeout      = null)
        {

            try
            {

                SetEVSEBusyStatusRequest = new SetEVSEBusyStatusRequest(

                                               //ToDo: What to do with: <partnerIdType>eMI3</partnerIdType>?

                                               SetEVSEBusyStatusRequestXML.MapValueOrFail    ("partnerId",         Partner_Id.       Parse),

                                               //ToDo: What to do with: <operatorIdType>eMI3</operatorIdType>
                                               SetEVSEBusyStatusRequestXML.MapValueOrFail    ("operatorId",        Operator_Id.      Parse),

                                               //ToDo: What to do with: <EVSEIdType>eMI3</EVSEIdType>
                                               SetEVSEBusyStatusRequestXML.MapValueOrFail    ("EVSEId",            EVSE_Id.          Parse),

                                               SetEVSEBusyStatusRequestXML.MapValueOrFail    ("statusEventDate",   DateTime.         Parse),
                                               SetEVSEBusyStatusRequestXML.MapValueOrFail    ("busyStatus",        ConversionMethods.AsEVSEBusyStatusTypes),
                                               SetEVSEBusyStatusRequestXML.MapValueOrNullable("transactionId",     Transaction_Id.   Parse),
                                               SetEVSEBusyStatusRequestXML.MapValueOrNullable("busyStatusUntil",   DateTime.         Parse),

                                               SetEVSEBusyStatusRequestXML.MapValueOrNull    ("busyStatusComment"),

                                               HTTPRequest,
                                               Timestamp,
                                               EventTrackingId,
                                               RequestTimeout,
                                               CancellationToken

                                           );


                if (CustomSendSetEVSEBusyStatusRequestParser is not null)
                    SetEVSEBusyStatusRequest = CustomSendSetEVSEBusyStatusRequestParser(SetEVSEBusyStatusRequestXML,
                                                                                                        SetEVSEBusyStatusRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetEVSEBusyStatusRequestXML, e);

                SetEVSEBusyStatusRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetEVSEBusyStatusRequestText, ..., out SetEVSEBusyStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP SetEVSEBusyStatus request.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetEVSEBusyStatusRequestParser">An optional delegate to parse custom SetEVSEBusyStatusRequest XML elements.</param>
        /// <param name="SetEVSEBusyStatusRequest">The parsed SetEVSEBusyStatus request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                             SetEVSEBusyStatusRequestText,
                                       CustomXMLParserDelegate<SetEVSEBusyStatusRequest>  CustomSendSetEVSEBusyStatusRequestParser,
                                       out SetEVSEBusyStatusRequest                       SetEVSEBusyStatusRequest,
                                       OnExceptionDelegate                                OnException         = null,

                                       HTTPRequest                                        HTTPRequest         = null,
                                       DateTimeOffset?                                    Timestamp           = null,
                                       CancellationToken                                  CancellationToken   = default,
                                       EventTracking_Id                                   EventTrackingId     = null,
                                       TimeSpan?                                          RequestTimeout      = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetEVSEBusyStatusRequestText).Root,
                             CustomSendSetEVSEBusyStatusRequestParser,
                             out SetEVSEBusyStatusRequest,
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
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetEVSEBusyStatusRequestText, e);
            }

            SetEVSEBusyStatusRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetEVSEBusyStatusRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetEVSEBusyStatusRequestSerializer">A delegate to serialize custom set EVSE busy status request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetEVSEBusyStatusRequest> CustomSetEVSEBusyStatusRequestSerializer = null)
        {

            var XML = new XElement(eMIPNS.EVCIDynamic + "eMIP_ToIOP_SetEVSEBusyStatusRequest",

                          TransactionId.HasValue
                              ? new XElement("transactionId",      TransactionId.        ToString())
                              : null,

                          new XElement("partnerIdType",            PartnerId.Format.     AsText()),
                          new XElement("partnerId",                PartnerId.            ToString()),

                          new XElement("operatorIdType",           OperatorId.Format.    AsText()),
                          new XElement("operatorId",               OperatorId.           ToString()),

                          new XElement("EVSEIdType",               EVSEId.Format.        AsText()),
                          new XElement("EVSEId",                   EVSEId.               ToString()),

                          new XElement("statusEventDate",          StatusEventDate.      ToISO8601(false).Replace("Z", "")),
                          new XElement("busyStatus",               BusyStatus.           AsNumber()),

                          BusyStatusUntil.HasValue
                              ? new XElement("busyStatusUntil",    BusyStatusUntil.Value.ToISO8601(false).Replace("Z", ""))
                              : null,

                          BusyStatusComment.IsNeitherNullNorEmpty()
                              ? new XElement("busyStatusComment",  BusyStatusComment)
                              : null

                      );


            return CustomSetEVSEBusyStatusRequestSerializer is not null
                       ? CustomSetEVSEBusyStatusRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetEVSEBusyStatusRequest1, SetEVSEBusyStatusRequest2)

        /// <summary>
        /// Compares two SetEVSEBusyStatus requests for equality.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequest1">A SetEVSEBusyStatus request.</param>
        /// <param name="SetEVSEBusyStatusRequest2">Another SetEVSEBusyStatus request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetEVSEBusyStatusRequest SetEVSEBusyStatusRequest1, SetEVSEBusyStatusRequest SetEVSEBusyStatusRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetEVSEBusyStatusRequest1, SetEVSEBusyStatusRequest2))
                return true;

            // If one is null, but not both, return false.
            if (SetEVSEBusyStatusRequest1 is null || SetEVSEBusyStatusRequest2 is null)
                return false;

            return SetEVSEBusyStatusRequest1.Equals(SetEVSEBusyStatusRequest2);

        }

        #endregion

        #region Operator != (SetEVSEBusyStatusRequest1, SetEVSEBusyStatusRequest2)

        /// <summary>
        /// Compares two SetEVSEBusyStatus requests for inequality.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequest1">A SetEVSEBusyStatus request.</param>
        /// <param name="SetEVSEBusyStatusRequest2">Another SetEVSEBusyStatus request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetEVSEBusyStatusRequest SetEVSEBusyStatusRequest1, SetEVSEBusyStatusRequest SetEVSEBusyStatusRequest2)

            => !(SetEVSEBusyStatusRequest1 == SetEVSEBusyStatusRequest2);

        #endregion

        #endregion

        #region IEquatable<SetEVSEBusyStatusRequest> Members

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

            var SetEVSEBusyStatusRequest = Object as SetEVSEBusyStatusRequest;
            if ((Object) SetEVSEBusyStatusRequest is null)
                return false;

            return Equals(SetEVSEBusyStatusRequest);

        }

        #endregion

        #region Equals(SetEVSEBusyStatusRequest)

        /// <summary>
        /// Compares two SetEVSEBusyStatus requests for equality.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequest">A SetEVSEBusyStatus request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetEVSEBusyStatusRequest SetEVSEBusyStatusRequest)
        {

            if ((Object) SetEVSEBusyStatusRequest is null)
                return false;

            return ((!TransactionId.HasValue && !SetEVSEBusyStatusRequest.TransactionId.HasValue) ||
                     (TransactionId.HasValue && SetEVSEBusyStatusRequest.TransactionId.HasValue && TransactionId.Value.Equals(SetEVSEBusyStatusRequest.TransactionId.Value))) &&

                   PartnerId.      Equals(SetEVSEBusyStatusRequest.PartnerId)       &&
                   OperatorId.     Equals(SetEVSEBusyStatusRequest.OperatorId)      &&
                   EVSEId.         Equals(SetEVSEBusyStatusRequest.EVSEId)          &&
                   StatusEventDate.Equals(SetEVSEBusyStatusRequest.StatusEventDate) &&
                   BusyStatus.     Equals(SetEVSEBusyStatusRequest.BusyStatus)      &&

                   ((!BusyStatusUntil.HasValue && !SetEVSEBusyStatusRequest.BusyStatusUntil.HasValue) ||
                     (BusyStatusUntil.HasValue &&  SetEVSEBusyStatusRequest.BusyStatusUntil.HasValue && BusyStatusUntil.Value.Equals(SetEVSEBusyStatusRequest.BusyStatusUntil.Value))) &&

                   ((!BusyStatusComment.IsNeitherNullNorEmpty() && !SetEVSEBusyStatusRequest.BusyStatusComment.IsNeitherNullNorEmpty()) ||
                     (BusyStatusComment.IsNeitherNullNorEmpty() &&  SetEVSEBusyStatusRequest.BusyStatusComment.IsNeitherNullNorEmpty() && BusyStatusComment.Equals(SetEVSEBusyStatusRequest.BusyStatusComment)));

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

                       PartnerId.      GetHashCode() * 21 ^
                       OperatorId.     GetHashCode() * 17 ^
                       EVSEId.         GetHashCode() * 13 ^
                       StatusEventDate.GetHashCode() * 11 ^
                       BusyStatus.     GetHashCode() *  7 ^

                       (BusyStatusUntil.HasValue
                            ? BusyStatusUntil.  GetHashCode() * 5
                            : 0) ^

                       (BusyStatusComment.IsNeitherNullNorEmpty()
                            ? BusyStatusComment.GetHashCode() * 3
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
                             EVSEId, " -> ", BusyStatus.AsText());

        #endregion

    }

}
