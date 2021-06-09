using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000AEB RID: 2795
	public class TransitionAction_CheckGiveGift : TransitionAction
	{
		// Token: 0x060041EA RID: 16874 RVA: 0x00188C9C File Offset: 0x00186E9C
		public override void DoAction(Transition trans)
		{
			if (DebugSettings.instantVisitorsGift || (trans.target.lord.numPawnsLostViolently == 0 && Rand.Chance(VisitorGiftForPlayerUtility.ChanceToLeaveGift(trans.target.lord.faction, trans.Map))))
			{
				VisitorGiftForPlayerUtility.GiveGift(trans.target.lord.ownedPawns, trans.target.lord.faction);
			}
		}
	}
}
