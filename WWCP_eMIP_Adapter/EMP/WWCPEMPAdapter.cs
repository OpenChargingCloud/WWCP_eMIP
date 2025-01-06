///*
// * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
// * This file is part of WWCP eMIP <https://github.com/OpenChargingCloud/WWCP_eMIP>
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using System;
//using System.Linq;
//using System.Threading;
//using System.Net.Security;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;

//using Org.BouncyCastle.Bcpg.OpenPgp;

//using org.GraphDefined.Vanaheimr.Illias;
//using org.GraphDefined.Vanaheimr.Hermod;
//using org.GraphDefined.Vanaheimr.Hermod.DNS;
//using org.GraphDefined.Vanaheimr.Hermod.HTTP;
//using org.GraphDefined.Vanaheimr.Aegir;

//#endregion

//namespace cloud.charging.open.protocols.eMIPv0_7_4.EMP
//{

//    /// <summary>
//    /// A WWCP wrapper for the eMIP EMP Roaming client which maps
//    /// WWCP data structures onto eMIP data structures and vice versa.
//    /// </summary>
//    public class WWCPEMPAdapter : AWWCPEMPAdapter,
//                                  IEMPRoamingProvider,
//                                  IEquatable <WWCPEMPAdapter>,
//                                  IComparable<WWCPEMPAdapter>,
//                                  IComparable
//    {

//        #region Data

//        //private        readonly  EVSE2EVSEDataRecordDelegate                            _EVSE2EVSEDataRecord;

//        //private        readonly  EVSEStatusUpdate2EVSEStatusRecordDelegate              _EVSEStatusUpdate2EVSEStatusRecord;

//        private        readonly  WWCPChargeDetailRecord2ChargeDetailRecordDelegate      _WWCPChargeDetailRecord2eMIPChargeDetailRecord;

//        //private        readonly  EVSEDataRecord2XMLDelegate                             _EVSEDataRecord2XML;

//        //private        readonly  EVSEStatusRecord2XMLDelegate                           _EVSEStatusRecord2XML;

//        //private        readonly  ChargeDetailRecord2XMLDelegate                         _ChargeDetailRecord2XML;

//        private static readonly  Regex                                                  pattern                      = new Regex(@"\s=\s");

//        private readonly        List<ChargeDetailRecord>                                eMIP_ChargeDetailRecords_Queue;

//        protected readonly      SemaphoreSlim                                           FlusheMIPChargeDetailRecordsLock      = new SemaphoreSlim(1, 1);

//        public readonly static  TimeSpan                                                DefaultRequestTimeout       = TimeSpan.FromSeconds(30);


//        /// <summary>
//        /// The default send heartbeats intervall.
//        /// </summary>
//        public readonly static  TimeSpan                                                DefaultSendHeartbeatsEvery  = TimeSpan.FromMinutes(5);

//        private readonly        SemaphoreSlim                                           SendHeartbeatLock           = new SemaphoreSlim(1, 1);
//        private readonly        Timer                                                   SendHeartbeatsTimer;

//        private                 UInt64                                                  _SendHeartbeatsRunId         = 1;

//        #endregion

//        #region Properties

//        IId ISendAuthorizeStartStop.AuthId
//            => Id;

//        IId ISendChargeDetailRecords.Id
//            => Id;

//        IEnumerable<IId> ISendChargeDetailRecords.Ids
//            => Ids.Cast<IId>();

//        /// <summary>
//        /// The wrapped EMP roaming object.
//        /// </summary>
//        public EMPRoaming EMPRoaming { get; }


//        /// <summary>
//        /// The EMP client.
//        /// </summary>
//        public EMPClient EMPClient
//            => EMPRoaming?.EMPClient;

//        /// <summary>
//        /// The EMP client logger.
//        /// </summary>
//        public EMPClient.EMPClientLogger ClientLogger
//            => EMPRoaming?.EMPClient?.Logger;


//        /// <summary>
//        /// The EMP server.
//        /// </summary>
//        public EMPServer EMPServer
//            => EMPRoaming?.EMPServer;

//        /// <summary>
//        /// The EMP server logger.
//        /// </summary>
//        public EMPServerLogger ServerLogger
//            => EMPRoaming?.EMPServerLogger;



//        /// <summary>
//        /// The partner identification.
//        /// </summary>
//        public Partner_Id  PartnerId { get; }


//        /// <summary>
//        /// This service can be disabled, e.g. for debugging reasons.
//        /// </summary>
//        public Boolean     DisableSendHeartbeats             { get; set; }

//        public TimeSpan    SendHeartbeatsEvery               { get; }

//        EMPRoamingProvider_Id IEMPRoamingProvider.Id => throw new NotImplementedException();

//        I18NString IEMPRoamingProvider.Name => throw new NotImplementedException();

//        RoamingNetwork IEMPRoamingProvider.RoamingNetwork => throw new NotImplementedException();

//        bool IPullData.DisablePullData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//        protected readonly CustomEVSEIdMapperDelegate CustomEVSEIdMapper;

//        #endregion

//        #region Events

//        // Client logging...

