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
using System.Linq;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4.EMP
{

    /// <summary>
    /// An eMIP EMP HTTP/SOAP/XML Server API.
    /// </summary>
    public class EMPServer : ASOAPServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public new const           String           DefaultHTTPServerName      = "GraphDefined eMIP " + Version.Number + " HTTP/SOAP/XML EMP API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort      = IPPort.Parse(2002);

        /// <summary>
        /// The default HTTP/SOAP/XML server URI prefix.
        /// </summary>
        public new static readonly HTTPPath         DefaultURIPrefix           = HTTPPath.Parse("/");

        /// <summary>
        /// The default HTTP/SOAP/XML URI for eMIP authorization requests.
        /// </summary>
        public     const           String           DefaultAuthorisationURI    = "";

        /// <summary>
        /// The default HTTP/SOAP/XML content type.
        /// </summary>
        public new static readonly HTTPContentType  DefaultContentType         = HTTPContentType.XMLTEXT_UTF8;

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public new static readonly TimeSpan         DefaultRequestTimeout      = TimeSpan.FromMinutes(1);

        #endregion

        #region Properties

        /// <summary>
        /// The identification of this HTTP/SOAP service.
        /// </summary>
        public String  ServiceId           { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for eMIP authorization requests.
        /// </summary>
        public String  AuthorisationURI    { get; }

        #endregion

        #region Custom request/response mappers

        public OnExceptionDelegate                                           OnException                                       { get; set; }

        #endregion

        #region Events


        #endregion

        #region Constructor(s)

        #region EMPServer(HTTPServerName, ServiceId = null, TCPPort = default, URIPrefix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the eMIP HTTP/SOAP/XML EMP API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="AuthorisationURI">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public EMPServer(String           HTTPServerName            = DefaultHTTPServerName,
                         String           ServiceId                 = null,
                         IPPort?          TCPPort                   = null,
                         HTTPPath?        URIPrefix                 = null,
                         String           AuthorisationURI          = DefaultAuthorisationURI,
                         HTTPContentType  ContentType               = null,
                         Boolean          RegisterHTTPRootService   = true,
                         DNSClient        DNSClient                 = null,
                         Boolean          AutoStart                 = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort     ?? DefaultHTTPServerPort,
                   URIPrefix   ?? DefaultURIPrefix,
                   ContentType ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            this.ServiceId         = ServiceId        ?? nameof(EMPServer);
            this.AuthorisationURI  = AuthorisationURI ?? DefaultAuthorisationURI;

            RegisterURITemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region EMPServer(SOAPServer, ServiceId = null, URIPrefix = default)

        /// <summary>
        /// Use the given SOAP server for the eMIP HTTP/SOAP/XML EMP API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="AuthorisationURI">The HTTP/SOAP/XML URI for eMIP authorization requests.</param>
        public EMPServer(SOAPServer  SOAPServer,
                         String      ServiceId         = null,
                         HTTPPath?   URIPrefix         = null,
                         String      AuthorisationURI  = DefaultAuthorisationURI)

            : base(SOAPServer,
                   URIPrefix ?? DefaultURIPrefix)

        {

            this.ServiceId         = ServiceId        ?? nameof(EMPServer);
            this.AuthorisationURI  = AuthorisationURI ?? DefaultAuthorisationURI;

            RegisterURITemplates();

        }

        #endregion

        #endregion


        #region (override) RegisterURITemplates()

        /// <summary>
        /// Register all URI templates for this SOAP API.
        /// </summary>
        protected void RegisterURITemplates()
        {



        }

        #endregion


    }

}
