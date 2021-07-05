using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D7 RID: 1751
	public class JobDriver_SmoothFloor : JobDriver_AffectFloor
	{
		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x060030D9 RID: 12505 RVA: 0x0011EB98 File Offset: 0x0011CD98
		protected override int BaseWorkAmount
		{
			get
			{
				return 2800;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x060030DA RID: 12506 RVA: 0x0011EB9F File Offset: 0x0011CD9F
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x060030DB RID: 12507 RVA: 0x0011EBA6 File Offset: 0x0011CDA6
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.SmoothingSpeed;
			}
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x0011EB51 File Offset: 0x0011CD51
		public JobDriver_SmoothFloor()
		{
			this.clearSnow = true;
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x0011EBB0 File Offset: 0x0011CDB0
		protected override void DoEffect(IntVec3 c)
		{
			TerrainDef smoothedTerrain = base.TargetLocA.GetTerrain(base.Map).smoothedTerrain;
			base.Map.terrainGrid.SetTerrain(base.TargetLocA, smoothedTerrain);
			FilthMaker.RemoveAllFilth(base.TargetLocA, base.Map);
		}
	}
}