//        #region OnPushEVSEDataWWCPRequest/-Response

//        ///// <summary>
//        ///// An event fired whenever new EVSE data will be send upstream.
//        ///// </summary>
//        //public event OnPushEVSEDataWWCPRequestDelegate   OnPushEVSEDataWWCPRequest;

//        ///// <summary>
//        ///// An event fired whenever new EVSE data had been sent upstream.
//        ///// </summary>
//        //public event OnPushEVSEDataWWCPResponseDelegate  OnPushEVSEDataWWCPResponse;

//        #endregion

//        #region OnPushEVSEStatusWWCPRequest/-Response

//        ///// <summary>
//        ///// An event fired whenever new EVSE status will be send upstream.
//        ///// </summary>
//        //public event OnPushEVSEStatusWWCPRequestDelegate   OnPushEVSEStatusWWCPRequest;

//        ///// <summary>
//        ///// An event fired whenever new EVSE status had been sent upstream.
//        ///// </summary>
//        //public event OnPushEVSEStatusWWCPResponseDelegate  OnPushEVSEStatusWWCPResponse;

//        #endregion


//        #region OnAuthorizeStartRequest/-Response

//        /// <summary>
//        /// An event fired whenever an authentication token will be verified for charging.
//        /// </summary>
//        public event OnAuthorizeStartRequestDelegate                  OnAuthorizeStartRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified for charging.
//        /// </summary>
//        public event OnAuthorizeStartResponseDelegate                 OnAuthorizeStartResponse;


//        /// <summary>
//        /// An event fired whenever an authentication token will be verified for charging at the given EVSE.
//        /// </summary>
//        public event OnAuthorizeEVSEStartRequestDelegate              OnAuthorizeEVSEStartRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified for charging at the given EVSE.
//        /// </summary>
//        public event OnAuthorizeEVSEStartResponseDelegate             OnAuthorizeEVSEStartResponse;


//        /// <summary>
//        /// An event fired whenever an authentication token will be verified for charging at the given charging station.
//        /// </summary>
//        public event OnAuthorizeChargingStationStartRequestDelegate   OnAuthorizeChargingStationStartRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified for charging at the given charging station.
//        /// </summary>
//        public event OnAuthorizeChargingStationStartResponseDelegate  OnAuthorizeChargingStationStartResponse;


//        /// <summary>
//        /// An event fired whenever an authentication token will be verified for charging at the given charging pool.
//        /// </summary>
//        public event OnAuthorizeChargingPoolStartRequestDelegate      OnAuthorizeChargingPoolStartRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified for charging at the given charging pool.
//        /// </summary>
//        public event OnAuthorizeChargingPoolStartResponseDelegate     OnAuthorizeChargingPoolStartResponse;

//        #endregion

//        #region OnAuthorizeStopRequest/-Response

//        /// <summary>
//        /// An event fired whenever an authentication token will be verified to stop a charging process.
//        /// </summary>
//        public event OnAuthorizeStopRequestDelegate                  OnAuthorizeStopRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified to stop a charging process.
//        /// </summary>
//        public event OnAuthorizeStopResponseDelegate                 OnAuthorizeStopResponse;


//        /// <summary>
//        /// An event fired whenever an authentication token will be verified to stop a charging process at the given EVSE.
//        /// </summary>
//        public event OnAuthorizeEVSEStopRequestDelegate              OnAuthorizeEVSEStopRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified to stop a charging process at the given EVSE.
//        /// </summary>
//        public event OnAuthorizeEVSEStopResponseDelegate             OnAuthorizeEVSEStopResponse;


//        /// <summary>
//        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging station.
//        /// </summary>
//        public event OnAuthorizeChargingStationStopRequestDelegate   OnAuthorizeChargingStationStopRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging station.
//        /// </summary>
//        public event OnAuthorizeChargingStationStopResponseDelegate  OnAuthorizeChargingStationStopResponse;


//        /// <summary>
//        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging pool.
//        /// </summary>
//        public event OnAuthorizeChargingPoolStopRequestDelegate      OnAuthorizeChargingPoolStopRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging pool.
//        /// </summary>
//        public event OnAuthorizeChargingPoolStopResponseDelegate     OnAuthorizeChargingPoolStopResponse;

//        #endregion

//        #region OnSendCDRRequest/-Response

//        /// <summary>
//        /// An event fired whenever a charge detail record was enqueued for later sending upstream.
//        /// </summary>
//        public event OnSendCDRRequestDelegate   OnEnqueueSendCDRsRequest;

//        /// <summary>
//        /// An event fired whenever a charge detail record will be send upstream.
//        /// </summary>
//        public event OnSendCDRRequestDelegate   OnSendCDRsRequest;

//        /// <summary>
//        /// An event fired whenever a charge detail record had been sent upstream.
//        /// </summary>
//        public event OnSendCDRResponseDelegate  OnSendCDRsResponse;

//        #endregion


//        #region SendHeartbeat

//        public delegate void SendHeartbeatStartedDelegate(AWWCPCSOAdapter Sender, DateTime StartTime, TimeSpan Every, UInt64 RunId);

