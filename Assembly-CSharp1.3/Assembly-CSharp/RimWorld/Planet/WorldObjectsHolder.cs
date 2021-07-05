using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001800 RID: 6144
	public class WorldObjectsHolder : IExposable
	{
		// Token: 0x1700177E RID: 6014
		// (get) Token: 0x06008F74 RID: 36724 RVA: 0x00335A6C File Offset: 0x00333C6C
		public List<WorldObject> AllWorldObjects
		{
			get
			{
				return this.worldObjects;
			}
		}

		// Token: 0x1700177F RID: 6015
		// (get) Token: 0x06008F75 RID: 36725 RVA: 0x00335A74 File Offset: 0x00333C74
		public List<Caravan> Caravans
		{
			get
			{
				return this.caravans;
			}
		}

		// Token: 0x17001780 RID: 6016
		// (get) Token: 0x06008F76 RID: 36726 RVA: 0x00335A7C File Offset: 0x00333C7C
		public List<Settlement> Settlements
		{
			get
			{
				return this.settlements;
			}
		}

		// Token: 0x17001781 RID: 6017
		// (get) Token: 0x06008F77 RID: 36727 RVA: 0x00335A84 File Offset: 0x00333C84
		public List<TravelingTransportPods> TravelingTransportPods
		{
			get
			{
				return this.travelingTransportPods;
			}
		}

		// Token: 0x17001782 RID: 6018
		// (get) Token: 0x06008F78 RID: 36728 RVA: 0x00335A8C File Offset: 0x00333C8C
		public List<Settlement> SettlementBases
		{
			get
			{
				return this.settlementBases;
			}
		}

		// Token: 0x17001783 RID: 6019
		// (get) Token: 0x06008F79 RID: 36729 RVA: 0x00335A94 File Offset: 0x00333C94
		public List<DestroyedSettlement> DestroyedSettlements
		{
			get
			{
				return this.destroyedSettlements;
			}
		}

		// Token: 0x17001784 RID: 6020
		// (get) Token: 0x06008F7A RID: 36730 RVA: 0x00335A9C File Offset: 0x00333C9C
		public List<RoutePlannerWaypoint> RoutePlannerWaypoints
		{
			get
			{
				return this.routePlannerWaypoints;
			}
		}

		// Token: 0x17001785 RID: 6021
		// (get) Token: 0x06008F7B RID: 36731 RVA: 0x00335AA4 File Offset: 0x00333CA4
		public List<MapParent> MapParents
		{
			get
			{
				return this.mapParents;
			}
		}

		// Token: 0x17001786 RID: 6022
		// (get) Token: 0x06008F7C RID: 36732 RVA: 0x00335AAC File Offset: 0x00333CAC
		public List<Site> Sites
		{
			get
			{
				return this.sites;
			}
		}

		// Token: 0x17001787 RID: 6023
		// (get) Token: 0x06008F7D RID: 36733 RVA: 0x00335AB4 File Offset: 0x00333CB4
		public List<PeaceTalks> PeaceTalks
		{
			get
			{
				return this.peaceTalks;
			}
		}

		// Token: 0x17001788 RID: 6024
		// (get) Token: 0x06008F7E RID: 36734 RVA: 0x00335ABC File Offset: 0x00333CBC
		public int WorldObjectsCount
		{
			get
			{
				return this.worldObjects.Count;
			}
		}

		// Token: 0x17001789 RID: 6025
		// (get) Token: 0x06008F7F RID: 36735 RVA: 0x00335AC9 File Offset: 0x00333CC9
		public int CaravansCount
		{
			get
			{
				return this.caravans.Count;
			}
		}

		// Token: 0x1700178A RID: 6026
		// (get) Token: 0x06008F80 RID: 36736 RVA: 0x00335AD6 File Offset: 0x00333CD6
		public int RoutePlannerWaypointsCount
		{
			get
			{
				return this.routePlannerWaypoints.Count;
			}
		}

		// Token: 0x1700178B RID: 6027
		// (get) Token: 0x06008F81 RID: 36737 RVA: 0x00335AE4 File Offset: 0x00333CE4
		public int PlayerControlledCaravansCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.caravans.Count; i++)
				{
					if (this.caravans[i].IsPlayerControlled)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06008F82 RID: 36738 RVA: 0x00335B24 File Offset: 0x00333D24
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				WorldObjectsHolder.tmpUnsavedWorldObjects.Clear();
				for (int i = this.worldObjects.Count - 1; i >= 0; i--)
				{
					if (!this.worldObjects[i].def.saved)
					{
						WorldObjectsHolder.tmpUnsavedWorldObjects.Add(this.worldObjects[i]);
						this.worldObjects.RemoveAt(i);
					}
				}
			}
			Scribe_Collections.Look<WorldObject>(ref this.worldObjects, "worldObjects", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.worldObjects.AddRange(WorldObjectsHolder.tmpUnsavedWorldObjects);
				WorldObjectsHolder.tmpUnsavedWorldObjects.Clear();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.worldObjects.RemoveAll((WorldObject wo) => wo == null);
				this.Recache();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.worldObjects.RemoveAll((WorldObject wo) => wo == null || wo.def == null) != 0)
				{
					Log.Error("Some WorldObjects had null def after loading.");
				}
				for (int j = this.worldObjects.Count - 1; j >= 0; j--)
				{
					try
					{
						this.worldObjects[j].SpawnSetup();
					}
					catch (Exception arg)
					{
						Log.Error("Exception spawning WorldObject: " + arg);
						this.worldObjects.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x06008F83 RID: 36739 RVA: 0x00335CA4 File Offset: 0x00333EA4
		public void Add(WorldObject o)
		{
			if (this.worldObjects.Contains(o))
			{
				Log.Error("Tried to add world object " + o + " to world, but it's already here.");
				return;
			}
			if (o.Tile < 0)
			{
				Log.Error("Tried to add world object " + o + " but its tile is not set. Setting to 0.");
				o.Tile = 0;
			}
			this.worldObjects.Add(o);
			this.AddToCache(o);
			o.SpawnSetup();
			o.PostAdd();
		}

		// Token: 0x06008F84 RID: 36740 RVA: 0x00335D19 File Offset: 0x00333F19
		public void Remove(WorldObject o)
		{
			if (!this.worldObjects.Contains(o))
			{
				Log.Error("Tried to remove world object " + o + " from world, but it's not here.");
				return;
			}
			this.worldObjects.Remove(o);
			this.RemoveFromCache(o);
			o.PostRemove();
		}

		// Token: 0x06008F85 RID: 36741 RVA: 0x00335D5C File Offset: 0x00333F5C
		public void WorldObjectsHolderTick()
		{
			WorldObjectsHolder.tmpWorldObjects.Clear();
			WorldObjectsHolder.tmpWorldObjects.AddRange(this.worldObjects);
			for (int i = 0; i < WorldObjectsHolder.tmpWorldObjects.Count; i++)
			{
				WorldObjectsHolder.tmpWorldObjects[i].Tick();
			}
		}

		// Token: 0x06008F86 RID: 36742 RVA: 0x00335DA8 File Offset: 0x00333FA8
		private void AddToCache(WorldObject o)
		{
			this.worldObjectsHashSet.Add(o);
			if (o is Caravan)
			{
				this.caravans.Add((Caravan)o);
			}
			if (o is Settlement)
			{
				this.settlements.Add((Settlement)o);
			}
			if (o is TravelingTransportPods)
			{
				this.travelingTransportPods.Add((TravelingTransportPods)o);
			}
			if (o is Settlement)
			{
				this.settlementBases.Add((Settlement)o);
			}
			if (o is DestroyedSettlement)
			{
				this.destroyedSettlements.Add((DestroyedSettlement)o);
			}
			if (o is RoutePlannerWaypoint)
			{
				this.routePlannerWaypoints.Add((RoutePlannerWaypoint)o);
			}
			if (o is MapParent)
			{
				this.mapParents.Add((MapParent)o);
			}
			if (o is Site)
			{
				this.sites.Add((Site)o);
			}
			if (o is PeaceTalks)
			{
				this.peaceTalks.Add((PeaceTalks)o);
			}
		}

		// Token: 0x06008F87 RID: 36743 RVA: 0x00335EA4 File Offset: 0x003340A4
		private void RemoveFromCache(WorldObject o)
		{
			this.worldObjectsHashSet.Remove(o);
			if (o is Caravan)
			{
				this.caravans.Remove((Caravan)o);
			}
			if (o is Settlement)
			{
				this.settlements.Remove((Settlement)o);
			}
			if (o is TravelingTransportPods)
			{
				this.travelingTransportPods.Remove((TravelingTransportPods)o);
			}
			if (o is Settlement)
			{
				this.settlementBases.Remove((Settlement)o);
			}
			if (o is DestroyedSettlement)
			{
				this.destroyedSettlements.Remove((DestroyedSettlement)o);
			}
			if (o is RoutePlannerWaypoint)
			{
				this.routePlannerWaypoints.Remove((RoutePlannerWaypoint)o);
			}
			if (o is MapParent)
			{
				this.mapParents.Remove((MapParent)o);
			}
			if (o is Site)
			{
				this.sites.Remove((Site)o);
			}
			if (o is PeaceTalks)
			{
				this.peaceTalks.Remove((PeaceTalks)o);
			}
		}

		// Token: 0x06008F88 RID: 36744 RVA: 0x00335FA8 File Offset: 0x003341A8
		private void Recache()
		{
			this.worldObjectsHashSet.Clear();
			this.caravans.Clear();
			this.settlements.Clear();
			this.travelingTransportPods.Clear();
			this.settlementBases.Clear();
			this.destroyedSettlements.Clear();
			this.routePlannerWaypoints.Clear();
			this.mapParents.Clear();
			this.sites.Clear();
			this.peaceTalks.Clear();
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				this.AddToCache(this.worldObjects[i]);
			}
		}

		// Token: 0x06008F89 RID: 36745 RVA: 0x0033604B File Offset: 0x0033424B
		public bool Contains(WorldObject o)
		{
			return o != null && this.worldObjectsHashSet.Contains(o);
		}

		// Token: 0x06008F8A RID: 36746 RVA: 0x0033605E File Offset: 0x0033425E
		public IEnumerable<WorldObject> ObjectsAt(int tileID)
		{
			if (tileID < 0)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.worldObjects.Count; i = num + 1)
			{
				if (this.worldObjects[i].Tile == tileID)
				{
					yield return this.worldObjects[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06008F8B RID: 36747 RVA: 0x00336078 File Offset: 0x00334278
		public bool AnyWorldObjectAt(int tile)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008F8C RID: 36748 RVA: 0x003360B2 File Offset: 0x003342B2
		public bool AnyWorldObjectAt<T>(int tile) where T : WorldObject
		{
			return this.WorldObjectAt<T>(tile) != null;
		}

		// Token: 0x06008F8D RID: 36749 RVA: 0x003360C4 File Offset: 0x003342C4
		public T WorldObjectAt<T>(int tile) where T : WorldObject
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile && this.worldObjects[i] is T)
				{
					return this.worldObjects[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x06008F8E RID: 36750 RVA: 0x0033612E File Offset: 0x0033432E
		public bool AnyWorldObjectAt(int tile, WorldObjectDef def)
		{
			return this.WorldObjectAt(tile, def) != null;
		}

		// Token: 0x06008F8F RID: 36751 RVA: 0x0033613C File Offset: 0x0033433C
		public WorldObject WorldObjectAt(int tile, WorldObjectDef def)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile && this.worldObjects[i].def == def)
				{
					return this.worldObjects[i];
				}
			}
			return null;
		}

		// Token: 0x06008F90 RID: 36752 RVA: 0x00336195 File Offset: 0x00334395
		public bool AnySettlementAt(int tile)
		{
			return this.SettlementAt(tile) != null;
		}

		// Token: 0x06008F91 RID: 36753 RVA: 0x003361A4 File Offset: 0x003343A4
		public Settlement SettlementAt(int tile)
		{
			for (int i = 0; i < this.settlements.Count; i++)
			{
				if (this.settlements[i].Tile == tile)
				{
					return this.settlements[i];
				}
			}
			return null;
		}

		// Token: 0x06008F92 RID: 36754 RVA: 0x003361E9 File Offset: 0x003343E9
		public bool AnySettlementBaseAt(int tile)
		{
			return this.SettlementBaseAt(tile) != null;
		}

		// Token: 0x06008F93 RID: 36755 RVA: 0x003361F8 File Offset: 0x003343F8
		public Settlement SettlementBaseAt(int tile)
		{
			for (int i = 0; i < this.settlementBases.Count; i++)
			{
				if (this.settlementBases[i].Tile == tile)
				{
					return this.settlementBases[i];
				}
			}
			return null;
		}

		// Token: 0x06008F94 RID: 36756 RVA: 0x0033623D File Offset: 0x0033443D
		public bool AnySiteAt(int tile)
		{
			return this.SiteAt(tile) != null;
		}

		// Token: 0x06008F95 RID: 36757 RVA: 0x0033624C File Offset: 0x0033444C
		public Site SiteAt(int tile)
		{
			for (int i = 0; i < this.sites.Count; i++)
			{
				if (this.sites[i].Tile == tile)
				{
					return this.sites[i];
				}
			}
			return null;
		}

		// Token: 0x06008F96 RID: 36758 RVA: 0x00336291 File Offset: 0x00334491
		public bool AnyDestroyedSettlementAt(int tile)
		{
			return this.DestroyedSettlementAt(tile) != null;
		}

		// Token: 0x06008F97 RID: 36759 RVA: 0x003362A0 File Offset: 0x003344A0
		public DestroyedSettlement DestroyedSettlementAt(int tile)
		{
			for (int i = 0; i < this.destroyedSettlements.Count; i++)
			{
				if (this.destroyedSettlements[i].Tile == tile)
				{
					return this.destroyedSettlements[i];
				}
			}
			return null;
		}

		// Token: 0x06008F98 RID: 36760 RVA: 0x003362E5 File Offset: 0x003344E5
		public bool AnyMapParentAt(int tile)
		{
			return this.MapParentAt(tile) != null;
		}

		// Token: 0x06008F99 RID: 36761 RVA: 0x003362F4 File Offset: 0x003344F4
		public MapParent MapParentAt(int tile)
		{
			for (int i = 0; i < this.mapParents.Count; i++)
			{
				if (this.mapParents[i].Tile == tile)
				{
					return this.mapParents[i];
				}
			}
			return null;
		}

		// Token: 0x06008F9A RID: 36762 RVA: 0x00336339 File Offset: 0x00334539
		public bool AnyWorldObjectOfDefAt(WorldObjectDef def, int tile)
		{
			return this.WorldObjectOfDefAt(def, tile) != null;
		}

		// Token: 0x06008F9B RID: 36763 RVA: 0x00336348 File Offset: 0x00334548
		public WorldObject WorldObjectOfDefAt(WorldObjectDef def, int tile)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].def == def && this.worldObjects[i].Tile == tile)
				{
					return this.worldObjects[i];
				}
			}
			return null;
		}

		// Token: 0x06008F9C RID: 36764 RVA: 0x003363A4 File Offset: 0x003345A4
		public Caravan PlayerControlledCaravanAt(int tile)
		{
			for (int i = 0; i < this.caravans.Count; i++)
			{
				if (this.caravans[i].Tile == tile && this.caravans[i].IsPlayerControlled)
				{
					return this.caravans[i];
				}
			}
			return null;
		}

		// Token: 0x06008F9D RID: 36765 RVA: 0x003363FC File Offset: 0x003345FC
		public bool AnySettlementBaseAtOrAdjacent(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < this.settlementBases.Count; i++)
			{
				if (worldGrid.IsNeighborOrSame(this.settlementBases[i].Tile, tile))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008F9E RID: 36766 RVA: 0x00336444 File Offset: 0x00334644
		public RoutePlannerWaypoint RoutePlannerWaypointAt(int tile)
		{
			for (int i = 0; i < this.routePlannerWaypoints.Count; i++)
			{
				if (this.routePlannerWaypoints[i].Tile == tile)
				{
					return this.routePlannerWaypoints[i];
				}
			}
			return null;
		}

		// Token: 0x06008F9F RID: 36767 RVA: 0x0033648C File Offset: 0x0033468C
		public void GetPlayerControlledCaravansAt(int tile, List<Caravan> outCaravans)
		{
			outCaravans.Clear();
			for (int i = 0; i < this.caravans.Count; i++)
			{
				Caravan caravan = this.caravans[i];
				if (caravan.Tile == tile && caravan.IsPlayerControlled)
				{
					outCaravans.Add(caravan);
				}
			}
		}

		// Token: 0x04005A24 RID: 23076
		private List<WorldObject> worldObjects = new List<WorldObject>();

		// Token: 0x04005A25 RID: 23077
		private HashSet<WorldObject> worldObjectsHashSet = new HashSet<WorldObject>();

		// Token: 0x04005A26 RID: 23078
		private List<Caravan> caravans = new List<Caravan>();

		// Token: 0x04005A27 RID: 23079
		private List<Settlement> settlements = new List<Settlement>();

		// Token: 0x04005A28 RID: 23080
		private List<TravelingTransportPods> travelingTransportPods = new List<TravelingTransportPods>();

		// Token: 0x04005A29 RID: 23081
		private List<Settlement> settlementBases = new List<Settlement>();

		// Token: 0x04005A2A RID: 23082
		private List<DestroyedSettlement> destroyedSettlements = new List<DestroyedSettlement>();

		// Token: 0x04005A2B RID: 23083
		private List<RoutePlannerWaypoint> routePlannerWaypoints = new List<RoutePlannerWaypoint>();

		// Token: 0x04005A2C RID: 23084
		private List<MapParent> mapParents = new List<MapParent>();

		// Token: 0x04005A2D RID: 23085
		private List<Site> sites = new List<Site>();

		// Token: 0x04005A2E RID: 23086
		private List<PeaceTalks> peaceTalks = new List<PeaceTalks>();

		// Token: 0x04005A2F RID: 23087
		private static List<WorldObject> tmpUnsavedWorldObjects = new List<WorldObject>();

		// Token: 0x04005A30 RID: 23088
		private static List<WorldObject> tmpWorldObjects = new List<WorldObject>();
	}
}
