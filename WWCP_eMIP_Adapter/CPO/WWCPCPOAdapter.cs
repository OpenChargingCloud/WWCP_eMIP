﻿/*
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
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Org.BouncyCastle.Bcpg.OpenPgp;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.CPO
{

    /// <summary>
    /// A WWCP wrapper for the eMIP CPO Roaming client which maps
    /// WWCP data structures onto eMIP data structures and vice versa.
    /// </summary>
    public class WWCPCPOAdapter : AWWCPCSOAdapter,
                                  ICSORoamingProvider,
                                  IEquatable <WWCPCPOAdapter>,
                                  IComparable<WWCPCPOAdapter>,
                                  IComparable
    {

        #region Data

        //private        readonly  EVSE2EVSEDataRecordDelegate                            _EVSE2EVSEDataRecord;

        //private        readonly  EVSEStatusUpdate2EVSEStatusRecordDelegate              _EVSEStatusUpdate2EVSEStatusRecord;

        private        readonly  WWCPChargeDetailRecord2ChargeDetailRecordDelegate      _WWCPChargeDetailRecord2eMIPChargeDetailRecord;

        //private        readonly  EVSEDataRecord2XMLDelegate                             _EVSEDataRecord2XML;

        //private        readonly  EVSEStatusRecord2XMLDelegate                           _EVSEStatusRecord2XML;

        //private        readonly  ChargeDetailRecord2XMLDelegate                         _ChargeDetailRecord2XML;

        private static readonly  Regex                                                  pattern                      = new Regex(@"\s=\s");

        private readonly        List<ChargeDetailRecord>                                eMIP_ChargeDetailRecords_Queue;

        //protected readonly      SemaphoreSlim                                           FlusheMIPChargeDetailRecordsLock      = new SemaphoreSlim(1, 1);

        public readonly static  TimeSpan                                                DefaultRequestTimeout       = TimeSpan.FromSeconds(30);


        /// <summary>
        /// The default send heartbeats intervall.
        /// </summary>
        public readonly static  TimeSpan                                                DefaultSendHeartbeatsEvery  = TimeSpan.FromMinutes(5);

        private readonly        SemaphoreSlim                                           SendHeartbeatLock           = new SemaphoreSlim(1, 1);
        private readonly        Timer                                                   SendHeartbeatsTimer;

        private                 UInt64                                                  _SendHeartbeatsRunId         = 1;

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
        public event OnAuthorizeStartRequestDelegate                  OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate                 OnAuthorizeStartResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStartRequestDelegate              OnAuthorizeEVSEStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStartResponseDelegate             OnAuthorizeEVSEStartResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStartRequestDelegate   OnAuthorizeChargingStationStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStartResponseDelegate  OnAuthorizeChargingStationStartResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStartRequestDelegate      OnAuthorizeChargingPoolStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStartResponseDelegate     OnAuthorizeChargingPoolStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate                  OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate                 OnAuthorizeStopResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStopRequestDelegate              OnAuthorizeEVSEStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStopResponseDelegate             OnAuthorizeEVSEStopResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStopRequestDelegate   OnAuthorizeChargingStationStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStopResponseDelegate  OnAuthorizeChargingStationStopResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStopRequestDelegate      OnAuthorizeChargingPoolStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStopResponseDelegate     OnAuthorizeChargingPoolStopResponse;

        #endregion

        #region OnSendCDRRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record was enqueued for later sending upstream.
        /// </summary>
        public event OnSendCDRRequestDelegate   OnEnqueueSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record will be send upstream.
        /// </summary>
        public event OnSendCDRRequestDelegate   OnSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record had been sent upstream.
        /// </summary>
        public event OnSendCDRResponseDelegate  OnSendCDRsResponse;

        #endregion


        #region SendHeartbeat

        public delegate void SendHeartbeatStartedDelegate(AWWCPCSOAdapter Sender, DateTime StartTime, TimeSpan Every, UInt64 RunId);

        public event SendHeartbeatStartedDelegate SendHeartbeatStartedEvent;


        public delegate void SendHeartbeatFinishedDelegate(AWWCPCSOAdapter Sender, DateTime StartTime, DateTime EndTime, TimeSpan Runtime, TimeSpan Every, UInt64 RunId);

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
        /// <param name="PublicKeyRing">The public key ring of the entity.</param>
        /// <param name="SecretKeyRing">The secrect key ring of the entity.</param>
        /// <param name="DNSClient">The attached DNS service.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id                              Id,
                              I18NString                                         Name,
                              I18NString                                         Description,
                              RoamingNetwork                                     RoamingNetwork,

                              Partner_Id                                         PartnerId,
                              CPORoaming                                         CPORoaming,

                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,
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

                              PgpPublicKeyRing                                   PublicKeyRing                                   = null,
                              PgpSecretKeyRing                                   SecretKeyRing                                   = null,
                              DNSClient                                          DNSClient                                       = null)

            : base(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   CustomEVSEIdMapper,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   PublicKeyRing,
                   SecretKeyRing,
                   DNSClient ?? CPORoaming?.DNSClient)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),        "The given roaming provider name must not be null or empty!");

            if (CPORoaming == null)
                throw new ArgumentNullException(nameof(CPORoaming),  "The given eMIP CPO Roaming object must not be null!");

            #endregion

            this.PartnerId                                        = PartnerId;
            this.CPORoaming                                       = CPORoaming;

            //this._EVSE2EVSEDataRecord                             = EVSE2EVSEDataRecord;
            //this._EVSEStatusUpdate2EVSEStatusRecord               = EVSEStatusUpdate2EVSEStatusRecord;
            this._WWCPChargeDetailRecord2eMIPChargeDetailRecord   = WWCPChargeDetailRecord2eMIPChargeDetailRecord;

            this.eMIP_ChargeDetailRecords_Queue                   = new List<ChargeDetailRecord>();

            this.SendHeartbeatsEvery                              = SendHeartbeatsEvery ?? DefaultSendHeartbeatsEvery;
            this.SendHeartbeatsTimer                              = new Timer(SendHeartbeat, null, this.SendHeartbeatsEvery, this.SendHeartbeatsEvery);
            this.DisableSendHeartbeats                            = DisableSendHeartbeats;

            // Link incoming eMIP events...

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
        /// <param name="PublicKeyRing">The public key ring of the entity.</param>
        /// <param name="SecretKeyRing">The secrect key ring of the entity.</param>
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

                              PgpPublicKeyRing                                   PublicKeyRing                                   = null,
                              PgpSecretKeyRing                                   SecretKeyRing                                   = null,
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

                   PublicKeyRing,
                   SecretKeyRing,
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
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
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
        /// <param name="PublicKeyRing">The public key ring of the entity.</param>
        /// <param name="SecretKeyRing">The secrect key ring of the entity.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id                              Id,
                              I18NString                                         Name,
                              I18NString                                         Description,
                              RoamingNetwork                                     RoamingNetwork,

                              Partner_Id                                         PartnerId,

                              String                                             RemoteHostname,
                              IPPort                                             RemoteTCPPort                                   = null,
                              RemoteCertificateValidationCallback                RemoteCertificateValidator                      = null,
                              LocalCertificateSelectionCallback                  ClientCertificateSelector                       = null,
                              String                                             RemoteHTTPVirtualHost                           = null,
                              String                                             URIPrefix                                       = CPOClient.DefaultURIPrefix,
                              String                                             HTTPUserAgent                                   = CPOClient.DefaultHTTPUserAgent,
                              TimeSpan?                                          RequestTimeout                                  = null,
                              Byte?                                              MaxNumberOfRetries                              = CPOClient.DefaultMaxNumberOfRetries,

                              String                                             ServerName                                      = CPOServer.DefaultHTTPServerName,
                              String                                             ServiceId                                       = null,
                              IPPort                                             ServerTCPPort                                   = null,
                              String                                             ServerURIPrefix                                 = CPOServer.DefaultURIPrefix,
                              HTTPContentType                                    ServerContentType                               = null,
                              Boolean                                            ServerRegisterHTTPRootService                   = true,
                              Boolean                                            ServerAutoStart                                 = false,

                              String                                             ClientLoggingContext                            = CPOClient.CPOClientLogger.DefaultContext,
                              String                                             ServerLoggingContext                            = CPOServerLogger.DefaultContext,
                              LogfileCreatorDelegate                             LogfileCreator                                  = null,

                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,
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

                              PgpPublicKeyRing                                   PublicKeyRing                                   = null,
                              PgpSecretKeyRing                                   SecretKeyRing                                   = null,
                              DNSClient                                          DNSClient                                       = null)

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
                                  URIPrefix,
                                  HTTPUserAgent,
                                  RequestTimeout,
                                  MaxNumberOfRetries,

                                  ServerName,
                                  ServiceId,
                                  ServerTCPPort,
                                  ServerURIPrefix,
                                  ServerContentType,
                                  ServerRegisterHTTPRootService,
                                  false,

                                  ClientLoggingContext,
                                  ServerLoggingContext,
                                  LogfileCreator,

                                  DNSClient),

                   IncludeEVSEIds,
                   IncludeEVSEs,
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

                   PublicKeyRing,
                   SecretKeyRing,
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
                                          Warnings.Add(Warning.Create(e.Message, evsestatusupdate));
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
                                                                   null, //Transaction_Id.Random(),
                                                                   null, //AvailabilityAdminStatusUntil
                                                                   null, //AvailabilityAdminStatusComment

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
                                                               ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
                                                               : Warnings.AddAndReturnList("No HTTP body received!"),
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
                                          Warnings.Add(Warning.Create(e.Message, evsestatusupdate));
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
                                                           null, //Transaction_Id.Random(),
                                                           null, //BusyStatusUntil
                                                           null, //BusyStatusComment

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
                                                               ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
                                                               : Warnings.AddAndReturnList("No HTTP body received!"),
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

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
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

                        FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToRemoveQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

                    var FilteredEVSEs = EVSEs.Where(evse => _IncludeEVSEs(evse) &&
                                                            _IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

                    var FilteredEVSEs = EVSEs.Where(evse => _IncludeEVSEs(evse) &&
                                                            _IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

                    var FilteredEVSEs = EVSEs.Where(evse => _IncludeEVSEs(evse) &&
                                                            _IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToUpdateQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

                    var FilteredEVSEs = EVSEs.Where(evse => _IncludeEVSEs(evse) &&
                                                            _IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToRemoveQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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
                return PushEVSEStatusResult.NoOperation(Id,
                                                        this);

            if (DisablePushStatus)
                return PushEVSEStatusResult.AdminDown(Id,
                                                      this,
                                                      StatusUpdates);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                return await UpdateStatus(this,
                                          StatusUpdates,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout).

                             ConfigureAwait(false);

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

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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
                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
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

                        FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

                    var AddData = false;

                    foreach (var evse in ChargingPool.EVSEs)
                    {
                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
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

                        FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

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

        #region AuthorizeStart/-Stop  directly...

        #region AuthorizeStart(AuthIdentification,                    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),   "The given authentication token must not be null!");


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
                                                OperatorId,
                                                AuthIdentification,
                                                ChargingProduct,
                                                SessionId,
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            DateTime         Endtime;
            TimeSpan         Runtime;
            AuthStartResult  result;

            //if (DisableAuthentication)
            //{

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.AdminDown(Id,
                                                     this,
                                                     SessionId,
                                                     Runtime);

            //}

            //else
            //{

            //    var response = await CPORoaming.
            //                             AuthorizeStart(OperatorId.HasValue
            //                                                ? OperatorId.Value.ToeMIP(DefaultOperatorIdFormat)
            //                                                : DefaultOperatorId,
            //                                            AuthIdentification. ToeMIP().RFIDId.Value,
            //                                            null,
            //                                            ChargingProduct?.Id.ToeMIP(),
            //                                            SessionId.          ToeMIP(),
            //                                            null,

            //                                            Timestamp,
            //                                            CancellationToken,
            //                                            EventTrackingId,
            //                                            RequestTimeout);


            //    Endtime  = DateTime.UtcNow;
            //    Runtime  = Endtime - StartTime;

            //    if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
            //        response.Content                     != null              &&
            //        response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
            //    {

            //        result = AuthStartResult.Authorized(
            //                     Id,
            //                     this,
            //                     response.Content.SessionId.ToWWCP().Value,
            //                     ProviderId:      response.Content.ProviderId.ToWWCP(),
            //                     Description:     response.Content.StatusCode.Description,
            //                     AdditionalInfo:  response.Content.StatusCode.AdditionalInfo,
            //                     Runtime:         Runtime
            //                 );

            //    }

            //    else
            //        result = AuthStartResult.NotAuthorized(
            //                     Id,
            //                     this,
            //                     SessionId,
            //                     response.Content.ProviderId.ToWWCP(),
            //                     response.Content.StatusCode.Description,
            //                     response.Content.StatusCode.AdditionalInfo,
            //                     Runtime
            //                 );

            //}


            #region Send OnAuthorizeStartResponse event

            try
            {

                OnAuthorizeStartResponse?.Invoke(Endtime,
                                                 Timestamp.Value,
                                                 this,
                                                 Id.ToString(),
                                                 EventTrackingId,
                                                 RoamingNetwork.Id,
                                                 OperatorId,
                                                 AuthIdentification,
                                                 ChargingProduct,
                                                 SessionId,
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

        #region AuthorizeStart(AuthIdentification, EVSEId,            ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartEVSEResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           WWCP.EVSE_Id                 EVSEId,
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

            #region Send OnAuthorizeEVSEStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeEVSEStartRequest?.Invoke(StartTime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id.ToString(),
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    OperatorId,
                                                    AuthIdentification,
                                                    EVSEId,
                                                    ChargingProduct,
                                                    SessionId,
                                                    new ISendAuthorizeStartStop[0],
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeEVSEStartRequest));
            }

            #endregion


            DateTime             Endtime;
            TimeSpan             Runtime;
            AuthStartEVSEResult  result;

            //if (DisableAuthentication)
            //{

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartEVSEResult.AdminDown(Id,
                                                         this,
                                                         SessionId,
                                                         Runtime);

            //}

            //else
            //{

            //    var response  = await CPORoaming.
            //                              AuthorizeStart(OperatorId.HasValue
            //                                                ? OperatorId.Value.ToeMIP(DefaultOperatorIdFormat)
            //                                                : DefaultOperatorId,
            //                                             AuthIdentification. ToeMIP().RFIDId.Value,
            //                                             EVSEId.             ToeMIP(),
            //                                             ChargingProduct?.Id.ToeMIP(),
            //                                             SessionId.          ToeMIP(),
            //                                             null,

            //                                             Timestamp,
            //                                             CancellationToken,
            //                                             EventTrackingId,
            //                                             RequestTimeout);


            //    Endtime  = DateTime.UtcNow;
            //    Runtime  = Endtime - StartTime;

            //    if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
            //        response.Content                     != null              &&
            //        response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
            //    {

            //        result = AuthStartEVSEResult.Authorized(
            //                     Id,
            //                     this,
            //                     response.Content.SessionId.ToWWCP().Value,
            //                     ProviderId:      response.Content.ProviderId.ToWWCP(),
            //                     Description:     response.Content.StatusCode.Description,
            //                     AdditionalInfo:  response.Content.StatusCode.AdditionalInfo,
            //                     NumberOfRetries: response.NumberOfRetries,
            //                     Runtime:         Runtime
            //                 );

            //    }

            //    else
            //        result = AuthStartEVSEResult.NotAuthorized(
            //                     Id,
            //                     this,
            //                     SessionId,
            //                     response.Content.ProviderId.ToWWCP(),
            //                     response.Content.StatusCode.Description,
            //                     response.Content.StatusCode.AdditionalInfo,
            //                     Runtime
            //                 );

            //}


            #region Send OnAuthorizeEVSEStartResponse event

            try
            {

                OnAuthorizeEVSEStartResponse?.Invoke(Endtime,
                                                     Timestamp.Value,
                                                     this,
                                                     Id.ToString(),
                                                     EventTrackingId,
                                                     RoamingNetwork.Id,
                                                     OperatorId,
                                                     AuthIdentification,
                                                     EVSEId,
                                                     ChargingProduct,
                                                     SessionId,
                                                     new ISendAuthorizeStartStop[0],
                                                     RequestTimeout,
                                                     result,
                                                     Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeEVSEStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(AuthIdentification, ChargingStationId, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingStationId">The unique identification charging station.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingStationResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           WWCP.ChargingStation_Id      ChargingStationId,
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

            #region Send OnAuthorizeChargingStationStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingStationStartRequest?.Invoke(StartTime,
                                                               Timestamp.Value,
                                                               this,
                                                               Id.ToString(),
                                                               EventTrackingId,
                                                               RoamingNetwork.Id,
                                                               OperatorId,
                                                               AuthIdentification,
                                                               ChargingStationId,
                                                               ChargingProduct,
                                                               SessionId,
                                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStartRequest));
            }

            #endregion


            DateTime                        Endtime;
            TimeSpan                        Runtime;
            AuthStartChargingStationResult  result;

            //if (DisableAuthentication)
            //{

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartChargingStationResult.AdminDown(Id,
                                                                    this,
                                                                    SessionId,
                                                                    Runtime);

            //}

            //else
            //{

            //    var response  = await CPORoaming.
            //                              AuthorizeStart(OperatorId.HasValue
            //                                                ? OperatorId.Value.ToeMIP(DefaultOperatorIdFormat)
            //                                                : DefaultOperatorId,
            //                                             AuthIdentification. ToeMIP().RFIDId.Value,
            //                                             null,
            //                                             ChargingProduct?.Id.ToeMIP(),
            //                                             SessionId.          ToeMIP(),
            //                                             null,

            //                                             Timestamp,
            //                                             CancellationToken,
            //                                             EventTrackingId,
            //                                             RequestTimeout);


            //    Endtime  = DateTime.UtcNow;
            //    Runtime  = Endtime - StartTime;

            //    if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
            //        response.Content                     != null              &&
            //        response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
            //    {

            //        result = AuthStartChargingStationResult.Authorized(
            //                     Id,
            //                     this,
            //                     response.Content.SessionId.ToWWCP().Value,
            //                     ProviderId:      response.Content.ProviderId.ToWWCP(),
            //                     Description:     response.Content.StatusCode.Description,
            //                     AdditionalInfo:  response.Content.StatusCode.AdditionalInfo,
            //                     Runtime:         Runtime
            //                 );

            //    }

            //    else
            //        result = AuthStartChargingStationResult.NotAuthorized(
            //                     Id,
            //                     this,
            //                     SessionId,
            //                     response.Content.ProviderId.ToWWCP(),
            //                     response.Content.StatusCode.Description,
            //                     response.Content.StatusCode.AdditionalInfo,
            //                     Runtime
            //                 );

            //}


            #region Send OnAuthorizeChargingStationStartResponse event

            try
            {

                OnAuthorizeChargingStationStartResponse?.Invoke(Endtime,
                                                                Timestamp.Value,
                                                                this,
                                                                Id.ToString(),
                                                                EventTrackingId,
                                                                RoamingNetwork.Id,
                                                                OperatorId,
                                                                AuthIdentification,
                                                                ChargingStationId,
                                                                ChargingProduct,
                                                                SessionId,
                                                                RequestTimeout,
                                                                result,
                                                                Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(AuthIdentification, ChargingPoolId,    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging pool.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingPoolId">The unique identification charging pool.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingPoolResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           WWCP.ChargingPool_Id         ChargingPoolId,
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

            #region Send OnAuthorizeChargingPoolStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingPoolStartRequest?.Invoke(StartTime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id.ToString(),
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            AuthIdentification,
                                                            ChargingPoolId,
                                                            ChargingProduct,
                                                            SessionId,
                                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStartRequest));
            }

            #endregion


            DateTime                     Endtime;
            TimeSpan                     Runtime;
            AuthStartChargingPoolResult  result;

            //if (DisableAuthentication)
            //{

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartChargingPoolResult.AdminDown(Id,
                                                                 this,
                                                                 SessionId,
                                                                 Runtime);

            //}

            //else
            //{

            //    var response  = await CPORoaming.
            //                              AuthorizeStart(OperatorId.HasValue
            //                                                ? OperatorId.Value.ToeMIP(DefaultOperatorIdFormat)
            //                                                : DefaultOperatorId,
            //                                             AuthIdentification. ToeMIP().RFIDId.Value,
            //                                             null,
            //                                             ChargingProduct?.Id.ToeMIP(),
            //                                             SessionId.          ToeMIP(),
            //                                             null,

            //                                             Timestamp,
            //                                             CancellationToken,
            //                                             EventTrackingId,
            //                                             RequestTimeout);


            //    Endtime  = DateTime.UtcNow;
            //    Runtime  = Endtime - StartTime;

            //    if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
            //        response.Content                     != null              &&
            //        response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
            //    {

            //        result = AuthStartChargingPoolResult.Authorized(
            //                     Id,
            //                     this,
            //                     response.Content.SessionId.ToWWCP().Value,
            //                     ProviderId:      response.Content.ProviderId.ToWWCP(),
            //                     Description:     response.Content.StatusCode.Description,
            //                     AdditionalInfo:  response.Content.StatusCode.AdditionalInfo,
            //                     Runtime:         Runtime
            //                 );

            //    }

            //    else
            //        result = AuthStartChargingPoolResult.NotAuthorized(
            //                     Id,
            //                     this,
            //                     SessionId,
            //                     response.Content.ProviderId.ToWWCP(),
            //                     response.Content.StatusCode.Description,
            //                     response.Content.StatusCode.AdditionalInfo,
            //                     Runtime
            //                 );

            //}


            #region Send OnAuthorizeChargingPoolStartResponse event

            try
            {

                OnAuthorizeChargingPoolStartResponse?.Invoke(Endtime,
                                                             Timestamp.Value,
                                                             this,
                                                             Id.ToString(),
                                                             EventTrackingId,
                                                             RoamingNetwork.Id,
                                                             OperatorId,
                                                             AuthIdentification,
                                                             ChargingPoolId,
                                                             ChargingProduct,
                                                             SessionId,
                                                             RequestTimeout,
                                                             result,
                                                             Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStartResponse));
            }

            #endregion

            return result;

        }

        #endregion


        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        #region AuthorizeStop(SessionId, AuthIdentification,                    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
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
                                               OperatorId,
                                               SessionId,
                                               AuthIdentification,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            DateTime        Endtime;
            TimeSpan        Runtime;
            AuthStopResult  result;

            //if (DisableAuthentication)
            //{

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.AdminDown(Id,
                                                    this,
                                                    SessionId,
                                                    Runtime);

            //}

            //else
            //{

            //    var response = await CPORoaming.AuthorizeStop(OperatorId.HasValue
            //                                                      ? OperatorId.Value.ToeMIP(DefaultOperatorIdFormat)
            //                                                      : DefaultOperatorId,
            //                                                  SessionId.         ToeMIP(),
            //                                                  AuthIdentification.ToeMIP().RFIDId.Value,
            //                                                  null,
            //                                                  null,

            //                                                  Timestamp,
            //                                                  CancellationToken,
            //                                                  EventTrackingId,
            //                                                  RequestTimeout);


            //    Endtime  = DateTime.UtcNow;
            //    Runtime  = Endtime - StartTime;

            //    if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
            //        response.Content                     != null              &&
            //        response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
            //    {

            //        result = AuthStopResult.Authorized(
            //                     Id,
            //                     this,
            //                     SessionId,
            //                     response.Content.ProviderId.ToWWCP(),
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.Description
            //                         : null,
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.AdditionalInfo
            //                         : null
            //                 );

            //    }
            //    else
            //        result = AuthStopResult.NotAuthorized(
            //                     Id,
            //                     this,
            //                     SessionId,
            //                     response.Content?.ProviderId.ToWWCP(),
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.Description
            //                         : null,
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.AdditionalInfo
            //                         : null
            //                 );

            //}


            #region Send OnAuthorizeStopResponse event

            try
            {

                OnAuthorizeStopResponse?.Invoke(Endtime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                OperatorId,
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

        #region AuthorizeStop(SessionId, AuthIdentification, EVSEId,            OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopEVSEResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          WWCP.EVSE_Id                 EVSEId,
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

            #region Send OnAuthorizeEVSEStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeEVSEStopRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   Id.ToString(),
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   OperatorId,
                                                   EVSEId,
                                                   SessionId,
                                                   AuthIdentification,
                                                   RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeEVSEStopRequest));
            }

            #endregion


            DateTime            Endtime;
            TimeSpan            Runtime;
            AuthStopEVSEResult  result;

            //if (DisableAuthentication)
            //{

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopEVSEResult.AdminDown(Id,
                                                        this,
                                                        SessionId,
                                                        Runtime);

            //}

            //else
            //{

            //    var response  = await CPORoaming.AuthorizeStop(OperatorId.HasValue
            //                                                      ? OperatorId.Value.ToeMIP(DefaultOperatorIdFormat)
            //                                                      : DefaultOperatorId,
            //                                                   SessionId.         ToeMIP(),
            //                                                   AuthIdentification.ToeMIP().RFIDId.Value,
            //                                                   EVSEId.            ToeMIP(),
            //                                                   null,

            //                                                   Timestamp,
            //                                                   CancellationToken,
            //                                                   EventTrackingId,
            //                                                   RequestTimeout);


            //    Endtime  = DateTime.UtcNow;
            //    Runtime  = Endtime - StartTime;

            //    if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
            //        response.Content                     != null              &&
            //        response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
            //    {

            //        result = AuthStopEVSEResult.Authorized(
            //                     Id,
            //                     this,
            //                     SessionId,
            //                     response.Content?.ProviderId?.ToWWCP(),
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.Description
            //                         : null,
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.AdditionalInfo
            //                         : null
            //                 );

            //    }
            //    else
            //        result = AuthStopEVSEResult.NotAuthorized(
            //                     Id,
            //                     this,
            //                     SessionId,
            //                     response.Content?.ProviderId?.ToWWCP(),
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.Description
            //                         : null,
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.AdditionalInfo
            //                         : null
            //                 );

            //}


            #region Send OnAuthorizeEVSEStopResponse event

            try
            {

                OnAuthorizeEVSEStopResponse?.Invoke(Endtime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id.ToString(),
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    OperatorId,
                                                    EVSEId,
                                                    SessionId,
                                                    AuthIdentification,
                                                    RequestTimeout,
                                                    result,
                                                    Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeEVSEStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, AuthIdentification, ChargingStationId, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingStationResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          WWCP.ChargingStation_Id      ChargingStationId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
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

            #region Send OnAuthorizeChargingStationStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingStationStopRequest?.Invoke(StartTime,
                                                              Timestamp.Value,
                                                              this,
                                                              Id.ToString(),
                                                              EventTrackingId,
                                                              RoamingNetwork.Id,
                                                              OperatorId,
                                                              ChargingStationId,
                                                              SessionId,
                                                              AuthIdentification,
                                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStopRequest));
            }

            #endregion


            DateTime                       Endtime;
            TimeSpan                       Runtime;
            AuthStopChargingStationResult  result;

            //if (DisableAuthentication)
            //{

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopChargingStationResult.AdminDown(Id,
                                                                   this,
                                                                   SessionId,
                                                                   Runtime);

            //}

            //else
            //{

            //    var response  = await CPORoaming.AuthorizeStop(OperatorId.HasValue
            //                                                      ? OperatorId.Value.ToeMIP(DefaultOperatorIdFormat)
            //                                                      : DefaultOperatorId,
            //                                                   SessionId.         ToeMIP(),
            //                                                   AuthIdentification.ToeMIP().RFIDId.Value,
            //                                                   null,
            //                                                   null,

            //                                                   Timestamp,
            //                                                   CancellationToken,
            //                                                   EventTrackingId,
            //                                                   RequestTimeout);


            //    Endtime  = DateTime.UtcNow;
            //    Runtime  = Endtime - StartTime;

            //    if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
            //        response.Content                     != null              &&
            //        response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
            //    {

            //        result = AuthStopChargingStationResult.Authorized(
            //                     Id,
            //                     this,
            //                     SessionId,
            //                     response.Content?.ProviderId?.ToWWCP(),
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.Description
            //                         : null,
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.AdditionalInfo
            //                         : null
            //                 );

            //    }
            //    else
            //        result = AuthStopChargingStationResult.NotAuthorized(
            //                     Id,
            //                     this,
            //                     SessionId,
            //                     response.Content?.ProviderId?.ToWWCP(),
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.Description
            //                         : null,
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.AdditionalInfo
            //                         : null
            //                 );

            //}


            #region Send OnAuthorizeChargingStationStopResponse event

            try
            {

                OnAuthorizeChargingStationStopResponse?.Invoke(Endtime,
                                                               Timestamp.Value,
                                                               this,
                                                               Id.ToString(),
                                                               EventTrackingId,
                                                               RoamingNetwork.Id,
                                                               OperatorId,
                                                               ChargingStationId,
                                                               SessionId,
                                                               AuthIdentification,
                                                               RequestTimeout,
                                                               result,
                                                               Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, AuthIdentification, ChargingPoolId,    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging pool.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingPoolResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          WWCP.ChargingPool_Id         ChargingPoolId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
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

            #region Send OnAuthorizeChargingPoolStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingPoolStopRequest?.Invoke(StartTime,
                                                           Timestamp.Value,
                                                           this,
                                                           Id.ToString(),
                                                           EventTrackingId,
                                                           RoamingNetwork.Id,
                                                           OperatorId,
                                                           ChargingPoolId,
                                                           SessionId,
                                                           AuthIdentification,
                                                           RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStopRequest));
            }

            #endregion


            DateTime                    Endtime;
            TimeSpan                    Runtime;
            AuthStopChargingPoolResult  result;

            //if (DisableAuthentication)
            //{

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopChargingPoolResult.AdminDown(Id,
                                                                this,
                                                                SessionId,
                                                                Runtime);

            //}

            //else
            //{

            //    var response  = await CPORoaming.AuthorizeStop(OperatorId.HasValue
            //                                                      ? OperatorId.Value.ToeMIP(DefaultOperatorIdFormat)
            //                                                      : DefaultOperatorId,
            //                                                   SessionId.         ToeMIP(),
            //                                                   AuthIdentification.ToeMIP().RFIDId.Value,
            //                                                   null,
            //                                                   null,

            //                                                   Timestamp,
            //                                                   CancellationToken,
            //                                                   EventTrackingId,
            //                                                   RequestTimeout);


            //    Endtime  = DateTime.UtcNow;
            //    Runtime  = Endtime - StartTime;

            //    if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
            //        response.Content                     != null              &&
            //        response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
            //    {

            //        result = AuthStopChargingPoolResult.Authorized(
            //                     Id,
            //                     this,
            //                     SessionId,
            //                     response.Content?.ProviderId?.ToWWCP(),
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.Description
            //                         : null,
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.AdditionalInfo
            //                         : null
            //                 );

            //    }
            //    else
            //        result = AuthStopChargingPoolResult.NotAuthorized(
            //                     Id,
            //                     this,
            //                     SessionId,
            //                     response.Content?.ProviderId?.ToWWCP(),
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.Description
            //                         : null,
            //                     response.Content.StatusCode.HasResult
            //                         ? response.Content.StatusCode.AdditionalInfo
            //                         : null
            //                 );

            //}


            #region Send OnAuthorizeChargingPoolStopResponse event

            try
            {

                OnAuthorizeChargingPoolStopResponse?.Invoke(Endtime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id.ToString(),
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            ChargingPoolId,
                                                            SessionId,
                                                            AuthIdentification,
                                                            RequestTimeout,
                                                            result,
                                                            Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

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
        async Task<SendCDRsResult>

            ISendChargeDetailRecords.SendChargeDetailRecords(IEnumerable<WWCP.ChargeDetailRecord>  ChargeDetailRecords,
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


            //#region if disabled => 'AdminDown'...

            DateTime        Endtime;
            TimeSpan        Runtime;
            SendCDRsResult  result;

            //if (DisableSendChargeDetailRecords)
            //{

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = SendCDRsResult.AdminDown(Id,
                                                    this,
                                                    ChargeDetailRecords,
                                                    Runtime: Runtime);

            //}

            //#endregion

            //else
            //{

            //    var LockTaken = await FlusheMIPChargeDetailRecordsLock.WaitAsync(TimeSpan.FromSeconds(60));

            //    try
            //    {

            //        if (LockTaken)
            //        {

            //            var SendCDRsResults = new List<SendCDRResult>();

            //            #region if enqueuing is requested...

            //            if (TransmissionType == TransmissionTypes.Enqueue)
            //            {

            //                #region Send OnEnqueueSendCDRRequest event

            //                try
            //                {

            //                    OnEnqueueSendCDRsRequest?.Invoke(DateTime.UtcNow,
            //                                                     Timestamp.Value,
            //                                                     this,
            //                                                     Id.ToString(),
            //                                                     EventTrackingId,
            //                                                     RoamingNetwork.Id,
            //                                                     ChargeDetailRecords,
            //                                                     RequestTimeout);

            //                }
            //                catch (Exception e)
            //                {
            //                    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
            //                }

            //                #endregion

            //                foreach (var ChargeDetailRecord in ChargeDetailRecords)
            //                {

            //                    try
            //                    {

            //                        eMIP_ChargeDetailRecords_Queue.Add(ChargeDetailRecord.ToeMIP(_WWCPChargeDetailRecord2eMIPChargeDetailRecord));
            //                        SendCDRsResults.Add(new SendCDRResult(ChargeDetailRecord,
            //                                                              SendCDRResultTypes.Enqueued));

            //                    }
            //                    catch (Exception e)
            //                    {
            //                        SendCDRsResults.Add(new SendCDRResult(ChargeDetailRecord,
            //                                                              SendCDRResultTypes.CouldNotConvertCDRFormat,
            //                                                              e.Message));
            //                    }

            //                }

            //                Endtime  = DateTime.UtcNow;
            //                Runtime  = Endtime - StartTime;
            //                result   = SendCDRsResult.Enqueued(Id,
            //                                                   this,
            //                                                   "Enqueued for at least " + FlushChargeDetailRecordsEvery.TotalSeconds + " seconds!",
            //                                                   SendCDRsResults.SafeWhere(cdrresult => cdrresult.Result != SendCDRResultTypes.Enqueued),
            //                                                   Runtime: Runtime);

            //                FlushChargeDetailRecordsTimer.Change(_FlushChargeDetailRecordsEvery, Timeout.Infinite);

            //            }

            //            #endregion

            //            #region ...or send at once!

            //            else
            //            {

            //                HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>> response;

            //                foreach (var ChargeDetailRecord in ChargeDetailRecords)
            //                {

            //                    try
            //                    {

            //                        response = await CPORoaming.SendChargeDetailRecord(ChargeDetailRecord.ToeMIP(_WWCPChargeDetailRecord2eMIPChargeDetailRecord),

            //                                                                           Timestamp,
            //                                                                           CancellationToken,
            //                                                                           EventTrackingId,
            //                                                                           RequestTimeout);

            //                        if (response.HTTPStatusCode == HTTPStatusCode.OK &&
            //                            response.Content        != null              &&
            //                            response.Content.Result)
            //                        {
            //                            SendCDRsResults.Add(new SendCDRResult(ChargeDetailRecord,
            //                                                                  SendCDRResultTypes.Success));
            //                        }

            //                        else
            //                            SendCDRsResults.Add(new SendCDRResult(ChargeDetailRecord,
            //                                                                  SendCDRResultTypes.Error,
            //                                                                  response.HTTPBodyAsUTF8String));

            //                    }
            //                    catch (Exception e)
            //                    {
            //                        SendCDRsResults.Add(new SendCDRResult(ChargeDetailRecord,
            //                                                              SendCDRResultTypes.CouldNotConvertCDRFormat,
            //                                                              e.Message));
            //                    }

            //                }

            //                Endtime  = DateTime.UtcNow;
            //                Runtime  = Endtime - StartTime;

            //                if      (SendCDRsResults.All(cdrresult => cdrresult.Result == SendCDRResultTypes.Success))
            //                    result = SendCDRsResult.Success(Id,
            //                                                    this,
            //                                                    Runtime: Runtime);

            //                else
            //                    result = SendCDRsResult.Error(Id,
            //                                                  this,
            //                                                  SendCDRsResults.
            //                                                      Where (cdrresult => cdrresult.Result != SendCDRResultTypes.Success).
            //                                                      Select(cdrresult => cdrresult.ChargeDetailRecord),
            //                                                  Runtime: Runtime);


            //            }

            //            #endregion

            //        }

            //        #region Could not get the lock for toooo long!

            //        else
            //        {

            //            Endtime  = DateTime.UtcNow;
            //            Runtime  = Endtime - StartTime;
            //            result   = SendCDRsResult.Timeout(Id,
            //                                              this,
            //                                              "Could not " + (TransmissionType == TransmissionTypes.Enqueue ? "enqueue" : "send") + " charge detail records!",
            //                                              ChargeDetailRecords.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Timeout)),
            //                                              Runtime: Runtime);

            //        }

            //        #endregion

            //    }
            //    finally
            //    {
            //        if (LockTaken)
            //            FlusheMIPChargeDetailRecordsLock.Release();
            //    }

            //}


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
                                           result,
                                           Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsResponse));
            }

            #endregion

            return result;

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
                                                      TimeSpan.FromMilliseconds(_FlushEVSEFastStatusEvery),
                                                      _SendHeartbeatsRunId);

                    #endregion

                    var SendHeartbeatResponse = await CPORoaming.SendHeartbeat(PartnerId,
                                                                               Operator_Id.Parse("DE*BDO"),
                                                                               Transaction_Id.Random(),

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

            var EVSEsToAddQueueCopy                = new HashSet<EVSE>();
            var EVSEsToUpdateQueueCopy             = new HashSet<EVSE>();
            var EVSEStatusChangesDelayedQueueCopy  = new List<EVSEStatusUpdate>();
            var EVSEsToRemoveQueueCopy             = new HashSet<EVSE>();
            var EVSEsUpdateLogCopy                 = new Dictionary<EVSE,            PropertyUpdateInfos[]>();
            var ChargingStationsUpdateLogCopy      = new Dictionary<ChargingStation, PropertyUpdateInfos[]>();
            var ChargingPoolsUpdateLogCopy         = new Dictionary<ChargingPool,    PropertyUpdateInfos[]>();

            await DataAndStatusLock.WaitAsync();

            try
            {

                // Copy 'EVSEs to add', remove originals...
                EVSEsToAddQueueCopy                      = new HashSet<EVSE>                (EVSEsToAddQueue);
                EVSEsToAddQueue.Clear();

                // Copy 'EVSEs to update', remove originals...
                EVSEsToUpdateQueueCopy                   = new HashSet<EVSE>                (EVSEsToUpdateQueue);
                EVSEsToUpdateQueue.Clear();

                // Copy 'EVSE status changes', remove originals...
                EVSEStatusChangesDelayedQueueCopy        = new List<EVSEStatusUpdate>       (EVSEStatusChangesDelayedQueue);
                EVSEStatusChangesDelayedQueueCopy.AddRange(EVSEsToAddQueueCopy.SafeSelect(evse => new EVSEStatusUpdate(evse, evse.Status, evse.Status)));
                EVSEStatusChangesDelayedQueue.Clear();

                // Copy 'EVSEs to remove', remove originals...
                EVSEsToRemoveQueueCopy                   = new HashSet<EVSE>                (EVSEsToRemoveQueue);
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
                FlushEVSEDataAndStatusTimer.Change(Timeout.Infinite, Timeout.Infinite);

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

            #region Send changed EVSE status

            if (!DisablePushStatus &&
                EVSEStatusChangesDelayedQueueCopy.Count > 0)
            {

                //var PushEVSEStatusTask = PushEVSEStatus(EVSEStatusChangesDelayedQueueCopy,
                //                                        _FlushEVSEDataRunId == 1
                //                                            ? ActionTypes.fullLoad
                //                                            : ActionTypes.update,
                //                                        EventTrackingId: EventTrackingId);

                //PushEVSEStatusTask.Wait();

                //if (PushEVSEStatusTask.Result.Warnings.Any())
                //{

                //    SendOnWarnings(DateTime.UtcNow,
                //                   nameof(WWCPCPOAdapter) + Id,
                //                   "PushEVSEStatusTask",
                //                   PushEVSEStatusTask.Result.Warnings);

                //}

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

            await DataAndStatusLock.WaitAsync();

            try
            {

                // Copy 'EVSE status changes', remove originals...
                EVSEAdminStatusFastQueueCopy  = new List<EVSEAdminStatusUpdate>(EVSEAdminStatusChangesFastQueue.Where(statuschange => !EVSEsToAddQueue.Any(evse => evse == statuschange.EVSE)));
                EVSEStatusFastQueueCopy       = new List<EVSEStatusUpdate>     (EVSEStatusChangesFastQueue.     Where(statuschange => !EVSEsToAddQueue.Any(evse => evse == statuschange.EVSE)));

                // Add all evse status changes of EVSEs *NOT YET UPLOADED* into the delayed queue...
                var EVSEAdminStatusChangesDelayed = EVSEAdminStatusChangesFastQueue.Where(statuschange => EVSEsToAddQueue.Any(evse => evse == statuschange.EVSE)).ToArray();
                var EVSEStatusChangesDelayed      = EVSEStatusChangesFastQueue.     Where(statuschange => EVSEsToAddQueue.Any(evse => evse == statuschange.EVSE)).ToArray();

                if (EVSEAdminStatusChangesDelayed.Length > 0)
                    EVSEAdminStatusChangesDelayedQueue.AddRange(EVSEAdminStatusChangesDelayed);

                if (EVSEStatusChangesDelayed.     Length > 0)
                    EVSEStatusChangesDelayedQueue.     AddRange(EVSEStatusChangesDelayed);

                EVSEAdminStatusChangesFastQueue.Clear();
                EVSEStatusChangesFastQueue.     Clear();


                // Stop the timer. Will be rescheduled by next EVSE status change...
                FlushEVSEFastStatusTimer.Change(Timeout.Infinite, Timeout.Infinite);

            }
            finally
            {
                DataAndStatusLock.Release();
            }

            #endregion

            // Use events to check if something went wrong!
            var EventTrackingId = EventTracking_Id.New;

            #region Send changed EVSE status

            if (EVSEAdminStatusFastQueueCopy.Count > 0)
            {

                var _PushEVSEAdminStatus = await SetEVSEAvailabilityStatus(EVSEAdminStatusFastQueueCopy,
                                                                           EventTrackingId: EventTrackingId).
                                                     ConfigureAwait(false);

                if (_PushEVSEAdminStatus.Warnings.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "PushEVSEAdminFastStatus",
                                   _PushEVSEAdminStatus.Warnings);

                }

            }

            if (EVSEStatusFastQueueCopy.Count > 0)
            {

                var _PushEVSEStatus = await SetEVSEBusyStatus(EVSEStatusFastQueueCopy,
                                                              EventTrackingId: EventTrackingId).
                                                ConfigureAwait(false);

                if (_PushEVSEStatus.Warnings.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "PushEVSEFastStatus",
                                   _PushEVSEStatus.Warnings);

                }

            }

            #endregion

        }

        #endregion

        #region (timer) FlushChargeDetailRecords()

        protected override Boolean SkipFlushChargeDetailRecordsQueues()
            => eMIP_ChargeDetailRecords_Queue.Count == 0;

        protected override async Task FlushChargeDetailRecordsQueues()
        {

            //#region Make a thread local copy of all data

            //var LockTaken                    = await FlusheMIPChargeDetailRecordsLock.WaitAsync(TimeSpan.FromSeconds(30));
            //var ChargeDetailRecordQueueCopy  = new List<ChargeDetailRecord>();

            //try
            //{

            //    if (LockTaken)
            //    {

            //        // Copy CDRs, empty original queue...
            //        ChargeDetailRecordQueueCopy.AddRange(eMIP_ChargeDetailRecords_Queue);
            //        eMIP_ChargeDetailRecords_Queue.Clear();

            //        //// Stop the timer. Will be rescheduled by the next CDR...
            //        //FlushChargeDetailRecordsTimer.Change(Timeout.Infinite, Timeout.Infinite);

            //    }

            //}
            //catch (Exception e)
            //{

            //    while (e.InnerException != null)
            //        e = e.InnerException;

            //    DebugX.LogT(nameof(WWCPCPOAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

            //}

            //finally
            //{
            //    if (LockTaken)
            //        FlusheMIPChargeDetailRecordsLock.Release();
            //}

            //#endregion

            // Use the events to evaluate if something went wrong!

            #region Send charge detail records

            //if (ChargeDetailRecordQueueCopy.Count > 0)
            //{

            //    var EventTrackingId  = EventTracking_Id.New;
                //var results          = new List<HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>>>();

                //foreach (var chargedetailrecord in ChargeDetailRecordQueueCopy)
                //    results.Add(await CPORoaming.SendChargeDetailRecord(chargedetailrecord,
                //                                                        DateTime.UtcNow,
                //                                                        new CancellationTokenSource().Token,
                //                                                        EventTrackingId,
                //                                                        DefaultRequestTimeout));

                //var Warnings         = results.Where(result => result.Content

            //}

            #endregion

            //ToDo: Send FlushChargeDetailRecordsQueues result event...
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
            if (((Object) WWCPCPOAdapter1 == null) || ((Object) WWCPCPOAdapter2 == null))
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
        public static Boolean operator < (WWCPCPOAdapter  WWCPCPOAdapter1,
                                          WWCPCPOAdapter  WWCPCPOAdapter2)
        {

            if ((Object) WWCPCPOAdapter1 == null)
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
        public static Boolean operator <= (WWCPCPOAdapter WWCPCPOAdapter1,
                                           WWCPCPOAdapter WWCPCPOAdapter2)

            => !(WWCPCPOAdapter1 > WWCPCPOAdapter2);

        #endregion

        #region Operator >  (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (WWCPCPOAdapter WWCPCPOAdapter1,
                                          WWCPCPOAdapter WWCPCPOAdapter2)
        {

            if ((Object) WWCPCPOAdapter1 == null)
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
        public static Boolean operator >= (WWCPCPOAdapter WWCPCPOAdapter1,
                                           WWCPCPOAdapter WWCPCPOAdapter2)

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

            var WWCPCPOAdapter = Object as WWCPCPOAdapter;
            if ((Object) WWCPCPOAdapter == null)
                throw new ArgumentException("The given object is not an WWCPCPOAdapter!", nameof(Object));

            return CompareTo(WWCPCPOAdapter);

        }

        #endregion

        #region CompareTo(WWCPCPOAdapter)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter">An WWCPCPOAdapter object to compare with.</param>
        public Int32 CompareTo(WWCPCPOAdapter WWCPCPOAdapter)
        {

            if ((Object) WWCPCPOAdapter == null)
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

            var WWCPCPOAdapter = Object as WWCPCPOAdapter;
            if ((Object) WWCPCPOAdapter == null)
                return false;

            return Equals(WWCPCPOAdapter);

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

            if ((Object) WWCPCPOAdapter == null)
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