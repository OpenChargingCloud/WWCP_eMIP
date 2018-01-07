/*
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

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// The common interface for all builders of custom data holding objects.
    /// </summary>
    public interface ICustomDataBuilder : ICustomData
    {

        /// <summary>
        /// Set the given custom data.
        /// </summary>
        /// <param name="Key">The key of the custom data.</param>
        /// <param name="Value">The value of the custom data.</param>
        void SetCustomData(String Key, Object Value);

        /// <summary>
        /// Remove the given custom data key.
        /// </summary>
        /// <param name="Key">The key of the custom data.</param>
        Object RemoveCustomData(String Key);

        /// <summary>
        /// Remove the given custom data.
        /// </summary>
        /// <param name="Key">The key of the custom data.</param>
        /// <param name="Value">The value of the custom data.</param>
        Object RemoveCustomData(String Key, Object Value);

    }

}