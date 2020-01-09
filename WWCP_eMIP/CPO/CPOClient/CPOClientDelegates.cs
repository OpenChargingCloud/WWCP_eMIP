/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    ///// <summary>
    ///// A delegate for filtering EVSE status records.
    ///// </summary>
    ///// <param name="EVSEStatusRecord">An EVSE status record.</param>
    //public delegate Boolean IncludeEVSEStatusRecordsDelegate(EVSEStatusRecord  EVSEStatusRecord);


    #region OnHeartbeatRequest/-Response

    /// <summary>
    /// A delegate called whenever a heartbeat will be send upstream.
    /// </summary>
    public delegate Task OnSendHeartbeatRequestDelegate (DateTime                                 LogTimestamp,
                                                         DateTime                                 RequestTimestamp,
                                                         ICPOClient                               Sender,
                                                         String                                   SenderId,
                                                         EventTracking_Id                         EventTrackingId,
                                                         Partner_Id                               PartnerId,
                                                         Operator_Id                              OperatorId,
                                                         Transaction_Id?                          TransactionId,
                                                         TimeSpan                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever a heartbeat had been sent upstream.
    /// </summary>
    public delegate Task OnSendHeartbeatResponseDelegate(DateTime                                 LogTimestamp,
                                                         DateTime                                 RequestTimestamp,
                                                         ICPOClient                               Sender,
                                                         String                                   SenderId,
                                                         EventTracking_Id                         EventTrackingId,
                                                         Partner_Id                               PartnerId,
                                                         Operator_Id                              OperatorId,
                                                         Transaction_Id?                          TransactionId,
                                                         TimeSpan                                 RequestTimeout,
                                                         HeartbeatResponse                        Result,
                                                         TimeSpan                                 Duration);

    #endregion


    #region OnSetChargingPoolAvailabilityStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever a charging pool availability status will be send upstream.
    /// </summary>
    public delegate Task OnSetChargingPoolAvailabilityStatusRequestDelegate (DateTime                                    LogTimestamp,
                                                                             DateTime                                    RequestTimestamp,
                                                                             ICPOClient                                  Sender,
                                                                             String                                      SenderId,
                                                                             EventTracking_Id                            EventTrackingId,

                                                                             Partner_Id                                  PartnerId,
                                                                             Operator_Id                                 OperatorId,
                                                                             ChargingPool_Id                             ChargingPoolId,
                                                                             DateTime                                    StatusEventDate,
                                                                             ChargingPoolAvailabilityStatusTypes         AvailabilityStatus,
                                                                             Transaction_Id?                             TransactionId,
                                                                             DateTime?                                   AvailabilityStatusUntil,
                                                                             String                                      AvailabilityStatusComment,

                                                                             TimeSpan                                    RequestTimeout);

    /// <summary>
    /// A delegate called whenever a charging pool availability status had been sent upstream.
    /// </summary>
    public delegate Task OnSetChargingPoolAvailabilityStatusResponseDelegate(DateTime                                    LogTimestamp,
                                                                             DateTime                                    RequestTimestamp,
                                                                             ICPOClient                                  Sender,
                                                                             String                                      SenderId,
                                                                             EventTracking_Id                            EventTrackingId,

                                                                             Partner_Id                                  PartnerId,
                                                                             Operator_Id                                 OperatorId,
                                                                             ChargingPool_Id                             ChargingPoolId,
                                                                             DateTime                                    StatusEventDate,
                                                                             ChargingPoolAvailabilityStatusTypes         AvailabilityStatus,
                                                                             Transaction_Id?                             TransactionId,
                                                                             DateTime?                                   AvailabilityStatusUntil,
                                                                             String                                      AvailabilityStatusComment,

                                                                             TimeSpan                                    RequestTimeout,
                                                                             SetChargingPoolAvailabilityStatusResponse   Result,
                                                                             TimeSpan                                    Duration);

    #endregion

    #region OnSetChargingStationAvailabilityStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever a charging station availability status will be send upstream.
    /// </summary>
    public delegate Task OnSetChargingStationAvailabilityStatusRequestDelegate (DateTime                                       LogTimestamp,
                                                                                DateTime                                       RequestTimestamp,
                                                                                ICPOClient                                     Sender,
                                                                                String                                         SenderId,
                                                                                EventTracking_Id                               EventTrackingId,

                                                                                Partner_Id                                     PartnerId,
                                                                                Operator_Id                                    OperatorId,
                                                                                ChargingStation_Id                             ChargingStationId,
                                                                                DateTime                                       StatusEventDate,
                                                                                ChargingStationAvailabilityStatusTypes         AvailabilityStatus,
                                                                                Transaction_Id?                                TransactionId,
                                                                                DateTime?                                      AvailabilityStatusUntil,
                                                                                String                                         AvailabilityStatusComment,

                                                                                TimeSpan                                       RequestTimeout);

    /// <summary>
    /// A delegate called whenever a charging station availability status had been sent upstream.
    /// </summary>
    public delegate Task OnSetChargingStationAvailabilityStatusResponseDelegate(DateTime                                       LogTimestamp,
                                                                                DateTime                                       RequestTimestamp,
                                                                                ICPOClient                                     Sender,
                                                                                String                                         SenderId,
                                                                                EventTracking_Id                               EventTrackingId,

                                                                                Partner_Id                                     PartnerId,
                                                                                Operator_Id                                    OperatorId,
                                                                                ChargingStation_Id                             ChargingStationId,
                                                                                DateTime                                       StatusEventDate,
                                                                                ChargingStationAvailabilityStatusTypes         AvailabilityStatus,
                                                                                Transaction_Id?                                TransactionId,
                                                                                DateTime?                                      AvailabilityStatusUntil,
                                                                                String                                         AvailabilityStatusComment,

                                                                                TimeSpan                                       RequestTimeout,
                                                                                SetChargingStationAvailabilityStatusResponse   Result,
                                                                                TimeSpan                                       Duration);

    #endregion

    #region OnSetEVSEAvailabilityStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever an EVSE availability status will be send upstream.
    /// </summary>
    public delegate Task OnSetEVSEAvailabilityStatusRequestDelegate (DateTime                            LogTimestamp,
                                                                     DateTime                            RequestTimestamp,
                                                                     ICPOClient                          Sender,
                                                                     String                              SenderId,
                                                                     EventTracking_Id                    EventTrackingId,

                                                                     Partner_Id                          PartnerId,
                                                                     Operator_Id                         OperatorId,
                                                                     EVSE_Id                             EVSEId,
                                                                     DateTime                            StatusEventDate,
                                                                     EVSEAvailabilityStatusTypes         AvailabilityStatus,
                                                                     Transaction_Id?                     TransactionId,
                                                                     DateTime?                           AvailabilityStatusUntil,
                                                                     String                              AvailabilityStatusComment,

                                                                     TimeSpan                            RequestTimeout);

    /// <summary>
    /// A delegate called whenever an EVSE availability status had been sent upstream.
    /// </summary>
    public delegate Task OnSetEVSEAvailabilityStatusResponseDelegate(DateTime                            LogTimestamp,
                                                                     DateTime                            RequestTimestamp,
                                                                     ICPOClient                          Sender,
                                                                     String                              SenderId,
                                                                     EventTracking_Id                    EventTrackingId,

                                                                     Partner_Id                          PartnerId,
                                                                     Operator_Id                         OperatorId,
                                                                     EVSE_Id                             EVSEId,
                                                                     DateTime                            StatusEventDate,
                                                                     EVSEAvailabilityStatusTypes         AvailabilityStatus,
                                                                     Transaction_Id?                     TransactionId,
                                                                     DateTime?                           AvailabilityStatusUntil,
                                                                     String                              AvailabilityStatusComment,

                                                                     TimeSpan                            RequestTimeout,
                                                                     SetEVSEAvailabilityStatusResponse   Result,
                                                                     TimeSpan                            Duration);

    #endregion

    #region OnSetChargingConnectorAvailabilityStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever a charging connector availability status will be send upstream.
    /// </summary>
    public delegate Task OnSetChargingConnectorAvailabilityStatusRequestDelegate (DateTime                                         LogTimestamp,
                                                                                  DateTime                                         RequestTimestamp,
                                                                                  ICPOClient                                       Sender,
                                                                                  String                                           SenderId,
                                                                                  EventTracking_Id                                 EventTrackingId,

                                                                                  Partner_Id                                       PartnerId,
                                                                                  Operator_Id                                      OperatorId,
                                                                                  ChargingConnector_Id                             ChargingConnectorId,
                                                                                  DateTime                                         StatusEventDate,
                                                                                  ChargingConnectorAvailabilityStatusTypes         AvailabilityStatus,
                                                                                  Transaction_Id?                                  TransactionId,
                                                                                  DateTime?                                        AvailabilityStatusUntil,
                                                                                  String                                           AvailabilityStatusComment,

                                                                                  TimeSpan                                         RequestTimeout);

    /// <summary>
    /// A delegate called whenever a charging connector availability status had been sent upstream.
    /// </summary>
    public delegate Task OnSetChargingConnectorAvailabilityStatusResponseDelegate(DateTime                                         LogTimestamp,
                                                                                  DateTime                                         RequestTimestamp,
                                                                                  ICPOClient                                       Sender,
                                                                                  String                                           SenderId,
                                                                                  EventTracking_Id                                 EventTrackingId,

                                                                                  Partner_Id                                       PartnerId,
                                                                                  Operator_Id                                      OperatorId,
                                                                                  ChargingConnector_Id                             ChargingConnectorId,
                                                                                  DateTime                                         StatusEventDate,
                                                                                  ChargingConnectorAvailabilityStatusTypes         AvailabilityStatus,
                                                                                  Transaction_Id?                                  TransactionId,
                                                                                  DateTime?                                        AvailabilityStatusUntil,
                                                                                  String                                           AvailabilityStatusComment,

                                                                                  TimeSpan                                         RequestTimeout,
                                                                                  SetChargingConnectorAvailabilityStatusResponse   Result,
                                                                                  TimeSpan                                         Duration);

    #endregion


    #region OnSetEVSEBusyStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever an EVSE busy status will be send upstream.
    /// </summary>
    public delegate Task OnSetEVSEBusyStatusRequestDelegate (DateTime                                 LogTimestamp,
                                                             DateTime                                 RequestTimestamp,
                                                             ICPOClient                               Sender,
                                                             String                                   SenderId,
                                                             EventTracking_Id                         EventTrackingId,

                                                             Partner_Id                               PartnerId,
                                                             Operator_Id                              OperatorId,
                                                             EVSE_Id                                  EVSEId,
                                                             DateTime                                 StatusEventDate,
                                                             EVSEBusyStatusTypes                      BusyStatus,
                                                             Transaction_Id?                          TransactionId,
                                                             DateTime?                                BusyStatusUntil,
                                                             String                                   BusyStatusComment,

                                                             TimeSpan                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever an EVSE busy status had been sent upstream.
    /// </summary>
    public delegate Task OnSetEVSEBusyStatusResponseDelegate(DateTime                                 LogTimestamp,
                                                             DateTime                                 RequestTimestamp,
                                                             ICPOClient                               Sender,
                                                             String                                   SenderId,
                                                             EventTracking_Id                         EventTrackingId,

                                                             Partner_Id                               PartnerId,
                                                             Operator_Id                              OperatorId,
                                                             EVSE_Id                                  EVSEId,
                                                             DateTime                                 StatusEventDate,
                                                             EVSEBusyStatusTypes                      BusyStatus,
                                                             Transaction_Id?                          TransactionId,
                                                             DateTime?                                BusyStatusUntil,
                                                             String                                   BusyStatusComment,

                                                             TimeSpan                                 RequestTimeout,
                                                             SetEVSEBusyStatusResponse                Result,
                                                             TimeSpan                                 Duration);

    #endregion

    #region OnSetEVSESyntheticStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever an EVSE synthetic status will be send upstream.
    /// </summary>
    public delegate Task OnSetEVSESyntheticStatusRequestDelegate (DateTime                                 LogTimestamp,
                                                                  DateTime                                 RequestTimestamp,
                                                                  ICPOClient                               Sender,
                                                                  String                                   SenderId,
                                                                  EventTracking_Id                         EventTrackingId,

                                                                  Partner_Id                               PartnerId,
                                                                  Operator_Id                              OperatorId,
                                                                  EVSE_Id                                  EVSEId,
                                                                  Transaction_Id?                          TransactionId,
                                                                  DateTime?                                AvailabilityStatusEventDate,
                                                                  EVSEAvailabilityStatusTypes?             AvailabilityStatus,
                                                                  DateTime?                                AvailabilityStatusUntil,
                                                                  String                                   AvailabilityStatusComment,
                                                                  DateTime?                                BusyStatusEventDate,
                                                                  EVSEBusyStatusTypes?                     BusyStatus,
                                                                  DateTime?                                BusyStatusUntil,
                                                                  String                                   BusyStatusComment,

                                                                  TimeSpan                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever an EVSE synthetic status had been sent upstream.
    /// </summary>
    public delegate Task OnSetEVSESyntheticStatusResponseDelegate(DateTime                                 LogTimestamp,
                                                                  DateTime                                 RequestTimestamp,
                                                                  ICPOClient                               Sender,
                                                                  String                                   SenderId,
                                                                  EventTracking_Id                         EventTrackingId,

                                                                  Partner_Id                               PartnerId,
                                                                  Operator_Id                              OperatorId,
                                                                  EVSE_Id                                  EVSEId,
                                                                  Transaction_Id?                          TransactionId,
                                                                  DateTime?                                AvailabilityStatusEventDate,
                                                                  EVSEAvailabilityStatusTypes?             AvailabilityStatus,
                                                                  DateTime?                                AvailabilityStatusUntil,
                                                                  String                                   AvailabilityStatusComment,
                                                                  DateTime?                                BusyStatusEventDate,
                                                                  EVSEBusyStatusTypes?                     BusyStatus,
                                                                  DateTime?                                BusyStatusUntil,
                                                                  String                                   BusyStatusComment,

                                                                  TimeSpan                                 RequestTimeout,
                                                                  SetEVSESyntheticStatusResponse           Result,
                                                                  TimeSpan                                 Duration);

    #endregion


    #region OnGetServiceAuthorisationRequest/-Response

    /// <summary>
    /// A delegate called whenever a GetServiceAuthorisation request will be send upstream.
    /// </summary>
    public delegate Task OnGetServiceAuthorisationRequestDelegate (DateTime                                 LogTimestamp,
                                                                   DateTime                                 RequestTimestamp,
                                                                   ICPOClient                               Sender,
                                                                   String                                   SenderId,
                                                                   EventTracking_Id                         EventTrackingId,

                                                                   Partner_Id                               PartnerId,
                                                                   Operator_Id                              OperatorId,
                                                                   EVSE_Id                                  EVSEId,
                                                                   User_Id                                  UserId,
                                                                   Service_Id                               RequestedServiceId,
                                                                   Transaction_Id?                          TransactionId,
                                                                   PartnerServiceSession_Id?                PartnerServiceSessionId,

                                                                   TimeSpan                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever a GetServiceAuthorisation request had been sent upstream.
    /// </summary>
    public delegate Task OnGetServiceAuthorisationResponseDelegate(DateTime                                 LogTimestamp,
                                                                   DateTime                                 RequestTimestamp,
                                                                   ICPOClient                               Sender,
                                                                   String                                   SenderId,
                                                                   EventTracking_Id                         EventTrackingId,

                                                                   Partner_Id                               PartnerId,
                                                                   Operator_Id                              OperatorId,
                                                                   EVSE_Id                                  EVSEId,
                                                                   User_Id                                  UserId,
                                                                   Service_Id                               RequestedServiceId,
                                                                   Transaction_Id?                          TransactionId,
                                                                   PartnerServiceSession_Id?                PartnerServiceSessionId,

                                                                   TimeSpan                                 RequestTimeout,
                                                                   GetServiceAuthorisationResponse          Result,
                                                                   TimeSpan                                 Duration);

    #endregion

    #region OnSetSessionEventReportRequest/-Response

    /// <summary>
    /// A delegate called whenever a SetSessionEventReport request will be send upstream.
    /// </summary>
    public delegate Task OnSetSessionEventReportRequestDelegate (DateTime                        LogTimestamp,
                                                                 DateTime                        RequestTimestamp,
                                                                 ICPOClient                      Sender,
                                                                 String                          SenderId,
                                                                 EventTracking_Id                EventTrackingId,

                                                                 Partner_Id                      PartnerId,
                                                                 Operator_Id                     OperatorId,
                                                                 ServiceSession_Id               ServiceSessionId,
                                                                 SessionEvent                    SessionEvent,

                                                                 Transaction_Id?                 TransactionIdl,
                                                                 PartnerServiceSession_Id?       ExecPartnerSessionId,

                                                                 TimeSpan                        RequestTimeout);

    /// <summary>
    /// A delegate called whenever a SetSessionEventReport request had been sent upstream.
    /// </summary>
    public delegate Task OnSetSessionEventReportResponseDelegate(DateTime                        LogTimestamp,
                                                                 DateTime                        RequestTimestamp,
                                                                 ICPOClient                      Sender,
                                                                 String                          SenderId,
                                                                 EventTracking_Id                EventTrackingId,

                                                                 Partner_Id                      PartnerId,
                                                                 Operator_Id                     OperatorId,
                                                                 ServiceSession_Id               ServiceSessionId,
                                                                 SessionEvent                    SessionEvent,

                                                                 Transaction_Id?                 TransactionIdl,
                                                                 PartnerServiceSession_Id?       ExecPartnerSessionId,

                                                                 TimeSpan                        RequestTimeout,
                                                                 SetSessionEventReportResponse   Result,
                                                                 TimeSpan                        Duration);

    #endregion


    #region OnSetChargeDetailRecordRequest/-Response

    /// <summary>
    /// A delegate called whenever new charge detail record will be send upstream.
    /// </summary>
    public delegate Task OnSetChargeDetailRecordRequestDelegate (DateTime                                 LogTimestamp,
                                                                 DateTime                                 RequestTimestamp,
                                                                 ICPOClient                               Sender,
                                                                 String                                   SenderId,
                                                                 EventTracking_Id                         EventTrackingId,

                                                                 Partner_Id                               PartnerId,
                                                                 Operator_Id                              OperatorId,
                                                                 ChargeDetailRecord                       ChargeDetailRecord,
                                                                 Transaction_Id?                          TransactionId,

                                                                 TimeSpan                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever a charge detail record had been sent upstream.
    /// </summary>
    public delegate Task OnSetChargeDetailRecordResponseDelegate(DateTime                                 LogTimestamp,
                                                                 DateTime                                 RequestTimestamp,
                                                                 ICPOClient                               Sender,
                                                                 String                                   SenderId,
                                                                 EventTracking_Id                         EventTrackingId,

                                                                 Partner_Id                               PartnerId,
                                                                 Operator_Id                              OperatorId,
                                                                 ChargeDetailRecord                       ChargeDetailRecord,
                                                                 Transaction_Id?                          TransactionId,

                                                                 TimeSpan                                 RequestTimeout,
                                                                 SetChargeDetailRecordResponse            Result,
                                                                 TimeSpan                                 Duration);

    #endregion

}
