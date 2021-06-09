using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B54 RID: 2900
	public class JobDriver_BuildRoof : JobDriver_AffectRoof
	{
		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06004431 RID: 17457 RVA: 0x0001B6B4 File Offset: 0x000198B4
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004432 RID: 17458 RVA: 0x000326E3 File Offset: 0x000308E3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.areaManager.BuildRoof[base.Cell]);
			this.FailOn(() => !RoofCollapseUtility.WithinRangeOfRoofHolder(base.Cell, base.Map, false));
			this.FailOn(() => !RoofCollapseUtility.ConnectedToRoofHolder(base.Cell, base.Map, true));
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x0018FB34 File Offset: 0x0018DD34
		protected override void DoEffect()
		{
			JobDriver_BuildRoof.builtRoofs.Clear();
			for (int i = 0; i < 9; i++)
			{
				IntVec3 intVec = base.Cell + GenAdj.AdjacentCellsAndInside[i];
				if (intVec.InBounds(base.Map) && base.Map.areaManager.BuildRoof[intVec] && !intVec.Roofed(base.Map) && RoofCollapseUtility.WithinRangeOfRoofHolder(intVec, base.Map, false) && RoofUtility.FirstBlockingThing(intVec, base.Map) == null)
				{
					base.Map.roofGrid.SetRoof(intVec, RoofDefOf.RoofConstructed);
					MoteMaker.PlaceTempRoof(intVec, base.Map);
					JobDriver_BuildRoof.builtRoofs.Add(intVec);
				}
			}
			JobDriver_BuildRoof.builtRoofs.Clear();
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x000326F3 File Offset: 0x000308F3
		protected override bool DoWorkFailOn()
		{
			return base.Cell.Roofed(base.Map);
		}

		// Token: 0x04002E5B RID: 11867
		private static List<IntVec3> builtRoofs = new List<IntVec3>();
	}
}
