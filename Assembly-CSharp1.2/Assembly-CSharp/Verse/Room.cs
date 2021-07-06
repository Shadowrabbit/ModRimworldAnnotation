using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002EB RID: 747
	public sealed class Room
	{
		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x00013818 File Offset: 0x00011A18
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

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060012EA RID: 4842 RVA: 0x00013835 File Offset: 0x00011A35
		public RegionType RegionType
		{
			get
			{
				if (!this.regions.Any<Region>())
				{
					return RegionType.None;
				}
				return this.regions[0].type;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x00013857 File Offset: 0x00011A57
		public List<Region> Regions
		{
			get
			{
				return this.regions;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x0001385F File Offset: 0x00011A5F
		public int RegionCount
		{
			get
			{
				return this.regions.Count;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x0001386C File Offset: 0x00011A6C
		public bool IsHuge
		{
			get
			{
				return this.regions.Count > 60;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x0001387D File Offset: 0x00011A7D
		public bool Dereferenced
		{
			get
			{
				return this.regions.Count == 0;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x0001388D File Offset: 0x00011A8D
		public bool TouchesMapEdge
		{
			get
			{
				return this.numRegionsTouchingMapEdge > 0;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x00013898 File Offset: 0x00011A98
		public float Temperature
		{
			get
			{
				return this.Group.Temperature;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x000138A5 File Offset: 0x00011AA5
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.Group.UsesOutdoorTemperature;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x000138B2 File Offset: 0x00011AB2
		// (set) Token: 0x060012F3 RID: 4851 RVA: 0x000138BA File Offset: 0x00011ABA
		public RoomGroup Group
		{
			get
			{
				return this.groupInt;
			}
			set
			{
				if (value == this.groupInt)
				{
					return;
				}
				if (this.groupInt != null)
				{
					this.groupInt.RemoveRoom(this);
				}
				this.groupInt = value;
				if (this.groupInt != null)
				{
					this.groupInt.AddRoom(this);
				}
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x000C8844 File Offset: 0x000C6A44
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.regions.Count; i++)
					{
						this.cachedCellCount += this.regions[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x000138F5 File Offset: 0x00011AF5
		public int OpenRoofCount
		{
			get
			{
				return this.OpenRoofCountStopAt(int.MaxValue);
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060012F6 RID: 4854 RVA: 0x00013902 File Offset: 0x00011B02
		public bool PsychologicallyOutdoors
		{
			get
			{
				return this.OpenRoofCountStopAt(300) >= 300 || (this.Group.AnyRoomTouchesMapEdge && (float)this.OpenRoofCount / (float)this.CellCount >= 0.5f);
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x0001393E File Offset: 0x00011B3E
		public bool OutdoorsForWork
		{
			get
			{
				return this.OpenRoofCountStopAt(101) > 100 || (float)this.OpenRoofCount > (float)this.CellCount * 0.25f;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x000C889C File Offset: 0x000C6A9C
		public List<Room> Neighbors
		{
			get
			{
				this.uniqueNeighborsSet.Clear();
				this.uniqueNeighbors.Clear();
				for (int i = 0; i < this.regions.Count; i++)
				{
					foreach (Region region in this.regions[i].Neighbors)
					{
						if (this.uniqueNeighborsSet.Add(region.Room) && region.Room != this)
						{
							this.uniqueNeighbors.Add(region.Room);
						}
					}
				}
				this.uniqueNeighborsSet.Clear();
				return this.uniqueNeighbors;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x00013965 File Offset: 0x00011B65
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.regions.Count; i = num + 1)
				{
					foreach (IntVec3 intVec in this.regions[i].Cells)
					{
						yield return intVec;
					}
					IEnumerator<IntVec3> enumerator = null;
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x00013975 File Offset: 0x00011B75
		public IEnumerable<IntVec3> BorderCells
		{
			get
			{
				foreach (IntVec3 c in this.Cells)
				{
					int num;
					for (int i = 0; i < 8; i = num)
					{
						IntVec3 intVec = c + GenAdj.AdjacentCells[i];
						Region region = (c + GenAdj.AdjacentCells[i]).GetRegion(this.Map, RegionType.Set_Passable);
						if (region == null || region.Room != this)
						{
							yield return intVec;
						}
						num = i + 1;
					}
				}
				IEnumerator<IntVec3> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x00013985 File Offset: 0x00011B85
		public IEnumerable<Pawn> Owners
		{
			get
			{
				if (this.TouchesMapEdge)
				{
					yield break;
				}
				if (this.IsHuge)
				{
					yield break;
				}
				if (this.Role != RoomRoleDefOf.Bedroom && this.Role != RoomRoleDefOf.PrisonCell && this.Role != RoomRoleDefOf.Barracks && this.Role != RoomRoleDefOf.PrisonBarracks)
				{
					yield break;
				}
				Pawn pawn = null;
				Pawn secondOwner = null;
				foreach (Building_Bed building_Bed in this.ContainedBeds)
				{
					if (building_Bed.def.building.bed_humanlike)
					{
						for (int i = 0; i < building_Bed.OwnersForReading.Count; i++)
						{
							if (pawn == null)
							{
								pawn = building_Bed.OwnersForReading[i];
							}
							else
							{
								if (secondOwner != null)
								{
									yield break;
								}
								secondOwner = building_Bed.OwnersForReading[i];
							}
						}
					}
				}
				if (pawn != null)
				{
					if (secondOwner == null)
					{
						yield return pawn;
					}
					else if (LovePartnerRelationUtility.LovePartnerRelationExists(pawn, secondOwner))
					{
						yield return pawn;
						yield return secondOwner;
					}
				}
				yield break;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060012FC RID: 4860 RVA: 0x00013995 File Offset: 0x00011B95
		public IEnumerable<Building_Bed> ContainedBeds
		{
			get
			{
				List<Thing> things = this.ContainedAndAdjacentThings;
				int num;
				for (int i = 0; i < things.Count; i = num + 1)
				{
					Building_Bed building_Bed = things[i] as Building_Bed;
					if (building_Bed != null)
					{
						yield return building_Bed;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x000139A5 File Offset: 0x00011BA5
		public bool Fogged
		{
			get
			{
				return this.regions.Count != 0 && this.regions[0].AnyCell.Fogged(this.Map);
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060012FE RID: 4862 RVA: 0x000139D2 File Offset: 0x00011BD2
		public bool IsDoorway
		{
			get
			{
				return this.regions.Count == 1 && this.regions[0].IsDoorway;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x000C8958 File Offset: 0x000C6B58
		public List<Thing> ContainedAndAdjacentThings
		{
			get
			{
				this.uniqueContainedThingsSet.Clear();
				this.uniqueContainedThings.Clear();
				for (int i = 0; i < this.regions.Count; i++)
				{
					List<Thing> allThings = this.regions[i].ListerThings.AllThings;
					if (allThings != null)
					{
						for (int j = 0; j < allThings.Count; j++)
						{
							Thing item = allThings[j];
							if (this.uniqueContainedThingsSet.Add(item))
							{
								this.uniqueContainedThings.Add(item);
							}
						}
					}
				}
				this.uniqueContainedThingsSet.Clear();
				return this.uniqueContainedThings;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001300 RID: 4864 RVA: 0x000139F5 File Offset: 0x00011BF5
		public RoomRoleDef Role
		{
			get
			{
				if (this.statsAndRoleDirty)
				{
					this.UpdateRoomStatsAndRole();
				}
				return this.role;
			}
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00013A0B File Offset: 0x00011C0B
		public static Room MakeNew(Map map)
		{
			Room room = new Room();
			room.mapIndex = (sbyte)map.Index;
			room.ID = Room.nextRoomID;
			Room.nextRoomID++;
			return room;
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x000C89F0 File Offset: 0x000C6BF0
		public void AddRegion(Region r)
		{
			if (this.regions.Contains(r))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add the same region twice to Room. region=",
					r,
					", room=",
					this
				}), false);
				return;
			}
			this.regions.Add(r);
			if (r.touchesMapEdge)
			{
				this.numRegionsTouchingMapEdge++;
			}
			if (this.regions.Count == 1)
			{
				this.Map.regionGrid.allRooms.Add(this);
			}
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x000C8A7C File Offset: 0x000C6C7C
		public void RemoveRegion(Region r)
		{
			if (!this.regions.Contains(r))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to remove region from Room but this region is not here. region=",
					r,
					", room=",
					this
				}), false);
				return;
			}
			this.regions.Remove(r);
			if (r.touchesMapEdge)
			{
				this.numRegionsTouchingMapEdge--;
			}
			if (this.regions.Count == 0)
			{
				this.Group = null;
				this.cachedOpenRoofCount = -1;
				this.cachedOpenRoofState = null;
				this.statsAndRoleDirty = true;
				this.Map.regionGrid.allRooms.Remove(this);
			}
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x00013A36 File Offset: 0x00011C36
		public void Notify_MyMapRemoved()
		{
			this.mapIndex = -1;
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x000C8B24 File Offset: 0x000C6D24
		public void Notify_ContainedThingSpawnedOrDespawned(Thing th)
		{
			if (th.def.category != ThingCategory.Mote && th.def.category != ThingCategory.Projectile && th.def.category != ThingCategory.Ethereal && th.def.category != ThingCategory.Pawn)
			{
				if (this.IsDoorway)
				{
					for (int i = 0; i < this.regions[0].links.Count; i++)
					{
						Region otherRegion = this.regions[0].links[i].GetOtherRegion(this.regions[0]);
						if (otherRegion != null && !otherRegion.IsDoorway)
						{
							otherRegion.Room.Notify_ContainedThingSpawnedOrDespawned(th);
						}
					}
				}
				this.statsAndRoleDirty = true;
			}
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x00013A3F File Offset: 0x00011C3F
		public void Notify_TerrainChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x00013A3F File Offset: 0x00011C3F
		public void Notify_BedTypeChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00013A48 File Offset: 0x00011C48
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			this.Group.Notify_RoofChanged();
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x000C8BE4 File Offset: 0x000C6DE4
		public void Notify_RoomShapeOrContainedBedsChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			if (Current.ProgramState == ProgramState.Playing && !this.Fogged)
			{
				this.Map.autoBuildRoofAreaSetter.TryGenerateAreaFor(this);
			}
			this.isPrisonCell = false;
			if (Building_Bed.RoomCanBePrisonCell(this))
			{
				List<Thing> containedAndAdjacentThings = this.ContainedAndAdjacentThings;
				for (int i = 0; i < containedAndAdjacentThings.Count; i++)
				{
					Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
					if (building_Bed != null && building_Bed.ForPrisoners)
					{
						this.isPrisonCell = true;
						break;
					}
				}
			}
			List<Thing> list = this.Map.listerThings.ThingsOfDef(ThingDefOf.NutrientPasteDispenser);
			for (int j = 0; j < list.Count; j++)
			{
				list[j].Notify_ColorChanged();
			}
			if (Current.ProgramState == ProgramState.Playing && this.isPrisonCell)
			{
				foreach (Building_Bed building_Bed2 in this.ContainedBeds)
				{
					building_Bed2.ForPrisoners = true;
				}
			}
			this.lastChangeTick = Find.TickManager.TicksGame;
			this.statsAndRoleDirty = true;
			FacilitiesUtility.NotifyFacilitiesAboutChangedLOSBlockers(this.regions);
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x00013A63 File Offset: 0x00011C63
		public bool ContainsCell(IntVec3 cell)
		{
			return this.Map != null && cell.GetRoom(this.Map, RegionType.Set_All) == this;
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000C8D1C File Offset: 0x000C6F1C
		public bool ContainsThing(ThingDef def)
		{
			for (int i = 0; i < this.regions.Count; i++)
			{
				if (this.regions[i].ListerThings.ThingsOfDef(def).Any<Thing>())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x00013A7F File Offset: 0x00011C7F
		public IEnumerable<Thing> ContainedThings(ThingDef def)
		{
			this.uniqueContainedThingsOfDef.Clear();
			int num;
			for (int i = 0; i < this.regions.Count; i = num)
			{
				List<Thing> things = this.regions[i].ListerThings.ThingsOfDef(def);
				for (int j = 0; j < things.Count; j = num)
				{
					if (this.uniqueContainedThingsOfDef.Add(things[j]))
					{
						yield return things[j];
					}
					num = j + 1;
				}
				things = null;
				num = i + 1;
			}
			this.uniqueContainedThingsOfDef.Clear();
			yield break;
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x000C8D60 File Offset: 0x000C6F60
		public int ThingCount(ThingDef def)
		{
			this.uniqueContainedThingsOfDef.Clear();
			int num = 0;
			for (int i = 0; i < this.regions.Count; i++)
			{
				List<Thing> list = this.regions[i].ListerThings.ThingsOfDef(def);
				for (int j = 0; j < list.Count; j++)
				{
					if (this.uniqueContainedThingsOfDef.Add(list[j]))
					{
						num += list[j].stackCount;
					}
				}
			}
			this.uniqueContainedThingsOfDef.Clear();
			return num;
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x000C8DE8 File Offset: 0x000C6FE8
		public void DecrementMapIndex()
		{
			if (this.mapIndex <= 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to decrement map index for room ",
					this.ID,
					", but mapIndex=",
					this.mapIndex
				}), false);
				return;
			}
			this.mapIndex -= 1;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00013A96 File Offset: 0x00011C96
		public float GetStat(RoomStatDef roomStat)
		{
			if (this.statsAndRoleDirty)
			{
				this.UpdateRoomStatsAndRole();
			}
			if (this.stats == null)
			{
				return roomStat.roomlessScore;
			}
			return this.stats[roomStat];
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x00013AC1 File Offset: 0x00011CC1
		public RoomStatScoreStage GetStatScoreStage(RoomStatDef stat)
		{
			return stat.GetScoreStage(this.GetStat(stat));
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x000C8E4C File Offset: 0x000C704C
		public void DrawFieldEdges()
		{
			Room.fields.Clear();
			Room.fields.AddRange(this.Cells);
			Color color = this.isPrisonCell ? Room.PrisonFieldColor : Room.NonPrisonFieldColor;
			color.a = Pulser.PulseBrightness(1f, 0.6f);
			GenDraw.DrawFieldEdges(Room.fields, color);
			Room.fields.Clear();
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x000C8EB4 File Offset: 0x000C70B4
		public int OpenRoofCountStopAt(int threshold)
		{
			if (this.cachedOpenRoofCount == -1 && this.cachedOpenRoofState == null)
			{
				this.cachedOpenRoofCount = 0;
				this.cachedOpenRoofState = this.Cells.GetEnumerator();
			}
			if (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState != null)
			{
				RoofGrid roofGrid = this.Map.roofGrid;
				while (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState.MoveNext())
				{
					if (!roofGrid.Roofed(this.cachedOpenRoofState.Current))
					{
						this.cachedOpenRoofCount++;
					}
				}
				if (this.cachedOpenRoofCount < threshold)
				{
					this.cachedOpenRoofState = null;
				}
			}
			return this.cachedOpenRoofCount;
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x000C8F58 File Offset: 0x000C7158
		private void UpdateRoomStatsAndRole()
		{
			this.statsAndRoleDirty = false;
			if (!this.TouchesMapEdge && this.RegionType == RegionType.Normal && this.regions.Count <= 36)
			{
				if (this.stats == null)
				{
					this.stats = new DefMap<RoomStatDef, float>();
				}
				foreach (RoomStatDef roomStatDef in from x in DefDatabase<RoomStatDef>.AllDefs
				orderby x.updatePriority descending
				select x)
				{
					this.stats[roomStatDef] = roomStatDef.Worker.GetScore(this);
				}
				this.role = DefDatabase<RoomRoleDef>.AllDefs.MaxBy((RoomRoleDef x) => x.Worker.GetScore(this));
				return;
			}
			this.stats = null;
			this.role = RoomRoleDefOf.None;
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x000C904C File Offset: 0x000C724C
		internal void DebugDraw()
		{
			int hashCode = this.GetHashCode();
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)hashCode * 0.01f);
			}
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x000C90A8 File Offset: 0x000C72A8
		internal string DebugString()
		{
			return string.Concat(new object[]
			{
				"Room ID=",
				this.ID,
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>(),
				"\n  RegionCount=",
				this.RegionCount,
				"\n  RegionType=",
				this.RegionType,
				"\n  CellCount=",
				this.CellCount,
				"\n  OpenRoofCount=",
				this.OpenRoofCount,
				"\n  numRegionsTouchingMapEdge=",
				this.numRegionsTouchingMapEdge,
				"\n  lastChangeTick=",
				this.lastChangeTick,
				"\n  isPrisonCell=",
				this.isPrisonCell.ToString(),
				"\n  RoomGroup=",
				(this.Group != null) ? this.Group.ID.ToString() : "null"
			});
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x000C91C4 File Offset: 0x000C73C4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Room(roomID=",
				this.ID,
				", first=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				", RegionsCount=",
				this.RegionCount.ToString(),
				", lastChangeTick=",
				this.lastChangeTick,
				")"
			});
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00013AD0 File Offset: 0x00011CD0
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478890);
		}

		// Token: 0x04000F19 RID: 3865
		public sbyte mapIndex = -1;

		// Token: 0x04000F1A RID: 3866
		private RoomGroup groupInt;

		// Token: 0x04000F1B RID: 3867
		private List<Region> regions = new List<Region>();

		// Token: 0x04000F1C RID: 3868
		public int ID = -16161616;

		// Token: 0x04000F1D RID: 3869
		public int lastChangeTick = -1;

		// Token: 0x04000F1E RID: 3870
		private int numRegionsTouchingMapEdge;

		// Token: 0x04000F1F RID: 3871
		private int cachedOpenRoofCount = -1;

		// Token: 0x04000F20 RID: 3872
		private IEnumerator<IntVec3> cachedOpenRoofState;

		// Token: 0x04000F21 RID: 3873
		public bool isPrisonCell;

		// Token: 0x04000F22 RID: 3874
		private int cachedCellCount = -1;

		// Token: 0x04000F23 RID: 3875
		private bool statsAndRoleDirty = true;

		// Token: 0x04000F24 RID: 3876
		private DefMap<RoomStatDef, float> stats = new DefMap<RoomStatDef, float>();

		// Token: 0x04000F25 RID: 3877
		private RoomRoleDef role;

		// Token: 0x04000F26 RID: 3878
		public int newOrReusedRoomGroupIndex = -1;

		// Token: 0x04000F27 RID: 3879
		private static int nextRoomID;

		// Token: 0x04000F28 RID: 3880
		private const int RegionCountHuge = 60;

		// Token: 0x04000F29 RID: 3881
		private const int MaxRegionsToAssignRoomRole = 36;

		// Token: 0x04000F2A RID: 3882
		private static readonly Color PrisonFieldColor = new Color(1f, 0.7f, 0.2f);

		// Token: 0x04000F2B RID: 3883
		private static readonly Color NonPrisonFieldColor = new Color(0.3f, 0.3f, 1f);

		// Token: 0x04000F2C RID: 3884
		private HashSet<Room> uniqueNeighborsSet = new HashSet<Room>();

		// Token: 0x04000F2D RID: 3885
		private List<Room> uniqueNeighbors = new List<Room>();

		// Token: 0x04000F2E RID: 3886
		private HashSet<Thing> uniqueContainedThingsSet = new HashSet<Thing>();

		// Token: 0x04000F2F RID: 3887
		private List<Thing> uniqueContainedThings = new List<Thing>();

		// Token: 0x04000F30 RID: 3888
		private HashSet<Thing> uniqueContainedThingsOfDef = new HashSet<Thing>();

		// Token: 0x04000F31 RID: 3889
		private static List<IntVec3> fields = new List<IntVec3>();
	}
}
