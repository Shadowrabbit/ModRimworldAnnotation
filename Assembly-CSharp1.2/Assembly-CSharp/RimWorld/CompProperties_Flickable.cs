using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F24 RID: 3876
	public class CompProperties_Flickable : CompProperties
	{
		// Token: 0x0600558D RID: 21901 RVA: 0x0003B644 File Offset: 0x00039844
		public CompProperties_Flickable()
		{
			this.compClass = typeof(CompFlickable);
		}

		// Token: 0x040036BE RID: 14014
		[NoTranslate]
		public string commandTexture = "UI/Commands/DesirePower";

		// Token: 0x040036BF RID: 14015
		[NoTranslate]
		public string commandLabelKey = "CommandDesignateTogglePowerLabel";

		// Token: 0x040036C0 RID: 14016
		[NoTranslate]
		public string commandDescKey = "CommandDesignateTogglePowerDesc";
	}
}
