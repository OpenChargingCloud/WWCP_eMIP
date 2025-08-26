/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Text;
using System.Xml.Linq;
using System.Security.Cryptography;

using SOAPNS = org.GraphDefined.Vanaheimr.Hermod.SOAP;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// eMIP SOAP helpers.
    /// </summary>
    public static class SOAP
    {

        /// <summary>
        /// Encapsulate the given XML within a XML SOAP frame.
        /// </summary>
        /// <param name="SOAPBody">The internal XML for the SOAP body.</param>
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        public static XElement Encapsulation(XElement                      SOAPBody,
                                             SOAPNS.XMLNamespacesDelegate  XMLNamespaces = null)
        {

            #region Initial checks

            if (SOAPBody is null)
                throw new ArgumentNullException(nameof(SOAPBody),  "The given XML must not be null!");

            if (XMLNamespaces is null)
                XMLNamespaces = xml => xml;

            #endregion

            return XMLNamespaces(
                new XElement(SOAPNS.v1_2.NS.SOAPEnvelope + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "SOAP",  SOAPNS.v1_2.NS.SOAPEnvelope.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "eMIP",  eMIPNS.Default.             NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "dyn",   eMIPNS.EVCIDynamic.         NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "auth",  eMIPNS.Authorisation.       NamespaceName),

                    new XElement(SOAPNS.v1_2.NS.SOAPEnvelope + "Header"),
                    new XElement(SOAPNS.v1_2.NS.SOAPEnvelope + "Body",  SOAPBody)
                )
            );

        }

    }

}
