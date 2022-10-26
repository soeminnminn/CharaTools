using System;
using System.Collections.Generic;
using System.Linq;
using MessagePack;
using CharaTools.AIChara;

namespace AdditionalAccessoryControls
{
    public class Controller
    {
        public const string GUID = "orange.spork.additionalaccessorycontrolsplugin";
        public const string PluginName = "Additional Accessory Controls";
        public const string Version = "1.2.6";

        public const string accessoryDataKey = "accessoryData";
        public const string overrideDataKey = "overrideData";

        private AdditionalAccessorySlotData[] slotData = null;  // Mirrors character accessory data
        public AdditionalAccessoryCoordinateData CoordinateOverrideData { get; set; } = null;

        public AdditionalAccessorySlotData[] SlotData // Always enable exhibitionism
        {
            get => slotData;
        }

        public void LoadFromCoordinate(ExtendedPlugin coordinate)
        {
            // Load Coordinate Extension Data
            AdditionalAccessorySlotData[] coordinateSlotData = null;

            PluginData coordinateData = coordinate.GetExtendedDataById(GUID);
            if (coordinateData != null)
            {
                // Grab and deserialize
                object accessoryData = null;
                if (!coordinateData.data.TryGetValue("coordinateAccessoryData", out accessoryData))
                    coordinateData.data.TryGetValue("accessoryData", out accessoryData);

                if (accessoryData != null)
                {
                    byte[] coordinateAccessorySlotBinary = accessoryData as byte[];
                    if (coordinateAccessorySlotBinary != null && coordinateAccessorySlotBinary.Length > 0)
                    {
                        coordinateSlotData = MessagePackSerializer.Deserialize<AdditionalAccessorySlotData[]>(coordinateAccessorySlotBinary);
                    }
                }

                object overrideData = null;
                if (!coordinateData.data.TryGetValue("coordinateOverrideData", out overrideData))
                    coordinateData.data.TryGetValue("overrideData", out overrideData);

                byte[] coordinateOverrideBinary = overrideData as byte[];
                if (coordinateOverrideBinary != null && coordinateOverrideBinary.Length > 0)
                {
                    CoordinateOverrideData = MessagePackSerializer.Deserialize<AdditionalAccessoryCoordinateData>(coordinateOverrideBinary);
                }
                else
                {
                    CoordinateOverrideData = new AdditionalAccessoryCoordinateData();
                }
            }

            slotData = coordinateSlotData;
        }

        public void SaveToCoordinate(ExtendedPlugin coordinate)
        {
            var data = new PluginData();
            data.data["accessoryData"] = MessagePackSerializer.Serialize(slotData);
            data.data["overrideData"] = MessagePackSerializer.Serialize(CoordinateOverrideData);
            coordinate.SetExtendedDataById(GUID, data);
        }
    }
}
