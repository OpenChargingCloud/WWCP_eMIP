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

using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.EMP
{

    /// <summary>
    /// A SetServiceAuthorisationRequest request.
    /// </summary>
    public class SetServiceAuthorisationRequest : ARequest<SetServiceAuthorisationRequest>
    {

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id                OperatorId                  { get; }

        /// <summary>
        /// The EVSE identification.
        /// </summary>
        public EVSE_Id                    EVSEId                      { get; }

        /// <summary>
        /// The user identification.
        /// </summary>
        public User_Id                    UserId                      { get; }

        /// <summary>
        /// The service identification for which an authorisation is requested.
        /// </summary>
        public Service_Id                 RequestedServiceId          { get; }

        /// <summary>
        /// The partner service session identification.
        /// </summary>
        public PartnerServiceSession_Id?  PartnerServiceSessionId     { get; }

        /// <summary>
        /// Whether to start or stop the charging process.
        /// </summary>
        public RemoteStartStopValues      AuthorisationValue          { get; }

        /// <summary>
        /// Whether the eMSP wishes to receive intermediate charging session records.
        /// </summary>
        public Boolean                    IntermediateCDRRequested    { get; }

        /// <summary>
        /// Anonymized alias of the contract id between the end-user and the eMSP.
        /// </summary>
        public Contract_Id?               UserContractIdAlias         { get; }

        /// <summary>
        /// An optional meter limits for this authorisation:
        /// The eMSP can authorise the charge but for less than x kWh or y minutes, or z euros.
        /// </summary>
        public IEnumerable<MeterReport>   MeterLimits                 { get; }

        /// <summary>
        /// eMSP parameter string (reserved for future use).
        /// </summary>
        public String                     Parameter                   { get; }

        /// <summary>
        /// The booking identification.
        /// </summary>
        public Booking_Id?                BookingId                   { get; }

        /// <summary>
        /// The booking identification.
        /// </summary>
        public Booking_Id?                SalePartnerBookingId        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a SetServiceAuthorisationRequest XML/SOAP request.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="UserId">The user identification.</param>
        /// <param name="RequestedServiceId">The service identification for which an authorisation is requested.</param>
        /// <param name="PartnerServiceSessionId">The partner service session identification.</param>
        /// <param name="AuthorisationValue">Whether to start or stop the charging process.</param>
        /// <param name="IntermediateCDRRequested">Whether the eMSP wishes to receive intermediate charging session records.</param>
        /// 
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// <param name="UserContractIdAlias">Anonymized alias of the contract id between the end-user and the eMSP.</param>
        /// <param name="MeterLimits">Meter limits for this authorisation: The eMSP can authorise the charge but for less than x Wh or y minutes, or z euros.</param>
        /// <param name="Parameter">eMSP parameter string (reserved for future use).</param>
        /// <param name="BookingId"></param>
        /// <param name="SalePartnerBookingId"></param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetServiceAuthorisationRequest(Partner_Id                 PartnerId,
                                              Operator_Id                OperatorId,
                                              EVSE_Id                    EVSEId,
                                              User_Id                    UserId,
                                              Service_Id                 RequestedServiceId,
                                              RemoteStartStopValues      AuthorisationValue,
                                              Boolean                    IntermediateCDRRequested,

                                              Transaction_Id?            TransactionId             = null,
                                              PartnerServiceSession_Id?  PartnerServiceSessionId   = null,
                                              Contract_Id?               UserContractIdAlias       = null,
                                              IEnumerable<MeterReport>?  MeterLimits               = null,
                                              String?                    Parameter                 = null,
                                              Booking_Id?                BookingId                 = null,
                                              Booking_Id?                SalePartnerBookingId      = null,

                                              HTTPRequest?               HTTPRequest               = null,
                                              DateTime?                  Timestamp                 = null,
                                              CancellationToken          CancellationToken         = default,
                                              EventTracking_Id?          EventTrackingId           = null,
                                              TimeSpan?                  RequestTimeout            = null)

            : base(HTTPRequest,
                   PartnerId,
                   TransactionId,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.OperatorId                = OperatorId;
            this.EVSEId                    = EVSEId;
            this.UserId                    = UserId;
            this.RequestedServiceId        = RequestedServiceId;
            this.AuthorisationValue        = AuthorisationValue;
            this.IntermediateCDRRequested  = IntermediateCDRRequested;

            this.PartnerServiceSessionId   = PartnerServiceSessionId;
            this.UserContractIdAlias       = UserContractIdAlias;
            this.MeterLimits               = MeterLimits ?? Array.Empty<MeterReport>();
            this.Parameter                 = Parameter   ?? String.Empty;
            this.BookingId                 = BookingId;
            this.SalePartnerBookingId      = SalePartnerBookingId;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:aut  = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //
        //    <soap:Header/>
        //    <soap:Body>
        //       <aut:eMIP_ToIOP_SetServiceAuthorisationRequest>
        //
        //          <!--Optional:-->
        //          <transactionId>?</transactionId>
        //
        //          <partnerIdType>?</partnerIdType>
        //          <partnerId>?</partnerId>
        //          <operatorIdType>?</operatorIdType>
        //          <operatorId>?</operatorId>
        //          <EVSEIdType>?</EVSEIdType>
        //          <EVSEId>?</EVSEId>
        //          <userIdType>?</userIdType>
        //          <userId>?</userId>
        //
        //          <requestedServiceId>?</requestedServiceId>
        //
        //          <!--Optional:-->
        //          <partnerServiceSessionId>?</partnerServiceSessionId>
        //
        //          <authorisationValue>?</authorisationValue>
        //          <intermediateCDRRequested>?</intermediateCDRRequested>
        //
        //          <!--Optional:-->
        //          <userContractIdAlias>?</userContractIdAlias>
        //
        //          <!--Optional:-->
        //          <meterLimitList>
        //             <!--Zero or more repetitions:-->
        //             <meterReport>
        //                <meterTypeId>?</meterTypeId>
        //                <meterValue>?</meterValue>
        //                <meterUnit>?</meterUnit>
        //             </meterReport>
        //          </meterLimitList>
        //
        //          <!--Optional:-->
        //          <parameter>IOP={specific data for IOP}CPO={comment}</parameter>
        //
        //          <!--Optional:-->
        //          <bookingId>?</bookingId>
        //
        //          <!--Optional:-->
        //          <salePartnerBookingId>?</salePartnerBookingId>
        //
        //       </aut:eMIP_ToIOP_SetServiceAuthorisationRequest>
        //    </soap:Body>
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (SetServiceAuthorisationRequestXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetServiceAuthorisationRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetServiceAuthorisationRequestParser">An optional delegate to parse custom SetServiceAuthorisationRequest XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static SetServiceAuthorisationRequest? Parse(XElement                                                  SetServiceAuthorisationRequestXML,
                                                            CustomXMLParserDelegate<SetServiceAuthorisationRequest>?  CustomSendSetServiceAuthorisationRequestParser   = null,
                                                            CustomXMLParserDelegate<MeterReport>?                     CustomMeterReportParser                          = null,
                                                            OnExceptionDelegate?                                      OnException                                      = null,

                                                            HTTPRequest?                                              HTTPRequest                                      = null,
                                                            DateTime?                                                 Timestamp                                        = null,
                                                            EventTracking_Id?                                         EventTrackingId                                  = null,
                                                            TimeSpan?                                                 RequestTimeout                                   = null,
                                                            CancellationToken                                         CancellationToken                                = default)
        {

            if (TryParse(SetServiceAuthorisationRequestXML,
                         out var setServiceAuthorisationRequest,
                         CustomSendSetServiceAuthorisationRequestParser,
                         CustomMeterReportParser,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CancellationToken))
            {
                return setServiceAuthorisationRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SetServiceAuthorisationRequestXML,  ..., out SetServiceAuthorisationRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetServiceAuthorisationRequestXML">The XML to parse.</param>
        /// <param name="SetServiceAuthorisationRequest">The parsed heartbeat request.</param>
        /// <param name="CustomSendSetServiceAuthorisationRequestParser">An optional delegate to parse custom SetServiceAuthorisationRequest XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Boolean TryParse(XElement                                                  SetServiceAuthorisationRequestXML,
                                       out SetServiceAuthorisationRequest?                       SetServiceAuthorisationRequest,
                                       CustomXMLParserDelegate<SetServiceAuthorisationRequest>?  CustomSendSetServiceAuthorisationRequestParser   = null,
                                       CustomXMLParserDelegate<MeterReport>?                     CustomMeterReportParser                          = null,
                                       OnExceptionDelegate?                                      OnException                                      = null,

                                       HTTPRequest?                                              HTTPRequest                                      = null,
                                       DateTime?                                                 Timestamp                                        = null,
                                       EventTracking_Id?                                         EventTrackingId                                  = null,
                                       TimeSpan?                                                 RequestTimeout                                   = null,
                                       CancellationToken                                         CancellationToken                                = default)
        {

            try
            {

                SetServiceAuthorisationRequest = new SetServiceAuthorisationRequest(

                                                     //ToDo: What to do with: <partnerIdType>eMI3</partnerIdType>?
                                                     SetServiceAuthorisationRequestXML.MapValueOrFail       ("partnerId",                Partner_Id.Parse),

                                                     //ToDo: What to do with: <operatorIdType>eMI3</operatorIdType>
                                                     SetServiceAuthorisationRequestXML.MapValueOrFail       ("operatorId",               Operator_Id.Parse),

                                                     //ToDo: What to do with: <EVSEIdType>eMI3</EVSEIdType>
                                                     SetServiceAuthorisationRequestXML.MapValueOrFail       ("EVSEId",                   EVSE_Id.Parse),

                                                     SetServiceAuthorisationRequestXML.MapValueOrFail       ("userId",                   s => User_Id.Parse(s,
                                                         SetServiceAuthorisationRequestXML.MapValueOrFail   ("userIdType",               UserIdFormatsExtensions.Parse))),

                                                     SetServiceAuthorisationRequestXML.MapValueOrFail       ("requestedServiceId",       Service_Id.Parse),
                                                     SetServiceAuthorisationRequestXML.MapValueOrFail       ("authorisationValue",       ConversionMethods.AsRemoteStartStopValue),
                                                     SetServiceAuthorisationRequestXML.MapBooleanOrFail     ("intermediateCDRRequested"),

                                                     SetServiceAuthorisationRequestXML.MapValueOrNullable   ("transactionId",            Transaction_Id.Parse),
                                                     SetServiceAuthorisationRequestXML.MapValueOrNullable   ("partnerServiceSessionId",  PartnerServiceSession_Id.Parse),
                                                     SetServiceAuthorisationRequestXML.MapValueOrNullable   ("userContractIdAlias",      Contract_Id.Parse),

                                                     SetServiceAuthorisationRequestXML.MapElements          ("meterLimitList",
                                                                                                             "meterReport",              xml => MeterReport.Parse(xml,
                                                                                                                                                                  CustomMeterReportParser,
                                                                                                                                                                  OnException)),
                                                     SetServiceAuthorisationRequestXML.ElementValueOrDefault("userContractIdAlias"),
                                                     SetServiceAuthorisationRequestXML.MapValueOrNullable   ("bookingId",                Booking_Id.Parse),
                                                     SetServiceAuthorisationRequestXML.MapValueOrNullable   ("salePartnerBookingId",     Booking_Id.Parse),

                                                     HTTPRequest,
                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout

                                                 );


                if (CustomSendSetServiceAuthorisationRequestParser is not null)
                    SetServiceAuthorisationRequest = CustomSendSetServiceAuthorisationRequestParser(SetServiceAuthorisationRequestXML,
                                                                                                    SetServiceAuthorisationRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetServiceAuthorisationRequestXML, e);

                SetServiceAuthorisationRequest = null;
                return false;

            }

        }

        #endregion

        #region ToXML(CustomSetServiceAuthorisationRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetServiceAuthorisationRequestSerializer">A delegate to serialize custom set EVSE busy status request XML elements.</param>
        /// <param name="CustomMeterReportSerializer">A delegate to serialize custom MeterReport XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetServiceAuthorisationRequest>?  CustomSetServiceAuthorisationRequestSerializer  = null,
                              CustomXMLSerializerDelegate<MeterReport>?                     CustomMeterReportSerializer                     = null)
        {

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_ToIOP_SetServiceAuthorisationRequest",

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
                          new XElement("authorisationValue",             AuthorisationValue.           ToString()),
                          new XElement("intermediateCDRRequested",       IntermediateCDRRequested ? "1" : "0"),

                          PartnerServiceSessionId.HasValue
                              ? new XElement("serviceSessionId",         PartnerServiceSessionId.      ToString())
                              : null,

                          UserContractIdAlias.HasValue
                              ? new XElement("userContractIdAlias",      UserContractIdAlias.          ToString())
                              : null,

                          MeterLimits.SafeAny()
                              ? new XElement("meterLimitList",           MeterLimits.Select(meterreport => meterreport.ToXML(CustomMeterReportSerializer: CustomMeterReportSerializer)))
                              : null,

                          Parameter.IsNotNullOrEmpty()
                              ? new XElement("parameter",                Parameter)
                              : null,

                          BookingId.HasValue
                              ? new XElement("bookingId",                BookingId.                    ToString())
                              : null,

                          SalePartnerBookingId.HasValue
                              ? new XElement("salePartnerBookingId",     SalePartnerBookingId.         ToString())
                              : null

                      );


            return CustomSetServiceAuthorisationRequestSerializer is not null
                       ? CustomSetServiceAuthorisationRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetServiceAuthorisationRequest1, SetServiceAuthorisationRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetServiceAuthorisationRequest1">A heartbeat request.</param>
        /// <param name="SetServiceAuthorisationRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetServiceAuthorisationRequest SetServiceAuthorisationRequest1,
                                           SetServiceAuthorisationRequest SetServiceAuthorisationRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetServiceAuthorisationRequest1, SetServiceAuthorisationRequest2))
                return true;

            // If one is null, but not both, return false.
            if (SetServiceAuthorisationRequest1 is null || SetServiceAuthorisationRequest2 is null)
                return false;

            return SetServiceAuthorisationRequest1.Equals(SetServiceAuthorisationRequest2);

        }

        #endregion

        #region Operator != (SetServiceAuthorisationRequest1, SetServiceAuthorisationRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="SetServiceAuthorisationRequest1">A heartbeat request.</param>
        /// <param name="SetServiceAuthorisationRequest2">Another heartbeat request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetServiceAuthorisationRequest SetServiceAuthorisationRequest1,
                                           SetServiceAuthorisationRequest SetServiceAuthorisationRequest2)

            => !(SetServiceAuthorisationRequest1 == SetServiceAuthorisationRequest2);

        #endregion

        #endregion

        #region IEquatable<SetServiceAuthorisationRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two SetServiceAuthorisation requests of this object.
        /// </summary>
        /// <param name="Object">A SetServiceAuthorisation request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SetServiceAuthorisationRequest setServiceAuthorisationRequest &&
                   Equals(setServiceAuthorisationRequest);

        #endregion

        #region Equals(SetServiceAuthorisationRequest)

        /// <summary>
        /// Compares two SetServiceAuthorisation requests of this object.
        /// </summary>
        /// <param name="SetServiceAuthorisationRequest">A SetServiceAuthorisation request to compare with.</param>
        public override Boolean Equals(SetServiceAuthorisationRequest? SetServiceAuthorisationRequest)

            => SetServiceAuthorisationRequest is not null &&

            ((!TransactionId.HasValue && !SetServiceAuthorisationRequest.TransactionId.HasValue) ||
              (TransactionId.HasValue &&  SetServiceAuthorisationRequest.TransactionId.HasValue && TransactionId.Value.Equals(SetServiceAuthorisationRequest.TransactionId.Value))) &&

               PartnerId.         Equals(SetServiceAuthorisationRequest.PartnerId)          &&
               OperatorId.        Equals(SetServiceAuthorisationRequest.OperatorId)         &&
               EVSEId.            Equals(SetServiceAuthorisationRequest.EVSEId)             &&
               UserId.            Equals(SetServiceAuthorisationRequest.UserId)             &&
               RequestedServiceId.Equals(SetServiceAuthorisationRequest.RequestedServiceId);

               //((!PartnerServiceSessionId.HasValue && !SetServiceAuthorisationRequest.PartnerServiceSessionId.HasValue) ||
               //  (PartnerServiceSessionId.HasValue &&  SetServiceAuthorisationRequest.PartnerServiceSessionId.HasValue && PartnerServiceSessionId.Value.Equals(SetServiceAuthorisationRequest.PartnerServiceSessionId.Value)));

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

                return (TransactionId?.    GetHashCode() ?? 0) * 19 ^
                        PartnerId.         GetHashCode()       * 17 ^
                        OperatorId.        GetHashCode()       * 13 ^
                        EVSEId.            GetHashCode()       * 11 ^
                        UserId.            GetHashCode()       *  7 ^
                        RequestedServiceId.GetHashCode()       *  3;

                       //(PartnerServiceSessionId.HasValue
                       //     ? PartnerServiceSessionId.GetHashCode()
                       //     : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{PartnerId} / {OperatorId}: {UserId} ({RequestedServiceId}) @ {EVSEId}";

        #endregion

    }

}
