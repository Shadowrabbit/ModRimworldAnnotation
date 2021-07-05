using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200028F RID: 655
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		// Token: 0x0600125C RID: 4700 RVA: 0x0006A182 File Offset: 0x00068382
		public HediffCompProperties_DrugEffectFactor()
		{
			this.compClass = typeof(HediffComp_DrugEffectFactor);
		}

		// Token: 0x04000DE8 RID: 3560
		public ChemicalDef chemical;
	}
}
