using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200027C RID: 636
	public sealed class MapPawns
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x000B97C8 File Offset: 0x000B79C8
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

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06001062 RID: 4194 RVA: 0x000B981C File Offset: 0x000B7A1C
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

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001063 RID: 4195 RVA: 0x0001230B File Offset: 0x0001050B
		public List<Pawn> FreeColonists
		{
			get
			{
				return this.FreeHumanlikesOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001064 RID: 4196 RVA: 0x000B988C File Offset: 0x000B7A8C
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

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06001065 RID: 4197 RVA: 0x000B98E4 File Offset: 0x000B7AE4
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

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06001066 RID: 4198 RVA: 0x000B9934 File Offset: 0x000B7B34
		public int ColonistCount
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					Log.Error("ColonistCount while not playing. This should get the starting player pawn count.", false);
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

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06001067 RID: 4199 RVA: 0x00012318 File Offset: 0x00010518
		public int AllPawnsCount
		{
			get
			{
				return this.AllPawns.Count;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x00012325 File Offset: 0x00010525
		public int AllPawnsUnspawnedCount
		{
			get
			{
				return this.AllPawnsUnspawned.Count;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x00012332 File Offset: 0x00010532
		public int FreeColonistsCount
		{
			get
			{
				return this.FreeColonists.Count;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600106A RID: 4202 RVA: 0x0001233F File Offset: 0x0001053F
		public int PrisonersOfColonyCount
		{
			get
			{
				return this.PrisonersOfColony.Count;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600106B RID: 4203 RVA: 0x0001233F File Offset: 0x0001053F
		public int FreeColonistsAndPrisonersCount
		{
			get
			{
				return this.PrisonersOfColony.Count;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x000B9984 File Offset: 0x000B7B84
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
					if (list[j] is IActiveDropPod || list[j] is PawnFlyer || list[j].TryGetComp<CompTransporter>() != null)
					{
						IThingHolder thingHolder = list[j].TryGetComp<CompTransporter>();
						IThingHolder holder = thingHolder ?? ((IThingHolder)list[j]);
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(holder, this.tmpThings, true, null);
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

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x0001234C File Offset: 0x0001054C
		public List<Pawn> AllPawnsSpawned
		{
			get
			{
				return this.pawnsSpawned;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x0600106E RID: 4206 RVA: 0x00012354 File Offset: 0x00010554
		public List<Pawn> FreeColonistsSpawned
		{
			get
			{
				return this.FreeHumanlikesSpawnedOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x00012361 File Offset: 0x00010561
		public List<Pawn> PrisonersOfColonySpawned
		{
			get
			{
				return this.prisonersOfColonySpawned;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x000B9B78 File Offset: 0x000B7D78
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

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x000B9BC8 File Offset: 0x000B7DC8
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

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x000B9C30 File Offset: 0x000B7E30
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

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x000B9C88 File Offset: 0x000B7E88
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

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x000B9CE0 File Offset: 0x000B7EE0
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

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x000B9D38 File Offset: 0x000B7F38
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

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x00012369 File Offset: 0x00010569
		public int AllPawnsSpawnedCount
		{
			get
			{
				return this.pawnsSpawned.Count;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x00012376 File Offset: 0x00010576
		public int FreeColonistsSpawnedCount
		{
			get
			{
				return this.FreeColonistsSpawned.Count;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x00012383 File Offset: 0x00010583
		public int PrisonersOfColonySpawnedCount
		{
			get
			{
				return this.PrisonersOfColonySpawned.Count;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x00012390 File Offset: 0x00010590
		public int FreeColonistsAndPrisonersSpawnedCount
		{
			get
			{
				return this.FreeColonistsAndPrisonersSpawned.Count;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x000B9D94 File Offset: 0x000B7F94
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

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x000B9DD4 File Offset: 0x000B7FD4
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
					Building_CryptosleepCasket building_CryptosleepCasket = list[j] as Building_CryptosleepCasket;
					if ((building_CryptosleepCasket != null && building_CryptosleepCasket.def.building.isPlayerEjectable) || list[j] is IActiveDropPod || list[j] is PawnFlyer || list[j].TryGetComp<CompTransporter>() != null)
					{
						IThingHolder thingHolder = list[j].TryGetComp<CompTransporter>();
						IThingHolder holder = thingHolder ?? ((IThingHolder)list[j]);
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(holder, this.tmpThings, true, null);
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

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x000B9F18 File Offset: 0x000B8118
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

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x000B9F54 File Offset: 0x000B8154
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

		// Token: 0x0600107E RID: 4222 RVA: 0x000B9F90 File Offset: 0x000B8190
		public MapPawns(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x000BA068 File Offset: 0x000B8268
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

		// Token: 0x06001080 RID: 4224 RVA: 0x000BA0BC File Offset: 0x000B82BC
		public List<Pawn> PawnsInFaction(Faction faction)
		{
			if (faction == null)
			{
				Log.Error("Called PawnsInFaction with null faction.", false);
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

		// Token: 0x06001081 RID: 4225 RVA: 0x0001239D File Offset: 0x0001059D
		public List<Pawn> SpawnedPawnsInFaction(Faction faction)
		{
			this.EnsureFactionsListsInit();
			if (faction == null)
			{
				Log.Error("Called SpawnedPawnsInFaction with null faction.", false);
				return new List<Pawn>();
			}
			return this.pawnsInFactionSpawned[faction];
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x000BA13C File Offset: 0x000B833C
		public List<Pawn> FreeHumanlikesOfFaction(Faction faction)
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
				if (allPawns[i].Faction == faction && allPawns[i].HostFaction == null && allPawns[i].RaceProps.Humanlike)
				{
					list.Add(allPawns[i]);
				}
			}
			return list;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x000BA1C8 File Offset: 0x000B83C8
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

		// Token: 0x06001084 RID: 4228 RVA: 0x000BA248 File Offset: 0x000B8448
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
				}), false);
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
				}), false);
				return;
			}
			if (p.Map != this.map)
			{
				Log.Warning("Tried to register pawn " + p + " but his Map is not this one.", false);
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
			this.DoListChangedNotifications();
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x000BA3C8 File Offset: 0x000B85C8
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
			this.DoListChangedNotifications();
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x000123C5 File Offset: 0x000105C5
		public void UpdateRegistryForPawn(Pawn p)
		{
			this.DeRegisterPawn(p);
			if (p.Spawned && p.Map == this.map)
			{
				this.RegisterPawn(p);
			}
			this.DoListChangedNotifications();
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x000123F1 File Offset: 0x000105F1
		private void DoListChangedNotifications()
		{
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
			if (Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x000BA434 File Offset: 0x000B8634
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
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04000D30 RID: 3376
		private Map map;

		// Token: 0x04000D31 RID: 3377
		private List<Pawn> pawnsSpawned = new List<Pawn>();

		// Token: 0x04000D32 RID: 3378
		private Dictionary<Faction, List<Pawn>> pawnsInFactionSpawned = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x04000D33 RID: 3379
		private List<Pawn> prisonersOfColonySpawned = new List<Pawn>();

		// Token: 0x04000D34 RID: 3380
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04000D35 RID: 3381
		private List<Pawn> allPawnsResult = new List<Pawn>();

		// Token: 0x04000D36 RID: 3382
		private List<Pawn> allPawnsUnspawnedResult = new List<Pawn>();

		// Token: 0x04000D37 RID: 3383
		private List<Pawn> prisonersOfColonyResult = new List<Pawn>();

		// Token: 0x04000D38 RID: 3384
		private List<Pawn> freeColonistsAndPrisonersResult = new List<Pawn>();

		// Token: 0x04000D39 RID: 3385
		private List<Pawn> freeColonistsAndPrisonersSpawnedResult = new List<Pawn>();

		// Token: 0x04000D3A RID: 3386
		private List<Pawn> spawnedPawnsWithAnyHediffResult = new List<Pawn>();

		// Token: 0x04000D3B RID: 3387
		private List<Pawn> spawnedHungryPawnsResult = new List<Pawn>();

		// Token: 0x04000D3C RID: 3388
		private List<Pawn> spawnedDownedPawnsResult = new List<Pawn>();

		// Token: 0x04000D3D RID: 3389
		private List<Pawn> spawnedPawnsWhoShouldHaveSurgeryDoneNowResult = new List<Pawn>();

		// Token: 0x04000D3E RID: 3390
		private List<Pawn> spawnedPawnsWhoShouldHaveInventoryUnloadedResult = new List<Pawn>();

		// Token: 0x04000D3F RID: 3391
		private Dictionary<Faction, List<Pawn>> pawnsInFactionResult = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x04000D40 RID: 3392
		private Dictionary<Faction, List<Pawn>> freeHumanlikesOfFactionResult = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x04000D41 RID: 3393
		private Dictionary<Faction, List<Pawn>> freeHumanlikesSpawnedOfFactionResult = new Dictionary<Faction, List<Pawn>>();
	}
}
