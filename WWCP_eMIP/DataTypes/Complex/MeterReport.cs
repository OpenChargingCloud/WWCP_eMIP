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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// A meter report.
    /// </summary>
    public class MeterReport : ACustomData,
                               IEquatable<MeterReport>,
                               IComparable<MeterReport>,
                               IComparable
    {

        #region Properties

        /// <summary>
        /// The meter value.
        /// </summary>
        public String      Value    { get; }

        /// <summary>
        /// The unit of the meter value.
        /// </summary>
        public String      Unit     { get; }

        /// <summary>
        /// The type of the meter value (energy, duration, ...).
        /// </summary>
        public MeterTypes  Type     { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new meter report.
        /// </summary>
        /// <param name="Value">The meter value.</param>
        /// <param name="Unit">The unit of the meter value.</param>
        /// <param name="Type">The type of the meter value.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public MeterReport(String                               Value,
                           String                               Unit,
                           MeterTypes                           Type,
                           IReadOnlyDictionary<String, Object>  CustomData    = null)

            : base(CustomData)

        {

            #region Initial checks

            if (Value.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Value),  "The given value must not be null or empty!");

            if (Unit. IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Unit),   "The given unit must not be null or empty!");

            #endregion

            this.Value  = Value.Trim();
            this.Unit   = Unit. Trim();
            this.Type   = Type;

        }

        #endregion


        #region (static) Create(Value, Unit, Type, ...)

        /// <summary>
        /// Create a new meter report.
        /// </summary>
        /// <param name="Value">The meter value.</param>
        /// <param name="Unit">The unit of the meter value.</param>
        /// <param name="Type">The type of the meter value (energy, duration, ...).</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public static MeterReport Create(String                               Value,
                                         String                               Unit,
                                         MeterTypes                           Type,
                                         IReadOnlyDictionary<String, Object>  CustomData    = null)

            => new MeterReport(Value,
                               Unit,
                               Type,
                               CustomData);

        #endregion


        #region Documentation

        // MeteridType
        //
        // Code  Signification                                              Unit
        // 1     Total Duration (total duration for the charge session)     "min"   (for Minutes)
        // 2     Total Energy (total energy for the charge session)         "Wh"    (for Watt x hours)
        // 3     B2B Service Cost        Local Currency ("EUR" for Euro)
        // 10xx  Charging duration at a given level of power ("x" value)    "min"   (for Minutes)
        //       Example: 1012 means "Charging duration at 12 kW"
        // 21    Average power, per step of 1 minutes                       "W"     (for Watts)
        //       (Repetitive meter)
        // 25    Average power, per step of 5 minutes                       "W"     (for Watts)
        //       (Repetitive meter)
        // 31    Maximum consumed power reached, per step of 1 minutes      "W"     (for Watts)
        //       (Repetitive meter)
        // 35    Maximum consumed power reached, per step of 5 minutes      "W"     (for Watts)
        //       (Repetitive meter)
        // 41    Delivered energy, per step of 1 minutes                    "Wh"    (for Watt x hours)
        //       (Repetitive meter)
        // 45    Delivered energy, per step of 5 minutes                    "Wh"    (for Watt x hours)
        //       (Repetitive meter)
        //
        // Meter value is a string coded decimal (XML). The decimal separator is "."

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:eMIP = "https://api-iop.gireve.com/schemas/AuthorisationV1/">
        //
        //  [...]
        //
        //  <meterReport>
        //     <meterTypeId>?</meterTypeId>
        //     <meterValue>?</meterValue>
        //     <meterUnit>?</meterUnit>
        //  </meterReport>
        //
        //  [...]
        //
        // </soap:Envelope>

        #endregion

        #region (static) Parse(MeterReportXML,  CustomMeterReportParser = null, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP meter report.
        /// </summary>
        /// <param name="MeterReportXML">The XML to parse.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static MeterReport Parse(XElement                              MeterReportXML,
                                        CustomXMLParserDelegate<MeterReport>  CustomMeterReportParser   = null,
                                        OnExceptionDelegate                   OnException               = null)
        {

            if (TryParse(MeterReportXML,
                         out MeterReport _MeterReport,
                         CustomMeterReportParser,
                         OnException))
            {
                return _MeterReport;
            }

            return null;

        }

        #endregion

        #region (static) Parse(MeterReportText, CustomMeterReportParser = null, OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP meter report.
        /// </summary>
        /// <param name="MeterReportText">The text to parse.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static MeterReport Parse(String                                MeterReportText,
                                        CustomXMLParserDelegate<MeterReport>  CustomMeterReportParser   = null,
                                        OnExceptionDelegate                   OnException               = null)
        {

            if (TryParse(MeterReportText,
                         out MeterReport _MeterReport,
                         CustomMeterReportParser,
                         OnException))
            {
                return _MeterReport;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(MeterReportXML,  out MeterReport, CustomMeterReportParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OIOI MeterReport.
        /// </summary>
        /// <param name="MeterReportXML">The XML to parse.</param>
        /// <param name="MeterReport">The parsed meter report.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                              MeterReportXML,
                                       out MeterReport                       MeterReport,
                                       CustomXMLParserDelegate<MeterReport>  CustomMeterReportParser   = null,
                                       OnExceptionDelegate                   OnException               = null)
        {

            try
            {

                MeterReport = new MeterReport(

                                  MeterReportXML.ElementValueOrFail ("meterTypeId"),
                                  MeterReportXML.ElementValueOrFail ("meterValue"),
                                  MeterReportXML.MapEnumValuesOrFail("meterUnit",  MeterTypes.Parse)

                              );

                if (CustomMeterReportParser != null)
                    MeterReport = CustomMeterReportParser(MeterReportXML, MeterReport);

                return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow,
                                    MeterReportXML,
                                    e);
            }

            MeterReport = null;
            return false;

        }

        #endregion

        #region (static) TryParse(MeterReportText, out MeterReport, CustomMeterReportParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OIOI meter report.
        /// </summary>
        /// <param name="MeterReportText">The text to parse.</param>
        /// <param name="MeterReport">The parsed meter report.</param>
        /// <param name="CustomMeterReportParser">An optional delegate to parse custom MeterReport XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                MeterReportText,
                                       out MeterReport                       MeterReport,
                                       CustomXMLParserDelegate<MeterReport>  CustomMeterReportParser   = null,
                                       OnExceptionDelegate                   OnException               = null)
        {

            try
            {

                return TryParse(XElement.Parse(MeterReportText),
                                out MeterReport,
                                CustomMeterReportParser,
                                OnException);

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, MeterReportText, e);

                MeterReport = null;
                return false;

            }

        }

        #endregion

        #region ToXML(XName = null, CustomMeterReportSerializer = null)

        /// <summary>
        /// Return an XML representation of this EVSE data record.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        /// <param name="CustomMeterReportSerializer">A delegate to serialize custom MeterReport XML elements.</param>
        public XElement ToXML(XName                                     XName                         = null,
                              CustomXMLSerializerDelegate<MeterReport>  CustomMeterReportSerializer   = null)
        {

            var XML = new XElement(XName ?? "meterReport",

                          new XElement("meterTypeId",  Type.Code),
                          new XElement("meterValue",   Value),
                          new XElement("meterUnit",    Unit)

                      );

            return CustomMeterReportSerializer != null
                       ? CustomMeterReportSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (MeterReport1, MeterReport2)

        /// <summary>
        /// Compares two meter reportes for equality.
        /// </summary>
        /// <param name="MeterReport1">An meter report.</param>
        /// <param name="MeterReport2">Another meter report.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (MeterReport MeterReport1, MeterReport MeterReport2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(MeterReport1, MeterReport2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) MeterReport1 == null) || ((Object) MeterReport2 == null))
                return false;

            return MeterReport1.Equals(MeterReport2);

        }

        #endregion

        #region Operator != (MeterReport1, MeterReport2)

        /// <summary>
        /// Compares two meter reportes for inequality.
        /// </summary>
        /// <param name="MeterReport1">An meter report.</param>
        /// <param name="MeterReport2">Another meter report.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (MeterReport MeterReport1, MeterReport MeterReport2)

            => !(MeterReport1 == MeterReport2);

        #endregion

        #region Operator <  (MeterReport1, MeterReport2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterReport1">An meter report.</param>
        /// <param name="MeterReport2">Another meter report.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (MeterReport MeterReport1, MeterReport MeterReport2)
        {

            if ((Object) MeterReport1 == null)
                throw new ArgumentNullException(nameof(MeterReport1), "The given MeterReport1 must not be null!");

            return MeterReport1.CompareTo(MeterReport2) < 0;

        }

        #endregion

        #region Operator <= (MeterReport1, MeterReport2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterReport1">An meter report.</param>
        /// <param name="MeterReport2">Another meter report.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (MeterReport MeterReport1, MeterReport MeterReport2)
            => !(MeterReport1 > MeterReport2);

        #endregion

        #region Operator >  (MeterReport1, MeterReport2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterReport1">An meter report.</param>
        /// <param name="MeterReport2">Another meter report.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (MeterReport MeterReport1, MeterReport MeterReport2)
        {

            if ((Object)MeterReport1 == null)
                throw new ArgumentNullException(nameof(MeterReport1), "The given MeterReport1 must not be null!");

            return MeterReport1.CompareTo(MeterReport2) > 0;

        }

        #endregion

        #region Operator >= (MeterReport1, MeterReport2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterReport1">An meter report.</param>
        /// <param name="MeterReport2">Another meter report.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (MeterReport MeterReport1, MeterReport MeterReport2)
            => !(MeterReport1 < MeterReport2);

        #endregion

        #endregion

        #region IComparable<MeterReport> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var MeterReport = Object as MeterReport;
            if ((Object) MeterReport == null)
                throw new ArgumentException("The given object is not an meter report identification!", nameof(Object));

            return CompareTo(MeterReport);

        }

        #endregion

        #region CompareTo(MeterReport)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterReport">An object to compare with.</param>
        public Int32 CompareTo(MeterReport MeterReport)
        {

            if ((Object) MeterReport == null)
                throw new ArgumentNullException(nameof(MeterReport), "The given meter report must not be null!");

            var _Result = String.Compare(Value, MeterReport.Value, StringComparison.Ordinal);

            if (_Result == 0)
                _Result = String.Compare(Unit,  MeterReport.Unit,  StringComparison.Ordinal);

            if (_Result == 0)
                _Result = Type.CompareTo(MeterReport.Type);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<MeterReport> Members

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

            var MeterReport = Object as MeterReport;
            if ((Object) MeterReport == null)
                return false;

            return Equals(MeterReport);

        }

        #endregion

        #region Equals(MeterReport)

        /// <summary>
        /// Compares two meter reportes for equality.
        /// </summary>
        /// <param name="MeterReport">An meter report to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(MeterReport MeterReport)
        {

            if ((Object) MeterReport == null)
                return false;

            return Value.Equals(MeterReport.Value) &&
                   Unit. Equals(MeterReport.Type)  &&
                   Type. Equals(MeterReport.Unit);

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

                return Type. GetHashCode() * 5 ^
                       Unit. GetHashCode() * 3 ^
                       Value.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Value, " ",
                             Unit, " (",
                             Type, ")");

        #endregion

    }

}