//        public event SendHeartbeatStartedDelegate SendHeartbeatStartedEvent;


//        public delegate void SendHeartbeatFinishedDelegate(AWWCPCSOAdapter Sender, DateTime StartTime, DateTime EndTime, TimeSpan Runtime, TimeSpan Every, UInt64 RunId);

//        public event SendHeartbeatFinishedDelegate SendHeartbeatFinishedEvent;

//        #endregion

//        #endregion

//        #region Constructor(s)

//        #region WWCPEMPAdapter(Id, Name, RoamingNetwork, EMPRoaming, EVSE2EVSEDataRecord = null)

//        /// <summary>
//        /// Create a new WWCP wrapper for the eMIP roaming client for Charging Station Operators/EMPs.
//        /// </summary>
//        /// <param name="Id">The unique identification of the roaming provider.</param>
//        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
//        /// <param name="Description">An optional (multi-language) description of the charging station operator roaming provider.</param>
//        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
//        /// 
//        /// <param name="PartnerId">The unique identification of an eMIP communication partner.</param>
//        /// <param name="EMPRoaming">A eMIP EMP roaming object to be mapped to WWCP.</param>
//        /// 
//        /// <param name="IncludeEVSEIds">Only include the EVSE matching the given delegate.</param>
//        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
//        /// <param name="CustomEVSEIdMapper">A delegate to customize the mapping of EVSE identifications.</param>
//        /// 
//        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
//        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
//        /// <param name="WWCPChargeDetailRecord2eMIPChargeDetailRecord">A delegate to process a charge detail record, e.g. before pushing it to the roaming provider.</param>
//        /// 
//        /// <param name="SendHeartbeatsEvery">The heartbeat intervall.</param>
//        /// <param name="ServiceCheckEvery">The service check intervall.</param>
//        /// <param name="StatusCheckEvery">The status check intervall.</param>
//        /// <param name="CDRCheckEvery">The charge detail record intervall.</param>
//        /// 
//        /// <param name="DisableSendHeartbeats">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
//        /// 
//        /// <param name="PublicKeyRing">The public key ring of the entity.</param>
//        /// <param name="SecretKeyRing">The secrect key ring of the entity.</param>
//        /// <param name="DNSClient">The attached DNS service.</param>
//        public WWCPEMPAdapter(CSORoamingProvider_Id                              Id,
//                              I18NString                                         Name,
//                              I18NString                                         Description,
//                              RoamingNetwork                                     RoamingNetwork,

//                              Partner_Id                                         PartnerId,
//                              EMPRoaming                                         EMPRoaming,

//                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
//                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,
//                              CustomEVSEIdMapperDelegate                         CustomEVSEIdMapper                              = null,

//                              //EVSE2EVSEDataRecordDelegate                        EVSE2EVSEDataRecord                             = null,
//                              //EVSEStatusUpdate2EVSEStatusRecordDelegate          EVSEStatusUpdate2EVSEStatusRecord               = null,
//                              WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2eMIPChargeDetailRecord   = null,

//                              TimeSpan?                                          SendHeartbeatsEvery                             = null,
//                              TimeSpan?                                          ServiceCheckEvery                               = null,
//                              TimeSpan?                                          StatusCheckEvery                                = null,
//                              TimeSpan?                                          CDRCheckEvery                                   = null,

//                              Boolean                                            DisableSendHeartbeats                           = false,
//                              Boolean                                            DisablePushData                                 = false,
//                              Boolean                                            DisablePushStatus                               = false,
//                              Boolean                                            DisableAuthentication                           = false,
//                              Boolean                                            DisableSendChargeDetailRecords                  = false,

//                              PgpPublicKeyRing                                   PublicKeyRing                                   = null,
//                              PgpSecretKeyRing                                   SecretKeyRing                                   = null,
//                              DNSClient                                          DNSClient                                       = null)

//            : base(Id,
//                   Name,
//                   Description,
//                   RoamingNetwork,

//                   IncludeEVSEIds,
//                   IncludeEVSEs,
//                   //CustomEVSEIdMapper,

//                   ServiceCheckEvery,
//                   StatusCheckEvery,
//                   CDRCheckEvery,

//                   DisablePushData,
//                   DisablePushStatus,
//                   DisableAuthentication,
//                   DisableSendChargeDetailRecords,

//                   PublicKeyRing,
//                   SecretKeyRing,
//                   DNSClient ?? EMPRoaming?.DNSClient)

//        {

//            #region Initial checks

//            if (Name.IsNullOrEmpty())
//                throw new ArgumentNullException(nameof(Name),        "The given roaming provider name must not be null or empty!");

//            if (EMPRoaming == null)
//                throw new ArgumentNullException(nameof(EMPRoaming),  "The given eMIP EMP Roaming object must not be null!");

//            #endregion

//            this.PartnerId                                        = PartnerId;
//            this.EMPRoaming                                       = EMPRoaming;

