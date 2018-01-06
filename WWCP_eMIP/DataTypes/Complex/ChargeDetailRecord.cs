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
    /// A charge detail record.
    /// </summary>
    public class ChargeDetailRecord : ACustomData,
                                      IEquatable<ChargeDetailRecord>,
                                      IComparable<ChargeDetailRecord>,
                                      IComparable
    {

        #region Properties

        /// <summary>
        /// The meter value.
        /// </summary>
        public String        Value    { get; }

        /// <summary>
        /// The unit of the meter value.
        /// </summary>
        public String        Unit     { get; }

        /// <summary>
        /// The type of the meter value.
        /// </summary>
        public MeterTypeIds  Type     { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charge detail record.
        /// </summary>
        /// <param name="Value">The meter value.</param>
        /// <param name="Unit">The unit of the meter value.</param>
        /// <param name="Type">The type of the meter value.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargeDetailRecord(String                               Value,
                           String                               Unit,
                           MeterTypeIds                         Type,
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
        /// Create a new charge detail record.
        /// </summary>
        /// <param name="Value">The meter value.</param>
        /// <param name="Unit">The unit of the meter value.</param>
        /// <param name="Type">The type of the meter value.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public static ChargeDetailRecord Create(String                               Value,
                                         String                               Unit,
                                         MeterTypeIds                         Type,
                                         IReadOnlyDictionary<String, Object>  CustomData    = null)

            => new ChargeDetailRecord(Value,
                               Unit,
                               Type,
                               CustomData);

        #endregion


        #region Documentation

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

        #region (static) Parse(ChargeDetailRecordXML,  CustomChargeDetailRecordParser = null, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordXML">The XML to parse.</param>
        /// <param name="CustomChargeDetailRecordParser">An optional delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargeDetailRecord Parse(XElement                                     ChargeDetailRecordXML,
                                               CustomXMLParserDelegate<ChargeDetailRecord>  CustomChargeDetailRecordParser   = null,
                                               OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(ChargeDetailRecordXML,
                         out ChargeDetailRecord _ChargeDetailRecord,
                         CustomChargeDetailRecordParser,
                         OnException))

                return _ChargeDetailRecord;

            return null;

        }

        #endregion

        #region (static) Parse(ChargeDetailRecordText, CustomChargeDetailRecordParser = null, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordText">The text to parse.</param>
        /// <param name="CustomChargeDetailRecordParser">An optional delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargeDetailRecord Parse(String                                ChargeDetailRecordText,
                                        CustomXMLParserDelegate<ChargeDetailRecord>  CustomChargeDetailRecordParser   = null,
                                        OnExceptionDelegate                   OnException               = null)
        {

            if (TryParse(ChargeDetailRecordText,
                         out ChargeDetailRecord _ChargeDetailRecord,
                         CustomChargeDetailRecordParser,
                         OnException))

                return _ChargeDetailRecord;

            return null;

        }

        #endregion

        #region (static) TryParse(ChargeDetailRecordXML,  out ChargeDetailRecord, CustomChargeDetailRecordParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OIOI ChargeDetailRecord.
        /// </summary>
        /// <param name="ChargeDetailRecordXML">The XML to parse.</param>
        /// <param name="ChargeDetailRecord">The parsed charge detail record.</param>
        /// <param name="CustomChargeDetailRecordParser">An optional delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                              ChargeDetailRecordXML,
                                       out ChargeDetailRecord                       ChargeDetailRecord,
                                       CustomXMLParserDelegate<ChargeDetailRecord>  CustomChargeDetailRecordParser   = null,
                                       OnExceptionDelegate                   OnException               = null)
        {

            try
            {

                ChargeDetailRecord = new ChargeDetailRecord(ChargeDetailRecordXML.ElementValueOrFail (eMIPNS.Authorisation + "meterTypeId"),
                                              ChargeDetailRecordXML.ElementValueOrFail (eMIPNS.Authorisation + "meterValue"),
                                              ChargeDetailRecordXML.MapEnumValuesOrFail(eMIPNS.Authorisation + "meterUnit",
                                                                                 ConversionMethods.AsMeterTypeId));

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

        #region (static) TryParse(ChargeDetailRecordText, out ChargeDetailRecord, CustomChargeDetailRecordParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OIOI charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordText">The text to parse.</param>
        /// <param name="ChargeDetailRecord">The parsed charge detail record.</param>
        /// <param name="CustomChargeDetailRecordParser">An optional delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                ChargeDetailRecordText,
                                       out ChargeDetailRecord                       ChargeDetailRecord,
                                       CustomXMLParserDelegate<ChargeDetailRecord>  CustomChargeDetailRecordParser   = null,
                                       OnExceptionDelegate                   OnException               = null)
        {

            try
            {

                return TryParse(XElement.Parse(ChargeDetailRecordText),
                                out ChargeDetailRecord,
                                CustomChargeDetailRecordParser,
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

        #region ToXML(XName = null, CustomChargeDetailRecordSerializer = null)

        /// <summary>
        /// Return an XML representation of this EVSE data record.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A delegate to serialize custom ChargeDetailRecord XML elements.</param>
        public XElement ToXML(XName                                            XName                                = null,
                              CustomXMLSerializerDelegate<ChargeDetailRecord>  CustomChargeDetailRecordSerializer   = null)
        {

            var XML = new XElement(XName ?? eMIPNS.Authorisation + "meterReport",

                          new XElement(eMIPNS.Authorisation + "meterTypeId",  Type.AsNumber()),
                          new XElement(eMIPNS.Authorisation + "meterValue",   Value),
                          new XElement(eMIPNS.Authorisation + "meterUnit",    Unit)

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
            if (Object.ReferenceEquals(ChargeDetailRecord1, ChargeDetailRecord2))
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
                throw new ArgumentException("The given object is not an charge detail record identification!", nameof(Object));

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

            var _Result = String.Compare(Value, ChargeDetailRecord.Value, StringComparison.Ordinal);

            if (_Result == 0)
                _Result = String.Compare(Unit,  ChargeDetailRecord.Unit,  StringComparison.Ordinal);

            if (_Result == 0)
                _Result = Type.CompareTo(ChargeDetailRecord.Type);

            return _Result;

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

            return Value.Equals(ChargeDetailRecord.Value) &&
                   Unit. Equals(ChargeDetailRecord.Type)  &&
                   Type. Equals(ChargeDetailRecord.Unit);

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Value, " ",
                             Unit, " (",
                             Type.AsText(), ")");

        #endregion

    }

}
