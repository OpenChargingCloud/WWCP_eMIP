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
    /// A Heartbeat request.
    /// </summary>
    public class HeartbeatRequest : ARequest<HeartbeatRequest>
    {

        #region Properties

        /// <summary>
        /// The partner identification.
        /// </summary>
        public Partner_Id   PartnerId     { get; }

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id  OperatorId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a Heartbeat XML/SOAP request.
        /// </summary>
        /// <param name="PartnerId">The partner identification.</param>
        /// <param name="OperatorId">The operator identification.</param>
        /// <param name="TransactionId">An optional transaction identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public HeartbeatRequest(Partner_Id          PartnerId,
                                Operator_Id         OperatorId,
                                Transaction_Id?     TransactionId       = null,

                                DateTime?           Timestamp           = null,
                                CancellationToken?  CancellationToken   = null,
                                EventTracking_Id    EventTrackingId     = null,
                                TimeSpan?           RequestTimeout      = null)

            : base(TransactionId,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.PartnerId      = PartnerId;
            this.OperatorId     = OperatorId;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/PlatformV1/">
        //
        //   <soap:Header />
        //
        //   <soap:Body>
        //     <eMIP:eMIP_ToIOP_HeartbeatRequest>
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
        //     </eMIP:eMIP_ToIOP_HeartbeatRequest>
        //   </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (HeartbeatRequestXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="HeartbeatRequestXML">The XML to parse.</param>
        /// <param name="CustomSendHeartbeatRequestParser">An optional delegate to parse custom HeartbeatRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static HeartbeatRequest Parse(XElement                                   HeartbeatRequestXML,
                                             CustomXMLParserDelegate<HeartbeatRequest>  CustomSendHeartbeatRequestParser,
                                             OnExceptionDelegate                        OnException = null)
        {

            if (TryParse(HeartbeatRequestXML,
                         CustomSendHeartbeatRequestParser,
                         out HeartbeatRequest _HeartbeatRequest,
                         OnException))
            {
                return _HeartbeatRequest;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (HeartbeatRequestText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="HeartbeatRequestText">The text to parse.</param>
        /// <param name="CustomSendHeartbeatRequestParser">An optional delegate to parse custom HeartbeatRequest XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static HeartbeatRequest Parse(String                                     HeartbeatRequestText,
                                             CustomXMLParserDelegate<HeartbeatRequest>  CustomSendHeartbeatRequestParser,
                                             OnExceptionDelegate                        OnException = null)
        {

            if (TryParse(HeartbeatRequestText,
                         CustomSendHeartbeatRequestParser,
                         out HeartbeatRequest _HeartbeatRequest,
                         OnException))
            {
                return _HeartbeatRequest;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(HeartbeatRequestXML,  ..., out HeartbeatRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="HeartbeatRequestXML">The XML to parse.</param>
        /// <param name="CustomSendHeartbeatRequestParser">An optional delegate to parse custom HeartbeatRequest XML elements.</param>
        /// <param name="HeartbeatRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                   HeartbeatRequestXML,
                                       CustomXMLParserDelegate<HeartbeatRequest>  CustomSendHeartbeatRequestParser,
                                       out HeartbeatRequest                       HeartbeatRequest,
                                       OnExceptionDelegate                        OnException  = null)
        {

            try
            {

                HeartbeatRequest = new HeartbeatRequest(

                                       HeartbeatRequestXML.MapValueOrFail    (eMIPNS.Default + "partnerId",
                                                                              Partner_Id.Parse),

                                       HeartbeatRequestXML.MapValueOrFail    (eMIPNS.Default + "operatorId",
                                                                              Operator_Id.Parse),

                                       HeartbeatRequestXML.MapValueOrNullable(eMIPNS.Default + "transactionId",
                                                                              Transaction_Id.Parse)

                                   );


                if (CustomSendHeartbeatRequestParser != null)
                    HeartbeatRequest = CustomSendHeartbeatRequestParser(HeartbeatRequestXML,
                                                                        HeartbeatRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, HeartbeatRequestXML, e);

                HeartbeatRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(HeartbeatRequestText, ..., out HeartbeatRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="HeartbeatRequestText">The text to parse.</param>
        /// <param name="CustomSendHeartbeatRequestParser">An optional delegate to parse custom HeartbeatRequest XML elements.</param>
        /// <param name="HeartbeatRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                     HeartbeatRequestText,
                                       CustomXMLParserDelegate<HeartbeatRequest>  CustomSendHeartbeatRequestParser,
                                       out HeartbeatRequest                       HeartbeatRequest,
                                       OnExceptionDelegate                        OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(HeartbeatRequestText).Root,
                             CustomSendHeartbeatRequestParser,
                             out HeartbeatRequest,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, HeartbeatRequestText, e);
            }

            HeartbeatRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomHeartbeatRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomHeartbeatRequestSerializer">A delegate to serialize custom Heartbeat request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<HeartbeatRequest> CustomHeartbeatRequestSerializer = null)
        {

            var XML = new XElement(eMIPNS.Default + "eMIP_ToIOP_HeartbeatRequest",

                          TransactionId.HasValue
                              ? new XElement(eMIPNS.Default + "transactionId",  TransactionId.ToString())
                              : null,

                          new XElement(eMIPNS.Default + "partnerIdType",        PartnerId. Format.ToString()),
                          new XElement(eMIPNS.Default + "partnerId",            PartnerId.        ToString()),

                          new XElement(eMIPNS.Default + "operatorIdType",       OperatorId.Format.ToString()),
                          new XElement(eMIPNS.Default + "operatorId",           OperatorId.       ToString())

                      );


            return CustomHeartbeatRequestSerializer != null
                       ? CustomHeartbeatRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (HeartbeatRequest1, HeartbeatRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="HeartbeatRequest1">A heartbeat request.</param>
        /// <param name="HeartbeatRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (HeartbeatRequest HeartbeatRequest1, HeartbeatRequest HeartbeatRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(HeartbeatRequest1, HeartbeatRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) HeartbeatRequest1 == null) || ((Object) HeartbeatRequest2 == null))
                return false;

            return HeartbeatRequest1.Equals(HeartbeatRequest2);

        }

        #endregion

        #region Operator != (HeartbeatRequest1, HeartbeatRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="HeartbeatRequest1">A heartbeat request.</param>
        /// <param name="HeartbeatRequest2">Another heartbeat request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (HeartbeatRequest HeartbeatRequest1, HeartbeatRequest HeartbeatRequest2)

            => !(HeartbeatRequest1 == HeartbeatRequest2);

        #endregion

        #endregion

        #region IEquatable<HeartbeatRequest> Members

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

            var HeartbeatRequest = Object as HeartbeatRequest;
            if ((Object) HeartbeatRequest == null)
                return false;

            return Equals(HeartbeatRequest);

        }

        #endregion

        #region Equals(HeartbeatRequest)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="HeartbeatRequest">A heartbeat request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(HeartbeatRequest HeartbeatRequest)
        {

            if ((Object) HeartbeatRequest == null)
                return false;

            return ((!TransactionId.HasValue && !HeartbeatRequest.TransactionId.HasValue) ||
                     (TransactionId.HasValue && HeartbeatRequest.TransactionId.HasValue && TransactionId.Value.Equals(HeartbeatRequest.TransactionId.Value))) &&

                   PartnerId. Equals(HeartbeatRequest.PartnerId) &&
                   OperatorId.Equals(HeartbeatRequest.OperatorId);

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
                            ? TransactionId.GetHashCode() * 5
                            : 0) ^

                       PartnerId. GetHashCode() * 3 ^
                       OperatorId.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(PartnerId, " / ",
                             OperatorId);

        #endregion


    }

}
