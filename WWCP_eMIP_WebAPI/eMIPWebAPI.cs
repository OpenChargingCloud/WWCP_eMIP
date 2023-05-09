/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using cloud.charging.open.protocols.WWCP;
using cloud.charging.open.protocols.eMIPv0_7_4.EMP;
using cloud.charging.open.protocols.eMIPv0_7_4.CPO;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4.WebAPI
{

    /// <summary>
    /// eMIP+ HTTP API extention methods.
    /// </summary>
    public static class ExtensionMethods
    {

        #region ParseRoamingNetwork(this HTTPRequest, HTTPServer, out RoamingNetwork, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network
        /// for the given HTTP hostname and HTTP query parameter
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetwork(this HTTPRequest                             HTTPRequest,
                                                  HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer,
                                                  out RoamingNetwork                           RoamingNetwork,
                                                  out HTTPResponse                             HTTPResponse)
        {

            if (HTTPServer == null)
                Console.WriteLine("HTTPServer == null!");

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException("HTTPRequest",  "The given HTTP request must not be null!");

            if (HTTPServer == null)
                throw new ArgumentNullException("HTTPServer",   "The given HTTP server must not be null!");

            #endregion

            RoamingNetwork_Id RoamingNetworkId;
                              RoamingNetwork    = null;
                              HTTPResponse      = null;

            if (HTTPRequest.ParsedURLParameters.Length < 1)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                };

                return false;

            }

            if (!RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURLParameters[0], out RoamingNetworkId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid RoamingNetworkId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            RoamingNetwork  = HTTPServer.
                                  GetAllTenants(HTTPRequest.Host).
                                  FirstOrDefault(roamingnetwork => roamingnetwork.Id == RoamingNetworkId);

            if (RoamingNetwork == null) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown RoamingNetworkId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            return true;

        }

        #endregion

    }


    /// <summary>
    /// A HTTP API providing eMIP+ data structures.
    /// </summary>
    public class eMIPWebAPI
    {

        #region Data

        /// <summary>
        /// The default HTTP URI prefix.
        /// </summary>
        public static readonly HTTPPath             DefaultURLPathPrefix        = HTTPPath.Parse("/ext/eMIPPlus");

        /// <summary>
        /// The default HTTP realm, if HTTP Basic Authentication is used.
        /// </summary>
        public const           String               DefaultHTTPRealm            = "Open Charging Cloud eMIP+ WebAPI";

        //ToDo: http://www.iana.org/form/media-types

        /// <summary>
        /// The HTTP content type for serving eMIP+ XML data.
        /// </summary>
        public static readonly HTTPContentType      eMIPPlusXMLContentType      = new HTTPContentType("application", "vnd.eMIPPlus+xml", "utf-8", null, null);

        /// <summary>
        /// The HTTP content type for serving eMIP+ HTML data.
        /// </summary>
        public static readonly HTTPContentType      eMIPPlusHTMLContentType     = new HTTPContentType("application", "vnd.eMIPPlus+html", "utf-8", null, null);


        private readonly XMLNamespacesDelegate      XMLNamespaces;
        private readonly XMLPostProcessingDelegate  XMLPostProcessing;

        public static readonly HTTPEventSource_Id   DebugLogId                  = HTTPEventSource_Id.Parse("eMIPDebugLog");

        #endregion

        #region Properties

        /// <summary>
        /// The HTTP server for serving the eMIP+ WebAPI.
        /// </summary>
        public HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer        { get; }

        /// <summary>
        /// The HTTP URI prefix.
        /// </summary>
        public HTTPPath                                     URLPathPrefix     { get; }

        /// <summary>
        /// The HTTP realm, if HTTP Basic Authentication is used.
        /// </summary>
        public String                                       HTTPRealm         { get; }

        /// <summary>
        /// An enumeration of logins for an optional HTTP Basic Authentication.
        /// </summary>
        public IEnumerable<KeyValuePair<String, String>>    HTTPLogins        { get; }


        public Boolean                                      DisableLogging    { get; }

        public String                                       LoggingPath       { get; }

        /// <summary>
        /// Send debug information via HTTP Server Sent Events.
        /// </summary>
        public HTTPEventSource<JObject>                     DebugLog          { get; }


        /// <summary>
        /// The DNS client to use.
        /// </summary>
        public DNSClient                                    DNSClient         { get; }


        private readonly List<WWCPCPOAdapter> _CPOAdapters;

        public IEnumerable<WWCPCPOAdapter> CPOAdapters
            => _CPOAdapters;


        protected readonly CustomEVSEIdMapperDelegate      CustomEVSEIdMapper;

        protected readonly CustomOperatorIdMapperDelegate  CustomOperatorIdMapper;

        #endregion

        #region Events

        #region Generic HTTP/SOAP server logging

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog    = new ();

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog   = new ();

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog      = new ();

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Attach the eMIP+ WebAPI to the given HTTP server.
        /// </summary>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="URLPathPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="HTTPRealm">The HTTP realm, if HTTP Basic Authentication is used.</param>
        /// <param name="HTTPLogins">An enumeration of logins for an optional HTTP Basic Authentication.</param>
        /// 
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        /// <param name="XMLPostProcessing">An optional delegate to process the XML after its final creation.</param>
        public eMIPWebAPI(HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer,
                          HTTPPath?                                    URLPathPrefix            = null,
                          String                                       HTTPRealm                = DefaultHTTPRealm,
                          IEnumerable<KeyValuePair<String, String>>    HTTPLogins               = null,

                          XMLNamespacesDelegate                        XMLNamespaces            = null,
                          XMLPostProcessingDelegate                    XMLPostProcessing        = null,

                          CustomOperatorIdMapperDelegate               CustomOperatorIdMapper   = null,
                          CustomEVSEIdMapperDelegate                   CustomEVSEIdMapper       = null,

                          Boolean?                                     DisableLogging           = false,
                          String?                                      LoggingPath              = null)

        {

            this.HTTPServer              = HTTPServer    ?? throw new ArgumentNullException(nameof(HTTPServer), "The given HTTP server must not be null!");
            this.URLPathPrefix           = URLPathPrefix ?? DefaultURLPathPrefix;
            this.HTTPRealm               = HTTPRealm.IsNotNullOrEmpty() ? HTTPRealm : DefaultHTTPRealm;
            this.HTTPLogins              = HTTPLogins    ?? new KeyValuePair<String, String>[0];
            this.DNSClient               = HTTPServer.DNSClient;

            this.XMLNamespaces           = XMLNamespaces;
            this.XMLPostProcessing       = XMLPostProcessing;

            this.CustomOperatorIdMapper  = CustomOperatorIdMapper;
            this.CustomEVSEIdMapper      = CustomEVSEIdMapper;

            this._CPOAdapters            = new List<WWCPCPOAdapter>();

            // Link HTTP events...
            HTTPServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            HTTPServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            HTTPServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);


            // Logging
            this.DisableLogging          = this.DisableLogging == false;
            this.LoggingPath             = LoggingPath    ?? Path.Combine(AppContext.BaseDirectory, "eMIPWebAPI");

            if (this.LoggingPath[^1] != Path.DirectorySeparatorChar)
                this.LoggingPath += Path.DirectorySeparatorChar;

            if (!this.DisableLogging)
                Directory.CreateDirectory(this.LoggingPath);

            this.DebugLog                = HTTPServer.AddJSONEventSource(EventIdentification:      DebugLogId,
                                                                         HTTPAPI:                  null,
                                                                         URLTemplate:              this.URLPathPrefix + "/" + DebugLogId.ToString(),
                                                                         MaxNumberOfCachedEvents:  10000,
                                                                         RetryIntervall:           TimeSpan.FromSeconds(5),
                                                                         EnableLogging:            true,
                                                                         LogfilePath:              this.LoggingPath);

            RegisterURITemplates();

        }

        #endregion


        #region (private) RegisterURITemplates()

        private void RegisterURITemplates()
        {

            #region / (HTTPRoot)

            HTTPServer.RegisterResourcesFolder(null,
                                               HTTPHostname.Any,
                                               URLPathPrefix,
                                               "cloud.charging.open.protocols.eMIPv0_7_4.WebAPI.HTTPRoot",
                                               DefaultFilename: "index.html");

            #endregion


            #region GET   ~/ResendAll

            // ========================================================================================
            // curl -v -X SET -H "Accept: application/json" \
            //      http://127.0.0.1:3004/RNs/Prod/IO/Gireve/WebAPI/ResendAll
            // ========================================================================================
            HTTPServer.AddMethodCallback(null,
                                         HTTPHostname.Any,
                                         HTTPMethod.SET,
                                         URLPathPrefix + "/ResendAll",
                                         //HTTPContentType.JSON_UTF8,
                                         //HTTPRequestLogger:  SendAuthStartEVSERequest,
                                         //HTTPResponseLogger: SendAuthStartEVSEResponse,
                                         HTTPDelegate: async Request => {

                                             var Adapter = _CPOAdapters.FirstOrDefault();
                                             if (Adapter != null)
                                             {

                                                 var AllEVSEs = Adapter.RoamingNetwork.EVSEs.Where(evse => Adapter.IncludeEVSEIds(evse.Id) && Adapter.IncludeEVSEs(evse)).ToArray();

                                                 foreach (var evse in AllEVSEs)
                                                 {

                                                     var evseId = evse.Id.ToEMIP();

                                                     if (evseId.HasValue)
                                                     {

                                                         await Adapter.CPORoaming.SetEVSEAvailabilityStatus(PartnerId:           Adapter.PartnerId,
                                                                                                            OperatorId:          evse.Id.OperatorId.ToEMIP(CustomOperatorIdMapper),
                                                                                                            EVSEId:              evseId.Value,
                                                                                                            StatusEventDate:     evse.AdminStatus.Timestamp,
                                                                                                            AvailabilityStatus:  evse.AdminStatus.Value.ToEMIP(),
                                                                                                            TransactionId:       Transaction_Id.Random());

                                                         await Adapter.CPORoaming.SetEVSEBusyStatus        (PartnerId:           Adapter.PartnerId,
                                                                                                            OperatorId:          evse.Id.OperatorId.ToEMIP(CustomOperatorIdMapper),
                                                                                                            EVSEId:              evseId.Value,
                                                                                                            StatusEventDate:     evse.Status.Timestamp,
                                                                                                            BusyStatus:          evse.Status.Value.ToEMIP(),
                                                                                                            TransactionId:       Transaction_Id.Random());

                                                     }

                                                 }

                                             }

                                             return new HTTPResponse.Builder(Request) {
                                                        HTTPStatusCode  = HTTPStatusCode.OK,
                                                        Connection      = "close"
                                                    };

                                   }, AllowReplacement: URLReplacement.Fail);

            #endregion

        }

        #endregion


        public void Add(WWCPCPOAdapter CPOAdapter)
        {

            _CPOAdapters.Add(CPOAdapter);

            #region OnGetServiceAuthorisationRequest/-Response

            CPOAdapter.CPOClient.OnGetServiceAuthorisationRequest += async  (LogTimestamp,
                                                                             RequestTimestamp,
                                                                             Sender,
                                                                             SenderId,
                                                                             EventTrackingId,
                                                                             PartnerId,
                                                                             OperatorId,
                                                                             EVSEId,
                                                                             UserId,
                                                                             ServiceId,
                                                                             TransactionId,
                                                                             PartnerServiceSessionId,
                                                                             RequestTimeout) => await DebugLog.SubmitEvent("GetServiceAuthorisationRequest",
                                                                                                                           JSONObject.Create(
                                                                                                                               new JProperty("timestamp",                      RequestTimestamp.    ToIso8601()),
                                                                                                                               new JProperty("eventTrackingId",                EventTrackingId.     ToString()),
                                                                                                                               //new JProperty("roamingNetworkId",               RoamingNetworkId.    ToString()),
                                                                                                                               //EMPRoamingProviderId.HasValue
                                                                                                                               //    ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                                                                                                                               //    : null,
                                                                                                                               //CSORoamingProviderId.HasValue
                                                                                                                               //    ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                                                                                                                               //    : null,
                                                                                                                               new JProperty("partnerId",                      PartnerId.           ToString()),
                                                                                                                               new JProperty("operatorId",                     OperatorId.          ToString()),
                                                                                                                               new JProperty("EVSEId",                         EVSEId.              ToString()),
                                                                                                                               new JProperty("userId",                         UserId.              ToString()),
                                                                                                                               new JProperty("serviceId",                      ServiceId.           ToString()),
                                                                                                                               TransactionId.HasValue
                                                                                                                                   ? new JProperty("transactionId",            TransactionId.       ToString())
                                                                                                                                   : null,
                                                                                                                               PartnerServiceSessionId.HasValue
                                                                                                                                   ? new JProperty("partnerServiceSessionId",  PartnerServiceSessionId.ToString())
                                                                                                                                   : null,
                                                                                                                               new JProperty("requestTimeout",                 Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                        ));

            CPOAdapter.CPOClient.OnGetServiceAuthorisationResponse += async (LogTimestamp,
                                                                             RequestTimestamp,
                                                                             Sender,
                                                                             SenderId,
                                                                             EventTrackingId,
                                                                             PartnerId,
                                                                             OperatorId,
                                                                             EVSEId,
                                                                             UserId,
                                                                             ServiceId,
                                                                             TransactionId,
                                                                             PartnerServiceSessionId,
                                                                             RequestTimeout,
                                                                             Result,
                                                                             Runtime) => await DebugLog.SubmitEvent("GetServiceAuthorisationResponse",
                                                                                                                    JSONObject.Create(
                                                                                                                        new JProperty("timestamp",                   RequestTimestamp.    ToIso8601()),
                                                                                                                        new JProperty("eventTrackingId",             EventTrackingId.     ToString()),
                                                                                                                        //new JProperty("roamingNetworkId",            RoamingNetwork.Id.   ToString()),
                                                                                                                        //EMPRoamingProviderId.HasValue
                                                                                                                        //    ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                                                                                                                        //    : null,
                                                                                                                        //CSORoamingProviderId.HasValue
                                                                                                                        //    ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                                                                                                                        //    : null,
                                                                                                                        new JProperty("partnerId",                      PartnerId.           ToString()),
                                                                                                                        new JProperty("operatorId",                     OperatorId.          ToString()),
                                                                                                                        new JProperty("EVSEId",                         EVSEId.              ToString()),
                                                                                                                        new JProperty("userId",                         UserId.              ToString()),
                                                                                                                        new JProperty("serviceId",                      ServiceId.           ToString()),
                                                                                                                        TransactionId.HasValue
                                                                                                                            ? new JProperty("transactionId",            TransactionId.       ToString())
                                                                                                                            : null,
                                                                                                                        PartnerServiceSessionId.HasValue
                                                                                                                            ? new JProperty("partnerServiceSessionId",  PartnerServiceSessionId.ToString())
                                                                                                                            : null,
                                                                                                                        new JProperty("requestTimeout",                 Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                        new JProperty("result",                         Result.              ToJSON()),
                                                                                                                        new JProperty("runtime",                        Math.Round(Runtime.TotalMilliseconds,   0))

                                                                                                                    ));

            #endregion

        }


    }

}
