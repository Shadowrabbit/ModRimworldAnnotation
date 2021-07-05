using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000683 RID: 1667
	public class TransitionAction_CheckGiveGift : TransitionAction
	{
		// Token: 0x06002F06 RID: 12038 RVA: 0x00117F40 File Offset: 0x00116140
		public override void DoAction(Transition trans)
		{
			if (DebugSettings.instantVisitorsGift || (trans.target.lord.numPawnsLostViolently == 0 && Rand.Chance(VisitorGiftForPlayerUtility.ChanceToLeaveGift(trans.target.lord.faction, trans.Map))))
			{
				VisitorGiftForPlayerUtility.GiveRandomGift(trans.target.lord.ownedPawns, trans.target.lord.faction);
			}
		}
	}
}
