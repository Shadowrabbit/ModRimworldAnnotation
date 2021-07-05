using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020003A2 RID: 930
	public static class DebugAutotests
	{
		// Token: 0x06001BA5 RID: 7077 RVA: 0x000A18BF File Offset: 0x0009FABF
		[DebugAction("Autotests", "Make colony (full)", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeColonyFull()
		{
			Autotests_ColonyMaker.MakeColony_Full();
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x000A18C6 File Offset: 0x0009FAC6
		[DebugAction("Autotests", "Make colony (animals)", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeColonyAnimals()
		{
			Autotests_ColonyMaker.MakeColony_Animals();
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x000A18D0 File Offset: 0x0009FAD0
		[DebugAction("Autotests", "Test force downed x100", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestForceDownedx100()
		{
			for (int i = 0; i < 100; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
				HealthUtility.DamageUntilDowned(pawn, true);
				if (pawn.Dead)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn died while force downing: ",
						pawn,
						" at ",
						pawn.Position
					}));
					return;
				}
			}
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x000A1984 File Offset: 0x0009FB84
		[DebugAction("Autotests", "Test force kill x100", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestForceKillx100()
		{
			for (int i = 0; i < 100; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
				HealthUtility.DamageUntilDead(pawn);
				if (!pawn.Dead)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn died not die: ",
						pawn,
						" at ",
						pawn.Position
					}));
					return;
				}
			}
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x000A1A38 File Offset: 0x0009FC38
		[DebugAction("Autotests", "Test Surgery fail catastrophic x100", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestSurgeryFailCatastrophicx100()
		{
			for (int i = 0; i < 100; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
				pawn.health.forceIncap = true;
				BodyPartRecord part = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).RandomElement<BodyPartRecord>();
				HealthUtility.GiveInjuriesOperationFailureCatastrophic(pawn, part);
				pawn.health.forceIncap = false;
				if (pawn.Dead)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn died: ",
						pawn,
						" at ",
						pawn.Position
					}));
				}
			}
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x000A1B1C File Offset: 0x0009FD1C
		[DebugAction("Autotests", "Test Surgery fail ridiculous x100", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestSurgeryFailRidiculousx100()
		{
			for (int i = 0; i < 100; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
				pawn.health.forceIncap = true;
				HealthUtility.GiveInjuriesOperationFailureRidiculous(pawn);
				pawn.health.forceIncap = false;
				if (pawn.Dead)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn died: ",
						pawn,
						" at ",
						pawn.Position
					}));
				}
			}
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x000A1BE8 File Offset: 0x0009FDE8
		[DebugAction("Autotests", "Test generate pawn x1000", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestGeneratePawnx1000()
		{
			float[] array = new float[]
			{
				10f,
				20f,
				50f,
				100f,
				200f,
				500f,
				1000f,
				2000f,
				5000f,
				1E+20f
			};
			int[] array2 = new int[array.Length];
			for (int i = 0; i < 1000; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				PerfLogger.Reset();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				float ms = PerfLogger.Duration() * 1000f;
				array2[array.FirstIndexOf((float time) => ms <= time)]++;
				if (pawn.Dead)
				{
					Log.Error("Pawn is dead");
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Pawn creation time histogram:");
			for (int j = 0; j < array2.Length; j++)
			{
				stringBuilder.AppendLine(string.Format("<{0}ms: {1}", array[j], array2[j]));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x000A1CE8 File Offset: 0x0009FEE8
		[DebugAction("Autotests", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GeneratePawnsOfAllShapes()
		{
			Rot4[] array = new Rot4[]
			{
				Rot4.North,
				Rot4.East,
				Rot4.South,
				Rot4.West
			};
			IntVec3 intVec = UI.MouseCell();
			foreach (BodyTypeDef bodyTypeDef in DefDatabase<BodyTypeDef>.AllDefs)
			{
				IntVec3 intVec2 = intVec;
				foreach (Rot4 rot in array)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false)
					{
						ForceBodyType = bodyTypeDef
					});
					string text = bodyTypeDef.defName + "-" + rot.ToStringWord();
					pawn.Name = new NameTriple(text, text, text);
					GenSpawn.Spawn(pawn, intVec2, Find.CurrentMap, WipeMode.Vanish);
					pawn.apparel.DestroyAll(DestroyMode.Vanish);
					pawn.drafter.Drafted = true;
					pawn.stances.SetStance(new Stance_Warmup(100000, intVec2 + rot.FacingCell, null));
					intVec2 += IntVec3.South * 2;
				}
				intVec += IntVec3.East * 2;
			}
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x000A1ED0 File Offset: 0x000A00D0
		[DebugAction("Autotests", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CheckRegionListers()
		{
			Autotests_RegionListers.CheckBugs(Find.CurrentMap);
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x000A1EDC File Offset: 0x000A00DC
		[DebugAction("Autotests", "Test time-to-down", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestTimeToDown()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<PawnKindDef> enumerator = (from kd in DefDatabase<PawnKindDef>.AllDefs
			orderby kd.defName
			select kd).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kindDef = enumerator.Current;
					list.Add(new DebugMenuOption(kindDef.label, DebugMenuOptionMode.Action, delegate()
					{
						if (kindDef == PawnKindDefOf.Colonist)
						{
							Log.Message("Current colonist TTD reference point: 22.3 seconds, stddev 8.35 seconds");
						}
						List<float> results = new List<float>();
						List<PawnKindDef> list2 = new List<PawnKindDef>();
						List<PawnKindDef> list3 = new List<PawnKindDef>();
						list2.Add(kindDef);
						list3.Add(kindDef);
						ArenaUtility.BeginArenaFightSet(1000, list2, list3, delegate(ArenaUtility.ArenaResult result)
						{
							if (result.winner != ArenaUtility.ArenaResult.Winner.Other)
							{
								results.Add(result.tickDuration.TicksToSeconds());
							}
						}, delegate
						{
							string format = "Finished {0} tests; time-to-down {1}, stddev {2}\n\nraw: {3}";
							object[] array = new object[4];
							array[0] = results.Count;
							array[1] = results.Average();
							array[2] = GenMath.Stddev(results);
							array[3] = (from res in results
							select res.ToString()).ToLineList(null, false);
							Log.Message(string.Format(format, array));
						});
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x000A1F8C File Offset: 0x000A018C
		[DebugAction("Autotests", "Battle Royale All PawnKinds", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BattleRoyaleAllPawnKinds()
		{
			ArenaUtility.PerformBattleRoyale(DefDatabase<PawnKindDef>.AllDefs);
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x000A1F98 File Offset: 0x000A0198
		[DebugAction("Autotests", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BattleRoyaleHumanlikes()
		{
			ArenaUtility.PerformBattleRoyale(from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Humanlike
			select k);
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x000A1FC8 File Offset: 0x000A01C8
		[DebugAction("Autotests", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BattleRoyaleByDamagetype()
		{
			PawnKindDef[] array = new PawnKindDef[]
			{
				PawnKindDefOf.Colonist,
				PawnKindDefOf.Muffalo
			};
			IEnumerable<ToolCapacityDef> enumerable = from tc in DefDatabase<ToolCapacityDef>.AllDefsListForReading
			where tc != ToolCapacityDefOf.KickMaterialInEyes
			select tc;
			Func<PawnKindDef, ToolCapacityDef, string> func = (PawnKindDef pkd, ToolCapacityDef dd) => string.Format("{0}_{1}", pkd.label, dd.defName);
			if (DebugAutotests.pawnKindsForDamageTypeBattleRoyale == null)
			{
				DebugAutotests.pawnKindsForDamageTypeBattleRoyale = new List<PawnKindDef>();
				foreach (PawnKindDef pawnKindDef in array)
				{
					using (IEnumerator<ToolCapacityDef> enumerator = enumerable.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ToolCapacityDef toolType = enumerator.Current;
							string text = func(pawnKindDef, toolType);
							ThingDef thingDef = Gen.MemberwiseClone<ThingDef>(pawnKindDef.race);
							thingDef.defName = text;
							thingDef.label = text;
							thingDef.tools = new List<Tool>(pawnKindDef.race.tools.Select(delegate(Tool tool)
							{
								Tool tool2 = Gen.MemberwiseClone<Tool>(tool);
								tool2.capacities = new List<ToolCapacityDef>();
								tool2.capacities.Add(toolType);
								return tool2;
							}));
							PawnKindDef pawnKindDef2 = Gen.MemberwiseClone<PawnKindDef>(pawnKindDef);
							pawnKindDef2.defName = text;
							pawnKindDef2.label = text;
							pawnKindDef2.race = thingDef;
							DebugAutotests.pawnKindsForDamageTypeBattleRoyale.Add(pawnKindDef2);
						}
					}
				}
			}
			ArenaUtility.PerformBattleRoyale(DebugAutotests.pawnKindsForDamageTypeBattleRoyale);
		}

		// Token: 0x040011A4 RID: 4516
		private static List<PawnKindDef> pawnKindsForDamageTypeBattleRoyale;
	}
}
