using System;
using System.Collections.Generic;

namespace CharaTools.AIChara
{
    public static class ChaListDefine
    {
		public enum CategoryNo
		{
			unknown = -1,
			cha_sample_m = 0,
			cha_sample_f = 1,
			init_mind_param = 3,
			cha_sample_voice = 5,
			mole_layout = 6,
			facepaint_layout = 7,
			bodypaint_layout = 8,
			mo_head = 110,
			mt_skin_f = 111,
			mt_detail_f = 112,
			mt_beard = 121,
			mt_skin_b = 131,
			mt_detail_b = 132,
			mt_sunburn = 133,
			mo_top = 140,
			mo_bot = 141,
			mo_gloves = 144,
			mo_shoes = 147,
			fo_head = 210,
			ft_skin_f = 211,
			ft_detail_f = 212,
			ft_skin_b = 231,
			ft_detail_b = 232,
			ft_sunburn = 233,
			fo_top = 240,
			fo_bot = 241,
			fo_inner_t = 242,
			fo_inner_b = 243,
			fo_gloves = 244,
			fo_panst = 245,
			fo_socks = 246,
			fo_shoes = 247,
			so_hair_b = 300,
			so_hair_f = 301,
			so_hair_s = 302,
			so_hair_o = 303,
			preset_hair_color = 305,
			st_hairmeshptn = 306,
			st_paint = 313,
			st_eyebrow = 314,
			st_eyelash = 315,
			st_eyeshadow = 316,
			st_eye = 317,
			st_eyeblack = 318,
			st_eye_hl = 319,
			st_cheek = 320,
			st_lip = 322,
			st_mole = 323,
			st_nip = 334,
			st_underhair = 335,
			preset_skin_color = 336,
			st_pattern = 348,
			ao_none = 350,
			ao_head = 351,
			ao_ear = 352,
			ao_glasses = 353,
			ao_face = 354,
			ao_neck = 355,
			ao_shoulder = 356,
			ao_chest = 357,
			ao_waist = 358,
			ao_back = 359,
			ao_arm = 360,
			ao_hand = 361,
			ao_leg = 362,
			ao_kokan = 363,
			custom_pose_m = 500,
			custom_pose_f = 501,
			custom_eyebrow_m = 502,
			custom_eyebrow_f = 503,
			custom_eye_m = 504,
			custom_eye_f = 505,
			custom_mouth_m = 506,
			custom_mouth_f = 507
		}

		public enum KeyType
		{
			Unknown = -1,
			ListIndex,
			Category,
			DistributionNo,
			ID,
			Kind,
			Possess,
			Name,
			EN_US,
			ZH_CN,
			ZH_TW,
			JA_JP_PT,
			EN_US_PT,
			ZH_CN_PT,
			ZH_TW_PT,
			MainManifest,
			MainAB,
			MainData,
			MainData02,
			Weights,
			RingOff,
			AddScale,
			AddTex,
			CenterScale,
			CenterX,
			CenterY,
			ColorMask02Tex,
			ColorMask03Tex,
			ColorMaskTex,
			Coordinate,
			Data01,
			Data02,
			Data03,
			Eye01,
			Eye02,
			Eye03,
			EyeMax01,
			EyeMax02,
			EyeMax03,
			Eyebrow01,
			Eyebrow02,
			Eyebrow03,
			EyeHiLight01,
			EyeHiLight02,
			EyeHiLight03,
			GlossTex,
			HeadID,
			KokanHide,
			MainTex,
			MainTex02,
			MainTex03,
			MainTexAB,
			MatData,
			Mouth01,
			Mouth02,
			Mouth03,
			MouthMax01,
			MouthMax02,
			MouthMax03,
			MoveX,
			MoveY,
			NormalMapTex,
			NotBra,
			OcclusionMapTex,
			OverBodyMask,
			OverBodyMaskAB,
			OverBraMask,
			OverBraMaskAB,
			BreakDisableMask,
			OverInnerTBMask,
			OverInnerTBMaskAB,
			OverInnerBMask,
			OverInnerBMaskAB,
			OverPanstMask,
			OverPanstMaskAB,
			OverBodyBMask,
			OverBodyBMaskAB,
			Parent,
			PosX,
			PosY,
			Preset,
			Rot,
			Scale,
			SetHair,
			ShapeAnime,
			StateType,
			ThumbAB,
			ThumbTex,
			Clip,
			OverBraType,
			Pattern,
			Target,
			Correct,
			TexManifest,
			TexAB,
			TexD,
			TexC,
			FAVOR,
			ENJOYMENT,
			AVERSION,
			SLAVERY,
			SampleH,
			SampleS,
			SampleV,
			TopH,
			TopS,
			TopV,
			BaseH,
			BaseS,
			BaseV,
			UnderH,
			UnderS,
			UnderV,
			SpecularH,
			SpecularS,
			SpecularV,
			Metallic,
			Smoothness,
			IKAB,
			IKData
		}

