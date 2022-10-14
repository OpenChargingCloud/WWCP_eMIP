/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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
//using WWCP = cloud.charging.open.protocols.WWCP;
using cloud.charging.open.protocols.WWCP;

#endregion

namespace cloud.charging.open.protocols.eMIPv0_7_4
{

    /// <summary>
    /// Helper methods to map eMIP data type values to
    /// WWCP data type values and vice versa.
    /// </summary>
    public static class eMIPMapper
    {

        #region EVSEIds

        /// <summary>
        /// Convert a WWCP EVSE identification into an eMIP EVSE identification.
        /// </summary>
        /// <param name="EVSEId">A WWCP EVSE identification.</param>
        /// <param name="CustomEVSEIdMapper">A delegate to customize the mapping of EVSE identifications.</param>
        public static EVSE_Id? ToEMIP(this WWCP.EVSE_Id            EVSEId,
                                      CustomEVSEIdMapperDelegate?  CustomEVSEIdMapper  = null)
        {

            if (EVSE_Id.TryParse(CustomEVSEIdMapper is not null
                                     ? CustomEVSEIdMapper(EVSEId.ToString())
                                     : EVSEId.ToString(),
                                 out EVSE_Id eMIPEVSEId))
            {
                return eMIPEVSEId;
            }

            return null;

        }

        /// <summary>
        /// Convert an eMIP EVSE identification into a WWCP EVSE identification.
        /// </summary>
        /// <param name="EVSEId">An eMIP EVSE identification.</param>
        public static WWCP.EVSE_Id? ToWWCP(this EVSE_Id EVSEId)
        {

            if (WWCP.EVSE_Id.TryParse(EVSEId.ToString(), out WWCP.EVSE_Id WWCPEVSEId))
                return WWCPEVSEId;

            return null;

        }

        /// <summary>
        /// Convert an eMIP EVSE identification into a WWCP EVSE identification.
        /// </summary>
        /// <param name="EVSEId">An eMIP EVSE identification.</param>
        public static WWCP.EVSE_Id? ToWWCP(this EVSE_Id? EVSEId)
        {

            if (!EVSEId.HasValue)
                return null;

            if (WWCP.EVSE_Id.TryParse(EVSEId.ToString(), out WWCP.EVSE_Id WWCPEVSEId))
                return WWCPEVSEId;

            return null;

        }

        #endregion

        #region EVSEStatusTypes / EVSEBusyStatusTypes

        /// <summary>
        /// Convert a WWCP EVSE status into an eMIP EVSE busy status.
        /// </summary>
        /// <param name="EVSEStatusType"></param>
        /// <returns></returns>
        public static EVSEBusyStatusTypes ToEMIP(this EVSEStatusTypes EVSEStatusType)
        {

            switch (EVSEStatusType)
            {

                case EVSEStatusTypes.Available:
                    return EVSEBusyStatusTypes.Free;

                case EVSEStatusTypes.Reserved:
                    return EVSEBusyStatusTypes.Reserved;

                // case EVSEStatusTypes.Charging:
                // case EVSEStatusTypes.DoorNotClosed:
                // case EVSEStatusTypes.Faulted:
                // case EVSEStatusTypes.Offline:
                // case EVSEStatusTypes.OutOfService:
                // case EVSEStatusTypes.PluggedIn:
                // case EVSEStatusTypes.WaitingForPlugin:

                default:
                    return EVSEBusyStatusTypes.Busy;

            }

        }

        #endregion

        #region EVSEAdminStatusTypes / EVSEAvailabilityStatusTypes

        /// <summary>
        /// Convert a WWCP EVSE status into an eMIP EVSE availability status.
        /// </summary>
        /// <param name="EVSEAdminStatusType"></param>
        /// <returns></returns>
        public static EVSEAvailabilityStatusTypes ToEMIP(this EVSEAdminStatusTypes EVSEAdminStatusType)
        {

            switch (EVSEAdminStatusType)
            {

                case EVSEAdminStatusTypes.OutOfService:
                case EVSEAdminStatusTypes.Blocked:
                case EVSEAdminStatusTypes.InternalUse:
                    return EVSEAvailabilityStatusTypes.OutOfOrder;

                case EVSEAdminStatusTypes.Operational:
                    return EVSEAvailabilityStatusTypes.InService;

                case EVSEAdminStatusTypes.Planned:
                case EVSEAdminStatusTypes.InDeployment:
                    return EVSEAvailabilityStatusTypes.Future;

                case EVSEAdminStatusTypes.Deleted:
                    return EVSEAvailabilityStatusTypes.Deleted;

                default:
                    return EVSEAvailabilityStatusTypes.OutOfOrder;

            }

        }