//            //this._EVSE2EVSEDataRecord                             = EVSE2EVSEDataRecord;
//            //this._EVSEStatusUpdate2EVSEStatusRecord               = EVSEStatusUpdate2EVSEStatusRecord;
//            this._WWCPChargeDetailRecord2eMIPChargeDetailRecord   = WWCPChargeDetailRecord2eMIPChargeDetailRecord;

//            this.eMIP_ChargeDetailRecords_Queue                   = new List<ChargeDetailRecord>();

//            this.SendHeartbeatsEvery                              = SendHeartbeatsEvery ?? DefaultSendHeartbeatsEvery;
//            this.SendHeartbeatsTimer                              = new Timer(SendHeartbeat, null, this.SendHeartbeatsEvery, this.SendHeartbeatsEvery);
//            this.DisableSendHeartbeats                            = DisableSendHeartbeats;

//            this.CustomEVSEIdMapper                               = CustomEVSEIdMapper;

//            // Link incoming eMIP events...

//        }

//        #endregion

//        #region WWCPEMPAdapter(Id, Name, RoamingNetwork, EMPClient, EMPServer, EVSEDataRecordProcessing = null)

//        /// <summary>
//        /// Create a new WWCP wrapper for the eMIP roaming client for Charging Station Operators/EMPs.
//        /// </summary>
//        /// <param name="Id">The unique identification of the roaming provider.</param>
//        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
//        /// <param name="Description">An optional (multi-language) description of the charging station operator roaming provider.</param>
//        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
//        /// 
//        /// <param name="PartnerId">The unique identification of an eMIP communication partner.</param>
//        /// <param name="EMPClient">An eMIP EMP client.</param>
//        /// <param name="EMPServer">An eMIP EMP sever.</param>
//        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
//        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
//        /// 
//        /// <param name="IncludeEVSEIds">Only include the EVSE matching the given delegate.</param>
//        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
//        /// <param name="CustomEVSEIdMapper">A delegate to customize the mapping of EVSE identifications.</param>
//        /// 
//        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
//        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
//        /// <param name="WWCPChargeDetailRecord2eMIPChargeDetailRecord">A delegate to process a charge detail record, e.g. before pushing it to the roaming provider.</param>
//        /// 
//        /// <param name="SendHeartbeatsEvery">The heartbeat intervall.</param>
//        /// <param name="ServiceCheckEvery">The service check intervall.</param>
//        /// <param name="StatusCheckEvery">The status check intervall.</param>
//        /// <param name="CDRCheckEvery">The charge detail record intervall.</param>
//        /// 
//        /// <param name="DisableSendHeartbeats">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
//        /// 
//        /// <param name="PublicKeyRing">The public key ring of the entity.</param>
//        /// <param name="SecretKeyRing">The secrect key ring of the entity.</param>
//        /// <param name="DNSClient">An optional DNS client to use.</param>
//        public WWCPEMPAdapter(CSORoamingProvider_Id                              Id,
//                              I18NString                                         Name,
//                              I18NString                                         Description,
//                              RoamingNetwork                                     RoamingNetwork,

//                              Partner_Id                                         PartnerId,
//                              EMPClient                                          EMPClient,
//                              EMPServer                                          EMPServer,
//                              String                                             ServerLoggingContext                            = EMPServerLogger.DefaultContext,
//                              LogfileCreatorDelegate                             LogfileCreator                                  = null,

//                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
//                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,
//                              CustomEVSEIdMapperDelegate                         CustomEVSEIdMapper                              = null,

//                              //EVSE2EVSEDataRecordDelegate                        EVSE2EVSEDataRecord                             = null,
//                              //EVSEStatusUpdate2EVSEStatusRecordDelegate          EVSEStatusUpdate2EVSEStatusRecord               = null,
//                              WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2eMIPChargeDetailRecord   = null,

//                              TimeSpan?                                          SendHeartbeatsEvery                             = null,
//                              TimeSpan?                                          ServiceCheckEvery                               = null,
//                              TimeSpan?                                          StatusCheckEvery                                = null,
//                              TimeSpan?                                          CDRCheckEvery                                   = null,

//                              Boolean                                            DisableSendHeartbeats                           = false,
//                              Boolean                                            DisablePushData                                 = false,
//                              Boolean                                            DisablePushStatus                               = false,
//                              Boolean                                            DisableAuthentication                           = false,
//                              Boolean                                            DisableSendChargeDetailRecords                  = false,

//                              PgpPublicKeyRing                                   PublicKeyRing                                   = null,
//                              PgpSecretKeyRing                                   SecretKeyRing                                   = null,
//                              DNSClient                                          DNSClient                                       = null)

//            : this(Id,
//                   Name,
//                   Description,
//                   RoamingNetwork,

//                   PartnerId,
//                   new EMPRoaming(EMPClient,
//                                  EMPServer,
//                                  ServerLoggingContext,
//                                  LogfileCreator),

//                   IncludeEVSEIds,
//                   IncludeEVSEs,
//                   CustomEVSEIdMapper,

//                   //EVSE2EVSEDataRecord,
//                   //EVSEStatusUpdate2EVSEStatusRecord,
//                   WWCPChargeDetailRecord2eMIPChargeDetailRecord,

