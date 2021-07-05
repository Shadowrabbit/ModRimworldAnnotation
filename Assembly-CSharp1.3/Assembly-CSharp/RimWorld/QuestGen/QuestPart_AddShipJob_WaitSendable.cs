using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001661 RID: 5729
	public class QuestPart_AddShipJob_WaitSendable : QuestPart_AddShipJob_Wait
	{
		// Token: 0x0600858F RID: 34191 RVA: 0x002FED3C File Offset: 0x002FCF3C
		public override ShipJob GetShipJob()
		{
			ShipJob_WaitSendable shipJob_WaitSendable = (ShipJob_WaitSendable)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_WaitSendable.destination = this.destination;
			shipJob_WaitSendable.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied;
			shipJob_WaitSendable.showGizmos = this.showGizmos;
			shipJob_WaitSendable.sendAwayIfAllDespawned = this.sendAwayIfAllDespawned;
			shipJob_WaitSendable.sendAwayIfAnyDespawnedDownedOrDead = this.sendAwayIfAnyDespawnedDownedOrDead;
			return shipJob_WaitSendable;
		}

		// Token: 0x06008590 RID: 34192 RVA: 0x002FED95 File Offset: 0x002FCF95
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.destination, "destination", false);
		}

		// Token: 0x0400536A RID: 21354
		public MapParent destination;
	}
}
