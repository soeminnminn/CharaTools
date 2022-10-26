using System;
using System.Collections.Generic;
using CharaTools.AIChara;
using MessagePack;

namespace Plugin.MaterialEditor
{
    public class MaterialEditorCharaController
    {
        /// <summary>
        /// Parts of the body
        /// </summary>
        public static HashSet<string> BodyParts = new HashSet<string> {
            "o_eyebase_L", "o_eyebase_R", "o_eyelashes", "o_eyeshadow", "o_head", "o_namida", "o_tang", "o_tooth", "o_body_cf", "o_mnpa", "o_mnpb", "cm_o_dan00", "o_tang",
            "cm_o_dan00", "o_tang", "o_silhouette_cf", "o_body_cf", "o_body_cm", "o_head" };

        /// <summary>
        /// Parts of the body hierarchy which contains clothes and should not be traversed
        /// </summary>
        public static HashSet<string> ClothesParts = new HashSet<string> { "ct_clothesTop", "ct_clothesBot", "ct_inner_t", "ct_inner_b", "ct_gloves", "ct_panst", "ct_socks", "ct_shoes" };

        /// <summary>
        /// Parts of the mouth that need special handling
        /// </summary>
        public static HashSet<string> MouthParts = new HashSet<string> { "o_tooth", "o_tang" };

        /// <summary>
        /// Type of the shader property
        /// </summary>
        public enum ShaderPropertyType
        {
            /// <summary>
            /// Texture
            /// </summary>
            Texture,
            /// <summary>
            /// Color, Vector4, Vector3, Vector2
            /// </summary>
            Color,
            /// <summary>
            /// Float, Int, Bool
            /// </summary>
            Float
        }
        /// <summary>
        /// Properties of a renderer that can be set
        /// </summary>
        public enum RendererProperties
        {
            /// <summary>
            /// Whether the renderer is enabled
            /// </summary>
            Enabled,
            /// <summary>
            /// ShadowCastingMode of the renderer
            /// </summary>
            ShadowCastingMode,
            /// <summary>
            /// Whether the renderer will receive shadows cast by other objects
            /// </summary>
            ReceiveShadows
        }

        /// <summary>
        /// Type of object, used for saving MaterialEditor data.
        /// </summary>
        public enum ObjectType
        {
            /// <summary>
            /// Unknown type, things should never be of this type
            /// </summary>
            Unknown,
            /// <summary>
            /// Clothing
            /// </summary>
            Clothing,
            /// <summary>
            /// Accessory
            /// </summary>
            Accessory,
            /// <summary>
            /// Hair
            /// </summary>
            Hair,
            /// <summary>
            /// Parts of a character
            /// </summary>
            Character
        };

        /// <summary>
        /// Data storage class for renderer properties
        /// </summary>
        [Serializable]
        [MessagePackObject]
        public class RendererProperty
        {
            /// <summary>
            /// Type of the object
            /// </summary>
            [Key("ObjectType")]
            public ObjectType ObjectType;
            /// <summary>
            /// Coordinate index, always 0 except in Koikatsu
            /// </summary>
            [Key("CoordinateIndex")]
            public int CoordinateIndex;
            /// <summary>
            /// Slot of the accessory, hair, or clothing
            /// </summary>
            [Key("Slot")]
            public int Slot;
            /// <summary>
            /// Name of the renderer
            /// </summary>
            [Key("RendererName")]
            public string RendererName;
            /// <summary>
            /// Property type
            /// </summary>
            [Key("Property")]
            public RendererProperties Property;
            /// <summary>
            /// Value
            /// </summary>
            [Key("Value")]
            public string Value;
            /// <summary>
            /// Original value
            /// </summary>
            [Key("ValueOriginal")]
            public string ValueOriginal;

            public RendererProperty()
            { }

            /// <summary>
            /// Data storage class for renderer properties
            /// </summary>
            /// <param name="objectType">Type of the object</param>
            /// <param name="coordinateIndex">Coordinate index, always 0 except in Koikatsu</param>
            /// <param name="slot">Slot of the accessory, hair, or clothing</param>
            /// <param name="rendererName">Name of the renderer</param>
            /// <param name="property">Property type</param>
            /// <param name="value">Value</param>
            /// <param name="valueOriginal">Original</param>
            public RendererProperty(ObjectType objectType, int coordinateIndex, int slot, string rendererName, RendererProperties property, string value, string valueOriginal)
            {
                ObjectType = objectType;
                CoordinateIndex = coordinateIndex;
                Slot = slot;
                RendererName = rendererName.Replace("(Instance)", "").Trim();
                Property = property;
                Value = value;
                ValueOriginal = valueOriginal;
            }
        }

