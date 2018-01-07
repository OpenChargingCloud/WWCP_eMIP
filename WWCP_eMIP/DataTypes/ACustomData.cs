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

using org.GraphDefined.Vanaheimr.Illias;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// The abstract class for all custom data holding objects.
    /// </summary>
    public abstract class ACustomData : ICustomData
    {

        #region Data

        protected Dictionary<String, Object> CustomData;

        #endregion

        #region Constructor(s)

        protected ACustomData(IReadOnlyDictionary<String, Object> CustomData = null)
        {

            if (CustomData != null)
            {

                this.CustomData = new Dictionary<String, Object>();

                foreach (var item in CustomData)
                {

                    if (!this.CustomData.ContainsKey(item.Key))
                        this.CustomData.Add(item.Key, item.Value);

                    else
                        this.CustomData[item.Key] = item.Value;

                }

            }

        }

        protected ACustomData(IDictionary<String, Object> CustomData = null)
        {

            if (CustomData != null)
            {

                this.CustomData = new Dictionary<String, Object>();

                foreach (var item in CustomData)
                {

                    if (!this.CustomData.ContainsKey(item.Key))
                        this.CustomData.Add(item.Key, item.Value);

                    else
                        this.CustomData[item.Key] = item.Value;

                }

            }

        }

        protected ACustomData(IEnumerable<KeyValuePair<String, Object>> CustomData = null)
        {

            if (CustomData != null)
            {

                this.CustomData = new Dictionary<String, Object>();

                foreach (var item in CustomData)
                {

                    if (!this.CustomData.ContainsKey(item.Key))
                        this.CustomData.Add(item.Key, item.Value);

                    else
                        this.CustomData[item.Key] = item.Value;

                }

            }

        }

        #endregion


        public Boolean HasCustomData
        {

            get
            {

                if (CustomData == null)
                    return false;

                return CustomData.Count > 0;

            }

        }

        public Boolean IsDefined(String Key)
        {

            if (CustomData == null || Key.IsNullOrEmpty())
                return false;

            return CustomData.TryGetValue(Key, out Object _Value);

        }

        public Object GetCustomData(String Key)
        {

            if (CustomData != null   &&
                Key.IsNotNullOrEmpty() &&
                CustomData.TryGetValue(Key, out Object _Value))
            {
                return _Value;
            }

            return null;

        }

        public T GetCustomDataAs<T>(String Key)
        {

            try
            {

                if (CustomData != null     &&
                    Key.IsNotNullOrEmpty() &&
                    CustomData.TryGetValue(Key, out Object _Value))
                {
                    return (T) _Value;
                }

            }
            catch (Exception)
            { }

            return default(T);

        }


        public void IfDefined(String          Key,
                              Action<Object>  ValueDelegate)
        {

            if (CustomData    != null  &&
                ValueDelegate != null  &&
                Key.IsNotNullOrEmpty() &&
                CustomData.TryGetValue(Key, out Object _Value))
            {
                ValueDelegate(_Value);
            }

        }

        public void WhenDefinedAs<T>(String     Key,
                                     Action<T>  ValueDelegate)
        {

            try
            {

                if (CustomData    != null  &&
                    ValueDelegate != null  &&
                    Key.IsNotNullOrEmpty() &&
                    CustomData.TryGetValue(Key, out Object _Value))
                {
                    ValueDelegate((T) _Value);
                }

            }
            catch (Exception)
            { }

        }

    }

}
