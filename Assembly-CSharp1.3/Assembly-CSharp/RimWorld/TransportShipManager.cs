using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE0 RID: 4064
	public class TransportShipManager : IExposable
	{
		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x06005FB2 RID: 24498 RVA: 0x0020B684 File Offset: 0x00209884
		public List<TransportShip> AllTransportShips
		{
			get
			{
				return this.ships;
			}
		}

		// Token: 0x06005FB3 RID: 24499 RVA: 0x0020B68C File Offset: 0x0020988C
		public void RegisterShipObject(TransportShip s)
		{
			this.ships.Add(s);
		}

		// Token: 0x06005FB4 RID: 24500 RVA: 0x0020B69A File Offset: 0x0020989A
		public void DeregisterShipObject(TransportShip s)
		{
			if (s != null)
			{
				s.EndCurrentJob();
				this.ships.Remove(s);
			}
		}

		// Token: 0x06005FB5 RID: 24501 RVA: 0x0020B6B4 File Offset: 0x002098B4
		public void ShipObjectsTick()
		{
			for (int i = this.ships.Count - 1; i >= 0; i--)
			{
				this.ships[i].Tick();
			}
		}

		// Token: 0x06005FB6 RID: 24502 RVA: 0x0020B6EC File Offset: 0x002098EC
		public void ExposeData()
		{
			Scribe_Collections.Look<TransportShip>(ref this.ships, "ships", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ships.RemoveAll((TransportShip x) => x == null) != 0)
				{
					Log.Error("Removed some null ship objects.");
				}
			}
		}

		// Token: 0x040036F6 RID: 14070
		private List<TransportShip> ships = new List<TransportShip>();
	}
}
