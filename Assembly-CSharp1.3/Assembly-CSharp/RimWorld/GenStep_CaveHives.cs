using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C8C RID: 3212
	public class GenStep_CaveHives : GenStep
	{
		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06004AE5 RID: 19173 RVA: 0x0018BCEB File Offset: 0x00189EEB
		public override int SeedPart
		{
			get
			{
				return 349641510;
			}
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x0018BCF4 File Offset: 0x00189EF4
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!Find.Storyteller.difficulty.allowCaveHives)
			{
				return;
			}
			if (Faction.OfInsects == null)
			{
				return;
			}
			MapGenFloatGrid caves = MapGenerator.Caves;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			float num = 0.7f;
			int num2 = 0;
			this.rockCells.Clear();
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (elevation[intVec] > num)
				{
					this.rockCells.Add(intVec);
				}
				if (caves[intVec] > 0f)
				{
					num2++;
				}
			}
			List<IntVec3> list = (from c in map.AllCells
			where map.thingGrid.ThingsAt(c).Any((Thing thing) => thing.Faction != null)
			select c).ToList<IntVec3>();
			GenMorphology.Dilate(list, 50, map, null);
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>(list);
			int num3 = GenMath.RoundRandom((float)num2 / 1000f);
			GenMorphology.Erode(this.rockCells, 10, map, null);
			this.possibleSpawnCells.Clear();
			for (int i = 0; i < this.rockCells.Count; i++)
			{
				if (caves[this.rockCells[i]] > 0f && !hashSet.Contains(this.rockCells[i]))
				{
					this.possibleSpawnCells.Add(this.rockCells[i]);
				}
			}
			this.spawnedHives.Clear();
			for (int j = 0; j < num3; j++)
			{
				this.TrySpawnHive(map);
			}
			this.spawnedHives.Clear();
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x0018BEB4 File Offset: 0x0018A0B4
		private void TrySpawnHive(Map map)
		{
			IntVec3 intVec;
			if (!this.TryFindHiveSpawnCell(map, out intVec))
			{
				return;
			}
			this.possibleSpawnCells.Remove(intVec);
			Hive hive = (Hive)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Hive, null), intVec, map, WipeMode.Vanish);
			hive.SetFaction(Faction.OfInsects, null);
			hive.PawnSpawner.aggressive = false;
			(from x in hive.GetComps<CompSpawner>()
			where x.PropsSpawner.thingToSpawn == ThingDefOf.GlowPod
			select x).First<CompSpawner>().TryDoSpawn();
			hive.PawnSpawner.SpawnPawnsUntilPoints(Rand.Range(200f, 500f));
			hive.PawnSpawner.canSpawnPawns = false;
			hive.GetComp<CompSpawnerHives>().canSpawnHives = false;
			this.spawnedHives.Add(hive);
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x0018BF80 File Offset: 0x0018A180
		private bool TryFindHiveSpawnCell(Map map, out IntVec3 spawnCell)
		{
			float num = -1f;
			IntVec3 intVec = IntVec3.Invalid;
			Func<IntVec3, bool> <>9__0;
			for (int i = 0; i < 3; i++)
			{
				IEnumerable<IntVec3> source = this.possibleSpawnCells;
				Func<IntVec3, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((IntVec3 x) => x.Standable(map) && x.GetFirstItem(map) == null && x.GetFirstBuilding(map) == null && x.GetFirstPawn(map) == null));
				}
				IntVec3 intVec2;
				if (!source.Where(predicate).TryRandomElement(out intVec2))
				{
					break;
				}
				float num2 = -1f;
				for (int j = 0; j < this.spawnedHives.Count; j++)
				{
					float num3 = (float)intVec2.DistanceToSquared(this.spawnedHives[j].Position);
					if (num2 < 0f || num3 < num2)
					{
						num2 = num3;
					}
				}
				if (!intVec.IsValid || num2 > num)
				{
					intVec = intVec2;
					num = num2;
				}
			}
			spawnCell = intVec;
			return spawnCell.IsValid;
		}

		// Token: 0x04002D5F RID: 11615
		private List<IntVec3> rockCells = new List<IntVec3>();

		// Token: 0x04002D60 RID: 11616
		private List<IntVec3> possibleSpawnCells = new List<IntVec3>();

		// Token: 0x04002D61 RID: 11617
		private List<Hive> spawnedHives = new List<Hive>();

		// Token: 0x04002D62 RID: 11618
		private const int MinDistToOpenSpace = 10;

		// Token: 0x04002D63 RID: 11619
		private const int MinDistFromFactionBase = 50;

		// Token: 0x04002D64 RID: 11620
		private const float CaveCellsPerHive = 1000f;
	}
}
