using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CharaTools.AIChara;
using MessagePack;

namespace Plugin.AdvIKPlugin
{
    public class IKResizeAdjustment
    {
        public static readonly Dictionary<IKTarget, string> IKTargetNames = new Dictionary<IKTarget, string>
        {
            { IKTarget.BODY, "f_t_hips" },
            { IKTarget.LEFT_SHOULDER, "f_t_shoulder_L"},
            { IKTarget.LEFT_ELBOW, "f_t_elbo_L"},
            { IKTarget.LEFT_HAND, "f_t_arm_L"},
            { IKTarget.RIGHT_SHOULDER, "f_t_shoulder_R"},
            { IKTarget.RIGHT_ELBOW, "f_t_elbo_R"},
            { IKTarget.RIGHT_HAND, "f_t_arm_R"},
            { IKTarget.LEFT_THIGH, "f_t_thigh_L"},
            { IKTarget.LEFT_KNEE, "f_t_knee_L"},
            { IKTarget.LEFT_FOOT, "f_t_leg_L"},
            { IKTarget.RIGHT_THIGH, "f_t_thigh_R"},
            { IKTarget.RIGHT_KNEE, "f_t_knee_R"},
            { IKTarget.RIGHT_FOOT, "f_t_leg_R"},
        };

        public static readonly Dictionary<IKChain, IKTarget[]> IKChainTargets = new Dictionary<IKChain, IKTarget[]>
        {
            { IKChain.BODY, new IKTarget[] {IKTarget.LEFT_SHOULDER, IKTarget.RIGHT_SHOULDER, IKTarget.LEFT_THIGH, IKTarget.RIGHT_THIGH} },
            { IKChain.LEFT_ARM, new IKTarget[] {IKTarget.LEFT_ELBOW, IKTarget.LEFT_HAND} },
            { IKChain.LEFT_LEG, new IKTarget[] {IKTarget.LEFT_KNEE, IKTarget.LEFT_FOOT} },
            { IKChain.RIGHT_ARM, new IKTarget[] {IKTarget.RIGHT_ELBOW, IKTarget.RIGHT_HAND} },
            { IKChain.RIGHT_LEG, new IKTarget[] {IKTarget.RIGHT_KNEE, IKTarget.RIGHT_FOOT} },
        };

        public static readonly Dictionary<IKChain, IKTarget> IKChainBases = new Dictionary<IKChain, IKTarget>
        {
            { IKChain.BODY, IKTarget.BODY },
            { IKChain.LEFT_ARM, IKTarget.LEFT_SHOULDER },
            { IKChain.RIGHT_ARM, IKTarget.RIGHT_SHOULDER },
            { IKChain.LEFT_LEG, IKTarget.LEFT_SHOULDER },
            { IKChain.RIGHT_LEG, IKTarget.RIGHT_SHOULDER },
        };

        public static readonly Dictionary<IKChain, IKScale[]> IKChainScale = new Dictionary<IKChain, IKScale[]>
        {
            { IKChain.BODY, new IKScale[] { IKScale.BODY, IKScale.BODY } },
            { IKChain.LEFT_ARM, new IKScale[] { IKScale.LEFT_UPPER_ARM, IKScale.LEFT_ARM } },
            { IKChain.RIGHT_ARM, new IKScale[] { IKScale.RIGHT_UPPER_ARM, IKScale.RIGHT_ARM } },
            { IKChain.LEFT_LEG, new IKScale[] { IKScale.LEFT_UPPER_LEG, IKScale.LEFT_LEG } },
            { IKChain.RIGHT_LEG, new IKScale[] { IKScale.RIGHT_UPPER_LEG, IKScale.RIGHT_LEG } }
        };
    }

    public enum IKTarget
    {
        BODY = 0,
        LEFT_SHOULDER = 1,
        LEFT_ELBOW = 2,
        LEFT_HAND = 3,
        RIGHT_SHOULDER = 4,
        RIGHT_ELBOW = 5,
        RIGHT_HAND = 6,
        LEFT_THIGH = 7,
        LEFT_KNEE = 8,
        LEFT_FOOT = 9,
        RIGHT_THIGH = 10,
        RIGHT_KNEE = 11,
        RIGHT_FOOT = 12
    }

    public enum IKScale
    {
        BODY = 0,
        LEFT_UPPER_ARM = 1,
        LEFT_LOWER_ARM = 2,
        LEFT_ARM = 3,
        RIGHT_UPPER_ARM = 4,
        RIGHT_LOWER_ARM = 5,
        RIGHT_ARM = 6,
        LEFT_UPPER_LEG = 7,
        LEFT_LOWER_LEG = 8,
        LEFT_LEG = 9,
        RIGHT_UPPER_LEG = 10,
        RIGHT_LOWER_LEG = 11,
        RIGHT_LEG = 12
    }

    public enum IKChain
    {
        BODY = 0,
        LEFT_ARM = 1,
        LEFT_LEG = 2,
        RIGHT_ARM = 3,
        RIGHT_LEG = 4
    }

    public enum IKResizeChainAdjustment
    {
        OFF = 1,
        CHAIN = 2
    }

    public enum IKResizeCentroid
    {
        NONE = 0,
        AUTO = 1,
        FEET_CENTER = 2,
        FEET_LEFT = 3,
        FEET_RIGHT = 4,
        THIGH_CENTER = 5,
        THIGH_LEFT = 6,
        THIGH_RIGHT = 7,
        BODY = 8,
        SHOULDER_CENTER = 9,
        SHOULDER_LEFT = 10,
        SHOULDER_RIGHT = 11,
        HAND_CENTER = 12,
        HAND_LEFT = 13,
        HAND_RIGHT = 14,
        KNEE_CENTER = 15,
        KNEE_LEFT = 16,
        KNEE_RIGHT = 17,
        ELBOW_CENTER = 18,
        ELBOW_LEFT = 19,
        ELBOW_RIGHT = 20,
        RESIZE = 21
    }
}
