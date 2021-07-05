using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001316 RID: 4886
	public static class MechClusterGenerator
	{
		// Token: 0x060069C4 RID: 27076 RVA: 0x000480ED File Offset: 0x000462ED
		[Obsolete]
		public static MechClusterSketch GenerateClusterSketch(float points, bool startDormant = true)
		{
			return MechClusterGenerator.GenerateClusterSketch(points, null, startDormant);
		}

		// Token: 0x060069C5 RID: 27077 RVA: 0x000480F7 File Offset: 0x000462F7
		[Obsolete]
		public static MechClusterSketch GenerateClusterSketch(float points, Map map, bool startDormant = true)
		{
			return MechClusterGenerator.GenerateClusterSketch_NewTemp(points, map, startDormant, false);
		}

		// Token: 0x060069C6 RID: 27078 RVA: 0x00208F88 File Offset: 0x00207188
		public static MechClusterSketch GenerateClusterSketch_NewTemp(float points, Map map, bool startDormant = true, bool forceNoConditionCauser = false)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Mech clusters are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 657122, false);
				return new MechClusterSketch(new Sketch(), new List<MechClusterSketch.Mech>(), startDormant);
			}
			points = Mathf.Min(points, 10000f);
			float num = points;
			List<MechClusterSketch.Mech> list = null;
			if (Rand.Chance(MechClusterGenerator.PointsToPawnsChanceCurve.Evaluate(points)))
			{
				List<PawnKindDef> list2 = (from def in DefDatabase<PawnKindDef>.AllDefsListForReading
				where def.RaceProps.IsMechanoid
				select def).ToList<PawnKindDef>();
				list = new List<MechClusterSketch.Mech>();
				float num2 = Rand.ByCurve(MechClusterGenerator.PawnPointsRandomPercentOfTotalCurve) * num;
				num2 = Mathf.Max(num2, list2.Min((PawnKindDef x) => x.combatPower));
				float pawnPointsLeft = num2;
				Func<PawnKindDef, bool> <>9__2;
				while (pawnPointsLeft > 0f)
				{
					IEnumerable<PawnKindDef> source = list2;
					Func<PawnKindDef, bool> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((PawnKindDef def) => def.combatPower <= pawnPointsLeft));
					}
					PawnKindDef pawnKindDef;
					if (!source.Where(predicate).TryRandomElement(out pawnKindDef))
					{
						break;
					}
					pawnPointsLeft -= pawnKindDef.combatPower;
					list.Add(new MechClusterSketch.Mech(pawnKindDef));
				}
				num -= num2 - pawnPointsLeft;
			}
			Sketch buildingsSketch = SketchGen.Generate(SketchResolverDefOf.MechCluster, new ResolveParams
			{
				points = new float?(num),
				totalPoints = new float?(points),
				mechClusterDormant = new bool?(startDormant),
				sketch = new Sketch(),
				mechClusterForMap = map,
				forceNoConditionCauser = new bool?(forceNoConditionCauser)
			});
			if (list != null)
			{
				List<IntVec3> pawnUsedSpots = new List<IntVec3>();
				Func<IntVec3, bool> <>9__3;
				for (int i = 0; i < list.Count; i++)
				{
					MechClusterSketch.Mech pawn = list[i];
					IEnumerable<IntVec3> source2 = buildingsSketch.OccupiedRect;
					Func<IntVec3, bool> predicate2;
					if ((predicate2 = <>9__3) == null)
					{
						predicate2 = (<>9__3 = ((IntVec3 c) => !buildingsSketch.ThingsAt(c).Any<SketchThing>() && !pawnUsedSpots.Contains(c)));
					}
					IntVec3 intVec;
					if (!source2.Where(predicate2).TryRandomElement(out intVec))
					{
						CellRect cellRect = buildingsSketch.OccupiedRect;
						IEnumerable<IntVec3> source3;
						Func<IntVec3, bool> predicate3;
						Func<IntVec3, bool> <>9__4;
						do
						{
							cellRect = cellRect.ExpandedBy(1);
							source3 = cellRect;
							if ((predicate3 = <>9__4) == null)
							{
								predicate3 = (<>9__4 = ((IntVec3 x) => !buildingsSketch.WouldCollide(pawn.kindDef.race, x, Rot4.North) && !pawnUsedSpots.Contains(x)));
							}
						}
						while (!source3.Where(predicate3).TryRandomElement(out intVec));
					}
					pawnUsedSpots.Add(intVec);
					pawn.position = intVec;
					list[i] = pawn;
				}
			}
			return new MechClusterSketch(buildingsSketch, list, startDormant);
		}

		// Token: 0x060069C7 RID: 27079 RVA: 0x00209288 File Offset: 0x00207488
		public static void ResolveSketch(ResolveParams parms)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Mech clusters are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 673321, false);
				return;
			}
			bool canBeDormant = parms.mechClusterDormant == null || parms.mechClusterDormant.Value;
			float num;
			if (parms.points != null)
			{
				num = parms.points.Value;
			}
			else
			{
				num = 2000f;
				Log.Error("No points given for mech cluster generation. Default to " + num, false);
			}
			float value = (parms.totalPoints != null) ? parms.totalPoints.Value : num;
			IntVec2 value2;
			if (parms.mechClusterSize != null)
			{
				value2 = parms.mechClusterSize.Value;
			}
			else
			{
				int num2 = GenMath.RoundRandom(MechClusterGenerator.PointsToSizeCurve.Evaluate(num) * MechClusterGenerator.SizeRandomFactorRange.RandomInRange);
				int num3 = GenMath.RoundRandom(MechClusterGenerator.PointsToSizeCurve.Evaluate(num) * MechClusterGenerator.SizeRandomFactorRange.RandomInRange);
				if (parms.mechClusterForMap != null)
				{
					CellRect cellRect = LargestAreaFinder.FindLargestRect(parms.mechClusterForMap, (IntVec3 x) => !x.Impassable(parms.mechClusterForMap) && x.GetTerrain(parms.mechClusterForMap).affordances.Contains(TerrainAffordanceDefOf.Heavy), Mathf.Max(num2, num3));
					num2 = Mathf.Min(num2, cellRect.Width);
					num3 = Mathf.Min(num3, cellRect.Height);
				}
				value2 = new IntVec2(num2, num3);
			}
			Sketch sketch = new Sketch();
			if (Rand.Chance(MechClusterGenerator.WallsChanceCurve.Evaluate(num)))
			{
				ResolveParams parms2 = parms;
				parms2.sketch = sketch;
				parms2.mechClusterSize = new IntVec2?(value2);
				SketchResolverDefOf.MechClusterWalls.Resolve(parms2);
			}
			List<ThingDef> buildingDefsForCluster_NewTemp = MechClusterGenerator.GetBuildingDefsForCluster_NewTemp(num, value2, canBeDormant, new float?(value), parms.forceNoConditionCauser != null && parms.forceNoConditionCauser.Value);
			MechClusterGenerator.AddBuildingsToSketch(sketch, value2, buildingDefsForCluster_NewTemp);
			parms.sketch.MergeAt(sketch, default(IntVec3), Sketch.SpawnPosType.OccupiedCenter, true);
		}

		// Token: 0x060069C8 RID: 27080 RVA: 0x00048102 File Offset: 0x00046302
		[Obsolete("Only need this overload to not break mod compatibility.")]
		private static List<ThingDef> GetBuildingDefsForCluster(float points, IntVec2 size, bool canBeDormant)
		{
			return MechClusterGenerator.GetBuildingDefsForCluster_NewTemp(points, size, canBeDormant, new float?(0f));
		}

		// Token: 0x060069C9 RID: 27081 RVA: 0x00048116 File Offset: 0x00046316
		[Obsolete("Only need this overload to not break mod compatibility.")]
		private static List<ThingDef> GetBuildingDefsForCluster_NewTemp(float points, IntVec2 size, bool canBeDormant, float? totalPoints)
		{
			return MechClusterGenerator.GetBuildingDefsForCluster_NewTemp(points, size, canBeDormant, totalPoints, false);
		}

		// Token: 0x060069CA RID: 27082 RVA: 0x002094B4 File Offset: 0x002076B4
		private static List<ThingDef> GetBuildingDefsForCluster_NewTemp(float points, IntVec2 size, bool canBeDormant, float? totalPoints, bool forceNoConditionCauser)
		{
			List<ThingDef> list = new List<ThingDef>();
			List<ThingDef> list2 = DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef def)
			{
				if (def.building == null || def.building.buildingTags == null || !def.building.buildingTags.Contains("MechClusterMember"))
				{
					return false;
				}
				if (totalPoints != null)
				{
					float num6 = (float)def.building.minMechClusterPoints;
					float? totalPoints2 = totalPoints;
					return num6 <= totalPoints2.GetValueOrDefault() & totalPoints2 != null;
				}
				return true;
			}).ToList<ThingDef>();
			if (!forceNoConditionCauser)
			{
				int num = GenMath.RoundRandom(MechClusterGenerator.ProblemCauserCountCurve.Evaluate(points));
				for (int i = 0; i < num; i++)
				{
					ThingDef item;
					if (!(from x in list2
					where x.building.buildingTags.Contains("MechClusterProblemCauser")
					select x).TryRandomElementByWeight((ThingDef t) => t.generateCommonality, out item))
					{
						break;
					}
					list.Add(item);
				}
			}
			if (canBeDormant)
			{
				if (Rand.Chance(0.5f))
				{
					list.Add(ThingDefOf.ActivatorCountdown);
				}
				if (Rand.Chance(0.5f))
				{
					int num2 = GenMath.RoundRandom(MechClusterGenerator.ActivatorProximitysCountCurve.Evaluate(points));
					for (int j = 0; j < num2; j++)
					{
						list.Add(ThingDefOf.ActivatorProximity);
					}
				}
			}
			if (Rand.Chance(MechClusterGenerator.GoodBuildingChanceCurve.Evaluate(points)))
			{
				int num3 = Rand.RangeInclusive(0, GenMath.RoundRandom(MechClusterGenerator.GoodBuildingMaxCountCurve.Evaluate(points)));
				for (int k = 0; k < num3; k++)
				{
					ThingDef item2;
					if (!(from x in list2
					where x.building.buildingTags.Contains("MechClusterMemberGood")
					select x).TryRandomElement(out item2))
					{
						break;
					}
					list.Add(item2);
				}
			}
			int num4 = Rand.RangeInclusive(Mathf.FloorToInt(MechClusterGenerator.LampBuildingMinCountCurve.Evaluate(points)), Mathf.CeilToInt(MechClusterGenerator.LampBuildingMaxCountCurve.Evaluate(points)));
			for (int l = 0; l < num4; l++)
			{
				ThingDef item3;
				if (!(from x in list2
				where x.building.buildingTags.Contains("MechClusterMemberLamp")
				select x).TryRandomElement(out item3))
				{
					break;
				}
				list.Add(item3);
			}
			if (Rand.Chance(MechClusterGenerator.BulletShieldChanceCurve.Evaluate(points)))
			{
				points *= 0.85f;
				int num5 = Rand.RangeInclusive(0, GenMath.RoundRandom(MechClusterGenerator.BulletShieldMaxCountCurve.Evaluate(points)));
				for (int m = 0; m < num5; m++)
				{
					list.Add(ThingDefOf.ShieldGeneratorBullets);
				}
			}
			if (Rand.Chance(MechClusterGenerator.MortarShieldChanceCurve.Evaluate(points)))
			{
				points *= 0.9f;
				list.Add(ThingDefOf.ShieldGeneratorMortar);
			}
			float pointsLeft = points;
			ThingDef thingDef = (from x in list2
			where x.building.buildingTags.Contains("MechClusterCombatThreat")
			select x).MinBy((ThingDef x) => x.building.combatPower);
			Func<ThingDef, bool> <>9__7;
			ThingDef thingDef2;
			for (pointsLeft = Mathf.Max(pointsLeft, thingDef.building.combatPower); pointsLeft > 0f; pointsLeft -= thingDef2.building.combatPower)
			{
				IEnumerable<ThingDef> source = list2;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__7) == null)
				{
					predicate = (<>9__7 = ((ThingDef x) => x.building.combatPower <= pointsLeft && x.building.buildingTags.Contains("MechClusterCombatThreat")));
				}
				if (!source.Where(predicate).TryRandomElement(out thingDef2))
				{
					break;
				}
				list.Add(thingDef2);
			}
			return list;
		}

		// Token: 0x060069CB RID: 27083 RVA: 0x002097F4 File Offset: 0x002079F4
		private static bool TryRandomBuildingWithTag(string tag, List<ThingDef> allowedBuildings, List<ThingDef> generatedBuildings, IntVec2 size, out ThingDef result)
		{
			return (from x in allowedBuildings
			where x.building.buildingTags.Contains(tag)
			select x).TryRandomElement(out result);
		}

		// Token: 0x060069CC RID: 27084 RVA: 0x00209828 File Offset: 0x00207A28
		private static void AddBuildingsToSketch(Sketch sketch, IntVec2 size, List<ThingDef> buildings)
		{
			List<CellRect> edgeWallRects = new List<CellRect>
			{
				new CellRect(0, 0, size.x, 1),
				new CellRect(0, 0, 1, size.z),
				new CellRect(size.x - 1, 0, 1, size.z),
				new CellRect(0, size.z - 1, size.x, 1)
			};
			foreach (ThingDef thingDef in from x in buildings
			orderby x.building.IsTurret && !x.building.IsMortar
			select x)
			{
				bool flag = thingDef.building.IsTurret && !thingDef.building.IsMortar;
				IntVec3 intVec;
				if (MechClusterGenerator.TryFindRandomPlaceFor(thingDef, sketch, size, out intVec, false, flag, flag, !flag, edgeWallRects) || MechClusterGenerator.TryFindRandomPlaceFor(thingDef, sketch, size + new IntVec2(6, 6), out intVec, false, flag, flag, !flag, edgeWallRects))
				{
					sketch.AddThing(thingDef, intVec, Rot4.North, GenStuff.RandomStuffByCommonalityFor(thingDef, TechLevel.Undefined), 1, null, null, true);
					if (thingDef == ThingDefOf.Turret_AutoMiniTurret)
					{
						if (intVec.x < size.x / 2)
						{
							if (intVec.z < size.z / 2)
							{
								sketch.AddThing(ThingDefOf.Barricade, new IntVec3(intVec.x - 1, 0, intVec.z), Rot4.North, ThingDefOf.Steel, 1, null, null, false);
								sketch.AddThing(ThingDefOf.Barricade, new IntVec3(intVec.x - 1, 0, intVec.z - 1), Rot4.North, ThingDefOf.Steel, 1, null, null, false);
								sketch.AddThing(ThingDefOf.Barricade, new IntVec3(intVec.x, 0, intVec.z - 1), Rot4.North, ThingDefOf.Steel, 1, null, null, false);
							}
							else
							{
								sketch.AddThing(ThingDefOf.Barricade, new IntVec3(intVec.x - 1, 0, intVec.z), Rot4.North, ThingDefOf.Steel, 1, null, null, false);
								sketch.AddThing(ThingDefOf.Barricade, new IntVec3(intVec.x - 1, 0, intVec.z + 1), Rot4.North, ThingDefOf.Steel, 1, null, null, false);
								sketch.AddThing(ThingDefOf.Barricade, new IntVec3(intVec.x, 0, intVec.z + 1), Rot4.North, ThingDefOf.Steel, 1, null, null, false);
							}
						}
						else if (intVec.z < size.z / 2)
						{
							sketch.AddThing(ThingDefOf.Barricade, new IntVec3(intVec.x + 1, 0, intVec.z), Rot4.North, ThingDefOf.Steel, 1, null, null, false);
							sketch.AddThing(ThingDefOf.Barricade, new IntVec3(intVec.x + 1, 0, intVec.z - 1), Rot4.North, ThingDefOf.Steel, 1, null, null, false);
							sketch.AddThing(ThingDefOf.Barricade, new IntVec3(intVec.x, 0, intVec.z - 1), Rot4.North, ThingDefOf.Steel, 1, null, null, false);
						}
						else
						{
							sketch.AddThing(ThingDefOf.Barricade, new IntVec3(intVec.x + 1, 0, intVec.z), Rot4.North, ThingDefOf.Steel, 1, null, null, false);
							sketch.AddThing(ThingDefOf.Barricade, new IntVec3(intVec.x + 1, 0, intVec.z + 1), Rot4.North, ThingDefOf.Steel, 1, null, null, false);
							sketch.AddThing(ThingDefOf.Barricade, new IntVec3(intVec.x, 0, intVec.z + 1), Rot4.North, ThingDefOf.Steel, 1, null, null, false);
						}
					}
				}
			}
		}

		// Token: 0x060069CD RID: 27085 RVA: 0x00209CFC File Offset: 0x00207EFC
		private static bool TryFindRandomPlaceFor(ThingDef thingDef, Sketch sketch, IntVec2 size, out IntVec3 pos, bool lowerLeftQuarterOnly, bool avoidCenter, bool requireLOSToEdge, bool avoidEdge, List<CellRect> edgeWallRects)
		{
			int i = 0;
			while (i < 200)
			{
				CellRect cellRect = new CellRect(0, 0, size.x, size.z);
				if (lowerLeftQuarterOnly)
				{
					cellRect = new CellRect(cellRect.minX, cellRect.minZ, cellRect.Width / 2, cellRect.Height / 2);
				}
				IntVec3 randomCell = cellRect.RandomCell;
				if (avoidCenter)
				{
					CellRect cellRect2 = CellRect.CenteredOn(new CellRect(0, 0, size.x, size.z).CenterCell, size.x / 2, size.z / 2);
					int num = 0;
					while (num < 5 && cellRect2.Contains(randomCell))
					{
						randomCell = cellRect.RandomCell;
						num++;
					}
				}
				if (avoidEdge)
				{
					CellRect cellRect3 = CellRect.CenteredOn(new CellRect(0, 0, size.x, size.z).CenterCell, Mathf.RoundToInt((float)size.x * 0.75f), Mathf.RoundToInt((float)size.z * 0.75f));
					int num2 = 0;
					while (num2 < 5 && !cellRect3.Contains(randomCell))
					{
						randomCell = cellRect.RandomCell;
						num2++;
					}
				}
				if (!requireLOSToEdge)
				{
					goto IL_1A9;
				}
				IntVec3 end = randomCell;
				end.x += size.x + 1;
				IntVec3 end2 = randomCell;
				end2.x -= size.x + 1;
				IntVec3 end3 = randomCell;
				end3.z -= size.z + 1;
				IntVec3 end4 = randomCell;
				end4.z += size.z + 1;
				if (sketch.LineOfSight(randomCell, end, false, null) || sketch.LineOfSight(randomCell, end2, false, null) || sketch.LineOfSight(randomCell, end3, false, null) || sketch.LineOfSight(randomCell, end4, false, null))
				{
					goto IL_1A9;
				}
				IL_26E:
				i++;
				continue;
				IL_1A9:
				if (thingDef.building.minDistanceToSameTypeOfBuilding > 0)
				{
					bool flag = false;
					for (int j = 0; j < sketch.Things.Count; j++)
					{
						if (sketch.Things[j].def == thingDef && sketch.Things[j].pos.InHorDistOf(randomCell, (float)thingDef.building.minDistanceToSameTypeOfBuilding))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						goto IL_26E;
					}
				}
				bool flag2 = false;
				CellRect cellRect4 = GenAdj.OccupiedRect(randomCell, Rot4.North, thingDef.Size);
				for (int k = 0; k < 4; k++)
				{
					if (cellRect4.Overlaps(edgeWallRects[k]))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && !sketch.WouldCollide(thingDef, randomCell, Rot4.North))
				{
					pos = randomCell;
					return true;
				}
				goto IL_26E;
			}
			pos = IntVec3.Invalid;
			return false;
		}

		// Token: 0x060069CE RID: 27086 RVA: 0x00209F94 File Offset: 0x00208194
		[DebugOutput]
		public static void MechClusterBuildingSelection()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (float localPoints2 in DebugActionsUtility.PointsOptions(false))
			{
				float localPoints = localPoints2;
				list.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate()
				{
					string text = "";
					for (int i = 0; i < 50; i++)
					{
						int num = Rand.Range(10, 20);
						List<ThingDef> buildingDefsForCluster_NewTemp = MechClusterGenerator.GetBuildingDefsForCluster_NewTemp(localPoints, new IntVec2(num, num), true, new float?(localPoints));
						text = string.Concat(new object[]
						{
							text,
							"points: ",
							localPoints,
							" , size: ",
							num
						});
						foreach (ThingDef thingDef in buildingDefsForCluster_NewTemp)
						{
							text = text + "\n- " + thingDef.defName;
						}
						text += "\n\n";
					}
					Log.Message(text, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x04004658 RID: 18008
		public const string MechClusterMemberTag = "MechClusterMember";

		// Token: 0x04004659 RID: 18009
		public const string MechClusterMemberGoodTag = "MechClusterMemberGood";

		// Token: 0x0400465A RID: 18010
		public const string MechClusterMemberLampTag = "MechClusterMemberLamp";

		// Token: 0x0400465B RID: 18011
		public const string MechClusterActivatorTag = "MechClusterActivator";

		// Token: 0x0400465C RID: 18012
		public const string MechClusterCombatThreatTag = "MechClusterCombatThreat";

		// Token: 0x0400465D RID: 18013
		public const string MechClusterProblemCauserTag = "MechClusterProblemCauser";

		// Token: 0x0400465E RID: 18014
		public const float MaxPoints = 10000f;

		// Token: 0x0400465F RID: 18015
		public static readonly SimpleCurve PointsToPawnsChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 0.75f),
				true
			}
		};

		// Token: 0x04004660 RID: 18016
		public static readonly SimpleCurve PawnPointsRandomPercentOfTotalCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(0.8f, 0f),
				true
			}
		};

		// Token: 0x04004661 RID: 18017
		private static readonly FloatRange SizeRandomFactorRange = new FloatRange(0.8f, 2f);

		// Token: 0x04004662 RID: 18018
		private static readonly SimpleCurve PointsToSizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 7f),
				true
			},
			{
				new CurvePoint(1000f, 10f),
				true
			},
			{
				new CurvePoint(2000f, 20f),
				true
			},
			{
				new CurvePoint(5000f, 25f),
				true
			}
		};

		// Token: 0x04004663 RID: 18019
		private static readonly SimpleCurve ProblemCauserCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 0.5f),
				true
			},
			{
				new CurvePoint(800f, 0.9f),
				true
			},
			{
				new CurvePoint(1200f, 0.95f),
				true
			}
		};

		// Token: 0x04004664 RID: 18020
		private static readonly SimpleCurve WallsChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 0.35f),
				true
			},
			{
				new CurvePoint(1000f, 0.5f),
				true
			}
		};

		// Token: 0x04004665 RID: 18021
		private const float ActivatorCountdownChance = 0.5f;

		// Token: 0x04004666 RID: 18022
		private const float ActivatorProximityChance = 0.5f;

		// Token: 0x04004667 RID: 18023
		private static readonly SimpleCurve ActivatorProximitysCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(600f, 1f),
				true
			},
			{
				new CurvePoint(1800f, 2f),
				true
			},
			{
				new CurvePoint(3000f, 3f),
				true
			},
			{
				new CurvePoint(5000f, 4f),
				true
			}
		};

		// Token: 0x04004668 RID: 18024
		private static readonly SimpleCurve GoodBuildingChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 0.5f),
				true
			}
		};

		// Token: 0x04004669 RID: 18025
		private static readonly SimpleCurve GoodBuildingMaxCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 1f),
				true
			},
			{
				new CurvePoint(700f, 2f),
				true
			},
			{
				new CurvePoint(1000f, 3f),
				true
			},
			{
				new CurvePoint(1300f, 4f),
				true
			},
			{
				new CurvePoint(2000f, 5f),
				true
			},
			{
				new CurvePoint(3000f, 6f),
				true
			},
			{
				new CurvePoint(5000f, 7f),
				true
			}
		};

		// Token: 0x0400466A RID: 18026
		private static readonly SimpleCurve LampBuildingMinCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 1f),
				true
			},
			{
				new CurvePoint(1000f, 2f),
				true
			}
		};

		// Token: 0x0400466B RID: 18027
		private static readonly SimpleCurve LampBuildingMaxCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 1f),
				true
			},
			{
				new CurvePoint(1000f, 4f),
				true
			},
			{
				new CurvePoint(2000f, 6f),
				true
			}
		};

		// Token: 0x0400466C RID: 18028
		private static readonly SimpleCurve BulletShieldChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 0.1f),
				true
			},
			{
				new CurvePoint(1000f, 0.4f),
				true
			},
			{
				new CurvePoint(2200f, 0.5f),
				true
			}
		};

		// Token: 0x0400466D RID: 18029
		private const float BulletShieldTotalPointsFactor = 0.85f;

		// Token: 0x0400466E RID: 18030
		private static readonly SimpleCurve BulletShieldMaxCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 1f),
				true
			},
			{
				new CurvePoint(3000f, 1.5f),
				true
			}
		};

		// Token: 0x0400466F RID: 18031
		private const float MortarShieldTotalPointsFactor = 0.9f;

		// Token: 0x04004670 RID: 18032
		private static readonly SimpleCurve MortarShieldChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 0.1f),
				true
			},
			{
				new CurvePoint(1000f, 0.4f),
				true
			},
			{
				new CurvePoint(2200f, 0.5f),
				true
			}
		};

		// Token: 0x04004671 RID: 18033
		private const float BuildingRechooseWeight = 200f;
	}
}
