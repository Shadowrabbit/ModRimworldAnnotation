using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F83 RID: 3971
	public class RitualPosition_InCircle : RitualPosition
	{
		// Token: 0x06005E04 RID: 24068 RVA: 0x00204638 File Offset: 0x00202838
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			if (this.preferredRotation != null)
			{
				for (int i = this.distRange.min; i <= this.distRange.max; i++)
				{
					IntVec3 intVec = spot + this.preferredRotation.Value.FacingCell * i;
					if (intVec.InBounds(p.Map) && intVec.Standable(p.Map))
					{
						return new PawnStagePosition(intVec, null, this.preferredRotation.Value.Opposite, this.highlight);
					}
				}
			}
			Thing thing = spot.GetThingList(p.Map).FirstOrDefault((Thing t) => t == ritual.selectedTarget.Thing);
			CellRect spectateRect = (thing != null) ? thing.OccupiedRect() : CellRect.SingleCell(spot);
			IntVec3 cell;
			if (SpectatorCellFinder.TryFindCircleSpectatorCellFor(p, spectateRect, (float)this.distRange.min, (float)this.distRange.max, p.Map, out cell, null, null))
			{
				return new PawnStagePosition(cell, null, Rot4.Invalid, this.highlight);
			}
			return new PawnStagePosition(IntVec3.Invalid, null, Rot4.Invalid, this.highlight);
		}

		// Token: 0x06005E05 RID: 24069 RVA: 0x00204770 File Offset: 0x00202970
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.distRange, "distRange", default(IntRange), false);
			Scribe_Values.Look<Rot4?>(ref this.preferredRotation, "preferredRotation", null, false);
		}

		// Token: 0x04003659 RID: 13913
		public Rot4? preferredRotation;

		// Token: 0x0400365A RID: 13914
		public IntRange distRange;
	}
}