//                   SendHeartbeatsEvery,
//                   ServiceCheckEvery,
//                   StatusCheckEvery,
//                   CDRCheckEvery,

//                   DisableSendHeartbeats,
//                   DisablePushData,
//                   DisablePushStatus,
//                   DisableAuthentication,
//                   DisableSendChargeDetailRecords,

//                   PublicKeyRing,
//                   SecretKeyRing,
//                   DNSClient ?? EMPServer?.DNSClient)

//        { }

//        #endregion

//        #region WWCPEMPAdapter(Id, Name, RoamingNetwork, RemoteHostName, ...)

//        /// <summary>
//        /// Create a new WWCP wrapper for the eMIP roaming client for Charging Station Operators/EMPs.
//        /// </summary>
//        /// <param name="Id">The unique identification of the roaming provider.</param>
//        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
//        /// <param name="Description">An optional (multi-language) description of the charging station operator roaming provider.</param>
//        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
//        /// 
//        /// <param name="PartnerId">The unique identification of an eMIP communication partner.</param>
//        /// 
//        /// <param name="RemoteHostname">The hostname of the remote eMIP service.</param>
//        /// <param name="RemoteTCPPort">An optional TCP port of the remote eMIP service.</param>
//        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
//        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
//        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote eMIP service.</param>
//        /// <param name="URLPrefix">An default URI prefix.</param>
//        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
//        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
//        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
//        /// 
//        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
//        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
//        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
//        /// <param name="ServerURLPrefix">An optional prefix for the HTTP URIs.</param>
//        /// <param name="ServerAuthorisationURL">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
//        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
//        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
//        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
//        /// 
//        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
//        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
//        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
//        /// 
//        /// <param name="IncludeEVSEIds">Only include the EVSE matching the given delegate.</param>
//        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
//        /// <param name="CustomEVSEIdMapper">A delegate to customize the mapping of EVSE identifications.</param>
//        /// 
//        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
//        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
//        /// <param name="WWCPChargeDetailRecord2eMIPChargeDetailRecord">A delegate to process a charge detail record, e.g. before pushing it to the roaming provider.</param>
//        /// 
//        /// <param name="SendHeartbeatsEvery">The heartbeat intervall.</param>
//        /// <param name="ServiceCheckEvery">The service check intervall.</param>
//        /// <param name="StatusCheckEvery">The status check intervall.</param>
//        /// <param name="CDRCheckEvery">The charge detail record intervall.</param>
//        /// 
//        /// <param name="DisableSendHeartbeats">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
//        /// 
//        /// <param name="PublicKeyRing">The public key ring of the entity.</param>
//        /// <param name="SecretKeyRing">The secrect key ring of the entity.</param>
//        /// <param name="DNSClient">An optional DNS client to use.</param>
//        public WWCPEMPAdapter(CSORoamingProvider_Id                              Id,
//                              I18NString                                         Name,
//                              I18NString                                         Description,
//                              RoamingNetwork                                     RoamingNetwork,

//                              Partner_Id                                         PartnerId,

//                              HTTPHostname                                       RemoteHostname,
//                              IPPort?                                            RemoteTCPPort                                   = null,
//                              RemoteCertificateValidationCallback                RemoteCertificateValidator                      = null,
//                              LocalCertificateSelectionCallback                  ClientCertificateSelector                       = null,
//                              HTTPHostname?                                      RemoteHTTPVirtualHost                           = null,
//                              HTTPPath?                                          URLPrefix                                       = null,
//                              String                                             HTTPUserAgent                                   = EMPClient.DefaultHTTPUserAgent,
//                              TimeSpan?                                          RequestTimeout                                  = null,
//                              Byte?                                              MaxNumberOfRetries                              = EMPClient.DefaultMaxNumberOfRetries,

//                              String                                             ServerName                                      = EMPServer.DefaultHTTPServerName,
//                              String                                             ServiceId                                       = null,
//                              IPPort?                                            ServerTCPPort                                   = null,
//                              HTTPPath?                                          ServerURLPrefix                                 = null,
//                              String                                             ServerAuthorisationURL                          = EMPServer.DefaultAuthorisationURL,
//                              HTTPContentType                                    ServerContentType                               = null,
//                              Boolean                                            ServerRegisterHTTPRootService                   = true,
//                              Boolean                                            ServerAutoStart                                 = false,

//                              String                                             ClientLoggingContext                            = EMPClient.EMPClientLogger.DefaultContext,
//                              String                                             ServerLoggingContext                            = EMPServerLogger.DefaultContext,
//                              LogfileCreatorDelegate                             LogfileCreator                                  = null,

//                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
//                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,
//                              CustomEVSEIdMapperDelegate                         CustomEVSEIdMapper                              = null,

//                              //EVSE2EVSEDataRecordDelegate                        EVSE2EVSEDataRecord                             = null,
//                              //EVSEStatusUpdate2EVSEStatusRecordDelegate          EVSEStatusUpdate2EVSEStatusRecord               = null,
//                              WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2eMIPChargeDetailRecord   = null,

