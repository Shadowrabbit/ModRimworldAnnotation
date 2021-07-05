using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001366 RID: 4966
	public class CompProperties_AbilityEffect : AbilityCompProperties
	{
		// Token: 0x040047B4 RID: 18356
		public int goodwillImpact;

		// Token: 0x040047B5 RID: 18357
		public bool psychic;

		// Token: 0x040047B6 RID: 18358
		public bool applicableToMechs = true;

		// Token: 0x040047B7 RID: 18359
		public bool applyGoodwillImpactToLodgers = true;

		// Token: 0x040047B8 RID: 18360
		public ClamorDef clamorType;

		// Token: 0x040047B9 RID: 18361
		public int clamorRadius;

		// Token: 0x040047BA RID: 18362
		public float screenShakeIntensity;

		// Token: 0x040047BB RID: 18363
		public SoundDef sound;

		// Token: 0x040047BC RID: 18364
		public string customLetterLabel;

		// Token: 0x040047BD RID: 18365
		public string customLetterText;

		// Token: 0x040047BE RID: 18366
		public bool sendLetter = true;

		// Token: 0x040047BF RID: 18367
		public string message;

		// Token: 0x040047C0 RID: 18368
		public MessageTypeDef messageType;

		// Token: 0x040047C1 RID: 18369
		public float weight = 1f;

		// Token: 0x040047C2 RID: 18370
		public bool availableWhenTargetIsWounded = true;
	}
}
