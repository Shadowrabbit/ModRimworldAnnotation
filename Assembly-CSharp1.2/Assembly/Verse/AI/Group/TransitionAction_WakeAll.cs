using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000AE5 RID: 2789
	public class TransitionAction_WakeAll : TransitionAction
	{
		// Token: 0x060041DD RID: 16861 RVA: 0x00188ADC File Offset: 0x00186CDC
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