		public static readonly Version CheckItemVersion = new Version("0.0.0");

		public static readonly string CheckItemFile = "save/checkitem.dat";

		public static string GetCategoryName(int no)
		{
			switch(no)
            {
				case 0: return "キャラサンプル男";
				case 1: return "キャラサンプル女";
				case 3: return "心情による初期補正値";
				case 5: return "サンプル音声";
				case 6: return "ホクロの配置設定";
				case 7: return "フェイスペイントの配置設定";
				case 8: return "ボディーペイントの配置設定";
				case 110: return "男頭";
				case 111: return "男肌";
				case 112: return "男シワ";
				case 121: return "男ヒゲ";
				case 131: return "男肌";
				case 132: return "男肉感";
				case 133: return "男日焼け";
				case 140: return "男服上";
				case 141: return "男服下";
				case 144: return "男手袋";
				case 147: return "男靴";
				case 210: return "女頭";
				case 211: return "女肌";
				case 212: return "女シワ";
				case 231: return "女肌";
				case 232: return "女肉感";
				case 233: return "女日焼け";
				case 240: return "女服上";
				case 241: return "女服下";
				case 242: return "女インナー上";
				case 243: return "女インナー下";
				case 244: return "女手袋";
				case 245: return "女パンスト";
				case 246: return "女靴下";
				case 247: return "女靴";
				case 300: return "後ろ髪";
				case 301: return "前髪";
				case 302: return "横髪";
				case 303: return "エクステ";
				case 305: return "髪の毛のカラープリセット";
				case 306: return "髪メッシュパターン";
				case 313: return "ペイント顔・体";
				case 314: return "眉毛";
				case 315: return "睫毛";
				case 316: return "アイシャドウ";
				case 317: return "瞳";
				case 318: return "瞳孔";
				case 319: return "ハイライト";
				case 320: return "チーク";
				case 322: return "リップ";
				case 323: return "ホクロ";
				case 334: return "乳首";
				case 335: return "アンダーヘア";
				case 336: return "肌のカラープリセット";
				case 348: return "パターンテクスチャ";
				case 350: return "アクセサリ(なし)";
				case 351: return "アクセサリ頭";
				case 352: return "アクセサリ耳";
				case 353: return "アクセサリ眼鏡";
				case 354: return "アクセサリ顔";
				case 355: return "アクセサリ首";
				case 356: return "アクセサリ肩";
				case 357: return "アクセサリ胸";
				case 358: return "アクセサリ腰";
				case 359: return "アクセサリ背中";
				case 360: return "アクセサリ腕";
				case 361: return "アクセサリ手";
				case 362: return "アクセサリ脚";
				case 363: return "アクセサリ股間";
				case 500: return "カスタム男ポーズ";
				case 501: return "カスタム女ポーズ";
				case 502: return "カスタム男眉パターン";
				case 503: return "カスタム女眉パターン";
				case 504: return "カスタム男目パターン";
				case 505: return "カスタム女目パターン";
				case 506: return "カスタム男口パターン";
				case 507: return "カスタム女口パターン";
				default: return "";
			}
		}
	}
}
