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
    /// A SetEVSEAvailabilityStatus request.
    /// </summary>
    public class SetEVSEAvailabilityStatusRequest : ARequest<SetEVSEAvailabilityStatusRequest>
    {

        #region Properties

        /// <summary>
        /// The partner identification.
        /// </summary>
        public Partner_Id                   PartnerId                    { get; }

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id                  OperatorId                   { get; }

        /// <summary>
        /// The EVSE identification.
        /// </summary>
        public EVSE_Id                      EVSEId                       { get; }

        /// <summary>
        /// The timestamp of the status change.
        /// </summary>
        public DateTime                     StatusEventDate              { get; }

        /// <summary>
        /// The EVSE availability status.
        /// </summary>
        public EVSEAvailabilityStatusTypes  AvailabilityStatus           { get; }

        /// <summary>
        /// The optional timestamp until which the given availability status is valid.
        /// </summary>
        public DateTime?                    AvailabilityStatusUntil      { get; }

        /// <summary>
        /// The optional comment about the availability status.
        /// </summary>
        public String                       AvailabilityStatusComment    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a SetEVSEAvailabilityStatus XML/SOAP request.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="StatusEventDate">The timestamp of the status change.</param>
        /// <param name="AvailabilityStatus">The EVSE availability status.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="AvailabilityStatusUntil">An optional timestamp until which the given availability status is valid.</param>
        /// <param name="AvailabilityStatusComment">An optional comment about the availability status.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetEVSEAvailabilityStatusRequest(Partner_Id                   PartnerId,
                                                Operator_Id                  OperatorId,
                                                EVSE_Id                      EVSEId,
                                                DateTime                     StatusEventDate,
                                                EVSEAvailabilityStatusTypes  AvailabilityStatus,
                                                Transaction_Id?              TransactionId               = null,
                                                DateTime?                    AvailabilityStatusUntil     = null,
                                                String                       AvailabilityStatusComment   = null,

                                                DateTime?                    Timestamp                   = null,
                                                CancellationToken?           CancellationToken           = null,
                                                EventTracking_Id             EventTrackingId             = null,
                                                TimeSpan?                    RequestTimeout              = null)

            : base(TransactionId,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.PartnerId                  = PartnerId;
            this.OperatorId                 = OperatorId;
            this.EVSEId                     = EVSEId;
            this.StatusEventDate            = StatusEventDate;
            this.AvailabilityStatus         = AvailabilityStatus;
            this.AvailabilityStatusUntil    = AvailabilityStatusUntil;
            this.AvailabilityStatusComment  = AvailabilityStatusComment?.Trim();

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/EVCIDynamicV1/">
        //
        //   <soap:Header />
        //
        //   <soap:Body>
        //     <eMIP:eMIP_ToIOP_SetEVSEAvailabilityStatusRequest>
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
        //       <availabilityStatus>?</availabilityStatus>
        //
        //       <!--Optional:-->
        //       <availabilityStatusUntil>?</availabilityStatusUntil>
        //
        //       <!--Optional:-->
        //       <availabilityStatusComment>?</availabilityStatusComment>
        //
        //     </eMIP:eMIP_ToIOP_SetEVSEAvailabilityStatusRequest>
        //   </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (SetEVSEAvailabilityStatusRequestXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetEVSEAvailabilityStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSEAvailabilityStatusRequestParser">An optional delegate to parse custom SetEVSEAvailabilityStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetEVSEAvailabilityStatusRequest Parse(XElement                                                   SetEVSEAvailabilityStatusRequestXML,
                                                             CustomXMLParserDelegate<SetEVSEAvailabilityStatusRequest>  CustomSendSetEVSEAvailabilityStatusRequestParser,
                                                             OnExceptionDelegate                                        OnException = null)
        {

            if (TryParse(SetEVSEAvailabilityStatusRequestXML,
                         CustomSendSetEVSEAvailabilityStatusRequestParser,
                         out SetEVSEAvailabilityStatusRequest _SetEVSEAvailabilityStatusRequest,
                         OnException))
            {
                return _SetEVSEAvailabilityStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (SetEVSEAvailabilityStatusRequestText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetEVSEAvailabilityStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetEVSEAvailabilityStatusRequestParser">An optional delegate to parse custom SetEVSEAvailabilityStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetEVSEAvailabilityStatusRequest Parse(String                                                     SetEVSEAvailabilityStatusRequestText,
                                                             CustomXMLParserDelegate<SetEVSEAvailabilityStatusRequest>  CustomSendSetEVSEAvailabilityStatusRequestParser,
                                                             OnExceptionDelegate                                        OnException = null)
        {

            if (TryParse(SetEVSEAvailabilityStatusRequestText,
                         CustomSendSetEVSEAvailabilityStatusRequestParser,
                         out SetEVSEAvailabilityStatusRequest _SetEVSEAvailabilityStatusRequest,
                         OnException))
            {
                return _SetEVSEAvailabilityStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SetEVSEAvailabilityStatusRequestXML,  ..., out SetEVSEAvailabilityStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetEVSEAvailabilityStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSEAvailabilityStatusRequestParser">An optional delegate to parse custom SetEVSEAvailabilityStatusRequest XML elements.</param>
        /// <param name="SetEVSEAvailabilityStatusRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                                   SetEVSEAvailabilityStatusRequestXML,
                                       CustomXMLParserDelegate<SetEVSEAvailabilityStatusRequest>  CustomSendSetEVSEAvailabilityStatusRequestParser,
                                       out SetEVSEAvailabilityStatusRequest                       SetEVSEAvailabilityStatusRequest,
                                       OnExceptionDelegate                                        OnException  = null)
        {

            try
            {

                SetEVSEAvailabilityStatusRequest = new SetEVSEAvailabilityStatusRequest(

                                                       //ToDo: What to do with: <partnerIdType>eMI3</partnerIdType>?

                                                       SetEVSEAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "partnerId",
                                                                                                              Partner_Id.Parse),

                                                       //ToDo: What to do with: <operatorIdType>eMI3</operatorIdType>

                                                       SetEVSEAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "operatorId",
                                                                                                              Operator_Id.Parse),

                                                       //ToDo: What to do with: <EVSEIdType>eMI3</EVSEIdType>

                                                       SetEVSEAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "EVSEId",
                                                                                                              EVSE_Id.Parse),

                                                       SetEVSEAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "statusEventDate",
                                                                                                              DateTime.Parse),

                                                       SetEVSEAvailabilityStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "availabilityStatus",
                                                                                                              ConversionMethods.AsEVSEAvailabilityStatusTypes),

                                                       SetEVSEAvailabilityStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "transactionId",
                                                                                                              Transaction_Id.Parse),

                                                       SetEVSEAvailabilityStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "availabilityStatusUntil",
                                                                                                              DateTime.Parse),

                                                       SetEVSEAvailabilityStatusRequestXML.MapValueOrNull    (eMIPNS.EVCIDynamic + "availabilityStatusComment")

                                                   );


                if (CustomSendSetEVSEAvailabilityStatusRequestParser != null)
                    SetEVSEAvailabilityStatusRequest = CustomSendSetEVSEAvailabilityStatusRequestParser(SetEVSEAvailabilityStatusRequestXML,
                                                                                                        SetEVSEAvailabilityStatusRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetEVSEAvailabilityStatusRequestXML, e);

                SetEVSEAvailabilityStatusRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetEVSEAvailabilityStatusRequestText, ..., out SetEVSEAvailabilityStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetEVSEAvailabilityStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetEVSEAvailabilityStatusRequestParser">An optional delegate to parse custom SetEVSEAvailabilityStatusRequest XML elements.</param>
        /// <param name="SetEVSEAvailabilityStatusRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                                     SetEVSEAvailabilityStatusRequestText,
                                       CustomXMLParserDelegate<SetEVSEAvailabilityStatusRequest>  CustomSendSetEVSEAvailabilityStatusRequestParser,
                                       out SetEVSEAvailabilityStatusRequest                       SetEVSEAvailabilityStatusRequest,
                                       OnExceptionDelegate                                        OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetEVSEAvailabilityStatusRequestText).Root,
                             CustomSendSetEVSEAvailabilityStatusRequestParser,
                             out SetEVSEAvailabilityStatusRequest,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SetEVSEAvailabilityStatusRequestText, e);
            }

            SetEVSEAvailabilityStatusRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetEVSEAvailabilityStatusRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetEVSEAvailabilityStatusRequestSerializer">A delegate to serialize custom set EVSE availability status request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetEVSEAvailabilityStatusRequest> CustomSetEVSEAvailabilityStatusRequestSerializer = null)
        {

            var XML = new XElement(eMIPNS.EVCIDynamic + "eMIP_ToIOP_SetEVSEAvailabilityStatusRequest",

                          TransactionId.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "transactionId",              TransactionId.                ToString())
                              : null,

                          new XElement(eMIPNS.EVCIDynamic + "partnerIdType",                    PartnerId.Format.             ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "partnerId",                        PartnerId.                    ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "operatorIdType",                   OperatorId.Format.            ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "operatorId",                       OperatorId.                   ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "EVSEIdType",                       EVSEId.Format.                ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "EVSEId",                           EVSEId.                       ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "statusEventDate",                  StatusEventDate.              ToIso8601(false)),
                          new XElement(eMIPNS.EVCIDynamic + "availabilityStatus",               AvailabilityStatus.           AsNumber()),

                          AvailabilityStatusUntil.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "availabilityStatusUntil",    AvailabilityStatusUntil.Value.ToIso8601(false))
                              : null,

                          AvailabilityStatusComment.IsNeitherNullNorEmpty()
                              ? new XElement(eMIPNS.EVCIDynamic + "availabilityStatusComment",  AvailabilityStatusComment)
                              : null

                      );


            return CustomSetEVSEAvailabilityStatusRequestSerializer != null
                       ? CustomSetEVSEAvailabilityStatusRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetEVSEAvailabilityStatusRequest1, SetEVSEAvailabilityStatusRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetEVSEAvailabilityStatusRequest1">A heartbeat request.</param>
        /// <param name="SetEVSEAvailabilityStatusRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetEVSEAvailabilityStatusRequest SetEVSEAvailabilityStatusRequest1, SetEVSEAvailabilityStatusRequest SetEVSEAvailabilityStatusRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SetEVSEAvailabilityStatusRequest1, SetEVSEAvailabilityStatusRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SetEVSEAvailabilityStatusRequest1 == null) || ((Object) SetEVSEAvailabilityStatusRequest2 == null))
                return false;

            return SetEVSEAvailabilityStatusRequest1.Equals(SetEVSEAvailabilityStatusRequest2);

        }

        #endregion

        #region Operator != (SetEVSEAvailabilityStatusRequest1, SetEVSEAvailabilityStatusRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="SetEVSEAvailabilityStatusRequest1">A heartbeat request.</param>
        /// <param name="SetEVSEAvailabilityStatusRequest2">Another heartbeat request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetEVSEAvailabilityStatusRequest SetEVSEAvailabilityStatusRequest1, SetEVSEAvailabilityStatusRequest SetEVSEAvailabilityStatusRequest2)

            => !(SetEVSEAvailabilityStatusRequest1 == SetEVSEAvailabilityStatusRequest2);

        #endregion

        #endregion

        #region IEquatable<SetEVSEAvailabilityStatusRequest> Members

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

            var SetEVSEAvailabilityStatusRequest = Object as SetEVSEAvailabilityStatusRequest;
            if ((Object) SetEVSEAvailabilityStatusRequest == null)
                return false;

            return Equals(SetEVSEAvailabilityStatusRequest);

        }

        #endregion

        #region Equals(SetEVSEAvailabilityStatusRequest)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetEVSEAvailabilityStatusRequest">A heartbeat request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetEVSEAvailabilityStatusRequest SetEVSEAvailabilityStatusRequest)
        {

            if ((Object) SetEVSEAvailabilityStatusRequest == null)
                return false;

            return ((!TransactionId.HasValue && !SetEVSEAvailabilityStatusRequest.TransactionId.HasValue) ||
                     (TransactionId.HasValue && SetEVSEAvailabilityStatusRequest.TransactionId.HasValue && TransactionId.Value.Equals(SetEVSEAvailabilityStatusRequest.TransactionId.Value))) &&

                   PartnerId.         Equals(SetEVSEAvailabilityStatusRequest.PartnerId)          &&
                   OperatorId.        Equals(SetEVSEAvailabilityStatusRequest.OperatorId)         &&
                   EVSEId.            Equals(SetEVSEAvailabilityStatusRequest.EVSEId)             &&
                   StatusEventDate.   Equals(SetEVSEAvailabilityStatusRequest.StatusEventDate)    &&
                   AvailabilityStatus.Equals(SetEVSEAvailabilityStatusRequest.AvailabilityStatus) &&

                   ((!AvailabilityStatusUntil.HasValue && !SetEVSEAvailabilityStatusRequest.AvailabilityStatusUntil.HasValue) ||
                     (AvailabilityStatusUntil.HasValue &&  SetEVSEAvailabilityStatusRequest.AvailabilityStatusUntil.HasValue && AvailabilityStatusUntil.Value.Equals(SetEVSEAvailabilityStatusRequest.AvailabilityStatusUntil.Value))) &&

                   ((!AvailabilityStatusComment.IsNeitherNullNorEmpty() && !SetEVSEAvailabilityStatusRequest.AvailabilityStatusComment.IsNeitherNullNorEmpty()) ||
                     (AvailabilityStatusComment.IsNeitherNullNorEmpty() &&  SetEVSEAvailabilityStatusRequest.AvailabilityStatusComment.IsNeitherNullNorEmpty() && AvailabilityStatusComment.Equals(SetEVSEAvailabilityStatusRequest.AvailabilityStatusComment)));

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
                       EVSEId.            GetHashCode() * 13 ^
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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(PartnerId,  " / ",
                             OperatorId, ": ",
                             EVSEId, " -> ", AvailabilityStatus.AsText());

        #endregion

    }

}
