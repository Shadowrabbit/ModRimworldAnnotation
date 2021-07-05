using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF1 RID: 2801
	public sealed class GameEnder : IExposable
	{
		// Token: 0x060041E6 RID: 16870 RVA: 0x00161378 File Offset: 0x0015F578
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.gameEnding, "gameEnding", false, false);
			Scribe_Values.Look<int>(ref this.ticksToGameOver, "ticksToGameOver", -1, false);
		}

		// Token: 0x060041E7 RID: 16871 RVA: 0x001613A0 File Offset: 0x0015F5A0
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

		// Token: 0x060041E8 RID: 16872 RVA: 0x00161512 File Offset: 0x0015F712
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

		// Token: 0x060041E9 RID: 16873 RVA: 0x00161548 File Offset: 0x0015F748
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

		// Token: 0x0400282C RID: 10284
		public bool gameEnding;

		// Token: 0x0400282D RID: 10285
		private int ticksToGameOver = -1;

		// Token: 0x0400282E RID: 10286
		private const int GameEndCountdownDuration = 400;
	}
}
