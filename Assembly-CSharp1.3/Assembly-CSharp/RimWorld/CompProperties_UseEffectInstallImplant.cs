using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200120B RID: 4619
	public class CompProperties_UseEffectInstallImplant : CompProperties_Usable
	{
		// Token: 0x06006EF5 RID: 28405 RVA: 0x002517AB File Offset: 0x0024F9AB
		public CompProperties_UseEffectInstallImplant()
		{
			this.compClass = typeof(CompUseEffect_InstallImplant);
		}

		// Token: 0x04003D5E RID: 15710
		public HediffDef hediffDef;

		// Token: 0x04003D5F RID: 15711
		public BodyPartDef bodyPart;

		// Token: 0x04003D60 RID: 15712
		public bool canUpgrade;

		// Token: 0x04003D61 RID: 15713
		public bool allowNonColonists;
	}
}
