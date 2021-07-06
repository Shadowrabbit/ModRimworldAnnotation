using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003D2 RID: 978
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		// Token: 0x06001832 RID: 6194 RVA: 0x00017015 File Offset: 0x00015215
		public HediffCompProperties_DrugEffectFactor()
		{
			this.compClass = typeof(HediffComp_DrugEffectFactor);
		}

		// Token: 0x04001259 RID: 4697
		public ChemicalDef chemical;
	}
}