//                              TimeSpan?                                          SendHeartbeatsEvery                             = null,
//                              TimeSpan?                                          ServiceCheckEvery                               = null,
//                              TimeSpan?                                          StatusCheckEvery                                = null,
//                              TimeSpan?                                          CDRCheckEvery                                   = null,

//                              Boolean                                            DisableSendHeartbeats                           = false,
//                              Boolean                                            DisablePushData                                 = false,
//                              Boolean                                            DisablePushStatus                               = false,
//                              Boolean                                            DisableAuthentication                           = false,
//                              Boolean                                            DisableSendChargeDetailRecords                  = false,

//                              PgpPublicKeyRing                                   PublicKeyRing                                   = null,
//                              PgpSecretKeyRing                                   SecretKeyRing                                   = null,
//                              DNSClient                                          DNSClient                                       = null)

//            : this(Id,
//                   Name,
//                   Description,
//                   RoamingNetwork,

//                   PartnerId,
//                   new EMPRoaming(Id.ToString(),
//                                  RemoteHostname,
//                                  RemoteTCPPort,
//                                  RemoteCertificateValidator,
//                                  ClientCertificateSelector,
//                                  RemoteHTTPVirtualHost,
//                                  URLPrefix ?? EMPClient.DefaultURLPrefix,
//                                  HTTPUserAgent,
//                                  RequestTimeout,
//                                  MaxNumberOfRetries,

//                                  ServerName,
//                                  ServiceId,
//                                  ServerTCPPort,
//                                  ServerURLPrefix        ?? EMPServer.DefaultURLPrefix,
//                                  ServerAuthorisationURL ?? EMPServer.DefaultAuthorisationURL,
//                                  ServerContentType,
//                                  ServerRegisterHTTPRootService,
//                                  false,

//                                  ClientLoggingContext,
//                                  ServerLoggingContext,
//                                  LogfileCreator,

//                                  DNSClient),

//                   IncludeEVSEIds,
//                   IncludeEVSEs,
//                   CustomEVSEIdMapper,

//                   //EVSE2EVSEDataRecord,
//                   //EVSEStatusUpdate2EVSEStatusRecord,
//                   WWCPChargeDetailRecord2eMIPChargeDetailRecord,

//                   SendHeartbeatsEvery,
//                   ServiceCheckEvery,
//                   StatusCheckEvery,
//                   CDRCheckEvery,

//                   DisableSendHeartbeats,
//                   DisablePushData,
//                   DisablePushStatus,
//                   DisableAuthentication,
//                   DisableSendChargeDetailRecords,

//                   PublicKeyRing,
//                   SecretKeyRing,
//                   DNSClient)

//        {

//            if (ServerAutoStart)
//                EMPServer.Start();

//        }

//        #endregion

//        #endregion


//        #region IEMPRoamingProvider Members

//        event OnReserveEVSERequestDelegate IEMPRoamingProvider.OnReserveEVSERequest
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnReserveEVSEResponseDelegate IEMPRoamingProvider.OnReserveEVSEResponse
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnCancelReservationRequestDelegate IEMPRoamingProvider.OnCancelReservationRequest
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnCancelReservationResponseDelegate IEMPRoamingProvider.OnCancelReservationResponse
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnRemoteStartEVSERequestDelegate IEMPRoamingProvider.OnRemoteStartEVSERequest
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnRemoteStartEVSEResponseDelegate IEMPRoamingProvider.OnRemoteStartEVSEResponse
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnRemoteStopEVSERequestDelegate IEMPRoamingProvider.OnRemoteStopEVSERequest
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnRemoteStopEVSEResponseDelegate IEMPRoamingProvider.OnRemoteStopEVSEResponse
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnGetCDRsRequestDelegate IEMPRoamingProvider.OnGetChargeDetailRecordsRequest
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnGetCDRsResponseDelegate IEMPRoamingProvider.OnGetChargeDetailRecordsResponse
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnAuthorizeEVSEStartRequestDelegate IEMPRoamingProvider.OnAuthorizeEVSEStartRequest
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnAuthorizeEVSEStartResponseDelegate IEMPRoamingProvider.OnAuthorizeEVSEStartResponse
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnAuthorizeEVSEStopRequestDelegate IEMPRoamingProvider.OnAuthorizeEVSEStopRequest
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnAuthorizeEVSEStopResponseDelegate IEMPRoamingProvider.OnAuthorizeEVSEStopResponse
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnSendCDRRequestDelegate IEMPRoamingProvider.OnChargeDetailRecordRequest
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnSendCDRResponseDelegate IEMPRoamingProvider.OnChargeDetailRecordResponse
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        #endregion


//        // RN -> External service requests...

//        // -----------------------------------------------------------------------------------------------------

//        #region (timer) SendHeartbeat(State)

//        private void SendHeartbeat(Object State)
//        {
//            if (!DisableSendHeartbeats)
//                SendHeartbeat2().Wait();
//        }

//        private async Task SendHeartbeat2()
//        {

//            var LockTaken = await SendHeartbeatLock.WaitAsync(0).ConfigureAwait(false);

//            try
//            {

//                if (LockTaken)
//                {

//                    #region Send SendHeartbeatStarted Event...

