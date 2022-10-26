using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessagePack;

namespace Plugin.OverlayMods
{
    /// <summary>
    /// A clothes overlay texture holder.
    /// </summary>
    [MessagePackObject]
    public class ClothesTexData
    {
        [IgnoreMember]
        private byte[] _textureBytes;

        [Key(0)]
        public byte[] TextureBytes
        {
            get => _textureBytes;
            set
            {
                _textureBytes = value;
            }
        }

        [Key(1)]
        public bool Override;
    }
}
