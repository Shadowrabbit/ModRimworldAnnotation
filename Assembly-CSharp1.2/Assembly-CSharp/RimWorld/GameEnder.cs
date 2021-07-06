using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101A RID: 4122
	public sealed class GameEnder : IExposable
	{
		// Token: 0x060059ED RID: 23021 RVA: 0x0003E6C5 File Offset: 0x0003C8C5
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.gameEnding, "gameEnding", false, false);
			Scribe_Values.Look<int>(ref this.ticksToGameOver, "ticksToGameOver", -1, false);
		}

		// Token: 0x060059EE RID: 23022 RVA: 0x001D3F10 File Offset: 0x001D2110
		public void CheckOrUpdateGameOver()
		{
			if (Find.TickManager.TicksGame < 300)
			{
				return;
			}
			if (ShipCountdown.CountingDown)
			{
				this.gameEnding = false;
				return;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount >= 1)
				{
					this.gameEnding = false;
					return;
				}
			}
			for (int j = 0; j < maps.Count; j++)
			{
				List<Pawn> allPawnsSpawned = maps[j].mapPawns.AllPawnsSpawned;
				for (int k = 0; k < allPawnsSpawned.Count; k++)
				{
					if (allPawnsSpawned[k].carryTracker != null)
					{
						Pawn pawn = allPawnsSpawned[k].carryTracker.CarriedThing as Pawn;
						if (pawn != null && pawn.IsFreeColonist)
						{
							this.gameEnding = false;
							return;
						}
					}
				}
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int l = 0; l < caravans.Count; l++)
			{
				if (this.IsPlayerControlledWithFreeColonist(caravans[l]))
				{
					this.gameEnding = false;
					return;
				}
			}
			List<TravelingTransportPods> travelingTransportPods = Find.WorldObjects.TravelingTransportPods;
			for (int m = 0; m < travelingTransportPods.Count; m++)
			{
				if (travelingTransportPods[m].PodsHaveAnyFreeColonist)
				{
					this.gameEnding = false;
					return;
				}
			}
			if (QuestUtility.TotalBorrowedColonistCount() > 0)
			{
				return;
			}
			if (this.gameEnding)
			{
				return;
			}
			this.gameEnding = true;
			this.ticksToGameOver = 400;
		}

		// Token: 0x060059EF RID: 23023 RVA: 0x0003E6EB File Offset: 0x0003C8EB
		public void GameEndTick()
		{
			if (this.gameEnding)
			{
				this.ticksToGameOver--;
				if (this.ticksToGameOver == 0)
				{
					GenGameEnd.EndGameDialogMessage("GameOverEveryoneDead".Translate(), true);
				}
			}
		}

		// Token: 0x060059F0 RID: 23024 RVA: 0x001D4084 File Offset: 0x001D2284
		private bool IsPlayerControlledWithFreeColonist(Caravan caravan)
		{
			if (!caravan.IsPlayerControlled)
			{
				return false;
			}
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				if (pawn.IsColonist && pawn.HostFaction == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003C8D RID: 15501
		public bool gameEnding;

		// Token: 0x04003C8E RID: 15502
		private int ticksToGameOver = -1;

		// Token: 0x04003C8F RID: 15503
		private const int GameEndCountdownDuration = 400;
	}
}
