using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020007F4 RID: 2036
	public static class ArenaUtility
	{
		// Token: 0x06003398 RID: 13208 RVA: 0x0002885C File Offset: 0x00026A5C
		public static bool ValidateArenaCapability()
		{
			if (Find.World.info.planetCoverage < 0.299f)
			{
				Log.Error("Planet coverage must be 30%+ to ensure a representative mix of biomes.", false);
				return false;
			}
			return true;
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x0014FC10 File Offset: 0x0014DE10
		public static void BeginArenaFight(List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback)
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Debug_Arena);
			mapParent.Tile = TileFinder.RandomSettlementTileFor(Faction.OfPlayer, true, (int tile) => lhs.Concat(rhs).Any((PawnKindDef pawnkind) => Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, pawnkind.race)));
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null);
			IntVec3 spot;
			IntVec3 spot2;
			MultipleCaravansCellFinder.FindStartingCellsFor2Groups(orGenerateMap, out spot, out spot2);
			List<Pawn> lhs2 = ArenaUtility.SpawnPawnSet(orGenerateMap, lhs, spot, Faction.OfAncients);
			List<Pawn> rhs2 = ArenaUtility.SpawnPawnSet(orGenerateMap, rhs, spot2, Faction.OfAncientsHostile);
			DebugArena component = mapParent.GetComponent<DebugArena>();
			component.lhs = lhs2;
			component.rhs = rhs2;
			component.callback = callback;
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x0014FCD8 File Offset: 0x0014DED8
		public static List<Pawn> SpawnPawnSet(Map map, List<PawnKindDef> kinds, IntVec3 spot, Faction faction)
		{
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < kinds.Count; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(kinds[i], faction);
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(spot, map, 12, null);
				GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
				list.Add(pawn);
			}
			LordMaker.MakeNewLord(faction, new LordJob_DefendPoint(map.Center, null, false, true), map, list);
			return list;
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x0014FD50 File Offset: 0x0014DF50
		private static bool ArenaFightQueue(List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback, ArenaUtility.ArenaSetState state)
		{
			if (!ArenaUtility.ValidateArenaCapability())
			{
				return false;
			}
			if (state.live < 15)
			{
				ArenaUtility.BeginArenaFight(lhs, rhs, delegate(ArenaUtility.ArenaResult result)
				{
					state.live--;
					callback(result);
				});
				state.live++;
				return true;
			}
			return false;
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x0014FDB4 File Offset: 0x0014DFB4
		public static void BeginArenaFightSet(int count, List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback, Action report)
		{
			if (!ArenaUtility.ValidateArenaCapability())
			{
				return;
			}
			int remaining = count;
			ArenaUtility.ArenaSetState state = new ArenaUtility.ArenaSetState();
			Action<ArenaUtility.ArenaResult> <>9__1;
			Func<bool> <>9__0;
			for (int i = 0; i < count; i++)
			{
				GameComponent_DebugTools component = Current.Game.GetComponent<GameComponent_DebugTools>();
				Func<bool> callback2;
				if ((callback2 = <>9__0) == null)
				{
					callback2 = (<>9__0 = delegate()
					{
						List<PawnKindDef> lhs2 = lhs;
						List<PawnKindDef> rhs2 = rhs;
						Action<ArenaUtility.ArenaResult> callback3;
						if ((callback3 = <>9__1) == null)
						{
							callback3 = (<>9__1 = delegate(ArenaUtility.ArenaResult result)
							{
								callback(result);
								int remaining;
								remaining--;
								remaining = remaining;
								if (remaining % 10 == 0)
								{
									report();
								}
							});
						}
						return ArenaUtility.ArenaFightQueue(lhs2, rhs2, callback3, state);
					});
				}
				component.AddPerFrameCallback(callback2);
			}
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x0014FE38 File Offset: 0x0014E038
		public static void PerformBattleRoyale(IEnumerable<PawnKindDef> kindsEnumerable)
		{
			if (!ArenaUtility.ValidateArenaCapability())
			{
				return;
			}
			List<PawnKindDef> kinds = kindsEnumerable.ToList<PawnKindDef>();
			Dictionary<PawnKindDef, float> ratings = new Dictionary<PawnKindDef, float>();
			foreach (PawnKindDef pawnKindDef in kinds)
			{
				ratings[pawnKindDef] = EloUtility.CalculateRating(pawnKindDef.combatPower, 1500f, 60f);
			}
			int currentFights = 0;
			int completeFights = 0;
			Current.Game.GetComponent<GameComponent_DebugTools>().AddPerFrameCallback(delegate
			{
				int currentFights;
				if (currentFights >= 15)
				{
					return false;
				}
				PawnKindDef lhsDef = kinds.RandomElement<PawnKindDef>();
				PawnKindDef rhsDef = kinds.RandomElement<PawnKindDef>();
				float num = EloUtility.CalculateExpectation(ratings[lhsDef], ratings[rhsDef]);
				float num2 = 1f - num;
				float num3 = num;
				float num4 = Mathf.Min(num2, num3);
				num2 /= num4;
				num3 /= num4;
				float num5 = Mathf.Max(num2, num3);
				if (num5 > 40f)
				{
					return false;
				}
				float num6 = 40f / num5;
				float num7 = (float)Math.Exp((double)Rand.Range(0f, (float)Math.Log((double)num6)));
				num2 *= num7;
				num3 *= num7;
				List<PawnKindDef> lhs = Enumerable.Repeat<PawnKindDef>(lhsDef, GenMath.RoundRandom(num2)).ToList<PawnKindDef>();
				List<PawnKindDef> rhs = Enumerable.Repeat<PawnKindDef>(rhsDef, GenMath.RoundRandom(num3)).ToList<PawnKindDef>();
				currentFights++;
				currentFights = currentFights;
				ArenaUtility.BeginArenaFight(lhs, rhs, delegate(ArenaUtility.ArenaResult result)
				{
					int num8 = currentFights - 1;
					currentFights = num8;
					num8 = completeFights + 1;
					completeFights = num8;
					if (result.winner != ArenaUtility.ArenaResult.Winner.Other)
					{
						float value = ratings[lhsDef];
						float value2 = ratings[rhsDef];
						float kfactor = 8f * Mathf.Pow(0.5f, Time.realtimeSinceStartup / 900f);
						EloUtility.Update(ref value, ref value2, 0.5f, (float)((result.winner == ArenaUtility.ArenaResult.Winner.Lhs) ? 1 : 0), kfactor);
						ratings[lhsDef] = value;
						ratings[rhsDef] = value2;
						Log.Message(string.Format("Scores after {0} trials:\n\n{1}", completeFights, (from v in ratings
						orderby v.Value
						select string.Format("  {0}: {1}->{2} (rating {2})", new object[]
						{
							v.Key.label,
							v.Key.combatPower,
							EloUtility.CalculateLinearScore(v.Value, 1500f, 60f).ToString("F0"),
							v.Value.ToString("F0")
						})).ToLineList(null, false)), false);
					}
				});
				return false;
			});
		}

		// Token: 0x0400234D RID: 9037
		private const int liveSimultaneous = 15;

		// Token: 0x020007F5 RID: 2037
		public struct ArenaResult
		{
			// Token: 0x0400234E RID: 9038
			public ArenaUtility.ArenaResult.Winner winner;

			// Token: 0x0400234F RID: 9039
			public int tickDuration;

			// Token: 0x020007F6 RID: 2038
			public enum Winner
			{
				// Token: 0x04002351 RID: 9041
				Other,
				// Token: 0x04002352 RID: 9042
				Lhs,
				// Token: 0x04002353 RID: 9043
				Rhs
			}
		}

		// Token: 0x020007F7 RID: 2039
		private class ArenaSetState
		{
			// Token: 0x04002354 RID: 9044
			public int live;
		}
	}
}
