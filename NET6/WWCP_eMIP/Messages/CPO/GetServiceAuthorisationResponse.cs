/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod.JSON;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A GetServiceAuthorisation response.
    /// </summary>
    public class GetServiceAuthorisationResponse : AResponse<GetServiceAuthorisationRequest,
                                                             GetServiceAuthorisationResponse>
    {

        #region Properties

        /// <summary>
        /// The result of the authorisation.
        /// </summary>
        public AuthorisationValues       AuthorisationValue          { get; }

        /// <summary>
        /// The GIREVE session id for this service session.
        /// </summary>
        public ServiceSession_Id         ServiceSessionId              { get; }

        /// <summary>
        /// Whether the eMSP wishes to receive intermediate charging session records.
        /// </summary>
        public Boolean                   IntermediateCDRRequested    { get; }


        /// <summary>
        /// The optional sales operator identification.
        /// </summary>
        public Provider_Id?              SalesPartnerOperatorId      { get; }

        /// <summary>
        /// An optional alias of the contract id between the end-user and the eMSP.
        /// This alias may have been anonymised by the eMSP.
        /// </summary>
        public Contract_Id?              UserContractIdAlias         { get; }

        /// <summary>
        /// An optional meter limits for this authorisation:
        /// The eMSP can authorise the charge but for less than x kWh or y minutes, or z euros.
        /// </summary>
        public IEnumerable<MeterReport>  MeterLimits                 { get; }

        /// <summary>
        /// Optional information from the CPO to the eMSP.
        /// </summary>
        public String                    Parameter                   { get; }

        #endregion

        #region Constructor(s)

        #region GetServiceAuthorisationResponse(Request, TransactionId, RequestStatus, CustomData = null)

        /// <summary>
        /// Create a new GetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The GetServiceAuthorisation request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// 
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public GetServiceAuthorisationResponse(GetServiceAuthorisationRequest       Request,
                                               Transaction_Id                       TransactionId,
                                               RequestStatus                        RequestStatus,

                                               HTTPResponse                         HTTPResponse   = null,
                                               IReadOnlyDictionary<String, Object>  CustomData     = null)

            : this(Request,
                   TransactionId,
                   AuthorisationValues.KO,
                   ServiceSession_Id.Zero,
                   false,
                   RequestStatus,
                   null,
                   null,
                   null,
                   null,
                   HTTPResponse,
                   CustomData)

        { }

        #endregion

        #region GetServiceAuthorisationResponse(Request, TransactionId, ...,           CustomData = null)

        /// <summary>
        /// Create a new GetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The GetServiceAuthorisation request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="AuthorisationValue">The result of the authorisation.</param>
        /// <param name="ServiceSessionId">The GIREVE session id for this service session.</param>
        /// <param name="IntermediateCDRRequested">Whether the eMSP wishes to receive intermediate charging session records.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// 
        /// <param name="SalesPartnerOperatorId">The optional sales operator identification.</param>
        /// <param name="UserContractIdAlias">An optional alias of the contract id between the end-user and the eMSP. This alias may have been anonymised by the eMSP.</param>
        /// <param name="MeterLimits">An optional meter limits for this authorisation: The eMSP can authorise the charge but for less than x kWh or y minutes, or z euros.</param>
        /// <param name="Parameter">Optional information from the CPO to the eMSP.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public GetServiceAuthorisationResponse(GetServiceAuthorisationRequest       Request,
                                               Transaction_Id                       TransactionId,
                                               AuthorisationValues                  AuthorisationValue,
                                               ServiceSession_Id                    ServiceSessionId,
                                               Boolean                              IntermediateCDRRequested,
                                               RequestStatus                        RequestStatus,

                                               Provider_Id?                         SalesPartnerOperatorId   = null,
                                               Contract_Id?                         UserContractIdAlias      = null,
                                               IEnumerable<MeterReport>             MeterLimits              = null,
                                               String                               Parameter                = null,
                                               HTTPResponse                         HTTPResponse             = null,
                                               IReadOnlyDictionary<String, Object>  CustomData               = null)

            : base(Request,
                   TransactionId,
                   RequestStatus,
                   HTTPResponse,
                   CustomData)

        {

            this.AuthorisationValue        = AuthorisationValue;
            this.ServiceSessionId          = ServiceSessionId;
            this.IntermediateCDRRequested  = IntermediateCDRRequested;

            this.SalesPartnerOperatorId    = SalesPartnerOperatorId;
            this.UserContractIdAlias       = UserContractIdAlias;
            this.MeterLimits               = MeterLimits ?? new MeterReport[0];
            this.Parameter                 = Parameter;

        }

        #endregion

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //
        //    <soap:Header/>
        //
        //    <soap:Body>
        //       <eMIP:eMIP_ToIOP_GetServiceAuthorisationResponse>
        //
        //          <transactionId>TRANSACTION_46151</transactionId>
        //
        //          <!--Optional:-->
        //          <salePartnerOperatorIdType>?</salePartnerOperatorIdType>
        //
        //          <!--Optional:-->
        //          <salePartnerOperatorId>?</salePartnerOperatorId>
        //
        //          <authorisationValue>?</authorisationValue>
        //          <serviceSessionId>?</serviceSessionId>
        //          <intermediateCDRRequested>?</intermediateCDRRequested>
        //
        //          <!--Optional:-->
        //          <userContractIdAlias>?</userContractIdAlias>
        //
        //          <!--Optional:-->
        //          <meterLimitList>
        //
        //             <!--Zero or more repetitions:-->
        //             <meterReport>
        //                <meterTypeId>?</meterTypeId>
        //                <meterValue>?</meterValue>
        //                <meterUnit>?</meterUnit>
        //             </meterReport>
        //
        //          </meterLimitList>
        //
        //          <!--Optional:-->
        //          <!-- IOP={specific data for IOP}CPO={comment} -->
        //          <parameter>?</parameter>
        //
        //          <!--       1: OK-Normal -->
        //          <!--     202: OK-Warning: There is no roaming contract between the CPO and the eMSP for the requested service! -->
        //          <!--     203: OK-Warning: The eMSP of the end-user cannot be identified! -->
        //          <!--   10210: Ko-Error:   The eMSP did not respond correctly to the request! -->
        //          <!--  <10000: OK:         Reserved for future use! -->
        //          <!-- >=10000: Ko-Error:   Reserved for future use! -->
        //          <requestStatus>1</requestStatus>
        //
        //       </eMIP:eMIP_ToIOP_GetServiceAuthorisationResponse>
        //    </soap:Body>
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, GetServiceAuthorisationResponseXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a GetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The GetServiceAuthorisation request leading to this response.</param>
        /// <param name="GetServiceAuthorisationResponseXML">The XML to parse.</param>
        /// <param name="CustomSendGetServiceAuthorisationResponseParser">An optional delegate to parse custom GetServiceAuthorisationResponse XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetServiceAuthorisationResponse Parse(GetServiceAuthorisationRequest                            Request,
                                                            XElement                                                  GetServiceAuthorisationResponseXML,
                                                            CustomXMLParserDelegate<GetServiceAuthorisationResponse>  CustomSendGetServiceAuthorisationResponseParser  = null,
                                                            CustomXMLParserDelegate<MeterReport>                      CustomMeterReportParser                          = null,
                                                            HTTPResponse                                              HTTPResponse                                     = null,
                                                            OnExceptionDelegate                                       OnException                                      = null)
        {

            if (TryParse(Request,
                         GetServiceAuthorisationResponseXML,
                         out GetServiceAuthorisationResponse GetServiceAuthorisationResponse,
                         CustomSendGetServiceAuthorisationResponseParser,
                         CustomMeterReportParser,
                         HTTPResponse,
                         OnException))
            {
                return GetServiceAuthorisationResponse;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (Request, GetServiceAuthorisationResponseText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a GetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The GetServiceAuthorisation request leading to this response.</param>
        /// <param name="GetServiceAuthorisationResponseText">The text to parse.</param>
        /// <param name="CustomSendGetServiceAuthorisationResponseParser">An optional delegate to parse custom GetServiceAuthorisationResponse XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetServiceAuthorisationResponse Parse(GetServiceAuthorisationRequest                            Request,
                                                            String                                                    GetServiceAuthorisationResponseText,
                                                            CustomXMLParserDelegate<GetServiceAuthorisationResponse>  CustomSendGetServiceAuthorisationResponseParser  = null,
                                                            CustomXMLParserDelegate<MeterReport>                      CustomMeterReportParser                          = null,
                                                            HTTPResponse                                              HTTPResponse                                     = null,
                                                            OnExceptionDelegate                                       OnException                                      = null)
        {

            if (TryParse(Request,
                         GetServiceAuthorisationResponseText,
                         out GetServiceAuthorisationResponse GetServiceAuthorisationResponse,
                         CustomSendGetServiceAuthorisationResponseParser,
                         CustomMeterReportParser,
                         HTTPResponse,
                         OnException))
            {
                return GetServiceAuthorisationResponse;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Request, GetServiceAuthorisationResponseXML,  ..., out GetServiceAuthorisationResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a GetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The GetServiceAuthorisation request leading to this response.</param>
        /// <param name="GetServiceAuthorisationResponseXML">The XML to parse.</param>
        /// <param name="GetServiceAuthorisationResponse">The parsed GetServiceAuthorisation response.</param>
        /// <param name="CustomSendGetServiceAuthorisationResponseParser">An optional delegate to parse custom GetServiceAuthorisationResponse XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetServiceAuthorisationRequest                            Request,
                                       XElement                                                  GetServiceAuthorisationResponseXML,
                                       out GetServiceAuthorisationResponse                       GetServiceAuthorisationResponse,
                                       CustomXMLParserDelegate<GetServiceAuthorisationResponse>  CustomSendGetServiceAuthorisationResponseParser  = null,
                                       CustomXMLParserDelegate<MeterReport>                      CustomMeterReportParser                          = null,
                                       HTTPResponse                                              HTTPResponse                                     = null,
                                       OnExceptionDelegate                                       OnException                                      = null)
        {

            try
            {

                GetServiceAuthorisationResponse = new GetServiceAuthorisationResponse(

                                                      Request,

                                                      GetServiceAuthorisationResponseXML.MapValueOrFail    ("transactionId",             Transaction_Id.Parse),
                                                      GetServiceAuthorisationResponseXML.MapValueOrFail    ("authorisationValue",        ConversionMethods.AsAuthorisationValue),
                                                      GetServiceAuthorisationResponseXML.MapValueOrFail    ("serviceSessionId",          ServiceSession_Id.Parse),
                                                      GetServiceAuthorisationResponseXML.MapBooleanOrFail  ("intermediateCDRRequested"),

                                                      GetServiceAuthorisationResponseXML.MapValueOrFail    ("requestStatus",             RequestStatus.Parse),
                                                      GetServiceAuthorisationResponseXML.MapValueOrNullable("salePartnerOperatorId",     s => Provider_Id.Parse(s.Replace("*", "-"))),
                                                      GetServiceAuthorisationResponseXML.MapValueOrNullable("userContractIdAlias",       Contract_Id.Parse),

                                                      GetServiceAuthorisationResponseXML.MapElements       ("meterLimitList",
                                                                                                            "meterReport",
                                                                                                            s => MeterReport.Parse(s,
                                                                                                                                   CustomMeterReportParser,
                                                                                                                                   OnException)),

                                                      GetServiceAuthorisationResponseXML.MapValueOrNull    ("parameter"),

                                                      HTTPResponse

                                                  );


                if (CustomSendGetServiceAuthorisationResponseParser != null)
                    GetServiceAuthorisationResponse = CustomSendGetServiceAuthorisationResponseParser(GetServiceAuthorisationResponseXML,
                                                                                                      GetServiceAuthorisationResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, GetServiceAuthorisationResponseXML, e);

                GetServiceAuthorisationResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, GetServiceAuthorisationResponseText, ..., out GetServiceAuthorisationResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a GetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The GetServiceAuthorisation request leading to this response.</param>
        /// <param name="GetServiceAuthorisationResponseText">The text to parse.</param>
        /// <param name="GetServiceAuthorisationResponse">The parsed GetServiceAuthorisation response.</param>
        /// <param name="CustomSendGetServiceAuthorisationResponseParser">An optional delegate to parse custom GetServiceAuthorisationResponse XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetServiceAuthorisationRequest                            Request,
                                       String                                                    GetServiceAuthorisationResponseText,
                                       out GetServiceAuthorisationResponse                       GetServiceAuthorisationResponse,
                                       CustomXMLParserDelegate<GetServiceAuthorisationResponse>  CustomSendGetServiceAuthorisationResponseParser  = null,
                                       CustomXMLParserDelegate<MeterReport>                      CustomMeterReportParser                          = null,
                                       HTTPResponse                                              HTTPResponse                                     = null,
                                       OnExceptionDelegate                                       OnException                                      = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(GetServiceAuthorisationResponseText).Root,
                             out GetServiceAuthorisationResponse,
                             CustomSendGetServiceAuthorisationResponseParser,
                             CustomMeterReportParser,
                             HTTPResponse,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, GetServiceAuthorisationResponseText, e);
            }

            GetServiceAuthorisationResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomGetServiceAuthorisationResponseSerializer = null, CustomMeterReportSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomGetServiceAuthorisationResponseSerializer">A delegate to serialize custom Heartbeat response XML elements.</param>
        /// <param name="CustomMeterReportSerializer">A delegate to serialize custom MeterReport XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<GetServiceAuthorisationResponse> CustomGetServiceAuthorisationResponseSerializer   = null,
                              CustomXMLSerializerDelegate<MeterReport>                     CustomMeterReportSerializer                       = null)
        {

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_ToIOP_GetServiceAuthorisationResponse",

                          new XElement("transactionId",                      TransactionId.                      ToString()),

                          SalesPartnerOperatorId.HasValue
                              ? new XElement("salePartnerOperatorIdType",    SalesPartnerOperatorId.Value.Format.AsText())
                              : null,

                          SalesPartnerOperatorId.HasValue
                              ? new XElement("salePartnerOperatorId",        SalesPartnerOperatorId.       Value.ToString())
                              : null,

                          new XElement("authorisationValue",                 AuthorisationValue.                 AsNumber()),
                          new XElement("serviceSessionId",                   ServiceSessionId.                   ToString()),
                          new XElement("intermediateCDRRequested",           IntermediateCDRRequested ? "1" : "0"),

                          UserContractIdAlias.HasValue
                              ? new XElement("userContractIdAlias",          UserContractIdAlias.          Value.ToString())
                              : null,

                          MeterLimits.Any()
                              ? new XElement("meterLimitList",
                                    MeterLimits.Select(meterreport => meterreport.ToXML(CustomMeterReportSerializer: CustomMeterReportSerializer))
                                )
                              : null,

                          new XElement("parameter",                          Parameter),

                          new XElement("requestStatus",                      RequestStatus.ToString())

                      );


            return CustomGetServiceAuthorisationResponseSerializer != null
                       ? CustomGetServiceAuthorisationResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region ToJSON(CustomGetServiceAuthorisationResponseSerializer = null, CustomMeterReportSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomGetServiceAuthorisationResponseSerializer">A delegate to serialize custom Heartbeat response JSON objects.</param>
        /// <param name="CustomMeterReportSerializer">A delegate to serialize custom MeterReport JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<GetServiceAuthorisationResponse> CustomGetServiceAuthorisationResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<MeterReport>                     CustomMeterReportSerializer                       = null)
        {

            var JSON = JSONObject.Create(

                          new JProperty("transactionId",                      TransactionId.                      ToString()),

                          SalesPartnerOperatorId.HasValue
                              ? new JProperty("salePartnerOperatorIdType",    SalesPartnerOperatorId.Value.Format.AsText())
                              : null,

                          SalesPartnerOperatorId.HasValue
                              ? new JProperty("salePartnerOperatorId",        SalesPartnerOperatorId.       Value.ToString())
                              : null,

                          new JProperty("authorisationValue",                 AuthorisationValue.                 AsNumber()),
                          new JProperty("serviceSessionId",                   ServiceSessionId.                   ToString()),
                          new JProperty("intermediateCDRRequested",           IntermediateCDRRequested ? "1" : "0"),

                          UserContractIdAlias.HasValue
                              ? new JProperty("userContractIdAlias",          UserContractIdAlias.          Value.ToString())
                              : null,

                          MeterLimits.Any()
                              ? new JProperty("meterLimitList",
                                    MeterLimits.Select(meterreport => meterreport.ToJSON(CustomMeterReportSerializer: CustomMeterReportSerializer))
                                )
                              : null,

                          new JProperty("parameter",                          Parameter),

                          new JProperty("requestStatus",                      RequestStatus.ToString())

                      );


            return CustomGetServiceAuthorisationResponseSerializer != null
                       ? CustomGetServiceAuthorisationResponseSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (GetServiceAuthorisationResponse1, GetServiceAuthorisationResponse2)

        /// <summary>
        /// Compares two GetServiceAuthorisation responses for equality.
        /// </summary>
        /// <param name="GetServiceAuthorisationResponse1">A GetServiceAuthorisation response.</param>
        /// <param name="GetServiceAuthorisationResponse2">Another GetServiceAuthorisation response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetServiceAuthorisationResponse GetServiceAuthorisationResponse1, GetServiceAuthorisationResponse GetServiceAuthorisationResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetServiceAuthorisationResponse1, GetServiceAuthorisationResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetServiceAuthorisationResponse1 == null) || ((Object) GetServiceAuthorisationResponse2 == null))
                return false;

            return GetServiceAuthorisationResponse1.Equals(GetServiceAuthorisationResponse2);

        }

        #endregion

        #region Operator != (GetServiceAuthorisationResponse1, GetServiceAuthorisationResponse2)

        /// <summary>
        /// Compares two GetServiceAuthorisation responses for inequality.
        /// </summary>
        /// <param name="GetServiceAuthorisationResponse1">A GetServiceAuthorisation response.</param>
        /// <param name="GetServiceAuthorisationResponse2">Another GetServiceAuthorisation response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetServiceAuthorisationResponse GetServiceAuthorisationResponse1, GetServiceAuthorisationResponse GetServiceAuthorisationResponse2)
            => !(GetServiceAuthorisationResponse1 == GetServiceAuthorisationResponse2);

        #endregion

        #endregion

        #region IEquatable<GetServiceAuthorisationResponse> Members

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

            var GetServiceAuthorisationResponse = Object as GetServiceAuthorisationResponse;
            if ((Object) GetServiceAuthorisationResponse == null)
                return false;

            return Equals(GetServiceAuthorisationResponse);

        }

        #endregion

        #region Equals(GetServiceAuthorisationResponse)

        /// <summary>
        /// Compares two GetServiceAuthorisation responses for equality.
        /// </summary>
        /// <param name="GetServiceAuthorisationResponse">A GetServiceAuthorisation response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetServiceAuthorisationResponse GetServiceAuthorisationResponse)
        {

            if ((Object) GetServiceAuthorisationResponse == null)
                return false;

            return TransactionId.           Equals(GetServiceAuthorisationResponse.TransactionId)            &&
                   AuthorisationValue.      Equals(GetServiceAuthorisationResponse.AuthorisationValue)       &&
                   ServiceSessionId.        Equals(GetServiceAuthorisationResponse.ServiceSessionId)         &&
                   IntermediateCDRRequested.Equals(GetServiceAuthorisationResponse.IntermediateCDRRequested) &&
                   RequestStatus.           Equals(GetServiceAuthorisationResponse.RequestStatus)            &&

                   ((!SalesPartnerOperatorId.HasValue && !GetServiceAuthorisationResponse.SalesPartnerOperatorId.HasValue) ||
                     (SalesPartnerOperatorId.HasValue &&  GetServiceAuthorisationResponse.SalesPartnerOperatorId.HasValue && SalesPartnerOperatorId.Value.Equals(GetServiceAuthorisationResponse.SalesPartnerOperatorId.Value))) &&

                   ((!UserContractIdAlias.       HasValue && !GetServiceAuthorisationResponse.UserContractIdAlias.       HasValue) ||
                     (UserContractIdAlias.       HasValue &&  GetServiceAuthorisationResponse.UserContractIdAlias.       HasValue && UserContractIdAlias.       Value.Equals(GetServiceAuthorisationResponse.UserContractIdAlias.Value)));

                   // ToDo: Compare MeterLimitLists!

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

                return TransactionId.                    GetHashCode() * 19 ^
                       AuthorisationValue.               GetHashCode() * 17 ^
                       ServiceSessionId.                 GetHashCode() * 13 ^
                       IntermediateCDRRequested.         GetHashCode() * 11 ^
                       RequestStatus.                    GetHashCode() *  7 ^

                       (SalesPartnerOperatorId.HasValue
                            ? SalesPartnerOperatorId.    GetHashCode() *  5
                            : 0) ^

                       (UserContractIdAlias.HasValue
                            ? UserContractIdAlias.       GetHashCode() *  3
                            : 0) ^

                       // ToDo: Add MeterLimits.GetHashCode()!

                       (Parameter.IsNotNullOrEmpty()
                            ? Parameter.                 GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(TransactionId, ", ",
                             RequestStatus, " -> ",
                             AuthorisationValue);

        #endregion


        #region ToBuilder

        /// <summary>
        /// Return a response builder.
        /// </summary>
        public Builder ToBuilder
            => new Builder(this);

        #endregion

        #region (class) Builder

        /// <summary>
        /// A GetServiceAuthorisation response builder.
        /// </summary>
        public new class Builder : AResponseBuilder<GetServiceAuthorisationRequest,
                                                    GetServiceAuthorisationResponse>
        {

            #region Properties

            /// <summary>
            /// The transaction identification.
            /// </summary>
            public Transaction_Id            TransactionId                 { get; set; }

            /// <summary>
            /// The result of the authorisation.
            /// </summary>
            public AuthorisationValues       AuthorisationValue            { get; set; }

            /// <summary>
            /// The GIREVE session id for this service session.
            /// </summary>
            public ServiceSession_Id         ServiceSessionId              { get; set; }

            /// <summary>
            /// Whether the eMSP wishes to receive intermediate charging session records.
            /// </summary>
            public Boolean                   IntermediateCDRRequested      { get; set; }

            /// <summary>
            /// The status of the request.
            /// </summary>
            public RequestStatus             RequestStatus                 { get; set; }

            /// <summary>
            /// The optional sales operator identification.
            /// </summary>
            public Provider_Id?              SalesPartnerOperatorId        { get; set; }

            /// <summary>
            /// An optional alias of the contract id between the end-user and the eMSP.
            /// This alias may have been anonymised by the eMSP.
            /// </summary>
            public Contract_Id?              UserContractIdAlias           { get; set; }

            /// <summary>
            /// An optional meter limits for this authorisation:
            /// The eMSP can authorise the charge but for less than x kWh or y minutes, or z euros.
            /// </summary>
            public IEnumerable<MeterReport>  MeterLimits                   { get; set; }

            /// <summary>
            /// Optional information from the CPO to the eMSP.
            /// </summary>
            public String                    Parameter                     { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,                         CustomData = null)

            /// <summary>
            /// Create a new GetServiceAuthorisation response builder.
            /// </summary>
            /// <param name="Request">A GetServiceAuthorisation request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(GetServiceAuthorisationRequest       Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(GetServiceAuthorisationResponse, CustomData = null)

            /// <summary>
            /// Create a new GetServiceAuthorisation response builder.
            /// </summary>
            /// <param name="GetServiceAuthorisationResponse">A GetServiceAuthorisation response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(GetServiceAuthorisationResponse      GetServiceAuthorisationResponse   = null,
                           IReadOnlyDictionary<String, Object>  CustomData                        = null)

                : base(GetServiceAuthorisationResponse?.Request,
                       GetServiceAuthorisationResponse.HasInternalData
                           ? CustomData?.Count > 0
                                 ? GetServiceAuthorisationResponse.InternalData.Concat(CustomData)
                                 : GetServiceAuthorisationResponse.InternalData
                           : CustomData)

            {

                if (GetServiceAuthorisationResponse != null)
                {

                    this.TransactionId              = GetServiceAuthorisationResponse.TransactionId;

                    this.TransactionId              = GetServiceAuthorisationResponse.TransactionId;
                    this.AuthorisationValue         = GetServiceAuthorisationResponse.AuthorisationValue;
                    this.ServiceSessionId           = GetServiceAuthorisationResponse.ServiceSessionId;
                    this.IntermediateCDRRequested   = GetServiceAuthorisationResponse.IntermediateCDRRequested;
                    this.RequestStatus              = GetServiceAuthorisationResponse.RequestStatus;

                    this.SalesPartnerOperatorId     = GetServiceAuthorisationResponse.SalesPartnerOperatorId;
                    this.UserContractIdAlias        = GetServiceAuthorisationResponse.UserContractIdAlias;
                    this.MeterLimits                = GetServiceAuthorisationResponse.MeterLimits;
                    this.Parameter                  = GetServiceAuthorisationResponse.Parameter;

                    this.RequestStatus              = GetServiceAuthorisationResponse.RequestStatus;

                }

            }

            #endregion

            #endregion


            #region Equals(GetServiceAuthorisationResponse)

            /// <summary>
            /// Compares two GetServiceAuthorisation responses for equality.
            /// </summary>
            /// <param name="GetServiceAuthorisationResponse">A GetServiceAuthorisation response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(GetServiceAuthorisationResponse GetServiceAuthorisationResponse)
            {

                if ((Object) GetServiceAuthorisationResponse == null)
                    return false;

                return TransactionId.           Equals(GetServiceAuthorisationResponse.TransactionId)            &&
                       AuthorisationValue.      Equals(GetServiceAuthorisationResponse.AuthorisationValue)       &&
                       ServiceSessionId.        Equals(GetServiceAuthorisationResponse.ServiceSessionId)         &&
                       IntermediateCDRRequested.Equals(GetServiceAuthorisationResponse.IntermediateCDRRequested) &&
                       RequestStatus.           Equals(GetServiceAuthorisationResponse.RequestStatus)            &&

                       ((!SalesPartnerOperatorId.HasValue && !GetServiceAuthorisationResponse.SalesPartnerOperatorId.HasValue) ||
                         (SalesPartnerOperatorId.HasValue &&  GetServiceAuthorisationResponse.SalesPartnerOperatorId.HasValue && SalesPartnerOperatorId.Value.Equals(GetServiceAuthorisationResponse.SalesPartnerOperatorId.Value))) &&

                       ((!UserContractIdAlias.       HasValue && !GetServiceAuthorisationResponse.UserContractIdAlias.       HasValue) ||
                         (UserContractIdAlias.       HasValue &&  GetServiceAuthorisationResponse.UserContractIdAlias.       HasValue && UserContractIdAlias.       Value.Equals(GetServiceAuthorisationResponse.UserContractIdAlias.Value)));

                       // ToDo: Compare MeterLimitLists!

            }

            #endregion

            #region (implicit) "ToImmutable()"

            /// <summary>
            /// Return an immutable GetServiceAuthorisation response.
            /// </summary>
            /// <param name="Builder">A GetServiceAuthorisation response builder.</param>
            public static implicit operator GetServiceAuthorisationResponse(Builder Builder)

                => new GetServiceAuthorisationResponse(Builder.Request,
                                                       Builder.TransactionId,
                                                       Builder.AuthorisationValue,
                                                       Builder.ServiceSessionId,
                                                       Builder.IntermediateCDRRequested,
                                                       Builder.RequestStatus,

                                                       Builder.SalesPartnerOperatorId,
                                                       Builder.UserContractIdAlias,
                                                       Builder.MeterLimits,
                                                       Builder.Parameter);

            #endregion

        }

        #endregion

    }

}
