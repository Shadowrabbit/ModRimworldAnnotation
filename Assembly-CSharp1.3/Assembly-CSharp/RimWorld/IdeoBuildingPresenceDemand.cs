using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EEB RID: 3819
	public class IdeoBuildingPresenceDemand : IExposable, ILoadReferenceable
	{
		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x06005AA5 RID: 23205 RVA: 0x001F58B8 File Offset: 0x001F3AB8
		public List<string> RoomRequirementsInfo
		{
			get
			{
				if (this.roomRequirements.NullOrEmpty<RoomRequirement>())
				{
					return null;
				}
				if (this.roomRequirementsInfoCached == null || Find.TickManager.TicksGame - this.roomRequirementsInfoCacheTick >= 20)
				{
					if (this.roomRequirementsInfoCached == null)
					{
						this.roomRequirementsInfoCached = new List<string>();
					}
					else
					{
						this.roomRequirementsInfoCached.Clear();
					}
					Thing building = (Find.CurrentMap != null) ? this.BestBuilding(Find.CurrentMap, false) : null;
					Room effectiveRoom = this.GetEffectiveRoom(building);
					foreach (RoomRequirement roomRequirement in this.roomRequirements)
					{
						if (effectiveRoom == null || !roomRequirement.MetOrDisabled(effectiveRoom, null))
						{
							this.roomRequirementsInfoCached.Add(roomRequirement.LabelCap(effectiveRoom));
						}
					}
					this.roomRequirementsInfoCacheTick = Find.TickManager.TicksGame;
				}
				return this.roomRequirementsInfoCached;
			}
		}

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x06005AA6 RID: 23206 RVA: 0x001F59A8 File Offset: 0x001F3BA8
		public Alert_IdeoBuildingMissing AlertCachedMissingMissing
		{
			get
			{
				if (this.alertCachedMissing == null)
				{
					this.alertCachedMissing = new Alert_IdeoBuildingMissing(this);
				}
				return this.alertCachedMissing;
			}
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06005AA7 RID: 23207 RVA: 0x001F59C4 File Offset: 0x001F3BC4
		public Alert_IdeoBuildingDisrespected AlertCachedMissingDisrespected
		{
			get
			{
				if (this.alertCachedDisrespected == null)
				{
					this.alertCachedDisrespected = new Alert_IdeoBuildingDisrespected(this);
				}
				return this.alertCachedDisrespected;
			}
		}

		// Token: 0x06005AA8 RID: 23208 RVA: 0x001F59E0 File Offset: 0x001F3BE0
		public IdeoBuildingPresenceDemand()
		{
		}

		// Token: 0x06005AA9 RID: 23209 RVA: 0x001F59F6 File Offset: 0x001F3BF6
		public IdeoBuildingPresenceDemand(Precept_Building precept)
		{
			this.parent = precept;
			this.ID = Find.UniqueIDsManager.GetNextPresenceDemandID();
		}

		// Token: 0x06005AAA RID: 23210 RVA: 0x001F5A24 File Offset: 0x001F3C24
		public bool AppliesTo(Map map)
		{
			return (this.parent.ThingDef.ritualFocus == null || !this.parent.ThingDef.ritualFocus.consumable) && (DebugSettings.activateAllBuildingDemands || ExpectationsUtility.CurrentExpectationFor(map).order >= this.minExpectation.order);
		}

		// Token: 0x06005AAB RID: 23211 RVA: 0x001F5A80 File Offset: 0x001F3C80
		public bool BuildingPresent(Map map)
		{
			return map.listerBuildings.ColonistsHaveBuilding((Thing t) => this.parent.ThingDef == t.def && t.StyleSourcePrecept == this.parent);
		}

		// Token: 0x06005AAC RID: 23212 RVA: 0x001F5A9C File Offset: 0x001F3C9C
		public bool RequirementsSatisfied(Map map)
		{
			if (this.roomRequirements.NullOrEmpty<RoomRequirement>())
			{
				return true;
			}
			foreach (Building building in map.listerBuildings.AllBuildingsColonistOfDef(this.parent.ThingDef))
			{
				if (building.StyleSourcePrecept == this.parent && building.GetRoom(RegionType.Set_All) != null && this.NumRequirementsMet(building) == this.roomRequirements.Count)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005AAD RID: 23213 RVA: 0x001F5B34 File Offset: 0x001F3D34
		private int NumRequirementsMet(Thing building)
		{
			Room effectiveRoom = this.GetEffectiveRoom(building);
			if (effectiveRoom == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.roomRequirements.Count; i++)
			{
				if (this.roomRequirements[i].MetOrDisabled(effectiveRoom, null))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06005AAE RID: 23214 RVA: 0x001F5B80 File Offset: 0x001F3D80
		public Room GetEffectiveRoom(Thing building)
		{
			Room room = (building != null) ? building.GetRoom(RegionType.Set_All) : null;
			if (room == null || room.PsychologicallyOutdoors)
			{
				return null;
			}
			return room;
		}

		// Token: 0x06005AAF RID: 23215 RVA: 0x001F5BAC File Offset: 0x001F3DAC
		public Thing BestBuilding(Map map, bool ignoreFullyMetRequirements = false)
		{
			Thing result = null;
			int num = -1;
			foreach (Building building in map.listerBuildings.AllBuildingsColonistOfDef(this.parent.ThingDef))
			{
				if (building.StyleSourcePrecept == this.parent)
				{
					int num2 = this.NumRequirementsMet(building);
					if (this.roomRequirements == null || num2 != this.roomRequirements.Count || !ignoreFullyMetRequirements)
					{
						if (this.GetEffectiveRoom(building) != null)
						{
							num2++;
						}
						if (num2 > num)
						{
							result = building;
							num = num2;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005AB0 RID: 23216 RVA: 0x001F5C58 File Offset: 0x001F3E58
		public IEnumerable<Thing> AllBuildings(Map map)
		{
			foreach (Building building in map.listerBuildings.AllBuildingsColonistOfDef(this.parent.ThingDef))
			{
				if (building.StyleSourcePrecept == this.parent)
				{
					yield return building;
				}
			}
			IEnumerator<Building> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005AB1 RID: 23217 RVA: 0x001F5C6F File Offset: 0x001F3E6F
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Defs.Look<ExpectationDef>(ref this.minExpectation, "minExpectation");
			Scribe_Collections.Look<RoomRequirement>(ref this.roomRequirements, "roomRequirements", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06005AB2 RID: 23218 RVA: 0x001F5CA9 File Offset: 0x001F3EA9
		public string GetUniqueLoadID()
		{
			return "IdeoBuildingPresenceDemand_" + this.ID;
		}

		// Token: 0x04003511 RID: 13585
		public Precept_Building parent;

		// Token: 0x04003512 RID: 13586
		public ExpectationDef minExpectation;

		// Token: 0x04003513 RID: 13587
		public List<RoomRequirement> roomRequirements;

		// Token: 0x04003514 RID: 13588
		public int ID = -1;

		// Token: 0x04003515 RID: 13589
		private List<string> roomRequirementsInfoCached;

		// Token: 0x04003516 RID: 13590
		private int roomRequirementsInfoCacheTick = -1;

		// Token: 0x04003517 RID: 13591
		private Alert_IdeoBuildingMissing alertCachedMissing;

		// Token: 0x04003518 RID: 13592
		private Alert_IdeoBuildingDisrespected alertCachedDisrespected;
	}
}
