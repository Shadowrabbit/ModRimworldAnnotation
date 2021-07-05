using System;

namespace Verse.AI.Group
{
	// Token: 0x02000680 RID: 1664
	public class TransitionAction_SetDefendLocalGroup : TransitionAction
	{
		// Token: 0x06002F00 RID: 12032 RVA: 0x00117E2D File Offset: 0x0011602D
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