        #endregion



        public static PartnerProduct_Id? ToEMIP(this ChargingProduct_Id ProductId)
            => PartnerProduct_Id.Parse(ProductId.ToString());

        public static ChargingProduct_Id ToWWCP(this PartnerProduct_Id ProductId)
            => ChargingProduct_Id.Parse(ProductId.ToString());


        public static Operator_Id ToEMIP(this ChargingStationOperator_Id  OperatorId,
                                         CustomOperatorIdMapperDelegate   CustomOperatorIdMapper = null)

            => Operator_Id.Parse(CustomOperatorIdMapper != null
                                     ? CustomOperatorIdMapper(OperatorId.ToString())
                                     : OperatorId.ToString());

        public static ChargingStationOperator_Id? ToWWCP(this Operator_Id OperatorId)
        {

            if (ChargingStationOperator_Id.TryParse(OperatorId.ToString(), out ChargingStationOperator_Id ChargingStationOperatorId))
                return ChargingStationOperatorId;

            return null;

        }


        public static ServiceSession_Id ToEMIP(this ChargingSession_Id ChargingSessionId)
            => ServiceSession_Id.Parse(ChargingSessionId.ToString());

        public static ServiceSession_Id? ToEMIP(this ChargingSession_Id? ChargingSessionId)
            => ChargingSessionId.HasValue
                   ? ServiceSession_Id.Parse(ChargingSessionId.ToString())
                   : new ServiceSession_Id?();

        public static ChargingSession_Id ToWWCP(this ServiceSession_Id ServiceSessionId)
            => ChargingSession_Id.Parse(ServiceSessionId.ToString());

        public static ChargingSession_Id? ToWWCP(this ServiceSession_Id? ServiceSessionId)
            => ServiceSessionId.HasValue
                   ? ChargingSession_Id.Parse(ServiceSessionId.ToString())
                   : new ChargingSession_Id?();


        public static eMobilityProvider_Id ToWWCP_ProviderId(this Partner_Id PartnerId)
            => eMobilityProvider_Id.Parse(PartnerId.ToString());

        public static RemoteAuthentication ToWWCP(this User_Id UserId)
        {

            if (UserId.Format == UserIdFormats.eMA  ||
                UserId.Format == UserIdFormats.eMI3 ||
                UserId.Format == UserIdFormats.EVCO ||
                UserId.Format == UserIdFormats.EMP_SPEC)
            {
                return RemoteAuthentication.FromRemoteIdentification(eMobilityAccount_Id.Parse(UserId.ToString()));
            }

            if (UserId.Format == UserIdFormats.RFID_UID)
            {
                return RemoteAuthentication.FromAuthToken(Auth_Token.Parse(UserId.ToString()));
            }

            return null;

        }


        public static Provider_Id ToEMIP(this eMobilityProvider_Id ProviderId)
            => Provider_Id.Parse(ProviderId.ToString());

        public static eMobilityProvider_Id ToWWCP(this Provider_Id ProviderId)
            => eMobilityProvider_Id.Parse(ProviderId.ToString());


        public static Provider_Id? ToEMIP(this eMobilityProvider_Id? ProviderId)
            => ProviderId.HasValue
                   ? Provider_Id.Parse(ProviderId.ToString())
                   : new Provider_Id?();

        public static eMobilityProvider_Id? ToWWCP(this Provider_Id? ProviderId)
            => ProviderId.HasValue
                   ? eMobilityProvider_Id.Parse(ProviderId.ToString())
                   : new eMobilityProvider_Id?();


