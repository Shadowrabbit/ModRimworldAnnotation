using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001665 RID: 5733
	public static class SendTransportShipAwayUtility
	{
		// Token: 0x0600859B RID: 34203 RVA: 0x002FEFA4 File Offset: 0x002FD1A4
		public static void SendTransportShipAway(TransportShip transportShip, bool unloadContents, TransportShipDropMode unsatisfiedDropMode = TransportShipDropMode.NonRequired)
		{
			if (transportShip == null || transportShip.Disposed)
			{
				return;
			}
			if (!transportShip.started)
			{
				transportShip.Dispose();
				return;
			}
			if (!transportShip.ShipExistsAndIsSpawned || transportShip.LeavingSoonAutomatically)
			{
				return;
			}
			ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway);
			shipJob_FlyAway.dropMode = unsatisfiedDropMode;
			if (unloadContents)
			{
				transportShip.ForceJob(ShipJobDefOf.Unload);
				transportShip.AddJob(shipJob_FlyAway);
				return;
			}
			transportShip.ForceJob(shipJob_FlyAway);
		}
	}
}
