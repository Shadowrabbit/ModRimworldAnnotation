using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B65 RID: 2917
	public class JobDriver_RemoveRoof : JobDriver_AffectRoof
	{
		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x0600449F RID: 17567 RVA: 0x0002EB44 File Offset: 0x0002CD44
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x00032A8D File Offset: 0x00030C8D
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

		// Token: 0x060044A1 RID: 17569 RVA: 0x0019083C File Offset: 0x0018EA3C
		protected override void DoEffect()
		{
			JobDriver_RemoveRoof.removedRoofs.Clear();
			base.Map.roofGrid.SetRoof(base.Cell, null);
			JobDriver_RemoveRoof.removedRoofs.Add(base.Cell);
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(JobDriver_RemoveRoof.removedRoofs, base.Map, true, false);
			JobDriver_RemoveRoof.removedRoofs.Clear();
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x00032A9D File Offset: 0x00030C9D
		protected override bool DoWorkFailOn()
		{
			return !base.Cell.Roofed(base.Map);
		}

		// Token: 0x04002E89 RID: 11913
		private static List<IntVec3> removedRoofs = new List<IntVec3>();
	}
}
