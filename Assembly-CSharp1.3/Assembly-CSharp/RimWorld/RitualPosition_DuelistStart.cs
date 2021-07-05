using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F85 RID: 3973
	public class RitualPosition_DuelistStart : RitualPosition
	{
		// Token: 0x06005E0A RID: 24074 RVA: 0x0020484C File Offset: 0x00202A4C
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			int num = 0;
			for (int i = 0; i < RitualPosition_DuelistStart.Rotations.Length; i++)
			{
				Rot4 rot = RitualPosition_DuelistStart.Rotations[i];
				IntVec3 b = rot.FacingCell * this.distFromTarget;
				IntVec3 intVec = spot + b;
				if (intVec.InBounds(p.Map) && intVec.Standable(p.Map))
				{
					if (num == this.duelistIndex)
					{
						return new PawnStagePosition(intVec, null, Rot4.FromIntVec3(rot.FacingCell), this.highlight);
					}
					num++;
				}
			}
			Thing thing = spot.GetThingList(p.Map).FirstOrDefault((Thing t) => t == ritual.selectedTarget.Thing);
			CellRect spectateRect = (thing != null) ? thing.OccupiedRect() : CellRect.SingleCell(spot);
			IntVec3 cell;
			if (SpectatorCellFinder.TryFindCircleSpectatorCellFor(p, spectateRect, 1f, (float)(this.distFromTarget * 2), p.Map, out cell, null, null))
			{
				return new PawnStagePosition(cell, null, Rot4.Invalid, this.highlight);
			}
			return new PawnStagePosition(IntVec3.Invalid, null, Rot4.Invalid, this.highlight);
		}

		// Token: 0x06005E0B RID: 24075 RVA: 0x0020496E File Offset: 0x00202B6E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.distFromTarget, "distFromTarget", 0, false);
			Scribe_Values.Look<int>(ref this.duelistIndex, "duelistIndex", 0, false);
		}

		// Token: 0x0400365C RID: 13916
		public int distFromTarget;

		// Token: 0x0400365D RID: 13917
		public int duelistIndex;

		// Token: 0x0400365E RID: 13918
		private static readonly Rot4[] Rotations = new Rot4[]
		{
			Rot4.West,
			Rot4.East,
			Rot4.North,
			Rot4.South
		};
	}
}
