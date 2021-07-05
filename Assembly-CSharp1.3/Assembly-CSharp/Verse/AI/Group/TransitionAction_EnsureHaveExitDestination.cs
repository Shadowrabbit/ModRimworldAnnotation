using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200067F RID: 1663
	public class TransitionAction_EnsureHaveExitDestination : TransitionAction
	{
		// Token: 0x06002EFE RID: 12030 RVA: 0x00117DD4 File Offset: 0x00115FD4
		public override void DoAction(Transition trans)
		{
			LordToil_Travel lordToil_Travel = (LordToil_Travel)trans.target;
			if (lordToil_Travel.HasDestination())
			{
				return;
			}
			Pawn pawn = lordToil_Travel.lord.ownedPawns.RandomElement<Pawn>();
			IntVec3 destination;
			if (!CellFinder.TryFindRandomPawnExitCell(pawn, out destination))
			{
				RCellFinder.TryFindRandomPawnEntryCell(out destination, pawn.Map, 0f, false, null);
			}
			lordToil_Travel.SetDestination(destination);
		}
	}
}