//                    var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

//                    SendHeartbeatStartedEvent?.Invoke(this,
//                                                      StartTime,
//                                                      FlushEVSEFastStatusEvery,
//                                                      _SendHeartbeatsRunId);

//                    #endregion

//                    var SendHeartbeatResponse = await EMPRoaming.SendHeartbeat(PartnerId,
//                                                                               Operator_Id.Parse("DE*BDO"),
//                                                                               Transaction_Id.Random(),

//                                                                               org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
//                                                                               new CancellationTokenSource().Token,
//                                                                               EventTracking_Id.New,
//                                                                               DefaultRequestTimeout).
//                                                                 ConfigureAwait(false);

//                    #region Send SendHeartbeatFinished Event...

//                    var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

//                    SendHeartbeatFinishedEvent?.Invoke(this,
//                                                       StartTime,
//                                                       EndTime,
//                                                       EndTime - StartTime,
//                                                       SendHeartbeatsEvery,
//                                                       _SendHeartbeatsRunId);

//                    #endregion

//                    _SendHeartbeatsRunId++;

//                }

//            }
//            catch (Exception e)
//            {

//                while (e.InnerException != null)
//                    e = e.InnerException;

//                DebugX.LogT(GetType().Name + ".SendHeartbeat '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

//                //OnWWCPEMPAdapterException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
//                //                                  this,
//                //                                  e);

//            }

//            finally
//            {

//                if (LockTaken)
//                {
//                    SendHeartbeatLock.Release();
//                }

//                else
//                    DebugX.LogT("SendHeartbeatLock exited!");

//            }

//        }

//        #endregion

//        // -----------------------------------------------------------------------------------------------------


//        #region Operator overloading

//        #region Operator == (WWCPEMPAdapter1, WWCPEMPAdapter2)

//        /// <summary>
//        /// Compares two WWCPEMPAdapters for equality.
//        /// </summary>
//        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
//        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
//        /// <returns>True if both match; False otherwise.</returns>
//        public static Boolean operator == (WWCPEMPAdapter WWCPEMPAdapter1, WWCPEMPAdapter WWCPEMPAdapter2)
//        {

//            // If both are null, or both are same instance, return true.
//            if (Object.ReferenceEquals(WWCPEMPAdapter1, WWCPEMPAdapter2))
//                return true;

//            // If one is null, but not both, return false.
//            if (((Object) WWCPEMPAdapter1 == null) || ((Object) WWCPEMPAdapter2 == null))
//                return false;

//            return WWCPEMPAdapter1.Equals(WWCPEMPAdapter2);

//        }

//        #endregion

//        #region Operator != (WWCPEMPAdapter1, WWCPEMPAdapter2)

//        /// <summary>
//        /// Compares two WWCPEMPAdapters for inequality.
//        /// </summary>
//        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
//        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
//        /// <returns>False if both match; True otherwise.</returns>
//        public static Boolean operator != (WWCPEMPAdapter WWCPEMPAdapter1, WWCPEMPAdapter WWCPEMPAdapter2)

//            => !(WWCPEMPAdapter1 == WWCPEMPAdapter2);

//        #endregion

//        #region Operator <  (WWCPEMPAdapter1, WWCPEMPAdapter2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
//        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator < (WWCPEMPAdapter  WWCPEMPAdapter1,
//                                          WWCPEMPAdapter  WWCPEMPAdapter2)
//        {

//            if ((Object) WWCPEMPAdapter1 == null)
//                throw new ArgumentNullException(nameof(WWCPEMPAdapter1),  "The given WWCPEMPAdapter must not be null!");

//            return WWCPEMPAdapter1.CompareTo(WWCPEMPAdapter2) < 0;

//        }

//        #endregion

//        #region Operator <= (WWCPEMPAdapter1, WWCPEMPAdapter2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
//        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator <= (WWCPEMPAdapter WWCPEMPAdapter1,
//                                           WWCPEMPAdapter WWCPEMPAdapter2)

//            => !(WWCPEMPAdapter1 > WWCPEMPAdapter2);

//        #endregion

//        #region Operator >  (WWCPEMPAdapter1, WWCPEMPAdapter2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
//        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator > (WWCPEMPAdapter WWCPEMPAdapter1,
//                                          WWCPEMPAdapter WWCPEMPAdapter2)
//        {

//            if ((Object) WWCPEMPAdapter1 == null)
//                throw new ArgumentNullException(nameof(WWCPEMPAdapter1),  "The given WWCPEMPAdapter must not be null!");

//            return WWCPEMPAdapter1.CompareTo(WWCPEMPAdapter2) > 0;

//        }

//        #endregion

//        #region Operator >= (WWCPEMPAdapter1, WWCPEMPAdapter2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
//        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator >= (WWCPEMPAdapter WWCPEMPAdapter1,
//                                           WWCPEMPAdapter WWCPEMPAdapter2)

//            => !(WWCPEMPAdapter1 < WWCPEMPAdapter2);

//        #endregion

//        #endregion

//        #region IComparable<WWCPEMPAdapter> Members

