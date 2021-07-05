using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020001EE RID: 494
	public static class MapGenerator
	{
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x0004E686 File Offset: 0x0004C886
		public static MapGenFloatGrid Elevation
		{
			get
			{
				return MapGenerator.FloatGridNamed("Elevation");
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x0004E692 File Offset: 0x0004C892
		public static MapGenFloatGrid Fertility
		{
			get
			{
				return MapGenerator.FloatGridNamed("Fertility");
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x0004E69E File Offset: 0x0004C89E
		public static MapGenFloatGrid Caves
		{
			get
			{
				return MapGenerator.FloatGridNamed("Caves");
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x0004E6AA File Offset: 0x0004C8AA
		// (set) Token: 0x06000DE8 RID: 3560 RVA: 0x0004E6CD File Offset: 0x0004C8CD
		public static IntVec3 PlayerStartSpot
		{
			get
			{
				if (!MapGenerator.playerStartSpotInt.IsValid)
				{
					Log.Error("Accessing player start spot before setting it.");
					return IntVec3.Zero;
				}
				return MapGenerator.playerStartSpotInt;
			}
			set
			{
				MapGenerator.playerStartSpotInt = value;
			}
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0004E6D8 File Offset: 0x0004C8D8
		public static Map GenerateMap(IntVec3 mapSize, MapParent parent, MapGeneratorDef mapGenerator, IEnumerable<GenStepWithParams> extraGenStepDefs = null, Action<Map> extraInitBeforeContentGen = null)
		{
			ProgramState programState = Current.ProgramState;
			Current.ProgramState = ProgramState.MapInitializing;
			MapGenerator.playerStartSpotInt = IntVec3.Invalid;
			MapGenerator.rootsToUnfog.Clear();
			MapGenerator.data.Clear();
			MapGenerator.mapBeingGenerated = null;
			DeepProfiler.Start("InitNewGeneratedMap");
			Rand.PushState();
			int seed = Gen.HashCombineInt(Find.World.info.Seed, parent.Tile);
			Rand.Seed = seed;
			Map result;
			try
			{
				if (parent != null && parent.HasMap)
				{
					Log.Error("Tried to generate a new map and set " + parent + " as its parent, but this world object already has a map. One world object can't have more than 1 map.");
					parent = null;
				}
				DeepProfiler.Start("Set up map");
				Map map = new Map();
				map.uniqueID = Find.UniqueIDsManager.GetNextMapID();
				map.generationTick = GenTicks.TicksGame;
				MapGenerator.mapBeingGenerated = map;
				map.info.Size = mapSize;
				map.info.parent = parent;
				map.ConstructComponents();
				DeepProfiler.End();
				Current.Game.AddMap(map);
				if (extraInitBeforeContentGen != null)
				{
					extraInitBeforeContentGen(map);
				}
				if (mapGenerator == null)
				{
					Log.Error("Attempted to generate map without generator; falling back on encounter map");
					mapGenerator = MapGeneratorDefOf.Encounter;
				}
				IEnumerable<GenStepWithParams> enumerable = from x in mapGenerator.genSteps
				select new GenStepWithParams(x, default(GenStepParams));
				if (extraGenStepDefs != null)
				{
					enumerable = enumerable.Concat(extraGenStepDefs);
				}
				map.areaManager.AddStartingAreas();
				map.weatherDecider.StartInitialWeather();
				DeepProfiler.Start("Generate contents into map");
				MapGenerator.GenerateContentsIntoMap(enumerable, map, seed);
				DeepProfiler.End();
				Find.Scenario.PostMapGenerate(map);
				DeepProfiler.Start("Finalize map init");
				map.FinalizeInit();
				DeepProfiler.End();
				DeepProfiler.Start("MapComponent.MapGenerated()");
				MapComponentUtility.MapGenerated(map);
				DeepProfiler.End();
				if (parent != null)
				{
					parent.PostMapGenerate();
				}
				result = map;
			}
			finally
			{
				DeepProfiler.End();
				MapGenerator.mapBeingGenerated = null;
				Current.ProgramState = programState;
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0004E8C4 File Offset: 0x0004CAC4
		public static void GenerateContentsIntoMap(IEnumerable<GenStepWithParams> genStepDefs, Map map, int seed)
		{
			MapGenerator.data.Clear();
			Rand.PushState();
			try
			{
				Rand.Seed = seed;
				RockNoises.Init(map);
				MapGenerator.tmpGenSteps.Clear();
				MapGenerator.tmpGenSteps.AddRange(from x in genStepDefs
				orderby x.def.order, x.def.index
				select x);
				for (int i = 0; i < MapGenerator.tmpGenSteps.Count; i++)
				{
					DeepProfiler.Start("GenStep - " + MapGenerator.tmpGenSteps[i].def);
					try
					{
						Rand.Seed = Gen.HashCombineInt(seed, MapGenerator.GetSeedPart(MapGenerator.tmpGenSteps, i));
						MapGenerator.tmpGenSteps[i].def.genStep.Generate(map, MapGenerator.tmpGenSteps[i].parms);
					}
					catch (Exception arg)
					{
						Log.Error("Error in GenStep: " + arg);
					}
					finally
					{
						DeepProfiler.End();
					}
				}
			}
			finally
			{
				Rand.PopState();
				RockNoises.Reset();
				MapGenerator.data.Clear();
			}
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0004EA1C File Offset: 0x0004CC1C
		public static T GetVar<T>(string name)
		{
			object obj;
			if (MapGenerator.data.TryGetValue(name, out obj))
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0004EA48 File Offset: 0x0004CC48
		public static bool TryGetVar<T>(string name, out T var)
		{
			object obj;
			if (MapGenerator.data.TryGetValue(name, out obj))
			{
				var = (T)((object)obj);
				return true;
			}
			var = default(T);
			return false;
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0004EA7A File Offset: 0x0004CC7A
		public static void SetVar<T>(string name, T var)
		{
			MapGenerator.data[name] = var;
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0004EA90 File Offset: 0x0004CC90
		public static MapGenFloatGrid FloatGridNamed(string name)
		{
			MapGenFloatGrid var = MapGenerator.GetVar<MapGenFloatGrid>(name);
			if (var != null)
			{
				return var;
			}
			MapGenFloatGrid mapGenFloatGrid = new MapGenFloatGrid(MapGenerator.mapBeingGenerated);
			MapGenerator.SetVar<MapGenFloatGrid>(name, mapGenFloatGrid);
			return mapGenFloatGrid;
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0004EABC File Offset: 0x0004CCBC
		private static int GetSeedPart(List<GenStepWithParams> genSteps, int index)
		{
			int seedPart = genSteps[index].def.genStep.SeedPart;
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				if (MapGenerator.tmpGenSteps[i].def.genStep.SeedPart == seedPart)
				{
					num++;
				}
			}
			return seedPart + num;
		}

		// Token: 0x04000B5D RID: 2909
		public static Map mapBeingGenerated;

		// Token: 0x04000B5E RID: 2910
		private static Dictionary<string, object> data = new Dictionary<string, object>();

		// Token: 0x04000B5F RID: 2911
		private static IntVec3 playerStartSpotInt = IntVec3.Invalid;

		// Token: 0x04000B60 RID: 2912
		public static List<IntVec3> rootsToUnfog = new List<IntVec3>();

		// Token: 0x04000B61 RID: 2913
		private static List<GenStepWithParams> tmpGenSteps = new List<GenStepWithParams>();

		// Token: 0x04000B62 RID: 2914
		public const string ElevationName = "Elevation";

		// Token: 0x04000B63 RID: 2915
		public const string FertilityName = "Fertility";

		// Token: 0x04000B64 RID: 2916
		public const string CavesName = "Caves";

		// Token: 0x04000B65 RID: 2917
		public const string RectOfInterestName = "RectOfInterest";

		// Token: 0x04000B66 RID: 2918
		public const string UsedRectsName = "UsedRects";

		// Token: 0x04000B67 RID: 2919
		public const string RectOfInterestTurretsGenStepsCount = "RectOfInterestTurretsGenStepsCount";
	}
}
