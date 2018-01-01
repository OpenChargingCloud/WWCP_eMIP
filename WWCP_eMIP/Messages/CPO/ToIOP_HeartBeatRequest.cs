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
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// An eMIP HeartBeat request.
    /// </summary>
    public class ToIOP_HeartBeatRequest : ARequest<ToIOP_HeartBeatRequest>
    {

        #region Properties

        /// <summary>
        /// The partner identification.
        /// </summary>
        public Partner_Id       PartnerId        { get; }

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id      OperatorId       { get; }

        /// <summary>
        /// The optional transaction identification.
        /// </summary>
        public Transaction_Id?  TransactionId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an eMIP AddCDRs XML/SOAP request.
        /// </summary>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public ToIOP_HeartBeatRequest(Partner_Id          PartnerId,
                                      Operator_Id         OperatorId,
                                      Transaction_Id?     TransactionId       = null,

                                      DateTime?           Timestamp           = null,
                                      CancellationToken?  CancellationToken   = null,
                                      EventTracking_Id    EventTrackingId     = null,
                                      TimeSpan?           RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.PartnerId      = PartnerId;
            this.OperatorId     = OperatorId;
            this.TransactionId  = TransactionId;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope">
        //
        //   <soap:Header />
        //
        //   <soap:Body xmlns:eMIP="https://api-iop.gireve.com/schemas/PlatformV1/">
        //     <eMIP:eMIP_ToIOP_HeartBeatRequest>
        //
        //       <transactionId>TRANSACTION_46151</transactionId>
        //
        //       <partnerIdType>eMI3</partnerIdType>
        //       <partnerId>FR*MSP</partnerId>
        //
        //       <operatorIdType>eMI3</operatorIdType>
        //       <operatorId>FR*798</operatorId>
        //
        //     </eMIP:eMIP_ToIOP_HeartBeatRequest>
        //   </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse(HeartBeatRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="HeartBeatRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ToIOP_HeartBeatRequest Parse(XElement             HeartBeatRequestXML,
                                                   OnExceptionDelegate  OnException = null)
        {

            if (TryParse(HeartBeatRequestXML, out ToIOP_HeartBeatRequest _HeartBeatRequest, OnException))
                return _HeartBeatRequest;

            return null;

        }

        #endregion

        #region (static) Parse(HeartBeatRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="HeartBeatRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ToIOP_HeartBeatRequest Parse(String               HeartBeatRequestText,
                                                   OnExceptionDelegate  OnException = null)
        {

            if (TryParse(HeartBeatRequestText, out ToIOP_HeartBeatRequest _HeartBeatRequest, OnException))
                return _HeartBeatRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(HeartBeatRequestXML,  out HeartBeatRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="HeartBeatRequestXML">The XML to parse.</param>
        /// <param name="HeartBeatRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                    HeartBeatRequestXML,
                                       out ToIOP_HeartBeatRequest  HeartBeatRequest,
                                       OnExceptionDelegate         OnException  = null)
        {

            try
            {

                HeartBeatRequest = new ToIOP_HeartBeatRequest(

                                       HeartBeatRequestXML.MapValueOrFail    (eMIPNS.Default + "partnerId",
                                                                              Partner_Id.Parse),

                                       HeartBeatRequestXML.MapValueOrFail    (eMIPNS.Default + "operatorId",
                                                                              Operator_Id.Parse),

                                       HeartBeatRequestXML.MapValueOrNullable(eMIPNS.Default + "transactionId",
                                                                              Transaction_Id.Parse)

                                   );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, HeartBeatRequestXML, e);

                HeartBeatRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(HeartBeatRequestText, out HeartBeatRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP heartbeat request.
        /// </summary>
        /// <param name="HeartBeatRequestText">The text to parse.</param>
        /// <param name="HeartBeatRequest">The parsed heartbeat request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                      HeartBeatRequestText,
                                       out ToIOP_HeartBeatRequest  HeartBeatRequest,
                                       OnExceptionDelegate         OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(HeartBeatRequestText).Root,
                             out HeartBeatRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, HeartBeatRequestText, e);
            }

            HeartBeatRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomHeartBeatRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomHeartBeatRequestSerializer">A delegate to serialize custom HeartBeat request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<ToIOP_HeartBeatRequest> CustomHeartBeatRequestSerializer = null)
        {

            var XML = new XElement(eMIPNS.Default + "eMIP_ToIOP_HeartBeatRequest",

                          TransactionId.HasValue
                              ? new XElement(eMIPNS.Default + "transactionId",  TransactionId.ToString())
                              : null,

                          new XElement(eMIPNS.Default + "partnerIdType",        PartnerId. Format.ToString()),
                          new XElement(eMIPNS.Default + "partnerId",            PartnerId.        ToString()),

                          new XElement(eMIPNS.Default + "operatorIdType",       OperatorId.Format.ToString()),
                          new XElement(eMIPNS.Default + "operatorId",           OperatorId.       ToString())

                      );


            return CustomHeartBeatRequestSerializer != null
                       ? CustomHeartBeatRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (HeartBeatRequest1, HeartBeatRequest2)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="HeartBeatRequest1">A heartbeat request.</param>
        /// <param name="HeartBeatRequest2">Another heartbeat request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ToIOP_HeartBeatRequest HeartBeatRequest1, ToIOP_HeartBeatRequest HeartBeatRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(HeartBeatRequest1, HeartBeatRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) HeartBeatRequest1 == null) || ((Object) HeartBeatRequest2 == null))
                return false;

            return HeartBeatRequest1.Equals(HeartBeatRequest2);

        }

        #endregion

        #region Operator != (HeartBeatRequest1, HeartBeatRequest2)

        /// <summary>
        /// Compares two heartbeat requests for inequality.
        /// </summary>
        /// <param name="HeartBeatRequest1">A heartbeat request.</param>
        /// <param name="HeartBeatRequest2">Another heartbeat request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ToIOP_HeartBeatRequest HeartBeatRequest1, ToIOP_HeartBeatRequest HeartBeatRequest2)

            => !(HeartBeatRequest1 == HeartBeatRequest2);

        #endregion

        #endregion

        #region IEquatable<HeartBeatRequest> Members

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

            var HeartBeatRequest = Object as ToIOP_HeartBeatRequest;
            if ((Object) HeartBeatRequest == null)
                return false;

            return Equals(HeartBeatRequest);

        }

        #endregion

        #region Equals(HeartBeatRequest)

        /// <summary>
        /// Compares two heartbeat requests for equality.
        /// </summary>
        /// <param name="HeartBeatRequest">A heartbeat request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ToIOP_HeartBeatRequest HeartBeatRequest)
        {

            if ((Object) HeartBeatRequest == null)
                return false;

            return ((!TransactionId.HasValue && !HeartBeatRequest.TransactionId.HasValue) ||
                     (TransactionId.HasValue && HeartBeatRequest.TransactionId.HasValue && TransactionId.Value.Equals(HeartBeatRequest.TransactionId.Value))) &&

                   PartnerId. Equals(HeartBeatRequest.PartnerId) &&
                   OperatorId.Equals(HeartBeatRequest.OperatorId);

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

            => String.Concat(CDRInfos.Count(), " charge detail record(s)");

        #endregion


    }

}
