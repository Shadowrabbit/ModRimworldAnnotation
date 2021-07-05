using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000681 RID: 1665
	public class TransitionAction_SetDefendTrader : TransitionAction
	{
		// Token: 0x06002F02 RID: 12034 RVA: 0x00117E54 File Offset: 0x00116054
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			Pawn pawn = TraderCaravanUtility.FindTrader(lordToil_DefendPoint.lord);
			if (pawn != null)
			{
				lordToil_DefendPoint.SetDefendPoint(pawn.Position);
				return;
			}
			IEnumerable<Pawn> source = from x in lordToil_DefendPoint.lord.ownedPawns
			where x.GetTraderCaravanRole() == TraderCaravanRole.Carrier
			select x;
			if (source.Any<Pawn>())
			{
				lordToil_DefendPoint.SetDefendPoint(source.RandomElement<Pawn>().Position);
				return;
			}
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
