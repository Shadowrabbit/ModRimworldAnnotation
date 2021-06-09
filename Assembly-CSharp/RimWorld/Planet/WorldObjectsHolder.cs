using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021B1 RID: 8625
	public class WorldObjectsHolder : IExposable
	{
		// Token: 0x17001B58 RID: 7000
		// (get) Token: 0x0600B85D RID: 47197 RVA: 0x000778B7 File Offset: 0x00075AB7
		public List<WorldObject> AllWorldObjects
		{
			get
			{
				return this.worldObjects;
			}
		}

		// Token: 0x17001B59 RID: 7001
		// (get) Token: 0x0600B85E RID: 47198 RVA: 0x000778BF File Offset: 0x00075ABF
		public List<Caravan> Caravans
		{
			get
			{
				return this.caravans;
			}
		}

		// Token: 0x17001B5A RID: 7002
		// (get) Token: 0x0600B85F RID: 47199 RVA: 0x000778C7 File Offset: 0x00075AC7
		public List<Settlement> Settlements
		{
			get
			{
				return this.settlements;
			}
		}

		// Token: 0x17001B5B RID: 7003
		// (get) Token: 0x0600B860 RID: 47200 RVA: 0x000778CF File Offset: 0x00075ACF
		public List<TravelingTransportPods> TravelingTransportPods
		{
			get
			{
				return this.travelingTransportPods;
			}
		}

		// Token: 0x17001B5C RID: 7004
		// (get) Token: 0x0600B861 RID: 47201 RVA: 0x000778D7 File Offset: 0x00075AD7
		public List<Settlement> SettlementBases
		{
			get
			{
				return this.settlementBases;
			}
		}

		// Token: 0x17001B5D RID: 7005
		// (get) Token: 0x0600B862 RID: 47202 RVA: 0x000778DF File Offset: 0x00075ADF
		public List<DestroyedSettlement> DestroyedSettlements
		{
			get
			{
				return this.destroyedSettlements;
			}
		}

		// Token: 0x17001B5E RID: 7006
		// (get) Token: 0x0600B863 RID: 47203 RVA: 0x000778E7 File Offset: 0x00075AE7
		public List<RoutePlannerWaypoint> RoutePlannerWaypoints
		{
			get
			{
				return this.routePlannerWaypoints;
			}
		}

		// Token: 0x17001B5F RID: 7007
		// (get) Token: 0x0600B864 RID: 47204 RVA: 0x000778EF File Offset: 0x00075AEF
		public List<MapParent> MapParents
		{
			get
			{
				return this.mapParents;
			}
		}

		// Token: 0x17001B60 RID: 7008
		// (get) Token: 0x0600B865 RID: 47205 RVA: 0x000778F7 File Offset: 0x00075AF7
		public List<Site> Sites
		{
			get
			{
				return this.sites;
			}
		}

		// Token: 0x17001B61 RID: 7009
		// (get) Token: 0x0600B866 RID: 47206 RVA: 0x000778FF File Offset: 0x00075AFF
		public List<PeaceTalks> PeaceTalks
		{
			get
			{
				return this.peaceTalks;
			}
		}

		// Token: 0x17001B62 RID: 7010
		// (get) Token: 0x0600B867 RID: 47207 RVA: 0x00077907 File Offset: 0x00075B07
		public int WorldObjectsCount
		{
			get
			{
				return this.worldObjects.Count;
			}
		}

		// Token: 0x17001B63 RID: 7011
		// (get) Token: 0x0600B868 RID: 47208 RVA: 0x00077914 File Offset: 0x00075B14
		public int CaravansCount
		{
			get
			{
				return this.caravans.Count;
			}
		}

		// Token: 0x17001B64 RID: 7012
		// (get) Token: 0x0600B869 RID: 47209 RVA: 0x00077921 File Offset: 0x00075B21
		public int RoutePlannerWaypointsCount
		{
			get
			{
				return this.routePlannerWaypoints.Count;
			}
		}

		// Token: 0x17001B65 RID: 7013
		// (get) Token: 0x0600B86A RID: 47210 RVA: 0x00350268 File Offset: 0x0034E468
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

		// Token: 0x0600B86B RID: 47211 RVA: 0x003502A8 File Offset: 0x0034E4A8
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
					Log.Error("Some WorldObjects had null def after loading.", false);
				}
				for (int j = this.worldObjects.Count - 1; j >= 0; j--)
				{
					try
					{
						this.worldObjects[j].SpawnSetup();
					}
					catch (Exception arg)
					{
						Log.Error("Exception spawning WorldObject: " + arg, false);
						this.worldObjects.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x0600B86C RID: 47212 RVA: 0x00350428 File Offset: 0x0034E628
		public void Add(WorldObject o)
		{
			if (this.worldObjects.Contains(o))
			{
				Log.Error("Tried to add world object " + o + " to world, but it's already here.", false);
				return;
			}
			if (o.Tile < 0)
			{
				Log.Error("Tried to add world object " + o + " but its tile is not set. Setting to 0.", false);
				o.Tile = 0;
			}
			this.worldObjects.Add(o);
			this.AddToCache(o);
			o.SpawnSetup();
			o.PostAdd();
		}

		// Token: 0x0600B86D RID: 47213 RVA: 0x003504A0 File Offset: 0x0034E6A0
		public void Remove(WorldObject o)
		{
			if (!this.worldObjects.Contains(o))
			{
				Log.Error("Tried to remove world object " + o + " from world, but it's not here.", false);
				return;
			}
			this.worldObjects.Remove(o);
			this.RemoveFromCache(o);
			o.PostRemove();
		}

		// Token: 0x0600B86E RID: 47214 RVA: 0x003504EC File Offset: 0x0034E6EC
		public void WorldObjectsHolderTick()
		{
			WorldObjectsHolder.tmpWorldObjects.Clear();
			WorldObjectsHolder.tmpWorldObjects.AddRange(this.worldObjects);
			for (int i = 0; i < WorldObjectsHolder.tmpWorldObjects.Count; i++)
			{
				WorldObjectsHolder.tmpWorldObjects[i].Tick();
			}
		}

		// Token: 0x0600B86F RID: 47215 RVA: 0x00350538 File Offset: 0x0034E738
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

		// Token: 0x0600B870 RID: 47216 RVA: 0x00350634 File Offset: 0x0034E834
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

		// Token: 0x0600B871 RID: 47217 RVA: 0x00350738 File Offset: 0x0034E938
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

		// Token: 0x0600B872 RID: 47218 RVA: 0x0007792E File Offset: 0x00075B2E
		public bool Contains(WorldObject o)
		{
			return o != null && this.worldObjectsHashSet.Contains(o);
		}

		// Token: 0x0600B873 RID: 47219 RVA: 0x00077941 File Offset: 0x00075B41
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

		// Token: 0x0600B874 RID: 47220 RVA: 0x003507DC File Offset: 0x0034E9DC
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

		// Token: 0x0600B875 RID: 47221 RVA: 0x00077958 File Offset: 0x00075B58
		public bool AnyWorldObjectAt<T>(int tile) where T : WorldObject
		{
			return this.WorldObjectAt<T>(tile) != null;
		}

		// Token: 0x0600B876 RID: 47222 RVA: 0x00350818 File Offset: 0x0034EA18
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

		// Token: 0x0600B877 RID: 47223 RVA: 0x00077969 File Offset: 0x00075B69
		public bool AnyWorldObjectAt(int tile, WorldObjectDef def)
		{
			return this.WorldObjectAt(tile, def) != null;
		}

		// Token: 0x0600B878 RID: 47224 RVA: 0x00350884 File Offset: 0x0034EA84
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

		// Token: 0x0600B879 RID: 47225 RVA: 0x00077976 File Offset: 0x00075B76
		public bool AnySettlementAt(int tile)
		{
			return this.SettlementAt(tile) != null;
		}

		// Token: 0x0600B87A RID: 47226 RVA: 0x003508E0 File Offset: 0x0034EAE0
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

		// Token: 0x0600B87B RID: 47227 RVA: 0x00077982 File Offset: 0x00075B82
		public bool AnySettlementBaseAt(int tile)
		{
			return this.SettlementBaseAt(tile) != null;
		}

		// Token: 0x0600B87C RID: 47228 RVA: 0x00350928 File Offset: 0x0034EB28
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

		// Token: 0x0600B87D RID: 47229 RVA: 0x0007798E File Offset: 0x00075B8E
		public bool AnySiteAt(int tile)
		{
			return this.SiteAt(tile) != null;
		}

		// Token: 0x0600B87E RID: 47230 RVA: 0x00350970 File Offset: 0x0034EB70
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

		// Token: 0x0600B87F RID: 47231 RVA: 0x0007799A File Offset: 0x00075B9A
		public bool AnyDestroyedSettlementAt(int tile)
		{
			return this.DestroyedSettlementAt(tile) != null;
		}

		// Token: 0x0600B880 RID: 47232 RVA: 0x003509B8 File Offset: 0x0034EBB8
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

		// Token: 0x0600B881 RID: 47233 RVA: 0x000779A6 File Offset: 0x00075BA6
		public bool AnyMapParentAt(int tile)
		{
			return this.MapParentAt(tile) != null;
		}

		// Token: 0x0600B882 RID: 47234 RVA: 0x00350A00 File Offset: 0x0034EC00
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

		// Token: 0x0600B883 RID: 47235 RVA: 0x000779B2 File Offset: 0x00075BB2
		public bool AnyWorldObjectOfDefAt(WorldObjectDef def, int tile)
		{
			return this.WorldObjectOfDefAt(def, tile) != null;
		}

		// Token: 0x0600B884 RID: 47236 RVA: 0x00350A48 File Offset: 0x0034EC48
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

		// Token: 0x0600B885 RID: 47237 RVA: 0x00350AA4 File Offset: 0x0034ECA4
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

		// Token: 0x0600B886 RID: 47238 RVA: 0x00350AFC File Offset: 0x0034ECFC
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

		// Token: 0x0600B887 RID: 47239 RVA: 0x00350B44 File Offset: 0x0034ED44
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

		// Token: 0x0600B888 RID: 47240 RVA: 0x00350B8C File Offset: 0x0034ED8C
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

		// Token: 0x04007DDB RID: 32219
		private List<WorldObject> worldObjects = new List<WorldObject>();

		// Token: 0x04007DDC RID: 32220
		private HashSet<WorldObject> worldObjectsHashSet = new HashSet<WorldObject>();

		// Token: 0x04007DDD RID: 32221
		private List<Caravan> caravans = new List<Caravan>();

		// Token: 0x04007DDE RID: 32222
		private List<Settlement> settlements = new List<Settlement>();

		// Token: 0x04007DDF RID: 32223
		private List<TravelingTransportPods> travelingTransportPods = new List<TravelingTransportPods>();

		// Token: 0x04007DE0 RID: 32224
		private List<Settlement> settlementBases = new List<Settlement>();

		// Token: 0x04007DE1 RID: 32225
		private List<DestroyedSettlement> destroyedSettlements = new List<DestroyedSettlement>();

		// Token: 0x04007DE2 RID: 32226
		private List<RoutePlannerWaypoint> routePlannerWaypoints = new List<RoutePlannerWaypoint>();

		// Token: 0x04007DE3 RID: 32227
		private List<MapParent> mapParents = new List<MapParent>();

		// Token: 0x04007DE4 RID: 32228
		private List<Site> sites = new List<Site>();

		// Token: 0x04007DE5 RID: 32229
		private List<PeaceTalks> peaceTalks = new List<PeaceTalks>();

		// Token: 0x04007DE6 RID: 32230
		private static List<WorldObject> tmpUnsavedWorldObjects = new List<WorldObject>();

		// Token: 0x04007DE7 RID: 32231
		private static List<WorldObject> tmpWorldObjects = new List<WorldObject>();
	}
}
