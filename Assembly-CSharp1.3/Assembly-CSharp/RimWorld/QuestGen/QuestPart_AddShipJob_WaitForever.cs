using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001660 RID: 5728
	public class QuestPart_AddShipJob_WaitForever : QuestPart_AddShipJob_Wait
	{
		// Token: 0x0600858D RID: 34189 RVA: 0x002FECEC File Offset: 0x002FCEEC
		public override ShipJob GetShipJob()
		{
			ShipJob_WaitForever shipJob_WaitForever = (ShipJob_WaitForever)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_WaitForever.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied;
			shipJob_WaitForever.showGizmos = this.showGizmos;
			shipJob_WaitForever.sendAwayIfAllDespawned = this.sendAwayIfAllDespawned;
			shipJob_WaitForever.sendAwayIfAnyDespawnedDownedOrDead = this.sendAwayIfAnyDespawnedDownedOrDead;
			return shipJob_WaitForever;
		}
	}
}