        public static User_Id? ToEMIP(this AAuthentication Authentication)
        {

            if (Authentication.AuthToken.HasValue)
                return new User_Id(Authentication.ToString(),
                                   UserIdFormats.RFID_UID);

            // Might be a DIN or ISO!!!
            if (Authentication.RemoteIdentification.HasValue)
                return new User_Id(Authentication.ToString(),
                                   UserIdFormats.eMA);

            return null;

        }



        #region ToEMIP(this ChargeDetailRecord, WWCPChargeDetailRecord2ChargeDetailRecord = null)

        public static String WWCP_CDR = "WWCP.CDR";

        /// <summary>
        /// Convert a WWCP charge detail record into a corresponding eMIP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">A WWCP charge detail record.</param>
        /// <param name="WWCPChargeDetailRecord2ChargeDetailRecord">A delegate which allows you to modify the convertion from WWCP charge detail records to eMIP charge detail records.</param>
        public static ChargeDetailRecord ToEMIP(this WWCP.ChargeDetailRecord                           ChargeDetailRecord,
                                                CPO.WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2ChargeDetailRecord = null)

        {

            var CDR  = new ChargeDetailRecord(
                           CDRNature:               CDRNatures.Final,
                           ServiceSessionId:        ServiceSession_Id.Parse(ChargeDetailRecord.SessionId.ToString()),
                           RequestedServiceId:      Service_Id.GenericChargeService,
                           EVSEId:                  ChargeDetailRecord.EVSEId.Value.ToEMIP().Value,
                           UserId:                  ChargeDetailRecord.AuthenticationStart.ToEMIP().Value,
                           StartTime:               ChargeDetailRecord.SessionTime.StartTime,
                           EndTime:                 ChargeDetailRecord.SessionTime.EndTime.Value,
                           UserContractIdAlias:     null,
                           ExecPartnerSessionId:    null,
                           ExecPartnerOperatorId:   null,
                           SalesPartnerSessionId:   null,//ChargeDetailRecord.GetCustomDataAs<PartnerSession_Id?>("eMIP.PartnerSessionId"),
                           SalesPartnerOperatorId:  null,
                           PartnerProductId:        ChargeDetailRecord.ChargingProduct?.Id.ToEMIP(),
                           MeterReports:            new MeterReport[] {
                                                        MeterReport.Create(ChargeDetailRecord.Duration.HasValue
                                                                               ?  ChargeDetailRecord.Duration.Value.TotalMinutes.ToString("0.##").Replace(',', '.')
                                                                               : (ChargeDetailRecord.EnergyMeteringValues.Last().Timestamp - ChargeDetailRecord.EnergyMeteringValues.First().Timestamp).TotalMinutes.ToString("0.##").Replace(',', '.'),
                                                                           "min",
                                                                           MeterTypes.TotalDuration),
                                                        MeterReport.Create(ChargeDetailRecord.ConsumedEnergy.HasValue
                                                                               ?  ChargeDetailRecord.ConsumedEnergy.Value.ToString("0.##").Replace(',', '.')
                                                                               : (ChargeDetailRecord.EnergyMeteringValues.Last().Value - ChargeDetailRecord.EnergyMeteringValues.First().Value).ToString("0.##").Replace(',', '.'),
                                                                           "kWh",
                                                                           MeterTypes.TotalEnergy),
                                                    },
                           InternalData:            new UserDefinedDictionary(
                                                        new Dictionary<String, Object?> {
                                                            { WWCP_CDR, ChargeDetailRecord }
                                                        }
                                                    ));

            if (WWCPChargeDetailRecord2ChargeDetailRecord != null)
                CDR = WWCPChargeDetailRecord2ChargeDetailRecord(ChargeDetailRecord, CDR);

            return CDR;

        }

        #endregion

        #region ToWWCP(this ChargeDetailRecord, ChargeDetailRecord2WWCPChargeDetailRecord = null)

