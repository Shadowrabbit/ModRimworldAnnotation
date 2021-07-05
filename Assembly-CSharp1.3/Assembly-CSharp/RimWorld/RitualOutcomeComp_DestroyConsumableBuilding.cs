using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F67 RID: 3943
	public class RitualOutcomeComp_DestroyConsumableBuilding : RitualOutcomeComp
	{
		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x06005D80 RID: 23936 RVA: 0x001FF9CC File Offset: 0x001FDBCC
		protected virtual string LabelForDesc
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		// Token: 0x06005D81 RID: 23937 RVA: 0x00200FC6 File Offset: 0x001FF1C6
		public override bool Applies(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget.HasThing;
		}
	}
}
