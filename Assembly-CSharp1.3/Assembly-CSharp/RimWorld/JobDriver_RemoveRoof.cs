using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006DF RID: 1759
	public class JobDriver_RemoveRoof : JobDriver_AffectRoof
	{
		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06003112 RID: 12562 RVA: 0x00034716 File Offset: 0x00032916
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x0011F127 File Offset: 0x0011D327
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.areaManager.NoRoof[base.Cell]);
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x0011F138 File Offset: 0x0011D338
		protected override void DoEffect()
		{
			JobDriver_RemoveRoof.removedRoofs.Clear();
			base.Map.roofGrid.SetRoof(base.Cell, null);
			JobDriver_RemoveRoof.removedRoofs.Add(base.Cell);
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(JobDriver_RemoveRoof.removedRoofs, base.Map, true, false);
			JobDriver_RemoveRoof.removedRoofs.Clear();
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x0011F192 File Offset: 0x0011D392
		protected override bool DoWorkFailOn()
		{
			return !base.Cell.Roofed(base.Map);
		}

		// Token: 0x04001D67 RID: 7527
		private static List<IntVec3> removedRoofs = new List<IntVec3>();
	}
}
