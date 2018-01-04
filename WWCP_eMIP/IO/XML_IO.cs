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
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.eMIPv0_7_4
{

    /// <summary>
    /// OCHP XML I/O.
    /// </summary>
    public static partial class XML_IO
    {

        //#region AsEVSEAvailabilityStatusTypes(Text)

        //public static EVSEAvailabilityStatusTypes AsEVSEAvailabilityStatusTypes(Byte Number)
        //    => AsEVSEAvailabilityStatusTypes(Number.ToString());

        //public static EVSEAvailabilityStatusTypes AsEVSEAvailabilityStatusTypes(String Text)
        //{

        //    switch (Text)
        //    {

        //        case "1":
        //            return EVSEAvailabilityStatusTypes.OutOfOrder;

        //        case "2":
        //            return EVSEAvailabilityStatusTypes.InService;

        //        case "3":
        //            return EVSEAvailabilityStatusTypes.Future;

        //        case "4":
        //            return EVSEAvailabilityStatusTypes.Deleted;

        //        default:
        //            return EVSEAvailabilityStatusTypes.Unspecified;

        //    }

        //}

        //#endregion

        //#region AsNumber(this DeltaType)

        //public static Byte AsNumber(this EVSEAvailabilityStatusTypes AvailabilityStatus)
        //{

        //    switch (AvailabilityStatus)
        //    {

        //        case EVSEAvailabilityStatusTypes.OutOfOrder:
        //            return 1;

        //        case EVSEAvailabilityStatusTypes.InService:
        //            return 2;

        //        case EVSEAvailabilityStatusTypes.Future:
        //            return 3;

        //        case EVSEAvailabilityStatusTypes.Deleted:
        //            return 4;

        //        default:
        //            return 0;

        //    }

        //}

        //#endregion

    }

}
