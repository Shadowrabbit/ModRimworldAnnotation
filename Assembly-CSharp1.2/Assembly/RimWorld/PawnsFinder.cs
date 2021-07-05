using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001443 RID: 5187
	public static class PawnsFinder
	{
		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x06006FDB RID: 28635 RVA: 0x00223E60 File Offset: 0x00222060
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

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x06006FDC RID: 28636 RVA: 0x00223EB8 File Offset: 0x002220B8
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

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x06006FDD RID: 28637 RVA: 0x0004B82D File Offset: 0x00049A2D
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

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x06006FDE RID: 28638 RVA: 0x00223F10 File Offset: 0x00222110
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

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x06006FDF RID: 28639 RVA: 0x00223F80 File Offset: 0x00222180
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

		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x06006FE0 RID: 28640 RVA: 0x00223FF0 File Offset: 0x002221F0
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

		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x06006FE1 RID: 28641 RVA: 0x00224038 File Offset: 0x00222238
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

		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x06006FE2 RID: 28642 RVA: 0x002242C4 File Offset: 0x002224C4
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

		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x06006FE3 RID: 28643 RVA: 0x00224318 File Offset: 0x00222518
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

		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x06006FE4 RID: 28644 RVA: 0x0022436C File Offset: 0x0022256C
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

		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x06006FE5 RID: 28645 RVA: 0x002243B4 File Offset: 0x002225B4
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

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x06006FE6 RID: 28646 RVA: 0x00224408 File Offset: 0x00222608
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

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x06006FE7 RID: 28647 RVA: 0x00224490 File Offset: 0x00222690
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

		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x06006FE8 RID: 28648 RVA: 0x002244E4 File Offset: 0x002226E4
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

		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x06006FE9 RID: 28649 RVA: 0x00224538 File Offset: 0x00222738
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

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x06006FEA RID: 28650 RVA: 0x00224598 File Offset: 0x00222798
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

		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x06006FEB RID: 28651 RVA: 0x002245F8 File Offset: 0x002227F8
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

		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x06006FEC RID: 28652 RVA: 0x00224654 File Offset: 0x00222854
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

		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x06006FED RID: 28653 RVA: 0x002246BC File Offset: 0x002228BC
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

		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x06006FEE RID: 28654 RVA: 0x00224710 File Offset: 0x00222910
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

		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x06006FEF RID: 28655 RVA: 0x00224758 File Offset: 0x00222958
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

		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x06006FF0 RID: 28656 RVA: 0x002247AC File Offset: 0x002229AC
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

		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x06006FF1 RID: 28657 RVA: 0x0022481C File Offset: 0x00222A1C
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

		// Token: 0x17001135 RID: 4405
		// (get) Token: 0x06006FF2 RID: 28658 RVA: 0x0022488C File Offset: 0x00222A8C
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

		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x06006FF3 RID: 28659 RVA: 0x002248FC File Offset: 0x00222AFC
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

		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x06006FF4 RID: 28660 RVA: 0x0022496C File Offset: 0x00222B6C
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

		// Token: 0x17001138 RID: 4408
		// (get) Token: 0x06006FF5 RID: 28661 RVA: 0x002249DC File Offset: 0x00222BDC
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

		// Token: 0x06006FF6 RID: 28662 RVA: 0x00224A4C File Offset: 0x00222C4C
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

		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x06006FF7 RID: 28663 RVA: 0x00224AD4 File Offset: 0x00222CD4
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

		// Token: 0x06006FF8 RID: 28664 RVA: 0x00224B68 File Offset: 0x00222D68
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

		// Token: 0x040049DE RID: 18910
		private static List<Pawn> allMapsWorldAndTemporary_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x040049DF RID: 18911
		private static List<Pawn> allMapsWorldAndTemporary_Alive_Result = new List<Pawn>();

		// Token: 0x040049E0 RID: 18912
		private static List<Pawn> allMapsAndWorld_Alive_Result = new List<Pawn>();

		// Token: 0x040049E1 RID: 18913
		private static List<Pawn> allMaps_Result = new List<Pawn>();

		// Token: 0x040049E2 RID: 18914
		private static List<Pawn> allMaps_Spawned_Result = new List<Pawn>();

		// Token: 0x040049E3 RID: 18915
		private static List<Pawn> all_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x040049E4 RID: 18916
		private static List<Pawn> temporary_Result = new List<Pawn>();

		// Token: 0x040049E5 RID: 18917
		private static List<Pawn> temporary_Alive_Result = new List<Pawn>();

		// Token: 0x040049E6 RID: 18918
		private static List<Pawn> temporary_Dead_Result = new List<Pawn>();

		// Token: 0x040049E7 RID: 18919
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_Result = new List<Pawn>();

		// Token: 0x040049E8 RID: 18920
		private static List<Pawn> allCaravansAndTravelingTransportPods_Alive_Result = new List<Pawn>();

		// Token: 0x040049E9 RID: 18921
		private static List<Pawn> allCaravansAndTravelingTransportPods_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x040049EA RID: 18922
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_Colonists_Result = new List<Pawn>();

		// Token: 0x040049EB RID: 18923
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_Result = new List<Pawn>();

		// Token: 0x040049EC RID: 18924
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoLodgers_Result = new List<Pawn>();

		// Token: 0x040049ED RID: 18925
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x040049EE RID: 18926
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_Result = new List<Pawn>();

		// Token: 0x040049EF RID: 18927
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x040049F0 RID: 18928
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony_Result = new List<Pawn>();

		// Token: 0x040049F1 RID: 18929
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		// Token: 0x040049F2 RID: 18930
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x040049F3 RID: 18931
		private static List<Pawn> allMaps_PrisonersOfColonySpawned_Result = new List<Pawn>();

		// Token: 0x040049F4 RID: 18932
		private static List<Pawn> allMaps_PrisonersOfColony_Result = new List<Pawn>();

		// Token: 0x040049F5 RID: 18933
		private static List<Pawn> allMaps_FreeColonists_Result = new List<Pawn>();

		// Token: 0x040049F6 RID: 18934
		private static List<Pawn> allMaps_FreeColonistsSpawned_Result = new List<Pawn>();

		// Token: 0x040049F7 RID: 18935
		private static List<Pawn> allMaps_FreeColonistsAndPrisonersSpawned_Result = new List<Pawn>();

		// Token: 0x040049F8 RID: 18936
		private static List<Pawn> allMaps_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		// Token: 0x040049F9 RID: 18937
		private static Dictionary<Faction, List<Pawn>> allMaps_SpawnedPawnsInFaction_Result = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x040049FA RID: 18938
		private static List<Pawn> homeMaps_FreeColonistsSpawned_Result = new List<Pawn>();
	}
}
