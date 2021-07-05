using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A14 RID: 2580
	public class CompProperties_Flickable : CompProperties
	{
		// Token: 0x06003F0D RID: 16141 RVA: 0x00157FFA File Offset: 0x001561FA
		public CompProperties_Flickable()
		{
			this.compClass = typeof(CompFlickable);
		}

		// Token: 0x0400221F RID: 8735
		[NoTranslate]
		public string commandTexture = "UI/Commands/DesirePower";

		// Token: 0x04002220 RID: 8736
		[NoTranslate]
		public string commandLabelKey = "CommandDesignateTogglePowerLabel";

		// Token: 0x04002221 RID: 8737
		[NoTranslate]
		public string commandDescKey = "CommandDesignateTogglePowerDesc";
	}
}
