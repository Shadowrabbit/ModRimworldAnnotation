using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001778 RID: 6008
	public class CompCauseGameCondition : ThingComp
	{
		// Token: 0x1700147C RID: 5244
		// (get) Token: 0x06008467 RID: 33895 RVA: 0x00058AE5 File Offset: 0x00056CE5
		public CompProperties_CausesGameCondition Props
		{
			get
			{
				return (CompProperties_CausesGameCondition)this.props;
			}
		}

		// Token: 0x1700147D RID: 5245
		// (get) Token: 0x06008468 RID: 33896 RVA: 0x00058AF2 File Offset: 0x00056CF2
		public GameConditionDef ConditionDef
		{
			get
			{
				return this.Props.conditionDef;
			}
		}

		// Token: 0x1700147E RID: 5246
		// (get) Token: 0x06008469 RID: 33897 RVA: 0x00058AFF File Offset: 0x00056CFF
		public IEnumerable<GameCondition> CausedConditions
		{
			get
			{
				return this.causedConditions.Values;
			}
		}

		// Token: 0x1700147F RID: 5247
		// (get) Token: 0x0600846A RID: 33898 RVA: 0x00058B0C File Offset: 0x00056D0C
		public bool Active
		{
			get
			{
				return this.initiatableComp == null || this.initiatableComp.Initiated;
			}
		}

		// Token: 0x17001480 RID: 5248
		// (get) Token: 0x0600846B RID: 33899 RVA: 0x00058B23 File Offset: 0x00056D23
		public int MyTile
		{
			get
			{
				if (this.siteLink != null)
				{
					return this.siteLink.Tile;
				}
				if (this.parent.SpawnedOrAnyParentSpawned)
				{
					return this.parent.Tile;
				}
				return -1;
			}
		}

		// Token: 0x0600846C RID: 33900 RVA: 0x00058B53 File Offset: 0x00056D53
		public void LinkWithSite(Site site)
		{
			this.siteLink = site;
		}

		// Token: 0x0600846D RID: 33901 RVA: 0x00058B5C File Offset: 0x00056D5C
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.CacheComps();
		}

		// Token: 0x0600846E RID: 33902 RVA: 0x00058B6A File Offset: 0x00056D6A
		private void CacheComps()
		{
			this.initiatableComp = this.parent.GetComp<CompInitiatable>();
		}

		// Token: 0x0600846F RID: 33903 RVA: 0x00273B34 File Offset: 0x00271D34
		public override void PostExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.causedConditions.RemoveAll((KeyValuePair<Map, GameCondition> x) => !Find.Maps.Contains(x.Key));
			}
			Scribe_References.Look<Site>(ref this.siteLink, "siteLink", false);
			Scribe_Collections.Look<Map, GameCondition>(ref this.causedConditions, "causedConditions", LookMode.Reference, LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.causedConditions.RemoveAll((KeyValuePair<Map, GameCondition> x) => x.Value == null);
				foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
				{
					keyValuePair.Value.conditionCauser = this.parent;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CacheComps();
			}
		}

		// Token: 0x06008470 RID: 33904 RVA: 0x00273C2C File Offset: 0x00271E2C
		public bool InAoE(int tile)
		{
			return this.MyTile != -1 && this.Active && Find.WorldGrid.TraversalDistanceBetween(this.MyTile, tile, true, this.Props.worldRange + 1) <= this.Props.worldRange;
		}

		// Token: 0x06008471 RID: 33905 RVA: 0x00273C7C File Offset: 0x00271E7C
		protected GameCondition GetConditionInstance(Map map)
		{
			GameCondition activeCondition;
			if (!this.causedConditions.TryGetValue(map, out activeCondition) && this.Props.preventConditionStacking)
			{
				activeCondition = map.GameConditionManager.GetActiveCondition(this.Props.conditionDef);
				if (activeCondition != null)
				{
					this.causedConditions.Add(map, activeCondition);
					this.SetupCondition(activeCondition, map);
				}
			}
			return activeCondition;
		}

		// Token: 0x06008472 RID: 33906 RVA: 0x00273CD8 File Offset: 0x00271ED8
		public override void CompTick()
		{
			if (this.Active)
			{
				foreach (Map map in Find.Maps)
				{
					if (this.InAoE(map.Tile))
					{
						this.EnforceConditionOn(map);
					}
				}
			}
			CompCauseGameCondition.tmpDeadConditionMaps.Clear();
			foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
			{
				if (keyValuePair.Value.Expired || !keyValuePair.Key.GameConditionManager.ConditionIsActive(keyValuePair.Value.def))
				{
					CompCauseGameCondition.tmpDeadConditionMaps.Add(keyValuePair.Key);
				}
			}
			foreach (Map key in CompCauseGameCondition.tmpDeadConditionMaps)
			{
				this.causedConditions.Remove(key);
			}
		}

		// Token: 0x06008473 RID: 33907 RVA: 0x00273E10 File Offset: 0x00272010
		private GameCondition EnforceConditionOn(Map map)
		{
			GameCondition gameCondition = this.GetConditionInstance(map);
			if (gameCondition == null)
			{
				gameCondition = this.CreateConditionOn(map);
			}
			else
			{
				gameCondition.TicksLeft = gameCondition.TransitionTicks;
			}
			return gameCondition;
		}

		// Token: 0x06008474 RID: 33908 RVA: 0x00273E40 File Offset: 0x00272040
		protected virtual GameCondition CreateConditionOn(Map map)
		{
			GameCondition gameCondition = GameConditionMaker.MakeCondition(this.ConditionDef, -1);
			gameCondition.Duration = gameCondition.TransitionTicks;
			gameCondition.conditionCauser = this.parent;
			map.gameConditionManager.RegisterCondition(gameCondition);
			this.causedConditions.Add(map, gameCondition);
			this.SetupCondition(gameCondition, map);
			return gameCondition;
		}

		// Token: 0x06008475 RID: 33909 RVA: 0x00058B7D File Offset: 0x00056D7D
		protected virtual void SetupCondition(GameCondition condition, Map map)
		{
			condition.suppressEndMessage = true;
		}

		// Token: 0x06008476 RID: 33910 RVA: 0x00273E94 File Offset: 0x00272094
		protected void ReSetupAllConditions()
		{
			foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
			{
				this.SetupCondition(keyValuePair.Value, keyValuePair.Key);
			}
		}

		// Token: 0x06008477 RID: 33911 RVA: 0x00273EF4 File Offset: 0x002720F4
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			Messages.Message("MessageConditionCauserDespawned".Translate(this.parent.def.LabelCap), new TargetInfo(this.parent.Position, previousMap, false), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06008478 RID: 33912 RVA: 0x00273F48 File Offset: 0x00272148
		public override string CompInspectStringExtra()
		{
			if (!Prefs.DevMode)
			{
				return base.CompInspectStringExtra();
			}
			GameCondition gameCondition = this.parent.Map.GameConditionManager.ActiveConditions.Find((GameCondition c) => c.def == this.Props.conditionDef);
			if (gameCondition == null)
			{
				return base.CompInspectStringExtra();
			}
			return string.Concat(new object[]
			{
				"[DEV] Current map condition\n[DEV] Ticks Passed: ",
				gameCondition.TicksPassed,
				"\n[DEV] Ticks Left: ",
				gameCondition.TicksLeft
			});
		}

		// Token: 0x06008479 RID: 33913 RVA: 0x00006A05 File Offset: 0x00004C05
		[Obsolete]
		public virtual void RandomizeSettings_NewTemp(float points)
		{
		}

		// Token: 0x0600847A RID: 33914 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void RandomizeSettings_NewTemp_NewTemp(Site site)
		{
		}

		// Token: 0x0600847B RID: 33915 RVA: 0x00006A05 File Offset: 0x00004C05
		[Obsolete]
		public virtual void RandomizeSettings()
		{
		}

		// Token: 0x040055D1 RID: 21969
		protected CompInitiatable initiatableComp;

		// Token: 0x040055D2 RID: 21970
		protected Site siteLink;

		// Token: 0x040055D3 RID: 21971
		private Dictionary<Map, GameCondition> causedConditions = new Dictionary<Map, GameCondition>();

		// Token: 0x040055D4 RID: 21972
		private static List<Map> tmpDeadConditionMaps = new List<Map>();
	}
}
