using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D1E RID: 3358
	public class CompProperties_AbilityEffect : AbilityCompProperties
	{
		// Token: 0x04002F60 RID: 12128
		public int goodwillImpact;

		// Token: 0x04002F61 RID: 12129
		public bool psychic;

		// Token: 0x04002F62 RID: 12130
		public bool applicableToMechs = true;

		// Token: 0x04002F63 RID: 12131
		public bool applyGoodwillImpactToLodgers = true;

		// Token: 0x04002F64 RID: 12132
		public ClamorDef clamorType;

		// Token: 0x04002F65 RID: 12133
		public int clamorRadius;

		// Token: 0x04002F66 RID: 12134
		public float screenShakeIntensity;

		// Token: 0x04002F67 RID: 12135
		public SoundDef sound;

		// Token: 0x04002F68 RID: 12136
		public SoundDef soundMale;

		// Token: 0x04002F69 RID: 12137
		public SoundDef soundFemale;

		// Token: 0x04002F6A RID: 12138
		public string customLetterLabel;

		// Token: 0x04002F6B RID: 12139
		public string customLetterText;

		// Token: 0x04002F6C RID: 12140
		public bool sendLetter = true;

		// Token: 0x04002F6D RID: 12141
		public string message;

		// Token: 0x04002F6E RID: 12142
		public MessageTypeDef messageType;

		// Token: 0x04002F6F RID: 12143
		public float weight = 1f;

		// Token: 0x04002F70 RID: 12144
		public bool availableWhenTargetIsWounded = true;
	}
}
