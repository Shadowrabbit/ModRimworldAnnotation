using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000491 RID: 1169
	public static class ArenaUtility
	{
		// Token: 0x060023BF RID: 9151 RVA: 0x000DE58B File Offset: 0x000DC78B
		public static bool ValidateArenaCapability()
		{
			if (Find.World.info.planetCoverage < 0.299f)
			{
				Log.Error("Planet coverage must be 30%+ to ensure a representative mix of biomes.");
				return false;
			}
			return true;
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x000DE5B0 File Offset: 0x000DC7B0
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

		// Token: 0x060023C1 RID: 9153 RVA: 0x000DE678 File Offset: 0x000DC878
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

		// Token: 0x060023C2 RID: 9154 RVA: 0x000DE6F0 File Offset: 0x000DC8F0
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

		// Token: 0x060023C3 RID: 9155 RVA: 0x000DE754 File Offset: 0x000DC954
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

		// Token: 0x060023C4 RID: 9156 RVA: 0x000DE7D8 File Offset: 0x000DC9D8
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
						})).ToLineList(null, false)));
					}
				});
				return false;
			});
		}

		// Token: 0x04001619 RID: 5657
		private const int liveSimultaneous = 15;

		// Token: 0x02001C8D RID: 7309
		public struct ArenaResult
		{
			// Token: 0x04006E27 RID: 28199
			public ArenaUtility.ArenaResult.Winner winner;

			// Token: 0x04006E28 RID: 28200
			public int tickDuration;

			// Token: 0x02002ABE RID: 10942
			public enum Winner
			{
				// Token: 0x0400A0AC RID: 41132
				Other,
				// Token: 0x0400A0AD RID: 41133
				Lhs,
				// Token: 0x0400A0AE RID: 41134
				Rhs
			}
		}

		// Token: 0x02001C8E RID: 7310
		private class ArenaSetState
		{
			// Token: 0x04006E29 RID: 28201
			public int live;
		}
	}
}
