using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD5 RID: 3541
	public static class PawnsFinder
	{
		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x0600521D RID: 21021 RVA: 0x001BB198 File Offset: 0x001B9398
		public static List<Pawn> AllMapsWorldAndTemporary_AliveOrDead
		{
			get
			{
				PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.Clear();
				PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(PawnsFinder.AllMapsWorldAndTemporary_Alive);
				if (Find.World != null)
				{
					PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(Find.WorldPawns.AllPawnsDead);
				}
				PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(PawnsFinder.Temporary_Dead);
				return PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result;
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x0600521E RID: 21022 RVA: 0x001BB1F0 File Offset: 0x001B93F0
		public static List<Pawn> AllMapsWorldAndTemporary_Alive
		{
			get
			{
				PawnsFinder.allMapsWorldAndTemporary_Alive_Result.Clear();
				PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(PawnsFinder.AllMaps);
				if (Find.World != null)
				{
					PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(Find.WorldPawns.AllPawnsAlive);
				}
				PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(PawnsFinder.Temporary_Alive);
				return PawnsFinder.allMapsWorldAndTemporary_Alive_Result;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x0600521F RID: 21023 RVA: 0x001BB245 File Offset: 0x001B9445
		public static List<Pawn> AllMapsAndWorld_Alive
		{
			get
			{
				PawnsFinder.allMapsAndWorld_Alive_Result.Clear();
				PawnsFinder.allMapsAndWorld_Alive_Result.AddRange(PawnsFinder.AllMaps);
				if (Find.World != null)
				{
					PawnsFinder.allMapsAndWorld_Alive_Result.AddRange(Find.WorldPawns.AllPawnsAlive);
				}
				return PawnsFinder.allMapsAndWorld_Alive_Result;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06005220 RID: 21024 RVA: 0x001BB280 File Offset: 0x001B9480
		public static List<Pawn> AllMaps
		{
			get
			{
				PawnsFinder.allMaps_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.AllPawns;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_Result.AddRange(maps[i].mapPawns.AllPawns);
					}
				}
				return PawnsFinder.allMaps_Result;
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06005221 RID: 21025 RVA: 0x001BB2F0 File Offset: 0x001B94F0
		public static List<Pawn> AllMaps_Spawned
		{
			get
			{
				PawnsFinder.allMaps_Spawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.AllPawnsSpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_Spawned_Result.AddRange(maps[i].mapPawns.AllPawnsSpawned);
					}
				}
				return PawnsFinder.allMaps_Spawned_Result;
			}
		}

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06005222 RID: 21026 RVA: 0x001BB360 File Offset: 0x001B9560
		public static List<Pawn> All_AliveOrDead
		{
			get
			{
				List<Pawn> allMapsWorldAndTemporary_AliveOrDead = PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead;
				List<Pawn> allCaravansAndTravelingTransportPods_AliveOrDead = PawnsFinder.AllCaravansAndTravelingTransportPods_AliveOrDead;
				if (allCaravansAndTravelingTransportPods_AliveOrDead.Count == 0)
				{
					return allMapsWorldAndTemporary_AliveOrDead;
				}
				PawnsFinder.all_AliveOrDead_Result.Clear();
				PawnsFinder.all_AliveOrDead_Result.AddRange(allMapsWorldAndTemporary_AliveOrDead);
				PawnsFinder.all_AliveOrDead_Result.AddRange(allCaravansAndTravelingTransportPods_AliveOrDead);
				return PawnsFinder.all_AliveOrDead_Result;
			}
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06005223 RID: 21027 RVA: 0x001BB3A8 File Offset: 0x001B95A8
		public static List<Pawn> Temporary
		{
			get
			{
				PawnsFinder.temporary_Result.Clear();
				List<List<Pawn>> pawnsBeingGeneratedNow = PawnGroupKindWorker.pawnsBeingGeneratedNow;
				for (int i = 0; i < pawnsBeingGeneratedNow.Count; i++)
				{
					PawnsFinder.temporary_Result.AddRange(pawnsBeingGeneratedNow[i]);
				}
				List<List<Thing>> thingsBeingGeneratedNow = ThingSetMaker.thingsBeingGeneratedNow;
				for (int j = 0; j < thingsBeingGeneratedNow.Count; j++)
				{
					List<Thing> list = thingsBeingGeneratedNow[j];
					for (int k = 0; k < list.Count; k++)
					{
						Pawn pawn = list[k] as Pawn;
						if (pawn != null)
						{
							PawnsFinder.temporary_Result.Add(pawn);
						}
					}
				}
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null)
				{
					List<Pawn> startingAndOptionalPawns = Find.GameInitData.startingAndOptionalPawns;
					for (int l = 0; l < startingAndOptionalPawns.Count; l++)
					{
						if (startingAndOptionalPawns[l] != null)
						{
							PawnsFinder.temporary_Result.Add(startingAndOptionalPawns[l]);
						}
					}
				}
				if (Find.World != null)
				{
					List<Site> sites = Find.WorldObjects.Sites;
					for (int m = 0; m < sites.Count; m++)
					{
						for (int n = 0; n < sites[m].parts.Count; n++)
						{
							if (sites[m].parts[n].things != null && sites[m].parts[n].things.contentsLookMode == LookMode.Deep)
							{
								ThingOwner things = sites[m].parts[n].things;
								for (int num = 0; num < things.Count; num++)
								{
									Pawn pawn2 = things[num] as Pawn;
									if (pawn2 != null)
									{
										PawnsFinder.temporary_Result.Add(pawn2);
									}
								}
							}
						}
					}
				}
				if (Find.World != null)
				{
					List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
					for (int num2 = 0; num2 < allWorldObjects.Count; num2++)
					{
						DownedRefugeeComp component = allWorldObjects[num2].GetComponent<DownedRefugeeComp>();
						if (component != null && component.pawn != null && component.pawn.Any)
						{
							PawnsFinder.temporary_Result.Add(component.pawn[0]);
						}
						PrisonerWillingToJoinComp component2 = allWorldObjects[num2].GetComponent<PrisonerWillingToJoinComp>();
						if (component2 != null && component2.pawn != null && component2.pawn.Any)
						{
							PawnsFinder.temporary_Result.Add(component2.pawn[0]);
						}
					}
				}
				return PawnsFinder.temporary_Result;
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06005224 RID: 21028 RVA: 0x001BB634 File Offset: 0x001B9834
		public static List<Pawn> Temporary_Alive
		{
			get
			{
				PawnsFinder.temporary_Alive_Result.Clear();
				List<Pawn> temporary = PawnsFinder.Temporary;
				for (int i = 0; i < temporary.Count; i++)
				{
					if (!temporary[i].Dead)
					{
						PawnsFinder.temporary_Alive_Result.Add(temporary[i]);
					}
				}
				return PawnsFinder.temporary_Alive_Result;
			}
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06005225 RID: 21029 RVA: 0x001BB688 File Offset: 0x001B9888
		public static List<Pawn> Temporary_Dead
		{
			get
			{
				PawnsFinder.temporary_Dead_Result.Clear();
				List<Pawn> temporary = PawnsFinder.Temporary;
				for (int i = 0; i < temporary.Count; i++)
				{
					if (temporary[i].Dead)
					{
						PawnsFinder.temporary_Dead_Result.Add(temporary[i]);
					}
				}
				return PawnsFinder.temporary_Dead_Result;
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x06005226 RID: 21030 RVA: 0x001BB6DC File Offset: 0x001B98DC
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive
		{
			get
			{
				List<Pawn> allMaps = PawnsFinder.AllMaps;
				List<Pawn> allCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllCaravansAndTravelingTransportPods_Alive;
				if (allCaravansAndTravelingTransportPods_Alive.Count == 0)
				{
					return allMaps;
				}
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Result.Clear();
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Result.AddRange(allMaps);
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Result.AddRange(allCaravansAndTravelingTransportPods_Alive);
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Result;
			}
		}

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x06005227 RID: 21031 RVA: 0x001BB724 File Offset: 0x001B9924
		public static List<Pawn> AllCaravansAndTravelingTransportPods_Alive
		{
			get
			{
				PawnsFinder.allCaravansAndTravelingTransportPods_Alive_Result.Clear();
				List<Pawn> allCaravansAndTravelingTransportPods_AliveOrDead = PawnsFinder.AllCaravansAndTravelingTransportPods_AliveOrDead;
				for (int i = 0; i < allCaravansAndTravelingTransportPods_AliveOrDead.Count; i++)
				{
					if (!allCaravansAndTravelingTransportPods_AliveOrDead[i].Dead)
					{
						PawnsFinder.allCaravansAndTravelingTransportPods_Alive_Result.Add(allCaravansAndTravelingTransportPods_AliveOrDead[i]);
					}
				}
				return PawnsFinder.allCaravansAndTravelingTransportPods_Alive_Result;
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06005228 RID: 21032 RVA: 0x001BB778 File Offset: 0x001B9978
		public static List<Pawn> AllCaravansAndTravelingTransportPods_AliveOrDead
		{
			get
			{
				PawnsFinder.allCaravansAndTravelingTransportPods_AliveOrDead_Result.Clear();
				if (Find.World != null)
				{
					List<Caravan> caravans = Find.WorldObjects.Caravans;
					for (int i = 0; i < caravans.Count; i++)
					{
						PawnsFinder.allCaravansAndTravelingTransportPods_AliveOrDead_Result.AddRange(caravans[i].PawnsListForReading);
					}
					List<TravelingTransportPods> travelingTransportPods = Find.WorldObjects.TravelingTransportPods;
					for (int j = 0; j < travelingTransportPods.Count; j++)
					{
						PawnsFinder.allCaravansAndTravelingTransportPods_AliveOrDead_Result.AddRange(travelingTransportPods[j].Pawns);
					}
				}
				return PawnsFinder.allCaravansAndTravelingTransportPods_AliveOrDead_Result;
			}
		}

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06005229 RID: 21033 RVA: 0x001BB800 File Offset: 0x001B9A00
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_Colonists
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Colonists_Result.Clear();
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].IsColonist)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Colonists_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Colonists_Result;
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x0600522A RID: 21034 RVA: 0x001BB854 File Offset: 0x001B9A54
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_Result.Clear();
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].IsFreeColonist)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_Result;
			}
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x0600522B RID: 21035 RVA: 0x001BB8A8 File Offset: 0x001B9AA8
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoLodgers
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoLodgers_Result.Clear();
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].IsFreeColonist && !allMapsCaravansAndTravelingTransportPods_Alive[i].IsQuestLodger())
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoLodgers_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoLodgers_Result;
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x0600522C RID: 21036 RVA: 0x001BB908 File Offset: 0x001B9B08
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep_Result.Clear();
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].IsFreeColonist && !allMapsCaravansAndTravelingTransportPods_Alive[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep_Result;
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x0600522D RID: 21037 RVA: 0x001BB968 File Offset: 0x001B9B68
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_Result.Clear();
				Faction ofPlayer = Faction.OfPlayer;
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].Faction == ofPlayer)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_Result;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x0600522E RID: 21038 RVA: 0x001BB9C4 File Offset: 0x001B9BC4
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep_Result.Clear();
				Faction ofPlayer = Faction.OfPlayer;
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].Faction == ofPlayer && !allMapsCaravansAndTravelingTransportPods_Alive[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep_Result;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x0600522F RID: 21039 RVA: 0x001BBA2C File Offset: 0x001B9C2C
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony_Result.Clear();
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].IsPrisonerOfColony)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony_Result;
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06005230 RID: 21040 RVA: 0x001BBA80 File Offset: 0x001B9C80
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners
		{
			get
			{
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists;
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony;
				if (allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count == 0)
				{
					return allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists;
				}
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result.Clear();
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result.AddRange(allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists);
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result.AddRange(allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony);
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result;
			}
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06005231 RID: 21041 RVA: 0x001BBAC8 File Offset: 0x001B9CC8
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result.Clear();
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners.Count; i++)
				{
					if (!allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result;
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06005232 RID: 21042 RVA: 0x001BBB1C File Offset: 0x001B9D1C
		public static List<Pawn> AllMaps_PrisonersOfColonySpawned
		{
			get
			{
				PawnsFinder.allMaps_PrisonersOfColonySpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.PrisonersOfColonySpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_PrisonersOfColonySpawned_Result.AddRange(maps[i].mapPawns.PrisonersOfColonySpawned);
					}
				}
				return PawnsFinder.allMaps_PrisonersOfColonySpawned_Result;
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06005233 RID: 21043 RVA: 0x001BBB8C File Offset: 0x001B9D8C
		public static List<Pawn> AllMaps_PrisonersOfColony
		{
			get
			{
				PawnsFinder.allMaps_PrisonersOfColony_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.PrisonersOfColony;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_PrisonersOfColony_Result.AddRange(maps[i].mapPawns.PrisonersOfColony);
					}
				}
				return PawnsFinder.allMaps_PrisonersOfColony_Result;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06005234 RID: 21044 RVA: 0x001BBBFC File Offset: 0x001B9DFC
		public static List<Pawn> AllMaps_FreeColonists
		{
			get
			{
				PawnsFinder.allMaps_FreeColonists_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonists;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonists_Result.AddRange(maps[i].mapPawns.FreeColonists);
					}
				}
				return PawnsFinder.allMaps_FreeColonists_Result;
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06005235 RID: 21045 RVA: 0x001BBC6C File Offset: 0x001B9E6C
		public static List<Pawn> AllMaps_FreeColonistsSpawned
		{
			get
			{
				PawnsFinder.allMaps_FreeColonistsSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonistsSpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonistsSpawned_Result.AddRange(maps[i].mapPawns.FreeColonistsSpawned);
					}
				}
				return PawnsFinder.allMaps_FreeColonistsSpawned_Result;
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06005236 RID: 21046 RVA: 0x001BBCDC File Offset: 0x001B9EDC
		public static List<Pawn> AllMaps_FreeColonistsAndPrisonersSpawned
		{
			get
			{
				PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonistsAndPrisonersSpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result.AddRange(maps[i].mapPawns.FreeColonistsAndPrisonersSpawned);
					}
				}
				return PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result;
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06005237 RID: 21047 RVA: 0x001BBD4C File Offset: 0x001B9F4C
		public static List<Pawn> AllMaps_FreeColonistsAndPrisoners
		{
			get
			{
				PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonistsAndPrisoners;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result.AddRange(maps[i].mapPawns.FreeColonistsAndPrisoners);
					}
				}
				return PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result;
			}
		}

		// Token: 0x06005238 RID: 21048 RVA: 0x001BBDBC File Offset: 0x001B9FBC
		public static List<Pawn> AllMaps_SpawnedPawnsInFaction(Faction faction)
		{
			List<Pawn> list;
			if (!PawnsFinder.allMaps_SpawnedPawnsInFaction_Result.TryGetValue(faction, out list))
			{
				list = new List<Pawn>();
				PawnsFinder.allMaps_SpawnedPawnsInFaction_Result.Add(faction, list);
			}
			list.Clear();
			if (Current.ProgramState != ProgramState.Entry)
			{
				List<Map> maps = Find.Maps;
				if (maps.Count == 1)
				{
					return maps[0].mapPawns.SpawnedPawnsInFaction(faction);
				}
				for (int i = 0; i < maps.Count; i++)
				{
					list.AddRange(maps[i].mapPawns.SpawnedPawnsInFaction(faction));
				}
			}
			return list;
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06005239 RID: 21049 RVA: 0x001BBE44 File Offset: 0x001BA044
		public static List<Pawn> HomeMaps_FreeColonistsSpawned
		{
			get
			{
				PawnsFinder.homeMaps_FreeColonistsSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						if (!maps[0].IsPlayerHome)
						{
							return PawnsFinder.homeMaps_FreeColonistsSpawned_Result;
						}
						return maps[0].mapPawns.FreeColonistsSpawned;
					}
					else
					{
						for (int i = 0; i < maps.Count; i++)
						{
							if (maps[i].IsPlayerHome)
							{
								PawnsFinder.homeMaps_FreeColonistsSpawned_Result.AddRange(maps[i].mapPawns.FreeColonistsSpawned);
							}
						}
					}
				}
				return PawnsFinder.homeMaps_FreeColonistsSpawned_Result;
			}
		}

		// Token: 0x0600523A RID: 21050 RVA: 0x001BBED8 File Offset: 0x001BA0D8
		public static void Clear()
		{
			PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.Clear();
			PawnsFinder.allMapsWorldAndTemporary_Alive_Result.Clear();
			PawnsFinder.allMapsAndWorld_Alive_Result.Clear();
			PawnsFinder.allMaps_Result.Clear();
			PawnsFinder.allMaps_Spawned_Result.Clear();
			PawnsFinder.all_AliveOrDead_Result.Clear();
			PawnsFinder.temporary_Result.Clear();
			PawnsFinder.temporary_Alive_Result.Clear();
			PawnsFinder.temporary_Dead_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Result.Clear();
			PawnsFinder.allCaravansAndTravelingTransportPods_Alive_Result.Clear();
			PawnsFinder.allCaravansAndTravelingTransportPods_AliveOrDead_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Colonists_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result.Clear();
			PawnsFinder.allMaps_PrisonersOfColonySpawned_Result.Clear();
			PawnsFinder.allMaps_PrisonersOfColony_Result.Clear();
			PawnsFinder.allMaps_FreeColonists_Result.Clear();
			PawnsFinder.allMaps_FreeColonistsSpawned_Result.Clear();
			PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result.Clear();
			PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result.Clear();
			PawnsFinder.allMaps_SpawnedPawnsInFaction_Result.Clear();
			PawnsFinder.homeMaps_FreeColonistsSpawned_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoLodgers_Result.Clear();
		}

		// Token: 0x0400307A RID: 12410
		private static List<Pawn> allMapsWorldAndTemporary_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x0400307B RID: 12411
		private static List<Pawn> allMapsWorldAndTemporary_Alive_Result = new List<Pawn>();

		// Token: 0x0400307C RID: 12412
		private static List<Pawn> allMapsAndWorld_Alive_Result = new List<Pawn>();

		// Token: 0x0400307D RID: 12413
		private static List<Pawn> allMaps_Result = new List<Pawn>();

		// Token: 0x0400307E RID: 12414
		private static List<Pawn> allMaps_Spawned_Result = new List<Pawn>();

		// Token: 0x0400307F RID: 12415
		private static List<Pawn> all_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x04003080 RID: 12416
		private static List<Pawn> temporary_Result = new List<Pawn>();

		// Token: 0x04003081 RID: 12417
		private static List<Pawn> temporary_Alive_Result = new List<Pawn>();

		// Token: 0x04003082 RID: 12418
		private static List<Pawn> temporary_Dead_Result = new List<Pawn>();

		// Token: 0x04003083 RID: 12419
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_Result = new List<Pawn>();

		// Token: 0x04003084 RID: 12420
		private static List<Pawn> allCaravansAndTravelingTransportPods_Alive_Result = new List<Pawn>();

		// Token: 0x04003085 RID: 12421
		private static List<Pawn> allCaravansAndTravelingTransportPods_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x04003086 RID: 12422
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_Colonists_Result = new List<Pawn>();

		// Token: 0x04003087 RID: 12423
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_Result = new List<Pawn>();

		// Token: 0x04003088 RID: 12424
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoLodgers_Result = new List<Pawn>();

		// Token: 0x04003089 RID: 12425
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x0400308A RID: 12426
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_Result = new List<Pawn>();

		// Token: 0x0400308B RID: 12427
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x0400308C RID: 12428
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony_Result = new List<Pawn>();

		// Token: 0x0400308D RID: 12429
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		// Token: 0x0400308E RID: 12430
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x0400308F RID: 12431
		private static List<Pawn> allMaps_PrisonersOfColonySpawned_Result = new List<Pawn>();

		// Token: 0x04003090 RID: 12432
		private static List<Pawn> allMaps_PrisonersOfColony_Result = new List<Pawn>();

		// Token: 0x04003091 RID: 12433
		private static List<Pawn> allMaps_FreeColonists_Result = new List<Pawn>();

		// Token: 0x04003092 RID: 12434
		private static List<Pawn> allMaps_FreeColonistsSpawned_Result = new List<Pawn>();

		// Token: 0x04003093 RID: 12435
		private static List<Pawn> allMaps_FreeColonistsAndPrisonersSpawned_Result = new List<Pawn>();

		// Token: 0x04003094 RID: 12436
		private static List<Pawn> allMaps_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		// Token: 0x04003095 RID: 12437
		private static Dictionary<Faction, List<Pawn>> allMaps_SpawnedPawnsInFaction_Result = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x04003096 RID: 12438
		private static List<Pawn> homeMaps_FreeColonistsSpawned_Result = new List<Pawn>();
	}
}
