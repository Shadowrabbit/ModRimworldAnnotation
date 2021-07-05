using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B4E RID: 2894
	public class JobDriver_RemoveFloor : JobDriver_AffectFloor
	{
		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06004410 RID: 17424 RVA: 0x000325BD File Offset: 0x000307BD
		protected override int BaseWorkAmount
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06004411 RID: 17425 RVA: 0x000325C4 File Offset: 0x000307C4
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06004412 RID: 17426 RVA: 0x000325CB File Offset: 0x000307CB
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.ConstructionSpeed;
			}
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x000325D2 File Offset: 0x000307D2
		public JobDriver_RemoveFloor()
		{
			this.clearSnow = true;
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x000325E1 File Offset: 0x000307E1
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
