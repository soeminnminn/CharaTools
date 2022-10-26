using System;
using System.Collections.Generic;

namespace CharaTools.HS2
{
    public class PersonalParameterInfo
    {
        [Serializable]
        public class Param
        {
            public int id;

            public int dependence;

            public List<int> desirePrioritys = new List<int>();

            public List<int> statusPrioritys = new List<int>();

            public List<int> warnings = new List<int>();
        }

        public byte m_Enabled;
        public string m_Name = string.Empty;

        public List<Param> param = new List<Param>();
    }
}
