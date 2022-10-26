using System;
using System.Collections.Generic;
using CharaTools.AIChara;
using MessagePack;

namespace OutfitPainter
{
    [Serializable]
    [MessagePackObject]
    public class OutfitPainterChannel
    {
        [Key(0)]
        public int ChannelId { get; set; }

        [Key(1)]
        public string ChannelDescription { get; set; }

        [Key(2)]
        public Color ChannelColor { get; set; }

        [Key(3)]
        public float ChannelGloss { get; set; }

        [Key(4)]
        public float ChannelMetallic { get; set; }

        [Key(5)]
        public List<OutfitPainterChannelAssignment> Assignments { get; set; } = new List<OutfitPainterChannelAssignment>();

        [IgnoreMember]
        public Color ChannelDisplayColor
        {
            get
            {
                return new Color(ChannelColor.r, ChannelColor.g, ChannelColor.b, 1.0f);
            }
        }


        [IgnoreMember]
        public Color InverseChannelColor
        {
            get
            {
                float r = 1.0f - ChannelColor.r;
                float g = 1.0f - ChannelColor.g;
                float b = 1.0f - ChannelColor.b;

                if ((r + g + b) / 3.0f > .5f)
                    return Color.Lerp(Color.white, new Color(r, g, b, 1.0f), .5f);
                else
                    return Color.Lerp(Color.black, new Color(r, g, b, 1.0f), .5f);
            }
        }

        public OutfitPainterChannel()
        {

        }

        public OutfitPainterChannel(int id, Color color, float gloss, float metallic, string description = null)
        {
            ChannelId = id;
            ChannelColor = color;
            ChannelGloss = gloss;
            ChannelMetallic = metallic;
            ChannelDescription = description == null ? id.ToString() : description;
        }
    }
}
