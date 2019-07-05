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
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// The common interface for all custom data holding objects.
    /// </summary>
    public interface ICustomData
    {

        Boolean HasCustomData   { get; }

        Boolean ContainsCustomData(String Key);
        Boolean ContainsCustomData(String Key, Object Value);

        void IfDefined     (String Key, Action<Object> ValueDelegate);
        void IfDefinedAs<T>(String Key, Action<T>      ValueDelegate);

        Object  GetCustomData        (String Key);
        T       GetCustomDataAs   <T>(String Key);
        Boolean TryGetCustomData     (String Key, out Object Value);
        Boolean TryGetCustomDataAs<T>(String Key, out T      Value);

        T       MapCustomData     <T>(String Key, Func<Object, T> ValueDelegate);

    }

}