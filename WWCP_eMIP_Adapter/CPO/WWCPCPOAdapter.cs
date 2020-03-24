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
using System.Linq;
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using Org.BouncyCastle.Crypto.Parameters;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A WWCP wrapper for the eMIP CPO Roaming client which maps
    /// WWCP data structures onto eMIP data structures and vice versa.
    /// </summary>
    public class WWCPCPOAdapter : AWWCPCSOAdapter<ChargeDetailRecord>,
                                  ICSORoamingProvider,
                                  IEquatable <WWCPCPOAdapter>,
                                  IComparable<WWCPCPOAdapter>,
                                  IComparable
    {

        #region Data

        //private        readonly  EVSE2EVSEDataRecordDelegate                            _EVSE2EVSEDataRecord;

        //private        readonly  EVSEStatusUpdate2EVSEStatusRecordDelegate              _EVSEStatusUpdate2EVSEStatusRecord;

        private        readonly  WWCPChargeDetailRecord2ChargeDetailRecordDelegate    _WWCPChargeDetailRecord2eMIPChargeDetailRecord;

        //private        readonly  EVSEDataRecord2XMLDelegate                             _EVSEDataRecord2XML;

        //private        readonly  EVSEStatusRecord2XMLDelegate                           _EVSEStatusRecord2XML;

        //private        readonly  ChargeDetailRecord2XMLDelegate                         _ChargeDetailRecord2XML;

        private static readonly  Regex                                                pattern                             = new Regex(@"\s=\s");

        /// <summary>
        /// The default send heartbeats intervall.
        /// </summary>
        public  readonly static  TimeSpan                                             DefaultSendHeartbeatsEvery          = TimeSpan.FromMinutes(5);

        private readonly         SemaphoreSlim                                        SendHeartbeatLock                   = new SemaphoreSlim(1, 1);
        private readonly         Timer                                                SendHeartbeatsTimer;

        private                  UInt64                                               _SendHeartbeatsRunId                = 1;

        #endregion

        #region Properties

        IId ISendAuthorizeStartStop.AuthId
            => Id;

        IId ISendChargeDetailRecords.Id
            => Id;

        IEnumerable<IId> ISendChargeDetailRecords.Ids
            => Ids.Cast<IId>();

        /// <summary>
        /// The wrapped CPO roaming object.
        /// </summary>
        public CPORoaming CPORoaming { get; }


        /// <summary>
        /// The CPO client.
        /// </summary>
        public CPOClient CPOClient
            => CPORoaming?.CPOClient;

        /// <summary>
        /// The CPO client logger.
        /// </summary>
        public CPOClient.CPOClientLogger ClientLogger
            => CPORoaming?.CPOClient?.Logger;


        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServer CPOServer
            => CPORoaming?.CPOServer;

        /// <summary>
        /// The CPO server logger.
        /// </summary>
        public CPOServerLogger ServerLogger
            => CPORoaming?.CPOServerLogger;



        /// <summary>
        /// The partner identification.
        /// </summary>
        public Partner_Id  PartnerId { get; }


        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean     DisableSendHeartbeats             { get; set; }

        public TimeSpan    SendHeartbeatsEvery               { get; }


        protected readonly CustomOperatorIdMapperDelegate  CustomOperatorIdMapper;

        protected readonly CustomEVSEIdMapperDelegate      CustomEVSEIdMapper;

        public Func<WWCP.ChargeDetailRecord, ChargeDetailRecordFilters> ChargeDetailRecordFilter { get; set; }

        #endregion

        #region Events

        // Client logging...

        #region OnPushEVSEDataWWCPRequest/-Response

        ///// <summary>
        ///// An event fired whenever new EVSE data will be send upstream.
        ///// </summary>
        //public event OnPushEVSEDataWWCPRequestDelegate   OnPushEVSEDataWWCPRequest;

        ///// <summary>
        ///// An event fired whenever new EVSE data had been sent upstream.
        ///// </summary>
        //public event OnPushEVSEDataWWCPResponseDelegate  OnPushEVSEDataWWCPResponse;

        #endregion

        #region OnPushEVSEStatusWWCPRequest/-Response

        ///// <summary>
        ///// An event fired whenever new EVSE status will be send upstream.
        ///// </summary>
        //public event OnPushEVSEStatusWWCPRequestDelegate   OnPushEVSEStatusWWCPRequest;

        ///// <summary>
        ///// An event fired whenever new EVSE status had been sent upstream.
        ///// </summary>
        //public event OnPushEVSEStatusWWCPResponseDelegate  OnPushEVSEStatusWWCPResponse;

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

        #endregion

        #region OnSendCDRRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record was enqueued for later sending upstream.
        /// </summary>
        public event OnSendCDRsRequestDelegate   OnEnqueueSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record will be send upstream.
        /// </summary>
        public event OnSendCDRsRequestDelegate   OnSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record had been sent upstream.
        /// </summary>
        public event OnSendCDRsResponseDelegate  OnSendCDRsResponse;

        #endregion


        #region SendHeartbeat

        public delegate void SendHeartbeatStartedDelegate(WWCPCPOAdapter Sender, DateTime StartTime, TimeSpan Every, UInt64 RunId);

        public event SendHeartbeatStartedDelegate SendHeartbeatStartedEvent;


        public delegate void SendHeartbeatFinishedDelegate(WWCPCPOAdapter Sender, DateTime StartTime, DateTime EndTime, TimeSpan Runtime, TimeSpan Every, UInt64 RunId);

        public event SendHeartbeatFinishedDelegate SendHeartbeatFinishedEvent;

        #endregion

        #endregion

        #region Constructor(s)

        #region WWCPCPOAdapter(Id, Name, RoamingNetwork, CPORoaming, EVSE2EVSEDataRecord = null)

        /// <summary>
        /// Create a new WWCP wrapper for the eMIP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="Description">An optional (multi-language) description of the charging station operator roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="PartnerId">The unique identification of an eMIP communication partner.</param>
        /// <param name="CPORoaming">A eMIP CPO roaming object to be mapped to WWCP.</param>
        /// 
        /// <param name="IncludeEVSEIds">Only include EVSE identificators matching the given delegate.</param>
        /// <param name="IncludeEVSEs">Only include EVSEs matching the given delegate.</param>
        /// <param name="CustomOperatorIdMapper">A delegate to customize the mapping of operator identifications.</param>
        /// <param name="CustomEVSEIdMapper">A delegate to customize the mapping of EVSE identifications.</param>
        /// 
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="WWCPChargeDetailRecord2eMIPChargeDetailRecord">A delegate to process a charge detail record, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="SendHeartbeatsEvery">The heartbeat intervall.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// <param name="CDRCheckEvery">The charge detail record intervall.</param>
        /// 
        /// <param name="DisableSendHeartbeats">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="DNSClient">The attached DNS service.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id                              Id,
                              I18NString                                         Name,
                              I18NString                                         Description,
                              RoamingNetwork                                     RoamingNetwork,

                              Partner_Id                                         PartnerId,
                              CPORoaming                                         CPORoaming,

                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,
                              CustomOperatorIdMapperDelegate                     CustomOperatorIdMapper                          = null,
                              CustomEVSEIdMapperDelegate                         CustomEVSEIdMapper                              = null,

                              //EVSE2EVSEDataRecordDelegate                        EVSE2EVSEDataRecord                             = null,
                              //EVSEStatusUpdate2EVSEStatusRecordDelegate          EVSEStatusUpdate2EVSEStatusRecord               = null,
                              WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2eMIPChargeDetailRecord   = null,

                              TimeSpan?                                          SendHeartbeatsEvery                             = null,
                              TimeSpan?                                          ServiceCheckEvery                               = null,
                              TimeSpan?                                          StatusCheckEvery                                = null,
                              TimeSpan?                                          CDRCheckEvery                                   = null,

                              Boolean                                            DisableSendHeartbeats                           = false,
                              Boolean                                            DisablePushData                                 = false,
                              Boolean                                            DisablePushStatus                               = false,
                              Boolean                                            DisableAuthentication                           = false,
                              Boolean                                            DisableSendChargeDetailRecords                  = false,

                              String                                             EllipticCurve                                   = "P-256",
                              ECPrivateKeyParameters                             PrivateKey                                      = null,
                              PublicKeyCertificates                              PublicKeyCertificates                           = null,

                              DNSClient                                          DNSClient                                       = null)

            : base(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   IncludeEVSEIds,
                   IncludeEVSEs,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,

                   DNSClient ?? CPORoaming?.DNSClient)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),        "The given roaming provider name must not be null or empty!");

            if (CPORoaming == null)
                throw new ArgumentNullException(nameof(CPORoaming),  "The given eMIP CPO Roaming object must not be null!");

            #endregion

            this.PartnerId                                       = PartnerId;
            this.CPORoaming                                      = CPORoaming;

            //this._EVSE2EVSEDataRecord                            = EVSE2EVSEDataRecord;
            //this._EVSEStatusUpdate2EVSEStatusRecord              = EVSEStatusUpdate2EVSEStatusRecord;
            this._WWCPChargeDetailRecord2eMIPChargeDetailRecord  = WWCPChargeDetailRecord2eMIPChargeDetailRecord;

            //this.eMIP_ChargeDetailRecords_Queue                  = new List<ChargeDetailRecord>();

            this.SendHeartbeatsEvery                             = SendHeartbeatsEvery ?? DefaultSendHeartbeatsEvery;
            this.SendHeartbeatsTimer                             = new Timer(SendHeartbeat, null, this.SendHeartbeatsEvery, this.SendHeartbeatsEvery);
            this.DisableSendHeartbeats                           = DisableSendHeartbeats;

            this.CustomOperatorIdMapper                          = CustomOperatorIdMapper;
            this.CustomEVSEIdMapper                              = CustomEVSEIdMapper;


            // Link incoming eMIP events...

            #region OnSetServiceAuthorisation

            this.CPORoaming.OnSetServiceAuthorisation += async (Timestamp,
                                                                Sender,
                                                                Request) => {

                // bookingId
                // meterLimitList

                #region Request mapping

                var chargingProduct = new ChargingProduct(Id:                ChargingProduct_Id.Random(),
                                                          IntermediateCDRs:  Request.IntermediateCDRRequested);

                //ChargingReservation_Id? ReservationId      = null;
                //TimeSpan?               MinDuration        = null;
                //Single?                 PlannedEnergy      = null;
                //ChargingProduct_Id?     ProductId          = ChargingProduct_Id.Parse("AC1");
                //ChargingProduct         ChargingProduct    = null;
                //PartnerProduct_Id?      PartnerProductId   = Request.PartnerProductId;

                //if (PartnerProductId != null && PartnerProductId.ToString().IsNotNullOrEmpty())
                //{

                //    // The PartnerProductId is a simple string...
                //    if (!PartnerProductId.Value.ToString().Contains("="))
                //    {
                //        ChargingProduct = new ChargingProduct(
                //                              ChargingProduct_Id.Parse(PartnerProductId.Value.ToString())
                //                          );
                //    }

                //    else
                //    {

                //        var ProductIdElements = PartnerProductId.ToString().DoubleSplit('|', '=');

                //        if (ProductIdElements.Any())
                //        {

                //            if (ProductIdElements.ContainsKey("R") &&
                //                ChargingReservation_Id.TryParse(Request.EVSEId.OperatorId.ToWWCP().Value,
                //                                                ProductIdElements["R"],
                //                                                out ChargingReservation_Id _ReservationId))
                //                ReservationId = _ReservationId;


                //            if (ProductIdElements.ContainsKey("D"))
                //            {

                //                var MinDurationText = ProductIdElements["D"];

                //                if (MinDurationText.EndsWith("sec", StringComparison.InvariantCulture))
                //                    MinDuration = TimeSpan.FromSeconds(UInt32.Parse(MinDurationText.Substring(0, MinDurationText.Length - 3)));

                //                if (MinDurationText.EndsWith("min", StringComparison.InvariantCulture))
                //                    MinDuration = TimeSpan.FromMinutes(UInt32.Parse(MinDurationText.Substring(0, MinDurationText.Length - 3)));

                //            }


                //            if (ProductIdElements.ContainsKey("E") &&
                //                Single.TryParse(ProductIdElements["E"], out Single _PlannedEnergy))
                //                PlannedEnergy = _PlannedEnergy;


                //            if (ProductIdElements.ContainsKey("P") &&
                //                ChargingProduct_Id.TryParse(ProductIdElements["P"], out ChargingProduct_Id _ProductId))
                //                ProductId = _ProductId;


                //            ChargingProduct = new ChargingProduct(
                //                                      ProductId.Value,
                //                                      MinDuration
                //                                  );

                //        }

                //    }

                //}

                #endregion

                var EVSEId = Request.EVSEId.ToWWCP();

                if (!EVSEId.HasValue)
                    return new SetServiceAuthorisationResponse(
                               Request,
                               Request.TransactionId ?? Transaction_Id.Zero,
                               RequestStatus.EVSENotReachable
                           );

                var response = await RoamingNetwork.
                                         RemoteStart(CSORoamingProvider:    this,
                                                     ChargingLocation:      ChargingLocation.FromEVSEId(EVSEId.Value),
                                                     ChargingProduct:       chargingProduct,
                                                     ReservationId:         null,
                                                     SessionId:             Request.ServiceSessionId.ToWWCP(),
                                                     ProviderId:            Request.PartnerId.       ToWWCP_ProviderId(),
                                                     RemoteAuthentication:  Request.UserId.          ToWWCP(),

                                                     Timestamp:             Request.Timestamp,
                                                     CancellationToken:     Request.CancellationToken,
                                                     EventTrackingId:       Request.EventTrackingId,
                                                     RequestTimeout:        Request.RequestTimeout).
                                         ConfigureAwait(false);


                if (response.Session != null)
                {

                    var Gireve = response.Session.AddJSON("Gireve");

                    if (Request.UserContractIdAlias.HasValue)
                        response.Session.SetJSON("Gireve", "userContractIdAlias",  Request);


                    if (Request.UserContractIdAlias.HasValue)
                        response.Session.SetJSON("Gireve", "userContractIdAlias",  Request.UserContractIdAlias);

                    if (Request.Parameter.IsNotNullOrEmpty())
                        response.Session.SetJSON("Gireve", "parameter",            Request.Parameter);

                    if (Request.HTTPRequest != null)
                    {

                        response.Session.SetJSON("Gireve", "remoteIPAddress",      Request.HTTPRequest.RemoteSocket.IPAddress.ToString());

                        if (Request.HTTPRequest.X_Real_IP       != null)
                            response.Session.SetJSON("Gireve", "realIP",           Request.HTTPRequest.X_Real_IP.ToString());

                        if (Request.HTTPRequest.X_Forwarded_For != null)
                            response.Session.SetJSON("Gireve", "forwardedFor",     new JArray(Request.HTTPRequest.X_Forwarded_For.Select(addr => addr.ToString()).AggregateWith(',')));

                    }

                }

                #region Response mapping

                                                                    if (response != null)
                {
                    switch (response.Result)
                    {

                        case RemoteStartResultTypes.Success:
                            return new SetServiceAuthorisationResponse(
                                       Request,
                                       Request.TransactionId ?? Transaction_Id.Zero,
                                       RequestStatus.Ok
                                   );

                        //case RemoteStartResultType.InvalidSessionId:
                        //case RemoteStartResultType.InvalidCredentials:

                        case RemoteStartResultTypes.Offline:
                        case RemoteStartResultTypes.Timeout:
                        case RemoteStartResultTypes.OutOfService:
                        case RemoteStartResultTypes.CommunicationError:
                            return new SetServiceAuthorisationResponse(
                                       Request,
                                       Request.TransactionId ?? Transaction_Id.Zero,
                                       RequestStatus.EVSENotReachable
                                   );

                        case RemoteStartResultTypes.Reserved:
                        case RemoteStartResultTypes.AlreadyInUse:
                            return new SetServiceAuthorisationResponse(
                                       Request,
                                       Request.TransactionId ?? Transaction_Id.Zero,
                                       RequestStatus.EVSEServiceNotAvailable
                                   );

                        case RemoteStartResultTypes.UnknownLocation:
                            return new SetServiceAuthorisationResponse(
                                       Request,
                                       Request.TransactionId ?? Transaction_Id.Zero,
                                       RequestStatus.EVSEServiceNotAvailable
                                   );

                    }
                }

                // ServiceNotAvailable
                return new SetServiceAuthorisationResponse(
                           Request,
                           Request.TransactionId ?? Transaction_Id.Zero,
                           RequestStatus.ServiceNotAvailable
                       );

                #endregion

            };

            #endregion

            #region OnSetSessionAction

            this.CPORoaming.OnSetSessionAction += async (Timestamp,
                                                         Sender,
                                                         Request) => {

                #region Request mapping

                //ChargingReservation_Id? ReservationId      = null;
                //TimeSpan?               MinDuration        = null;
                //Single?                 PlannedEnergy      = null;
                //ChargingProduct_Id?     ProductId          = ChargingProduct_Id.Parse("AC1");
                //ChargingProduct         ChargingProduct    = null;
                //PartnerProduct_Id?      PartnerProductId   = Request.PartnerProductId;

                //if (PartnerProductId != null && PartnerProductId.ToString().IsNotNullOrEmpty())
                //{

                //    // The PartnerProductId is a simple string...
                //    if (!PartnerProductId.Value.ToString().Contains("="))
                //    {
                //        ChargingProduct = new ChargingProduct(
                //                              ChargingProduct_Id.Parse(PartnerProductId.Value.ToString())
                //                          );
                //    }

                //    else
                //    {

                //        var ProductIdElements = PartnerProductId.ToString().DoubleSplit('|', '=');

                //        if (ProductIdElements.Any())
                //        {

                //            if (ProductIdElements.ContainsKey("R") &&
                //                ChargingReservation_Id.TryParse(Request.EVSEId.OperatorId.ToWWCP().Value,
                //                                                ProductIdElements["R"],
                //                                                out ChargingReservation_Id _ReservationId))
                //                ReservationId = _ReservationId;


                //            if (ProductIdElements.ContainsKey("D"))
                //            {

                //                var MinDurationText = ProductIdElements["D"];

                //                if (MinDurationText.EndsWith("sec", StringComparison.InvariantCulture))
                //                    MinDuration = TimeSpan.FromSeconds(UInt32.Parse(MinDurationText.Substring(0, MinDurationText.Length - 3)));

                //                if (MinDurationText.EndsWith("min", StringComparison.InvariantCulture))
                //                    MinDuration = TimeSpan.FromMinutes(UInt32.Parse(MinDurationText.Substring(0, MinDurationText.Length - 3)));

                //            }


                //            if (ProductIdElements.ContainsKey("E") &&
                //                Single.TryParse(ProductIdElements["E"], out Single _PlannedEnergy))
                //                PlannedEnergy = _PlannedEnergy;


                //            if (ProductIdElements.ContainsKey("P") &&
                //                ChargingProduct_Id.TryParse(ProductIdElements["P"], out ChargingProduct_Id _ProductId))
                //                ProductId = _ProductId;


                //            ChargingProduct = new ChargingProduct(
                //                                      ProductId.Value,
                //                                      MinDuration
                //                                  );

                //        }

                //    }

                //}

                #endregion

                var ServiceSessionId      = Request.ServiceSessionId;
                var execPartnerSessionId  = Request.ExecPartnerSessionId;
                var SessionAction         = Request.SessionAction;

                // Nature 0: Emergency Stop
                // Nature 1: Stop and terminate current operation
                // Nature 2: Suspend current operation
                // Nature 3: Restart current operation
                if (Request.SessionAction.Nature == SessionActionNatures.EmergencyStop ||
                    Request.SessionAction.Nature == SessionActionNatures.Stop)
                {

                    var response = await RoamingNetwork.
                                             RemoteStop(CSORoamingProvider:    this,
                                                        SessionId:             Request.ServiceSessionId.ToWWCP(),
                                                        ReservationHandling:   ReservationHandling.Close,
                                                        ProviderId:            null,
                                                        RemoteAuthentication:  null,

                                                        Timestamp:             Request.Timestamp,
                                                        CancellationToken:     Request.CancellationToken,
                                                        EventTrackingId:       Request.EventTrackingId,
                                                        RequestTimeout:        Request.RequestTimeout).
                                             ConfigureAwait(false);

                    #region Response mapping

                    if (response != null)
                    {
                        switch (response.Result)
                        {

                            case RemoteStopResultTypes.Success:
                                return new SetSessionActionRequestResponse(
                                           Request,
                                           Request.TransactionId ?? Transaction_Id.Zero,
                                           RequestStatus.Ok
                                       );

                            case RemoteStopResultTypes.AlreadyStopped:
                                return new SetSessionActionRequestResponse(
                                           Request,
                                           Request.TransactionId ?? Transaction_Id.Zero,
                                           RequestStatus.Ok
                                       );

                            case RemoteStopResultTypes.InvalidSessionId:
                                return new SetSessionActionRequestResponse(
                                           Request,
                                           Request.TransactionId ?? Transaction_Id.Zero,
                                           RequestStatus.SessionNotFound
                                       );

                            case RemoteStopResultTypes.Offline:
                            case RemoteStopResultTypes.Timeout:
                            case RemoteStopResultTypes.OutOfService:
                            case RemoteStopResultTypes.CommunicationError:
                                return new SetSessionActionRequestResponse(
                                           Request,
                                           Request.TransactionId ?? Transaction_Id.Zero,
                                           RequestStatus.EVSENotReachable
                                       );

                        }
                    }

                    #endregion

                }

                // CPOorEMSP_DoesNotRecogniseActionOrEventNature
                return new SetSessionActionRequestResponse(
                           Request,
                           Request.TransactionId ?? Transaction_Id.Zero,
                           RequestStatus.CPOorEMSP_DoesNotRecogniseActionOrEventNature
                       );

            };

            #endregion

        }

        #endregion

        #region WWCPCPOAdapter(Id, Name, RoamingNetwork, CPOClient, CPOServer, EVSEDataRecordProcessing = null)

        /// <summary>
        /// Create a new WWCP wrapper for the eMIP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="Description">An optional (multi-language) description of the charging station operator roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="PartnerId">The unique identification of an eMIP communication partner.</param>
        /// <param name="CPOClient">An eMIP CPO client.</param>
        /// <param name="CPOServer">An eMIP CPO sever.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="IncludeEVSEIds">Only include the EVSE matching the given delegate.</param>
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="CustomEVSEIdMapper">A delegate to customize the mapping of EVSE identifications.</param>
        /// 
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="WWCPChargeDetailRecord2eMIPChargeDetailRecord">A delegate to process a charge detail record, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="SendHeartbeatsEvery">The heartbeat intervall.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// <param name="CDRCheckEvery">The charge detail record intervall.</param>
        /// 
        /// <param name="DisableSendHeartbeats">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id                              Id,
                              I18NString                                         Name,
                              I18NString                                         Description,
                              RoamingNetwork                                     RoamingNetwork,

                              Partner_Id                                         PartnerId,
                              CPOClient                                          CPOClient,
                              CPOServer                                          CPOServer,
                              String                                             ServerLoggingContext                            = CPOServerLogger.DefaultContext,
                              LogfileCreatorDelegate                             LogfileCreator                                  = null,

                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,
                              CustomOperatorIdMapperDelegate                     CustomOperatorIdMapper                          = null,
                              CustomEVSEIdMapperDelegate                         CustomEVSEIdMapper                              = null,

                              //EVSE2EVSEDataRecordDelegate                        EVSE2EVSEDataRecord                             = null,
                              //EVSEStatusUpdate2EVSEStatusRecordDelegate          EVSEStatusUpdate2EVSEStatusRecord               = null,
                              WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2eMIPChargeDetailRecord   = null,

                              TimeSpan?                                          SendHeartbeatsEvery                             = null,
                              TimeSpan?                                          ServiceCheckEvery                               = null,
                              TimeSpan?                                          StatusCheckEvery                                = null,
                              TimeSpan?                                          CDRCheckEvery                                   = null,

                              Boolean                                            DisableSendHeartbeats                           = false,
                              Boolean                                            DisablePushData                                 = false,
                              Boolean                                            DisablePushStatus                               = false,
                              Boolean                                            DisableAuthentication                           = false,
                              Boolean                                            DisableSendChargeDetailRecords                  = false,

                              String                                             EllipticCurve                                   = "P-256",
                              ECPrivateKeyParameters                             PrivateKey                                      = null,
                              PublicKeyCertificates                              PublicKeyCertificates                           = null,

                              DNSClient                                          DNSClient                                       = null)

            : this(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   PartnerId,
                   new CPORoaming(CPOClient,
                                  CPOServer,
                                  ServerLoggingContext,
                                  LogfileCreator),

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   CustomOperatorIdMapper,
                   CustomEVSEIdMapper,

                   //EVSE2EVSEDataRecord,
                   //EVSEStatusUpdate2EVSEStatusRecord,
                   WWCPChargeDetailRecord2eMIPChargeDetailRecord,

                   SendHeartbeatsEvery,
                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisableSendHeartbeats,
                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,

                   DNSClient ?? CPOServer?.DNSClient)

        { }

        #endregion

        #region WWCPCPOAdapter(Id, Name, RoamingNetwork, RemoteHostName, ...)

        /// <summary>
        /// Create a new WWCP wrapper for the eMIP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="Description">An optional (multi-language) description of the charging station operator roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="PartnerId">The unique identification of an eMIP communication partner.</param>
        /// 
        /// <param name="RemoteHostname">The hostname of the remote eMIP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote eMIP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote eMIP service.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerAuthorisationURI">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="IncludeEVSEIds">Only include the EVSE matching the given delegate.</param>
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="CustomEVSEIdMapper">A delegate to customize the mapping of EVSE identifications.</param>
        /// 
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="WWCPChargeDetailRecord2eMIPChargeDetailRecord">A delegate to process a charge detail record, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="SendHeartbeatsEvery">The heartbeat intervall.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// <param name="CDRCheckEvery">The charge detail record intervall.</param>
        /// 
        /// <param name="DisableSendHeartbeats">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id                              Id,
                              I18NString                                         Name,
                              I18NString                                         Description,
                              RoamingNetwork                                     RoamingNetwork,

                              Partner_Id                                         PartnerId,

                              HTTPHostname                                       RemoteHostname,
                              IPPort?                                            RemoteTCPPort                                            = null,
                              RemoteCertificateValidationCallback                RemoteCertificateValidator                               = null,
                              LocalCertificateSelectionCallback                  ClientCertificateSelector                                = null,
                              HTTPHostname?                                      RemoteHTTPVirtualHost                                    = null,
                              HTTPPath?                                          URIPrefix                                                = null,
                              String                                             HTTPUserAgent                                            = CPOClient.DefaultHTTPUserAgent,
                              TimeSpan?                                          RequestTimeout                                           = null,
                              TransmissionRetryDelayDelegate                     TransmissionRetryDelay                                   = null,
                              Byte?                                              MaxNumberOfRetries                                       = CPOClient.DefaultMaxNumberOfRetries,

                              String                                             ServerName                                               = CPOServer.DefaultHTTPServerName,
                              String                                             ServiceId                                                = null,
                              IPPort?                                            ServerTCPPort                                            = null,
                              HTTPPath?                                          ServerURIPrefix                                          = null,
                              String                                             ServerAuthorisationURI                                   = CPOServer.DefaultAuthorisationURI,
                              HTTPContentType                                    ServerContentType                                        = null,
                              Boolean                                            ServerRegisterHTTPRootService                            = true,
                              Boolean                                            ServerAutoStart                                          = false,

                              String                                             ClientLoggingContext                                     = CPOClient.CPOClientLogger.DefaultContext,
                              String                                             ServerLoggingContext                                     = CPOServerLogger.DefaultContext,
                              LogfileCreatorDelegate                             LogfileCreator                                           = null,

                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                           = null,
                              IncludeEVSEDelegate                                IncludeEVSEs                                             = null,
                              CustomOperatorIdMapperDelegate                     CustomOperatorIdMapper                                   = null,
                              CustomEVSEIdMapperDelegate                         CustomEVSEIdMapper                                       = null,

                              //EVSE2EVSEDataRecordDelegate                        EVSE2EVSEDataRecord                                      = null,
                              //EVSEStatusUpdate2EVSEStatusRecordDelegate          EVSEStatusUpdate2EVSEStatusRecord                        = null,
                              WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2eMIPChargeDetailRecord            = null,

                              TimeSpan?                                          SendHeartbeatsEvery                                      = null,
                              TimeSpan?                                          ServiceCheckEvery                                        = null,
                              TimeSpan?                                          StatusCheckEvery                                         = null,
                              TimeSpan?                                          CDRCheckEvery                                            = null,

                              Boolean                                            DisableSendHeartbeats                                    = false,
                              Boolean                                            DisablePushData                                          = false,
                              Boolean                                            DisablePushStatus                                        = false,
                              Boolean                                            DisableAuthentication                                    = false,
                              Boolean                                            DisableSendChargeDetailRecords                           = false,

                              String                                             EllipticCurve                                            = "P-256",
                              ECPrivateKeyParameters                             PrivateKey                                               = null,
                              PublicKeyCertificates                              PublicKeyCertificates                                    = null,

                              CounterValues?                                     CPOClientSendHeartbeatCounter                            = null,
                              CounterValues?                                     CPOClientSetChargingPoolAvailabilityStatusCounter        = null,
                              CounterValues?                                     CPOClientSetChargingStationAvailabilityStatusCounter     = null,
                              CounterValues?                                     CPOClientSetEVSEAvailabilityStatusCounter                = null,
                              CounterValues?                                     CPOClientSetChargingConnectorAvailabilityStatusCounter   = null,
                              CounterValues?                                     CPOClientSetEVSEBusyStatusCounter                        = null,
                              CounterValues?                                     CPOClientSetEVSESyntheticStatusCounter                   = null,
                              CounterValues?                                     CPOClientGetServiceAuthorisationCounter                  = null,
                              CounterValues?                                     CPOClientSetSessionEventReportCounter                    = null,
                              CounterValues?                                     CPOClientSetChargeDetailRecordCounter                    = null,

                              DNSClient                                          DNSClient                                                = null)

            : this(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   PartnerId,
                   new CPORoaming(Id.ToString(),
                                  RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteCertificateValidator,
                                  ClientCertificateSelector,
                                  RemoteHTTPVirtualHost,
                                  URIPrefix ?? CPOClient.DefaultURIPrefix,
                                  HTTPUserAgent,
                                  RequestTimeout,
                                  TransmissionRetryDelay,
                                  MaxNumberOfRetries,

                                  ServerName,
                                  ServiceId,
                                  ServerTCPPort,
                                  ServerURIPrefix        ?? CPOServer.DefaultURIPrefix,
                                  ServerAuthorisationURI ?? CPOServer.DefaultAuthorisationURI,
                                  ServerContentType,
                                  ServerRegisterHTTPRootService,
                                  false,

                                  ClientLoggingContext,
                                  ServerLoggingContext,
                                  LogfileCreator,

                                  CPOClientSendHeartbeatCounter,
                                  CPOClientSetChargingPoolAvailabilityStatusCounter,
                                  CPOClientSetChargingStationAvailabilityStatusCounter,
                                  CPOClientSetEVSEAvailabilityStatusCounter,
                                  CPOClientSetChargingConnectorAvailabilityStatusCounter,
                                  CPOClientSetEVSEBusyStatusCounter,
                                  CPOClientSetEVSESyntheticStatusCounter,
                                  CPOClientGetServiceAuthorisationCounter,
                                  CPOClientSetSessionEventReportCounter,
                                  CPOClientSetChargeDetailRecordCounter,

                                  DNSClient),

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   CustomOperatorIdMapper,
                   CustomEVSEIdMapper,

                   //EVSE2EVSEDataRecord,
                   //EVSEStatusUpdate2EVSEStatusRecord,
                   WWCPChargeDetailRecord2eMIPChargeDetailRecord,

                   SendHeartbeatsEvery,
                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisableSendHeartbeats,
                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,

                   DNSClient)

        {

            if (ServerAutoStart)
                CPOServer.Start();

        }

        #endregion

        #endregion


        // RN -> External service requests...

        #region PushEVSEData/-Status directly...

        #region (private) PushEVSEData  (EVSEs,             ServerAction, ...)

        /// <summary>
        /// Upload the EVSE data of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        private async Task<PushEVSEDataResult>

            PushEVSEData(IEnumerable<EVSE>    EVSEs,

                         DateTime?            Timestamp          = null,
                         CancellationToken?   CancellationToken  = null,
                         EventTracking_Id     EventTrackingId    = null,
                         TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return PushEVSEDataResult.NoOperation(Id, this);


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;


            PushEVSEDataResult result;

            #endregion

            // Not implemented within the Gireve API!
            return PushEVSEDataResult.NoOperation(Id, this);

            //#region Get effective number of EVSEs/EVSEDataRecords to upload

            //var Warnings         = new List<Warning>();
            //var EVSEDataRecords  = new List<EVSEDataRecord>();

            //if (EVSEs.IsNeitherNullNorEmpty())
            //    foreach (var evse in EVSEs.Where(evse => evse != null          &&
            //                                             _IncludeEVSEs  (evse) &&
            //                                             _IncludeEVSEIds(evse.Id)))
            //    {

            //        try
            //        {

            //            // WWCP EVSE will be added as custom data "WWCP.EVSE"...
            //            EVSEDataRecords.Add(evse.ToeMIP(_EVSE2EVSEDataRecord));

            //        }
            //        catch (Exception e)
            //        {
            //            DebugX.Log(e.Message);
            //            Warnings.Add(Warning.Create(e.Message, evse));
            //        }

            //    }

            //#endregion

            //#region Send OnPushEVSEDataWWCPRequest event

            //var StartTime = DateTime.UtcNow;

            //try
            //{

            //    OnPushEVSEDataWWCPRequest?.Invoke(StartTime,
            //                                      Timestamp.Value,
            //                                      this,
            //                                      Id,
            //                                      EventTrackingId,
            //                                      RoamingNetwork.Id,
            //                                      ServerAction,
            //                                      EVSEDataRecords.ULongCount(),
            //                                      EVSEDataRecords,
            //                                      Warnings.Where(warning => warning.IsNotNullOrEmpty()),
            //                                      RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEDataWWCPRequest));
            //}

            //#endregion


            //DateTime Endtime;
            //TimeSpan Runtime;

            //if (EVSEDataRecords.Count > 0)
            //{

            //    var response = await CPORoaming.
            //                             PushEVSEData(EVSEDataRecords,
            //                                          DefaultOperatorId,
            //                                          DefaultOperatorName.IsNotNullOrEmpty() ? DefaultOperatorName : null,
            //                                          ServerAction,
            //                                          null,

            //                                          Timestamp,
            //                                          CancellationToken,
            //                                          EventTrackingId,
            //                                          RequestTimeout);


            //    if (response.HTTPStatusCode == HTTPStatusCode.OK &&
            //        response.Content != null)
            //    {

            //        // Success...
            //        if (response.Content.Result)
            //        {

            //            Endtime  = DateTime.UtcNow;
            //            Runtime  = Endtime - StartTime;
            //            result   = PushEVSEDataResult.Success(Id,
            //                                                  this,
            //                                                  response.Content.StatusCode.Description,
            //                                                  response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
            //                                                      ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
            //                                                      : Warnings,
            //                                                  Runtime);

            //        }

            //        // Operation failed... maybe the systems are out of sync?!
            //        // Try an automatic fullLoad in order to repair...
            //        else
            //        {

            //            if (ServerAction == // ActionTypes.insert ||
            //                ServerAction == // ActionTypes.update ||
            //                ServerAction == // ActionTypes.delete)
            //            {

            //                #region Add warnings...

            //                Warnings.Add(Warning.Create(ServerAction.ToString() + " of " + EVSEDataRecords.Count + " EVSEs failed!"));
            //                Warnings.Add(Warning.Create(response.Content.StatusCode.Code.ToString()));
            //                Warnings.Add(Warning.Create(response.Content.StatusCode.Description));

            //                if (response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty())
            //                    Warnings.Add(Warning.Create(response.Content.StatusCode.AdditionalInfo));

            //                Warnings.Add(Warning.Create("Will try to fix this issue via a 'fullLoad' of all EVSEs!"));

            //                #endregion

            //                #region Get all EVSEs from the roaming network

            //                var FullLoadEVSEs = RoamingNetwork.EVSEs.Where(evse => evse != null &&
            //                                                               _IncludeEVSEs(evse) &&
            //                                                               _IncludeEVSEIds(evse.Id)).
            //                                            Select(evse =>
            //                                            {

            //                                                try
            //                                                {

            //                                                    return evse.ToeMIP(_EVSE2EVSEDataRecord);

            //                                                }
            //                                                catch (Exception e)
            //                                                {
            //                                                    DebugX.Log(e.Message);
            //                                                    Warnings.Add(Warning.Create(e.Message, evse));
            //                                                }

            //                                                return null;

            //                                            }).
            //                                            Where(evsedatarecord => evsedatarecord != null).
            //                                            ToArray();

            //                #endregion

            //                #region Send request

            //                var FullLoadResponse = await CPORoaming.
            //                                                  PushEVSEData(FullLoadEVSEs,
            //                                                               DefaultOperatorId,
            //                                                               DefaultOperatorName.IsNotNullOrEmpty() ? DefaultOperatorName : null,
            //                                                               // ActionTypes.fullLoad,
            //                                                               null,

            //                                                               Timestamp,
            //                                                               CancellationToken,
            //                                                               EventTrackingId,
            //                                                               RequestTimeout).
            //                                                  ConfigureAwait(false);

            //                #endregion

            //                #region Result mapping

            //                Endtime = DateTime.UtcNow;
            //                Runtime = Endtime - StartTime;

            //                if (FullLoadResponse.HTTPStatusCode == HTTPStatusCode.OK &&
            //                    FullLoadResponse.Content != null)
            //                {

            //                    if (FullLoadResponse.Content.Result)
            //                        result = PushEVSEDataResult.Success(Id,
            //                                                        this,
            //                                                        FullLoadResponse.Content.StatusCode.Description,
            //                                                        FullLoadResponse.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
            //                                                            ? Warnings.AddAndReturnList(FullLoadResponse.Content.StatusCode.AdditionalInfo)
            //                                                            : Warnings,
            //                                                        Runtime);

            //                    else
            //                        result = PushEVSEDataResult.Error(Id,
            //                                                      this,
            //                                                      EVSEs,
            //                                                      FullLoadResponse.Content.StatusCode.Description,
            //                                                      FullLoadResponse.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
            //                                                          ? Warnings.AddAndReturnList(FullLoadResponse.Content.StatusCode.AdditionalInfo)
            //                                                          : Warnings,
            //                                                      Runtime);

            //                }

            //                else
            //                    result = PushEVSEDataResult.Error(Id,
            //                                                  this,
            //                                                  EVSEs,
            //                                                  FullLoadResponse.HTTPStatusCode.ToString(),
            //                                                  FullLoadResponse.HTTPBody != null
            //                                                      ? Warnings.AddAndReturnList(FullLoadResponse.HTTPBody.ToUTF8String())
            //                                                      : Warnings.AddAndReturnList("No HTTP body received!"),
            //                                                  Runtime);

            //                #endregion

            //            }

            //            // Or a 'fullLoad' Operation failed...
            //            else
            //            {

            //                Endtime  = DateTime.UtcNow;
            //                Runtime  = Endtime - StartTime;
            //                result   = PushEVSEDataResult.Error(Id,
            //                                                    this,
            //                                                    EVSEs,
            //                                                    response.HTTPStatusCode.ToString(),
            //                                                    response.HTTPBody != null
            //                                                        ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
            //                                                        : Warnings.AddAndReturnList("No HTTP body received!"),
            //                                                    Runtime);

            //            }

            //        }

            //    }
            //    else
            //    {

            //        Endtime  = DateTime.UtcNow;
            //        Runtime  = Endtime - StartTime;
            //        result   = PushEVSEDataResult.Error(Id,
            //                                            this,
            //                                            EVSEs,
            //                                            response.HTTPStatusCode.ToString(),
            //                                            response.HTTPBody != null
            //                                                ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
            //                                                : Warnings.AddAndReturnList("No HTTP body received!"),
            //                                            Runtime);

            //    }

            //}

            //#region ...or no EVSEs to push...

            //else
            //{

            //    Endtime  = DateTime.UtcNow;
            //    Runtime  = Endtime - StartTime;
            //    result   = PushEVSEDataResult.NoOperation(Id,
            //                                              this,
            //                                              "No EVSEDataRecords to push!",
            //                                              Warnings,
            //                                              DateTime.UtcNow - StartTime);

            //}

            //#endregion


            //#region Send OnPushEVSEDataResponse event

            //try
            //{

            //    OnPushEVSEDataWWCPResponse?.Invoke(Endtime,
            //                                       Timestamp.Value,
            //                                       this,
            //                                       Id,
            //                                       EventTrackingId,
            //                                       RoamingNetwork.Id,
            //                                       ServerAction,
            //                                       EVSEDataRecords.ULongCount(),
            //                                       EVSEDataRecords,
            //                                       RequestTimeout,
            //                                       result,
            //                                       Runtime);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEDataWWCPResponse));
            //}

            //#endregion

            return null;

        }

        #endregion

        #region (private) SetEVSEAvailabilityStatus(EVSEAdminStatusUpdates, ...)

        /// <summary>
        /// Upload the EVSE status of the given lookup of EVSE status types grouped by their Charging Station Operator.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdates">An enumeration of EVSE status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        private async Task<PushEVSEAdminStatusResult>

            SetEVSEAvailabilityStatus(IEnumerable<EVSEAdminStatusUpdate>  EVSEAdminStatusUpdates,

                                      DateTime?                           Timestamp           = null,
                                      CancellationToken?                  CancellationToken   = null,
                                      EventTracking_Id                    EventTrackingId     = null,
                                      TimeSpan?                           RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEAdminStatusUpdates == null || !EVSEAdminStatusUpdates.Any())
                return PushEVSEAdminStatusResult.NoOperation(Id, this);


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSEAdminStatus/EVSEAdminStatusRecords to upload

            var Warnings = new List<Warning>();

            var _EVSEAvailabilityAdminStatus = EVSEAdminStatusUpdates.
                                  ToLookup    (evsestatusupdate => evsestatusupdate.EVSE.Id,
                                               evsestatusupdate => evsestatusupdate).
                                  ToDictionary(group            => group.Key,
                                               group            => group.AsEnumerable().OrderByDescending(item => item.NewStatus.Timestamp)).
                                  Select      (evsestatusupdate => {

                                      try
                                      {

                                          var _EVSEId = evsestatusupdate.Key.ToEMIP(CustomEVSEIdMapper);

                                          if (!_EVSEId.HasValue)
                                              throw new InvalidEVSEIdentificationException(evsestatusupdate.Key.ToString());

                                          // Only push the current status of the latest status update!
                                          return new KeyValuePair<EVSEAdminStatusUpdate, EVSEAvailabilityStatus>?(
                                                     new KeyValuePair<EVSEAdminStatusUpdate, EVSEAvailabilityStatus>(
                                                         evsestatusupdate.Value.First(),
                                                         new EVSEAvailabilityStatus(
                                                             _EVSEId.Value,
                                                             evsestatusupdate.Value.First().NewStatus.Timestamp,
                                                             evsestatusupdate.Value.First().NewStatus.Value.ToEMIP()
                                                             // AvailabilityAdminStatusUntil
                                                             // AvailabilityAdminStatusComment
                                                         )
                                                     )
                                                 );

                                      }
                                      catch (Exception e)
                                      {
                                          DebugX.  Log(e.Message);
                                          Warnings.Add(Warning.Create(I18NString.Create(Languages.eng, e.Message), evsestatusupdate));
                                      }

                                      return null;

                                  }).
                                  Where(evsestatusrecord => evsestatusrecord != null).
                                  ToArray();

            PushEVSEAdminStatusResult result = null;
            var results = new List<PushEVSEAdminStatusResult>();

            #endregion

            #region Send OnEVSEAdminStatusPush event

            var StartTime = DateTime.UtcNow;

            //try
            //{

            //    OnPushEVSEAdminStatusWWCPRequest?.Invoke(StartTime,
            //                                        Timestamp.Value,
            //                                        this,
            //                                        Id,
            //                                        EventTrackingId,
            //                                        RoamingNetwork.Id,
            //                                        ActionType.update,
            //                                        _EVSEAdminStatus.ULongCount(),
            //                                        _EVSEAdminStatus,
            //                                        Warnings.Where(warning => warning.IsNotNullOrEmpty()),
            //                                        RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEAdminStatusWWCPRequest));
            //}

            #endregion


            //ToDo: Not clear if the Gireve API supports concurrent requests!
            foreach (var evseAdminStatus in _EVSEAvailabilityAdminStatus)
            {

                var response = await CPORoaming.
                                         SetEVSEAvailabilityStatus(PartnerId,
                                                                   evseAdminStatus.Value.Value.EVSEId.OperatorId,
                                                                   evseAdminStatus.Value.Value.EVSEId,
                                                                   evseAdminStatus.Value.Value.StatusEventDate,
                                                                   evseAdminStatus.Value.Value.AvailabilityStatus,
                                                                   Transaction_Id.Random(),
                                                                   null, //AvailabilityAdminStatusUntil
                                                                   null, //AvailabilityAdminStatusComment

                                                                   null,
                                                                   Timestamp,
                                                                   CancellationToken,
                                                                   EventTrackingId,
                                                                   RequestTimeout).

                                         ConfigureAwait(false);


                var Endtime = DateTime.UtcNow;
                var Runtime = Endtime - StartTime;

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content        != null)
                {

                    if (response.Content.RequestStatus == RequestStatus.Ok)
                        results.Add(PushEVSEAdminStatusResult.Success(Id,
                                                                 this,
                                                                 //response.Content.AdminStatusCode.Description,
                                                                 //response.Content.AdminStatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                 //    ? Warnings.AddAndReturnList(response.Content.AdminStatusCode.AdditionalInfo)
                                                                 //    : Warnings,
                                                                 Runtime: Runtime));

                    else
                        results.Add(PushEVSEAdminStatusResult.Error(Id,
                                                               this,
                                                               new EVSEAdminStatusUpdate[] { evseAdminStatus.Value.Key },
                                                               //response.Content.AdminStatusCode.Description,
                                                               //response.Content.AdminStatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                               //    ? Warnings.AddAndReturnList(response.Content.AdminStatusCode.AdditionalInfo)
                                                               //    : Warnings,
                                                               Runtime: Runtime));

                }
                else
                    results.Add(PushEVSEAdminStatusResult.Error(Id,
                                                           this,
                                                           new EVSEAdminStatusUpdate[] { evseAdminStatus.Value.Key },
                                                           response.HTTPStatusCode.ToString(),
                                                           response.HTTPBody != null
                                                               ? Warnings.AddAndReturnList(I18NString.Create(Languages.eng, response.HTTPBody.ToUTF8String()))
                                                               : Warnings.AddAndReturnList(I18NString.Create(Languages.eng, "No HTTP body received!")),
                                                           Runtime));

            }


            #region Send OnPushEVSEAdminStatusResponse event

            //try
            //{

            //    OnPushEVSEAdminStatusWWCPResponse?.Invoke(Endtime,
            //                                         Timestamp.Value,
            //                                         this,
            //                                         Id,
            //                                         EventTrackingId,
            //                                         RoamingNetwork.Id,
            //                                         ServerAction,
            //                                         _EVSEAdminStatus.ULongCount(),
            //                                         _EVSEAdminStatus,
            //                                         RequestTimeout,
            //                                         result,
            //                                         Runtime);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEAdminStatusWWCPResponse));
            //}

            #endregion

            return PushEVSEAdminStatusResult.Flatten(Id,
                                                     this,
                                                     results,
                                                     DateTime.UtcNow - StartTime);

        }

        #endregion

        #region (private) SetEVSEBusyStatus(EVSEStatusUpdates, ...)

        /// <summary>
        /// Upload the EVSE status of the given lookup of EVSE status types grouped by their Charging Station Operator.
        /// </summary>
        /// <param name="EVSEStatusUpdates">An enumeration of EVSE status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        private async Task<PushEVSEStatusResult>

            SetEVSEBusyStatus(IEnumerable<EVSEStatusUpdate>  EVSEStatusUpdates,

                              DateTime?                      Timestamp           = null,
                              CancellationToken?             CancellationToken   = null,
                              EventTracking_Id               EventTrackingId     = null,
                              TimeSpan?                      RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEStatusUpdates == null || !EVSEStatusUpdates.Any())
                return PushEVSEStatusResult.NoOperation(Id, this);


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSEStatus/EVSEStatusRecords to upload

            var Warnings = new List<Warning>();

            var _EVSEBusyStatus = EVSEStatusUpdates.
                                  ToLookup    (evsestatusupdate => evsestatusupdate.EVSE.Id,
                                               evsestatusupdate => evsestatusupdate).
                                  ToDictionary(group            => group.Key,
                                               group            => group.AsEnumerable().OrderByDescending(item => item.NewStatus.Timestamp)).
                                  Select      (evsestatusupdate => {

                                      try
                                      {

                                          var _EVSEId = evsestatusupdate.Key.ToEMIP(CustomEVSEIdMapper);

                                          if (!_EVSEId.HasValue)
                                              throw new InvalidEVSEIdentificationException(evsestatusupdate.Key.ToString());

                                          // Only push the current status of the latest status update!
                                          return new KeyValuePair<EVSEStatusUpdate, EVSEBusyStatus>?(
                                                     new KeyValuePair<EVSEStatusUpdate, EVSEBusyStatus>(
                                                         evsestatusupdate.Value.First(),
                                                         new EVSEBusyStatus(
                                                             _EVSEId.Value,
                                                             evsestatusupdate.Value.First().NewStatus.Timestamp,
                                                             evsestatusupdate.Value.First().NewStatus.Value.ToEMIP()
                                                             // BusyStatusUntil
                                                             // BusyStatusComment
                                                         )
                                                     )
                                                 );

                                      }
                                      catch (Exception e)
                                      {
                                          DebugX.  Log(e.Message);
                                          Warnings.Add(Warning.Create(I18NString.Create(Languages.eng, e.Message), evsestatusupdate));
                                      }

                                      return null;

                                  }).
                                  Where(evsestatusrecord => evsestatusrecord != null).
                                  ToArray();

            PushEVSEStatusResult result = null;
            var results = new List<PushEVSEStatusResult>();

            #endregion

            #region Send OnEVSEStatusPush event

            var StartTime = DateTime.UtcNow;

            //try
            //{

            //    OnPushEVSEStatusWWCPRequest?.Invoke(StartTime,
            //                                        Timestamp.Value,
            //                                        this,
            //                                        Id,
            //                                        EventTrackingId,
            //                                        RoamingNetwork.Id,
            //                                        ActionType.update,
            //                                        _EVSEStatus.ULongCount(),
            //                                        _EVSEStatus,
            //                                        Warnings.Where(warning => warning.IsNotNullOrEmpty()),
            //                                        RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEStatusWWCPRequest));
            //}

            #endregion


            //ToDo: Not clear if the Gireve API supports concurrent requests!
            foreach (var evseStatus in _EVSEBusyStatus)
            {

                var response = await CPORoaming.
                                         SetEVSEBusyStatus(PartnerId,
                                                           evseStatus.Value.Value.EVSEId.OperatorId,
                                                           evseStatus.Value.Value.EVSEId,
                                                           evseStatus.Value.Value.StatusEventDate,
                                                           evseStatus.Value.Value.BusyStatus,
                                                           Transaction_Id.Random(),
                                                           null, //BusyStatusUntil
                                                           null, //BusyStatusComment

                                                           null,
                                                           Timestamp,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           RequestTimeout).

                                         ConfigureAwait(false);


                var Endtime = DateTime.UtcNow;
                var Runtime = Endtime - StartTime;

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content        != null)
                {

                    if (response.Content.RequestStatus == RequestStatus.Ok)
                        results.Add(PushEVSEStatusResult.Success(Id,
                                                                 this,
                                                                 //response.Content.StatusCode.Description,
                                                                 //response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                 //    ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
                                                                 //    : Warnings,
                                                                 Runtime: Runtime));

                    else
                        results.Add(PushEVSEStatusResult.Error(Id,
                                                               this,
                                                               new EVSEStatusUpdate[] { evseStatus.Value.Key },
                                                               //response.Content.StatusCode.Description,
                                                               //response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                               //    ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
                                                               //    : Warnings,
                                                               Runtime: Runtime));

                }
                else
                    results.Add(PushEVSEStatusResult.Error(Id,
                                                           this,
                                                           new EVSEStatusUpdate[] { evseStatus.Value.Key },
                                                           response.HTTPStatusCode.ToString(),
                                                           response.HTTPBody != null
                                                               ? Warnings.AddAndReturnList(I18NString.Create(Languages.eng, response.HTTPBody.ToUTF8String()))
                                                               : Warnings.AddAndReturnList(I18NString.Create(Languages.eng, "No HTTP body received!")),
                                                           Runtime));

            }


            #region Send OnPushEVSEStatusResponse event

            //try
            //{

            //    OnPushEVSEStatusWWCPResponse?.Invoke(Endtime,
            //                                         Timestamp.Value,
            //                                         this,
            //                                         Id,
            //                                         EventTrackingId,
            //                                         RoamingNetwork.Id,
            //                                         ServerAction,
            //                                         _EVSEStatus.ULongCount(),
            //                                         _EVSEStatus,
            //                                         RequestTimeout,
            //                                         result,
            //                                         Runtime);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEStatusWWCPResponse));
            //}

            #endregion

            return PushEVSEStatusResult.Flatten(Id,
                                                this,
                                                results,
                                                DateTime.UtcNow - StartTime);

        }

        #endregion


        #region (Set/Add/Update/Delete) EVSE(s)...

        #region SetStaticData   (EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the given EVSE as new static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="EVSE">An EVSE to upload.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(EVSE                EVSE,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                return PushEVSEDataResult.NoOperation(Id,
                                                      this);

            if (DisablePushData)
                return PushEVSEDataResult.AdminDown(Id,
                                                    this,
                                                    new EVSE[] { EVSE });

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                return await SetStaticData(this,
                                           EVSE,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout).

                              ConfigureAwait(false);

            }

            #endregion

            return await PushEVSEData(new EVSE[] { EVSE },
                                      // ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region AddStaticData   (EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given EVSE to the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="EVSE">An EVSE to upload.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(EVSE                EVSE,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                return PushEVSEDataResult.NoOperation(Id,
                                                      this);

            if (DisablePushData)
                return PushEVSEDataResult.AdminDown(Id,
                                                    this,
                                                    new EVSE[] { EVSE });

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                return await AddStaticData(this,
                                           EVSE,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout).

                              ConfigureAwait(false);

            }

            #endregion

            return await PushEVSEData(new EVSE[] { EVSE },
                                      // ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(EVSE, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the static data of the given EVSE.
        /// The EVSE can be uploaded as a whole, or just a single property of the EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to update.</param>
        /// <param name="PropertyName">The name of the EVSE property to update.</param>
        /// <param name="OldValue">The old value of the EVSE property to update.</param>
        /// <param name="NewValue">The new value of the EVSE property to update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(EVSE                EVSE,
                                       String              PropertyName,
                                       Object              OldValue,
                                       Object              NewValue,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE         == null)
                throw new ArgumentNullException(nameof(EVSE),          "The given EVSE must not be null!");

            if (PropertyName == null)
                throw new ArgumentNullException(nameof(PropertyName),  "The given EVSE property name must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    if (IncludeEVSEs == null ||
                       (IncludeEVSEs != null && IncludeEVSEs(EVSE)))
                    {

                        if (EVSEsUpdateLog.TryGetValue(EVSE, out List<PropertyUpdateInfos> PropertyUpdateInfo))
                            PropertyUpdateInfo.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));

                        else
                        {

                            var List = new List<PropertyUpdateInfos>();
                            List.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));
                            EVSEsUpdateLog.Add(EVSE, List);

                        }

                        EVSEsToUpdateQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(new EVSE[] { EVSE },
                                      // ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the static data of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to delete.</param>
        /// <param name="TransmissionType">Whether to send the EVSE deletion directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(EVSE                EVSE,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    if (IncludeEVSEs == null ||
                       (IncludeEVSEs != null && IncludeEVSEs(EVSE)))
                    {

                        EVSEsToRemoveQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(new EVSE[] { EVSE },
                                      // ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion


        #region SetStaticData   (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the given enumeration of EVSEs as new static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<EVSE>   EVSEs,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return PushEVSEDataResult.NoOperation(Id, this);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    var FilteredEVSEs = EVSEs.Where(evse => IncludeEVSEs(evse) &&
                                                            IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        return PushEVSEDataResult.Enqueued(Id, this);

                    }

                    return PushEVSEDataResult.NoOperation(Id, this);

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

            }

            #endregion


            return await PushEVSEData(EVSEs,
                                      // ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region AddStaticData   (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs to the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<EVSE>   EVSEs,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return PushEVSEDataResult.NoOperation(Id, this);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    var FilteredEVSEs = EVSEs.Where(evse => IncludeEVSEs(evse) &&
                                                            IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        return PushEVSEDataResult.Enqueued(Id, this);

                    }

                    return PushEVSEDataResult.NoOperation(Id, this);

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

            }

            #endregion

            return await PushEVSEData(EVSEs,
                                      // ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSEs within the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<EVSE>   EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return PushEVSEDataResult.NoOperation(Id, this);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    var FilteredEVSEs = EVSEs.Where(evse => IncludeEVSEs(evse) &&
                                                            IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToUpdateQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        return PushEVSEDataResult.Enqueued(Id, this);

                    }

                    return PushEVSEDataResult.NoOperation(Id, this);

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

            }

            #endregion

            return await PushEVSEData(EVSEs,
                                      // ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given enumeration of EVSEs from the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<EVSE>   EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return PushEVSEDataResult.NoOperation(Id, this);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    var FilteredEVSEs = EVSEs.Where(evse => IncludeEVSEs(evse) &&
                                                            IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToRemoveQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        return PushEVSEDataResult.Enqueued(Id, this);

                    }

                    return PushEVSEDataResult.NoOperation(Id, this);

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

            }

            #endregion

            return await PushEVSEData(EVSEs,
                                      // ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                   TransmissionType,

                                               DateTime?                           Timestamp,
                                               CancellationToken?                  CancellationToken,
                                               EventTracking_Id                    EventTrackingId,
                                               TimeSpan?                           RequestTimeout)

        {

            #region Initial checks

            if (AdminStatusUpdates == null || !AdminStatusUpdates.Any())
                return PushEVSEAdminStatusResult.NoOperation(Id,
                                                         this);

            if (DisablePushStatus)
                return PushEVSEAdminStatusResult.AdminDown(Id,
                                                           this,
                                                           AdminStatusUpdates);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                return await UpdateStatus(this,
                                          AdminStatusUpdates,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout).

                             ConfigureAwait(false);

            }

            #endregion

            return await SetEVSEAvailabilityStatus(AdminStatusUpdates,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout).

                         ConfigureAwait(false);

        }

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of EVSE status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
                                     TransmissionTypes              TransmissionType,

                                     DateTime?                      Timestamp,
                                     CancellationToken?             CancellationToken,
                                     EventTracking_Id               EventTrackingId,
                                     TimeSpan?                      RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null || !StatusUpdates.Any())
                return PushEVSEStatusResult.NoOperation(Id, this);

            PushEVSEStatusResult result = null;

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var invokeTimer  = false;
                var LockTaken    = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var FilteredUpdates = StatusUpdates.Where(statusupdate => IncludeEVSEs  (statusupdate.EVSE) &&
                                                                                  IncludeEVSEIds(statusupdate.EVSE.Id)).
                                                            ToArray();

                        if (FilteredUpdates.Length > 0)
                        {

                            foreach (var Update in FilteredUpdates)
                            {

                                // Delay the status update until the EVSE data had been uploaded!
                                if (EVSEsToAddQueue.Any(evse => evse == Update.EVSE))
                                    EVSEStatusChangesDelayedQueue.Add(Update);

                                else
                                    EVSEStatusChangesFastQueue.Add(Update);

                            }

                            invokeTimer = true;

                            result = PushEVSEStatusResult.Enqueued(Id, this);

                        }

                        result = PushEVSEStatusResult.NoOperation(Id, this);

                    }

                }
                finally
                {
                    if (LockTaken)
                        DataAndStatusLock.Release();
                }

                if (!LockTaken)
                    return PushEVSEStatusResult.Error(Id, this, Description: "Could not acquire DataAndStatusLock!");

                if (invokeTimer)
                    FlushEVSEFastStatusTimer.Change(FlushEVSEFastStatusEvery, TimeSpan.FromMilliseconds(-1));

                return result;

            }

            #endregion

            return await SetEVSEBusyStatus(StatusUpdates,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout).

                         ConfigureAwait(false);

        }

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station(s)...

        #region SetStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station as new static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(ChargingStation     ChargingStation,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();
                {

                    foreach (var evse in ChargingStation)
                    {

                        if (IncludeEVSEs == null ||
                           (IncludeEVSEs != null && IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingStation.EVSEs,
                                      // ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station to the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(ChargingStation     ChargingStation,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();
                {

                    foreach (var evse in ChargingStation)
                    {

                        if (IncludeEVSEs == null ||
                           (IncludeEVSEs != null && IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingStation.EVSEs,
                                      // ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(ChargingStation, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station within the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="PropertyName">The name of the charging station property to update.</param>
        /// <param name="OldValue">The old value of the charging station property to update.</param>
        /// <param name="NewValue">The new value of the charging station property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(ChargingStation     ChargingStation,
                                       String              PropertyName,
                                       Object              OldValue,
                                       Object              NewValue,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();
                {

                    var AddData = false;

                    foreach (var evse in ChargingStation)
                    {
                        if (IncludeEVSEs == null ||
                           (IncludeEVSEs != null && IncludeEVSEs(evse)))
                        {
                            EVSEsToUpdateQueue.Add(evse);
                            AddData = true;
                        }
                    }

                    if (AddData)
                    {

                        if (ChargingStationsUpdateLog.TryGetValue(ChargingStation, out List<PropertyUpdateInfos> PropertyUpdateInfo))
                            PropertyUpdateInfo.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));

                        else
                        {
                            var List = new List<PropertyUpdateInfos>();
                            List.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));
                            ChargingStationsUpdateLog.Add(ChargingStation, List);
                        }

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingStation,
                                      // ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station from the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(ChargingStation     ChargingStation,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return PushEVSEData(ChargingStation.EVSEs,
                                // ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region SetStaticData   (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging stations as new static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                    TransmissionTypes             TransmissionType,

                                    DateTime?                     Timestamp,
                                    CancellationToken?            CancellationToken,
                                    EventTracking_Id              EventTrackingId,
                                    TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                // ActionTypes.fullLoad,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region AddStaticData   (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                    TransmissionTypes             TransmissionType,


                                    DateTime?                     Timestamp,
                                    CancellationToken?            CancellationToken,
                                    EventTracking_Id              EventTrackingId,
                                    TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                // ActionTypes.insert,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging stations within the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                       TransmissionTypes             TransmissionType,

                                       DateTime?                     Timestamp,
                                       CancellationToken?            CancellationToken,
                                       EventTracking_Id              EventTrackingId,
                                       TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                // ActionTypes.update,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging stations from the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                       TransmissionTypes             TransmissionType,

                                       DateTime?                     Timestamp,
                                       CancellationToken?            CancellationToken,
                                       EventTracking_Id              EventTrackingId,
                                       TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                // ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging station admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                              TransmissionType,

                                               DateTime?                                      Timestamp,
                                               CancellationToken?                             CancellationToken,
                                               EventTracking_Id                               EventTrackingId,
                                               TimeSpan?                                      RequestTimeout)


                => Task.FromResult(PushChargingStationAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                         TransmissionType,

                                     DateTime?                                 Timestamp,
                                     CancellationToken?                        CancellationToken,
                                     EventTracking_Id                          EventTrackingId,
                                     TimeSpan?                                 RequestTimeout)


                => Task.FromResult(PushChargingStationStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging pool(s)...

        #region SetStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging pool as new static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(ChargingPool        ChargingPool,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();
                {

                    foreach (var evse in ChargingPool.EVSEs)
                    {

                        if (IncludeEVSEs == null ||
                           (IncludeEVSEs != null && IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingPool.EVSEs,
                                      // ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging pool to the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(ChargingPool        ChargingPool,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();
                {

                    foreach (var evse in ChargingPool.EVSEs)
                    {

                        if (IncludeEVSEs == null ||
                           (IncludeEVSEs != null && IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingPool.EVSEs,
                                      // ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(ChargingPool, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging pool within the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="PropertyName">The name of the charging pool property to update.</param>
        /// <param name="OldValue">The old value of the charging pool property to update.</param>
        /// <param name="NewValue">The new value of the charging pool property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(ChargingPool        ChargingPool,
                                       String              PropertyName,
                                       Object              OldValue,
                                       Object              NewValue,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                return PushEVSEDataResult.NoOperation(Id,
                                                      this);

            if (DisablePushData)
                return PushEVSEDataResult.AdminDown(Id,
                                                    this);

            PushEVSEDataResult result = null;

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var invokeTimer = false;
                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var AddData = false;

                        foreach (var evse in ChargingPool.EVSEs)
                        {
                            if (IncludeEVSEs == null ||
                               (IncludeEVSEs != null && IncludeEVSEs(evse)))
                            {
                                EVSEsToUpdateQueue.Add(evse);
                                AddData = true;
                            }
                        }

                        if (AddData)
                        {

                            if (ChargingPoolsUpdateLog.TryGetValue(ChargingPool, out List<PropertyUpdateInfos> PropertyUpdateInfo))
                                PropertyUpdateInfo.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));

                            else
                            {
                                var List = new List<PropertyUpdateInfos>();
                                List.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));
                                ChargingPoolsUpdateLog.Add(ChargingPool, List);
                            }

                            invokeTimer = true;

                        }

                        result = PushEVSEDataResult.Enqueued(Id, this);

                    }

                    #region Could not get the lock for toooo long!

                    else
                    {

                        //Endtime  = DateTime.UtcNow;
                        //Runtime  = Endtime - StartTime;
                        result   = PushEVSEDataResult.Timeout(Id,
                                                              this,
                                                              new EVSE[0],// ChargeDetailRecords,
                                                              "Could not " + (TransmissionType == TransmissionTypes.Enqueue ? "enqueue" : "send") + " charge detail records!"
                                                              //ChargeDetailRecords.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Timeout)),
                                                              //Runtime: Runtime
                                                              );

                    }

                    #endregion

                }
                finally
                {
                    if (LockTaken)
                        DataAndStatusLock.Release();
                }

                if (!LockTaken)
                    return PushEVSEDataResult.Error(Id, this, Description: "Could not acquire DataAndStatusLock!");

                if (invokeTimer)
                    FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                return result;

            }

            #endregion

            return await PushEVSEData(ChargingPool.EVSEs,
                                      // ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging pool from the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(ChargingPool        ChargingPool,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return PushEVSEData(ChargingPool.EVSEs,
                                // ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region SetStaticData   (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging pools as new static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                    TransmissionTypes          TransmissionType,

                                    DateTime?                  Timestamp,
                                    CancellationToken?         CancellationToken,
                                    EventTracking_Id           EventTrackingId,
                                    TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                // ActionTypes.fullLoad,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region AddStaticData   (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                    TransmissionTypes          TransmissionType,

                                    DateTime?                  Timestamp,
                                    CancellationToken?         CancellationToken,
                                    EventTracking_Id           EventTrackingId,
                                    TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                // ActionTypes.insert,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging pools within the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                       TransmissionTypes          TransmissionType,

                                       DateTime?                  Timestamp,
                                       CancellationToken?         CancellationToken,
                                       EventTracking_Id           EventTrackingId,
                                       TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                // ActionTypes.update,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging pools from the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                       TransmissionTypes          TransmissionType,

                                       DateTime?                  Timestamp,
                                       CancellationToken?         CancellationToken,
                                       EventTracking_Id           EventTrackingId,
                                       TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                // ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging pool admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                           TransmissionType,

                                               DateTime?                                   Timestamp,
                                               CancellationToken?                          CancellationToken,
                                               EventTracking_Id                            EventTrackingId,
                                               TimeSpan?                                   RequestTimeout)


                => Task.FromResult(PushChargingPoolAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging pool status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingPoolStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                      TransmissionType,

                                     DateTime?                              Timestamp,
                                     CancellationToken?                     CancellationToken,
                                     EventTracking_Id                       EventTrackingId,
                                     TimeSpan?                              RequestTimeout)


                => Task.FromResult(PushChargingPoolStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station operator(s)...

        #region SetStaticData   (ChargingStationOperator, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station operator as new static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(ChargingStationOperator  ChargingStationOperator,

                                    DateTime?                     Timestamp,
                                    CancellationToken?            CancellationToken,
                                    EventTracking_Id              EventTrackingId,
                                    TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperator.EVSEs,
                                // ActionTypes.fullLoad,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region AddStaticData   (ChargingStationOperator, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station operator to the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(ChargingStationOperator  ChargingStationOperator,

                                    DateTime?                     Timestamp,
                                    CancellationToken?            CancellationToken,
                                    EventTracking_Id              EventTrackingId,
                                    TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperator.EVSEs,
                                // ActionTypes.insert,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(ChargingStationOperator, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station operator within the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(ChargingStationOperator  ChargingStationOperator,

                                       DateTime?                     Timestamp,
                                       CancellationToken?            CancellationToken,
                                       EventTracking_Id              EventTrackingId,
                                       TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperator.EVSEs,
                                // ActionTypes.update,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(ChargingStationOperator, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station operator from the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(ChargingStationOperator  ChargingStationOperator,

                                       DateTime?                     Timestamp,
                                       CancellationToken?            CancellationToken,
                                       EventTracking_Id              EventTrackingId,
                                       TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperator.EVSEs,
                                // ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region SetStaticData   (ChargingStationOperators, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging station operators as new static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                    DateTime?                                  Timestamp,
                                    CancellationToken?                         CancellationToken,
                                    EventTracking_Id                           EventTrackingId,
                                    TimeSpan?                                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),
                                // ActionTypes.fullLoad,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region AddStaticData   (ChargingStationOperators, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging station operators to the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                    DateTime?                                  Timestamp,
                                    CancellationToken?                         CancellationToken,
                                    EventTracking_Id                           EventTrackingId,
                                    TimeSpan?                                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),
                                // ActionTypes.insert,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(ChargingStationOperators, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging station operators within the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                       DateTime?                                  Timestamp,
                                       CancellationToken?                         CancellationToken,
                                       EventTracking_Id                           EventTrackingId,
                                       TimeSpan?                                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),
                                // ActionTypes.update,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(ChargingStationOperators, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging station operators from the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                       DateTime?                                  Timestamp,
                                       CancellationToken?                         CancellationToken,
                                       EventTracking_Id                           EventTrackingId,
                                       TimeSpan?                                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),
                                // ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region UpdateChargingStationOperatorAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging station operator admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationOperatorAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                                      TransmissionType,

                                               DateTime?                                              Timestamp,
                                               CancellationToken?                                     CancellationToken,
                                               EventTracking_Id                                       EventTrackingId,
                                               TimeSpan?                                              RequestTimeout)


                => Task.FromResult(PushChargingStationOperatorAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateChargingStationOperatorStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station operator status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationOperatorStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                                 TransmissionType,

                                     DateTime?                                         Timestamp,
                                     CancellationToken?                                CancellationToken,
                                     EventTracking_Id                                  EventTrackingId,
                                     TimeSpan?                                         RequestTimeout)


                => Task.FromResult(PushChargingStationOperatorStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Roaming network...

        #region SetStaticData   (RoamingNetwork, ...)

        /// <summary>
        /// Set the EVSE data of the given roaming network as new static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(RoamingNetwork      RoamingNetwork,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return PushEVSEData(RoamingNetwork.EVSEs,
                                // ActionTypes.fullLoad,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region AddStaticData   (RoamingNetwork, ...)

        /// <summary>
        /// Add the EVSE data of the given roaming network to the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(RoamingNetwork      RoamingNetwork,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return PushEVSEData(RoamingNetwork.EVSEs,
                                // ActionTypes.insert,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(RoamingNetwork, ...)

        /// <summary>
        /// Update the EVSE data of the given roaming network within the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(RoamingNetwork      RoamingNetwork,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return PushEVSEData(RoamingNetwork.EVSEs,
                                // ActionTypes.update,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(RoamingNetwork, ...)

        /// <summary>
        /// Delete the EVSE data of the given roaming network from the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to upload.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(RoamingNetwork      RoamingNetwork,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return PushEVSEData(RoamingNetwork.EVSEs,
                                // ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region UpdateRoamingNetworkAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming network admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of roaming network admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the roaming network admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushRoamingNetworkAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                             TransmissionType,

                                               DateTime?                                     Timestamp,
                                               CancellationToken?                            CancellationToken,
                                               EventTracking_Id                              EventTrackingId,
                                               TimeSpan?                                     RequestTimeout)


                => Task.FromResult(PushRoamingNetworkAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateRoamingNetworkStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming network status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of roaming network status updates.</param>
        /// <param name="TransmissionType">Whether to send the roaming network status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushRoamingNetworkStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<RoamingNetworkStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                        TransmissionType,

                                     DateTime?                                Timestamp,
                                     CancellationToken?                       CancellationToken,
                                     EventTracking_Id                         EventTrackingId,
                                     TimeSpan?                                RequestTimeout)


                => Task.FromResult(PushRoamingNetworkStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #endregion


        #region AuthorizeStart(           AuthIdentification, ChargingLocation = null, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          AuthIdentification,
                           ChargingLocation             ChargingLocation    = null,
                           ChargingProduct              ChargingProduct     = null,   // [maxlength: 100]
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeStartRequest?.Invoke(StartTime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                null,
                                                Id,
                                                OperatorId,
                                                AuthIdentification,
                                                ChargingLocation,
                                                ChargingProduct,
                                                SessionId,
                                                new ISendAuthorizeStartStop[0],
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            DateTime             Endtime;
            TimeSpan             Runtime;
            AuthStartResult  result;

            if (ChargingLocation?.EVSEId == null)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.UnknownLocation(Id,
                                                           this,
                                                           SessionId,
                                                           Runtime);

            }

            else if (DisableAuthentication)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.AdminDown(Id,
                                                         this,
                                                         SessionId,
                                                         Runtime);

            }

            else
            {

                var response  = await CPORoaming.
                                          GetServiceAuthorisation(PartnerId:                PartnerId,
                                                                  OperatorId:               OperatorId.Value.ToEMIP(CustomOperatorIdMapper),
                                                                  EVSEId:                   WWCP.EVSE_Id.Parse(CustomEVSEIdMapper(ChargingLocation.EVSEId.ToString())).ToEMIP().Value,
                                                                  UserId:                   User_Id.Parse(AuthIdentification.AuthToken.ToString()),
                                                                  RequestedServiceId:       Service_Id. Parse("1"),
                                                                  TransactionId:            Transaction_Id.Random(),
                                                                  PartnerServiceSessionId:  new PartnerServiceSession_Id?(),

                                                                  Timestamp:                Timestamp,
                                                                  CancellationToken:        CancellationToken,
                                                                  EventTrackingId:          EventTrackingId,
                                                                  RequestTimeout:           RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.RequestStatus.Code  == 1                 &&
                    response.Content.AuthorisationValue  == AuthorisationValues.OK)
                {

                    result = AuthStartResult.Authorized(
                                 Id,
                                 this,
                                 ChargingSession_Id.Parse(response.Content.ServiceSessionId.ToString()),
                                 ProviderId:       response.Content.SalesPartnerOperatorId.ToWWCP(),
                                 //Description:      response.Content.StatusCode.Description,
                                 //AdditionalInfo:   response.Content.StatusCode.AdditionalInfo,
                                 NumberOfRetries:  response.NumberOfRetries,
                                 Runtime:          Runtime
                             );

                }

                else
                    result = AuthStartResult.NotAuthorized(
                                 Id,
                                 this,
                                 SessionId,
                                 ProviderId:       response.Content.SalesPartnerOperatorId.ToWWCP(),
                                 //response.Content.StatusCode.Description,
                                 //response.Content.StatusCode.AdditionalInfo,
                                 Runtime:          Runtime
                             );

            }


            #region Send OnAuthorizeStartResponse event

            try
            {

                OnAuthorizeStartResponse?.Invoke(Endtime,
                                                 Timestamp.Value,
                                                 this,
                                                 Id.ToString(),
                                                 EventTrackingId,
                                                 RoamingNetwork.Id,
                                                 null,
                                                 Id,
                                                 OperatorId,
                                                 AuthIdentification,
                                                 ChargingLocation,
                                                 ChargingProduct,
                                                 SessionId,
                                                 new ISendAuthorizeStartStop[0],
                                                 RequestTimeout,
                                                 result,
                                                 Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop (SessionId, AuthIdentification, ChargingLocation = null,                                           OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">A (RFID) user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          AuthIdentification,
                          ChargingLocation             ChargingLocation    = null,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthIdentification  == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeStopRequest?.Invoke(StartTime,
                                               Timestamp.Value,
                                               this,
                                               Id.ToString(),
                                               EventTrackingId,
                                               RoamingNetwork.Id,
                                               null,
                                               Id,
                                               OperatorId,
                                               ChargingLocation,
                                               SessionId,
                                               AuthIdentification,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            DateTime            Endtime;
            TimeSpan            Runtime;
            AuthStopResult  result;

            if (ChargingLocation?.EVSEId == null)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.UnknownLocation(Id,
                                                          this,
                                                          SessionId,
                                                          Runtime);

            }

            else if (DisableAuthentication)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.AdminDown(Id,
                                                        this,
                                                        SessionId,
                                                        Runtime);

            }

            else
            {

                var response  = await CPORoaming.
                                          GetServiceAuthorisation(PartnerId:                PartnerId,
                                                                  OperatorId:               OperatorId.Value.ToEMIP(CustomOperatorIdMapper),
                                                                  EVSEId:                   WWCP.EVSE_Id.Parse(CustomEVSEIdMapper(ChargingLocation.EVSEId.ToString())).ToEMIP().Value,
                                                                  UserId:                   User_Id.Parse(AuthIdentification.AuthToken.ToString()),
                                                                  RequestedServiceId:       Service_Id. Parse("1"),
                                                                  TransactionId:            Transaction_Id.Random(),
                                                                  PartnerServiceSessionId:  new PartnerServiceSession_Id?(),

                                                                  Timestamp:                Timestamp,
                                                                  CancellationToken:        CancellationToken,
                                                                  EventTrackingId:          EventTrackingId,
                                                                  RequestTimeout:           RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.RequestStatus.Code  == 1                 &&
                    response.Content.AuthorisationValue  == AuthorisationValues.OK)
                {

                    result = AuthStopResult.Authorized(
                                 Id,
                                 this,
                                 ChargingSession_Id.Parse(response.Content.ServiceSessionId.ToString()),
                                 ProviderId:       response.Content.SalesPartnerOperatorId.ToWWCP(),
                                 //Description:      response.Content.StatusCode.Description,
                                 //AdditionalInfo:   response.Content.StatusCode.AdditionalInfo,
                                 //NumberOfRetries:  response.NumberOfRetries,
                                 Runtime:          Runtime
                             );

                }

                else
                    result = AuthStopResult.NotAuthorized(
                                 Id,
                                 this,
                                 SessionId,
                                 ProviderId:       response.Content.SalesPartnerOperatorId.ToWWCP(),
                                 //response.Content.StatusCode.Description,
                                 //response.Content.StatusCode.AdditionalInfo,
                                 Runtime:          Runtime
                             );

            }


            #region Send OnAuthorizeStopResponse event

            try
            {

                OnAuthorizeStopResponse?.Invoke(Endtime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                null,
                                                Id,
                                                OperatorId,
                                                ChargingLocation,
                                                SessionId,
                                                AuthIdentification,
                                                RequestTimeout,
                                                result,
                                                Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SendChargeDetailRecords(ChargeDetailRecords, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Send charge detail records to an eMIP server.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the CDR directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<WWCP.ChargeDetailRecord>  ChargeDetailRecords,
                                    TransmissionTypes                     TransmissionType,

                                    DateTime?                             Timestamp,
                                    CancellationToken?                    CancellationToken,
                                    EventTracking_Id                      EventTrackingId,
                                    TimeSpan?                             RequestTimeout)

        {

            #region Initial checks

            if (ChargeDetailRecords == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecords),  "The given enumeration of charge detail records must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Filter charge detail records

            var ForwardedCDRs  = new List<WWCP.ChargeDetailRecord>();
            var FilteredCDRs   = new List<SendCDRResult>();

            foreach (var cdr in ChargeDetailRecords)
            {

                if (ChargeDetailRecordFilter(cdr) == ChargeDetailRecordFilters.forward)
                    ForwardedCDRs.Add(cdr);

                else
                    FilteredCDRs.Add(SendCDRResult.Filtered(cdr,
                                                            Warning.Create(I18NString.Create(Languages.eng, "This charge detail record was filtered!"))));

            }

            #endregion

            #region Send OnSendCDRsRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnSendCDRsRequest?.Invoke(StartTime,
                                          Timestamp.Value,
                                          this,
                                          Id.ToString(),
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          ChargeDetailRecords,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            #region if disabled => 'AdminDown'...

            DateTime        Endtime;
            TimeSpan        Runtime;
            SendCDRsResult  results;

            if (DisableSendChargeDetailRecords)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                results  = SendCDRsResult.AdminDown(Id,
                                                    this,
                                                    ChargeDetailRecords,
                                                    Runtime: Runtime);

            }

            #endregion

            else
            {

                var invokeTimer  = false;
                var LockTaken    = await FlushChargeDetailRecordsLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var SendCDRsResults = new List<SendCDRResult>();

                        #region if enqueuing is requested...

                        if (TransmissionType == TransmissionTypes.Enqueue)
                        {

                            #region Send OnEnqueueSendCDRRequest event

                            try
                            {

                                OnEnqueueSendCDRsRequest?.Invoke(DateTime.UtcNow,
                                                                 Timestamp.Value,
                                                                 this,
                                                                 Id.ToString(),
                                                                 EventTrackingId,
                                                                 RoamingNetwork.Id,
                                                                 ChargeDetailRecords,
                                                                 RequestTimeout);

                            }
                            catch (Exception e)
                            {
                                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
                            }

                            #endregion

                            foreach (var chargeDetailRecord in ChargeDetailRecords)
                            {

                                try
                                {

                                    ChargeDetailRecordsQueue.Add(chargeDetailRecord.ToEMIP(_WWCPChargeDetailRecord2eMIPChargeDetailRecord));
                                    SendCDRsResults.Add(SendCDRResult.Enqueued(chargeDetailRecord));

                                }
                                catch (Exception e)
                                {
                                    SendCDRsResults.Add(SendCDRResult.CouldNotConvertCDRFormat(chargeDetailRecord,
                                                                                               Warning.Create(I18NString.Create(Languages.eng, e.Message))));
                                }

                            }

                            Endtime      = DateTime.UtcNow;
                            Runtime      = Endtime - StartTime;
                            results      = SendCDRsResult.Enqueued(Id,
                                                                   this,
                                                                   ChargeDetailRecords,
                                                                   "Enqueued for at least " + FlushChargeDetailRecordsEvery.TotalSeconds + " seconds!",
                                                                   //SendCDRsResults.SafeWhere(cdrresult => cdrresult.Result != SendCDRResultTypes.Enqueued),
                                                                   Runtime: Runtime);
                            invokeTimer  = true;

                        }

                        #endregion

                        #region ...or send at once!

                        else
                        {

                            HTTPResponse<SetChargeDetailRecordResponse> response;
                            SendCDRResult result;

                            foreach (var chargeDetailRecord in ChargeDetailRecords)
                            {

                                try
                                {

                                    response = await CPORoaming.SetChargeDetailRecord(PartnerId,
                                                                                      chargeDetailRecord.EVSEId.Value.OperatorId.ToEMIP(CustomOperatorIdMapper),
                                                                                      chargeDetailRecord.ToEMIP(_WWCPChargeDetailRecord2eMIPChargeDetailRecord),
                                                                                      Transaction_Id.Random(),

                                                                                      null,
                                                                                      Timestamp,
                                                                                      CancellationToken,
                                                                                      EventTrackingId,
                                                                                      RequestTimeout);

                                    if (response.HTTPStatusCode        == HTTPStatusCode.OK &&
                                        response.Content               != null              &&
                                        response.Content.RequestStatus == RequestStatus.Ok)
                                    {

                                        result = SendCDRResult.Success(chargeDetailRecord);

                                    }

                                    else
                                        result = SendCDRResult.Error(chargeDetailRecord,
                                                                     Warning.Create(I18NString.Create(Languages.eng, response.HTTPBodyAsUTF8String)));

                                }
                                catch (Exception e)
                                {
                                    result = SendCDRResult.CouldNotConvertCDRFormat(chargeDetailRecord,
                                                                                    I18NString.Create(Languages.eng, e.Message));
                                }

                                SendCDRsResults.Add(result);
                                RoamingNetwork.SessionsStore.CDRForwarded(chargeDetailRecord.SessionId, result);

                            }

                            Endtime  = DateTime.UtcNow;
                            Runtime  = Endtime - StartTime;

                            if (SendCDRsResults.All(cdrresult => cdrresult.Result == SendCDRResultTypes.Success))
                                results = SendCDRsResult.Success(Id,
                                                                 this,
                                                                 ChargeDetailRecords,
                                                                 Runtime: Runtime);

                            else
                                results = SendCDRsResult.Error(Id,
                                                               this,
                                                               SendCDRsResults.
                                                                   Where (cdrresult => cdrresult.Result != SendCDRResultTypes.Success).
                                                                   Select(cdrresult => cdrresult.ChargeDetailRecord),
                                                               Runtime: Runtime);

                        }

                        #endregion

                    }

                    #region Could not get the lock for toooo long!

                    else
                    {

                        Endtime  = DateTime.UtcNow;
                        Runtime  = Endtime - StartTime;
                        results  = SendCDRsResult.Timeout(Id,
                                                          this,
                                                          ChargeDetailRecords,
                                                          "Could not " + (TransmissionType == TransmissionTypes.Enqueue ? "enqueue" : "send") + " charge detail records!",
                                                          //ChargeDetailRecords.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Timeout)),
                                                          Runtime: Runtime);

                    }

                    #endregion

                }
                finally
                {
                    if (LockTaken)
                        FlushChargeDetailRecordsLock.Release();
                }

                if (invokeTimer)
                    FlushChargeDetailRecordsTimer.Change(FlushChargeDetailRecordsEvery, TimeSpan.FromMilliseconds(-1));

            }


            #region Send OnSendCDRsResponse event

            try
            {

                OnSendCDRsResponse?.Invoke(Endtime,
                                           Timestamp.Value,
                                           this,
                                           Id.ToString(),
                                           EventTrackingId,
                                           RoamingNetwork.Id,
                                           ChargeDetailRecords,
                                           RequestTimeout,
                                           results,
                                           Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsResponse));
            }

            #endregion

            return results;

        }

        #endregion


        // -----------------------------------------------------------------------------------------------------


        #region (timer) SendHeartbeat(State)

        private void SendHeartbeat(Object State)
        {
            if (!DisableSendHeartbeats)
                SendHeartbeat2().Wait();
        }

        private async Task SendHeartbeat2()
        {

            var LockTaken = await SendHeartbeatLock.WaitAsync(0).ConfigureAwait(false);

            try
            {

                if (LockTaken)
                {

                    #region Send SendHeartbeatStarted Event...

                    var StartTime = DateTime.UtcNow;

                    SendHeartbeatStartedEvent?.Invoke(this,
                                                      StartTime,
                                                      FlushEVSEFastStatusEvery,
                                                      _SendHeartbeatsRunId);

                    #endregion

                    var SendHeartbeatResponse = await CPORoaming.SendHeartbeat(PartnerId,
                                                                               Operator_Id.Parse("DE*BDO"),
                                                                               Transaction_Id.Random(),

                                                                               null,
                                                                               DateTime.UtcNow,
                                                                               new CancellationTokenSource().Token,
                                                                               EventTracking_Id.New,
                                                                               DefaultRequestTimeout).
                                                                 ConfigureAwait(false);

                    #region Send SendHeartbeatFinished Event...

                    var EndTime = DateTime.UtcNow;

                    SendHeartbeatFinishedEvent?.Invoke(this,
                                                       StartTime,
                                                       EndTime,
                                                       EndTime - StartTime,
                                                       SendHeartbeatsEvery,
                                                       _SendHeartbeatsRunId);

                    #endregion

                    _SendHeartbeatsRunId++;

                }

            }
            catch (Exception e)
            {

                while (e.InnerException != null)
                    e = e.InnerException;

                DebugX.LogT(GetType().Name + ".SendHeartbeat '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                //OnWWCPCPOAdapterException?.Invoke(DateTime.UtcNow,
                //                                  this,
                //                                  e);

            }

            finally
            {

                if (LockTaken)
                {
                    SendHeartbeatLock.Release();
                }

                else
                    DebugX.LogT("SendHeartbeatLock exited!");

            }

        }

        #endregion

        #region (timer) FlushEVSEDataAndStatus()

        protected override Boolean SkipFlushEVSEDataAndStatusQueues()
            => EVSEsToAddQueue.              Count == 0 &&
               EVSEsToUpdateQueue.           Count == 0 &&
               EVSEStatusChangesDelayedQueue.Count == 0 &&
               EVSEsToRemoveQueue.           Count == 0;

        protected override async Task FlushEVSEDataAndStatusQueues()
        {

            #region Get a copy of all current EVSE data and delayed status

            var EVSEsToAddQueueCopy                     = new HashSet<EVSE>();
            var EVSEsToUpdateQueueCopy                  = new HashSet<EVSE>();
            var EVSEAdminStatusChangesDelayedQueueCopy  = new List<EVSEAdminStatusUpdate>();
            var EVSEStatusChangesDelayedQueueCopy       = new List<EVSEStatusUpdate>();
            var EVSEsToRemoveQueueCopy                  = new HashSet<EVSE>();
            var EVSEsUpdateLogCopy                      = new Dictionary<EVSE,            PropertyUpdateInfos[]>();
            var ChargingStationsUpdateLogCopy           = new Dictionary<ChargingStation, PropertyUpdateInfos[]>();
            var ChargingPoolsUpdateLogCopy              = new Dictionary<ChargingPool,    PropertyUpdateInfos[]>();

            await DataAndStatusLock.WaitAsync();

            try
            {

                // Copy 'EVSEs to add', remove originals...
                EVSEsToAddQueueCopy                      = new HashSet<EVSE>              (EVSEsToAddQueue);
                EVSEsToAddQueue.Clear();

                // Copy 'EVSEs to update', remove originals...
                EVSEsToUpdateQueueCopy                   = new HashSet<EVSE>              (EVSEsToUpdateQueue);
                EVSEsToUpdateQueue.Clear();

                // Copy 'EVSE status changes', remove originals...
                EVSEAdminStatusChangesDelayedQueueCopy   = new List<EVSEAdminStatusUpdate>(EVSEAdminStatusChangesDelayedQueue);
                EVSEAdminStatusChangesDelayedQueueCopy.AddRange(EVSEsToAddQueueCopy.SafeSelect(evse => new EVSEAdminStatusUpdate(evse, evse.AdminStatus, evse.AdminStatus)));
                EVSEAdminStatusChangesDelayedQueue.Clear();

                // Copy 'EVSE status changes', remove originals...
                EVSEStatusChangesDelayedQueueCopy        = new List<EVSEStatusUpdate>     (EVSEStatusChangesDelayedQueue);
                EVSEStatusChangesDelayedQueueCopy.AddRange(EVSEsToAddQueueCopy.     SafeSelect(evse => new EVSEStatusUpdate     (evse, evse.Status,      evse.Status)));
                EVSEStatusChangesDelayedQueue.Clear();

                // Copy 'EVSEs to remove', remove originals...
                EVSEsToRemoveQueueCopy                   = new HashSet<EVSE>              (EVSEsToRemoveQueue);
                EVSEsToRemoveQueue.Clear();

                // Copy EVSE property updates
                EVSEsUpdateLog.           ForEach(_ => EVSEsUpdateLogCopy.           Add(_.Key, _.Value.ToArray()));
                EVSEsUpdateLog.Clear();

                // Copy charging station property updates
                ChargingStationsUpdateLog.ForEach(_ => ChargingStationsUpdateLogCopy.Add(_.Key, _.Value.ToArray()));
                ChargingStationsUpdateLog.Clear();

                // Copy charging pool property updates
                ChargingPoolsUpdateLog.   ForEach(_ => ChargingPoolsUpdateLogCopy.   Add(_.Key, _.Value.ToArray()));
                ChargingPoolsUpdateLog.Clear();


                // Stop the timer. Will be rescheduled by next EVSE data/status change...
                FlushEVSEDataAndStatusTimer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

            }
            finally
            {
                DataAndStatusLock.Release();
            }

            #endregion

            // Use events to check if something went wrong!
            var EventTrackingId = EventTracking_Id.New;

            //Thread.Sleep(30000);

            #region Send new EVSE data

            if (EVSEsToAddQueueCopy.Count > 0)
            {

                //var EVSEsToAddTask = PushEVSEData(EVSEsToAddQueueCopy,
                //                                  _FlushEVSEDataRunId == 1
                //                                      ? ActionTypes.fullLoad
                //                                      : ActionTypes.update,
                //                                  EventTrackingId: EventTrackingId);

                //EVSEsToAddTask.Wait();

                //if (EVSEsToAddTask.Result.Warnings.Any())
                //{

                //    SendOnWarnings(DateTime.UtcNow,
                //                   nameof(WWCPCPOAdapter) + Id,
                //                   "EVSEsToAddTask",
                //                   EVSEsToAddTask.Result.Warnings);

                //}

            }

            #endregion

            #region Send changed EVSE data

            if (EVSEsToUpdateQueueCopy.Count > 0)
            {

                // Surpress EVSE data updates for all newly added EVSEs
                var EVSEsWithoutNewEVSEs = EVSEsToUpdateQueueCopy.
                                               Where(evse => !EVSEsToAddQueueCopy.Contains(evse)).
                                               ToArray();


                if (EVSEsWithoutNewEVSEs.Length > 0)
                {

                    //var PushEVSEDataTask = PushEVSEData(EVSEsWithoutNewEVSEs,
                    //                                    ActionTypes.update,
                    //                                    EventTrackingId: EventTrackingId);

                    //PushEVSEDataTask.Wait();

                    //if (PushEVSEDataTask.Result.Warnings.Any())
                    //{

                    //    SendOnWarnings(DateTime.UtcNow,
                    //                   nameof(WWCPCPOAdapter) + Id,
                    //                   "PushEVSEDataTask",
                    //                   PushEVSEDataTask.Result.Warnings);

                    //}

                }

            }

            #endregion

            #region Send changed EVSE admin status

            if (!DisablePushStatus &&
                EVSEAdminStatusChangesDelayedQueueCopy.Count > 0)
            {

                var pushEVSEAdminStatusResult = await SetEVSEAvailabilityStatus(EVSEAdminStatusChangesDelayedQueueCopy,

                                                                                DateTime.UtcNow,
                                                                                new CancellationTokenSource().Token,
                                                                                EventTrackingId,
                                                                                DefaultRequestTimeout).
                                                          ConfigureAwait(false);

                if (pushEVSEAdminStatusResult.Warnings.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "SetEVSEAvailabilityStatus",
                                   pushEVSEAdminStatusResult.Warnings);

                }

            }

            #endregion

            #region Send changed EVSE status

            if (!DisablePushStatus &&
                EVSEStatusChangesDelayedQueueCopy.Count > 0)
            {

                var pushEVSEStatusResult = await SetEVSEBusyStatus(EVSEStatusChangesDelayedQueueCopy,

                                                                   DateTime.UtcNow,
                                                                   new CancellationTokenSource().Token,
                                                                   EventTrackingId,
                                                                   DefaultRequestTimeout).
                                                     ConfigureAwait(false);

                if (pushEVSEStatusResult.Warnings.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "SetEVSEBusyStatus",
                                   pushEVSEStatusResult.Warnings);

                }

            }

            #endregion

            #region Send removed charging stations

            if (EVSEsToRemoveQueueCopy.Count > 0)
            {

                var EVSEsToRemove = EVSEsToRemoveQueueCopy.ToArray();

                if (EVSEsToRemove.Length > 0)
                {

                    //var EVSEsToRemoveTask = PushEVSEData(EVSEsToRemove,
                    //                                     ActionTypes.delete,
                    //                                     EventTrackingId: EventTrackingId);

                    //EVSEsToRemoveTask.Wait();

                    //if (EVSEsToRemoveTask.Result.Warnings.Any())
                    //{

                    //    SendOnWarnings(DateTime.UtcNow,
                    //                   nameof(WWCPCPOAdapter) + Id,
                    //                   "EVSEsToRemoveTask",
                    //                   EVSEsToRemoveTask.Result.Warnings);

                    //}

                }

            }

            #endregion

        }

        #endregion

        #region (timer) FlushEVSEFastStatus()

        protected override Boolean SkipFlushEVSEFastStatusQueues()
            => EVSEAdminStatusChangesFastQueue.Count == 0 &&
               EVSEStatusChangesFastQueue.     Count == 0;

        protected override async Task FlushEVSEFastStatusQueues()
        {

            #region Get a copy of all current EVSE data and delayed status

            var EVSEAdminStatusFastQueueCopy  = new List<EVSEAdminStatusUpdate>();
            var EVSEStatusFastQueueCopy       = new List<EVSEStatusUpdate>();

            var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

            try
            {

                if (LockTaken)
                {

                    // Copy 'EVSE status changes', remove originals...
                    EVSEAdminStatusFastQueueCopy = new List<EVSEAdminStatusUpdate>(EVSEAdminStatusChangesFastQueue.Where(statuschange => !EVSEsToAddQueue.Any(evse => evse == statuschange.EVSE)));
                    EVSEStatusFastQueueCopy = new List<EVSEStatusUpdate>(EVSEStatusChangesFastQueue.Where(statuschange => !EVSEsToAddQueue.Any(evse => evse == statuschange.EVSE)));

                    // Add all evse status changes of EVSEs *NOT YET UPLOADED* into the delayed queue...
                    var EVSEAdminStatusChangesDelayed = EVSEAdminStatusChangesFastQueue.Where(statuschange => EVSEsToAddQueue.Any(evse => evse == statuschange.EVSE)).ToArray();
                    var EVSEStatusChangesDelayed = EVSEStatusChangesFastQueue.Where(statuschange => EVSEsToAddQueue.Any(evse => evse == statuschange.EVSE)).ToArray();

                    if (EVSEAdminStatusChangesDelayed.Length > 0)
                        EVSEAdminStatusChangesDelayedQueue.AddRange(EVSEAdminStatusChangesDelayed);

                    if (EVSEStatusChangesDelayed.Length > 0)
                        EVSEStatusChangesDelayedQueue.AddRange(EVSEStatusChangesDelayed);

                    EVSEAdminStatusChangesFastQueue.Clear();
                    EVSEStatusChangesFastQueue.Clear();


                    // Stop the timer. Will be rescheduled by next EVSE status change...
                    FlushEVSEFastStatusTimer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

                }

            }
            finally
            {
                if (LockTaken)
                    DataAndStatusLock.Release();
            }

            #endregion

            // Use events to check if something went wrong!
            var EventTrackingId = EventTracking_Id.New;

            #region Send changed EVSE admin status

            if (EVSEAdminStatusFastQueueCopy.Count > 0)
            {

                var pushEVSEAdminStatusResult = await SetEVSEAvailabilityStatus(EVSEAdminStatusFastQueueCopy,

                                                                                DateTime.UtcNow,
                                                                                new CancellationTokenSource().Token,
                                                                                EventTrackingId,
                                                                                DefaultRequestTimeout).
                                                          ConfigureAwait(false);

                if (pushEVSEAdminStatusResult.Warnings.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "SetEVSEAvailabilityStatus",
                                   pushEVSEAdminStatusResult.Warnings);

                }

            }

            #endregion

            #region Send changed EVSE status

            if (EVSEStatusFastQueueCopy.Count > 0)
            {

                var pushEVSEStatusResult = await SetEVSEBusyStatus(EVSEStatusFastQueueCopy,

                                                                   DateTime.UtcNow,
                                                                   new CancellationTokenSource().Token,
                                                                   EventTrackingId,
                                                                   DefaultRequestTimeout).
                                                     ConfigureAwait(false);

                if (pushEVSEStatusResult.Warnings.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "SetEVSEBusyStatus",
                                   pushEVSEStatusResult.Warnings);

                }

            }

            #endregion

        }

        #endregion

        #region (timer) FlushChargeDetailRecords()

        protected override Boolean SkipFlushChargeDetailRecordsQueues()
            => ChargeDetailRecordsQueue.Count == 0;

        protected override async Task FlushChargeDetailRecordsQueues(IEnumerable<ChargeDetailRecord> ChargeDetailRecords)
        {

            HTTPResponse<SetChargeDetailRecordResponse> response;
            SendCDRResult result;

            foreach (var chargeDetailRecord in ChargeDetailRecords)
            {

                try
                {

                    response = await CPORoaming.SetChargeDetailRecord(PartnerId,
                                                                      chargeDetailRecord.EVSEId.OperatorId,
                                                                      chargeDetailRecord,
                                                                      Transaction_Id.Random(),

                                                                      null,
                                                                      DateTime.UtcNow,
                                                                      new CancellationTokenSource().Token,
                                                                      EventTracking_Id.New,
                                                                      DefaultRequestTimeout);

                    if (response.HTTPStatusCode        == HTTPStatusCode.OK &&
                        response.Content               != null              &&
                        response.Content.RequestStatus == RequestStatus.Ok)
                    {

                        result = SendCDRResult.Success(chargeDetailRecord.GetCustomDataAs<WWCP.ChargeDetailRecord>(eMIPMapper.WWCP_CDR));

                    }

                    else
                        result = SendCDRResult.Error(chargeDetailRecord.GetCustomDataAs<WWCP.ChargeDetailRecord>(eMIPMapper.WWCP_CDR),
                                                     Warning.Create(I18NString.Create(Languages.eng, response.HTTPBodyAsUTF8String)));

                }
                catch (Exception e)
                {
                    result = SendCDRResult.CouldNotConvertCDRFormat(chargeDetailRecord.GetCustomDataAs<WWCP.ChargeDetailRecord>(eMIPMapper.WWCP_CDR),
                                                                    Warning.Create(I18NString.Create(Languages.eng, e.Message)));
                }

                RoamingNetwork.SessionsStore.CDRForwarded(chargeDetailRecord.ServiceSessionId.ToWWCP(), result);

            }

            //ToDo: Re-add to queue if it could not be send...

        }

        #endregion


        // -----------------------------------------------------------------------------------------------------


        #region Operator overloading

        #region Operator == (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two WWCPCPOAdapters for equality.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (WWCPCPOAdapter WWCPCPOAdapter1, WWCPCPOAdapter WWCPCPOAdapter2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(WWCPCPOAdapter1, WWCPCPOAdapter2))
                return true;

            // If one is null, but not both, return false.
            if (WWCPCPOAdapter1 is null || WWCPCPOAdapter2 is null)
                return false;

            return WWCPCPOAdapter1.Equals(WWCPCPOAdapter2);

        }

        #endregion

        #region Operator != (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two WWCPCPOAdapters for inequality.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (WWCPCPOAdapter WWCPCPOAdapter1, WWCPCPOAdapter WWCPCPOAdapter2)
            => !(WWCPCPOAdapter1 == WWCPCPOAdapter2);

        #endregion

        #region Operator <  (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (WWCPCPOAdapter  WWCPCPOAdapter1, WWCPCPOAdapter  WWCPCPOAdapter2)
        {

            if (WWCPCPOAdapter1 is null)
                throw new ArgumentNullException(nameof(WWCPCPOAdapter1),  "The given WWCPCPOAdapter must not be null!");

            return WWCPCPOAdapter1.CompareTo(WWCPCPOAdapter2) < 0;

        }

        #endregion

        #region Operator <= (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (WWCPCPOAdapter WWCPCPOAdapter1, WWCPCPOAdapter WWCPCPOAdapter2)
            => !(WWCPCPOAdapter1 > WWCPCPOAdapter2);

        #endregion

        #region Operator >  (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (WWCPCPOAdapter WWCPCPOAdapter1, WWCPCPOAdapter WWCPCPOAdapter2)
        {

            if (WWCPCPOAdapter1 is null)
                throw new ArgumentNullException(nameof(WWCPCPOAdapter1),  "The given WWCPCPOAdapter must not be null!");

            return WWCPCPOAdapter1.CompareTo(WWCPCPOAdapter2) > 0;

        }

        #endregion

        #region Operator >= (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (WWCPCPOAdapter WWCPCPOAdapter1, WWCPCPOAdapter WWCPCPOAdapter2)
            => !(WWCPCPOAdapter1 < WWCPCPOAdapter2);

        #endregion

        #endregion

        #region IComparable<WWCPCPOAdapter> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is WWCPCPOAdapter))
                throw new ArgumentException("The given object is not an WWCPCPOAdapter!", nameof(Object));

            return CompareTo(Object as WWCPCPOAdapter);

        }

        #endregion

        #region CompareTo(WWCPCPOAdapter)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter">An WWCPCPOAdapter object to compare with.</param>
        public Int32 CompareTo(WWCPCPOAdapter WWCPCPOAdapter)
        {

            if (WWCPCPOAdapter is null)
                throw new ArgumentNullException(nameof(WWCPCPOAdapter), "The given WWCPCPOAdapter must not be null!");

            return Id.CompareTo(WWCPCPOAdapter.Id);

        }

        #endregion

        #endregion

        #region IEquatable<WWCPCPOAdapter> Members

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

            if (!(Object is WWCPCPOAdapter))
                return false;

            return Equals(Object as WWCPCPOAdapter);

        }

        #endregion

        #region Equals(WWCPCPOAdapter)

        /// <summary>
        /// Compares two WWCPCPOAdapter for equality.
        /// </summary>
        /// <param name="WWCPCPOAdapter">An WWCPCPOAdapter to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(WWCPCPOAdapter WWCPCPOAdapter)
        {

            if (WWCPCPOAdapter is null)
                return false;

            return Id.Equals(WWCPCPOAdapter.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => "eMIP" + Version.Number + " CPO Adapter " + Id;

        #endregion


    }

}
