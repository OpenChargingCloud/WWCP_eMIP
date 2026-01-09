/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Xml.Linq;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// eMIP XML Namespaces
    /// </summary>
    public static class eMIPNS
    {

        /// <summary>
        /// The default namespace of the eMobility Protocol Inter-Operation (eMIP) Version 0.7.4.
        /// </summary>
        public static readonly XNamespace Default        = "https://api-iop.gireve.com/schemas/PlatformV1/";

        /// <summary>
        /// The EVCIDynamic namespace of the eMobility Protocol Inter-Operation (eMIP) Version 0.7.4.
        /// </summary>
        public static readonly XNamespace EVCIDynamic    = "https://api-iop.gireve.com/schemas/EVCIDynamicV1/";

        /// <summary>
        /// The Authorisation namespace of the eMobility Protocol Inter-Operation (eMIP) Version 0.7.4.
        /// </summary>
        public static readonly XNamespace Authorisation  = "https://api-iop.gireve.com/schemas/AuthorisationV1/";

    }

}
