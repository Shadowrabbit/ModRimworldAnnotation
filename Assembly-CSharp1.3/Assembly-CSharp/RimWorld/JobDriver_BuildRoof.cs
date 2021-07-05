using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006D9 RID: 1753
	public class JobDriver_BuildRoof : JobDriver_AffectRoof
	{
		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x060030E6 RID: 12518 RVA: 0x0009007E File Offset: 0x0008E27E
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x0011EC71 File Offset: 0x0011CE71
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

		// Token: 0x060030E8 RID: 12520 RVA: 0x0011EC84 File Offset: 0x0011CE84
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

		// Token: 0x060030E9 RID: 12521 RVA: 0x0011ED4D File Offset: 0x0011CF4D
		protected override bool DoWorkFailOn()
		{
			return base.Cell.Roofed(base.Map);
		}

		// Token: 0x04001D5E RID: 7518
		private static List<IntVec3> builtRoofs = new List<IntVec3>();
	}
}
