using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001657 RID: 5719
	public class QuestNode_AddShipJob_Wait : QuestNode_AddShipJob
	{
		// Token: 0x0600856F RID: 34159 RVA: 0x002FE65C File Offset: 0x002FC85C
		protected override void AddJobVars(ShipJob shipJob, Slate slate)
		{
			ShipJob_Wait shipJob_Wait;
			if ((shipJob_Wait = (shipJob as ShipJob_Wait)) != null)
			{
				shipJob_Wait.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied.GetValue(slate);
				shipJob_Wait.sendAwayIfAllDespawned = this.sendAwayIfAllDespawned.GetValue(slate);
			}
			ShipJob_WaitTime shipJob_WaitTime;
			if ((shipJob_WaitTime = (shipJob as ShipJob_WaitTime)) != null)
			{
				shipJob_WaitTime.duration = this.ticks.GetValue(slate);
			}
		}

		// Token: 0x04005350 RID: 21328
		public SlateRef<int> ticks;

		// Token: 0x04005351 RID: 21329
		public SlateRef<bool> leaveImmediatelyWhenSatisfied;

		// Token: 0x04005352 RID: 21330
		public SlateRef<List<Thing>> sendAwayIfAllDespawned;
	}
}
