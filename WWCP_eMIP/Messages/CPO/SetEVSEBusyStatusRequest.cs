/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A SetEVSEBusyStatus request.
    /// </summary>
    public class SetEVSEBusyStatusRequest : ARequest<SetEVSEBusyStatusRequest>
    {

        #region Properties

        /// <summary>
        /// The partner identification.
        /// </summary>
        public Partner_Id           PartnerId            { get; }

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
        public DateTime             StatusEventDate      { get; }

        /// <summary>
        /// The EVSE busy status.
        /// </summary>
        public EVSEBusyStatusTypes  BusyStatus           { get; }

        /// <summary>
        /// The optional transaction identification.
        /// </summary>
        public Transaction_Id?      TransactionId        { get; }

        /// <summary>
        /// The optional timestamp until which the given busy status is valid.
        /// </summary>
        public DateTime?            BusyStatusUntil      { get; }

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
        /// <param name="StatusEventDate">The timestamp of the status change.</param>
        /// <param name="BusyStatus">The EVSE busy status.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="BusyStatusUntil">An optional timestamp until which the given busy status is valid.</param>
        /// <param name="BusyStatusComment">An optional comment about the busy status.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetEVSEBusyStatusRequest(Partner_Id           PartnerId,
                                        Operator_Id          OperatorId,
                                        EVSE_Id              EVSEId,
                                        DateTime             StatusEventDate,
                                        EVSEBusyStatusTypes  BusyStatus,
                                        Transaction_Id?      TransactionId       = null,
                                        DateTime?            BusyStatusUntil     = null,
                                        String               BusyStatusComment   = null,

                                        DateTime?            Timestamp           = null,
                                        CancellationToken?   CancellationToken   = null,
                                        EventTracking_Id     EventTrackingId     = null,
                                        TimeSpan?            RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.PartnerId          = PartnerId;
            this.OperatorId         = OperatorId;
            this.EVSEId             = EVSEId;
            this.StatusEventDate    = StatusEventDate;
            this.BusyStatus         = BusyStatus;
            this.TransactionId      = TransactionId;
            this.BusyStatusUntil    = BusyStatusUntil;
            this.BusyStatusComment  = BusyStatusComment?.Trim();

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
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSEBusyStatusRequestParser">A delegate to parse custom SetEVSEBusyStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetEVSEBusyStatusRequest Parse(XElement                                           SetEVSEBusyStatusRequestXML,
                                                     CustomXMLParserDelegate<SetEVSEBusyStatusRequest>  CustomSendSetEVSEBusyStatusRequestParser,
                                                     OnExceptionDelegate                                OnException = null)
        {

            if (TryParse(SetEVSEBusyStatusRequestXML,
                         CustomSendSetEVSEBusyStatusRequestParser,
                         out SetEVSEBusyStatusRequest _SetEVSEBusyStatusRequest,
                         OnException))
            {
                return _SetEVSEBusyStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (SetEVSEBusyStatusRequestText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetEVSEBusyStatusRequestParser">A delegate to parse custom SetEVSEBusyStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetEVSEBusyStatusRequest Parse(String                                             SetEVSEBusyStatusRequestText,
                                                     CustomXMLParserDelegate<SetEVSEBusyStatusRequest>  CustomSendSetEVSEBusyStatusRequestParser,
                                                     OnExceptionDelegate                                OnException = null)
        {

            if (TryParse(SetEVSEBusyStatusRequestText,
                         CustomSendSetEVSEBusyStatusRequestParser,
                         out SetEVSEBusyStatusRequest _SetEVSEBusyStatusRequest,
                         OnException))
            {
                return _SetEVSEBusyStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SetEVSEBusyStatusRequestXML,  ..., out SetEVSEBusyStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSEBusyStatusRequestParser">A delegate to parse custom SetEVSEBusyStatusRequest XML elements.</param>
        /// <param name="SetEVSEBusyStatusRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                           SetEVSEBusyStatusRequestXML,
                                       CustomXMLParserDelegate<SetEVSEBusyStatusRequest>  CustomSendSetEVSEBusyStatusRequestParser,
                                       out SetEVSEBusyStatusRequest                       SetEVSEBusyStatusRequest,
                                       OnExceptionDelegate                                OnException  = null)
        {

            try
            {

                SetEVSEBusyStatusRequest = new SetEVSEBusyStatusRequest(

                                               //ToDo: What to do with: <partnerIdType>eMI3</partnerIdType>?

                                               SetEVSEBusyStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "partnerId",
                                                                                              Partner_Id.Parse),

                                               //ToDo: What to do with: <operatorIdType>eMI3</operatorIdType>

                                               SetEVSEBusyStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "operatorId",
                                                                                              Operator_Id.Parse),

                                               //ToDo: What to do with: <EVSEIdType>eMI3</EVSEIdType>

                                               SetEVSEBusyStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "EVSEId",
                                                                                              EVSE_Id.Parse),

                                               SetEVSEBusyStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "statusEventDate",
                                                                                              DateTime.Parse),

                                               SetEVSEBusyStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "busyStatus",
                                                                                              ConversionMethods.AsEVSEBusyStatusTypes),

                                               SetEVSEBusyStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "transactionId",
                                                                                              Transaction_Id.Parse),

                                               SetEVSEBusyStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "busyStatusUntil",
                                                                                              DateTime.Parse),

                                               SetEVSEBusyStatusRequestXML.MapValueOrNull    (eMIPNS.EVCIDynamic + "busyStatusComment")

                                           );


                if (CustomSendSetEVSEBusyStatusRequestParser != null)
                    SetEVSEBusyStatusRequest = CustomSendSetEVSEBusyStatusRequestParser(SetEVSEBusyStatusRequestXML,
                                                                                                        SetEVSEBusyStatusRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetEVSEBusyStatusRequestXML, e);

                SetEVSEBusyStatusRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetEVSEBusyStatusRequestText, ..., out SetEVSEBusyStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetEVSEBusyStatusRequestParser">A delegate to parse custom SetEVSEBusyStatusRequest XML elements.</param>
        /// <param name="SetEVSEBusyStatusRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                             SetEVSEBusyStatusRequestText,
                                       CustomXMLParserDelegate<SetEVSEBusyStatusRequest>  CustomSendSetEVSEBusyStatusRequestParser,
                                       out SetEVSEBusyStatusRequest                       SetEVSEBusyStatusRequest,
                                       OnExceptionDelegate                                OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetEVSEBusyStatusRequestText).Root,
                             CustomSendSetEVSEBusyStatusRequestParser,
                             out SetEVSEBusyStatusRequest,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SetEVSEBusyStatusRequestText, e);
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
                              ? new XElement(eMIPNS.EVCIDynamic + "transactionId",      TransactionId.        ToString())
                              : null,

                          new XElement(eMIPNS.EVCIDynamic + "partnerIdType",            PartnerId.Format.     ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "partnerId",                PartnerId.            ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "operatorIdType",           OperatorId.Format.    ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "operatorId",               OperatorId.           ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "EVSEIdType",               EVSEId.Format.        ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "EVSEId",                   EVSEId.               ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "statusEventDate",          StatusEventDate.      ToIso8601(false)),
                          new XElement(eMIPNS.EVCIDynamic + "busyStatus",               BusyStatus.           AsNumber()),

                          BusyStatusUntil.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "busyStatusUntil",    BusyStatusUntil.Value.ToIso8601(false))
                              : null,

                          BusyStatusComment.IsNeitherNullNorEmpty()
                              ? new XElement(eMIPNS.EVCIDynamic + "busyStatusComment",  BusyStatusComment)
                              : null

                      );


            return CustomSetEVSEBusyStatusRequestSerializer != null
                       ? CustomSetEVSEBusyStatusRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetEVSEBusyStatusRequest1, SetEVSEBusyStatusRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequest1">A heartbeat request.</param>
        /// <param name="SetEVSEBusyStatusRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetEVSEBusyStatusRequest SetEVSEBusyStatusRequest1, SetEVSEBusyStatusRequest SetEVSEBusyStatusRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SetEVSEBusyStatusRequest1, SetEVSEBusyStatusRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SetEVSEBusyStatusRequest1 == null) || ((Object) SetEVSEBusyStatusRequest2 == null))
                return false;

            return SetEVSEBusyStatusRequest1.Equals(SetEVSEBusyStatusRequest2);

        }

        #endregion

        #region Operator != (SetEVSEBusyStatusRequest1, SetEVSEBusyStatusRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequest1">A heartbeat request.</param>
        /// <param name="SetEVSEBusyStatusRequest2">Another heartbeat request.</param>
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

            if (Object == null)
                return false;

            var SetEVSEBusyStatusRequest = Object as SetEVSEBusyStatusRequest;
            if ((Object) SetEVSEBusyStatusRequest == null)
                return false;

            return Equals(SetEVSEBusyStatusRequest);

        }

        #endregion

        #region Equals(SetEVSEBusyStatusRequest)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetEVSEBusyStatusRequest">A heartbeat request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetEVSEBusyStatusRequest SetEVSEBusyStatusRequest)
        {

            if ((Object) SetEVSEBusyStatusRequest == null)
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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(PartnerId,  " / ",
                             OperatorId, ": ",
                             EVSEId, " -> ", BusyStatus.AsText());

        #endregion

    }

}
