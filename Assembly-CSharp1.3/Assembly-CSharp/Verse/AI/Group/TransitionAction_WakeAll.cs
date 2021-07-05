using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200067E RID: 1662
	public class TransitionAction_WakeAll : TransitionAction
	{
		// Token: 0x06002EFC RID: 12028 RVA: 0x00117D5C File Offset: 0x00115F5C
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				RestUtility.WakeUp(ownedPawns[i]);
			}
			List<Building> ownedBuildings = trans.target.lord.ownedBuildings;
			for (int j = 0; j < ownedBuildings.Count; j++)
			{
				CompCanBeDormant compCanBeDormant = ownedBuildings[j].TryGetComp<CompCanBeDormant>();
				if (compCanBeDormant != null)
				{
					compCanBeDormant.WakeUpWithDelay();
				}
			}
		}
	}
}
