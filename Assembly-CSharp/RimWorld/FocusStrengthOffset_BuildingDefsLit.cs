using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F4 RID: 6132
	public class FocusStrengthOffset_BuildingDefsLit : FocusStrengthOffset_BuildingDefs
	{
		// Token: 0x060087BA RID: 34746 RVA: 0x0005B0E8 File Offset: 0x000592E8
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return this.BuildingLit(parent) && base.CanApply(parent, user);
		}

		// Token: 0x060087BB RID: 34747 RVA: 0x0005B0FD File Offset: 0x000592FD
		protected override float OffsetForBuilding(Thing b)
		{
			if (this.BuildingLit(b))
			{
				return base.OffsetFor(b.def);
			}
			return 0f;
		}

		// Token: 0x060087BC RID: 34748 RVA: 0x0027BEA0 File Offset: 0x0027A0A0
		private bool BuildingLit(Thing b)
		{
			CompGlower compGlower = b.TryGetComp<CompGlower>();
			return compGlower != null && compGlower.Glows;
		}

		// Token: 0x060087BD RID: 34749 RVA: 0x0027C6CC File Offset: 0x0027A8CC
		protected override int BuildingCount(Thing parent)
		{
			if (parent == null || !parent.Spawned)
			{
				return 0;
			}
			int num = 0;
			List<Thing> forCell = parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent);
			for (int i = 0; i < forCell.Count; i++)
			{
				Thing b = forCell[i];
				if (this.BuildingLit(b))
				{
					num++;
				}
			}
			return Math.Min(num, this.maxBuildings);
		}
	}
}
