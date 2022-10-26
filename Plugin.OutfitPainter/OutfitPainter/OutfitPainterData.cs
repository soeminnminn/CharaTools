using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CharaTools.AIChara;
using MessagePack;

namespace OutfitPainter
{
    [Serializable]
    [MessagePackObject]
    public class OutfitPainterData
    {
        [Key(0)]
        public List<OutfitPainterChannel> Channels { get; set; }

        [IgnoreMember]
        public List<OutfitPainterChannel> ActiveChannels
        {
            get
            {
                return Channels.Where(c => c.Assignments.Count > 0).ToList();
            }
        }

        public OutfitPainterData()
        {
            Channels = new List<OutfitPainterChannel>();
            Channels.Clear();

            for (int i = 1; i <= 16; i++)
            {
                Channels.Add(new OutfitPainterChannel(i, Color.black, 0, 0));
            }            
        }

        public void SetDefault()
        {

            FindChannelById(1).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.TOP, -1, 1, false));
            FindChannelById(2).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.TOP, -1, 2, false));
            FindChannelById(3).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.TOP, -1, 3, false));

            FindChannelById(1).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.BOT, -1, 1, false));
            FindChannelById(2).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.BOT, -1, 2, false));
            FindChannelById(3).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.BOT, -1, 3, false));

            FindChannelById(1).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.INNER_TOP, -1, 1, false));
            FindChannelById(2).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.INNER_TOP, -1, 2, false));
            FindChannelById(3).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.INNER_TOP, -1, 3, false));

            FindChannelById(1).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.INNER_BOT, -1, 1, false));
            FindChannelById(2).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.INNER_BOT, -1, 2, false));
            FindChannelById(3).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.INNER_BOT, -1, 3, false));

            FindChannelById(1).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.GLOVE, -1, 1, false));
            FindChannelById(2).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.GLOVE, -1, 2, false));
            FindChannelById(3).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.GLOVE, -1, 3, false));

            FindChannelById(1).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.PANTYHOSE, -1, 1, false));
            FindChannelById(2).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.PANTYHOSE, -1, 2, false));
            FindChannelById(3).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.PANTYHOSE, -1, 3, false));

            FindChannelById(1).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.SOCK, -1, 1, false));
            FindChannelById(2).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.SOCK, -1, 2, false));
            FindChannelById(3).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.SOCK, -1, 3, false));

            FindChannelById(1).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.SHOE, -1, 1, false));
            FindChannelById(2).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.SHOE, -1, 2, false));
            FindChannelById(3).Assignments.Add(new OutfitPainterChannelAssignment(OutfitPainterSlot.SHOE, -1, 3, false));
        }

        public OutfitPainterChannel FindChannelById(int id)
        {
            return Channels.First(c => c.ChannelId == id);
        }

        public static OutfitPainterSlot SlotForClothesKind(int clothesKind)
        {
            switch (clothesKind)
            {
                case 0:
                    return OutfitPainterSlot.TOP;
                case 1:
                    return OutfitPainterSlot.BOT;
                case 2:
                    return OutfitPainterSlot.INNER_TOP;
                case 3:
                    return OutfitPainterSlot.INNER_BOT;
                case 4:
                    return OutfitPainterSlot.GLOVE;
                case 5:
                    return OutfitPainterSlot.PANTYHOSE;
                case 6:
                    return OutfitPainterSlot.SOCK;
                case 7:
                    return OutfitPainterSlot.SHOE;
            }
            return OutfitPainterSlot.ACCESSORY;

        }
    }
}