        /// <summary>
        /// Data storage class for float properties
        /// </summary>
        [Serializable]
        [MessagePackObject]
        public class MaterialFloatProperty
        {
            /// <summary>
            /// Type of the object
            /// </summary>
            [Key("ObjectType")]
            public ObjectType ObjectType;
            /// <summary>
            /// Coordinate index, always 0 except in Koikatsu
            /// </summary>
            [Key("CoordinateIndex")]
            public int CoordinateIndex;
            /// <summary>
            /// Slot of the accessory, hair, or clothing
            /// </summary>
            [Key("Slot")]
            public int Slot;
            /// <summary>
            /// Name of the material
            /// </summary>
            [Key("MaterialName")]
            public string MaterialName;
            /// <summary>
            /// Name of the property
            /// </summary>
            [Key("Property")]
            public string Property;
            /// <summary>
            /// Value
            /// </summary>
            [Key("Value")]
            public string Value;
            /// <summary>
            /// Original value
            /// </summary>
            [Key("ValueOriginal")]
            public string ValueOriginal;

            public MaterialFloatProperty()
            { }

            /// <summary>
            /// Data storage class for float properties
            /// </summary>
            /// <param name="objectType">Type of the object</param>
            /// <param name="coordinateIndex">Coordinate index, always 0 except in Koikatsu</param>
            /// <param name="slot">Slot of the accessory, hair, or clothing</param>
            /// <param name="materialName">Name of the material</param>
            /// <param name="property">Name of the property</param>
            /// <param name="value">Value</param>
            /// <param name="valueOriginal">Original value</param>
            public MaterialFloatProperty(ObjectType objectType, int coordinateIndex, int slot, string materialName, string property, string value, string valueOriginal)
            {
                ObjectType = objectType;
                CoordinateIndex = coordinateIndex;
                Slot = slot;
                MaterialName = materialName.Replace("(Instance)", "").Trim();
                Property = property;
                Value = value;
                ValueOriginal = valueOriginal;
            }
        }

        /// <summary>
        /// Data storage class for color properties
        /// </summary>
        [Serializable]
        [MessagePackObject]
        public class MaterialColorProperty
        {
            /// <summary>
            /// Type of the object
            /// </summary>
            [Key("ObjectType")]
            public ObjectType ObjectType;
            /// <summary>
            /// Coordinate index, always 0 except in Koikatsu
            /// </summary>
            [Key("CoordinateIndex")]
            public int CoordinateIndex;
            /// <summary>
            /// Slot of the accessory, hair, or clothing
            /// </summary>
            [Key("Slot")]
            public int Slot;
            /// <summary>
            /// Name of the material
            /// </summary>
            [Key("MaterialName")]
            public string MaterialName;
            /// <summary>
            /// Name of the property
            /// </summary>
            [Key("Property")]
            public string Property;
            /// <summary>
            /// Value
            /// </summary>
            [Key("Value")]
            public Color Value;
            /// <summary>
            /// Original value
            /// </summary>
            [Key("ValueOriginal")]
            public Color ValueOriginal;

            public MaterialColorProperty()
            { }

            /// <summary>
            /// Data storage class for color properties
            /// </summary>
            /// <param name="objectType">Type of the object</param>
            /// <param name="coordinateIndex">Coordinate index, always 0 except in Koikatsu</param>
            /// <param name="slot">Slot of the accessory, hair, or clothing</param>
            /// <param name="materialName">Name of the material</param>
            /// <param name="property">Name of the property</param>
            /// <param name="value">Value</param>
            /// <param name="valueOriginal">Original value</param>
            public MaterialColorProperty(ObjectType objectType, int coordinateIndex, int slot, string materialName, string property, Color value, Color valueOriginal)
            {
                ObjectType = objectType;
                CoordinateIndex = coordinateIndex;
                Slot = slot;
                MaterialName = materialName.Replace("(Instance)", "").Trim();
                Property = property;
                Value = value;
                ValueOriginal = valueOriginal;
            }
        }

