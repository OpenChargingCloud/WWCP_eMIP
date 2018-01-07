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
using System.Runtime.Serialization;

#endregion

namespace org.GraphDefined.WWCP.eMIPv1_4
{

    /// <summary>
    /// A general eMIP exception.
    /// </summary>
    public class eMIPException : ApplicationException
    {

        /// <summary>
        /// Create a new eMIP exception.
        /// </summary>
        public eMIPException()
        { }


        /// <summary>
        /// Create a new eMIP exception.
        /// </summary>
        /// <param name="Info">The object that holds the serialized object data.</param>
        /// <param name="Context">The contextual information about the source or destination.</param>
        protected eMIPException(SerializationInfo  Info,
                                StreamingContext   Context)

            : base(Info,
                   Context)

        { }


        /// <summary>
        /// Create a new eMIP exception.
        /// </summary>
        /// <param name="Message">A message that describes the error.</param>
        public eMIPException(String Message)

            : base(Message)

        { }


        /// <summary>
        /// Create a new eMIP exception.
        /// </summary>
        /// <param name="Message">A message that describes the error.</param>
        /// <param name="InnerException">The exception that is the cause of the current exception.</param>
        public eMIPException(String Message, Exception InnerException)

            : base(Message,
                   InnerException)

        { }


    }

}