//        #region CompareTo(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        public Int32 CompareTo(Object Object)
//        {

//            if (Object == null)
//                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

//            var WWCPEMPAdapter = Object as WWCPEMPAdapter;
//            if ((Object) WWCPEMPAdapter == null)
//                throw new ArgumentException("The given object is not an WWCPEMPAdapter!", nameof(Object));

//            return CompareTo(WWCPEMPAdapter);

//        }

//        #endregion

//        #region CompareTo(WWCPEMPAdapter)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="WWCPEMPAdapter">An WWCPEMPAdapter object to compare with.</param>
//        public Int32 CompareTo(WWCPEMPAdapter WWCPEMPAdapter)
//        {

//            if ((Object) WWCPEMPAdapter == null)
//                throw new ArgumentNullException(nameof(WWCPEMPAdapter), "The given WWCPEMPAdapter must not be null!");

//            return Id.CompareTo(WWCPEMPAdapter.Id);

//        }

//        #endregion

//        #endregion

//        #region IEquatable<WWCPEMPAdapter> Members

//        #region Equals(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        /// <returns>true|false</returns>
//        public override Boolean Equals(Object Object)
//        {

//            if (Object == null)
//                return false;

//            var WWCPEMPAdapter = Object as WWCPEMPAdapter;
//            if ((Object) WWCPEMPAdapter == null)
//                return false;

//            return Equals(WWCPEMPAdapter);

//        }

//        #endregion

//        #region Equals(WWCPEMPAdapter)

//        /// <summary>
//        /// Compares two WWCPEMPAdapter for equality.
//        /// </summary>
//        /// <param name="WWCPEMPAdapter">An WWCPEMPAdapter to compare with.</param>
//        /// <returns>True if both match; False otherwise.</returns>
//        public Boolean Equals(WWCPEMPAdapter WWCPEMPAdapter)
//        {

//            if ((Object) WWCPEMPAdapter == null)
//                return false;

//            return Id.Equals(WWCPEMPAdapter.Id);

//        }

//        #endregion

//        #endregion

//        #region GetHashCode()

//        /// <summary>
//        /// Get the hashcode of this object.
//        /// </summary>
//        public override Int32 GetHashCode()

//            => Id.GetHashCode();

//        #endregion

//        #region (override) ToString()

//        /// <summary>
//        /// Return a text representation of this object.
//        /// </summary>
//        public override String ToString()

//            => "eMIP" + Version.Number + " EMP Adapter " + Id;

//        #endregion


//        #region Crap

//        Task<IEnumerable<WWCP.ChargeDetailRecord>> IEMPRoamingProvider.GetChargeDetailRecords(DateTime From, DateTime? To, eMobilityProvider_Id? ProviderId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<EVSEDataPull> IPullData.PullEVSEData(DateTime? LastCall, GeoCoordinate? SearchCenter, float DistanceKM, eMobilityProvider_Id? ProviderId, IEnumerable<ChargingStationOperator_Id> OperatorIdFilter, IEnumerable<Country> CountryCodeFilter, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<EVSEStatusPull> IPullStatus.PullEVSEStatus(DateTime? LastCall, GeoCoordinate? SearchCenter, float DistanceKM, EVSEStatusTypes? EVSEStatusFilter, eMobilityProvider_Id? ProviderId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<ReservationResult> IReserveRemoteStartStop.Reserve(WWCP.EVSE_Id EVSEId, DateTime? StartTime, TimeSpan? Duration, ChargingReservation_Id? ReservationId, eMobilityProvider_Id? ProviderId, AuthIdentification Identification, ChargingProduct ChargingProduct, IEnumerable<AuthenticationToken> AuthTokens, IEnumerable<eMobilityAccount_Id> eMAIds, IEnumerable<uint> PINs, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<CancelReservationResult> IReserveRemoteStartStop.CancelReservation(ChargingReservation_Id ReservationId, ChargingReservationCancellationReason Reason, eMobilityProvider_Id? ProviderId, WWCP.EVSE_Id? EVSEId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<RemoteStartEVSEResult> IReserveRemoteStartStop.RemoteStart(WWCP.EVSE_Id EVSEId, ChargingProduct ChargingProduct, ChargingReservation_Id? ReservationId, ChargingSession_Id? SessionId, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<RemoteStartChargingStationResult> IReserveRemoteStartStop.RemoteStart(WWCP.ChargingStation_Id ChargingStationId, ChargingProduct ChargingProduct, ChargingReservation_Id? ReservationId, ChargingSession_Id? SessionId, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<RemoteStopResult> IReserveRemoteStartStop.RemoteStop(ChargingSession_Id SessionId, ReservationHandling? ReservationHandling, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<RemoteStopEVSEResult> IReserveRemoteStartStop.RemoteStop(WWCP.EVSE_Id EVSEId, ChargingSession_Id SessionId, ReservationHandling? ReservationHandling, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<RemoteStopChargingStationResult> IReserveRemoteStartStop.RemoteStop(WWCP.ChargingStation_Id ChargingStationId, ChargingSession_Id SessionId, ReservationHandling? ReservationHandling, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion


//    }

//}
