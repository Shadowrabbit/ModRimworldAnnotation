using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001FD RID: 509
	public sealed class Region
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x00050ACC File Offset: 0x0004ECCC
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

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x00050AE9 File Offset: 0x0004ECE9
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

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x00050AFC File Offset: 0x0004ECFC
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

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x00050B8D File Offset: 0x0004ED8D
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

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x00050B9D File Offset: 0x0004ED9D
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

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x00050BAD File Offset: 0x0004EDAD
		public Room Room
		{
			get
			{
				District district = this.District;
				if (district == null)
				{
					return null;
				}
				return district.Room;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x00050BC0 File Offset: 0x0004EDC0
		// (set) Token: 0x06000E50 RID: 3664 RVA: 0x00050BC8 File Offset: 0x0004EDC8
		public District District
		{
			get
			{
				return this.districtInt;
			}
			set
			{
				if (value == this.districtInt)
				{
					return;
				}
				if (this.districtInt != null)
				{
					this.districtInt.RemoveRegion(this);
				}
				this.districtInt = value;
				if (this.districtInt != null)
				{
					this.districtInt.AddRegion(this);
				}
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x00050C04 File Offset: 0x0004EE04
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

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x00050C5C File Offset: 0x0004EE5C
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
				Log.Error("Couldn't find any cell in region " + this.ToString());
				return this.extentsClose.RandomCell;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x00050CF4 File Offset: 0x0004EEF4
		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("id: " + this.id);
				stringBuilder.AppendLine("mapIndex: " + this.mapIndex);
				stringBuilder.AppendLine("links count: " + this.links.Count);
				stringBuilder.AppendLine("type: " + this.type);
				foreach (RegionLink regionLink in this.links)
				{
					stringBuilder.AppendLine("  --" + regionLink.ToString());
				}
				stringBuilder.AppendLine("valid: " + this.valid.ToString());
				stringBuilder.AppendLine("makeTick: " + this.debug_makeTick);
				stringBuilder.AppendLine("districtID: " + ((this.District != null) ? this.District.ID.ToString() : "null district!"));
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

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x00050F00 File Offset: 0x0004F100
		public bool DebugIsNew
		{
			get
			{
				return this.debug_makeTick > Find.TickManager.TicksGame - 60;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x00050F17 File Offset: 0x0004F117
		public ListerThings ListerThings
		{
			get
			{
				return this.listerThings;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x00050F1F File Offset: 0x0004F11F
		public bool IsDoorway
		{
			get
			{
				return this.door != null;
			}
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00050F2C File Offset: 0x0004F12C
		private Region()
		{
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00050FBC File Offset: 0x0004F1BC
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

		// Token: 0x06000E59 RID: 3673 RVA: 0x000510E8 File Offset: 0x0004F2E8
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
			bool flag = this.type == RegionType.Fence && tp.fenceBlocked && !tp.canBashFences;
			switch (tp.mode)
			{
			case TraverseMode.ByPawn:
			{
				if (this.door == null)
				{
					return !flag;
				}
				ByteGrid avoidGrid = tp.pawn.GetAvoidGrid(true);
				if (avoidGrid != null && avoidGrid[this.door.Position] == 255)
				{
					return false;
				}
				if (tp.pawn.HostileTo(this.door))
				{
					return this.door.CanPhysicallyPass(tp.pawn) || tp.canBashDoors;
				}
				return this.door.CanPhysicallyPass(tp.pawn) && !this.door.IsForbiddenToPass(tp.pawn);
			}
			case TraverseMode.PassDoors:
				return !flag;
			case TraverseMode.NoPassClosedDoors:
			case TraverseMode.NoPassClosedDoorsOrWater:
				return (this.door == null || this.door.FreePassage) && !flag;
			case TraverseMode.PassAllDestroyableThings:
				return true;
			case TraverseMode.PassAllDestroyableThingsNotWater:
				return true;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00051274 File Offset: 0x0004F474
		public Danger DangerFor(Pawn p)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (this.cachedDangersForFrame != RealTime.frameCount)
				{
					this.cachedDangers.Clear();
					this.cachedDangersForFrame = RealTime.frameCount;
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
				if (Region.cachedSafeTemperatureRangesForFrame != RealTime.frameCount)
				{
					Region.cachedSafeTemperatureRanges.Clear();
					Region.cachedSafeTemperatureRangesForFrame = RealTime.frameCount;
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

		// Token: 0x06000E5B RID: 3675 RVA: 0x00051398 File Offset: 0x0004F598
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

		// Token: 0x06000E5C RID: 3676 RVA: 0x0005143C File Offset: 0x0004F63C
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

		// Token: 0x06000E5D RID: 3677 RVA: 0x000514F8 File Offset: 0x0004F6F8
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

		// Token: 0x06000E5E RID: 3678 RVA: 0x00051520 File Offset: 0x0004F720
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
				}));
				return;
			}
			this.mapIndex -= 1;
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x00051582 File Offset: 0x0004F782
		public void Notify_MyMapRemoved()
		{
			this.listerThings.Clear();
			this.mapIndex = -1;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00051596 File Offset: 0x0004F796
		public static void ClearStaticData()
		{
			Region.cachedSafeTemperatureRanges.Clear();
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x000515A4 File Offset: 0x0004F7A4
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

		// Token: 0x06000E62 RID: 3682 RVA: 0x00051678 File Offset: 0x0004F878
		public void DebugDraw()
		{
			if (DebugViewSettings.drawRegionTraversal && Find.TickManager.TicksGame < this.debug_lastTraverseTick + 60)
			{
				float a = 1f - (float)(Find.TickManager.TicksGame - this.debug_lastTraverseTick) / 60f;
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), new Color(0f, 0f, 1f, a), null);
			}
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x000516F0 File Offset: 0x0004F8F0
		public void DebugDrawMouseover()
		{
			int num = Mathf.RoundToInt(Time.realtimeSinceStartup * 2f) % 2;
			if (DebugViewSettings.drawRegions)
			{
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), this.DebugColor(), null);
				foreach (Region region in this.Neighbors)
				{
					GenDraw.DrawFieldEdges(region.Cells.ToList<IntVec3>(), Color.grey, null);
				}
			}
			if (DebugViewSettings.drawRegionLinks)
			{
				foreach (RegionLink regionLink in this.links)
				{
					if (num == 1)
					{
						List<IntVec3> list = regionLink.span.Cells.ToList<IntVec3>();
						Material mat = DebugSolidColorMats.MaterialOf(Color.magenta * new Color(1f, 1f, 1f, 0.25f));
						foreach (IntVec3 c in list)
						{
							CellRenderer.RenderCell(c, mat);
						}
						GenDraw.DrawFieldEdges(list, Color.white, null);
					}
				}
			}
			if (DebugViewSettings.drawRegionThings)
			{
				foreach (Thing thing in this.listerThings.AllThings)
				{
					CellRenderer.RenderSpot(thing.TrueCenter(), (float)(thing.thingIDNumber % 256) / 256f, 0.15f);
				}
			}
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x000518DC File Offset: 0x0004FADC
		private Color DebugColor()
		{
			Color result;
			if (!this.valid)
			{
				result = Color.red;
			}
			else if (this.DebugIsNew)
			{
				result = Color.yellow;
			}
			else
			{
				result = Color.green;
			}
			return result;
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00051910 File Offset: 0x0004FB10
		public void Debug_Notify_Traversed()
		{
			this.debug_lastTraverseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00051922 File Offset: 0x0004FB22
		public override int GetHashCode()
		{
			return this.precalculatedHashCode;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0005192C File Offset: 0x0004FB2C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Region region = obj as Region;
			return region != null && region.id == this.id;
		}

		// Token: 0x04000B93 RID: 2963
		public RegionType type = RegionType.Normal;

		// Token: 0x04000B94 RID: 2964
		public int id = -1;

		// Token: 0x04000B95 RID: 2965
		public sbyte mapIndex = -1;

		// Token: 0x04000B96 RID: 2966
		private District districtInt;

		// Token: 0x04000B97 RID: 2967
		public List<RegionLink> links = new List<RegionLink>();

		// Token: 0x04000B98 RID: 2968
		public CellRect extentsClose;

		// Token: 0x04000B99 RID: 2969
		public CellRect extentsLimit;

		// Token: 0x04000B9A RID: 2970
		public Building_Door door;

		// Token: 0x04000B9B RID: 2971
		private int precalculatedHashCode;

		// Token: 0x04000B9C RID: 2972
		public bool touchesMapEdge;

		// Token: 0x04000B9D RID: 2973
		private int cachedCellCount = -1;

		// Token: 0x04000B9E RID: 2974
		public bool valid = true;

		// Token: 0x04000B9F RID: 2975
		private ListerThings listerThings = new ListerThings(ListerThingsUse.Region);

		// Token: 0x04000BA0 RID: 2976
		public uint[] closedIndex = new uint[RegionTraverser.NumWorkers];

		// Token: 0x04000BA1 RID: 2977
		public uint reachedIndex;

		// Token: 0x04000BA2 RID: 2978
		public int newRegionGroupIndex = -1;

		// Token: 0x04000BA3 RID: 2979
		private Dictionary<Area, AreaOverlap> cachedAreaOverlaps;

		// Token: 0x04000BA4 RID: 2980
		public int mark;

		// Token: 0x04000BA5 RID: 2981
		private List<KeyValuePair<Pawn, Danger>> cachedDangers = new List<KeyValuePair<Pawn, Danger>>();

		// Token: 0x04000BA6 RID: 2982
		private int cachedDangersForFrame;

		// Token: 0x04000BA7 RID: 2983
		private float cachedBaseDesiredPlantsCount;

		// Token: 0x04000BA8 RID: 2984
		private int cachedBaseDesiredPlantsCountForTick = -999999;

		// Token: 0x04000BA9 RID: 2985
		private static Dictionary<Pawn, FloatRange> cachedSafeTemperatureRanges = new Dictionary<Pawn, FloatRange>();

		// Token: 0x04000BAA RID: 2986
		private static int cachedSafeTemperatureRangesForFrame;

		// Token: 0x04000BAB RID: 2987
		private int debug_makeTick = -1000;

		// Token: 0x04000BAC RID: 2988
		private int debug_lastTraverseTick = -1000;

		// Token: 0x04000BAD RID: 2989
		private static int nextId = 1;

		// Token: 0x04000BAE RID: 2990
		public const int GridSize = 12;
	}
}
