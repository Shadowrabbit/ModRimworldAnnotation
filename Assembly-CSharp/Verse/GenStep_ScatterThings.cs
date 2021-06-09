using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002B1 RID: 689
	public class GenStep_ScatterThings : GenStep_Scatterer
	{
		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x00012C2F File Offset: 0x00010E2F
		public override int SeedPart
		{
			get
			{
				return 1158116095;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06001192 RID: 4498 RVA: 0x000C2CD8 File Offset: 0x000C0ED8
		private List<Rot4> PossibleRotations
		{
			get
			{
				if (this.possibleRotationsInt == null)
				{
					this.possibleRotationsInt = new List<Rot4>();
					if (this.thingDef.rotatable)
					{
						this.possibleRotationsInt.Add(Rot4.North);
						this.possibleRotationsInt.Add(Rot4.East);
						this.possibleRotationsInt.Add(Rot4.South);
						this.possibleRotationsInt.Add(Rot4.West);
					}
					else
					{
						this.possibleRotationsInt.Add(Rot4.North);
					}
				}
				return this.possibleRotationsInt;
			}
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x000C2D60 File Offset: 0x000C0F60
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!this.allowInWaterBiome && map.TileInfo.WaterCovered)
			{
				return;
			}
			int count = base.CalculateFinalCount(map);
			IntRange one;
			if (this.thingDef.ingestible != null && this.thingDef.ingestible.IsMeal && this.thingDef.stackLimit <= 10)
			{
				one = IntRange.one;
			}
			else if (this.thingDef.stackLimit > 5)
			{
				one = new IntRange(Mathf.RoundToInt((float)this.thingDef.stackLimit * 0.5f), this.thingDef.stackLimit);
			}
			else
			{
				one = new IntRange(this.thingDef.stackLimit, this.thingDef.stackLimit);
			}
			List<int> list = GenStep_ScatterThings.CountDividedIntoStacks(count, one);
			for (int i = 0; i < list.Count; i++)
			{
				IntVec3 intVec;
				if (!this.TryFindScatterCell(map, out intVec))
				{
					return;
				}
				this.ScatterAt(intVec, map, parms, list[i]);
				this.usedSpots.Add(intVec);
			}
			this.usedSpots.Clear();
			this.clusterCenter = IntVec3.Invalid;
			this.leftInCluster = 0;
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x000C2E74 File Offset: 0x000C1074
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (this.clusterSize > 1)
			{
				if (this.leftInCluster <= 0)
				{
					if (!base.TryFindScatterCell(map, out this.clusterCenter))
					{
						Log.Error("Could not find cluster center to scatter " + this.thingDef, false);
					}
					this.leftInCluster = this.clusterSize;
				}
				this.leftInCluster--;
				result = CellFinder.RandomClosewalkCellNear(this.clusterCenter, map, 4, delegate(IntVec3 x)
				{
					Rot4 rot;
					return this.TryGetRandomValidRotation(x, map, out rot);
				});
				return result.IsValid;
			}
			return base.TryFindScatterCell(map, out result);
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x000C2F24 File Offset: 0x000C1124
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int stackCount = 1)
		{
			Rot4 rot;
			if (!this.TryGetRandomValidRotation(loc, map, out rot))
			{
				Log.Warning("Could not find any valid rotation for " + this.thingDef, false);
				return;
			}
			if (this.clearSpaceSize > 0)
			{
				foreach (IntVec3 c in GridShapeMaker.IrregularLump(loc, map, this.clearSpaceSize))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
				}
			}
			Thing thing = ThingMaker.MakeThing(this.thingDef, this.stuff);
			if (this.thingDef.Minifiable)
			{
				thing = thing.MakeMinified();
			}
			if (thing.def.category == ThingCategory.Item)
			{
				thing.stackCount = stackCount;
				thing.SetForbidden(true, false);
				Thing thing2;
				GenPlace.TryPlaceThing(thing, loc, map, ThingPlaceMode.Near, out thing2, null, null, default(Rot4));
				if (this.nearPlayerStart && thing2 != null && thing2.def.category == ThingCategory.Item && TutorSystem.TutorialMode)
				{
					Find.TutorialState.AddStartingItem(thing2);
					return;
				}
			}
			else
			{
				GenSpawn.Spawn(thing, loc, map, rot, WipeMode.Vanish, false);
			}
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x000C3044 File Offset: 0x000C1244
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			Rot4 rot;
			if (!this.TryGetRandomValidRotation(loc, map, out rot))
			{
				return false;
			}
			if (this.terrainValidationRadius > 0f)
			{
				foreach (IntVec3 c in GenRadial.RadialCellsAround(loc, this.terrainValidationRadius, true))
				{
					if (c.InBounds(map))
					{
						TerrainDef terrain = c.GetTerrain(map);
						for (int i = 0; i < this.terrainValidationDisallowed.Count; i++)
						{
							if (terrain.HasTag(this.terrainValidationDisallowed[i]))
							{
								return false;
							}
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x000C3104 File Offset: 0x000C1304
		private bool TryGetRandomValidRotation(IntVec3 loc, Map map, out Rot4 rot)
		{
			List<Rot4> possibleRotations = this.PossibleRotations;
			for (int i = 0; i < possibleRotations.Count; i++)
			{
				if (this.IsRotationValid(loc, possibleRotations[i], map))
				{
					GenStep_ScatterThings.tmpRotations.Add(possibleRotations[i]);
				}
			}
			if (GenStep_ScatterThings.tmpRotations.TryRandomElement(out rot))
			{
				GenStep_ScatterThings.tmpRotations.Clear();
				return true;
			}
			rot = Rot4.Invalid;
			return false;
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x000C3170 File Offset: 0x000C1370
		private bool IsRotationValid(IntVec3 loc, Rot4 rot, Map map)
		{
			return GenAdj.OccupiedRect(loc, rot, this.thingDef.size).InBounds(map) && !GenSpawn.WouldWipeAnythingWith(loc, rot, this.thingDef, map, (Thing x) => x.def == this.thingDef || (x.def.category != ThingCategory.Plant && x.def.category != ThingCategory.Filth));
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x000C31BC File Offset: 0x000C13BC
		public static List<int> CountDividedIntoStacks(int count, IntRange stackSizeRange)
		{
			List<int> list = new List<int>();
			while (count > 0)
			{
				int num = Mathf.Min(count, stackSizeRange.RandomInRange);
				count -= num;
				list.Add(num);
			}
			if (stackSizeRange.max > 2)
			{
				for (int i = 0; i < list.Count * 4; i++)
				{
					int num2 = Rand.RangeInclusive(0, list.Count - 1);
					int num3 = Rand.RangeInclusive(0, list.Count - 1);
					if (num2 != num3 && list[num2] > list[num3])
					{
						int num4 = (int)((float)(list[num2] - list[num3]) * Rand.Value);
						List<int> list2 = list;
						int index = num2;
						list2[index] -= num4;
						list2 = list;
						index = num3;
						list2[index] += num4;
					}
				}
			}
			return list;
		}

		// Token: 0x04000E39 RID: 3641
		public ThingDef thingDef;

		// Token: 0x04000E3A RID: 3642
		public ThingDef stuff;

		// Token: 0x04000E3B RID: 3643
		public int clearSpaceSize;

		// Token: 0x04000E3C RID: 3644
		public int clusterSize = 1;

		// Token: 0x04000E3D RID: 3645
		public float terrainValidationRadius;

		// Token: 0x04000E3E RID: 3646
		[NoTranslate]
		private List<string> terrainValidationDisallowed;

		// Token: 0x04000E3F RID: 3647
		[Unsaved(false)]
		private IntVec3 clusterCenter;

		// Token: 0x04000E40 RID: 3648
		[Unsaved(false)]
		private int leftInCluster;

		// Token: 0x04000E41 RID: 3649
		private const int ClusterRadius = 4;

		// Token: 0x04000E42 RID: 3650
		private List<Rot4> possibleRotationsInt;

		// Token: 0x04000E43 RID: 3651
		private static List<Rot4> tmpRotations = new List<Rot4>();
	}
}
