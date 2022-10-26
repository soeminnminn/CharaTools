using System;
using System.Collections.Generic;
using MessagePack;

namespace CharaTools.AIChara
{
    [MessagePackObject(true)]
    public class BlockHeader
    {
        public List<Info> lstInfo { get; set; }

        public BlockHeader()
        {
            this.lstInfo = new List<Info>();
        }

        public Info SearchInfo(string name)
        {
            return this.lstInfo.Find(n => n.name == name);
        }

        [MessagePackObject(true)]
        public class Info
        {
            public string name { get; set; }

            public string version { get; set; }

            public long pos { get; set; }

            public long size { get; set; }

            public Info()
            {
                this.name = "";
                this.version = "";
                this.pos = 0L;
                this.size = 0L;
            }

            public override string ToString()
            {
                return this.name;
            }
        }
    }
}
