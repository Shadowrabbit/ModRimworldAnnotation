using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DA RID: 6362
	public class CompProperties_UseEffectInstallImplant : CompProperties_Usable
	{
		// Token: 0x06008CF6 RID: 36086 RVA: 0x0005E7F5 File Offset: 0x0005C9F5
		public CompProperties_UseEffectInstallImplant()
		{
			this.compClass = typeof(CompUseEffect_InstallImplant);
		}

		// Token: 0x04005A05 RID: 23045
		public HediffDef hediffDef;

		// Token: 0x04005A06 RID: 23046
		public BodyPartDef bodyPart;

		// Token: 0x04005A07 RID: 23047
		public bool canUpgrade;

		// Token: 0x04005A08 RID: 23048
		public bool allowNonColonists;
	}
}
