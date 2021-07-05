using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA4 RID: 2980
	public static class SendShuttleAwayQuestPartUtility
	{
		// Token: 0x06004590 RID: 17808 RVA: 0x00170AEC File Offset: 0x0016ECEC
		public static void SendAway(Thing shuttle, bool dropEverything)
		{
			CompShuttle compShuttle = shuttle.TryGetComp<CompShuttle>();
			if (compShuttle.shipParent == null)
			{
				compShuttle.shipParent = TransportShipMaker.MakeTransportShip(TransportShipDefOf.Ship_Shuttle, null, shuttle);
			}
			if (dropEverything)
			{
				compShuttle.shipParent.ForceJob(ShipJobDefOf.Unload);
				compShuttle.shipParent.AddJob(ShipJobDefOf.FlyAway);
				return;
			}
			compShuttle.shipParent.ForceJob(ShipJobDefOf.FlyAway);
		}
	}
}