        /// <summary>
        /// Data storage class for texture properties
        /// </summary>
        [Serializable]
        [MessagePackObject]
        public class MaterialTextureProperty
        {
            /// <summary>
            /// Type of the object
            /// </summary>
            [Key("ObjectType")]
            public ObjectType ObjectType;
            /// <summary>
            /// Coordinate index, always 0 except in Koikatsu
            /// </summary>
            [Key("CoordinateIndex")]
            public int CoordinateIndex;
            /// <summary>
            /// Slot of the accessory, hair, or clothing
            /// </summary>
            [Key("Slot")]
            public int Slot;
            /// <summary>
            /// Name of the material
            /// </summary>
            [Key("MaterialName")]
            public string MaterialName;
            /// <summary>
            /// Name of the property
            /// </summary>
            [Key("Property")]
            public string Property;
            /// <summary>
            /// ID of the texture as stored in the texture dictionary
            /// </summary>
            [Key("TexID")]
            public int? TexID;
            /// <summary>
            /// Texture offset value
            /// </summary>
            [Key("Offset")]
            public Vector2 Offset;
            /// <summary>
            /// Texture offset original value
            /// </summary>
            [Key("OffsetOriginal")]
            public Vector2 OffsetOriginal;
            /// <summary>
            /// Texture scale value
            /// </summary>
            [Key("Scale")]
            public Vector2 Scale;
            /// <summary>
            /// Texture scale original value
            /// </summary>
            [Key("ScaleOriginal")]
            public Vector2 ScaleOriginal;

            public MaterialTextureProperty()
            { }

            /// <summary>
            /// Data storage class for texture properties
            /// </summary>
            /// <param name="objectType">Type of the object</param>
            /// <param name="coordinateIndex">Coordinate index, always 0 except in Koikatsu</param>
            /// <param name="slot">Slot of the accessory, hair, or clothing</param>
            /// <param name="materialName">Name of the material</param>
            /// <param name="property">Name of the property</param>
            /// <param name="texID">ID of the texture as stored in the texture dictionary</param>
            /// <param name="offset">Texture offset value</param>
            /// <param name="offsetOriginal">Texture offset original value</param>
            /// <param name="scale">Texture scale value</param>
            /// <param name="scaleOriginal">Texture scale original value</param>
            public MaterialTextureProperty(ObjectType objectType, int coordinateIndex, int slot, string materialName, string property, int? texID = null, Vector2 offset = null, Vector2 offsetOriginal = null, Vector2 scale = null, Vector2 scaleOriginal = null)
            {
                ObjectType = objectType;
                CoordinateIndex = coordinateIndex;
                Slot = slot;
                MaterialName = materialName.Replace("(Instance)", "").Trim();
                Property = property;
                TexID = texID;
                Offset = offset;
                OffsetOriginal = offsetOriginal;
                Scale = scale;
                ScaleOriginal = scaleOriginal;
            }

            /// <summary>
            /// Check if the TexID, Offset, and Scale are all null. Safe to remove this data if true.
            /// </summary>
            /// <returns></returns>
            public bool NullCheck() => TexID == null && Offset == null && Scale == null;
        }

        /// <summary>
        /// Data storage class for shader data
        /// </summary>
        [Serializable]
        [MessagePackObject]
        public class MaterialShader
        {
            /// <summary>
            /// Type of the object
            /// </summary>
            [Key("ObjectType")]
            public ObjectType ObjectType;
            /// <summary>
            /// Coordinate index, always 0 except in Koikatsu
            /// </summary>
            [Key("CoordinateIndex")]
            public int CoordinateIndex;
            /// <summary>
            /// Slot of the accessory, hair, or clothing
            /// </summary>
            [Key("Slot")]
            public int Slot;
            /// <summary>
            /// Name of the material
            /// </summary>
            [Key("MaterialName")]
            public string MaterialName;
            /// <summary>
            /// Name of the shader
            /// </summary>
            [Key("ShaderName")]
            public string ShaderName;
            /// <summary>
            /// Name of the original shader
            /// </summary>
            [Key("ShaderNameOriginal")]
            public string ShaderNameOriginal;
            /// <summary>
            /// Render queue
            /// </summary>
            [Key("RenderQueue")]
            public int? RenderQueue;
            /// <summary>
            /// Original render queue
            /// </summary>
            [Key("RenderQueueOriginal")]
            public int? RenderQueueOriginal;

            public MaterialShader()
            { }

