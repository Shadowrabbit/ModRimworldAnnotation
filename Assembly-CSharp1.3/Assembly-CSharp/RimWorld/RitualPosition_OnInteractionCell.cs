using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F86 RID: 3974
	public class RitualPosition_OnInteractionCell : RitualPosition
	{
		// Token: 0x06005E0E RID: 24078 RVA: 0x002049D8 File Offset: 0x00202BD8
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			RitualPosition_OnInteractionCell.<>c__DisplayClass2_0 CS$<>8__locals1 = new RitualPosition_OnInteractionCell.<>c__DisplayClass2_0();
			CS$<>8__locals1.ritual = ritual;
			CS$<>8__locals1.p = p;
			CS$<>8__locals1.spot = spot;
			Thing thing = CS$<>8__locals1.spot.GetThingList(CS$<>8__locals1.ritual.Map).FirstOrDefault((Thing t) => t == CS$<>8__locals1.ritual.selectedTarget.Thing);
			CellRect rect = (thing != null) ? thing.OccupiedRect() : CellRect.CenteredOn(CS$<>8__locals1.spot, 0);
			CS$<>8__locals1.map = CS$<>8__locals1.p.MapHeld;
			IntVec3 intVec = thing.InteractionCell + this.offset;
			IntVec3 cell;
			if (CS$<>8__locals1.<GetCell>g__Validator|1(intVec))
			{
				cell = intVec;
			}
			else
			{
				cell = base.GetFallbackSpot(rect, CS$<>8__locals1.spot, CS$<>8__locals1.p, CS$<>8__locals1.ritual, new Func<IntVec3, bool>(CS$<>8__locals1.<GetCell>g__Validator|1));
			}
			return new PawnStagePosition(cell, thing, this.facing, this.highlight);
		}

		// Token: 0x06005E0F RID: 24079 RVA: 0x00204AAC File Offset: 0x00202CAC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Rot4>(ref this.facing, "facing", default(Rot4), false);
			Scribe_Values.Look<IntVec3>(ref this.offset, "offset", default(IntVec3), false);
		}

		// Token: 0x0400365F RID: 13919
		public Rot4 facing = Rot4.Invalid;

		// Token: 0x04003660 RID: 13920
		public IntVec3 offset = IntVec3.Zero;
	}
}
