using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CharaTools.Json
{
    public class ChaFileContractResolver : DefaultContractResolver
    {
        #region Properties
        public bool IsAIChara { get; set; }

        public bool ExcludeExtendedData { get; set; }
        #endregion

        #region Methods
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = x => {

                if (IsAIChara)
                {
                    if (property.PropertyType == typeof(AIChara.ChaFileParameter2)) return false;
                    if (property.PropertyType == typeof(AIChara.ChaFileGameInfo2)) return false;
                }

                if (ExcludeExtendedData)
                {
                    if (property.PropertyName == "ExtendedSaveData") return false;
                    if (property.PropertyType == typeof(AIChara.ChaFileControl.KKExData)) return false;
                }

                return !property.Ignored; 
            };

            return property;
        }
        #endregion
    }
}
