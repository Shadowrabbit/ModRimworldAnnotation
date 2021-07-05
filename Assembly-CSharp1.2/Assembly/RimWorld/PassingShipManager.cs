using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E5 RID: 6373
	public sealed class PassingShipManager : IExposable
	{
		// Token: 0x06008D2B RID: 36139 RVA: 0x0005E9A9 File Offset: 0x0005CBA9
		public PassingShipManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06008D2C RID: 36140 RVA: 0x0028E9AC File Offset: 0x0028CBAC
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

		// Token: 0x06008D2D RID: 36141 RVA: 0x0005E9C3 File Offset: 0x0005CBC3
		public void AddShip(PassingShip vis)
		{
			this.passingShips.Add(vis);
			vis.passingShipManager = this;
		}

		// Token: 0x06008D2E RID: 36142 RVA: 0x0005E9D8 File Offset: 0x0005CBD8
		public void RemoveShip(PassingShip vis)
		{
			this.passingShips.Remove(vis);
			vis.passingShipManager = null;
		}

		// Token: 0x06008D2F RID: 36143 RVA: 0x0028EA00 File Offset: 0x0028CC00
		public void PassingShipManagerTick()
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				this.passingShips[i].PassingShipTick();
			}
		}

		// Token: 0x06008D30 RID: 36144 RVA: 0x0028EA38 File Offset: 0x0028CC38
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

		// Token: 0x06008D31 RID: 36145 RVA: 0x0028EA84 File Offset: 0x0028CC84
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

		// Token: 0x04005A16 RID: 23062
		public Map map;

		// Token: 0x04005A17 RID: 23063
		public List<PassingShip> passingShips = new List<PassingShip>();

		// Token: 0x04005A18 RID: 23064
		private static List<PassingShip> tmpPassingShips = new List<PassingShip>();
	}
}
