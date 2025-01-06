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
    /// A SetChargeDetailRecordRequest request.
    /// </summary>
    public class SetChargeDetailRecordRequest : ARequest<SetChargeDetailRecordRequest>
    {

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id         OperatorId            { get; }

        /// <summary>
        /// The charge detail record.
        /// </summary>
        public ChargeDetailRecord  ChargeDetailRecord    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a SetChargeDetailRecord XML/SOAP request.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="ChargeDetailRecord">The charge detail record.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetChargeDetailRecordRequest(Partner_Id          PartnerId,
                                            Operator_Id         OperatorId,
                                            ChargeDetailRecord  ChargeDetailRecord,
                                            Transaction_Id?     TransactionId       = null,

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
            this.ChargeDetailRecord  = ChargeDetailRecord;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //
        //   <soap:Header />
        //
        //   <soap:Body>
        //     <eMIP:eMIP_ToIOP_SetChargeDetailRecordRequest>
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
        //       <chargeDetailRecord>
        //           [...]
        //       </chargeDetailRecord>
        //
        //     </eMIP:eMIP_ToIOP_SetChargeDetailRecordRequest>
        //   </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (SetChargeDetailRecordRequestXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargeDetailRecordRequestXML">The XML to parse.</param>
        /// <param name="CustomSendSetChargeDetailRecordRequestParser">An optional delegate to parse custom SetChargeDetailRecordRequest XML elements.</param>
        /// <param name="CustomChargeDetailRecordParser">An optional delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetChargeDetailRecordRequest Parse(XElement                                               SetChargeDetailRecordRequestXML,
                                                         CustomXMLParserDelegate<SetChargeDetailRecordRequest>  CustomSendSetChargeDetailRecordRequestParser   = null,
                                                         CustomXMLParserDelegate<ChargeDetailRecord>            CustomChargeDetailRecordParser                 = null,
                                                         CustomXMLParserDelegate<MeterReport>                   CustomMeterReportParser                        = null,
                                                         OnExceptionDelegate                                    OnException                                    = null,

                                                         HTTPRequest                                            HTTPRequest                                    = null,
                                                         DateTime?                                              Timestamp                                      = null,
                                                         CancellationToken                                      CancellationToken                              = default,
                                                         EventTracking_Id                                       EventTrackingId                                = null,
                                                         TimeSpan?                                              RequestTimeout                                 = null)
        {

            if (TryParse(SetChargeDetailRecordRequestXML,
                         out SetChargeDetailRecordRequest _SetChargeDetailRecordRequest,
                         CustomSendSetChargeDetailRecordRequestParser,
                         CustomChargeDetailRecordParser,
                         CustomMeterReportParser,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetChargeDetailRecordRequest;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (SetChargeDetailRecordRequestText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargeDetailRecordRequestText">The text to parse.</param>
        /// <param name="CustomSendSetChargeDetailRecordRequestParser">An optional delegate to parse custom SetChargeDetailRecordRequest XML elements.</param>
        /// <param name="CustomChargeDetailRecordParser">An optional delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static SetChargeDetailRecordRequest Parse(String                                                  SetChargeDetailRecordRequestText,
                                                         CustomXMLParserDelegate<SetChargeDetailRecordRequest>?  CustomSendSetChargeDetailRecordRequestParser   = null,
                                                         CustomXMLParserDelegate<ChargeDetailRecord>?            CustomChargeDetailRecordParser                 = null,
                                                         CustomXMLParserDelegate<MeterReport>?                   CustomMeterReportParser                        = null,
                                                         OnExceptionDelegate?                                    OnException                                    = null,

                                                         HTTPRequest?                                            HTTPRequest                                    = null,
                                                         DateTime?                                               Timestamp                                      = null,
                                                         CancellationToken                                       CancellationToken                              = default,
                                                         EventTracking_Id?                                       EventTrackingId                                = null,
                                                         TimeSpan?                                               RequestTimeout                                 = null)
        {

            if (TryParse(SetChargeDetailRecordRequestText,
                         out SetChargeDetailRecordRequest _SetChargeDetailRecordRequest,
                         CustomSendSetChargeDetailRecordRequestParser,
                         CustomChargeDetailRecordParser,
                         CustomMeterReportParser,
                         OnException,

                         HTTPRequest,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return _SetChargeDetailRecordRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(SetChargeDetailRecordRequestXML,  ..., out SetChargeDetailRecordRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargeDetailRecordRequestXML">The XML to parse.</param>
        /// <param name="SetChargeDetailRecordRequest">The parsed heartbeat request.</param>
        /// <param name="CustomSendSetChargeDetailRecordRequestParser">An optional delegate to parse custom SetChargeDetailRecordRequest XML elements.</param>
        /// <param name="CustomChargeDetailRecordParser">An optional delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                                SetChargeDetailRecordRequestXML,
                                       out SetChargeDetailRecordRequest                        SetChargeDetailRecordRequest,
                                       CustomXMLParserDelegate<SetChargeDetailRecordRequest>?  CustomSendSetChargeDetailRecordRequestParser   = null,
                                       CustomXMLParserDelegate<ChargeDetailRecord>?            CustomChargeDetailRecordParser                 = null,
                                       CustomXMLParserDelegate<MeterReport>?                   CustomMeterReportParser                        = null,
                                       OnExceptionDelegate?                                    OnException                                    = null,

                                       HTTPRequest?                                            HTTPRequest                                    = null,
                                       DateTime?                                               Timestamp                                      = null,
                                       CancellationToken                                       CancellationToken                              = default,
                                       EventTracking_Id?                                       EventTrackingId                                = null,
                                       TimeSpan?                                               RequestTimeout                                 = null)
        {

            try
            {

                SetChargeDetailRecordRequest = new SetChargeDetailRecordRequest(

                                                   SetChargeDetailRecordRequestXML.MapValueOrFail    ("partnerId",      Partner_Id.Parse),
                                                   SetChargeDetailRecordRequestXML.MapValueOrFail    ("operatorId",     Operator_Id.Parse),

                                                   SetChargeDetailRecordRequestXML.MapElementOrFail  ("chargeDetailRecord",
                                                                                                      (s, e) => ChargeDetailRecord.Parse(s,
                                                                                                                                         CustomChargeDetailRecordParser,
                                                                                                                                         CustomMeterReportParser,
                                                                                                                                         e),
                                                                                                      OnException),

                                                   SetChargeDetailRecordRequestXML.MapValueOrNullable("transactionId",  Transaction_Id.Parse),

                                                   HTTPRequest,
                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout

                                               );


                if (CustomSendSetChargeDetailRecordRequestParser is not null)
                    SetChargeDetailRecordRequest = CustomSendSetChargeDetailRecordRequestParser(SetChargeDetailRecordRequestXML,
                                                                                                SetChargeDetailRecordRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetChargeDetailRecordRequestXML, e);

                SetChargeDetailRecordRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetChargeDetailRecordRequestText, ..., out SetChargeDetailRecordRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="SetChargeDetailRecordRequestText">The text to parse.</param>
        /// <param name="SetChargeDetailRecordRequest">The parsed heartbeat request.</param>
        /// <param name="CustomSendSetChargeDetailRecordRequestParser">An optional delegate to parse custom SetChargeDetailRecordRequest XML elements.</param>
        /// <param name="CustomChargeDetailRecordParser">An optional delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="HTTPRequest">The correlated HTTP request of this eMIP request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                  SetChargeDetailRecordRequestText,
                                       out SetChargeDetailRecordRequest                        SetChargeDetailRecordRequest,
                                       CustomXMLParserDelegate<SetChargeDetailRecordRequest>?  CustomSendSetChargeDetailRecordRequestParser   = null,
                                       CustomXMLParserDelegate<ChargeDetailRecord>?            CustomChargeDetailRecordParser                 = null,
                                       CustomXMLParserDelegate<MeterReport>?                   CustomMeterReportParser                        = null,
                                       OnExceptionDelegate?                                    OnException                                    = null,

                                       HTTPRequest?                                            HTTPRequest                                    = null,
                                       DateTime?                                               Timestamp                                      = null,
                                       CancellationToken                                       CancellationToken                              = default,
                                       EventTracking_Id?                                       EventTrackingId                                = null,
                                       TimeSpan?                                               RequestTimeout                                 = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetChargeDetailRecordRequestText).Root,
                             out SetChargeDetailRecordRequest,
                             CustomSendSetChargeDetailRecordRequestParser,
                             CustomChargeDetailRecordParser,
                             CustomMeterReportParser,
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
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetChargeDetailRecordRequestText, e);
            }

            SetChargeDetailRecordRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomSetChargeDetailRecordRequestSerializer = null, CustomChargeDetailRecordSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomSetChargeDetailRecordRequestSerializer">A delegate to serialize custom SetChargeDetailRecord request XML elements.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A delegate to serialize custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomMeterReportSerializer">A delegate to serialize custom MeterReport XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<SetChargeDetailRecordRequest> CustomSetChargeDetailRecordRequestSerializer  = null,
                              CustomXMLSerializerDelegate<ChargeDetailRecord>           CustomChargeDetailRecordSerializer            = null,
                              CustomXMLSerializerDelegate<MeterReport>                  CustomMeterReportSerializer                   = null)
        {

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_ToIOP_SetChargeDetailRecordRequest",

                          TransactionId.HasValue
                              ? new XElement("transactionId",  TransactionId.ToString())
                              : null,

                          new XElement("partnerIdType",        PartnerId. Format.AsText()),
                          new XElement("partnerId",            PartnerId.        ToString()),

                          new XElement("operatorIdType",       OperatorId.Format.AsText()),
                          new XElement("operatorId",           OperatorId.       ToString()),

                          ChargeDetailRecord.ToXML(CustomChargeDetailRecordSerializer: CustomChargeDetailRecordSerializer,
                                                   CustomMeterReportSerializer:        CustomMeterReportSerializer)

                      );


            return CustomSetChargeDetailRecordRequestSerializer is not null
                       ? CustomSetChargeDetailRecordRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SetChargeDetailRecordRequest1, SetChargeDetailRecordRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetChargeDetailRecordRequest1">A heartbeat request.</param>
        /// <param name="SetChargeDetailRecordRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetChargeDetailRecordRequest SetChargeDetailRecordRequest1, SetChargeDetailRecordRequest SetChargeDetailRecordRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetChargeDetailRecordRequest1, SetChargeDetailRecordRequest2))
                return true;

            // If one is null, but not both, return false.
            if (SetChargeDetailRecordRequest1 is null || SetChargeDetailRecordRequest2 is null)
                return false;

            return SetChargeDetailRecordRequest1.Equals(SetChargeDetailRecordRequest2);

        }

        #endregion

        #region Operator != (SetChargeDetailRecordRequest1, SetChargeDetailRecordRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="SetChargeDetailRecordRequest1">A heartbeat request.</param>
        /// <param name="SetChargeDetailRecordRequest2">Another heartbeat request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetChargeDetailRecordRequest SetChargeDetailRecordRequest1, SetChargeDetailRecordRequest SetChargeDetailRecordRequest2)

            => !(SetChargeDetailRecordRequest1 == SetChargeDetailRecordRequest2);

        #endregion

        #endregion

        #region IEquatable<SetChargeDetailRecordRequest> Members

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

            var SetChargeDetailRecordRequest = Object as SetChargeDetailRecordRequest;
            if ((Object) SetChargeDetailRecordRequest == null)
                return false;

            return Equals(SetChargeDetailRecordRequest);

        }

        #endregion

        #region Equals(SetChargeDetailRecordRequest)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="SetChargeDetailRecordRequest">A heartbeat request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetChargeDetailRecordRequest SetChargeDetailRecordRequest)
        {

            if ((Object) SetChargeDetailRecordRequest == null)
                return false;

            return ((!TransactionId.HasValue && !SetChargeDetailRecordRequest.TransactionId.HasValue) ||
                     (TransactionId.HasValue && SetChargeDetailRecordRequest.TransactionId.HasValue && TransactionId.Value.Equals(SetChargeDetailRecordRequest.TransactionId.Value))) &&

                   PartnerId.         Equals(SetChargeDetailRecordRequest.PartnerId)  &&
                   OperatorId.        Equals(SetChargeDetailRecordRequest.OperatorId) &&
                   ChargeDetailRecord.Equals(SetChargeDetailRecordRequest.ChargeDetailRecord);

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
                            ? TransactionId.GetHashCode() * 7
                            : 0) ^

                       PartnerId.         GetHashCode() * 5 ^
                       OperatorId.        GetHashCode() * 3 ^
                       ChargeDetailRecord.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(PartnerId, " / ",
                             OperatorId);

        #endregion

    }

}
