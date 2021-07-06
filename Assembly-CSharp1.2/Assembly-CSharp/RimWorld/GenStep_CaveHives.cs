using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001279 RID: 4729
	public class GenStep_CaveHives : GenStep
	{
		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x06006718 RID: 26392 RVA: 0x00046724 File Offset: 0x00044924
		public override int SeedPart
		{
			get
			{
				return 349641510;
			}
		}

		// Token: 0x06006719 RID: 26393 RVA: 0x001FB280 File Offset: 0x001F9480
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!Find.Storyteller.difficultyValues.allowCaveHives)
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

		// Token: 0x0600671A RID: 26394 RVA: 0x001FB438 File Offset: 0x001F9638
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

		// Token: 0x0600671B RID: 26395 RVA: 0x001FB504 File Offset: 0x001F9704
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

		// Token: 0x04004480 RID: 17536
		private List<IntVec3> rockCells = new List<IntVec3>();

		// Token: 0x04004481 RID: 17537
		private List<IntVec3> possibleSpawnCells = new List<IntVec3>();

		// Token: 0x04004482 RID: 17538
		private List<Hive> spawnedHives = new List<Hive>();

		// Token: 0x04004483 RID: 17539
		private const int MinDistToOpenSpace = 10;

		// Token: 0x04004484 RID: 17540
		private const int MinDistFromFactionBase = 50;

		// Token: 0x04004485 RID: 17541
		private const float CaveCellsPerHive = 1000f;
	}
}
