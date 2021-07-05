using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AE7 RID: 2791
	public class TransitionAction_SetDefendLocalGroup : TransitionAction
	{
		// Token: 0x060041E1 RID: 16865 RVA: 0x00031090 File Offset: 0x0002F290
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
