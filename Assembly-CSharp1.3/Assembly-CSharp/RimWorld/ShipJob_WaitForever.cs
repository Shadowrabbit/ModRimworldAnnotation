using System;

namespace RimWorld
{
	// Token: 0x020008F3 RID: 2291
	public class ShipJob_WaitForever : ShipJob_Wait
	{
		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06003C0D RID: 15373 RVA: 0x0014EB94 File Offset: 0x0014CD94
		protected override bool ShouldEnd
		{
			get
			{
				return this.transportShip.shipThing == null || this.transportShip.shipThing.Destroyed;
			}
		}
	}
}
