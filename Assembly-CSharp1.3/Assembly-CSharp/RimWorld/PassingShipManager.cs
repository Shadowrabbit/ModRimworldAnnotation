using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001214 RID: 4628
	public sealed class PassingShipManager : IExposable
	{
		// Token: 0x06006F23 RID: 28451 RVA: 0x00251DA5 File Offset: 0x0024FFA5
		public PassingShipManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06006F24 RID: 28452 RVA: 0x00251DC0 File Offset: 0x0024FFC0
		public void ExposeData()
		{
			Scribe_Collections.Look<PassingShip>(ref this.passingShips, "passingShips", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.passingShips.Count; i++)
				{
					this.passingShips[i].passingShipManager = this;
				}
			}
		}

		// Token: 0x06006F25 RID: 28453 RVA: 0x00251E13 File Offset: 0x00250013
		public void AddShip(PassingShip vis)
		{
			this.passingShips.Add(vis);
			vis.passingShipManager = this;
		}

		// Token: 0x06006F26 RID: 28454 RVA: 0x00251E28 File Offset: 0x00250028
		public void RemoveShip(PassingShip vis)
		{
			this.passingShips.Remove(vis);
			vis.passingShipManager = null;
		}

		// Token: 0x06006F27 RID: 28455 RVA: 0x00251E40 File Offset: 0x00250040
		public void PassingShipManagerTick()
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				this.passingShips[i].PassingShipTick();
			}
		}

		// Token: 0x06006F28 RID: 28456 RVA: 0x00251E78 File Offset: 0x00250078
		public void RemoveAllShipsOfFaction(Faction faction)
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				if (this.passingShips[i].Faction == faction)
				{
					this.passingShips[i].Depart();
				}
			}
		}

		// Token: 0x06006F29 RID: 28457 RVA: 0x00251EC4 File Offset: 0x002500C4
		internal void DebugSendAllShipsAway()
		{
			PassingShipManager.tmpPassingShips.Clear();
			PassingShipManager.tmpPassingShips.AddRange(this.passingShips);
			for (int i = 0; i < PassingShipManager.tmpPassingShips.Count; i++)
			{
				PassingShipManager.tmpPassingShips[i].Depart();
			}
			Messages.Message("All passing ships sent away.", MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x04003D69 RID: 15721
		public Map map;

		// Token: 0x04003D6A RID: 15722
		public List<PassingShip> passingShips = new List<PassingShip>();

		// Token: 0x04003D6B RID: 15723
		private static List<PassingShip> tmpPassingShips = new List<PassingShip>();
	}
}
