/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// An eMIP request status.
    /// </summary>
    public struct RequestStatus : IId,
                                  IEquatable <RequestStatus>,
                                  IComparable<RequestStatus>
    {

        #region Data

        private static readonly Dictionary<Int32, RequestStatus> Lookup = new Dictionary<Int32, RequestStatus>();

        #endregion

        #region Properties

        /// <summary>
        /// The internal identification.
        /// </summary>
        public Int32   Code          { get; }

        /// <summary>
        /// The description of the result status.
        /// </summary>
        public String  Description   { get; }

        #endregion

        #region Constructor(s)

        #region (static)  RequestStatus() <- Does reflection!

        static RequestStatus()
        {

            RequestStatus requeststatus;

            foreach (var _MethodInfo in typeof(RequestStatus).GetMethods())
            {
                if (_MethodInfo.IsStatic &&
                    _MethodInfo.GetParameters().Length == 0)
                {

                    requeststatus = (RequestStatus) _MethodInfo.Invoke(Activator.CreateInstance(typeof(RequestStatus)), null);

                    if (!Lookup.ContainsKey(requeststatus.Code))
                        Lookup.Add(requeststatus.Code, requeststatus);

                }
            }

        }

        #endregion

        #region (private) RequestStatus(Code, Description = null)

        /// <summary>
        /// Create a new request status.
        /// </summary>
        /// <param name="Code">The numeric code of the status.</param>
        /// <param name="Description">The description of the result status.</param>
        private RequestStatus(Int32   Code,
                              String  Description = null)
        {

            this.Code         = Code;
            this.Description  = Description;

            lock (Lookup)
            {
                if (!Lookup.ContainsKey(Code))
                    Lookup.Add(Code, this);
            }

        }

        #endregion

        #endregion


        #region Register(Code, Description = null)

        /// <summary>
        /// Parse the given string as a request status.
        /// </summary>
        /// <param name="Code">The numeric code of the status.</param>
        /// <param name="Description">The description of the result status.</param>
        public static RequestStatus Register(Int32   Code,
                                             String  Description = null)

            => new RequestStatus(Code,
                                 Description);

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a request status.
        /// </summary>
        /// <param name="Text">A text representation of a request status.</param>
        public static RequestStatus Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a request status must not be null or empty!");

            #endregion

            return Parse(Int32.Parse(Text));

        }

        #endregion

        #region Parse   (Code)

        /// <summary>
        /// Parse the given number as a request status.
        /// </summary>
        /// <param name="Code">A numeric representation of a request status.</param>
        public static RequestStatus Parse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out RequestStatus Status))
                return Status;

            return new RequestStatus(Code);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a request status.
        /// </summary>
        /// <param name="Text">A text representation of a request status.</param>
        public static RequestStatus? TryParse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out Int32 Code))
                return new RequestStatus?();

            #endregion

            return TryParse(Code);

        }

        #endregion

        #region TryParse(Code)

        /// <summary>
        /// Try to parse the given number as a request status.
        /// </summary>
        /// <param name="Code">A numeric representation of a request status.</param>
        public static RequestStatus? TryParse(Int32 Code)
        {

            if (Lookup.TryGetValue(Code, out RequestStatus Status))
                return Status;

            return new RequestStatus(Code);

        }

        #endregion

        #region TryParse(Text, out RequestStatus)

        /// <summary>
        /// Try to parse the given string as a request status.
        /// </summary>
        /// <param name="Text">A text representation of a request status.</param>
        /// <param name="RequestStatus">The parsed request status.</param>
        public static Boolean TryParse(String Text, out RequestStatus RequestStatus)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            #endregion

            if (Text.IsNullOrEmpty() || !Int32.TryParse(Text, out Int32 Value))
            {
                RequestStatus = default(RequestStatus);
                return false;
            }

            RequestStatus = new RequestStatus(Value);
            return true;

        }

        #endregion

        #region TryParse(Code, out RequestStatus)

        /// <summary>
        /// Try to parse the given number as a request status.
        /// </summary>
        /// <param name="Code">A numeric representation of a request status.</param>
        /// <param name="RequestStatus">The parsed request status.</param>
        public static Boolean TryParse(Int32 Code, out RequestStatus RequestStatus)
        {

            if (Lookup.TryGetValue(Code, out RequestStatus))
                return true;

            RequestStatus = new RequestStatus(Code);
            return true;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this request status.
        /// </summary>
        public RequestStatus Clone

            => new RequestStatus(Code,
                                 new String(Description.ToCharArray()));

        #endregion


        #region Static definitions

        // Source: GIREVE eMIPv0.7.4-RequestStatus&ExceptionCodes-values_V12_Diff
        // All those will be reflected and added to the lookup within the static constructor!

        /// <summary>
        /// Ok!
        /// </summary>
        public static RequestStatus Ok
            => new RequestStatus(1,         "Ok!");



        /// <summary>
        /// Ok-Warning: The CPO of the EVSE cannot by identified!
        /// </summary>
        public static RequestStatus CPONotFound
            => new RequestStatus(201,       "The CPO of the EVSE cannot by identified!");

        /// <summary>
        /// Ok-Warning: There is no roaming contract between the CPO and the eMSP for the requested service!
        /// </summary>
        public static RequestStatus NoRoamingContract
            => new RequestStatus(202,       "There is no roaming contract between the CPO and the eMSP for the requested service!");

        /// <summary>
        /// Ok-Warning: The eMSP of the end-user cannot be identified!
        /// </summary>
        public static RequestStatus EMSPNotFound
            => new RequestStatus(203,       "The eMSP of the end-user cannot be identified!");

        /// <summary>
        /// Ok-Warning: The autorisation request is rejected by CPO: The requested service is not available on this EVSE!
        /// </summary>
        public static RequestStatus EVSEServiceNotAvailable
            => new RequestStatus(205,       "The autorisation request is rejected by CPO: The requested service is not available on this EVSE!");

        /// <summary>
        /// Ok-Warning: The autorisation request is rejected by CPO: The EVSE is not technically reachable (communication)!
        /// </summary>
        public static RequestStatus EVSENotReachable
            => new RequestStatus(206,       "The autorisation request is rejected by CPO: The EVSE is not technically reachable (communication)!");



        /// <summary>
        /// KO-Error 201: The autorisation request is rejected: Unknown error!
        /// </summary>
        public static RequestStatus UnknownAuthError
            => new RequestStatus(10201,     "The autorisation request is rejected: Unknown error!");

        /// <summary>
        /// KO-Error 207: The CPO of the EVSE is not reachable!
        /// </summary>
        public static RequestStatus CPO_NotReachable
            => new RequestStatus(10207,     "The CPO of the EVSE is not reachable!");

        /// <summary>
        /// KO-Error 210: The eMSP did not respond correctly to the request!
        /// </summary>
        public static RequestStatus EMSP_InvalidResponse
            => new RequestStatus(10210,     "The eMSP did not respond correctly to the request!");



        /// <summary>
        /// KO-Error 501: Session not found!
        /// </summary>
        public static RequestStatus SessionNotFound
            => new RequestStatus(10501,     "Session not found!");

        /// <summary>
        /// KO-Error 502: CPO/eMSP not found!
        /// </summary>
        public static RequestStatus CPOorEMSP_NotFound
            => new RequestStatus(10502,     "CPO/eMSP not found!");

        /// <summary>
        /// KO-Error 503: The CPO/eMSP does not accept Action/Event!
        /// </summary>
        public static RequestStatus CPOorEMSP_DoesNotAcceptActionOrEvent
            => new RequestStatus(10503,     "The CPO/eMSP does not accept Action/Event!");

        /// <summary>
        /// KO-Error 504: The request cannot be sent to the CPO/eMSP or the CPO/eMSP does not respond!
        /// </summary>
        public static RequestStatus CPOorEMSP_DoesNotRespond
            => new RequestStatus(10504,     "The request cannot be sent to the CPO/eMSP or the CPO/eMSP does not respond!");

        /// <summary>
        /// KO-Error 505: The CPO/eMSP returns an IOP Fault!
        /// </summary>
        public static RequestStatus CPOorEMSP_IOPFault
            => new RequestStatus(10505,     "The CPO/eMSP returns an IOP Fault!");

        /// <summary>
        /// KO-Error 506: The CPO/eMSP doesn't recognise the actionNature/eventNature: No action on its side!
        /// </summary>
        public static RequestStatus CPOorEMSP_DoesNotRecogniseActionOrEventNature
            => new RequestStatus(10506,     "The CPO/eMSP doesn't recognise the actionNature/eventNature: No action on its side!");

        /// <summary>
        /// KO-Error 507: The CPO/eMSP returns an error code: An error occured on its side during the action/report treatment!
        /// </summary>
        public static RequestStatus CPOorEMSP_ActionOrReportErrorOccured
            => new RequestStatus(10507,     "The CPO/eMSP returns an error code: An error occured on its side during the action/report treatment!");

        /// <summary>
        /// KO-Error 508: The requestor is neither eMSP nor CPO for this session!
        /// </summary>
        public static RequestStatus CPOorEMSP_IllegalSessionAccess
            => new RequestStatus(10508,     "The requestor is neither eMSP nor CPO for this session!");



        /// <summary>
        /// The charging pool/station/point/connector is unknown!
        /// </summary>
        public static RequestStatus UnknownEntity
            => new RequestStatus(10601,     "The charging pool/station/point/connector is unknown!");



        /// <summary>
        /// OKWarning701: eMSP doesn't accept this final CDR because one has already been received for this session (optional)!
        /// </summary>
        public static RequestStatus OKWarning701
            => new RequestStatus(10701,     "eMSP doesn't accept this final CDR because one has already been received for this session (optional)!");



        /// <summary>
        /// System error!
        /// </summary>
        public static RequestStatus SystemError
            => new RequestStatus(-9999990,  "System error!");

        /// <summary>
        /// HTTP error!
        /// </summary>
        public static RequestStatus HTTPError
            => new RequestStatus(-9999991,  "HTTP error!");

        /// <summary>
        /// Service not available!
        /// </summary>
        public static RequestStatus ServiceNotAvailable
            => new RequestStatus(-9999992,  "Service not available!");

        /// <summary>
        /// Data error!
        /// </summary>
        public static RequestStatus DataError
            => new RequestStatus(-9999993,  "Data error!");

        #endregion


        #region Operator overloading

        #region Operator == (RequestStatus1, RequestStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus1">A request status.</param>
        /// <param name="RequestStatus2">Another request status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RequestStatus RequestStatus1, RequestStatus RequestStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(RequestStatus1, RequestStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RequestStatus1 == null) || ((Object) RequestStatus2 == null))
                return false;

            return RequestStatus1.Equals(RequestStatus2);

        }

        #endregion

        #region Operator != (RequestStatus1, RequestStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus1">A request status.</param>
        /// <param name="RequestStatus2">Another request status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RequestStatus RequestStatus1, RequestStatus RequestStatus2)
            => !(RequestStatus1 == RequestStatus2);

        #endregion

        #region Operator <  (RequestStatus1, RequestStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus1">A request status.</param>
        /// <param name="RequestStatus2">Another request status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RequestStatus RequestStatus1, RequestStatus RequestStatus2)
        {

            if ((Object) RequestStatus1 == null)
                throw new ArgumentNullException(nameof(RequestStatus1), "The given RequestStatus1 must not be null!");

            return RequestStatus1.CompareTo(RequestStatus2) < 0;

        }

        #endregion

        #region Operator <= (RequestStatus1, RequestStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus1">A request status.</param>
        /// <param name="RequestStatus2">Another request status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RequestStatus RequestStatus1, RequestStatus RequestStatus2)
            => !(RequestStatus1 > RequestStatus2);

        #endregion

        #region Operator >  (RequestStatus1, RequestStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus1">A request status.</param>
        /// <param name="RequestStatus2">Another request status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RequestStatus RequestStatus1, RequestStatus RequestStatus2)
        {

            if ((Object) RequestStatus1 == null)
                throw new ArgumentNullException(nameof(RequestStatus1), "The given RequestStatus1 must not be null!");

            return RequestStatus1.CompareTo(RequestStatus2) > 0;

        }

        #endregion

        #region Operator >= (RequestStatus1, RequestStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus1">A request status.</param>
        /// <param name="RequestStatus2">Another request status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RequestStatus RequestStatus1, RequestStatus RequestStatus2)
            => !(RequestStatus1 < RequestStatus2);

        #endregion

        #endregion

        #region IComparable<RequestStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is RequestStatus))
                throw new ArgumentException("The given object is not a request status!",
                                            nameof(Object));

            return CompareTo((RequestStatus) Object);

        }

        #endregion

        #region CompareTo(RequestStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RequestStatus">An object to compare with.</param>
        public Int32 CompareTo(RequestStatus RequestStatus)
        {

            if ((Object) RequestStatus == null)
                throw new ArgumentNullException(nameof(RequestStatus),  "The given request status must not be null!");

            return Code.CompareTo(RequestStatus.Code);

        }

        #endregion

        #endregion

        #region IEquatable<RequestStatus> Members

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

            if (!(Object is RequestStatus))
                return false;

            return Equals((RequestStatus) Object);

        }

        #endregion

        #region Equals(RequestStatus)

        /// <summary>
        /// Compares two RequestStatuss for equality.
        /// </summary>
        /// <param name="RequestStatus">A request status to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RequestStatus RequestStatus)
        {

            if ((Object) RequestStatus == null)
                return false;

            return Code.Equals(RequestStatus.Code);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => Code.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Code,
                             Description.IsNotNullOrEmpty() ? ": " + Description : "");

        #endregion

    }

}