        /// <summary>
        /// Convert an eMIP charge detail record into a corresponding WWCP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">An eMIP charge detail record.</param>
        /// <param name="ChargeDetailRecord2WWCPChargeDetailRecord">A delegate which allows you to modify the convertion from eMIP charge detail records to WWCP charge detail records.</param>
        public static WWCP.ChargeDetailRecord ToWWCP(this ChargeDetailRecord                                ChargeDetailRecord,
                                                     CPO.ChargeDetailRecord2WWCPChargeDetailRecordDelegate  ChargeDetailRecord2WWCPChargeDetailRecord = null)
        {

            //var CustomData = new Dictionary<String, Object>();

            //CustomData.Add("eMIP.CDR", ChargeDetailRecord);

            //if (ChargeDetailRecord.PartnerSessionId.HasValue)
            //    CustomData.Add("eMIP.PartnerSessionId",  ChargeDetailRecord.PartnerSessionId.ToString());

            //if (ChargeDetailRecord.HubOperatorId.HasValue)
            //    CustomData.Add("eMIP.HubOperatorId",     ChargeDetailRecord.HubOperatorId.   ToString());

            //if (ChargeDetailRecord.HubProviderId.HasValue)
            //    CustomData.Add("eMIP.HubProviderId",     ChargeDetailRecord.HubProviderId.   ToString());


            //var CDR = new  WWCP.ChargeDetailRecord(SessionId:             ChargeDetailRecord.SessionId.ToWWCP(),
            //                                       EVSEId:                ChargeDetailRecord.EVSEId.   ToWWCP(),

            //                                       ChargingProduct:       ChargeDetailRecord.PartnerProductId.HasValue
            //                                                                  ? new ChargingProduct(ChargeDetailRecord.PartnerProductId.Value.ToWWCP())
            //                                                                  : null,

            //                                       SessionTime:           new StartEndDateTime(ChargeDetailRecord.SessionStart,
            //                                                                                   ChargeDetailRecord.SessionEnd),

            //                                       IdentificationStart:   ChargeDetailRecord.Identification.ToWWCP(),

            //                                       EnergyMeteringValues:  (ChargeDetailRecord.ChargingStart.HasValue &&
            //                                                               ChargeDetailRecord.ChargingEnd  .HasValue)
            //                                                                  ? new Timestamped<Single>[] {

            //                                                                        new Timestamped<Single>(
            //                                                                            ChargeDetailRecord.ChargingStart.  Value,
            //                                                                            ChargeDetailRecord.MeterValueStart.Value
            //                                                                        ),

            //                                                                        //ToDo: Meter values in between... but we don't have timestamps for them!

            //                                                                        new Timestamped<Single>(
            //                                                                            ChargeDetailRecord.ChargingEnd.  Value,
            //                                                                            ChargeDetailRecord.MeterValueEnd.Value
            //                                                                        )

            //                                                                    }
            //                                                                  : new Timestamped<Single>[0],

            //                                       //ConsumedEnergy:      Will be calculated!

            //                                       MeteringSignature:     ChargeDetailRecord.MeteringSignature,

            //                                       CustomData:            CustomData

            //                                      );

            //if (ChargeDetailRecord2WWCPChargeDetailRecord != null)
            //    CDR = ChargeDetailRecord2WWCPChargeDetailRecord(ChargeDetailRecord, CDR);

            return null;

        }

        #endregion



        #region ToWWCP(this BookingId)

        public static ChargingReservation_Id? ToWWCP(this Booking_Id? BookingId)
        {

            if (!BookingId.HasValue)
                return null;

            if (ChargingReservation_Id.TryParse(BookingId.ToString(), out ChargingReservation_Id ChargingReservationId))
                return ChargingReservationId;

            throw new ArgumentException(nameof(BookingId), "The given eMIP booking identification could not be mapped to a WWCP charging reservation identification!");

        }

        #endregion

        #region ToEMIP(this ChargingReservationId)

        public static Booking_Id? ToEMIP(this ChargingReservation_Id? ChargingReservationId)
        {

            if (!ChargingReservationId.HasValue)
                return null;

            if (Booking_Id.TryParse(ChargingReservationId.ToString(), out Booking_Id BookingId))
                return BookingId;

            throw new ArgumentException(nameof(ChargingReservationId), "The given WWCP charging reservation identification could not be mapped to an eMIP booking identification!");

        }

        #endregion


    }

}
