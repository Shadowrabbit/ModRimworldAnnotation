using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F80 RID: 3968
	public abstract class RitualPosition_VerticalThingCenter : RitualPosition
	{
		// Token: 0x06005DFD RID: 24061 RVA: 0x0020446C File Offset: 0x0020266C
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			RitualPosition_VerticalThingCenter.<>c__DisplayClass1_0 CS$<>8__locals1 = new RitualPosition_VerticalThingCenter.<>c__DisplayClass1_0();
			CS$<>8__locals1.ritual = ritual;
			CS$<>8__locals1.p = p;
			CS$<>8__locals1.spot = spot;
			Thing thing = CS$<>8__locals1.spot.GetThingList(CS$<>8__locals1.p.Map).FirstOrDefault((Thing t) => t == CS$<>8__locals1.ritual.selectedTarget.Thing);
			CS$<>8__locals1.rect = ((thing != null) ? thing.OccupiedRect() : CellRect.SingleCell(CS$<>8__locals1.spot));
			CS$<>8__locals1.map = CS$<>8__locals1.p.MapHeld;
			CellRect rect = this.GetRect(CS$<>8__locals1.rect);
			if (CS$<>8__locals1.<GetCell>g__Validator|1(rect.CenterCell))
			{
				return new PawnStagePosition(rect.CenterCell, thing, Rot4.Invalid, this.highlight);
			}
			IntVec3 cell = IntVec3.Invalid;
			int num = 0;
			while (num < 16 && num < rect.Width)
			{
				IntVec3 randomCell = rect.RandomCell;
				if (CS$<>8__locals1.<GetCell>g__Validator|1(randomCell))
				{
					cell = randomCell;
					break;
				}
				num++;
			}
			if (!cell.IsValid)
			{
				cell = base.GetFallbackSpot(CS$<>8__locals1.rect, CS$<>8__locals1.spot, CS$<>8__locals1.p, CS$<>8__locals1.ritual, new Func<IntVec3, bool>(CS$<>8__locals1.<GetCell>g__Validator|1));
			}
			return new PawnStagePosition(cell, thing, Rot4.Invalid, this.highlight);
		}

		// Token: 0x06005DFE RID: 24062
		protected abstract CellRect GetRect(CellRect thingRect);

		// Token: 0x04003658 RID: 13912
		public IntVec3 offset;
	}
}
