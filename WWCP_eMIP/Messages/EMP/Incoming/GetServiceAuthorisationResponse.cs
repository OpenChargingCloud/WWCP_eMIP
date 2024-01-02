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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.EMP
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
        /// The partner session id for this service session.
        /// </summary>
        public PartnerServiceSession_Id  PartnerServiceSessionId     { get; }

        /// <summary>
        /// Whether the eMSP wishes to receive intermediate charging session records.
        /// </summary>
        public Boolean                   IntermediateCDRRequested    { get; }


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
        public String?                   Parameter                   { get; }

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
        public GetServiceAuthorisationResponse(GetServiceAuthorisationRequest  Request,
                                               Transaction_Id                  TransactionId,
                                               RequestStatus                   RequestStatus,

                                               HTTPResponse?                   HTTPResponse   = null,
                                               JObject?                        CustomData     = null,
                                               UserDefinedDictionary?          InternalData   = null)
            : this(Request,
                   TransactionId,
                   AuthorisationValues.KO,
                   PartnerServiceSession_Id.Zero,
                   false,
                   RequestStatus,
                   null,
                   null,
                   null,
                   HTTPResponse,
                   CustomData,
                   InternalData)

        { }

        #endregion

        #region GetServiceAuthorisationResponse(Request, TransactionId, ...,           CustomData = null)

        /// <summary>
        /// Create a new GetServiceAuthorisation response.
        /// </summary>
        /// <param name="Request">The GetServiceAuthorisation request leading to this response.</param>
        /// <param name="TransactionId">A transaction identification.</param>
        /// <param name="AuthorisationValue">The result of the authorisation.</param>
        /// <param name="PartnerServiceSessionId">The partner session id for this service session.</param>
        /// <param name="IntermediateCDRRequested">Whether the eMSP wishes to receive intermediate charging session records.</param>
        /// <param name="RequestStatus">The status of the request.</param>
        /// 
        /// <param name="UserContractIdAlias">An optional alias of the contract id between the end-user and the eMSP. This alias may have been anonymised by the eMSP.</param>
        /// <param name="MeterLimits">An optional meter limits for this authorisation: The eMSP can authorise the charge but for less than x kWh or y minutes, or z euros.</param>
        /// <param name="Parameter">Optional information from the CPO to the eMSP.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public GetServiceAuthorisationResponse(GetServiceAuthorisationRequest  Request,
                                               Transaction_Id                  TransactionId,
                                               AuthorisationValues             AuthorisationValue,
                                               PartnerServiceSession_Id        PartnerServiceSessionId,
                                               Boolean                         IntermediateCDRRequested,
                                               RequestStatus                   RequestStatus,

                                               Contract_Id?                    UserContractIdAlias   = null,
                                               IEnumerable<MeterReport>?       MeterLimits           = null,
                                               String?                         Parameter             = null,
                                               HTTPResponse?                   HTTPResponse          = null,
                                               JObject?                        CustomData            = null,
                                               UserDefinedDictionary?          InternalData          = null)

            : base(Request,
                   TransactionId,
                   RequestStatus,
                   HTTPResponse,
                   CustomData,
                   InternalData)

        {

            this.AuthorisationValue        = AuthorisationValue;
            this.PartnerServiceSessionId   = PartnerServiceSessionId;
            this.IntermediateCDRRequested  = IntermediateCDRRequested;

            this.UserContractIdAlias       = UserContractIdAlias;
            this.MeterLimits               = MeterLimits ?? Array.Empty<MeterReport>();
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
        //       <eMIP:eMIP_FromIOP_GetServiceAuthorisationResponse>
        //
        //          <transactionId>TRANSACTION_46151</transactionId>
        //          <authorisationValue>?</authorisationValue>
        //          <partnerServiceSessionId>?</partnerServiceSessionId>
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
        //       </eMIP:eMIP_FromIOP_GetServiceAuthorisationResponse>
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
                                                            CustomXMLParserDelegate<GetServiceAuthorisationResponse>  CustomSendGetServiceAuthorisationResponseParser   = null,
                                                            CustomXMLParserDelegate<MeterReport>                      CustomMeterReportParser                           = null,
                                                            HTTPResponse                                              HTTPResponse                                      = null,
                                                            OnExceptionDelegate                                       OnException                                       = null)
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
                                                            CustomXMLParserDelegate<GetServiceAuthorisationResponse>  CustomSendGetServiceAuthorisationResponseParser   = null,
                                                            CustomXMLParserDelegate<MeterReport>                      CustomMeterReportParser                           = null,
                                                            HTTPResponse                                              HTTPResponse                                      = null,
                                                            OnExceptionDelegate                                       OnException                                       = null)
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
                                       CustomXMLParserDelegate<GetServiceAuthorisationResponse>  CustomSendGetServiceAuthorisationResponseParser   = null,
                                       CustomXMLParserDelegate<MeterReport>                      CustomMeterReportParser                           = null,
                                       HTTPResponse                                              HTTPResponse                                      = null,
                                       OnExceptionDelegate                                       OnException                                       = null)
        {

            try
            {

                GetServiceAuthorisationResponse = new GetServiceAuthorisationResponse(

                                                      Request,

                                                      GetServiceAuthorisationResponseXML.MapValueOrFail    ("transactionId",             Transaction_Id.Parse),
                                                      GetServiceAuthorisationResponseXML.MapValueOrFail    ("authorisationValue",        ConversionMethods.AsAuthorisationValue),
                                                      GetServiceAuthorisationResponseXML.MapValueOrFail    ("partnerServiceSessionId",   PartnerServiceSession_Id.Parse),
                                                      GetServiceAuthorisationResponseXML.MapBooleanOrFail  ("intermediateCDRRequested"),

                                                      GetServiceAuthorisationResponseXML.MapValueOrFail    ("requestStatus",             RequestStatus.Parse),
                                                      GetServiceAuthorisationResponseXML.MapValueOrNullable("userContractIdAlias",       Contract_Id.Parse),

                                                      GetServiceAuthorisationResponseXML.MapElements       ("meterLimitList",
                                                                                                            "meterReport",
                                                                                                            s => MeterReport.Parse(s,
                                                                                                                                   CustomMeterReportParser,
                                                                                                                                   OnException)),

                                                      GetServiceAuthorisationResponseXML.MapValueOrNull    ("parameter"),

                                                      HTTPResponse

                                                  );


                if (CustomSendGetServiceAuthorisationResponseParser is not null)
                    GetServiceAuthorisationResponse = CustomSendGetServiceAuthorisationResponseParser(GetServiceAuthorisationResponseXML,
                                                                                                      GetServiceAuthorisationResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, GetServiceAuthorisationResponseXML, e);

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
        /// <param name="CustomSendGetServiceAuthorisationResponseParser">An optional delegate to parse custom GetServiceAuthorisationResponse XML elements.</param>
        /// <param name="GetServiceAuthorisationResponse">The parsed GetServiceAuthorisation response.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="HTTPResponse">The correlated HTTP response of this eMIP response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetServiceAuthorisationRequest                            Request,
                                       String                                                    GetServiceAuthorisationResponseText,
                                       out GetServiceAuthorisationResponse                       GetServiceAuthorisationResponse,
                                       CustomXMLParserDelegate<GetServiceAuthorisationResponse>  CustomSendGetServiceAuthorisationResponseParser   = null,
                                       CustomXMLParserDelegate<MeterReport>                      CustomMeterReportParser                           = null,
                                       HTTPResponse                                              HTTPResponse                                      = null,
                                       OnExceptionDelegate                                       OnException                                       = null)
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
                OnException?.Invoke(Timestamp.Now, GetServiceAuthorisationResponseText, e);
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

            var XML = new XElement(eMIPNS.Authorisation + "eMIP_FromIOP_GetServiceAuthorisationResponse",

                          new XElement("transactionId",              TransactionId.                      ToString()),
                          new XElement("authorisationValue",         AuthorisationValue.                 AsNumber()),
                          new XElement("partnerServiceSessionId",    PartnerServiceSessionId.            ToString()),
                          new XElement("intermediateCDRRequested",   IntermediateCDRRequested ? "1" : "0"),

                          UserContractIdAlias.HasValue
                              ? new XElement("userContractIdAlias",  UserContractIdAlias.          Value.ToString())
                              : null,

                          MeterLimits.Any()
                              ? new XElement("meterLimitList",
                                    MeterLimits.Select(meterreport => meterreport.ToXML(CustomMeterReportSerializer: CustomMeterReportSerializer))
                                )
                              : null,

                          new XElement("parameter",                  Parameter),

                          new XElement("requestStatus",              RequestStatus.ToString())

                      );


            return CustomGetServiceAuthorisationResponseSerializer is not null
                       ? CustomGetServiceAuthorisationResponseSerializer(this, XML)
                       : XML;

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
            if ((GetServiceAuthorisationResponse1 is null) || (GetServiceAuthorisationResponse2 is null))
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

            if (Object is null)
                return false;

            if (!(Object is GetServiceAuthorisationResponse))
                return false;

            return Equals(Object as GetServiceAuthorisationResponse);

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

            if (GetServiceAuthorisationResponse is null)
                return false;

            return TransactionId.           Equals(GetServiceAuthorisationResponse.TransactionId)            &&
                   AuthorisationValue.      Equals(GetServiceAuthorisationResponse.AuthorisationValue)       &&
                   PartnerServiceSessionId. Equals(GetServiceAuthorisationResponse.PartnerServiceSessionId)  &&
                   IntermediateCDRRequested.Equals(GetServiceAuthorisationResponse.IntermediateCDRRequested) &&
                   RequestStatus.           Equals(GetServiceAuthorisationResponse.RequestStatus)            &&

                   ((!UserContractIdAlias.HasValue && !GetServiceAuthorisationResponse.UserContractIdAlias.HasValue) ||
                     (UserContractIdAlias.HasValue &&  GetServiceAuthorisationResponse.UserContractIdAlias.HasValue && UserContractIdAlias.Value.Equals(GetServiceAuthorisationResponse.UserContractIdAlias.Value)));

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

                return TransactionId.                    GetHashCode() * 17 ^
                       AuthorisationValue.               GetHashCode() * 13 ^
                       PartnerServiceSessionId.          GetHashCode() * 11 ^
                       IntermediateCDRRequested.         GetHashCode() *  7 ^
                       RequestStatus.                    GetHashCode() *  5 ^

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
            /// The result of the authorisation.
            /// </summary>
            public AuthorisationValues       AuthorisationValue            { get; set; }

            /// <summary>
            /// The partner session id for this service session.
            /// </summary>
            public PartnerServiceSession_Id  PartnerServiceSessionId       { get; set; }

            /// <summary>
            /// Whether the eMSP wishes to receive intermediate charging session records.
            /// </summary>
            public Boolean                   IntermediateCDRRequested      { get; set; }

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
            public Builder(GetServiceAuthorisationRequest  Request,
                           JObject?                        CustomData     = null,
                           UserDefinedDictionary?          InternalData   = null)

                : base(Request,
                       CustomData,
                       InternalData)

            { }

            #endregion

            #region Builder(GetServiceAuthorisationResponse, CustomData = null)

            /// <summary>
            /// Create a new GetServiceAuthorisation response builder.
            /// </summary>
            /// <param name="GetServiceAuthorisationResponse">A GetServiceAuthorisation response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(GetServiceAuthorisationResponse?  GetServiceAuthorisationResponse   = null,
                           JObject?                          CustomData                        = null,
                           UserDefinedDictionary?            InternalData                      = null)

                : base(GetServiceAuthorisationResponse?.Request,
                       CustomData,
                       GetServiceAuthorisationResponse.IsNotEmpty
                           ? InternalData.IsNotEmpty
                                 ? new UserDefinedDictionary(GetServiceAuthorisationResponse.InternalData.Concat(InternalData))
                                 : new UserDefinedDictionary(GetServiceAuthorisationResponse.InternalData)
                           : InternalData)

            {

                if (GetServiceAuthorisationResponse is not null)
                {

                    this.TransactionId              = GetServiceAuthorisationResponse.TransactionId;

                    this.TransactionId              = GetServiceAuthorisationResponse.TransactionId;
                    this.AuthorisationValue         = GetServiceAuthorisationResponse.AuthorisationValue;
                    this.PartnerServiceSessionId    = GetServiceAuthorisationResponse.PartnerServiceSessionId;
                    this.IntermediateCDRRequested   = GetServiceAuthorisationResponse.IntermediateCDRRequested;
                    this.RequestStatus              = GetServiceAuthorisationResponse.RequestStatus;

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

                if (GetServiceAuthorisationResponse is null)
                    return false;

                return TransactionId.           Equals(GetServiceAuthorisationResponse.TransactionId)            &&
                       AuthorisationValue.      Equals(GetServiceAuthorisationResponse.AuthorisationValue)       &&
                       PartnerServiceSessionId. Equals(GetServiceAuthorisationResponse.PartnerServiceSessionId)  &&
                       IntermediateCDRRequested.Equals(GetServiceAuthorisationResponse.IntermediateCDRRequested) &&
                       RequestStatus.           Equals(GetServiceAuthorisationResponse.RequestStatus)            &&

                       ((!UserContractIdAlias.HasValue && !GetServiceAuthorisationResponse.UserContractIdAlias.HasValue) ||
                         (UserContractIdAlias.HasValue &&  GetServiceAuthorisationResponse.UserContractIdAlias.HasValue && UserContractIdAlias.Value.Equals(GetServiceAuthorisationResponse.UserContractIdAlias.Value)));

                       // ToDo: Compare MeterLimitLists!

            }

            #endregion

            #region (implicit) "ToImmutable()"

            /// <summary>
            /// Return an immutable GetServiceAuthorisationResponse.
            /// </summary>
            /// <param name="Builder">A GetServiceAuthorisationResponse builder.</param>
            public static implicit operator GetServiceAuthorisationResponse(Builder Builder)

                => new GetServiceAuthorisationResponse(Builder.Request,
                                                       Builder.TransactionId,
                                                       Builder.AuthorisationValue,
                                                       Builder.PartnerServiceSessionId,
                                                       Builder.IntermediateCDRRequested,
                                                       Builder.RequestStatus,

                                                       Builder.UserContractIdAlias,
                                                       Builder.MeterLimits,
                                                       Builder.Parameter);

            #endregion

        }

        #endregion

    }

}
