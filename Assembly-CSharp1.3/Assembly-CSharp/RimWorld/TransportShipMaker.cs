using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDF RID: 4063
	public static class TransportShipMaker
	{
		// Token: 0x06005FB1 RID: 24497 RVA: 0x0020B62C File Offset: 0x0020982C
		public static TransportShip MakeTransportShip(TransportShipDef def, IEnumerable<Thing> contents, Thing shipThing = null)
		{
			TransportShip transportShip = new TransportShip(def);
			transportShip.shipThing = (shipThing ?? ThingMaker.MakeThing(def.shipThing, null));
			CompShuttle compShuttle = transportShip.shipThing.TryGetComp<CompShuttle>();
			if (compShuttle != null)
			{
				compShuttle.shipParent = transportShip;
			}
			if (contents != null)
			{
				transportShip.TransporterComp.innerContainer.TryAddRangeOrTransfer(contents, true, true);
			}
			return transportShip;
		}
	}
}
