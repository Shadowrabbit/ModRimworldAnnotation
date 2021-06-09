using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002CE RID: 718
	public sealed class Region
	{
		// Token: 0x1700034F RID: 847
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x000131E4 File Offset: 0x000113E4
		public Map Map
		{
			get
			{
				if (this.mapIndex >= 0)
				{
					return Find.Maps[(int)this.mapIndex];
				}
				return null;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x0600121D RID: 4637 RVA: 0x00013201 File Offset: 0x00011401
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				RegionGrid regions = this.Map.regionGrid;
				int num;
				for (int z = this.extentsClose.minZ; z <= this.extentsClose.maxZ; z = num + 1)
				{
					for (int x = this.extentsClose.minX; x <= this.extentsClose.maxX; x = num + 1)
					{
						IntVec3 intVec = new IntVec3(x, 0, z);
						if (regions.GetRegionAt_NoRebuild_InvalidAllowed(intVec) == this)
						{
							yield return intVec;
						}
						num = x;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x0600121E RID: 4638 RVA: 0x000C4EF8 File Offset: 0x000C30F8
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					RegionGrid regionGrid = this.Map.regionGrid;
					for (int i = this.extentsClose.minZ; i <= this.extentsClose.maxZ; i++)
					{
						for (int j = this.extentsClose.minX; j <= this.extentsClose.maxX; j++)
						{
							IntVec3 c = new IntVec3(j, 0, i);
							if (regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c) == this)
							{
								this.cachedCellCount++;
							}
						}
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x00013211 File Offset: 0x00011411
		public IEnumerable<Region> Neighbors
		{
			get
			{
				int num;
				for (int li = 0; li < this.links.Count; li = num + 1)
				{
					RegionLink link = this.links[li];
					for (int ri = 0; ri < 2; ri = num + 1)
					{
						if (link.regions[ri] != null && link.regions[ri] != this && link.regions[ri].valid)
						{
							yield return link.regions[ri];
						}
						num = ri;
					}
					link = null;
					num = li;
				}
				yield break;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001220 RID: 4640 RVA: 0x00013221 File Offset: 0x00011421
		public IEnumerable<Region> NeighborsOfSameType
		{
			get
			{
				int num;
				for (int li = 0; li < this.links.Count; li = num + 1)
				{
					RegionLink link = this.links[li];
					for (int ri = 0; ri < 2; ri = num + 1)
					{
						if (link.regions[ri] != null && link.regions[ri] != this && link.regions[ri].type == this.type && link.regions[ri].valid)
						{
							yield return link.regions[ri];
						}
						num = ri;
					}
					link = null;
					num = li;
				}
				yield break;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x00013231 File Offset: 0x00011431
		// (set) Token: 0x06001222 RID: 4642 RVA: 0x00013239 File Offset: 0x00011439
		public Room Room
		{
			get
			{
				return this.roomInt;
			}
			set
			{
				if (value == this.roomInt)
				{
					return;
				}
				if (this.roomInt != null)
				{
					this.roomInt.RemoveRegion(this);
				}
				this.roomInt = value;
				if (this.roomInt != null)
				{
					this.roomInt.AddRegion(this);
				}
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x000C4F8C File Offset: 0x000C318C
		public IntVec3 RandomCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				for (int i = 0; i < 1000; i++)
				{
					IntVec3 randomCell = this.extentsClose.RandomCell;
					if (directGrid[cellIndices.CellToIndex(randomCell)] == this)
					{
						return randomCell;
					}
				}
				return this.AnyCell;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001224 RID: 4644 RVA: 0x000C4FE4 File Offset: 0x000C31E4
		public IntVec3 AnyCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				foreach (IntVec3 intVec in this.extentsClose)
				{
					if (directGrid[cellIndices.CellToIndex(intVec)] == this)
					{
						return intVec;
					}
				}
				Log.Error("Couldn't find any cell in region " + this.ToString(), false);
				return this.extentsClose.RandomCell;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x000C5080 File Offset: 0x000C3280
		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("id: " + this.id);
				stringBuilder.AppendLine("mapIndex: " + this.mapIndex);
				stringBuilder.AppendLine("links count: " + this.links.Count);
				foreach (RegionLink regionLink in this.links)
				{
					stringBuilder.AppendLine("  --" + regionLink.ToString());
				}
				stringBuilder.AppendLine("valid: " + this.valid.ToString());
				stringBuilder.AppendLine("makeTick: " + this.debug_makeTick);
				stringBuilder.AppendLine("roomID: " + ((this.Room != null) ? this.Room.ID.ToString() : "null room!"));
				stringBuilder.AppendLine("extentsClose: " + this.extentsClose);
				stringBuilder.AppendLine("extentsLimit: " + this.extentsLimit);
				stringBuilder.AppendLine("ListerThings:");
				if (this.listerThings.AllThings != null)
				{
					for (int i = 0; i < this.listerThings.AllThings.Count; i++)
					{
						stringBuilder.AppendLine("  --" + this.listerThings.AllThings[i]);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001226 RID: 4646 RVA: 0x00013274 File Offset: 0x00011474
		public bool DebugIsNew
		{
			get
			{
				return this.debug_makeTick > Find.TickManager.TicksGame - 60;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001227 RID: 4647 RVA: 0x0001328B File Offset: 0x0001148B
		public ListerThings ListerThings
		{
			get
			{
				return this.listerThings;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x00013293 File Offset: 0x00011493
		public bool IsDoorway
		{
			get
			{
				return this.door != null;
			}
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x000C5240 File Offset: 0x000C3440
		private Region()
		{
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x000C52D0 File Offset: 0x000C34D0
		public static Region MakeNewUnfilled(IntVec3 root, Map map)
		{
			Region region = new Region();
			region.debug_makeTick = Find.TickManager.TicksGame;
			region.id = Region.nextId;
			Region.nextId++;
			region.mapIndex = (sbyte)map.Index;
			region.precalculatedHashCode = Gen.HashCombineInt(region.id, 1295813358);
			region.extentsClose.minX = root.x;
			region.extentsClose.maxX = root.x;
			region.extentsClose.minZ = root.z;
			region.extentsClose.maxZ = root.z;
			region.extentsLimit.minX = root.x - root.x % 12;
			region.extentsLimit.maxX = root.x + 12 - (root.x + 12) % 12 - 1;
			region.extentsLimit.minZ = root.z - root.z % 12;
			region.extentsLimit.maxZ = root.z + 12 - (root.z + 12) % 12 - 1;
			region.extentsLimit.ClipInsideMap(map);
			return region;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x000C53FC File Offset: 0x000C35FC
		public bool Allows(TraverseParms tp, bool isDestination)
		{
			if (tp.mode != TraverseMode.PassAllDestroyableThings && tp.mode != TraverseMode.PassAllDestroyableThingsNotWater && !this.type.Passable())
			{
				return false;
			}
			if (tp.maxDanger < Danger.Deadly && tp.pawn != null)
			{
				Danger danger = this.DangerFor(tp.pawn);
				if (isDestination || danger == Danger.Deadly)
				{
					Region region = tp.pawn.GetRegion(RegionType.Set_All);
					if ((region == null || danger > region.DangerFor(tp.pawn)) && danger > tp.maxDanger)
					{
						return false;
					}
				}
			}
			switch (tp.mode)
			{
			case TraverseMode.ByPawn:
			{
				if (this.door == null)
				{
					return true;
				}
				ByteGrid avoidGrid = tp.pawn.GetAvoidGrid(true);
				if (avoidGrid != null && avoidGrid[this.door.Position] == 255)
				{
					return false;
				}
				if (tp.pawn.HostileTo(this.door))
				{
					return this.door.CanPhysicallyPass(tp.pawn) || tp.canBash;
				}
				return this.door.CanPhysicallyPass(tp.pawn) && !this.door.IsForbiddenToPass(tp.pawn);
			}
			case TraverseMode.PassDoors:
				return true;
			case TraverseMode.NoPassClosedDoors:
				return this.door == null || this.door.FreePassage;
			case TraverseMode.PassAllDestroyableThings:
				return true;
			case TraverseMode.NoPassClosedDoorsOrWater:
				return this.door == null || this.door.FreePassage;
			case TraverseMode.PassAllDestroyableThingsNotWater:
				return true;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x000C556C File Offset: 0x000C376C
		public Danger DangerFor(Pawn p)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (this.cachedDangersForFrame != Time.frameCount)
				{
					this.cachedDangers.Clear();
					this.cachedDangersForFrame = Time.frameCount;
				}
				else
				{
					for (int i = 0; i < this.cachedDangers.Count; i++)
					{
						if (this.cachedDangers[i].Key == p)
						{
							return this.cachedDangers[i].Value;
						}
					}
				}
			}
			float temperature = this.Room.Temperature;
			FloatRange value;
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Region.cachedSafeTemperatureRangesForFrame != Time.frameCount)
				{
					Region.cachedSafeTemperatureRanges.Clear();
					Region.cachedSafeTemperatureRangesForFrame = Time.frameCount;
				}
				if (!Region.cachedSafeTemperatureRanges.TryGetValue(p, out value))
				{
					value = p.SafeTemperatureRange();
					Region.cachedSafeTemperatureRanges.Add(p, value);
				}
			}
			else
			{
				value = p.SafeTemperatureRange();
			}
			Danger danger;
			if (value.Includes(temperature))
			{
				danger = Danger.None;
			}
			else if (value.ExpandedBy(80f).Includes(temperature))
			{
				danger = Danger.Some;
			}
			else
			{
				danger = Danger.Deadly;
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.cachedDangers.Add(new KeyValuePair<Pawn, Danger>(p, danger));
			}
			return danger;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x000C5690 File Offset: 0x000C3890
		public float GetBaseDesiredPlantsCount(bool allowCache = true)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (allowCache && ticksGame - this.cachedBaseDesiredPlantsCountForTick < 2500)
			{
				return this.cachedBaseDesiredPlantsCount;
			}
			this.cachedBaseDesiredPlantsCount = 0f;
			Map map = this.Map;
			foreach (IntVec3 c in this.Cells)
			{
				this.cachedBaseDesiredPlantsCount += map.wildPlantSpawner.GetBaseDesiredPlantsCountAt(c);
			}
			this.cachedBaseDesiredPlantsCountForTick = ticksGame;
			return this.cachedBaseDesiredPlantsCount;
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x000C5734 File Offset: 0x000C3934
		public AreaOverlap OverlapWith(Area a)
		{
			if (a.TrueCount == 0)
			{
				return AreaOverlap.None;
			}
			if (this.Map != a.Map)
			{
				return AreaOverlap.None;
			}
			if (this.cachedAreaOverlaps == null)
			{
				this.cachedAreaOverlaps = new Dictionary<Area, AreaOverlap>();
			}
			AreaOverlap areaOverlap;
			if (!this.cachedAreaOverlaps.TryGetValue(a, out areaOverlap))
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 c in this.Cells)
				{
					num2++;
					if (a[c])
					{
						num++;
					}
				}
				if (num == 0)
				{
					areaOverlap = AreaOverlap.None;
				}
				else if (num == num2)
				{
					areaOverlap = AreaOverlap.Entire;
				}
				else
				{
					areaOverlap = AreaOverlap.Partial;
				}
				this.cachedAreaOverlaps.Add(a, areaOverlap);
			}
			return areaOverlap;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0001329E File Offset: 0x0001149E
		public void Notify_AreaChanged(Area a)
		{
			if (this.cachedAreaOverlaps == null)
			{
				return;
			}
			if (this.cachedAreaOverlaps.ContainsKey(a))
			{
				this.cachedAreaOverlaps.Remove(a);
			}
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x000C57F0 File Offset: 0x000C39F0
		public void DecrementMapIndex()
		{
			if (this.mapIndex <= 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to decrement map index for region ",
					this.id,
					", but mapIndex=",
					this.mapIndex
				}), false);
				return;
			}
			this.mapIndex -= 1;
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x000132C4 File Offset: 0x000114C4
		public void Notify_MyMapRemoved()
		{
			this.listerThings.Clear();
			this.mapIndex = -1;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x000132D8 File Offset: 0x000114D8
		public static void ClearStaticData()
		{
			Region.cachedSafeTemperatureRanges.Clear();
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x000C5854 File Offset: 0x000C3A54
		public override string ToString()
		{
			string str;
			if (this.door != null)
			{
				str = this.door.ToString();
			}
			else
			{
				str = "null";
			}
			return string.Concat(new object[]
			{
				"Region(id=",
				this.id,
				", mapIndex=",
				this.mapIndex,
				", center=",
				this.extentsClose.CenterCell,
				", links=",
				this.links.Count,
				", cells=",
				this.CellCount,
				(this.door != null) ? (", portal=" + str) : null,
				")"
			});
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x000C5928 File Offset: 0x000C3B28
		public void DebugDraw()
		{
			if (DebugViewSettings.drawRegionTraversal && Find.TickManager.TicksGame < this.debug_lastTraverseTick + 60)
			{
				float a = 1f - (float)(Find.TickManager.TicksGame - this.debug_lastTraverseTick) / 60f;
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), new Color(0f, 0f, 1f, a));
			}
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x000C5998 File Offset: 0x000C3B98
		public void DebugDrawMouseover()
		{
			int num = Mathf.RoundToInt(Time.realtimeSinceStartup * 2f) % 2;
			if (DebugViewSettings.drawRegions)
			{
				Color color;
				if (!this.valid)
				{
					color = Color.red;
				}
				else if (this.DebugIsNew)
				{
					color = Color.yellow;
				}
				else
				{
					color = Color.green;
				}
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), color);
				foreach (Region region in this.Neighbors)
				{
					GenDraw.DrawFieldEdges(region.Cells.ToList<IntVec3>(), Color.grey);
				}
			}
			if (DebugViewSettings.drawRegionLinks)
			{
				foreach (RegionLink regionLink in this.links)
				{
					if (num == 1)
					{
						foreach (IntVec3 c in regionLink.span.Cells)
						{
							CellRenderer.RenderCell(c, DebugSolidColorMats.MaterialOf(Color.magenta));
						}
					}
				}
			}
			if (DebugViewSettings.drawRegionThings)
			{
				foreach (Thing thing in this.listerThings.AllThings)
				{
					CellRenderer.RenderSpot(thing.TrueCenter(), (float)(thing.thingIDNumber % 256) / 256f);
				}
			}
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x000132E4 File Offset: 0x000114E4
		public void Debug_Notify_Traversed()
		{
			this.debug_lastTraverseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x000132F6 File Offset: 0x000114F6
		public override int GetHashCode()
		{
			return this.precalculatedHashCode;
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x000C5B40 File Offset: 0x000C3D40
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Region region = obj as Region;
			return region != null && region.id == this.id;
		}

		// Token: 0x04000E92 RID: 3730
		public RegionType type = RegionType.Normal;

		// Token: 0x04000E93 RID: 3731
		public int id = -1;

		// Token: 0x04000E94 RID: 3732
		public sbyte mapIndex = -1;

		// Token: 0x04000E95 RID: 3733
		private Room roomInt;

		// Token: 0x04000E96 RID: 3734
		public List<RegionLink> links = new List<RegionLink>();

		// Token: 0x04000E97 RID: 3735
		public CellRect extentsClose;

		// Token: 0x04000E98 RID: 3736
		public CellRect extentsLimit;

		// Token: 0x04000E99 RID: 3737
		public Building_Door door;

		// Token: 0x04000E9A RID: 3738
		private int precalculatedHashCode;

		// Token: 0x04000E9B RID: 3739
		public bool touchesMapEdge;

		// Token: 0x04000E9C RID: 3740
		private int cachedCellCount = -1;

		// Token: 0x04000E9D RID: 3741
		public bool valid = true;

		// Token: 0x04000E9E RID: 3742
		private ListerThings listerThings = new ListerThings(ListerThingsUse.Region);

		// Token: 0x04000E9F RID: 3743
		public uint[] closedIndex = new uint[RegionTraverser.NumWorkers];

		// Token: 0x04000EA0 RID: 3744
		public uint reachedIndex;

		// Token: 0x04000EA1 RID: 3745
		public int newRegionGroupIndex = -1;

		// Token: 0x04000EA2 RID: 3746
		private Dictionary<Area, AreaOverlap> cachedAreaOverlaps;

		// Token: 0x04000EA3 RID: 3747
		public int mark;

		// Token: 0x04000EA4 RID: 3748
		private List<KeyValuePair<Pawn, Danger>> cachedDangers = new List<KeyValuePair<Pawn, Danger>>();

		// Token: 0x04000EA5 RID: 3749
		private int cachedDangersForFrame;

		// Token: 0x04000EA6 RID: 3750
		private float cachedBaseDesiredPlantsCount;

		// Token: 0x04000EA7 RID: 3751
		private int cachedBaseDesiredPlantsCountForTick = -999999;

		// Token: 0x04000EA8 RID: 3752
		private static Dictionary<Pawn, FloatRange> cachedSafeTemperatureRanges = new Dictionary<Pawn, FloatRange>();

		// Token: 0x04000EA9 RID: 3753
		private static int cachedSafeTemperatureRangesForFrame;

		// Token: 0x04000EAA RID: 3754
		private int debug_makeTick = -1000;

		// Token: 0x04000EAB RID: 3755
		private int debug_lastTraverseTick = -1000;

		// Token: 0x04000EAC RID: 3756
		private static int nextId = 1;

		// Token: 0x04000EAD RID: 3757
		public const int GridSize = 12;
	}
}
