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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// A charge detail record.
    /// </summary>
    public class ChargeDetailRecord : AInternalData,
                                      IEquatable<ChargeDetailRecord>,
                                      IComparable<ChargeDetailRecord>,
                                      IComparable
    {

        #region Properties

        /// <summary>
        /// State of the charging session.
        /// </summary>
        public CDRNatures                CDRNature                { get; }

        /// <summary>
        /// GIREVE session id for this charging session.
        /// </summary>
        public ServiceSession_Id         ServiceSessionId         { get; }

        /// <summary>
        /// The unique identification of the requested service for this charging session.
        /// </summary>
        public Service_Id                RequestedServiceId       { get; }

        /// <summary>
        /// The unique identification of the EVSE used for charging.
        /// </summary>
        public EVSE_Id                   EVSEId                   { get; }

        /// <summary>
        /// Alias of the contract id between the end-user and the eMSP.
        /// This alias may have been anonymised by the eMSP.
        /// </summary>
        public Contract_Id?              UserContractIdAlias      { get; }

        /// <summary>
        /// The unique identification of the user.
        /// </summary>
        public User_Id                   UserId                   { get; }

        /// <summary>
        /// Start time of the charging session.
        /// </summary>
        public DateTime                  StartTime                { get; }

        /// <summary>
        /// End time of the charging session, or the timestamp of the meter reading for intermediate charge detail records.
        /// </summary>
        public DateTime                  EndTime                  { get; }


        /// <summary>
        /// Charging session identification at the operator.
        /// </summary>
        public ServiceSession_Id?        ExecPartnerSessionId     { get; }

        /// <summary>
        /// The unique identification of the charging operator.
        /// </summary>
        public Operator_Id?              ExecPartnerOperatorId    { get; }

        /// <summary>
        /// Charging session identification at the e-mobility provider.
        /// </summary>
        public ServiceSession_Id?        SalesPartnerSessionId    { get; }

        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        public Provider_Id?              SalesPartnerOperatorId   { get; }

        /// <summary>
        /// The unique identification of the charging product.
        /// </summary>
        public PartnerProduct_Id?        PartnerProductId         { get; }

        /// <summary>
        /// An optional enumeration of meter reports.
        /// </summary>
        public IEnumerable<MeterReport>  MeterReports             { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charge detail record.
        /// </summary>
        /// <param name="CDRNature">State of the charging session.</param>
        /// <param name="ServiceSessionId">GIREVE session id for this charging session.</param>
        /// <param name="RequestedServiceId">The unique identification of the requested service for this charging session.</param>
        /// <param name="EVSEId">The unique identification of the EVSE used for charging.</param>
        /// <param name="UserContractIdAlias">Alias of the contract id between the end-user and the eMSP. This alias may have been anonymised by the eMSP.</param>
        /// <param name="UserId">The unique identification of the user.</param>
        /// <param name="StartTime">Start time of the charging session.</param>
        /// <param name="EndTime">End time of the charging session, or the timestamp of the meter reading for intermediate charge detail records.</param>
        /// 
        /// <param name="ExecPartnerSessionId">Charging session identification at the operator.</param>
        /// <param name="ExecPartnerOperatorId">The unique identification of the charging operator.</param>
        /// <param name="SalesPartnerSessionId">Charging session identification at the e-mobility provider.</param>
        /// <param name="SalesPartnerOperatorId">The unique identification of the e-mobility provider.</param>
        /// <param name="PartnerProductId">The unique identification of the charging product.</param>
        /// <param name="MeterReports">An optional enumeration of meter reports.</param>
        /// 
        /// <param name="InternalData">An optional dictionary of customer-specific data.</param>
        public ChargeDetailRecord(CDRNatures                 CDRNature,
                                  ServiceSession_Id          ServiceSessionId,
                                  Service_Id                 RequestedServiceId,
                                  EVSE_Id                    EVSEId,
                                  User_Id                    UserId,
                                  DateTime                   StartTime,
                                  DateTime                   EndTime,

                                  Contract_Id?               UserContractIdAlias      = null,
                                  ServiceSession_Id?         ExecPartnerSessionId     = null,
                                  Operator_Id?               ExecPartnerOperatorId    = null,
                                  ServiceSession_Id?         SalesPartnerSessionId    = null,
                                  Provider_Id?               SalesPartnerOperatorId   = null,
                                  PartnerProduct_Id?         PartnerProductId         = null,
                                  IEnumerable<MeterReport>?  MeterReports             = null,

                                  JObject?                   CustomData               = null,
                                  UserDefinedDictionary?     InternalData             = null)

            : base(CustomData,
                   InternalData)

        {

            this.CDRNature               = CDRNature;
            this.ServiceSessionId        = ServiceSessionId;
            this.RequestedServiceId      = RequestedServiceId;
            this.EVSEId                  = EVSEId;
            this.UserContractIdAlias     = UserContractIdAlias;
            this.UserId                  = UserId;
            this.StartTime               = StartTime;
            this.EndTime                 = EndTime;

            this.ExecPartnerSessionId    = ExecPartnerSessionId;
            this.ExecPartnerOperatorId   = ExecPartnerOperatorId;
            this.SalesPartnerSessionId   = SalesPartnerSessionId;
            this.SalesPartnerOperatorId  = SalesPartnerOperatorId;
            this.PartnerProductId        = PartnerProductId;
            this.MeterReports            = MeterReports ?? Array.Empty<MeterReport>();

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //
        // [...]
        //
        // <chargeDetailRecord>
        //
        //    <CDRNature>?</CDRNature>
        //
        //    <serviceSessionId>?</serviceSessionId>
        //
        //    <!--Optional:-->
        //    <execPartnerSessionId>?</execPartnerSessionId>
        //
        //    <!--Optional:-->
        //    <execPartnerOperatorIdType>?</execPartnerOperatorIdType>
        //
        //    <!--Optional:-->
        //    <execPartnerOperatorId>?</execPartnerOperatorId>
        //
        //    <!--Optional:-->
        //    <salePartnerSessionId>?</salePartnerSessionId>
        //
        //    <!--Optional:-->
        //    <salePartnerOperatorIdType>?</salePartnerOperatorIdType>
        //
        //    <!--Optional:-->
        //    <salePartnerOperatorId>?</salePartnerOperatorId>
        //
        //    <requestedServiceId>?</requestedServiceId>
        //
        //    <EVSEIdType>?</EVSEIdType>
        //    <EVSEId>?</EVSEId>
        //
        //    <!--Optional:-->
        //    <userContractIdAlias>?</userContractIdAlias>
        //
        //    <userIdType>?</userIdType>
        //    <userId>?</userId>
        //
        //    <!--Optional:-->
        //    <partnerProductId>?</partnerProductId>
        //
        //    <startTime>?</startTime>
        //    <endTime>?</endTime>
        //
        //    <meterReportList>
        //       <!--Zero or more repetitions:-->
        //       <meterReport>
        //          <meterTypeId>?</meterTypeId>
        //          <meterValue>?</meterValue>
        //          <meterUnit>?</meterUnit>
        //       </meterReport>
        //    </meterReportList>
        //
        // </chargeDetailRecord>
        //
        // [...]
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (ChargeDetailRecordXML,                          ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordXML">The XML to parse.</param>
        /// <param name="CustomChargeDetailRecordParser">An optional delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargeDetailRecord Parse(XElement                                     ChargeDetailRecordXML,
                                               CustomXMLParserDelegate<ChargeDetailRecord>  CustomChargeDetailRecordParser   = null,
                                               CustomXMLParserDelegate<MeterReport>         CustomMeterReportParser          = null,
                                               OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(ChargeDetailRecordXML,
                         out ChargeDetailRecord _ChargeDetailRecord,
                         CustomChargeDetailRecordParser,
                         CustomMeterReportParser,
                         OnException))
            {
                return _ChargeDetailRecord;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (ChargeDetailRecordText,                         ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordText">The text to parse.</param>
        /// <param name="CustomChargeDetailRecordParser">An optional delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargeDetailRecord Parse(String                                       ChargeDetailRecordText,
                                               CustomXMLParserDelegate<ChargeDetailRecord>  CustomChargeDetailRecordParser   = null,
                                               CustomXMLParserDelegate<MeterReport>         CustomMeterReportParser          = null,
                                               OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(ChargeDetailRecordText,
                         out ChargeDetailRecord _ChargeDetailRecord,
                         CustomChargeDetailRecordParser,
                         CustomMeterReportParser,
                         OnException))
            {
                return _ChargeDetailRecord;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(ChargeDetailRecordXML,  out ChargeDetailRecord, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OIOI ChargeDetailRecord.
        /// </summary>
        /// <param name="ChargeDetailRecordXML">The XML to parse.</param>
        /// <param name="ChargeDetailRecord">The parsed charge detail record.</param>
        /// <param name="CustomChargeDetailRecordParser">An optional delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                     ChargeDetailRecordXML,
                                       out ChargeDetailRecord                       ChargeDetailRecord,
                                       CustomXMLParserDelegate<ChargeDetailRecord>  CustomChargeDetailRecordParser   = null,
                                       CustomXMLParserDelegate<MeterReport>         CustomMeterReportParser          = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            try
            {

                ChargeDetailRecord = new ChargeDetailRecord(

                                         ChargeDetailRecordXML.MapValueOrFail     ("CDRNature",              ConversionMethods.AsCDRNature),
                                         ChargeDetailRecordXML.MapValueOrFail     ("serviceSessionId",       ServiceSession_Id.Parse),
                                         ChargeDetailRecordXML.MapValueOrFail     ("requestedServiceId",     Service_Id.       Parse),
                                         ChargeDetailRecordXML.MapValueOrFail     ("EVSEId",                 EVSE_Id.          Parse),

                                         ChargeDetailRecordXML.MapValueOrFail     ("userId",                 s => User_Id.Parse(s,
                                             ChargeDetailRecordXML.MapValueOrFail("userIdType",              UserIdFormatsExtensions.Parse))),

                                         ChargeDetailRecordXML.MapValueOrFail     ("startTime",              DateTime.         Parse),
                                         ChargeDetailRecordXML.MapValueOrFail     ("endTime",                DateTime.         Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable ("userContractIdAlias",    Contract_Id.      Parse),
                                         ChargeDetailRecordXML.MapValueOrNullable ("execPartnerSessionId",   ServiceSession_Id.Parse),
                                         ChargeDetailRecordXML.MapValueOrNullable ("execPartnerOperatorId",  Operator_Id.      Parse),
                                         ChargeDetailRecordXML.MapValueOrNullable ("salePartnerSessionId",   ServiceSession_Id.Parse),
                                         ChargeDetailRecordXML.MapValueOrNullable ("salePartnerOperatorId",  Provider_Id.      Parse),
                                         ChargeDetailRecordXML.MapValueOrNullable ("partnerProductId",       PartnerProduct_Id.Parse),

                                         ChargeDetailRecordXML.MapElements        ("meterReportList",
                                                                                   "meterReport",
                                                                                   (s, e) => MeterReport.Parse(s, CustomMeterReportParser, e),
                                                                                   OnException)

                                     );

                if (CustomChargeDetailRecordParser != null)
                    ChargeDetailRecord = CustomChargeDetailRecordParser(ChargeDetailRecordXML,
                                                  ChargeDetailRecord);

                return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow,
                                    ChargeDetailRecordXML,
                                    e);
            }

            ChargeDetailRecord = null;
            return false;

        }

        #endregion

        #region (static) TryParse(ChargeDetailRecordText, out ChargeDetailRecord, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OIOI charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordText">The text to parse.</param>
        /// <param name="ChargeDetailRecord">The parsed charge detail record.</param>
        /// <param name="CustomChargeDetailRecordParser">An optional delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                       ChargeDetailRecordText,
                                       out ChargeDetailRecord                       ChargeDetailRecord,
                                       CustomXMLParserDelegate<ChargeDetailRecord>  CustomChargeDetailRecordParser   = null,
                                       CustomXMLParserDelegate<MeterReport>         CustomMeterReportParser          = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            try
            {

                return TryParse(XElement.Parse(ChargeDetailRecordText),
                                out ChargeDetailRecord,
                                CustomChargeDetailRecordParser,
                                CustomMeterReportParser,
                                OnException);

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, ChargeDetailRecordText, e);

                ChargeDetailRecord = null;
                return false;

            }

        }

        #endregion

        #region ToXML(XName = null, CustomChargeDetailRecordSerializer = null, CustomMeterReportSerializer = null)

        /// <summary>
        /// Return a XML representation of this EVSE data record.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A delegate to serialize custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomMeterReportSerializer">A delegate to serialize custom MeterReport XML elements.</param>
        public XElement ToXML(XName                                            XName                                = null,
                              CustomXMLSerializerDelegate<ChargeDetailRecord>  CustomChargeDetailRecordSerializer   = null,
                              CustomXMLSerializerDelegate<MeterReport>         CustomMeterReportSerializer          = null)
        {

            var XML = new XElement(XName ?? "chargeDetailRecord",

                          new XElement("CDRNature",                          CDRNature.                           AsText()),
                          new XElement("serviceSessionId",                   ServiceSessionId.                  ToString()),

                          ExecPartnerSessionId.HasValue
                              ? new XElement("execPartnerSessionId",         ExecPartnerSessionId.        Value.ToString())
                              : null,

                          ExecPartnerOperatorId.HasValue
                              ? new XElement("execPartnerOperatorIdType",    ExecPartnerOperatorId.Value.Format.AsText())
                              : null,

                          ExecPartnerOperatorId.HasValue
                              ? new XElement("execPartnerOperatorId",        ExecPartnerOperatorId.       Value.ToString())
                              : null,


                          SalesPartnerSessionId.HasValue
                              ? new XElement("execPartnerSessionId",         ExecPartnerSessionId.        Value.ToString())
                              : null,

                          SalesPartnerOperatorId.HasValue
                              ? new XElement("execPartnerOperatorIdType",    ExecPartnerOperatorId.Value.Format.AsText())
                              : null,

                          SalesPartnerOperatorId.HasValue
                              ? new XElement("execPartnerOperatorId",        ExecPartnerOperatorId.       Value.ToString())
                              : null,


                          new XElement("requestedServiceId",                 RequestedServiceId.                ToString()),

                          new XElement("EVSEIdType",                         EVSEId.Format.                     AsText()),
                          new XElement("EVSEId",                             EVSEId.                            ToString()),

                          new XElement("userContractIdAlias",                UserContractIdAlias.               ToString()),

                          new XElement("userIdType",                         UserId.Format.                     AsText()),
                          new XElement("userId",                             UserId.                            ToString()),

                          PartnerProductId.HasValue
                              ? new XElement("partnerProductId",             PartnerProductId.            Value.ToString())
                              : null,

                          new XElement("startTime",                          StartTime.                         ToIso8601(false)),
                          new XElement("endTime",                            EndTime.                           ToIso8601(false)),

                          new XElement("meterReportList",
                              MeterReports.Any()
                                  ? MeterReports.Select(meterreport => meterreport.ToXML(CustomMeterReportSerializer: CustomMeterReportSerializer))
                                  : null
                          )

                      );

            return CustomChargeDetailRecordSerializer != null
                       ? CustomChargeDetailRecordSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two charge detail recordes for equality.
        /// </summary>
        /// <param name="ChargeDetailRecord1">An charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargeDetailRecord1, ChargeDetailRecord2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargeDetailRecord1 == null) || ((Object) ChargeDetailRecord2 == null))
                return false;

            return ChargeDetailRecord1.Equals(ChargeDetailRecord2);

        }

        #endregion

        #region Operator != (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two charge detail recordes for inequality.
        /// </summary>
        /// <param name="ChargeDetailRecord1">An charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)

            => !(ChargeDetailRecord1 == ChargeDetailRecord2);

        #endregion

        #region Operator <  (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">An charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)
        {

            if ((Object) ChargeDetailRecord1 == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord1), "The given ChargeDetailRecord1 must not be null!");

            return ChargeDetailRecord1.CompareTo(ChargeDetailRecord2) < 0;

        }

        #endregion

        #region Operator <= (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">An charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)
            => !(ChargeDetailRecord1 > ChargeDetailRecord2);

        #endregion

        #region Operator >  (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">An charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)
        {

            if ((Object)ChargeDetailRecord1 == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord1), "The given ChargeDetailRecord1 must not be null!");

            return ChargeDetailRecord1.CompareTo(ChargeDetailRecord2) > 0;

        }

        #endregion

        #region Operator >= (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">An charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)
            => !(ChargeDetailRecord1 < ChargeDetailRecord2);

        #endregion

        #endregion

        #region IComparable<ChargeDetailRecord> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var ChargeDetailRecord = Object as ChargeDetailRecord;
            if ((Object) ChargeDetailRecord == null)
                throw new ArgumentException("The given object is not a charge detail record identification!", nameof(Object));

            return CompareTo(ChargeDetailRecord);

        }

        #endregion

        #region CompareTo(ChargeDetailRecord)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord">An object to compare with.</param>
        public Int32 CompareTo(ChargeDetailRecord ChargeDetailRecord)
        {

            if ((Object) ChargeDetailRecord == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord), "The given charge detail record must not be null!");

            return ServiceSessionId.CompareTo(ChargeDetailRecord.ServiceSessionId);

        }

        #endregion

        #endregion

        #region IEquatable<ChargeDetailRecord> Members

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

            var ChargeDetailRecord = Object as ChargeDetailRecord;
            if ((Object) ChargeDetailRecord == null)
                return false;

            return Equals(ChargeDetailRecord);

        }

        #endregion

        #region Equals(ChargeDetailRecord)

        /// <summary>
        /// Compares two charge detail recordes for equality.
        /// </summary>
        /// <param name="ChargeDetailRecord">An charge detail record to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargeDetailRecord ChargeDetailRecord)
        {

            if ((Object) ChargeDetailRecord == null)
                return false;

            return CDRNature.          Equals(ChargeDetailRecord.CDRNature)           &&
                   ServiceSessionId.   Equals(ChargeDetailRecord.ServiceSessionId)    &&
                   RequestedServiceId. Equals(ChargeDetailRecord.RequestedServiceId)  &&
                   EVSEId.             Equals(ChargeDetailRecord.EVSEId)              &&
                   UserContractIdAlias.Equals(ChargeDetailRecord.UserContractIdAlias) &&
                   UserId.             Equals(ChargeDetailRecord.UserId)              &&
                   StartTime.          Equals(ChargeDetailRecord.StartTime)           &&
                   EndTime.            Equals(ChargeDetailRecord.EndTime)             &&
                   MeterReports.       Equals(ChargeDetailRecord.MeterReports)        &&

                   ((!ExecPartnerSessionId.  HasValue && !ChargeDetailRecord.ExecPartnerSessionId.  HasValue) ||
                     (ExecPartnerSessionId.  HasValue &&  ChargeDetailRecord.ExecPartnerSessionId.  HasValue && ExecPartnerSessionId.  Value.Equals(ChargeDetailRecord.ExecPartnerSessionId.  Value))) &&

                   ((!ExecPartnerOperatorId. HasValue && !ChargeDetailRecord.ExecPartnerOperatorId. HasValue) ||
                     (ExecPartnerOperatorId. HasValue &&  ChargeDetailRecord.ExecPartnerOperatorId. HasValue && ExecPartnerOperatorId. Value.Equals(ChargeDetailRecord.ExecPartnerOperatorId. Value))) &&

                   ((!SalesPartnerSessionId. HasValue && !ChargeDetailRecord.SalesPartnerSessionId. HasValue) ||
                     (SalesPartnerSessionId. HasValue &&  ChargeDetailRecord.SalesPartnerSessionId. HasValue && SalesPartnerSessionId. Value.Equals(ChargeDetailRecord.SalesPartnerSessionId. Value))) &&

                   ((!SalesPartnerOperatorId.HasValue && !ChargeDetailRecord.SalesPartnerOperatorId.HasValue) ||
                     (SalesPartnerOperatorId.HasValue &&  ChargeDetailRecord.SalesPartnerOperatorId.HasValue && SalesPartnerOperatorId.Value.Equals(ChargeDetailRecord.SalesPartnerOperatorId.Value))) &&

                   ((!PartnerProductId.      HasValue && !ChargeDetailRecord.PartnerProductId.      HasValue) ||
                     (PartnerProductId.      HasValue &&  ChargeDetailRecord.PartnerProductId.      HasValue && PartnerProductId.      Value.Equals(ChargeDetailRecord.PartnerProductId.      Value)));

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

                return CDRNature.                    GetHashCode() * 41 ^
                       ServiceSessionId.             GetHashCode() * 37 ^
                       RequestedServiceId.           GetHashCode() * 31 ^
                       EVSEId.                       GetHashCode() * 27 ^
                       UserContractIdAlias.          GetHashCode() * 23 ^
                       UserId.                       GetHashCode() * 21 ^
                       StartTime.                    GetHashCode() * 19 ^
                       EndTime.                      GetHashCode() * 17 ^
                       MeterReports.                 GetHashCode() * 13 ^

                       (ExecPartnerSessionId.HasValue
                            ? ExecPartnerSessionId.  GetHashCode() * 11
                            : 0) ^

                       (ExecPartnerOperatorId.HasValue
                            ? ExecPartnerOperatorId. GetHashCode() * 7
                            : 0) ^

                       (SalesPartnerSessionId.HasValue
                            ? SalesPartnerSessionId. GetHashCode() * 5
                            : 0) ^

                       (SalesPartnerOperatorId.HasValue
                            ? SalesPartnerOperatorId.GetHashCode() * 3
                            : 0) ^

                       (PartnerProductId.HasValue
                            ? PartnerProductId.      GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(UserId, " @ ", EVSEId,
                             PartnerProductId.HasValue ? " consuming '" + PartnerProductId + "'" : "",
                             " for ", (EndTime - StartTime).TotalMinutes, " minutes",
                             " [", CDRNature, "] ");

        #endregion


        #region ToBuilder

        /// <summary>
        /// Return a charge detail record builder.
        /// </summary>
        public Builder ToBuilder
            => new Builder(this);

        #endregion

        #region (class) Builder

        /// <summary>
        /// A charge detail record builder.
        /// </summary>
        public class Builder : AInternalData.Builder,
                               IEquatable<Builder>
        {

            #region Properties

            /// <summary>
            /// State of the charging session.
            /// </summary>
            public CDRNatures                CDRNature                { get; set; }

            /// <summary>
            /// GIREVE session id for this charging session.
            /// </summary>
            public ServiceSession_Id         ServiceSessionId         { get; set; }

            /// <summary>
            /// The unique identification of the requested service for this charging session.
            /// </summary>
            public Service_Id                RequestedServiceId       { get; set; }

            /// <summary>
            /// The unique identification of the EVSE used for charging.
            /// </summary>
            public EVSE_Id                   EVSEId                   { get; set; }

            /// <summary>
            /// Alias of the contract id between the end-user and the eMSP.
            /// This alias may have been anonymised by the eMSP.
            /// </summary>
            public Contract_Id?              UserContractIdAlias      { get; set; }

            /// <summary>
            /// The unique identification of the user.
            /// </summary>
            public User_Id                   UserId                   { get; set; }

            /// <summary>
            /// Start time of the charging session.
            /// </summary>
            public DateTime                  StartTime                { get; set; }

            /// <summary>
            /// End time of the charging session, or the timestamp of the meter reading for intermediate charge detail records.
            /// </summary>
            public DateTime                  EndTime                  { get; set; }


            /// <summary>
            /// Charging session identification at the operator.
            /// </summary>
            public ServiceSession_Id?        ExecPartnerSessionId     { get; set; }

            /// <summary>
            /// The unique identification of the charging operator.
            /// </summary>
            public Operator_Id?              ExecPartnerOperatorId    { get; set; }

            /// <summary>
            /// Charging session identification at the e-mobility provider.
            /// </summary>
            public ServiceSession_Id?        SalesPartnerSessionId    { get; set; }

            /// <summary>
            /// The unique identification of the e-mobility provider.
            /// </summary>
            public Provider_Id?              SalesPartnerOperatorId   { get; set; }

            /// <summary>
            /// The unique identification of the charging product.
            /// </summary>
            public PartnerProduct_Id?        PartnerProductId         { get; set; }

            /// <summary>
            /// An optional enumeration of meter reports.
            /// </summary>
            public IEnumerable<MeterReport>  MeterReports             { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new charge detail record builder.
            /// </summary>
            /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(ChargeDetailRecord?     ChargeDetailRecord   = null,
                           JObject?                CustomData           = null,
                           UserDefinedDictionary?  InternalData         = null)

                : base(CustomData,
                       InternalData)

            {

                if (ChargeDetailRecord is not null)
                {

                    this.CDRNature               = ChargeDetailRecord.CDRNature;
                    this.ServiceSessionId        = ChargeDetailRecord.ServiceSessionId;
                    this.RequestedServiceId      = ChargeDetailRecord.RequestedServiceId;
                    this.EVSEId                  = ChargeDetailRecord.EVSEId;
                    this.UserContractIdAlias     = ChargeDetailRecord.UserContractIdAlias;
                    this.UserId                  = ChargeDetailRecord.UserId;
                    this.StartTime               = ChargeDetailRecord.StartTime;
                    this.EndTime                 = ChargeDetailRecord.EndTime;

                    this.ExecPartnerSessionId    = ChargeDetailRecord.ExecPartnerSessionId;
                    this.ExecPartnerOperatorId   = ChargeDetailRecord.ExecPartnerOperatorId;
                    this.SalesPartnerSessionId   = ChargeDetailRecord.SalesPartnerSessionId;
                    this.SalesPartnerOperatorId  = ChargeDetailRecord.SalesPartnerOperatorId;
                    this.PartnerProductId        = ChargeDetailRecord.PartnerProductId;
                    this.MeterReports            = ChargeDetailRecord.MeterReports;

                }

            }

            #endregion


            #region IEquatable<ChargeDetailRecordBuilder> Members

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

                var ChargeDetailRecordBuilder = Object as Builder;
                if (ChargeDetailRecordBuilder == null)
                    return false;

                return Equals(ChargeDetailRecordBuilder);

            }

            #endregion

            #region Equals(ChargeDetailRecord)

            /// <summary>
            /// Compares two charge detail records for equality.
            /// </summary>
            /// <param name="ChargeDetailRecord">A charge detail record to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public Boolean Equals(ChargeDetailRecord ChargeDetailRecord)
            {

                if ((Object) ChargeDetailRecord == null)
                    return false;

                return CDRNature.             Equals(ChargeDetailRecord.CDRNature)              &&
                       ServiceSessionId.      Equals(ChargeDetailRecord.ServiceSessionId)       &&
                       RequestedServiceId.    Equals(ChargeDetailRecord.RequestedServiceId)     &&
                       EVSEId.                Equals(ChargeDetailRecord.EVSEId)                 &&
                       UserContractIdAlias.   Equals(ChargeDetailRecord.UserContractIdAlias)    &&
                       UserId.                Equals(ChargeDetailRecord.UserId)                 &&
                       StartTime.             Equals(ChargeDetailRecord.StartTime)              &&
                       EndTime.               Equals(ChargeDetailRecord.EndTime)                &&

                       ExecPartnerSessionId.  Equals(ChargeDetailRecord.ExecPartnerSessionId)   &&
                       ExecPartnerOperatorId. Equals(ChargeDetailRecord.ExecPartnerOperatorId)  &&
                       SalesPartnerSessionId. Equals(ChargeDetailRecord.SalesPartnerSessionId)  &&
                       SalesPartnerOperatorId.Equals(ChargeDetailRecord.SalesPartnerOperatorId) &&
                       PartnerProductId.      Equals(ChargeDetailRecord.PartnerProductId)       &&
                       MeterReports.          Equals(ChargeDetailRecord.MeterReports);

            }

            #endregion

            #region Equals(ChargeDetailRecordBuilder)

            /// <summary>
            /// Compares two charge detail record builder for equality.
            /// </summary>
            /// <param name="ChargeDetailRecordBuilder">A charge detail record builder to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public Boolean Equals(Builder ChargeDetailRecordBuilder)
            {

                if (ChargeDetailRecordBuilder == null)
                    return false;

                return CDRNature.             Equals(ChargeDetailRecordBuilder.CDRNature)              &&
                       ServiceSessionId.      Equals(ChargeDetailRecordBuilder.ServiceSessionId)       &&
                       RequestedServiceId.    Equals(ChargeDetailRecordBuilder.RequestedServiceId)     &&
                       EVSEId.                Equals(ChargeDetailRecordBuilder.EVSEId)                 &&
                       UserContractIdAlias.   Equals(ChargeDetailRecordBuilder.UserContractIdAlias)    &&
                       UserId.                Equals(ChargeDetailRecordBuilder.UserId)                 &&
                       StartTime.             Equals(ChargeDetailRecordBuilder.StartTime)              &&
                       EndTime.               Equals(ChargeDetailRecordBuilder.EndTime)                &&

                       ExecPartnerSessionId.  Equals(ChargeDetailRecordBuilder.ExecPartnerSessionId)   &&
                       ExecPartnerOperatorId. Equals(ChargeDetailRecordBuilder.ExecPartnerOperatorId)  &&
                       SalesPartnerSessionId. Equals(ChargeDetailRecordBuilder.SalesPartnerSessionId)  &&
                       SalesPartnerOperatorId.Equals(ChargeDetailRecordBuilder.SalesPartnerOperatorId) &&
                       PartnerProductId.      Equals(ChargeDetailRecordBuilder.PartnerProductId)       &&
                       MeterReports.          Equals(ChargeDetailRecordBuilder.MeterReports);

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

                    return CDRNature.                    GetHashCode() * 41 ^
                           ServiceSessionId.             GetHashCode() * 37 ^
                           RequestedServiceId.           GetHashCode() * 31 ^
                           EVSEId.                       GetHashCode() * 27 ^
                           UserContractIdAlias.          GetHashCode() * 23 ^
                           UserId.                       GetHashCode() * 21 ^
                           StartTime.                    GetHashCode() * 19 ^
                           EndTime.                      GetHashCode() * 17 ^
                           MeterReports.                 GetHashCode() * 13 ^

                           (ExecPartnerSessionId.HasValue
                                ? ExecPartnerSessionId.  GetHashCode() * 11
                                : 0) ^

                           (ExecPartnerOperatorId.HasValue
                                ? ExecPartnerOperatorId. GetHashCode() * 7
                                : 0) ^

                           (SalesPartnerSessionId.HasValue
                                ? SalesPartnerSessionId. GetHashCode() * 5
                                : 0) ^

                           (SalesPartnerOperatorId.HasValue
                                ? SalesPartnerOperatorId.GetHashCode() * 3
                                : 0) ^

                           (PartnerProductId.HasValue
                                ? PartnerProductId.      GetHashCode()
                                : 0);

                }
            }

            #endregion


            #region ToImmutable

            /// <summary>
            /// Return an immutable representation.
            /// </summary>
            public ChargeDetailRecord ToImmutable

                => new ChargeDetailRecord(CDRNature,
                                          ServiceSessionId,
                                          RequestedServiceId,
                                          EVSEId,
                                          UserId,
                                          StartTime,
                                          EndTime,

                                          UserContractIdAlias,
                                          ExecPartnerSessionId,
                                          ExecPartnerOperatorId,
                                          SalesPartnerSessionId,
                                          SalesPartnerOperatorId,
                                          PartnerProductId,
                                          MeterReports,

                                          CustomData,
                                          InternalData);

            #endregion

        }

        #endregion

    }

}
