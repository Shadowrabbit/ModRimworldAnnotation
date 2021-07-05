using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8B RID: 3979
	public class RitualPosition_AnimaLinkingSpot : RitualPosition
	{
		// Token: 0x06005E25 RID: 24101 RVA: 0x00204D4C File Offset: 0x00202F4C
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			IntVec3 intVec;
			if (SpectatorCellFinder.TryFindCircleSpectatorCellFor(p, CellRect.CenteredOn(spot, 0), 2f, 3f, p.Map, out intVec, null, null))
			{
				return new PawnStagePosition(intVec, null, Rot4.FromAngleFlat((spot - intVec).AngleFlat), this.highlight);
			}
			Thing thing = ritual.selectedTarget.Thing;
			CompPsylinkable compPsylinkable = (thing != null) ? thing.TryGetComp<CompPsylinkable>() : null;
			LocalTargetInfo localTargetInfo;
			if (compPsylinkable != null && compPsylinkable.TryFindLinkSpot(p, out localTargetInfo))
			{
				Rot4 orientation = Rot4.FromAngleFlat((spot - localTargetInfo.Cell).AngleFlat);
				return new PawnStagePosition(localTargetInfo.Cell, null, orientation, this.highlight);
			}
			return new PawnStagePosition(IntVec3.Invalid, null, Rot4.Invalid, this.highlight);
		}
	}
}
