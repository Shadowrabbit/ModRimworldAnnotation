using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200115E RID: 4446
	public class FocusStrengthOffset_BuildingDefsLit : FocusStrengthOffset_BuildingDefs
	{
		// Token: 0x06006AD8 RID: 27352 RVA: 0x0023E0E2 File Offset: 0x0023C2E2
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return this.BuildingLit(parent) && base.CanApply(parent, user);
		}

		// Token: 0x06006AD9 RID: 27353 RVA: 0x0023E0F7 File Offset: 0x0023C2F7
		protected override float OffsetForBuilding(Thing b)
		{
			if (this.BuildingLit(b))
			{
				return base.OffsetFor(b.def);
			}
			return 0f;
		}

		// Token: 0x06006ADA RID: 27354 RVA: 0x0023E114 File Offset: 0x0023C314
		private bool BuildingLit(Thing b)
		{
			CompGlower compGlower = b.TryGetComp<CompGlower>();
			return compGlower != null && compGlower.Glows;
		}

		// Token: 0x06006ADB RID: 27355 RVA: 0x0023E134 File Offset: 0x0023C334
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
