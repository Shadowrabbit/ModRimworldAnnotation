using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ACE RID: 2766
	public class StyleItemSpawningProperties : IExposable
	{
		// Token: 0x0600414E RID: 16718 RVA: 0x0015F2FC File Offset: 0x0015D4FC
		public void ExposeData()
		{
			Scribe_Values.Look<StyleItemFrequency>(ref this.frequency, "frequency", StyleItemFrequency.Never, false);
			Scribe_Values.Look<StyleGender>(ref this.gender, "gender", StyleGender.Male, false);
		}

		// Token: 0x04002721 RID: 10017
		public StyleItemFrequency frequency;

		// Token: 0x04002722 RID: 10018
		public StyleGender gender = StyleGender.Any;
	}
}
