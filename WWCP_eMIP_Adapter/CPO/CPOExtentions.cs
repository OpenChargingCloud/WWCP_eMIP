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
using System.Net.Security;

using Org.BouncyCastle.Bcpg.OpenPgp;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using Org.BouncyCastle.Crypto.Parameters;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Extentions methods for the WWCP wrapper for eMIP roaming clients for charging station operators.
    /// </summary>
    public static class CPOExtentions
    {

        #region CreateeMIPv0_7_4_CSORoamingProvider(this RoamingNetwork, Id, Name, RemoteHostname, ... , Action = null)

        /// <summary>
        /// Create and register a new electric vehicle roaming provider
        /// using the eMIP protocol and having the given unique electric
        /// vehicle roaming provider identification.
        /// </summary>
        /// 
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="Description">An optional (multi-language) description of the charging station operator roaming provider.</param>
        /// 
        /// <param name="PartnerId">The unique identification of an eMIP communication partner.</param>
        /// 
        /// <param name="RemoteHostname">The hostname of the remote eMIP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote eMIP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote eMIP service.</param>
        /// <param name="URLPrefix">An default URI prefix.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ServerName"> An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURLPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerAuthorisationURL">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
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
        /// <param name="eMIPConfigurator">An optional delegate to configure the new eMIP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public static eMIPv0_7_4.CPO.WWCPCPOAdapter

            CreateeMIPv0_7_4_CSORoamingProvider(this RoamingNetwork                                               RoamingNetwork,
                                                EMPRoamingProvider_Id                                             Id,
                                                I18NString                                                        Name,
                                                I18NString                                                        Description,

                                                eMIPv0_7_4.Partner_Id                                             PartnerId,
                                                ChargingStationOperator                                           DefaultOperator,

                                                HTTPHostname                                                      RemoteHostname,
                                                IPPort?                                                           RemoteTCPPort                                            = null,
                                                HTTPHostname?                                                     RemoteHTTPVirtualHost                                    = null,
                                                RemoteCertificateValidationCallback                               RemoteCertificateValidator                               = null,
                                                LocalCertificateSelectionCallback                                 ClientCertificateSelector                                = null,
                                                HTTPPath?                                                         URLPrefix                                                = null,
                                                String                                                            HTTPUserAgent                                            = eMIPv0_7_4.CPO.CPOClient.DefaultHTTPUserAgent,
                                                TimeSpan?                                                         RequestTimeout                                           = null,
                                                TransmissionRetryDelayDelegate                                    TransmissionRetryDelay                                   = null,
                                                Byte?                                                             MaxNumberOfRetries                                       = eMIPv0_7_4.CPO.CPOClient.DefaultMaxNumberOfRetries,

                                                String                                                            ServerName                                               = eMIPv0_7_4.CPO.CPOServer.DefaultHTTPServerName,
                                                String                                                            ServiceId                                                = null,
                                                IPPort?                                                           ServerTCPPort                                            = null,
                                                HTTPPath?                                                         ServerURLPrefix                                          = null,
                                                String                                                            ServerAuthorisationURL                                   = eMIPv0_7_4.CPO.CPOServer.DefaultAuthorisationURL,
                                                HTTPContentType                                                   ServerContentType                                        = null,
                                                Boolean                                                           ServerRegisterHTTPRootService                            = true,
                                                Boolean                                                           ServerAutoStart                                          = false,

                                                String                                                            ClientLoggingContext                                     = eMIPv0_7_4.CPO.CPOClient.CPOClientLogger.DefaultContext,
                                                String                                                            ServerLoggingContext                                     = eMIPv0_7_4.CPO.CPOServerLogger.DefaultContext,
                                                LogfileCreatorDelegate                                            LogfileCreator                                           = null,

                                                //EVSE2EVSEDataRecordDelegate                                       EVSE2EVSEDataRecord                                      = null,
                                                //EVSEStatusUpdate2EVSEStatusRecordDelegate                         EVSEStatusUpdate2EVSEStatusRecord                        = null,
                                                eMIPv0_7_4.CPO.WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2eMIPChargeDetailRecord            = null,

                                                IncludeEVSEIdDelegate                                             IncludeEVSEIds                                           = null,
                                                IncludeEVSEDelegate                                               IncludeEVSEs                                             = null,
                                                IncludeChargingStationIdDelegate                                  IncludeChargingStationIds                                = null,
                                                IncludeChargingStationDelegate                                    IncludeChargingStations                                  = null,
                                                IncludeChargingPoolIdDelegate                                     IncludeChargingPoolIds                                   = null,
                                                IncludeChargingPoolDelegate                                       IncludeChargingPools                                     = null,
                                                ChargeDetailRecordFilterDelegate                                  ChargeDetailRecordFilter                                 = null,
                                                CustomOperatorIdMapperDelegate                                    CustomOperatorIdMapper                                   = null,
                                                CustomEVSEIdMapperDelegate                                        CustomEVSEIdMapper                                       = null,

                                                TimeSpan?                                                         SendHeartbeatsEvery                                      = null,
                                                TimeSpan?                                                         ServiceCheckEvery                                        = null,
                                                TimeSpan?                                                         StatusCheckEvery                                         = null,
                                                TimeSpan?                                                         CDRCheckEvery                                            = null,

                                                Boolean                                                           DisableSendHeartbeats                                    = false,
                                                Boolean                                                           DisablePushData                                          = false,
                                                Boolean                                                           DisablePushStatus                                        = false,
                                                Boolean                                                           DisableAuthentication                                    = false,
                                                Boolean                                                           DisableSendChargeDetailRecords                           = false,

                                                Action<eMIPv0_7_4.CPO.WWCPCPOAdapter>                             eMIPConfigurator                                         = null,
                                                Action<IEMPRoamingProvider>                                       Configurator                                             = null,

                                                String                                                            EllipticCurve                                            = "P-256",
                                                ECPrivateKeyParameters                                            PrivateKey                                               = null,
                                                PublicKeyCertificates                                             PublicKeyCertificates                                    = null,

                                                CounterValues?                                                    CPOClientSendHeartbeatCounter                            = null,
                                                CounterValues?                                                    CPOClientSetChargingPoolAvailabilityStatusCounter        = null,
                                                CounterValues?                                                    CPOClientSetChargingStationAvailabilityStatusCounter     = null,
                                                CounterValues?                                                    CPOClientSetEVSEAvailabilityStatusCounter                = null,
                                                CounterValues?                                                    CPOClientSetChargingConnectorAvailabilityStatusCounter   = null,
                                                CounterValues?                                                    CPOClientSetEVSEBusyStatusCounter                        = null,
                                                CounterValues?                                                    CPOClientSetEVSESyntheticStatusCounter                   = null,
                                                CounterValues?                                                    CPOClientGetServiceAuthorisationCounter                  = null,
                                                CounterValues?                                                    CPOClientSetSessionEventReportCounter                    = null,
                                                CounterValues?                                                    CPOClientSetChargeDetailRecordCounter                    = null,

                                                DNSClient                                                         DNSClient                                                = null)

        {

            #region Initial checks

            if (RoamingNetwork    == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (Id == null)
                throw new ArgumentNullException(nameof(Id),              "The given unique roaming provider identification must not be null!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

            if (RemoteHostname    == null)
                throw new ArgumentNullException(nameof(RemoteHostname),  "The given remote hostname must not be null!");

            #endregion

            var NewRoamingProvider = new eMIPv0_7_4.CPO.WWCPCPOAdapter(Id,
                                                                       Name,
                                                                       Description,
                                                                       RoamingNetwork,

                                                                       PartnerId,
                                                                       DefaultOperator,

                                                                       RemoteHostname,
                                                                       RemoteTCPPort,
                                                                       RemoteCertificateValidator,
                                                                       ClientCertificateSelector,
                                                                       RemoteHTTPVirtualHost,
                                                                       URLPrefix ?? eMIPv0_7_4.CPO.CPOClient.DefaultURLPrefix,
                                                                       HTTPUserAgent,
                                                                       RequestTimeout,
                                                                       TransmissionRetryDelay,
                                                                       MaxNumberOfRetries,

                                                                       ServerName,
                                                                       ServiceId,
                                                                       ServerTCPPort,
                                                                       ServerURLPrefix        ?? eMIPv0_7_4.CPO.CPOServer.DefaultURLPrefix,
                                                                       ServerAuthorisationURL ?? eMIPv0_7_4.CPO.CPOServer.DefaultAuthorisationURL,
                                                                       ServerContentType,
                                                                       ServerRegisterHTTPRootService,
                                                                       ServerAutoStart,

                                                                       ClientLoggingContext,
                                                                       ServerLoggingContext,
                                                                       LogfileCreator,

                                                                       IncludeEVSEIds,
                                                                       IncludeEVSEs,
                                                                       IncludeChargingStationIds,
                                                                       IncludeChargingStations,
                                                                       IncludeChargingPoolIds,
                                                                       IncludeChargingPools,
                                                                       ChargeDetailRecordFilter,
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

                                                                       DNSClient);


            eMIPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator) as eMIPv0_7_4.CPO.WWCPCPOAdapter;

        }

        #endregion

        #region CreateeMIPv0_7_4_CSORoamingProvider(this RoamingNetwork, Id, Name, SOAPServer, RemoteHostname, ...)

        /// <summary>
        /// Create and register a new electric vehicle roaming provider
        /// using the eMIP protocol and having the given unique electric
        /// vehicle roaming provider identification.
        /// </summary>
        /// 
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="Description">An optional (multi-language) description of the charging station operator roaming provider.</param>
        /// 
        /// <param name="PartnerId">The unique identification of an eMIP communication partner.</param>
        /// 
        /// <param name="RemoteHostname">The hostname of the remote eMIP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote eMIP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote eMIP service.</param>
        /// <param name="URLPrefix">An default URI prefix.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
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
        /// <param name="eMIPConfigurator">An optional delegate to configure the new eMIP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public static eMIPv0_7_4.CPO.WWCPCPOAdapter

            CreateeMIPv0_7_4_CSORoamingProvider(this RoamingNetwork                                               RoamingNetwork,
                                                EMPRoamingProvider_Id                                             Id,
                                                I18NString                                                        Name,
                                                I18NString                                                        Description,
                                                SOAPServer                                                        SOAPServer,

                                                eMIPv0_7_4.Partner_Id                                             PartnerId,
                                                ChargingStationOperator                                           DefaultOperator,


                                                HTTPHostname                                                      RemoteHostname,
                                                IPPort?                                                           RemoteTCPPort                                            = null,
                                                RemoteCertificateValidationCallback                               RemoteCertificateValidator                               = null,
                                                LocalCertificateSelectionCallback                                 ClientCertificateSelector                                = null,
                                                HTTPHostname?                                                     RemoteHTTPVirtualHost                                    = null,
                                                HTTPPath?                                                         URLPrefix                                                = null,
                                                String                                                            HTTPUserAgent                                            = eMIPv0_7_4.CPO.CPOClient.DefaultHTTPUserAgent,
                                                TimeSpan?                                                         RequestTimeout                                           = null,
                                                TransmissionRetryDelayDelegate                                    TransmissionRetryDelay                                   = null,
                                                Byte?                                                             MaxNumberOfRetries                                       = eMIPv0_7_4.CPO.CPOClient.DefaultMaxNumberOfRetries,

                                                HTTPPath?                                                         ServerURLPrefix                                          = null,
                                                String                                                            ServiceId                                                = null,

                                                String                                                            ClientLoggingContext                                     = eMIPv0_7_4.CPO.CPOClient.CPOClientLogger.DefaultContext,
                                                String                                                            ServerLoggingContext                                     = eMIPv0_7_4.CPO.CPOServerLogger.DefaultContext,
                                                LogfileCreatorDelegate                                            LogfileCreator                                           = null,

                                                eMIPv0_7_4.CPO.WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2eMIPChargeDetailRecord            = null,

                                                IncludeEVSEIdDelegate                                             IncludeEVSEIds                                           = null,
                                                IncludeEVSEDelegate                                               IncludeEVSEs                                             = null,
                                                IncludeChargingStationIdDelegate                                  IncludeChargingStationIds                                = null,
                                                IncludeChargingStationDelegate                                    IncludeChargingStations                                  = null,
                                                IncludeChargingPoolIdDelegate                                     IncludeChargingPoolIds                                   = null,
                                                IncludeChargingPoolDelegate                                       IncludeChargingPools                                     = null,
                                                ChargeDetailRecordFilterDelegate                                  ChargeDetailRecordFilter                                 = null,
                                                CustomOperatorIdMapperDelegate                                    CustomOperatorIdMapper                                   = null,
                                                CustomEVSEIdMapperDelegate                                        CustomEVSEIdMapper                                       = null,

                                                TimeSpan?                                                         SendHeartbeatsEvery                                      = null,
                                                TimeSpan?                                                         ServiceCheckEvery                                        = null,
                                                TimeSpan?                                                         StatusCheckEvery                                         = null,
                                                TimeSpan?                                                         CDRCheckEvery                                            = null,

                                                Boolean                                                           DisableSendHeartbeats                                    = false,
                                                Boolean                                                           DisablePushData                                          = false,
                                                Boolean                                                           DisablePushStatus                                        = false,
                                                Boolean                                                           DisableAuthentication                                    = false,
                                                Boolean                                                           DisableSendChargeDetailRecords                           = false,

                                                Action<eMIPv0_7_4.CPO.WWCPCPOAdapter>                             eMIPConfigurator                                         = null,
                                                Action<IEMPRoamingProvider>                                       Configurator                                             = null,

                                                String                                                            EllipticCurve                                            = "P-256",
                                                ECPrivateKeyParameters                                            PrivateKey                                               = null,
                                                PublicKeyCertificates                                             PublicKeyCertificates                                    = null,

                                                CounterValues?                                                    CPOClientSendHeartbeatCounter                            = null,
                                                CounterValues?                                                    CPOClientSetChargingPoolAvailabilityStatusCounter        = null,
                                                CounterValues?                                                    CPOClientSetChargingStationAvailabilityStatusCounter     = null,
                                                CounterValues?                                                    CPOClientSetEVSEAvailabilityStatusCounter                = null,
                                                CounterValues?                                                    CPOClientSetChargingConnectorAvailabilityStatusCounter   = null,
                                                CounterValues?                                                    CPOClientSetEVSEBusyStatusCounter                        = null,
                                                CounterValues?                                                    CPOClientSetEVSESyntheticStatusCounter                   = null,
                                                CounterValues?                                                    CPOClientGetServiceAuthorisationCounter                  = null,
                                                CounterValues?                                                    CPOClientSetSessionEventReportCounter                    = null,
                                                CounterValues?                                                    CPOClientSetChargeDetailRecordCounter                    = null,

                                                DNSClient                                                         DNSClient                                                = null)

        {

            #region Initial checks

            if (SOAPServer == null)
                throw new ArgumentNullException(nameof(SOAPServer),      "The given SOAP/HTTP server must not be null!");


            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (Id == null)
                throw new ArgumentNullException(nameof(Id),              "The given unique roaming provider identification must not be null!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

            if (RemoteHostname == null)
                throw new ArgumentNullException(nameof(RemoteHostname),  "The given remote hostname must not be null!");

            #endregion

            var NewRoamingProvider = new eMIPv0_7_4.CPO.WWCPCPOAdapter(Id,
                                                                       Name,
                                                                       Description,
                                                                       RoamingNetwork,

                                                                       PartnerId,
                                                                       DefaultOperator,

                                                                       new eMIPv0_7_4.CPO.CPOClient(Id.ToString(),
                                                                                                    RemoteHostname,
                                                                                                    RemoteTCPPort,
                                                                                                    RemoteCertificateValidator,
                                                                                                    ClientCertificateSelector,
                                                                                                    RemoteHTTPVirtualHost,
                                                                                                    URLPrefix ?? eMIPv0_7_4.CPO.CPOClient.DefaultURLPrefix,
                                                                                                    HTTPUserAgent,
                                                                                                    RequestTimeout,
                                                                                                    TransmissionRetryDelay,
                                                                                                    MaxNumberOfRetries,

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

                                                                                                    DNSClient,
                                                                                                    ClientLoggingContext,
                                                                                                    LogfileCreator),

                                                                       new eMIPv0_7_4.CPO.CPOServer(SOAPServer,
                                                                                                    ServiceId,
                                                                                                    ServerURLPrefix ?? eMIPv0_7_4.CPO.CPOServer.DefaultURLPrefix),

                                                                       ServerLoggingContext,
                                                                       LogfileCreator,

                                                                       IncludeEVSEIds,
                                                                       IncludeEVSEs,
                                                                       IncludeChargingStationIds,
                                                                       IncludeChargingStations,
                                                                       IncludeChargingPoolIds,
                                                                       IncludeChargingPools,
                                                                       ChargeDetailRecordFilter,
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

                                                                       DNSClient);

            eMIPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator) as eMIPv0_7_4.CPO.WWCPCPOAdapter;

        }

        #endregion

    }

}
