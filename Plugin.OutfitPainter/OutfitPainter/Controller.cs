using System;
using CharaTools.AIChara;
using MessagePack;

namespace OutfitPainter
{
    public class Controller
    {
        public const string GUID = "orange.spork.outfitpainter";
        public const string PluginName = "Outfit Painter";
        public const string Version = "1.0.1";
        public const string ExtSaveKey = "OutfitPainterData";

        public static OutfitPainterData LoadData(ExtendedPlugin file)
        {
            var data = file.GetExtendedDataById(GUID);
            return LoadData(data);
        }

        public static OutfitPainterData LoadData(PluginData data)
        {
            OutfitPainterData painterData;
            if (data != null)
            {
                byte[] outfitPainterBytes = (byte[])data.data[ExtSaveKey];
                if (outfitPainterBytes != null)
                {
                    painterData = MessagePackSerializer.Deserialize<OutfitPainterData>(outfitPainterBytes);
                }
                else
                {
                    painterData = new OutfitPainterData();
                    // Data.SetDefault();
                }

            }
            else
            {
                painterData = new OutfitPainterData();
                //
                //
                //Data.SetDefault();
            }

            return painterData;
        }

        public static PluginData SaveData(ExtendedPlugin file, OutfitPainterData painterData, bool addToFile = true)
        {
            var data = new PluginData();
            data.data[ExtSaveKey] = MessagePackSerializer.Serialize(painterData);
            
            if (addToFile)
                file.SetExtendedDataById(GUID, data);

            return data;
        }
    }
}
