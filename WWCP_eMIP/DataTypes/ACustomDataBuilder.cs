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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// The builder of custom data holding objects.
    /// </summary>
    public abstract class ACustomDataBuilder : ACustomData,
                                               ICustomDataBuilder
    {

        #region Constructor(s)

        /// <summary>
        /// Create a new custom data builder.
        /// </summary>
        /// <param name="CustomData">Custom data to add.</param>
        protected ACustomDataBuilder(IReadOnlyDictionary<String, Object>        CustomData  = null)
            : base(CustomData)
        { }

        /// <summary>
        /// Create a new custom data builder.
        /// </summary>
        /// <param name="CustomData">Custom data to add.</param>
        protected ACustomDataBuilder(IDictionary<String, Object>                CustomData  = null)
            : base(CustomData)
        { }

        /// <summary>
        /// Create a new custom data builder.
        /// </summary>
        /// <param name="CustomData">Custom data to add.</param>
        protected ACustomDataBuilder(IEnumerable<KeyValuePair<String, Object>>  CustomData  = null)
            : base(CustomData)
        { }

        #endregion


        /// <summary>
        /// Set the given custom data.
        /// </summary>
        /// <param name="Key">The key of the custom data.</param>
        /// <param name="Value">The value of the custom data.</param>
        public void SetCustomData(String Key,
                                  Object Value)
        {

            if (Key.IsNullOrEmpty())
                return;

            if (CustomData == null)
                CustomData = new Dictionary<String, Object>();

            if (!CustomData.ContainsKey(Key))
                CustomData.Add(Key, Value);

            else
                CustomData[Key] = Value;

        }


        /// <summary>
        /// Remove the given custom data key.
        /// </summary>
        /// <param name="Key">The key of the custom data.</param>
        public Object RemoveCustomData(String Key)
        {

            if (CustomData != null     &&
                Key.IsNotNullOrEmpty() &&
                CustomData.TryGetValue(Key, out Object Value))
            {

                CustomData.Remove(Key);

                return Value;

            }

            return null;

        }


        /// <summary>
        /// Remove the given custom data.
        /// </summary>
        /// <param name="Key">The key of the custom data.</param>
        /// <param name="Value">The value of the custom data.</param>
        public Object RemoveCustomData(String Key, Object Value)
        {

            if (CustomData != null     &&
                Key.IsNotNullOrEmpty() &&
                CustomData.TryGetValue(Key, out Object _Value) &&
                Value.Equals(_Value))
            {

                CustomData.Remove(Key);

                return _Value;

            }

            return null;

        }

    }

}