            /// <summary>
            /// Data storage class for shader data
            /// </summary>
            /// <param name="objectType">Type of the object</param>
            /// <param name="coordinateIndex">Coordinate index, always 0 except in Koikatsu</param>
            /// <param name="slot">Slot of the accessory, hair, or clothing</param>
            /// <param name="materialName">Name of the material</param>
            /// <param name="shaderName">Name of the shader</param>
            /// <param name="shaderNameOriginal">Name of the original shader</param>
            /// <param name="renderQueue">Render queue</param>
            /// <param name="renderQueueOriginal">Original render queue</param>
            public MaterialShader(ObjectType objectType, int coordinateIndex, int slot, string materialName, string shaderName, string shaderNameOriginal, int? renderQueue, int? renderQueueOriginal)
            {
                ObjectType = objectType;
                CoordinateIndex = coordinateIndex;
                Slot = slot;
                MaterialName = materialName.Replace("(Instance)", "").Trim();
                ShaderName = shaderName;
                ShaderNameOriginal = shaderNameOriginal;
                RenderQueue = renderQueue;
                RenderQueueOriginal = renderQueueOriginal;
            }
            /// <summary>
            /// Data storage class for shader data
            /// </summary>
            /// <param name="objectType">Type of the object</param>
            /// <param name="coordinateIndex">Coordinate index, always 0 except in Koikatsu</param>
            /// <param name="slot">Slot of the accessory, hair, or clothing</param>
            /// <param name="materialName">Name of the material</param>
            /// <param name="shaderName">Name of the shader</param>
            /// <param name="shaderNameOriginal">Name of the original shader</param>
            public MaterialShader(ObjectType objectType, int coordinateIndex, int slot, string materialName, string shaderName, string shaderNameOriginal)
            {
                ObjectType = objectType;
                CoordinateIndex = coordinateIndex;
                Slot = slot;
                MaterialName = materialName.Replace("(Instance)", "").Trim();
                ShaderName = shaderName;
                ShaderNameOriginal = shaderNameOriginal;
            }
            /// <summary>
            /// Data storage class for shader data
            /// </summary>
            /// <param name="objectType">Type of the object</param>
            /// <param name="coordinateIndex">Coordinate index, always 0 except in Koikatsu</param>
            /// <param name="slot">Slot of the accessory, hair, or clothing</param>
            /// <param name="materialName">Name of the material</param>
            /// <param name="renderQueue">Render queue</param>
            /// <param name="renderQueueOriginal">Original render queue</param>
            public MaterialShader(ObjectType objectType, int coordinateIndex, int slot, string materialName, int? renderQueue, int? renderQueueOriginal)
            {
                ObjectType = objectType;
                CoordinateIndex = coordinateIndex;
                Slot = slot;
                MaterialName = materialName.Replace("(Instance)", "").Trim();
                RenderQueue = renderQueue;
                RenderQueueOriginal = renderQueueOriginal;
            }

            /// <summary>
            /// Check if the shader name and render queue are both null. Safe to delete this data if true.
            /// </summary>
            /// <returns></returns>
            public bool NullCheck() => string.IsNullOrEmpty(ShaderName) && RenderQueue == null;
        }

        /// <summary>
        /// Data storage class for material copy info
        /// </summary>
        [Serializable]
        [MessagePackObject]
        public class MaterialCopy
        {
            /// <summary>
            /// Type of the object
            /// </summary>
            [Key("ObjectType")]
            public ObjectType ObjectType;
            /// <summary>
            /// Coordinate index, always 0 except in Koikatsu
            /// </summary>
            [Key("CoordinateIndex")]
            public int CoordinateIndex;
            /// <summary>
            /// Slot of the accessory, hair, or clothing
            /// </summary>
            [Key("Slot")]
            public int Slot;
            /// <summary>
            /// Name of the material
            /// </summary>
            [Key("MaterialName")]
            public string MaterialName;
            /// <summary>
            /// Name of the copy
            /// </summary>
            [Key("MaterialCopyName")]
            public string MaterialCopyName;

            public MaterialCopy()
            { }

            public MaterialCopy(ObjectType objectType, int coordinateIndex, int slot, string materialName, string materialCopyName)
            {
                ObjectType = objectType;
                CoordinateIndex = coordinateIndex;
                Slot = slot;
                MaterialName = materialName.Replace("(Instance)", "").Trim();
                MaterialCopyName = materialCopyName;
            }
        }
    }
}
