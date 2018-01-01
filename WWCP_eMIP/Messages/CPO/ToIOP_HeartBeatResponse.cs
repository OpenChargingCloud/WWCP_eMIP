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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// An eMIP add charge details records response.
    /// </summary>
    public class AddCDRsResponse : AResponse<ToIOP_HeartBeatRequest,
                                             AddCDRsResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of refused charge detail record identifications.
        /// </summary>
        public IEnumerable<CDR_Id>  ImplausibleCDRs   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse OK(ToIOP_HeartBeatRequest  Request,
                                         String          Description = null)

            => new AddCDRsResponse(Request,
                                   Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse Partly(ToIOP_HeartBeatRequest  Request,
                                             String          Description = null)

            => new AddCDRsResponse(Request,
                                   Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse NotAuthorized(ToIOP_HeartBeatRequest  Request,
                                                    String          Description = null)

            => new AddCDRsResponse(Request,
                                   Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse InvalidId(ToIOP_HeartBeatRequest  Request,
                                                String          Description = null)

            => new AddCDRsResponse(Request,
                                   Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse Server(ToIOP_HeartBeatRequest  Request,
                                             String          Description = null)

            => new AddCDRsResponse(Request,
                                   Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse Format(ToIOP_HeartBeatRequest  Request,
                                             String          Description = null)

            => new AddCDRsResponse(Request,
                                   Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new eMIP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Result">A generic eMIP result.</param>
        /// <param name="ImplausibleCDRs">An enumeration of refused charge detail record identifications.</param>
        public AddCDRsResponse(ToIOP_HeartBeatRequest       Request,
                               Result               Result,
                               IEnumerable<CDR_Id>  ImplausibleCDRs = null)

            : base(Request, Result)

        {

            this.ImplausibleCDRs  = ImplausibleCDRs ?? new CDR_Id[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:eMIP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <eMIP:AddCDRsResponse>
        //
        //         <eMIP:result>
        //            <eMIP:resultCode>
        //               <eMIP:resultCode>?</eMIP:resultCode>
        //            </eMIP:resultCode>
        //            <eMIP:resultDescription>?</eMIP:resultDescription>
        //         </eMIP:result>
        //
        //         <!--Zero or more repetitions:-->
        //         <eMIP:implausibleCdrsArray>?</eMIP:implausibleCdrsArray>
        //
        //      </eMIP:AddCDRsResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        // <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
        //   <soap:Body>
        //     <AddCDRsResponse xmlns="http://ochp.eu/1.4">
        //
        //       <result>
        //         <resultCode><resultCode>partly</resultCode></resultCode>
        //         <resultDescription>Only part of the data was accepted.</resultDescription>
        //       </result>
        //
        //       <implausibleCdrsArray>47E18B5C29FE4BDB818E74082F0101AD</implausibleCdrsArray>
        //       <implausibleCdrsArray>71267A5E0370400AAE3E47428986BE60</implausibleCdrsArray>
        //
        //     </AddCDRsResponse>
        //   </soap:Body>
        // </soap:Envelope>

        #endregion

        #region (static) Parse   (Request, AddCDRsResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an eMIP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="AddCDRsResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddCDRsResponse Parse(ToIOP_HeartBeatRequest       Request,
                                            XElement             AddCDRsResponseXML,
                                            OnExceptionDelegate  OnException = null)
        {

            if (TryParse(Request, AddCDRsResponseXML, out AddCDRsResponse _AddCDRsResponse, OnException))
                return _AddCDRsResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, AddCDRsResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an eMIP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="AddCDRsResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddCDRsResponse Parse(ToIOP_HeartBeatRequest       Request,
                                            String               AddCDRsResponseText,
                                            OnExceptionDelegate  OnException = null)
        {

            if (TryParse(Request, AddCDRsResponseText, out AddCDRsResponse _AddCDRsResponse, OnException))
                return _AddCDRsResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, AddCDRsResponseXML,  out AddCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an eMIP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="AddCDRsResponseXML">The XML to parse.</param>
        /// <param name="AddCDRsResponse">The parsed add charge details records response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(ToIOP_HeartBeatRequest       Request,
                                       XElement             AddCDRsResponseXML,
                                       out AddCDRsResponse  AddCDRsResponse,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                AddCDRsResponse = new AddCDRsResponse(

                                      Request,

                                      AddCDRsResponseXML.MapElementOrFail(eMIPNS.Default + "result",
                                                                          Result.Parse,
                                                                          OnException),

                                      AddCDRsResponseXML.MapValues       (eMIPNS.Default + "implausibleCdrsArray",
                                                                          CDR_Id.Parse)

                                  );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, AddCDRsResponseXML, e);

                AddCDRsResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, AddCDRsResponseText, out AddCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an eMIP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="AddCDRsResponseText">The text to parse.</param>
        /// <param name="AddCDRsResponse">The parsed add charge details records response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(ToIOP_HeartBeatRequest       Request,
                                       String               AddCDRsResponseText,
                                       out AddCDRsResponse  AddCDRsResponse,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(AddCDRsResponseText).Root,
                             out AddCDRsResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, AddCDRsResponseText, e);
            }

            AddCDRsResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(eMIPNS.Default + "AddCDRsResponse",

                   Result.ToXML(),

                   ImplausibleCDRs.Any()
                       ? ImplausibleCDRs.SafeSelect(cdrid => new XElement(eMIPNS.Default + "implausibleCdrsArray", cdrid.ToString()))
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (AddCDRsResponse1, AddCDRsResponse2)

        /// <summary>
        /// Compares two add charge details records responses for equality.
        /// </summary>
        /// <param name="AddCDRsResponse1">A add charge details records response.</param>
        /// <param name="AddCDRsResponse2">Another add charge details records response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AddCDRsResponse AddCDRsResponse1, AddCDRsResponse AddCDRsResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AddCDRsResponse1, AddCDRsResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AddCDRsResponse1 == null) || ((Object) AddCDRsResponse2 == null))
                return false;

            return AddCDRsResponse1.Equals(AddCDRsResponse2);

        }

        #endregion

        #region Operator != (AddCDRsResponse1, AddCDRsResponse2)

        /// <summary>
        /// Compares two add charge details records responses for inequality.
        /// </summary>
        /// <param name="AddCDRsResponse1">A add charge details records response.</param>
        /// <param name="AddCDRsResponse2">Another add charge details records response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AddCDRsResponse AddCDRsResponse1, AddCDRsResponse AddCDRsResponse2)

            => !(AddCDRsResponse1 == AddCDRsResponse2);

        #endregion

        #endregion

        #region IEquatable<AddCDRsResponse> Members

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

            // Check if the given object is a add charge details records response.
            var AddCDRsResponse = Object as AddCDRsResponse;
            if ((Object) AddCDRsResponse == null)
                return false;

            return this.Equals(AddCDRsResponse);

        }

        #endregion

        #region Equals(AddCDRsResponse)

        /// <summary>
        /// Compares two add charge details records responses for equality.
        /// </summary>
        /// <param name="AddCDRsResponse">A add charge details records response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AddCDRsResponse AddCDRsResponse)
        {

            if ((Object) AddCDRsResponse == null)
                return false;

            return this.Result.Equals(AddCDRsResponse.Result);

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

                return ImplausibleCDRs != null

                           ? Result.GetHashCode() * 11 ^
                             ImplausibleCDRs.SafeSelect(cdr => cdr.GetHashCode()).Aggregate((a, b) => a ^ b)

                           : Result.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Result,
                             ImplausibleCDRs.Any()
                                 ? " " + ImplausibleCDRs.Count() + " refused charge detail records"
                                 : "");

        #endregion



        public class Builder : ABuilder
        {

            #region Properties

            /// <summary>
            /// An enumeration of refused charge detail record identifications.
            /// </summary>
            public IEnumerable<CDR_Id>  ImplausibleCDRs   { get; set; }

            #endregion

            public Builder(AddCDRsResponse AddCDRsResponse = null)
            {

                if (AddCDRsResponse != null)
                {

                    this.ImplausibleCDRs = AddCDRsResponse.ImplausibleCDRs;

                    if (AddCDRsResponse.CustomData != null)
                        foreach (var item in AddCDRsResponse.CustomData)
                            CustomData.Add(item.Key, item.Value);

                }

            }


            //public Acknowledgement<T> ToImmutable()

            //    => new Acknowledgement<T>(Request,
            //                              Result,
            //                              StatusCode,
            //                              SessionId,
            //                              PartnerSessionId,
            //                              CustomData);

        }

    }

}
