using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010ED RID: 4333
	public class CompCauseGameCondition : ThingComp
	{
		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x060067AF RID: 26543 RVA: 0x00231A58 File Offset: 0x0022FC58
		public CompProperties_CausesGameCondition Props
		{
			get
			{
				return (CompProperties_CausesGameCondition)this.props;
			}
		}

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x060067B0 RID: 26544 RVA: 0x00231A65 File Offset: 0x0022FC65
		public GameConditionDef ConditionDef
		{
			get
			{
				return this.Props.conditionDef;
			}
		}

		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x060067B1 RID: 26545 RVA: 0x00231A72 File Offset: 0x0022FC72
		public IEnumerable<GameCondition> CausedConditions
		{
			get
			{
				return this.causedConditions.Values;
			}
		}

		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x060067B2 RID: 26546 RVA: 0x00231A7F File Offset: 0x0022FC7F
		public bool Active
		{
			get
			{
				return this.initiatableComp == null || this.initiatableComp.Initiated;
			}
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x060067B3 RID: 26547 RVA: 0x00231A96 File Offset: 0x0022FC96
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

		// Token: 0x060067B4 RID: 26548 RVA: 0x00231AC6 File Offset: 0x0022FCC6
		public void LinkWithSite(Site site)
		{
			this.siteLink = site;
		}

		// Token: 0x060067B5 RID: 26549 RVA: 0x00231ACF File Offset: 0x0022FCCF
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.CacheComps();
		}

		// Token: 0x060067B6 RID: 26550 RVA: 0x00231ADD File Offset: 0x0022FCDD
		private void CacheComps()
		{
			this.initiatableComp = this.parent.GetComp<CompInitiatable>();
		}

		// Token: 0x060067B7 RID: 26551 RVA: 0x00231AF0 File Offset: 0x0022FCF0
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

		// Token: 0x060067B8 RID: 26552 RVA: 0x00231BE8 File Offset: 0x0022FDE8
		public bool InAoE(int tile)
		{
			return this.MyTile != -1 && this.Active && Find.WorldGrid.TraversalDistanceBetween(this.MyTile, tile, true, this.Props.worldRange + 1) <= this.Props.worldRange;
		}

		// Token: 0x060067B9 RID: 26553 RVA: 0x00231C38 File Offset: 0x0022FE38
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

		// Token: 0x060067BA RID: 26554 RVA: 0x00231C94 File Offset: 0x0022FE94
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

		// Token: 0x060067BB RID: 26555 RVA: 0x00231DCC File Offset: 0x0022FFCC
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

		// Token: 0x060067BC RID: 26556 RVA: 0x00231DFC File Offset: 0x0022FFFC
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

		// Token: 0x060067BD RID: 26557 RVA: 0x00231E50 File Offset: 0x00230050
		protected virtual void SetupCondition(GameCondition condition, Map map)
		{
			condition.suppressEndMessage = true;
		}

		// Token: 0x060067BE RID: 26558 RVA: 0x00231E5C File Offset: 0x0023005C
		protected void ReSetupAllConditions()
		{
			foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
			{
				this.SetupCondition(keyValuePair.Value, keyValuePair.Key);
			}
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x00231EBC File Offset: 0x002300BC
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			Messages.Message("MessageConditionCauserDespawned".Translate(this.parent.def.LabelCap), new TargetInfo(this.parent.Position, previousMap, false), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x00231F10 File Offset: 0x00230110
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

		// Token: 0x060067C1 RID: 26561 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void RandomizeSettings(Site site)
		{
		}

		// Token: 0x04003A6A RID: 14954
		protected CompInitiatable initiatableComp;

		// Token: 0x04003A6B RID: 14955
		protected Site siteLink;

		// Token: 0x04003A6C RID: 14956
		private Dictionary<Map, GameCondition> causedConditions = new Dictionary<Map, GameCondition>();

		// Token: 0x04003A6D RID: 14957
		private static List<Map> tmpDeadConditionMaps = new List<Map>();
	}
}
