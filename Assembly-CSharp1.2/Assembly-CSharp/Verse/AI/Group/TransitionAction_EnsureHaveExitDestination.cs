using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000AE6 RID: 2790
	public class TransitionAction_EnsureHaveExitDestination : TransitionAction
	{
		// Token: 0x060041DF RID: 16863 RVA: 0x00188B54 File Offset: 0x00186D54
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
