using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F7F RID: 3967
	public class RitualPosition_BesideThing : RitualPosition
	{
		// Token: 0x06005DFB RID: 24059 RVA: 0x002042CC File Offset: 0x002024CC
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			RitualPosition_BesideThing.<>c__DisplayClass0_0 CS$<>8__locals1 = new RitualPosition_BesideThing.<>c__DisplayClass0_0();
			CS$<>8__locals1.ritual = ritual;
			CS$<>8__locals1.p = p;
			CS$<>8__locals1.spot = spot;
			Thing thing = CS$<>8__locals1.spot.GetThingList(CS$<>8__locals1.p.Map).FirstOrDefault((Thing t) => t == CS$<>8__locals1.ritual.selectedTarget.Thing);
			CS$<>8__locals1.rect = ((thing != null) ? thing.OccupiedRect() : CellRect.CenteredOn(CS$<>8__locals1.spot, 0));
			CS$<>8__locals1.map = CS$<>8__locals1.p.MapHeld;
			IntVec3 intVec = (thing != null) ? IntVec3.West.RotatedBy(thing.Rotation) : IntVec3.West;
			CellRect cellRect = new CellRect(CS$<>8__locals1.rect.minX + intVec.x, CS$<>8__locals1.rect.minZ + intVec.z, 1, CS$<>8__locals1.rect.Height);
			CellRect cellRect2 = new CellRect(CS$<>8__locals1.rect.maxX - intVec.z, CS$<>8__locals1.rect.minZ - intVec.z, 1, CS$<>8__locals1.rect.Height);
			IntVec3 cell = IntVec3.Invalid;
			for (int i = 0; i < 16; i++)
			{
				IntVec3 randomCell;
				if (Rand.Chance(0.5f))
				{
					randomCell = cellRect.RandomCell;
				}
				else
				{
					randomCell = cellRect2.RandomCell;
				}
				if (CS$<>8__locals1.<GetCell>g__Validator|1(randomCell))
				{
					cell = randomCell;
					break;
				}
			}
			if (!cell.IsValid)
			{
				cell = base.GetFallbackSpot(CS$<>8__locals1.rect, CS$<>8__locals1.spot, CS$<>8__locals1.p, CS$<>8__locals1.ritual, new Func<IntVec3, bool>(CS$<>8__locals1.<GetCell>g__Validator|1));
			}
			return new PawnStagePosition(cell, thing, Rot4.Invalid, this.highlight);
		}
	}
}
