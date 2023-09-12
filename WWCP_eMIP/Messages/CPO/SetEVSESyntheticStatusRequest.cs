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

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A SetEVSESyntheticStatus request.
    /// </summary>
    public class SetEVSESyntheticStatusRequest : ARequest<SetEVSESyntheticStatusRequest>
    {

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id                   OperatorId                     { get; }

        /// <summary>
        /// The EVSE identification.
        /// </summary>
        public EVSE_Id                       EVSEId                         { get; }


        /// <summary>
        /// The timestamp of the EVSE availability status change.
        /// </summary>
        public DateTime?                     AvailabilityStatusEventDate    { get; }

        /// <summary>
        /// The EVSE availability status.
        /// </summary>
        public EVSEAvailabilityStatusTypes?  AvailabilityStatus             { get; }

        /// <summary>
        /// The optional timestamp until which the given availability status is valid.
        /// </summary>
        public DateTime?                     AvailabilityStatusUntil        { get; }

        /// <summary>
        /// The optional comment about the availability status.
        /// </summary>
        public String                        AvailabilityStatusComment      { get; }


        /// <summary>
        /// The timestamp of the EVSE busy status change.
        /// </summary>
        public DateTime?                     BusyStatusEventDate            { get; }

        /// <summary>
        /// The EVSE busy status.
        /// </summary>
        public EVSEBusyStatusTypes?          BusyStatus                     { get; }

        /// <summary>
        /// The optional timestamp until which the given busy status is valid.
        /// </summary>
        public DateTime?                     BusyStatusUntil                { get; }

        /// <summary>
        /// The optional comment about the busy status.
        /// </summary>
        public String                        BusyStatusComment              { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a SetEVSESyntheticStatus XML/SOAP request.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// 
        /// <param name="AvailabilityStatusEventDate">The timestamp of the status update.</param>
        /// <param name="AvailabilityStatus">The EVSE availability status.</param>
        /// <param name="AvailabilityStatusUntil">An optional timestamp until which the given availability status is valid.</param>
        /// <param name="AvailabilityStatusComment">An optional comment about the availability status.</param>
        /// 
        /// <param name="BusyStatusEventDate">The timestamp of the status update.</param>
        /// <param name="BusyStatus">The EVSE busy status.</param>
        /// <param name="BusyStatusUntil">An optional timestamp until which the given busy status is valid.</param>
        /// <param name="BusyStatusComment">An optional comment about the busy status.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetEVSESyntheticStatusRequest(Partner_Id                    PartnerId,
                                             Operator_Id                   OperatorId,
                                             EVSE_Id                       EVSEId,
                                             Transaction_Id?               TransactionId                 = null,
                                             DateTime?                     AvailabilityStatusEventDate   = null,
                                             EVSEAvailabilityStatusTypes?  AvailabilityStatus            = null,
                                             DateTime?                     AvailabilityStatusUntil       = null,
                                             String?                       AvailabilityStatusComment     = null,
                                             DateTime?                     BusyStatusEventDate           = null,
                                             EVSEBusyStatusTypes?          BusyStatus                    = null,
                                             DateTime?                     BusyStatusUntil               = null,
                                             String?                       BusyStatusComment             = null,

                                             HTTPRequest?                  HTTPRequest                   = null,
                                             DateTime?                     Timestamp                     = null,
                                             CancellationToken             CancellationToken             = default,
                                             EventTracking_Id?             EventTrackingId               = null,
                                             TimeSpan?                     RequestTimeout                = null)

            : base(HTTPRequest,
                   PartnerId,
                   TransactionId,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.OperatorId                   = OperatorId;
            this.EVSEId                       = EVSEId;

            this.AvailabilityStatusEventDate  = AvailabilityStatusEventDate;
            this.AvailabilityStatus           = AvailabilityStatus;
            this.AvailabilityStatusUntil      = AvailabilityStatusUntil;
            this.AvailabilityStatusComment    = AvailabilityStatusComment?.Trim() ?? String.Empty;

            this.BusyStatusEventDate          = BusyStatusEventDate;
            this.BusyStatus                   = BusyStatus;
            this.BusyStatusUntil              = BusyStatusUntil;
            this.BusyStatusComment            = BusyStatusComment?.Trim() ?? String.Empty;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/EVCIDynamicV1/">
        //
        //   <soap:Header />
        //
        //   <soap:Body>
        //     <eMIP:eMIP_ToIOP_SetEVSESyntheticStatusRequest>
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
        //       <!--Optional:-->
        //       <availabilityStatusEventDate>?</availabilityStatusEventDate>
        //       <!--Optional:-->
        //       <availabilityStatus>?</availabilityStatus>
        //       <!--Optional:-->
        //       <availabilityStatusUntil>?</availabilityStatusUntil>
        //       <!--Optional:-->
        //       <availabilityStatusComment>?</availabilityStatusComment>
        //
        //       <!--Optional:-->
        //       <busyStatusEventDate>?</busyStatusEventDate>
        //       <!--Optional:-->
        //       <busyStatus>?</busyStatus>
        //       <!--Optional:-->
        //       <busyStatusUntil>?</busyStatusUntil>
        //       <!--Optional:-->
        //       <busyStatusComment>?</busyStatusComment>
        //
        //     </eMIP:eMIP_ToIOP_SetEVSESyntheticStatusRequest>
        //   </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (SetEVSESyntheticStatusRequestXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetEVSESyntheticStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSESyntheticStatusRequestParser">An optional delegate to parse custom SetEVSESyntheticStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetEVSESyntheticStatusRequest Parse(XElement                                                SetEVSESyntheticStatusRequestXML,
                                                          CustomXMLParserDelegate<SetEVSESyntheticStatusRequest>  CustomSendSetEVSESyntheticStatusRequestParser,
                                                          OnExceptionDelegate                                     OnException         = null,

                                                          HTTPRequest                                             HTTPRequest         = null,
                                                          DateTime?                                               Timestamp           = null,
                                                          CancellationToken                                       CancellationToken   = default,
                                                          EventTracking_Id                                        EventTrackingId     = null,
                                                          TimeSpan?                                               RequestTimeout      = null)
        {

            if (TryParse(SetEVSESyntheticStatusRequestXML,
                         CustomSendSetEVSESyntheticStatusRequestParser,
                         out SetEVSESyntheticStatusRequest _SetEVSESyntheticStatusRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetEVSESyntheticStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (SetEVSESyntheticStatusRequestText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetEVSESyntheticStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetEVSESyntheticStatusRequestParser">An optional delegate to parse custom SetEVSESyntheticStatusRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetEVSESyntheticStatusRequest Parse(String                                                  SetEVSESyntheticStatusRequestText,
                                                          CustomXMLParserDelegate<SetEVSESyntheticStatusRequest>  CustomSendSetEVSESyntheticStatusRequestParser,
                                                          OnExceptionDelegate                                     OnException         = null,

                                                          HTTPRequest                                             HTTPRequest         = null,
                                                          DateTime?                                               Timestamp           = null,
                                                          CancellationToken                                       CancellationToken   = default,
                                                          EventTracking_Id                                        EventTrackingId     = null,
                                                          TimeSpan?                                               RequestTimeout      = null)
        {

            if (TryParse(SetEVSESyntheticStatusRequestText,
                         CustomSendSetEVSESyntheticStatusRequestParser,
                         out SetEVSESyntheticStatusRequest _SetEVSESyntheticStatusRequest,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetEVSESyntheticStatusRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SetEVSESyntheticStatusRequestXML,  ..., out SetEVSESyntheticStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetEVSESyntheticStatusRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetEVSESyntheticStatusRequestParser">An optional delegate to parse custom SetEVSESyntheticStatusRequest XML elements.</param>
        /// <param name="SetEVSESyntheticStatusRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                                SetEVSESyntheticStatusRequestXML,
                                       CustomXMLParserDelegate<SetEVSESyntheticStatusRequest>  CustomSendSetEVSESyntheticStatusRequestParser,
                                       out SetEVSESyntheticStatusRequest                       SetEVSESyntheticStatusRequest,
                                       OnExceptionDelegate                                     OnException         = null,

                                       HTTPRequest                                             HTTPRequest         = null,
                                       DateTime?                                               Timestamp           = null,
                                       CancellationToken                                       CancellationToken   = default,
                                       EventTracking_Id                                        EventTrackingId     = null,
                                       TimeSpan?                                               RequestTimeout      = null)
        {

            try
            {

                SetEVSESyntheticStatusRequest = new SetEVSESyntheticStatusRequest(

                                                    //ToDo: What to do with: <partnerIdType>eMI3</partnerIdType>?

                                                    SetEVSESyntheticStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "partnerId",
                                                                                                        Partner_Id.Parse),

                                                    //ToDo: What to do with: <operatorIdType>eMI3</operatorIdType>

                                                    SetEVSESyntheticStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "operatorId",
                                                                                                        Operator_Id.Parse),

                                                    //ToDo: What to do with: <EVSEIdType>eMI3</EVSEIdType>

                                                    SetEVSESyntheticStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "EVSEId",
                                                                                                        EVSE_Id.Parse),

                                                    SetEVSESyntheticStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "transactionId",
                                                                                                        Transaction_Id.Parse),


                                                    SetEVSESyntheticStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "availabilityStatusEventDate",
                                                                                                        DateTime.Parse),

                                                    SetEVSESyntheticStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "availabilityStatus",
                                                                                                        ConversionMethods.AsEVSEAvailabilityStatusTypes),

                                                    SetEVSESyntheticStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "availabilityStatusUntil",
                                                                                                        DateTime.Parse),

                                                    SetEVSESyntheticStatusRequestXML.MapValueOrNull    (eMIPNS.EVCIDynamic + "availabilityStatusComment"),


                                                    SetEVSESyntheticStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "busyStatusEventDate",
                                                                                                        DateTime.Parse),

                                                    SetEVSESyntheticStatusRequestXML.MapValueOrFail    (eMIPNS.EVCIDynamic + "busyStatus",
                                                                                                        ConversionMethods.AsEVSEBusyStatusTypes),

                                                    SetEVSESyntheticStatusRequestXML.MapValueOrNullable(eMIPNS.EVCIDynamic + "busyStatusUntil",
                                                                                                        DateTime.Parse),

                                                    SetEVSESyntheticStatusRequestXML.MapValueOrNull    (eMIPNS.EVCIDynamic + "busyStatusComment"),

                                                    HTTPRequest,
                                                    Timestamp,
                                                    CancellationToken,
                                                    EventTrackingId,
                                                    RequestTimeout

                                                );


                if (CustomSendSetEVSESyntheticStatusRequestParser is not null)
                    SetEVSESyntheticStatusRequest = CustomSendSetEVSESyntheticStatusRequestParser(SetEVSESyntheticStatusRequestXML,
                                                                                                        SetEVSESyntheticStatusRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetEVSESyntheticStatusRequestXML, e);

                SetEVSESyntheticStatusRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetEVSESyntheticStatusRequestText, ..., out SetEVSESyntheticStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetEVSESyntheticStatusRequestText">The text to parse.</param>
        /// <param name="CustomSendSetEVSESyntheticStatusRequestParser">An optional delegate to parse custom SetEVSESyntheticStatusRequest XML elements.</param>
        /// <param name="SetEVSESyntheticStatusRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                  SetEVSESyntheticStatusRequestText,
                                       CustomXMLParserDelegate<SetEVSESyntheticStatusRequest>  CustomSendSetEVSESyntheticStatusRequestParser,
                                       out SetEVSESyntheticStatusRequest                       SetEVSESyntheticStatusRequest,
                                       OnExceptionDelegate                                     OnException         = null,

                                       HTTPRequest                                             HTTPRequest         = null,
                                       DateTime?                                               Timestamp           = null,
                                       CancellationToken                                       CancellationToken   = default,
                                       EventTracking_Id                                        EventTrackingId     = null,
                                       TimeSpan?                                               RequestTimeout      = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetEVSESyntheticStatusRequestText).Root,
                             CustomSendSetEVSESyntheticStatusRequestParser,
                             out SetEVSESyntheticStatusRequest,
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
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetEVSESyntheticStatusRequestText, e);
            }

            SetEVSESyntheticStatusRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetEVSESyntheticStatusRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetEVSESyntheticStatusRequestSerializer">A delegate to serialize custom set EVSE busy status request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetEVSESyntheticStatusRequest> CustomSetEVSESyntheticStatusRequestSerializer = null)
        {

            var XML = new XElement(eMIPNS.EVCIDynamic + "eMIP_ToIOP_SetEVSESyntheticStatusRequest",

                          TransactionId.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "transactionId",                TransactionId.        ToString())
                              : null,

                          new XElement(eMIPNS.EVCIDynamic + "partnerIdType",                      PartnerId.Format.     ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "partnerId",                          PartnerId.            ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "operatorIdType",                     OperatorId.Format.    ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "operatorId",                         OperatorId.           ToString()),

                          new XElement(eMIPNS.EVCIDynamic + "EVSEIdType",                         EVSEId.Format.        ToString()),
                          new XElement(eMIPNS.EVCIDynamic + "EVSEId",                             EVSEId.               ToString()),


                          AvailabilityStatusEventDate.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "availabilityStatusEventDate",  AvailabilityStatusEventDate.Value.ToIso8601(false))
                              : null,

                          AvailabilityStatus.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "availabilityStatus",           AvailabilityStatus.         Value.AsNumber())
                              : null,

                          AvailabilityStatusUntil.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "availabilityStatusUntil",      AvailabilityStatusUntil.    Value.ToIso8601(false))
                              : null,

                          AvailabilityStatusComment.IsNeitherNullNorEmpty()
                              ? new XElement(eMIPNS.EVCIDynamic + "availabilityStatusComment",    AvailabilityStatusComment)
                              : null,


                          BusyStatusEventDate.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "busyStatusEventDate",          BusyStatusEventDate.        Value.ToIso8601(false))
                              : null,

                          BusyStatus.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "busyStatus",                   BusyStatus.                 Value.AsNumber())
                              : null,

                          BusyStatusUntil.HasValue
                              ? new XElement(eMIPNS.EVCIDynamic + "busyStatusUntil",              BusyStatusUntil.            Value.ToIso8601(false))
                              : null,

                          BusyStatusComment.IsNeitherNullNorEmpty()
                              ? new XElement(eMIPNS.EVCIDynamic + "busyStatusComment",            BusyStatusComment)
                              : null

                      );


            return CustomSetEVSESyntheticStatusRequestSerializer is not null
                       ? CustomSetEVSESyntheticStatusRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetEVSESyntheticStatusRequest1, SetEVSESyntheticStatusRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetEVSESyntheticStatusRequest1">A heartbeat request.</param>
        /// <param name="SetEVSESyntheticStatusRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetEVSESyntheticStatusRequest SetEVSESyntheticStatusRequest1, SetEVSESyntheticStatusRequest SetEVSESyntheticStatusRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetEVSESyntheticStatusRequest1, SetEVSESyntheticStatusRequest2))
                return true;

            // If one is null, but not both, return false.
            if (SetEVSESyntheticStatusRequest1 is null || SetEVSESyntheticStatusRequest2 is null)
                return false;

            return SetEVSESyntheticStatusRequest1.Equals(SetEVSESyntheticStatusRequest2);

        }

        #endregion

        #region Operator != (SetEVSESyntheticStatusRequest1, SetEVSESyntheticStatusRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="SetEVSESyntheticStatusRequest1">A heartbeat request.</param>
        /// <param name="SetEVSESyntheticStatusRequest2">Another heartbeat request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetEVSESyntheticStatusRequest SetEVSESyntheticStatusRequest1, SetEVSESyntheticStatusRequest SetEVSESyntheticStatusRequest2)

            => !(SetEVSESyntheticStatusRequest1 == SetEVSESyntheticStatusRequest2);

        #endregion

        #endregion

        #region IEquatable<SetEVSESyntheticStatusRequest> Members

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

            var SetEVSESyntheticStatusRequest = Object as SetEVSESyntheticStatusRequest;
            if ((Object) SetEVSESyntheticStatusRequest == null)
                return false;

            return Equals(SetEVSESyntheticStatusRequest);

        }

        #endregion

        #region Equals(SetEVSESyntheticStatusRequest)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetEVSESyntheticStatusRequest">A heartbeat request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetEVSESyntheticStatusRequest SetEVSESyntheticStatusRequest)
        {

            if ((Object) SetEVSESyntheticStatusRequest == null)
                return false;

            return ((!TransactionId.HasValue && !SetEVSESyntheticStatusRequest.TransactionId.HasValue) ||
                     (TransactionId.HasValue && SetEVSESyntheticStatusRequest.TransactionId.HasValue && TransactionId.Value.Equals(SetEVSESyntheticStatusRequest.TransactionId.Value))) &&

                   PartnerId.      Equals(SetEVSESyntheticStatusRequest.PartnerId)       &&
                   OperatorId.     Equals(SetEVSESyntheticStatusRequest.OperatorId)      &&
                   EVSEId.         Equals(SetEVSESyntheticStatusRequest.EVSEId)          &&

                   ((!AvailabilityStatusEventDate.HasValue                && !SetEVSESyntheticStatusRequest.AvailabilityStatusEventDate.HasValue) ||
                     (AvailabilityStatusEventDate.HasValue                &&  SetEVSESyntheticStatusRequest.AvailabilityStatusEventDate.HasValue                && AvailabilityStatusEventDate.Value.Equals(SetEVSESyntheticStatusRequest.AvailabilityStatusEventDate.Value))) &&

                   ((!AvailabilityStatus.         HasValue                && !SetEVSESyntheticStatusRequest.AvailabilityStatus.         HasValue) ||
                     (AvailabilityStatus.         HasValue                &&  SetEVSESyntheticStatusRequest.AvailabilityStatus.         HasValue                && AvailabilityStatus.         Value.Equals(SetEVSESyntheticStatusRequest.AvailabilityStatus.Value))) &&

                   ((!AvailabilityStatusUntil.    HasValue                && !SetEVSESyntheticStatusRequest.AvailabilityStatusUntil.    HasValue) ||
                     (AvailabilityStatusUntil.    HasValue                &&  SetEVSESyntheticStatusRequest.AvailabilityStatusUntil.    HasValue                && AvailabilityStatusUntil.    Value.Equals(SetEVSESyntheticStatusRequest.AvailabilityStatusUntil.Value))) &&

                   ((!AvailabilityStatusComment.  IsNeitherNullNorEmpty() && !SetEVSESyntheticStatusRequest.AvailabilityStatusComment.  IsNeitherNullNorEmpty()) ||
                     (AvailabilityStatusComment.  IsNeitherNullNorEmpty() &&  SetEVSESyntheticStatusRequest.AvailabilityStatusComment.  IsNeitherNullNorEmpty() && AvailabilityStatusComment.        Equals(SetEVSESyntheticStatusRequest.AvailabilityStatusComment)) &&


                   ((!BusyStatusEventDate.HasValue                && !SetEVSESyntheticStatusRequest.BusyStatusEventDate.HasValue) ||
                     (BusyStatusEventDate.HasValue                &&  SetEVSESyntheticStatusRequest.BusyStatusEventDate.HasValue                && BusyStatusEventDate.Value.Equals(SetEVSESyntheticStatusRequest.BusyStatusEventDate.Value))) &&

                   ((!BusyStatus.         HasValue                && !SetEVSESyntheticStatusRequest.BusyStatus.         HasValue) ||
                     (BusyStatus.         HasValue                &&  SetEVSESyntheticStatusRequest.BusyStatus.         HasValue                && BusyStatus.         Value.Equals(SetEVSESyntheticStatusRequest.BusyStatus.Value))) &&

                   ((!BusyStatusUntil.    HasValue                && !SetEVSESyntheticStatusRequest.BusyStatusUntil.    HasValue) ||
                     (BusyStatusUntil.    HasValue                &&  SetEVSESyntheticStatusRequest.BusyStatusUntil.    HasValue                && BusyStatusUntil.    Value.Equals(SetEVSESyntheticStatusRequest.BusyStatusUntil.Value))) &&

                   ((!BusyStatusComment.  IsNeitherNullNorEmpty() && !SetEVSESyntheticStatusRequest.BusyStatusComment.  IsNeitherNullNorEmpty()) ||
                     (BusyStatusComment.  IsNeitherNullNorEmpty() &&  SetEVSESyntheticStatusRequest.BusyStatusComment.  IsNeitherNullNorEmpty() && BusyStatusComment.        Equals(SetEVSESyntheticStatusRequest.BusyStatusComment))));

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
                            ? TransactionId.              GetHashCode() * 31
                            : 0) ^

                       PartnerId.                         GetHashCode() * 29 ^
                       OperatorId.                        GetHashCode() * 23 ^
                       EVSEId.                            GetHashCode() * 21 ^


                       (AvailabilityStatusEventDate.HasValue
                            ? AvailabilityStatusEventDate.GetHashCode() * 19
                            : 0) ^

                       (AvailabilityStatus.HasValue
                            ? AvailabilityStatus.         GetHashCode() * 17
                            : 0) ^

                       (AvailabilityStatusUntil.HasValue
                            ? AvailabilityStatusUntil.    GetHashCode() * 13
                            : 0) ^

                       (AvailabilityStatusComment.IsNeitherNullNorEmpty()
                            ? AvailabilityStatusComment.  GetHashCode() * 11
                            : 0) ^


                       (BusyStatusEventDate.HasValue
                            ? BusyStatusEventDate.        GetHashCode() * 7
                            : 0) ^

                       (BusyStatus.HasValue
                            ? BusyStatus.                 GetHashCode() * 3
                            : 0) ^

                       (BusyStatusUntil.HasValue
                            ? BusyStatusUntil.            GetHashCode() * 3
                            : 0) ^

                       (BusyStatusComment.IsNeitherNullNorEmpty()
                            ? BusyStatusComment.          GetHashCode()
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
                             EVSEId, " -> ",

                             AvailabilityStatus.HasValue ? AvailabilityStatus.Value.AsText() + ", " : "",
                             BusyStatus.        HasValue ? BusyStatus.        Value.AsText()        : "");

        #endregion

    }

}
