using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B4F RID: 2895
	public class JobDriver_SmoothFloor : JobDriver_AffectFloor
	{
		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06004415 RID: 17429 RVA: 0x00032619 File Offset: 0x00030819
		protected override int BaseWorkAmount
		{
			get
			{
				return 2800;
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06004416 RID: 17430 RVA: 0x00032620 File Offset: 0x00030820
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06004417 RID: 17431 RVA: 0x00032627 File Offset: 0x00030827
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.SmoothingSpeed;
			}
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x000325D2 File Offset: 0x000307D2
		public JobDriver_SmoothFloor()
		{
			this.clearSnow = true;
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x0018F840 File Offset: 0x0018DA40
		protected override void DoEffect(IntVec3 c)
		{
			TerrainDef smoothedTerrain = base.TargetLocA.GetTerrain(base.Map).smoothedTerrain;
			base.Map.terrainGrid.SetTerrain(base.TargetLocA, smoothedTerrain);
			FilthMaker.RemoveAllFilth(base.TargetLocA, base.Map);
		}
	}
}
