using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D6 RID: 1750
	public class JobDriver_RemoveFloor : JobDriver_AffectFloor
	{
		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x060030D4 RID: 12500 RVA: 0x0011EB3C File Offset: 0x0011CD3C
		protected override int BaseWorkAmount
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x060030D5 RID: 12501 RVA: 0x0011EB43 File Offset: 0x0011CD43
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x060030D6 RID: 12502 RVA: 0x0011EB4A File Offset: 0x0011CD4A
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.ConstructionSpeed;
			}
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x0011EB51 File Offset: 0x0011CD51
		public JobDriver_RemoveFloor()
		{
			this.clearSnow = true;
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x0011EB60 File Offset: 0x0011CD60
		protected override void DoEffect(IntVec3 c)
		{
			if (base.Map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				base.Map.terrainGrid.RemoveTopLayer(base.TargetLocA, true);
				FilthMaker.RemoveAllFilth(c, base.Map);
			}
		}
	}
}
