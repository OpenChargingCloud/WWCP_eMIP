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

using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

using cloud.charging.open.protocols.WWCP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.CPO
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

        private        readonly  WWCPChargeDetailRecord2ChargeDetailRecordDelegate?   WWCPChargeDetailRecord2eMIPChargeDetailRecord;

        //private        readonly  EVSEDataRecord2XMLDelegate                             _EVSEDataRecord2XML;

        //private        readonly  EVSEStatusRecord2XMLDelegate                           _EVSEStatusRecord2XML;

        //private        readonly  ChargeDetailRecord2XMLDelegate                         _ChargeDetailRecord2XML;

        private static readonly  Regex                                                pattern                             = new (@"\s=\s");

        /// <summary>
        /// The default send heartbeats intervall.
        /// </summary>
        public  readonly static  TimeSpan                                             DefaultSendHeartbeatsEvery          = TimeSpan.FromMinutes(5);

        private readonly         SemaphoreSlim                                        SendHeartbeatLock                   = new (1, 1);
        private readonly         Timer                                                SendHeartbeatsTimer;

        private                  UInt64                                               _SendHeartbeatsRunId                = 1;

        protected readonly CustomOperatorIdMapperDelegate?  CustomOperatorIdMapper;

        protected readonly CustomEVSEIdMapperDelegate?      CustomEVSEIdMapper;


        /// <summary>
        /// The default logging context.
        /// </summary>
        public  const       String         DefaultLoggingContext        = "eMIPv0_7_4_CPOAdapter";

        public  const       String         DefaultHTTPAPI_LoggingPath   = "default";

        public  const       String         DefaultHTTPAPI_LogfileName   = "eMIPv0_7_4_CPOAdapter.log";


        /// <summary>
        /// The request timeout.
        /// </summary>
        public readonly     TimeSpan       RequestTimeout               = TimeSpan.FromSeconds(60);

        #endregion

        #region Properties

        IId IAuthorizeStartStop.AuthId
            => Id;

        IId ISendChargeDetailRecords.SendChargeDetailRecordsId
            => Id;

        /// <summary>
        /// The wrapped CPO roaming object.
        /// </summary>
        public CPORoaming CPORoaming { get; }

        public CPOClient? CPOClient
            => CPORoaming?.CPOClient;

        public CPOServer? CPOServer
            => CPORoaming?.CPOServer;



        /// <summary>
        /// The partner identification.
        /// </summary>
        public Partner_Id                PartnerId                { get; }


        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                   DisableSendHeartbeats    { get; set; }

        public TimeSpan                  SendHeartbeatsEvery      { get; }

        /// <summary>
        /// An optional default charging station operator identification.
        /// </summary>
        public IChargingStationOperator  DefaultOperator          { get; }

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
        public WWCPCPOAdapter(CSORoamingProvider_Id                               Id,
                              I18NString                                          Name,
                              I18NString                                          Description,
                              RoamingNetwork                                      RoamingNetwork,
                              CPORoaming                                          CPORoaming,

                              Partner_Id                                          PartnerId,
                              IChargingStationOperator                            DefaultOperator,

                              IncludeEVSEIdDelegate?                              IncludeEVSEIds                                  = null,
                              IncludeEVSEDelegate?                                IncludeEVSEs                                    = null,
                              IncludeChargingStationIdDelegate?                   IncludeChargingStationIds                       = null,
                              IncludeChargingStationDelegate?                     IncludeChargingStations                         = null,
                              IncludeChargingPoolIdDelegate?                      IncludeChargingPoolIds                          = null,
                              IncludeChargingPoolDelegate?                        IncludeChargingPools                            = null,
                              ChargeDetailRecordFilterDelegate?                   ChargeDetailRecordFilter                        = null,
                              CustomOperatorIdMapperDelegate?                     CustomOperatorIdMapper                          = null,
                              CustomEVSEIdMapperDelegate?                         CustomEVSEIdMapper                              = null,

                              //EVSE2EVSEDataRecordDelegate                         EVSE2EVSEDataRecord                             = null,
                              //EVSEStatusUpdate2EVSEStatusRecordDelegate           EVSEStatusUpdate2EVSEStatusRecord               = null,
                              WWCPChargeDetailRecord2ChargeDetailRecordDelegate?  WWCPChargeDetailRecord2eMIPChargeDetailRecord   = null,

                              TimeSpan?                                           SendHeartbeatsEvery                             = null,
                              TimeSpan?                                           ServiceCheckEvery                               = null,
                              TimeSpan?                                           StatusCheckEvery                                = null,
                              TimeSpan?                                           CDRCheckEvery                                   = null,

                              Boolean                                             DisableSendHeartbeats                           = false,
                              Boolean                                             DisablePushData                                 = false,
                              Boolean                                             DisablePushAdminStatus                          = true,
                              Boolean                                             DisablePushStatus                               = false,
                              Boolean                                             DisableAuthentication                           = false,
                              Boolean                                             DisableSendChargeDetailRecords                  = false,

                              String                                              EllipticCurve                                   = "P-256",
                              ECPrivateKeyParameters?                             PrivateKey                                      = null,
                              PublicKeyCertificates?                              PublicKeyCertificates                           = null,

                              Boolean?                                            IsDevelopment                                   = null,
                              IEnumerable<String>?                                DevelopmentServers                              = null,
                              Boolean?                                            DisableLogging                                  = false,
                              String?                                             LoggingPath                                     = DefaultHTTPAPI_LoggingPath,
                              String?                                             LoggingContext                                  = DefaultLoggingContext,
                              String?                                             LogfileName                                     = DefaultHTTPAPI_LogfileName,
                              LogfileCreatorDelegate?                             LogfileCreator                                  = null,

                              String?                                             ClientsLoggingPath                              = DefaultHTTPAPI_LoggingPath,
                              String?                                             ClientsLoggingContext                           = DefaultLoggingContext,
                              LogfileCreatorDelegate?                             ClientsLogfileCreator                           = null,
                              DNSClient?                                          DNSClient                                       = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   IncludeChargingStationIds,
                   IncludeChargingStations,
                   IncludeChargingPoolIds,
                   IncludeChargingPools,
                   null,
                   null,
                   ChargeDetailRecordFilter,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   null,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushAdminStatus,
                   DisablePushStatus,
                   true,
                   true,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,

                   IsDevelopment,
                   DevelopmentServers,
                   DisableLogging,
                   LoggingPath,
                   LoggingContext,
                   LogfileName,
                   LogfileCreator,

                   ClientsLoggingPath,
                   ClientsLoggingContext,
                   ClientsLogfileCreator,
                   DNSClient)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),        "The given roaming provider name must not be null or empty!");

            if (CPORoaming is null)
                throw new ArgumentNullException(nameof(CPORoaming),  "The given eMIP CPO Roaming object must not be null!");

            #endregion

            this.PartnerId                                       = PartnerId;
            this.DefaultOperator                                 = DefaultOperator ?? throw new ArgumentNullException(nameof(DefaultOperator),  "The given charging station operator must not be null!");
            this.CPORoaming                                      = CPORoaming      ?? throw new ArgumentNullException(nameof(CPORoaming),       "The given charging station operator roaming must not be null!");

            //this._EVSE2EVSEDataRecord                            = EVSE2EVSEDataRecord;
            //this._EVSEStatusUpdate2EVSEStatusRecord              = EVSEStatusUpdate2EVSEStatusRecord;
            this.WWCPChargeDetailRecord2eMIPChargeDetailRecord  = WWCPChargeDetailRecord2eMIPChargeDetailRecord;

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

                try
                {

                    // bookingId
                    // meterLimitList

                    #region Request mapping

                    var chargingProduct = new ChargingProduct(Id:                ChargingProduct_Id.NewRandom(),
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

                    var evseId = Request.EVSEId.ToWWCP();

                    if (!evseId.HasValue)
                        return new SetServiceAuthorisationResponse(
                                   Request,
                                   Request.TransactionId ?? Transaction_Id.Zero,
                                   RequestStatus.EVSENotReachable
                               );

                    var response = await RoamingNetwork.RemoteStart(
                                             CSORoamingProvider:       this,
                                             ChargingLocation:         ChargingLocation.FromEVSEId(evseId.Value),
                                             ChargingProduct:          chargingProduct,
                                             ReservationId:            null,
                                             SessionId:                Request.ServiceSessionId.    ToWWCP(),
                                             ProviderId:               Request.OperatorId.          ToWWCP_ProviderId(),
                                             RemoteAuthentication:     Request.UserContractIdAlias?.ToWWCP()
                                                                           ?? Request.UserId.       ToWWCP(),
                                             AdditionalSessionInfos:   JSONObject.Create(
                                                                           new JProperty("eMIP.userIdType",  Request.UserId.Format.AsText()),
                                                                           new JProperty("eMIP.userId",      Request.UserId.       ToString())
                                                                       ),
                                             AuthenticationPath:       Auth_Path.Parse(Id.ToString()),  // CSO Roaming Provider identification!

                                             Timestamp:                Request.Timestamp,
                                             EventTrackingId:          Request.EventTrackingId,
                                             RequestTimeout:           Request.RequestTimeout,
                                             CancellationToken:        Request.CancellationToken
                                         ).ConfigureAwait(false);


                    #region Add additional Gireve session infos

                    if (response.Session is not null)
                    {

                        await RoamingNetwork.SessionsStore.UpdateSession(
                                  response.Session.Id,
                                  session => {

                                      //var Gireve = session.AddJSON("Gireve");

                                      session.SetInternalData("Gireve.request",              Request.ToXML().ToString());


                                      if (Request.UserContractIdAlias.HasValue)
                                          session.SetInternalData("Gireve.userContractIdAlias",  Request.UserContractIdAlias.Value.ToString());

                                      if (Request.Parameter.IsNotNullOrEmpty())
                                          session.SetInternalData("Gireve.parameter",            Request.Parameter);

                                      if (Request.HTTPRequest is not null)
                                      {

                                          session.SetInternalData("Gireve.remoteIPAddress",      Request.HTTPRequest.RemoteSocket.IPAddress.ToString());

                                          if (Request.HTTPRequest.X_Real_IP       is not null)
                                              session.SetInternalData("Gireve.realIP",           Request.HTTPRequest.X_Real_IP.ToString());

                                          if (Request.HTTPRequest.X_Forwarded_For is not null)
                                              session.SetInternalData("Gireve.forwardedFor",     new JArray(Request.HTTPRequest.X_Forwarded_For.Select(addr => addr.ToString()).AggregateWith(',')));

                                      }

                                  }
                              );

                    }

                    #endregion

                    #region Response mapping

                    if (response is not null)
                    {
                        switch (response.Result)
                        {

                            case RemoteStartResultTypes.Success:
                            case RemoteStartResultTypes.AsyncOperation:
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


                }
                catch (Exception e)
                {

                    return new SetServiceAuthorisationResponse(
                               Request,
                               Request.TransactionId ?? Transaction_Id.Zero,
                               RequestStatus.DataError
                           );

                }

            };

            #endregion

            #region OnSetSessionAction

            this.CPORoaming.OnSetSessionAction += async (Timestamp,
                                                         Sender,
                                                         Request) => {

                try
                {

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
                                                 RemoteStop(this,
                                                            Request.ServiceSessionId.ToWWCP(),
                                                            ReservationHandling.Close,
                                                            Request.OperatorId.ToWWCP_ProviderId(),
                                                            null,
                                                            WWCP.Auth_Path.Parse(Id.ToString()),   // Authentication path == CSO Roaming Provider identification!

                                                            Request.Timestamp,
                                                            Request.EventTrackingId,
                                                            Request.RequestTimeout,
                                                            Request.CancellationToken).
                                                 ConfigureAwait(false);


                        #region Response mapping

                        if (response != null)
                        {
                            switch (response.Result)
                            {

                                case RemoteStopResultTypes.Success:
                                case RemoteStopResultTypes.AsyncOperation:
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

                }
                catch (Exception e)
                {

                    return new SetSessionActionRequestResponse(
                               Request,
                               Request.TransactionId ?? Transaction_Id.Zero,
                               RequestStatus.DataError
                           );

                }

            };

            #endregion

        }

        #endregion



        // RN -> External service requests...

        #region PushEVSEData/-Status directly...

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
                                      CancellationToken                   CancellationToken   = default,
                                      EventTracking_Id                    EventTrackingId     = null,
                                      TimeSpan?                           RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEAdminStatusUpdates == null || !EVSEAdminStatusUpdates.Any())
                return PushEVSEAdminStatusResult.NoOperation(Id, this);


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPORoaming.CPOClient.RequestTimeout;

            #endregion

            #region Get effective number of EVSEAdminStatus/EVSEAdminStatusRecords to upload

            var Warnings = new List<Warning>();

            var _EVSEAvailabilityAdminStatus = EVSEAdminStatusUpdates.
                                  ToLookup    (evsestatusupdate => evsestatusupdate.Id,
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

                                          Warnings.Add(
                                              Warning.Create(
                                                  e.Message,
                                                  evsestatusupdate
                                              )
                                          );

                                      }

                                      return null;

                                  }).
                                  Where(evsestatusrecord => evsestatusrecord != null).
                                  ToArray();

            PushEVSEAdminStatusResult result = null;
            var results = new List<PushEVSEAdminStatusResult>();

            #endregion

            #region Send OnEVSEAdminStatusPush event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
            //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEAdminStatusWWCPRequest));
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


                var Endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
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
                                                               ? Warnings.AddAndReturnList(I18NString.Create(response.HTTPBody.ToUTF8String()))
                                                               : Warnings.AddAndReturnList(I18NString.Create("No HTTP body received!")),
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
            //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEAdminStatusWWCPResponse));
            //}

            #endregion

            return PushEVSEAdminStatusResult.Flatten(Id,
                                                     this,
                                                     results,
                                                     org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - StartTime);

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
                              CancellationToken              CancellationToken   = default,
                              EventTracking_Id               EventTrackingId     = null,
                              TimeSpan?                      RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEStatusUpdates == null || !EVSEStatusUpdates.Any())
                return PushEVSEStatusResult.NoOperation(Id, this);


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPORoaming.CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSEStatus/EVSEStatusRecords to upload

            var Warnings = new List<Warning>();

            var _EVSEBusyStatus = EVSEStatusUpdates.
                                  ToLookup    (evsestatusupdate => evsestatusupdate.Id,
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

                                          Warnings.Add(
                                              Warning.Create(
                                                  e.Message,
                                                  evsestatusupdate
                                              )
                                          );

                                      }

                                      return null;

                                  }).
                                  Where(evsestatusrecord => evsestatusrecord != null).
                                  ToArray();

            PushEVSEStatusResult? result = null;
            var results = new List<PushEVSEStatusResult>();

            #endregion

            #region Send OnEVSEStatusPush event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
            //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEStatusWWCPRequest));
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


                var Endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
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
                                                               ? Warnings.AddAndReturnList(I18NString.Create(response.HTTPBody.ToUTF8String()))
                                                               : Warnings.AddAndReturnList(I18NString.Create("No HTTP body received!")),
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
            //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEStatusWWCPResponse));
            //}

            #endregion

            return PushEVSEStatusResult.Flatten(Id,
                                                this,
                                                results,
                                                org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - StartTime);

        }

        #endregion


        #region (Set/Add/Update/Delete) EVSE(s)...

        #region UpdateEVSEAdminStatus(EVSEAdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE admin status updates.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        async Task<PushEVSEAdminStatusResult>

            ISendAdminStatus.UpdateEVSEAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  EVSEAdminStatusUpdates,
                                                   TransmissionTypes                   TransmissionType,

                                                   DateTime?                           Timestamp,
                                                   EventTracking_Id?                   EventTrackingId,
                                                   TimeSpan?                           RequestTimeout,
                                                   CancellationToken                   CancellationToken)

        {

            #region Initial checks

            if (!EVSEAdminStatusUpdates.Any())
                return PushEVSEAdminStatusResult.NoOperation(Id,
                                                         this);

            if (DisableSendStatus)
                return PushEVSEAdminStatusResult.AdminDown(Id,
                                                           this,
                                                           EVSEAdminStatusUpdates);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                return await UpdateStatus(this,
                                          EVSEAdminStatusUpdates,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout).

                             ConfigureAwait(false);

            }

            #endregion

            return await SetEVSEAvailabilityStatus(EVSEAdminStatusUpdates,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout).

                         ConfigureAwait(false);

        }

        #endregion

        #region UpdateEVSEStatus     (EVSEStatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="EVSEStatusUpdates">An enumeration of EVSE status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        async Task<PushEVSEStatusResult>

            ISendStatus.UpdateEVSEStatus(IEnumerable<EVSEStatusUpdate>  EVSEStatusUpdates,
                                         TransmissionTypes              TransmissionType,

                                         DateTime?                      Timestamp,
                                         EventTracking_Id?              EventTrackingId,
                                         TimeSpan?                      RequestTimeout,
                                         CancellationToken              CancellationToken)

        {

            #region Initial checks

            if (!EVSEStatusUpdates.Any())
                return PushEVSEStatusResult.NoOperation(Id, this);

            PushEVSEStatusResult? result = null;

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var invokeTimer  = false;
                var LockTaken    = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var FilteredUpdates = EVSEStatusUpdates.Where(statusupdate => IncludeEVSEIds(statusupdate.Id)).
                                                            ToArray();

                        if (FilteredUpdates.Length > 0)
                        {

                            foreach (var update in FilteredUpdates)
                            {

                                // Delay the status update until the EVSE data had been uploaded!
                                if (evsesToAddQueue.Any(evse => evse.Id == update.Id))
                                    evseStatusChangesDelayedQueue.Add(update);

                                else
                                    evseStatusChangesFastQueue.Add(update);

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

            return await SetEVSEBusyStatus(EVSEStatusUpdates,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout).

                         ConfigureAwait(false);

        }

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
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          AuthIdentification,
                           ChargingLocation?            ChargingLocation      = null,
                           ChargingProduct?             ChargingProduct       = null,   // [maxlength: 100]
                           ChargingSession_Id?          SessionId             = null,
                           ChargingSession_Id?          CPOPartnerSessionId   = null,
                           ChargingStationOperator_Id?  OperatorId            = null,

                           DateTime?                    Timestamp             = null,
                           EventTracking_Id?            EventTrackingId       = null,
                           TimeSpan?                    RequestTimeout        = null,
                           CancellationToken            CancellationToken     = default)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPORoaming.CPOClient.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
                                                CPOPartnerSessionId,
                                                new ISendAuthorizeStartStop[0],
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            DateTime             Endtime;
            TimeSpan             Runtime;
            AuthStartResult  result;

            if (ChargingLocation?.EVSEId == null)
            {

                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.UnknownLocation(Id,
                                                           this,
                                                           SessionId:  SessionId,
                                                           Runtime:    Runtime);

            }

            else if (DisableAuthentication)
            {

                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.AdminDown(Id,
                                                         this,
                                                         SessionId:  SessionId,
                                                         Runtime:    Runtime);

            }

            else
            {

                var response  = await CPORoaming.
                                          GetServiceAuthorisation(PartnerId:                PartnerId,
                                                                  OperatorId:               (ChargingLocation.EVSEId?.OperatorId ?? OperatorId ?? DefaultOperator.Id).ToEMIP(CustomOperatorIdMapper),
                                                                  EVSEId:                   WWCP.EVSE_Id.Parse(CustomEVSEIdMapper(ChargingLocation.EVSEId.ToString())).ToEMIP().Value,
                                                                  UserId:                   User_Id.Parse(AuthIdentification.AuthToken.ToString()),
                                                                  RequestedServiceId:       Service_Id.GenericChargeService,
                                                                  TransactionId:            Transaction_Id.Random(),
                                                                  PartnerServiceSessionId:  new PartnerServiceSession_Id?(),

                                                                  Timestamp:                Timestamp,
                                                                  CancellationToken:        CancellationToken,
                                                                  EventTrackingId:          EventTrackingId,
                                                                  RequestTimeout:           RequestTimeout);


                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;

                if (response?.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response?.Content                     is not null          &&
                    response?.Content.RequestStatus.Code  == 1                 &&
                    response?.Content.AuthorisationValue  == AuthorisationValues.OK)
                {

                    result = AuthStartResult.Authorized(
                                 Id,
                                 this,
                                 SessionId:        ChargingSession_Id.Parse(response.Content.ServiceSessionId.ToString()),
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
                                 SessionId:        SessionId,
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
                                                 CPOPartnerSessionId,
                                                 new ISendAuthorizeStartStop[0],
                                                 RequestTimeout,
                                                 result,
                                                 Runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStartResponse));
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
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          AuthIdentification,
                          ChargingLocation?            ChargingLocation      = null,
                          ChargingSession_Id?          CPOPartnerSessionId   = null,
                          ChargingStationOperator_Id?  OperatorId            = null,

                          DateTime?                    Timestamp             = null,
                          EventTracking_Id?            EventTrackingId       = null,
                          TimeSpan?                    RequestTimeout        = null,
                          CancellationToken            CancellationToken     = default)
        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPORoaming.CPOClient.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
                                               CPOPartnerSessionId,
                                               AuthIdentification,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            DateTime        Endtime;
            TimeSpan        Runtime;
            AuthStopResult  result;

            if (ChargingLocation?.EVSEId is null)
            {

                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.UnknownLocation(Id,
                                                          this,
                                                          SessionId:  SessionId,
                                                          Runtime:    Runtime);

            }

            else if (DisableAuthentication)
            {

                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.AdminDown(Id,
                                                    this,
                                                    SessionId:  SessionId,
                                                    Runtime:    Runtime);

            }

            else
            {

                var response  = await CPORoaming.
                                          GetServiceAuthorisation(PartnerId:                PartnerId,
                                                                  OperatorId:               (ChargingLocation.EVSEId?.OperatorId ?? OperatorId ?? DefaultOperator.Id).ToEMIP(CustomOperatorIdMapper),
                                                                  EVSEId:                   WWCP.EVSE_Id.Parse(CustomEVSEIdMapper(ChargingLocation.EVSEId.ToString())).ToEMIP().Value,
                                                                  UserId:                   User_Id.Parse(AuthIdentification.AuthToken.ToString()),
                                                                  RequestedServiceId:       Service_Id.GenericChargeService,
                                                                  TransactionId:            Transaction_Id.Random(),
                                                                  PartnerServiceSessionId:  new PartnerServiceSession_Id?(),

                                                                  Timestamp:                Timestamp,
                                                                  CancellationToken:        CancellationToken,
                                                                  EventTrackingId:          EventTrackingId,
                                                                  RequestTimeout:           RequestTimeout);


                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;

                if (response?.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response?.Content                     is not null          &&
                    response?.Content.RequestStatus.Code  == 1                 &&
                    response?.Content.AuthorisationValue  == AuthorisationValues.OK)
                {

                    result = AuthStopResult.Authorized(
                                 Id,
                                 this,
                                 SessionId:        ChargingSession_Id.Parse(response.Content.ServiceSessionId.ToString()),
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
                                 SessionId:        SessionId,
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
                                                CPOPartnerSessionId,
                                                AuthIdentification,
                                                RequestTimeout,
                                                result,
                                                Runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SendChargeDetailRecord (ChargeDetailRecord,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Send a charge detail record to an eMIP server.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="TransmissionType">Whether to send the CDR directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(WWCP.ChargeDetailRecord  ChargeDetailRecord,
                                   TransmissionTypes        TransmissionType    = TransmissionTypes.Enqueue,

                                   DateTime?                Timestamp           = null,
                                   EventTracking_Id?        EventTrackingId     = null,
                                   TimeSpan?                RequestTimeout      = null,
                                   CancellationToken        CancellationToken   = default)

            => (await SendChargeDetailRecords(
                      [ ChargeDetailRecord ],
                      TransmissionType,
                      Timestamp,
                      EventTrackingId,
                      RequestTimeout,
                      CancellationToken)).First();

        #endregion

        #region SendChargeDetailRecords(ChargeDetailRecords, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Send charge detail records to an eMIP server.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the CDR directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<WWCP.ChargeDetailRecord>  ChargeDetailRecords,
                                    TransmissionTypes                     TransmissionType,

                                    DateTime?                             Timestamp,
                                    EventTracking_Id?                     EventTrackingId,
                                    TimeSpan?                             RequestTimeout,
                                    CancellationToken                     CancellationToken)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPORoaming.CPOClient.RequestTimeout;

            #endregion

            #region Filter charge detail records

            var ForwardedCDRs  = new List<WWCP.ChargeDetailRecord>();
            var FilteredCDRs   = new List<SendCDRResult>();

            foreach (var cdr in ChargeDetailRecords)
            {

                if (ChargeDetailRecordFilter(cdr) == ChargeDetailRecordFilters.forward)
                    ForwardedCDRs.Add(cdr);

                else
                    FilteredCDRs.Add(
                        SendCDRResult.Filtered(
                            org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                            Id,
                            cdr,
                            Warnings: Warnings.Create("This charge detail record was filtered!")
                        )
                    );

            }

            #endregion

            #region Send OnSendCDRsRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
                DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            #region if disabled => 'AdminDown'...

            DateTime        Endtime;
            TimeSpan        Runtime;
            SendCDRsResult  results;

            if (DisableSendChargeDetailRecords)
            {

                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;
                results  = SendCDRsResult.AdminDown(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                    Id,
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

                                OnEnqueueSendCDRsRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
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
                                DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
                            }

                            #endregion

                            foreach (var chargeDetailRecord in ChargeDetailRecords)
                            {

                                try
                                {

                                    chargeDetailRecordsQueue.Add(chargeDetailRecord.ToEMIP(
                                                                     RoamingNetwork.GetChargingSessionById(chargeDetailRecord.SessionId),
                                                                     WWCPChargeDetailRecord2eMIPChargeDetailRecord
                                                                 ));

                                    SendCDRsResults.Add(
                                        SendCDRResult.Enqueued(
                                            org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                            Id,
                                            chargeDetailRecord
                                        )
                                    );

                                }
                                catch (Exception e)
                                {
                                    SendCDRsResults.Add(
                                        SendCDRResult.CouldNotConvertCDRFormat(
                                            org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                            Id,
                                            chargeDetailRecord,
                                            Warnings: Warnings.Create(e.Message)
                                        )
                                    );
                                }

                            }

                            Endtime      = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            Runtime      = Endtime - StartTime;
                            results      = SendCDRsResult.Enqueued(
                                               org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                               Id,
                                               this,
                                               ChargeDetailRecords,
                                               I18NString.Create($"Enqueued for at least {FlushChargeDetailRecordsEvery.TotalSeconds} seconds!"),
                                               //SendCDRsResults.SafeWhere(cdrresult => cdrresult.Result != SendCDRResultTypes.Enqueued),
                                               Runtime: Runtime
                                           );
                            invokeTimer  = true;

                        }

                        #endregion

                        #region ...or send at once!

                        else
                        {

                            HTTPResponse<SetChargeDetailRecordResponse>  response;
                            SendCDRResult                                result;

                            foreach (var chargeDetailRecord in ChargeDetailRecords)
                            {

                                try
                                {

                                    response = await CPORoaming.SetChargeDetailRecord(
                                                         PartnerId,
                                                         chargeDetailRecord.EVSEId.Value.OperatorId.ToEMIP(CustomOperatorIdMapper),
                                                         chargeDetailRecord.ToEMIP(
                                                             RoamingNetwork.GetChargingSessionById(chargeDetailRecord.SessionId),
                                                             WWCPChargeDetailRecord2eMIPChargeDetailRecord
                                                         ),
                                                         Transaction_Id.Random(),

                                                         null,
                                                         Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         RequestTimeout
                                                     );

                                    if (response.HTTPStatusCode        == HTTPStatusCode.OK &&
                                        response.Content               is not null          &&
                                        response.Content.RequestStatus == RequestStatus.Ok)
                                    {

                                        result = SendCDRResult.Success(
                                                     org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                     Id,
                                                     chargeDetailRecord
                                                 );

                                    }

                                    else
                                        result = SendCDRResult.Error(
                                                     org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                     Id,
                                                     chargeDetailRecord,
                                                     Warnings: Warnings.Create(response.HTTPBodyAsUTF8String)
                                                 );

                                }
                                catch (Exception e)
                                {
                                    result = SendCDRResult.CouldNotConvertCDRFormat(
                                                 org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                 Id,
                                                 chargeDetailRecord,
                                                 I18NString.Create(e.Message)
                                             );
                                }

                                SendCDRsResults.Add(result);
                                //await RoamingNetwork.SessionsStore.CDRForwarded(chargeDetailRecord.SessionId, result);

                            }

                            await RoamingNetwork.ReceiveSendChargeDetailRecordResults(SendCDRsResults);

                            Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            Runtime  = Endtime - StartTime;

                            if (SendCDRsResults.All(cdrresult => cdrresult.Result == SendCDRResultTypes.Success))
                                results = SendCDRsResult.Success(
                                              org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                              Id,
                                              this,
                                              ChargeDetailRecords,
                                              Runtime: Runtime
                                          );

                            else
                                results = SendCDRsResult.Error(
                                              org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                              Id,
                                              this,
                                              SendCDRsResults.
                                                  Where (cdrresult => cdrresult.Result != SendCDRResultTypes.Success).
                                                  Select(cdrresult => cdrresult.ChargeDetailRecord),
                                              Runtime: Runtime
                                          );

                        }

                        #endregion

                    }

                    #region Could not get the lock for toooo long!

                    else
                    {

                        Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                        Runtime  = Endtime - StartTime;
                        results  = SendCDRsResult.Timeout(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                          Id,
                                                          this,
                                                          ChargeDetailRecords,
                                                          I18NString.Create("Could not " + (TransmissionType == TransmissionTypes.Enqueue ? "enqueue" : "send") + " charge detail records!"),
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
                DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsResponse));
            }

            #endregion

            return results;

        }

        #endregion


        // -----------------------------------------------------------------------------------------------------


        #region (timer) SendHeartbeat(State)

        private void SendHeartbeat(Object? State)
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

                    var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

                    SendHeartbeatStartedEvent?.Invoke(this,
                                                      StartTime,
                                                      FlushEVSEFastStatusEvery,
                                                      _SendHeartbeatsRunId);

                    #endregion

                    var SendHeartbeatResponse = await CPORoaming.SendHeartbeat(PartnerId,
                                                                               Operator_Id.Parse("DE*BDO"),
                                                                               Transaction_Id.Random(),

                                                                               null,
                                                                               org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                               new CancellationTokenSource().Token,
                                                                               EventTracking_Id.New,
                                                                               DefaultRequestTimeout).
                                                                 ConfigureAwait(false);

                    #region Send SendHeartbeatFinished Event...

                    var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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

                //OnWWCPCPOAdapterException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
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
            => evsesToAddQueue.              Count == 0 &&
               evsesToUpdateQueue.           Count == 0 &&
               evseStatusChangesDelayedQueue.Count == 0 &&
               evsesToRemoveQueue.           Count == 0;

        protected override async Task FlushEVSEDataAndStatusQueues()
        {

            #region Get a copy of all current EVSE data and delayed status

            var evsesToAddQueueCopy                     = new HashSet<IEVSE>();
            var evsesToUpdateQueueCopy                  = new HashSet<IEVSE>();
            var evseAdminStatusChangesDelayedQueueCopy  = new List<EVSEAdminStatusUpdate>();
            var evseStatusChangesDelayedQueueCopy       = new List<EVSEStatusUpdate>();
            var evsesToRemoveQueueCopy                  = new HashSet<IEVSE>();
            var evsesUpdateLogCopy                      = new Dictionary<IEVSE,            PropertyUpdateInfo[]>();
            var chargingStationsUpdateLogCopy           = new Dictionary<IChargingStation, PropertyUpdateInfo[]>();
            var chargingPoolsUpdateLogCopy              = new Dictionary<IChargingPool,    PropertyUpdateInfo[]>();

            await DataAndStatusLock.WaitAsync();

            try
            {

                // Copy 'EVSEs to add', remove originals...
                evsesToAddQueueCopy                      = new HashSet<IEVSE>             (evsesToAddQueue);
                evsesToAddQueue.Clear();

                // Copy 'EVSEs to update', remove originals...
                evsesToUpdateQueueCopy                   = new HashSet<IEVSE>             (evsesToUpdateQueue);
                evsesToUpdateQueue.Clear();

                // Copy 'EVSE status changes', remove originals...
                evseAdminStatusChangesDelayedQueueCopy   = new List<EVSEAdminStatusUpdate>(evseAdminStatusChangesDelayedQueue);
                evseAdminStatusChangesDelayedQueueCopy.AddRange(evsesToAddQueueCopy.SafeSelect(evse => new EVSEAdminStatusUpdate(evse.Id, evse.AdminStatus, evse.AdminStatus)));
                evseAdminStatusChangesDelayedQueue.Clear();

                // Copy 'EVSE status changes', remove originals...
                evseStatusChangesDelayedQueueCopy        = new List<EVSEStatusUpdate>     (evseStatusChangesDelayedQueue);
                evseStatusChangesDelayedQueueCopy.AddRange(evsesToAddQueueCopy.     SafeSelect(evse => new EVSEStatusUpdate     (evse.Id, evse.Status,      evse.Status)));
                evseStatusChangesDelayedQueue.Clear();

                // Copy 'EVSEs to remove', remove originals...
                evsesToRemoveQueueCopy                   = new HashSet<IEVSE>             (evsesToRemoveQueue);
                evsesToRemoveQueue.Clear();

                // Copy EVSE property updates
                evsesUpdateLog.           ForEach(_ => evsesUpdateLogCopy.           Add(_.Key, _.Value.ToArray()));
                evsesUpdateLog.Clear();

                // Copy charging station property updates
                chargingStationsUpdateLog.ForEach(_ => chargingStationsUpdateLogCopy.Add(_.Key, _.Value.ToArray()));
                chargingStationsUpdateLog.Clear();

                // Copy charging pool property updates
                chargingPoolsUpdateLog.   ForEach(_ => chargingPoolsUpdateLogCopy.   Add(_.Key, _.Value.ToArray()));
                chargingPoolsUpdateLog.Clear();


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

            if (evsesToAddQueueCopy.Count > 0)
            {

                //var EVSEsToAddTask = PushEVSEData(evsesToAddQueueCopy,
                //                                  _FlushEVSEDataRunId == 1
                //                                      ? ActionTypes.fullLoad
                //                                      : ActionTypes.update,
                //                                  EventTrackingId: EventTrackingId);

                //EVSEsToAddTask.Wait();

                //if (EVSEsToAddTask.Result.Warnings.Any())
                //{

                //    SendOnWarnings(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                //                   nameof(WWCPCPOAdapter) + Id,
                //                   "EVSEsToAddTask",
                //                   EVSEsToAddTask.Result.Warnings);

                //}

            }

            #endregion

            #region Send changed EVSE data

            if (evsesToUpdateQueueCopy.Count > 0)
            {

                // Surpress EVSE data updates for all newly added EVSEs
                var EVSEsWithoutNewEVSEs = evsesToUpdateQueueCopy.
                                               Where(evse => !evsesToAddQueueCopy.Contains(evse)).
                                               ToArray();


                if (EVSEsWithoutNewEVSEs.Length > 0)
                {

                    //var PushEVSEDataTask = PushEVSEData(EVSEsWithoutNewEVSEs,
                    //                                    ActionTypes.update,
                    //                                    EventTrackingId: EventTrackingId);

                    //PushEVSEDataTask.Wait();

                    //if (PushEVSEDataTask.Result.Warnings.Any())
                    //{

                    //    SendOnWarnings(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                    //                   nameof(WWCPCPOAdapter) + Id,
                    //                   "PushEVSEDataTask",
                    //                   PushEVSEDataTask.Result.Warnings);

                    //}

                }

            }

            #endregion

            #region Send changed EVSE admin status

            if (!DisableSendStatus &&
                evseAdminStatusChangesDelayedQueueCopy.Count > 0)
            {

                var pushEVSEAdminStatusResult = await SetEVSEAvailabilityStatus(evseAdminStatusChangesDelayedQueueCopy,

                                                                                org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                new CancellationTokenSource().Token,
                                                                                EventTrackingId,
                                                                                DefaultRequestTimeout).
                                                          ConfigureAwait(false);

                if (pushEVSEAdminStatusResult.Warnings.Any())
                {

                    SendOnWarnings(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "SetEVSEAvailabilityStatus",
                                   pushEVSEAdminStatusResult.Warnings);

                }

            }

            #endregion

            #region Send changed EVSE status

            if (!DisableSendStatus &&
                evseStatusChangesDelayedQueueCopy.Count > 0)
            {

                var pushEVSEStatusResult = await SetEVSEBusyStatus(evseStatusChangesDelayedQueueCopy,

                                                                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                   new CancellationTokenSource().Token,
                                                                   EventTrackingId,
                                                                   DefaultRequestTimeout).
                                                     ConfigureAwait(false);

                if (pushEVSEStatusResult.Warnings.Any())
                {

                    SendOnWarnings(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "SetEVSEBusyStatus",
                                   pushEVSEStatusResult.Warnings);

                }

            }

            #endregion

            #region Send removed charging stations

            if (evsesToRemoveQueueCopy.Count > 0)
            {

                var EVSEsToRemove = evsesToRemoveQueueCopy.ToArray();

                if (EVSEsToRemove.Length > 0)
                {

                    //var EVSEsToRemoveTask = PushEVSEData(EVSEsToRemove,
                    //                                     ActionTypes.delete,
                    //                                     EventTrackingId: EventTrackingId);

                    //EVSEsToRemoveTask.Wait();

                    //if (EVSEsToRemoveTask.Result.Warnings.Any())
                    //{

                    //    SendOnWarnings(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
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
            => evseAdminStatusChangesFastQueue.Count == 0 &&
               evseStatusChangesFastQueue.     Count == 0;

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
                    EVSEAdminStatusFastQueueCopy = new List<EVSEAdminStatusUpdate>(evseAdminStatusChangesFastQueue.Where(statuschange => !evsesToAddQueue.Any(evse => evse.Id == statuschange.Id)));
                    EVSEStatusFastQueueCopy = new List<EVSEStatusUpdate>(evseStatusChangesFastQueue.Where(statuschange => !evsesToAddQueue.Any(evse => evse.Id == statuschange.Id)));

                    // Add all evse status changes of EVSEs *NOT YET UPLOADED* into the delayed queue...
                    var EVSEAdminStatusChangesDelayed = evseAdminStatusChangesFastQueue.Where(statuschange => evsesToAddQueue.Any(evse => evse.Id == statuschange.Id)).ToArray();
                    var EVSEStatusChangesDelayed = evseStatusChangesFastQueue.Where(statuschange => evsesToAddQueue.Any(evse => evse.Id == statuschange.Id)).ToArray();

                    if (EVSEAdminStatusChangesDelayed.Length > 0)
                        evseAdminStatusChangesDelayedQueue.AddRange(EVSEAdminStatusChangesDelayed);

                    if (EVSEStatusChangesDelayed.Length > 0)
                        evseStatusChangesDelayedQueue.     AddRange(EVSEStatusChangesDelayed);

                    evseAdminStatusChangesFastQueue.Clear();
                    evseStatusChangesFastQueue.Clear();


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

                                                                                org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                new CancellationTokenSource().Token,
                                                                                EventTrackingId,
                                                                                DefaultRequestTimeout).
                                                          ConfigureAwait(false);

                if (pushEVSEAdminStatusResult.Warnings.Any())
                {

                    SendOnWarnings(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
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

                                                                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                   new CancellationTokenSource().Token,
                                                                   EventTrackingId,
                                                                   DefaultRequestTimeout).
                                                     ConfigureAwait(false);

                if (pushEVSEStatusResult.Warnings.Any())
                {

                    SendOnWarnings(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
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
            => chargeDetailRecordsQueue.Count == 0;

        protected override async Task FlushChargeDetailRecordsQueues(IEnumerable<ChargeDetailRecord> ChargeDetailRecords)
        {

            HTTPResponse<SetChargeDetailRecordResponse>  response;
            SendCDRResult                                result;

            foreach (var chargeDetailRecord in ChargeDetailRecords)
            {

                try
                {

                    response = await CPORoaming.SetChargeDetailRecord(PartnerId,
                                                                      chargeDetailRecord.EVSEId.OperatorId,
                                                                      chargeDetailRecord,
                                                                      Transaction_Id.Random(),

                                                                      null,
                                                                      Timestamp.Now,
                                                                      new CancellationTokenSource().Token,
                                                                      EventTracking_Id.New,
                                                                      DefaultRequestTimeout);

                    if (response.HTTPStatusCode        == HTTPStatusCode.OK &&
                        response.Content               is not null          &&
                        response.Content.RequestStatus == RequestStatus.Ok)
                    {

                        result = SendCDRResult.Success(
                                     Timestamp.Now,
                                     Id,
                                     chargeDetailRecord.GetInternalDataAs<WWCP.ChargeDetailRecord>(eMIPMapper.WWCP_CDR),
                                     Runtime: response.Runtime
                                 );

                    }

                    else
                        result = SendCDRResult.Error(
                                     Timestamp.Now,
                                     Id,
                                     chargeDetailRecord.GetInternalDataAs<WWCP.ChargeDetailRecord>(eMIPMapper.WWCP_CDR),
                                     Warnings: Warnings.Create(response.HTTPBodyAsUTF8String),
                                     Runtime:  response.Runtime
                                 );

                }
                catch (Exception e)
                {
                    result = SendCDRResult.CouldNotConvertCDRFormat(
                                 Timestamp.Now,
                                 Id,
                                 chargeDetailRecord.GetInternalDataAs<WWCP.ChargeDetailRecord>(eMIPMapper.WWCP_CDR),
                                 Warnings: Warnings.Create(e.Message),
                                 Runtime:  TimeSpan.Zero
                             );
                }

                await RoamingNetwork.ReceiveSendChargeDetailRecordResult(result);

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
        public override Int32 CompareTo(Object Object)
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

            => $"eMIP{Version.Number} CPO Adapter {Id}";

        #endregion


    }

}
