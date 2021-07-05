using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001C1 RID: 449
	public sealed class MapPawns
	{
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00044280 File Offset: 0x00042480
		public List<Pawn> AllPawns
		{
			get
			{
				List<Pawn> allPawnsUnspawned = this.AllPawnsUnspawned;
				if (allPawnsUnspawned.Count == 0)
				{
					return this.pawnsSpawned;
				}
				this.allPawnsResult.Clear();
				this.allPawnsResult.AddRange(this.pawnsSpawned);
				this.allPawnsResult.AddRange(allPawnsUnspawned);
				return this.allPawnsResult;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x000442D4 File Offset: 0x000424D4
		public List<Pawn> AllPawnsUnspawned
		{
			get
			{
				this.allPawnsUnspawnedResult.Clear();
				ThingOwnerUtility.GetAllThingsRecursively<Pawn>(this.map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), this.allPawnsUnspawnedResult, true, null, false);
				for (int i = this.allPawnsUnspawnedResult.Count - 1; i >= 0; i--)
				{
					if (this.allPawnsUnspawnedResult[i].Dead)
					{
						this.allPawnsUnspawnedResult.RemoveAt(i);
					}
				}
				return this.allPawnsUnspawnedResult;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00044344 File Offset: 0x00042544
		public List<Pawn> FreeColonists
		{
			get
			{
				return this.FreeHumanlikesOfFaction(Faction.OfPlayer, false);
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x00044352 File Offset: 0x00042552
		public List<Pawn> FreeColonists_NoHusks
		{
			get
			{
				return this.FreeHumanlikesOfFaction(Faction.OfPlayer, true);
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x00044360 File Offset: 0x00042560
		public List<Pawn> PrisonersOfColony
		{
			get
			{
				this.prisonersOfColonyResult.Clear();
				List<Pawn> allPawns = this.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					if (allPawns[i].IsPrisonerOfColony)
					{
						this.prisonersOfColonyResult.Add(allPawns[i]);
					}
				}
				return this.prisonersOfColonyResult;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x000443B8 File Offset: 0x000425B8
		public List<Pawn> FreeColonistsAndPrisoners
		{
			get
			{
				List<Pawn> freeColonists = this.FreeColonists;
				List<Pawn> prisonersOfColony = this.PrisonersOfColony;
				if (prisonersOfColony.Count == 0)
				{
					return freeColonists;
				}
				this.freeColonistsAndPrisonersResult.Clear();
				this.freeColonistsAndPrisonersResult.AddRange(freeColonists);
				this.freeColonistsAndPrisonersResult.AddRange(prisonersOfColony);
				return this.freeColonistsAndPrisonersResult;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x00044408 File Offset: 0x00042608
		public int ColonistCount
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					Log.Error("ColonistCount while not playing. This should get the starting player pawn count.");
					return 3;
				}
				int num = 0;
				List<Pawn> allPawns = this.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					if (allPawns[i].IsColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x00044456 File Offset: 0x00042656
		public int AllPawnsCount
		{
			get
			{
				return this.AllPawns.Count;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00044463 File Offset: 0x00042663
		public int AllPawnsUnspawnedCount
		{
			get
			{
				return this.AllPawnsUnspawned.Count;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x00044470 File Offset: 0x00042670
		public int FreeColonistsCount
		{
			get
			{
				return this.FreeColonists.Count;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x0004447D File Offset: 0x0004267D
		public int PrisonersOfColonyCount
		{
			get
			{
				return this.PrisonersOfColony.Count;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x0004447D File Offset: 0x0004267D
		public int FreeColonistsAndPrisonersCount
		{
			get
			{
				return this.PrisonersOfColony.Count;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x0004448C File Offset: 0x0004268C
		public bool AnyPawnBlockingMapRemoval
		{
			get
			{
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < this.pawnsSpawned.Count; i++)
				{
					if (!this.pawnsSpawned[i].Downed && this.pawnsSpawned[i].IsColonist)
					{
						return true;
					}
					if (this.pawnsSpawned[i].relations != null && this.pawnsSpawned[i].relations.relativeInvolvedInRescueQuest != null)
					{
						return true;
					}
					if (this.pawnsSpawned[i].Faction == ofPlayer || this.pawnsSpawned[i].HostFaction == ofPlayer)
					{
						Job curJob = this.pawnsSpawned[i].CurJob;
						if (curJob != null && curJob.exitMapOnArrival)
						{
							return true;
						}
					}
					if (CaravanExitMapUtility.FindCaravanToJoinFor(this.pawnsSpawned[i]) != null && !this.pawnsSpawned[i].Downed)
					{
						return true;
					}
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int j = 0; j < list.Count; j++)
				{
					IThingHolder thingHolder = MapPawns.PlayerEjectablePodHolder(list[j], false);
					if (thingHolder != null)
					{
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(thingHolder, this.tmpThings, true, null);
						for (int k = 0; k < this.tmpThings.Count; k++)
						{
							Pawn pawn = this.tmpThings[k] as Pawn;
							if (pawn != null && !pawn.Dead && !pawn.Downed && pawn.IsColonist)
							{
								this.tmpThings.Clear();
								return true;
							}
						}
					}
				}
				this.tmpThings.Clear();
				return false;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00044641 File Offset: 0x00042841
		public List<Pawn> AllPawnsSpawned
		{
			get
			{
				return this.pawnsSpawned;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x00044649 File Offset: 0x00042849
		public List<Pawn> FreeColonistsSpawned
		{
			get
			{
				return this.FreeHumanlikesSpawnedOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x00044656 File Offset: 0x00042856
		public List<Pawn> PrisonersOfColonySpawned
		{
			get
			{
				return this.prisonersOfColonySpawned;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x0004465E File Offset: 0x0004285E
		public List<Pawn> SlavesOfColonySpawned
		{
			get
			{
				return this.slavesOfColonySpawned;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x00044668 File Offset: 0x00042868
		public List<Pawn> FreeColonistsAndPrisonersSpawned
		{
			get
			{
				List<Pawn> freeColonistsSpawned = this.FreeColonistsSpawned;
				List<Pawn> list = this.PrisonersOfColonySpawned;
				if (list.Count == 0)
				{
					return freeColonistsSpawned;
				}
				this.freeColonistsAndPrisonersSpawnedResult.Clear();
				this.freeColonistsAndPrisonersSpawnedResult.AddRange(freeColonistsSpawned);
				this.freeColonistsAndPrisonersSpawnedResult.AddRange(list);
				return this.freeColonistsAndPrisonersSpawnedResult;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x000446B8 File Offset: 0x000428B8
		public List<Pawn> SpawnedPawnsWithAnyHediff
		{
			get
			{
				this.spawnedPawnsWithAnyHediffResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].health.hediffSet.hediffs.Count != 0)
					{
						this.spawnedPawnsWithAnyHediffResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWithAnyHediffResult;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x00044720 File Offset: 0x00042920
		public List<Pawn> SpawnedHungryPawns
		{
			get
			{
				this.spawnedHungryPawnsResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (FeedPatientUtility.IsHungry(allPawnsSpawned[i]))
					{
						this.spawnedHungryPawnsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedHungryPawnsResult;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x00044778 File Offset: 0x00042978
		public List<Pawn> SpawnedPawnsWithMiscNeeds
		{
			get
			{
				this.spawnedPawnsWithMiscNeedsResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (!allPawnsSpawned[i].needs.MiscNeeds.NullOrEmpty<Need>())
					{
						this.spawnedPawnsWithMiscNeedsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWithMiscNeedsResult;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x000447D8 File Offset: 0x000429D8
		public List<Pawn> SpawnedColonyAnimals
		{
			get
			{
				this.spawnedColonyAnimalsResult.Clear();
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].RaceProps.Animal)
					{
						this.spawnedColonyAnimalsResult.Add(list[i]);
					}
				}
				return this.spawnedColonyAnimalsResult;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x00044838 File Offset: 0x00042A38
		public List<Pawn> SpawnedDownedPawns
		{
			get
			{
				this.spawnedDownedPawnsResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].Downed)
					{
						this.spawnedDownedPawnsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedDownedPawnsResult;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x00044890 File Offset: 0x00042A90
		public List<Pawn> SpawnedPawnsWhoShouldHaveSurgeryDoneNow
		{
			get
			{
				this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (HealthAIUtility.ShouldHaveSurgeryDoneNow(allPawnsSpawned[i]))
					{
						this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x000448E8 File Offset: 0x00042AE8
		public List<Pawn> SpawnedPawnsWhoShouldHaveInventoryUnloaded
		{
			get
			{
				this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].inventory.UnloadEverything)
					{
						this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x00044943 File Offset: 0x00042B43
		public int AllPawnsSpawnedCount
		{
			get
			{
				return this.pawnsSpawned.Count;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00044950 File Offset: 0x00042B50
		public int FreeColonistsSpawnedCount
		{
			get
			{
				return this.FreeColonistsSpawned.Count;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x0004495D File Offset: 0x00042B5D
		public int PrisonersOfColonySpawnedCount
		{
			get
			{
				return this.PrisonersOfColonySpawned.Count;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x0004496A File Offset: 0x00042B6A
		public int FreeColonistsAndPrisonersSpawnedCount
		{
			get
			{
				return this.FreeColonistsAndPrisonersSpawned.Count;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x00044978 File Offset: 0x00042B78
		public int ColonistsSpawnedCount
		{
			get
			{
				int num = 0;
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x000449B8 File Offset: 0x00042BB8
		public int FreeColonistsSpawnedOrInPlayerEjectablePodsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.pawnsSpawned.Count; i++)
				{
					if (this.pawnsSpawned[i].IsFreeColonist)
					{
						num++;
					}
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int j = 0; j < list.Count; j++)
				{
					IThingHolder thingHolder = MapPawns.PlayerEjectablePodHolder(list[j], true);
					if (thingHolder != null)
					{
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(thingHolder, this.tmpThings, true, null);
						for (int k = 0; k < this.tmpThings.Count; k++)
						{
							Pawn pawn = this.tmpThings[k] as Pawn;
							if (pawn != null && !pawn.Dead && pawn.IsFreeColonist)
							{
								num++;
							}
						}
					}
				}
				this.tmpThings.Clear();
				return num;
			}
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00044A9C File Offset: 0x00042C9C
		private static IThingHolder PlayerEjectablePodHolder(Thing thing, bool includeCryptosleepCaskets = true)
		{
			Building_CryptosleepCasket building_CryptosleepCasket = thing as Building_CryptosleepCasket;
			CompTransporter compTransporter = thing.TryGetComp<CompTransporter>();
			CompBiosculpterPod compBiosculpterPod = thing.TryGetComp<CompBiosculpterPod>();
			if ((includeCryptosleepCaskets && building_CryptosleepCasket != null && building_CryptosleepCasket.def.building.isPlayerEjectable) || thing is IActiveDropPod || thing is PawnFlyer || compTransporter != null || compBiosculpterPod != null)
			{
				IThingHolder thingHolder = compTransporter;
				IThingHolder result;
				if ((result = thingHolder) == null)
				{
					thingHolder = compBiosculpterPod;
					result = (thingHolder ?? (thing as IThingHolder));
				}
				return result;
			}
			return null;
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x00044B02 File Offset: 0x00042D02
		public int SlavesAndPrisonersOfColonySpawnedCount
		{
			get
			{
				return this.SlavesAndPrisonersOfColonySpawned.Count;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00044B10 File Offset: 0x00042D10
		public bool AnyColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonist)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x00044B4C File Offset: 0x00042D4C
		public bool AnyFreeColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsFreeColonist)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00044B87 File Offset: 0x00042D87
		public List<Pawn> SlavesAndPrisonersOfColonySpawned
		{
			get
			{
				this.slavesAndPrisonersOfColonySpawnedResult.Clear();
				this.slavesAndPrisonersOfColonySpawnedResult.AddRange(this.prisonersOfColonySpawned);
				this.slavesAndPrisonersOfColonySpawnedResult.AddRange(this.slavesOfColonySpawned);
				return this.slavesAndPrisonersOfColonySpawnedResult;
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00044BBC File Offset: 0x00042DBC
		public MapPawns(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00044CC0 File Offset: 0x00042EC0
		private void EnsureFactionsListsInit()
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (!this.pawnsInFactionSpawned.ContainsKey(allFactionsListForReading[i]))
				{
					this.pawnsInFactionSpawned.Add(allFactionsListForReading[i], new List<Pawn>());
				}
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00044D14 File Offset: 0x00042F14
		public List<Pawn> PawnsInFaction(Faction faction)
		{
			if (faction == null)
			{
				Log.Error("Called PawnsInFaction with null faction.");
				return new List<Pawn>();
			}
			List<Pawn> list;
			if (!this.pawnsInFactionResult.TryGetValue(faction, out list))
			{
				list = new List<Pawn>();
				this.pawnsInFactionResult.Add(faction, list);
			}
			list.Clear();
			List<Pawn> allPawns = this.AllPawns;
			for (int i = 0; i < allPawns.Count; i++)
			{
				if (allPawns[i].Faction == faction)
				{
					list.Add(allPawns[i]);
				}
			}
			return list;
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00044D92 File Offset: 0x00042F92
		public List<Pawn> SpawnedPawnsInFaction(Faction faction)
		{
			this.EnsureFactionsListsInit();
			if (faction == null)
			{
				Log.Error("Called SpawnedPawnsInFaction with null faction.");
				return new List<Pawn>();
			}
			return this.pawnsInFactionSpawned[faction];
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00044DBC File Offset: 0x00042FBC
		public List<Pawn> FreeHumanlikesOfFaction(Faction faction, bool excludeHusks = false)
		{
			List<Pawn> list;
			if (!this.freeHumanlikesOfFactionResult.TryGetValue(faction, out list))
			{
				list = new List<Pawn>();
				this.freeHumanlikesOfFactionResult.Add(faction, list);
			}
			list.Clear();
			List<Pawn> allPawns = this.AllPawns;
			for (int i = 0; i < allPawns.Count; i++)
			{
				if (allPawns[i].Faction == faction && (allPawns[i].HostFaction == null || allPawns[i].IsSlave) && allPawns[i].RaceProps.Humanlike)
				{
					list.Add(allPawns[i]);
				}
			}
			return list;
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00044E58 File Offset: 0x00043058
		public List<Pawn> FreeHumanlikesSpawnedOfFaction(Faction faction)
		{
			List<Pawn> list;
			if (!this.freeHumanlikesSpawnedOfFactionResult.TryGetValue(faction, out list))
			{
				list = new List<Pawn>();
				this.freeHumanlikesSpawnedOfFactionResult.Add(faction, list);
			}
			list.Clear();
			List<Pawn> list2 = this.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list2.Count; i++)
			{
				if (list2[i].HostFaction == null && list2[i].RaceProps.Humanlike)
				{
					list.Add(list2[i]);
				}
			}
			return list;
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00044ED8 File Offset: 0x000430D8
		public void RegisterPawn(Pawn p)
		{
			if (p.Dead)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to register dead pawn ",
					p,
					" in ",
					base.GetType(),
					"."
				}));
				return;
			}
			if (!p.Spawned)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to register despawned pawn ",
					p,
					" in ",
					base.GetType(),
					"."
				}));
				return;
			}
			if (p.Map != this.map)
			{
				Log.Warning("Tried to register pawn " + p + " but his Map is not this one.");
				return;
			}
			if (!p.mindState.Active)
			{
				return;
			}
			this.EnsureFactionsListsInit();
			if (!this.pawnsSpawned.Contains(p))
			{
				this.pawnsSpawned.Add(p);
			}
			if (p.Faction != null && !this.pawnsInFactionSpawned[p.Faction].Contains(p))
			{
				this.pawnsInFactionSpawned[p.Faction].Add(p);
				if (p.Faction == Faction.OfPlayer)
				{
					this.pawnsInFactionSpawned[Faction.OfPlayer].InsertionSort(delegate(Pawn a, Pawn b)
					{
						int num = (a.playerSettings != null) ? a.playerSettings.joinTick : 0;
						int value = (b.playerSettings != null) ? b.playerSettings.joinTick : 0;
						return num.CompareTo(value);
					});
				}
			}
			if (p.IsPrisonerOfColony && !this.prisonersOfColonySpawned.Contains(p))
			{
				this.prisonersOfColonySpawned.Add(p);
			}
			if (p.IsSlaveOfColony && !this.slavesOfColonySpawned.Contains(p))
			{
				this.slavesOfColonySpawned.Add(p);
			}
			this.DoListChangedNotifications();
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00045078 File Offset: 0x00043278
		public void DeRegisterPawn(Pawn p)
		{
			this.EnsureFactionsListsInit();
			this.pawnsSpawned.Remove(p);
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				Faction key = allFactionsListForReading[i];
				this.pawnsInFactionSpawned[key].Remove(p);
			}
			this.prisonersOfColonySpawned.Remove(p);
			this.slavesOfColonySpawned.Remove(p);
			this.DoListChangedNotifications();
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000450EF File Offset: 0x000432EF
		public void UpdateRegistryForPawn(Pawn p)
		{
			this.DeRegisterPawn(p);
			if (p.Spawned && p.Map == this.map)
			{
				this.RegisterPawn(p);
			}
			this.DoListChangedNotifications();
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0004511B File Offset: 0x0004331B
		private void DoListChangedNotifications()
		{
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
			if (Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00045134 File Offset: 0x00043334
		public void LogListedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MapPawns:");
			stringBuilder.AppendLine("pawnsSpawned");
			foreach (Pawn pawn in this.pawnsSpawned)
			{
				stringBuilder.AppendLine("    " + pawn.ToString());
			}
			stringBuilder.AppendLine("AllPawnsUnspawned");
			foreach (Pawn pawn2 in this.AllPawnsUnspawned)
			{
				stringBuilder.AppendLine("    " + pawn2.ToString());
			}
			foreach (KeyValuePair<Faction, List<Pawn>> keyValuePair in this.pawnsInFactionSpawned)
			{
				stringBuilder.AppendLine("pawnsInFactionSpawned[" + keyValuePair.Key.ToString() + "]");
				foreach (Pawn pawn3 in keyValuePair.Value)
				{
					stringBuilder.AppendLine("    " + pawn3.ToString());
				}
			}
			stringBuilder.AppendLine("prisonersOfColonySpawned");
			foreach (Pawn pawn4 in this.prisonersOfColonySpawned)
			{
				stringBuilder.AppendLine("    " + pawn4.ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04000A5E RID: 2654
		private Map map;

		// Token: 0x04000A5F RID: 2655
		private List<Pawn> pawnsSpawned = new List<Pawn>();

		// Token: 0x04000A60 RID: 2656
		private Dictionary<Faction, List<Pawn>> pawnsInFactionSpawned = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x04000A61 RID: 2657
		private List<Pawn> prisonersOfColonySpawned = new List<Pawn>();

		// Token: 0x04000A62 RID: 2658
		private List<Pawn> slavesOfColonySpawned = new List<Pawn>();

		// Token: 0x04000A63 RID: 2659
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04000A64 RID: 2660
		private List<Pawn> allPawnsResult = new List<Pawn>();

		// Token: 0x04000A65 RID: 2661
		private List<Pawn> allPawnsUnspawnedResult = new List<Pawn>();

		// Token: 0x04000A66 RID: 2662
		private List<Pawn> prisonersOfColonyResult = new List<Pawn>();

		// Token: 0x04000A67 RID: 2663
		private List<Pawn> freeColonistsAndPrisonersResult = new List<Pawn>();

		// Token: 0x04000A68 RID: 2664
		private List<Pawn> freeColonistsAndPrisonersSpawnedResult = new List<Pawn>();

		// Token: 0x04000A69 RID: 2665
		private List<Pawn> spawnedPawnsWithAnyHediffResult = new List<Pawn>();

		// Token: 0x04000A6A RID: 2666
		private List<Pawn> spawnedHungryPawnsResult = new List<Pawn>();

		// Token: 0x04000A6B RID: 2667
		private List<Pawn> spawnedPawnsWithMiscNeedsResult = new List<Pawn>();

		// Token: 0x04000A6C RID: 2668
		private List<Pawn> spawnedColonyAnimalsResult = new List<Pawn>();

		// Token: 0x04000A6D RID: 2669
		private List<Pawn> spawnedDownedPawnsResult = new List<Pawn>();

		// Token: 0x04000A6E RID: 2670
		private List<Pawn> spawnedPawnsWhoShouldHaveSurgeryDoneNowResult = new List<Pawn>();

		// Token: 0x04000A6F RID: 2671
		private List<Pawn> spawnedPawnsWhoShouldHaveInventoryUnloadedResult = new List<Pawn>();

		// Token: 0x04000A70 RID: 2672
		private List<Pawn> slavesAndPrisonersOfColonySpawnedResult = new List<Pawn>();

		// Token: 0x04000A71 RID: 2673
		private Dictionary<Faction, List<Pawn>> pawnsInFactionResult = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x04000A72 RID: 2674
		private Dictionary<Faction, List<Pawn>> freeHumanlikesOfFactionResult = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x04000A73 RID: 2675
		private Dictionary<Faction, List<Pawn>> freeHumanlikesSpawnedOfFactionResult = new Dictionary<Faction, List<Pawn>>();
	}
}
