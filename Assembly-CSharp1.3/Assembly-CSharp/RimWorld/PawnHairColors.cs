using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC7 RID: 3527
	public static class PawnHairColors
	{
		// Token: 0x060051A2 RID: 20898 RVA: 0x001B8640 File Offset: 0x001B6840
		public static Color RandomHairColor(Color skinColor, int ageYears)
		{
			if (Rand.Value < 0.02f)
			{
				return new Color(Rand.Value, Rand.Value, Rand.Value);
			}
			if (ageYears > 40)
			{
				float num = GenMath.SmootherStep(40f, 75f, (float)ageYears);
				if (Rand.Value < num)
				{
					float num2 = Rand.Range(0.65f, 0.85f);
					return new Color(num2, num2, num2);
				}
			}
			if (PawnSkinColors.IsDarkSkin(skinColor) || Rand.Value < 0.5f)
			{
				float value = Rand.Value;
				if (value < 0.25f)
				{
					return PawnHairColors.DarkBlack;
				}
				if (value < 0.5f)
				{
					return PawnHairColors.MidBlack;
				}
				if (value < 0.75f)
				{
					return PawnHairColors.DarkReddish;
				}
				return PawnHairColors.DarkSaturatedReddish;
			}
			else
			{
				float value2 = Rand.Value;
				if (value2 < 0.25f)
				{
					return PawnHairColors.DarkBrown;
				}
				if (value2 < 0.5f)
				{
					return PawnHairColors.ReddishBrown;
				}
				if (value2 < 0.75f)
				{
					return PawnHairColors.SandyBlonde;
				}
				return PawnHairColors.Blonde;
			}
		}

		// Token: 0x04003062 RID: 12386
		public static readonly Color DarkBlack = new Color(0.2f, 0.2f, 0.2f);

		// Token: 0x04003063 RID: 12387
		public static readonly Color MidBlack = new Color(0.31f, 0.28f, 0.26f);

		// Token: 0x04003064 RID: 12388
		public static readonly Color DarkReddish = new Color(0.25f, 0.2f, 0.15f);

		// Token: 0x04003065 RID: 12389
		public static readonly Color DarkSaturatedReddish = new Color(0.3f, 0.2f, 0.1f);

		// Token: 0x04003066 RID: 12390
		public static readonly Color DarkBrown = new Color(0.3529412f, 0.22745098f, 0.1254902f);

		// Token: 0x04003067 RID: 12391
		public static readonly Color ReddishBrown = new Color(0.5176471f, 0.3254902f, 0.18431373f);

		// Token: 0x04003068 RID: 12392
		public static readonly Color SandyBlonde = new Color(0.75686276f, 0.57254905f, 0.33333334f);

		// Token: 0x04003069 RID: 12393
		public static readonly Color Blonde = new Color(0.92941177f, 0.7921569f, 0.6117647f);
	}
}
