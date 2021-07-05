using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

// Token: 0x02000008 RID: 8
public class ComplexWorker
{
	// Token: 0x06000012 RID: 18 RVA: 0x00002688 File Offset: 0x00000888
	public virtual Faction GetFixedHostileFactionForThreats()
	{
		return null;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x0000268C File Offset: 0x0000088C
	public virtual ComplexSketch GenerateSketch(IntVec2 size, Faction faction = null)
	{
		Sketch sketch = new Sketch();
		ThingDef thingDef = BaseGenUtility.RandomCheapWallStuff(faction ?? Faction.OfAncients, true);
		int entranceCount = GenMath.RoundRandom(ComplexWorker.EntranceCountOverAreaCurve.Evaluate((float)size.Area));
		ComplexLayout complexLayout = ComplexLayoutGenerator.GenerateRandomLayout(new CellRect(0, 0, size.x, size.z), 6, 6, 0.2f, null, entranceCount);
		ThingDef stuff = thingDef;
		for (int i = complexLayout.container.minX; i <= complexLayout.container.maxX; i++)
		{
			for (int j = complexLayout.container.minZ; j <= complexLayout.container.maxZ; j++)
			{
				IntVec3 intVec = new IntVec3(i, 0, j);
				int roomIdAt = complexLayout.GetRoomIdAt(intVec);
				if (complexLayout.IsWallAt(intVec))
				{
					sketch.AddThing(ThingDefOf.Wall, intVec, Rot4.North, (roomIdAt % 2 == 0) ? thingDef : ThingDefOf.Steel, 1, null, null, true);
				}
				if (complexLayout.IsFloorAt(intVec) || complexLayout.IsDoorAt(intVec))
				{
					sketch.AddTerrain(TerrainDefOf.PavedTile, intVec, true);
				}
				if (complexLayout.IsDoorAt(intVec))
				{
					sketch.AddThing(ThingDefOf.Door, intVec, Rot4.North, stuff, 1, null, null, true);
				}
			}
		}
		ComplexRoomParams roomParams = default(ComplexRoomParams);
		roomParams.sketch = sketch;
		List<ComplexRoomDef> usedDefs = new List<ComplexRoomDef>();
		Func<ComplexRoomDef, bool> <>9__0;
		foreach (ComplexRoom complexRoom in complexLayout.Rooms)
		{
			roomParams.room = complexRoom;
			IEnumerable<ComplexRoomDef> roomDefs = this.def.roomDefs;
			Func<ComplexRoomDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((ComplexRoomDef d) => d.CanResolve(roomParams) && usedDefs.Count((ComplexRoomDef ud) => ud == d) < d.maxCount));
			}
			ComplexRoomDef complexRoomDef;
			if (roomDefs.Where(predicate).TryRandomElementByWeight((ComplexRoomDef d) => d.selectionWeight, out complexRoomDef))
			{
				complexRoom.def = complexRoomDef;
				usedDefs.Add(complexRoom.def);
			}
		}
		foreach (ComplexRoom complexRoom2 in complexLayout.Rooms)
		{
			if (complexRoom2.def != null)
			{
				roomParams.room = complexRoom2;
				complexRoom2.def.ResolveSketch(roomParams);
			}
		}
		return new ComplexSketch
		{
			structure = sketch,
			layout = complexLayout,
			complexDef = this.def
		};
	}

	// Token: 0x06000014 RID: 20 RVA: 0x0000297C File Offset: 0x00000B7C
	public virtual void Spawn(ComplexSketch sketch, Map map, IntVec3 center, float? threatPoints = null, List<Thing> allSpawnedThings = null)
	{
		List<Thing> list = allSpawnedThings ?? new List<Thing>();
		sketch.structure.Spawn(map, center, null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, true, true, list, false, true, null, null);
		List<List<CellRect>> list2 = new List<List<CellRect>>();
		List<ComplexRoom> rooms = sketch.layout.Rooms;
		Predicate<IntVec3> <>9__1;
		for (int i = 0; i < rooms.Count; i++)
		{
			ComplexWorker.tmpRoomMapRects.Clear();
			for (int j = 0; j < rooms[i].rects.Count; j++)
			{
				ComplexWorker.tmpRoomMapRects.Add(rooms[i].rects[j].MovedBy(center));
			}
			List<CellRect> list3 = new List<CellRect>();
			for (int k = 0; k < ComplexWorker.tmpRoomMapRects.Count; k++)
			{
				CellRect rect = ComplexWorker.tmpRoomMapRects[k];
				Map map2 = map;
				HashSet<IntVec3> processed = new HashSet<IntVec3>();
				Predicate<IntVec3> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((IntVec3 c) => ComplexWorker.<Spawn>g__CanExpand|7_0(c, map)));
				}
				CellRect item = LargestAreaFinder.ExpandRect(rect, map2, processed, predicate, false);
				list3.Add(item);
			}
			list2.Add(list3);
			ComplexWorker.tmpRoomMapRects.Clear();
		}
		if (!sketch.thingsToSpawn.NullOrEmpty<Thing>())
		{
			HashSet<List<CellRect>> usedRooms = new HashSet<List<CellRect>>();
			foreach (Thing thing in sketch.thingsToSpawn)
			{
				List<CellRect> list4;
				Rot4 rot;
				IntVec3 loc = ComplexWorker.FindBestSpawnLocation(list2, thing.def, map, out list4, out rot, usedRooms);
				if (!loc.IsValid)
				{
					loc = ComplexWorker.FindBestSpawnLocation(list2, thing.def, map, out list4, out rot, null);
				}
				if (!loc.IsValid)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
				else
				{
					GenSpawn.Spawn(thing, loc, map, rot, WipeMode.Vanish, false);
					list.Add(thing);
					if (!sketch.thingDiscoveredMessage.NullOrEmpty())
					{
						string signalTag = "ThingDiscovered" + Find.UniqueIDsManager.GetNextSignalTagID();
						foreach (CellRect rect2 in list4)
						{
							RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
							rectTrigger.signalTag = signalTag;
							rectTrigger.Rect = rect2;
							GenSpawn.Spawn(rectTrigger, rect2.CenterCell, map, WipeMode.Vanish);
						}
						SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
						signalAction_Message.signalTag = signalTag;
						signalAction_Message.message = sketch.thingDiscoveredMessage;
						signalAction_Message.messageType = MessageTypeDefOf.PositiveEvent;
						signalAction_Message.lookTargets = thing;
						GenSpawn.Spawn(signalAction_Message, loc, map, WipeMode.Vanish);
					}
				}
			}
		}
		if (threatPoints != null)
		{
			this.SpawnThreats(sketch, map, center, threatPoints.Value, list, list2);
		}
		this.PostSpawnStructure(list2, map, list);
		sketch.thingsToSpawn.Clear();
		ComplexWorker.tmpSpawnedThreatThings.Clear();
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002CC8 File Offset: 0x00000EC8
	private void SpawnThreats(ComplexSketch sketch, Map map, IntVec3 center, float threatPoints, List<Thing> spawnedThings, List<List<CellRect>> roomRects)
	{
		ComplexResolveParams threatParams = default(ComplexResolveParams);
		threatParams.map = map;
		threatParams.complexRect = sketch.structure.OccupiedRect.MovedBy(center);
		threatParams.hostileFaction = this.GetFixedHostileFactionForThreats();
		threatParams.allRooms = roomRects;
		threatParams.points = threatPoints;
		StringBuilder stringBuilder = null;
		if (DebugViewSettings.logComplexGenPoints)
		{
			stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("----- Logging points for " + this.def.defName + ". -----");
			stringBuilder.AppendLine(string.Format("Total threat points: {0}", threatPoints));
			stringBuilder.AppendLine(string.Format("Room count: {0}", roomRects.Count));
			stringBuilder.AppendLine(string.Format("Approx points per room: {0}", threatParams.points));
			if (threatParams.hostileFaction != null)
			{
				stringBuilder.AppendLine(string.Format("Faction: {0}", threatParams.hostileFaction));
			}
		}
		ComplexWorker.useableThreats.Clear();
		ComplexWorker.useableThreats.AddRange(from t in this.def.threats
		where Rand.Chance(t.chancePerComplex)
		select t);
		float num = 0f;
		int num2 = 100;
		Dictionary<List<CellRect>, List<ComplexThreatDef>> usedThreatsByRoom = new Dictionary<List<CellRect>, List<ComplexThreatDef>>();
		while (num < threatPoints && num2 > 0)
		{
			num2--;
			List<CellRect> room = roomRects.RandomElement<List<CellRect>>();
			threatParams.room = room;
			threatParams.spawnedThings = spawnedThings;
			float b = threatPoints - num;
			threatParams.points = Mathf.Min(ComplexWorker.ThreatPointsFactorRange.RandomInRange * threatPoints, b);
			ComplexThreat complexThreat;
			if (ComplexWorker.useableThreats.Where(delegate(ComplexThreat t)
			{
				int num4 = 0;
				Func<ComplexThreatDef, bool> <>9__4;
				foreach (KeyValuePair<List<CellRect>, List<ComplexThreatDef>> keyValuePair in usedThreatsByRoom)
				{
					int num5 = num4;
					IEnumerable<ComplexThreatDef> value = keyValuePair.Value;
					Func<ComplexThreatDef, bool> predicate;
					if ((predicate = <>9__4) == null)
					{
						predicate = (<>9__4 = ((ComplexThreatDef td) => td == t.def));
					}
					num4 = num5 + value.Count(predicate);
				}
				return num4 < t.maxPerComplex && (!usedThreatsByRoom.ContainsKey(room) || usedThreatsByRoom[room].Count((ComplexThreatDef td) => td == t.def) < t.maxPerRoom) && t.def.Worker.CanResolve(threatParams);
			}).TryRandomElementByWeight((ComplexThreat t) => t.selectionWeight, out complexThreat))
			{
				if (stringBuilder != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("-> Resolving threat " + complexThreat.def.defName);
				}
				float num3 = 0f;
				complexThreat.def.Worker.Resolve(threatParams, ref num3, ComplexWorker.tmpSpawnedThreatThings, stringBuilder);
				num += num3;
				if (!usedThreatsByRoom.ContainsKey(room))
				{
					usedThreatsByRoom[room] = new List<ComplexThreatDef>();
				}
				usedThreatsByRoom[room].Add(complexThreat.def);
			}
		}
		if (stringBuilder != null)
		{
			stringBuilder.AppendLine(string.Format("Total threat points spent: {0}", num));
			Log.Message(stringBuilder.ToString());
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002FF0 File Offset: 0x000011F0
	private bool TryGetThingTriggerSignal(ComplexResolveParams threatParams, out string triggerSignal)
	{
		if (threatParams.room == null || threatParams.spawnedThings.NullOrEmpty<Thing>())
		{
			triggerSignal = null;
			return false;
		}
		List<CellRect> room = threatParams.room;
		for (int i = 0; i < threatParams.spawnedThings.Count; i++)
		{
			Thing thing = threatParams.spawnedThings[i];
			if (room.Any((CellRect r) => r.Contains(thing.Position)))
			{
				CompHackable compHackable = thing.TryGetComp<CompHackable>();
				if (compHackable != null && !compHackable.IsHacked)
				{
					if (Rand.Bool)
					{
						if (compHackable.hackingStartedSignal == null)
						{
							compHackable.hackingStartedSignal = "ThreatTriggerSignal" + Find.UniqueIDsManager.GetNextSignalTagID();
						}
						triggerSignal = compHackable.hackingStartedSignal;
					}
					else
					{
						if (compHackable.hackingCompletedSignal == null)
						{
							compHackable.hackingCompletedSignal = "ThreatTriggerSignal" + Find.UniqueIDsManager.GetNextSignalTagID();
						}
						triggerSignal = compHackable.hackingCompletedSignal;
					}
					return true;
				}
				Building_Casket building_Casket;
				if ((building_Casket = (thing as Building_Casket)) != null && building_Casket.CanOpen)
				{
					if (building_Casket.openedSignal.NullOrEmpty())
					{
						building_Casket.openedSignal = "ThreatTriggerSignal" + Find.UniqueIDsManager.GetNextSignalTagID();
					}
					triggerSignal = building_Casket.openedSignal;
					return true;
				}
			}
		}
		triggerSignal = null;
		return false;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000313F File Offset: 0x0000133F
	protected virtual void PostSpawnStructure(List<List<CellRect>> rooms, Map map, List<Thing> allSpawnedThings)
	{
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00003144 File Offset: 0x00001344
	protected static IntVec3 FindBestSpawnLocation(List<List<CellRect>> rooms, ThingDef thingDef, Map map, out List<CellRect> roomUsed, out Rot4 rotUsed, HashSet<List<CellRect>> usedRooms = null)
	{
		ComplexWorker.tmpSpawnLocations.Clear();
		foreach (List<CellRect> list in rooms.InRandomOrder(null))
		{
			if (usedRooms == null || !usedRooms.Contains(list))
			{
				ComplexWorker.tmpSpawnLocations.Clear();
				ComplexWorker.tmpSpawnLocations.AddRange(list.SelectMany((CellRect r) => r.Cells));
				foreach (IntVec3 intVec in ComplexWorker.tmpSpawnLocations.InRandomOrder(null))
				{
					for (int i = 0; i < 4; i++)
					{
						Rot4 rot = new Rot4(i);
						CellRect cellRect = GenAdj.OccupiedRect(intVec, rot, thingDef.size);
						bool flag = false;
						foreach (IntVec3 c in cellRect.Cells)
						{
							if (!c.Standable(map) || c.GetDoor(map) != null)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							bool flag2 = false;
							foreach (IntVec3 c2 in cellRect.ExpandedBy(1).EdgeCells)
							{
								if (c2.GetThingList(map).Any((Thing t) => t.def == ThingDefOf.Door))
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2 && ThingUtility.InteractionCellWhenAt(thingDef, intVec, rot, map).Standable(map))
							{
								ComplexWorker.tmpSpawnLocations.Clear();
								if (usedRooms != null)
								{
									usedRooms.Add(list);
								}
								roomUsed = list;
								rotUsed = rot;
								return intVec;
							}
						}
					}
				}
			}
		}
		roomUsed = null;
		rotUsed = default(Rot4);
		return IntVec3.Invalid;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00003478 File Offset: 0x00001678
	[CompilerGenerated]
	internal static bool <Spawn>g__CanExpand|7_0(IntVec3 c, Map m)
	{
		Building edifice = c.GetEdifice(m);
		return edifice != null && (edifice.def == ThingDefOf.Wall || edifice.def == ThingDefOf.Door);
	}

	// Token: 0x04000008 RID: 8
	private static readonly FloatRange ThreatPointsFactorRange = new FloatRange(0.25f, 0.35f);

	// Token: 0x04000009 RID: 9
	private static SimpleCurve EntranceCountOverAreaCurve = new SimpleCurve
	{
		{
			new CurvePoint(0f, 1f),
			true
		},
		{
			new CurvePoint(1000f, 1f),
			true
		},
		{
			new CurvePoint(1500f, 2f),
			true
		},
		{
			new CurvePoint(5000f, 3f),
			true
		},
		{
			new CurvePoint(10000f, 4f),
			true
		}
	};

	// Token: 0x0400000A RID: 10
	public ComplexDef def;

	// Token: 0x0400000B RID: 11
	private static List<CellRect> tmpRoomMapRects = new List<CellRect>();

	// Token: 0x0400000C RID: 12
	private static List<Thing> tmpSpawnedThreatThings = new List<Thing>();

	// Token: 0x0400000D RID: 13
	private static List<ComplexThreat> useableThreats = new List<ComplexThreat>();

	// Token: 0x0400000E RID: 14
	private const string ThreatTriggerSignal = "ThreatTriggerSignal";

	// Token: 0x0400000F RID: 15
	private static List<IntVec3> tmpSpawnLocations = new List<IntVec3>();
}
