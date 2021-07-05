using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200165F RID: 5727
	public class QuestPart_AddShipJob_WaitTime : QuestPart_AddShipJob_Wait
	{
		// Token: 0x0600858A RID: 34186 RVA: 0x002FEC70 File Offset: 0x002FCE70
		public override ShipJob GetShipJob()
		{
			ShipJob_WaitTime shipJob_WaitTime = (ShipJob_WaitTime)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_WaitTime.duration = this.duration;
			shipJob_WaitTime.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied;
			shipJob_WaitTime.showGizmos = this.showGizmos;
			shipJob_WaitTime.sendAwayIfAllDespawned = this.sendAwayIfAllDespawned;
			shipJob_WaitTime.sendAwayIfAnyDespawnedDownedOrDead = this.sendAwayIfAnyDespawnedDownedOrDead;
			return shipJob_WaitTime;
		}

		// Token: 0x0600858B RID: 34187 RVA: 0x002FECC9 File Offset: 0x002FCEC9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x04005369 RID: 21353
		public int duration;
	}
}
