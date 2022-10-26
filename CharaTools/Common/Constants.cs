using System;

namespace CharaTools
{
    public static class Constants
    {
        public static readonly string[] sMonth = new string[] {
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec"
        };

		#region Old
        public static readonly string[] sPersonality = new string[] 
        {
            "Composed",
            "Normal",
            "Hardworking",
            "Girlfriend",
            "Fashionable",
            "Timid",
            "Motherly",
            "Sadistic",
            "Open-Minded",
            "Airhead",
            "Careful",
            "Ideal Japanese",
            "Tomboy",
            "Obsessed"
        };

        public static readonly string[] sTrait = new string[] 
        {
            "None",
            "Clean Lover",
            "Lazy",
            "Fragile",
            "Tough",
            "Weak Bladder",
            "Patient",
            "Glass Heart",
            "Brave",
            "Perverted",
            "Self-Control",
            "At Will",
            "Sensitive"
        };

        public static readonly string[] sMentality = new string[] 
        {
            "None",
            "Curious",
            "Affectionate",
            "Lovestruck",
            "Awkward",
            "Reluctant",
            "Loathing",
            "Cooperative",
            "Obedient",
            "Submissive",
            "Interested",
            "Charmed",
            "Aroused"
        };

        public static readonly string[] sSexTrait = new string[] 
        {
            "None",
            "Horny",
            "Sadist",
            "Masochist",
            "Sensitive Breasts",
            "Sensitive Ass",
            "Sensitive Pussy",
            "Love Kisses",
            "Clean Freak",
            "Sex Hater",
            "Lonely"
        };
		#endregion
		
		public static readonly string[] ssPersonality = new string[] 
		{
			"Secretary",
			"Vanilla",
			"Caregiver",
			"Girl-Next-Door",
			"Airhead",
			"Scaredy Cat",
			"Mother Figure",
			"Dominatrix",
			"Daredevil",
			"Goofball",
			"Intellectual",
			"Japanese Ideal",
			"Tomboy",
			"Psycho Stalker"
		};
		public static readonly string[] ssTrait = new string[] 
		{
			"None",
			"Fastidious",
			"Lazy",
			"Frail",
			"Tough",
			"Weak Bladder",
			"Tenacious",
			"Glass Heart",
			"Indomitable",
			"Pent Up",
			"Iron Will",
			"Capricious",
			"Emotional"
		};
		public static readonly string[] ssMentality = new string[] 
		{
			"None",
			"Interested",
			"Likes You?",
			"Love at First",
			"Awkward",
			"Dislike",
			"Disgust",
			"Wants",
			"Dominate Me",
			"Obedient",
			"Fun-Loving",
			"Hurt Me",
			"Obey Me"
		};
		public static readonly string[] ssSexTrait = new string[] 
		{
			"None",
			"Lusty",
			"Sadist",
			"Masochist",
			"Sensitive Breasts",
			"Sensitive Ass",
			"Sensitive Groin",
			"Likes Kisses",
			"Clean Freak",
			"Sexually Cautious",
			"Lonely"
		};

        public static readonly string[] ssAccTypes = new string[]
        {
            "None",
            "Head",
            "Ears",
            "Glasses",
            "Face",
            "Neck",
            "Shoulder",
            "Breasts",
            "Waist",
            "Back",
            "Arms",
            "Hands",
            "Legs",
            "Crotch"
        };

        public enum CardResolutions
		{
            NoChange,
			Original, 	// 252x352
			Normal, 	// 504x704
			Large,  	// 756x1056
			ExtraLarge	// 1134x1584
		};

        public static readonly string[] ssImageFileExts = new string[] { ".bmp", ".jpg", ".jpeg", ".png", ".gif" };
	}
}
