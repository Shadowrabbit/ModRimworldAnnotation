using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F87 RID: 3975
	public abstract class RitualPosition_NearbyThing : RitualPosition
	{
		// Token: 0x06005E11 RID: 24081
		public abstract IEnumerable<Thing> CandidateThings(LordJob_Ritual ritual);

		// Token: 0x06005E12 RID: 24082
		public abstract IntVec3 PositionForThing(Thing t);

		// Token: 0x06005E13 RID: 24083 RVA: 0x00204B14 File Offset: 0x00202D14
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			foreach (Thing thing in this.CandidateThings(ritual))
			{
				if (this.IsUsableThing(thing, spot, ritual))
				{
					return new PawnStagePosition(this.PositionForThing(thing), thing, this.FacingDir(thing), this.highlight);
				}
			}
			return new PawnStagePosition(IntVec3.Invalid, null, Rot4.Invalid, this.highlight);
		}

		// Token: 0x06005E14 RID: 24084 RVA: 0x00204B9C File Offset: 0x00202D9C
		public virtual bool IsUsableThing(Thing thing, IntVec3 spot, TargetInfo ritualTarget)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (IntVec3 intVec in (ritualTarget.HasThing ? ritualTarget.Thing.OccupiedRect() : CellRect.CenteredOn(spot, 0)).Cells)
			{
				if (thing.Position.InHorDistOf(intVec, (float)this.maxDistanceToFocus))
				{
					flag = true;
				}
				if (GenSight.LineOfSight(thing.Position, intVec, ritualTarget.Map, true, null, 0, 0))
				{
					flag2 = true;
				}
			}
			return flag && flag2;
		}

		// Token: 0x06005E15 RID: 24085 RVA: 0x00204C44 File Offset: 0x00202E44
		public bool IsUsableThing(Thing thing, IntVec3 spot, LordJob_Ritual ritual)
		{
			return this.IsUsableThing(thing, spot, ritual.selectedTarget);
		}

		// Token: 0x06005E16 RID: 24086 RVA: 0x00204C54 File Offset: 0x00202E54
		public override bool CanUse(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			return this.GetCell(spot, p, ritual).cell.IsValid;
		}

		// Token: 0x06005E17 RID: 24087 RVA: 0x000FE241 File Offset: 0x000FC441
		protected virtual Rot4 FacingDir(Thing t)
		{
			return Rot4.Invalid;
		}

		// Token: 0x04003661 RID: 13921
		public int maxDistanceToFocus = 1;
	}
}
