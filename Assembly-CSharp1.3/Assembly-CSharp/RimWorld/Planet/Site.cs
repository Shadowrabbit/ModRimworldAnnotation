using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017D4 RID: 6100
	[StaticConstructorOnStartup]
	public class Site : MapParent
	{
		// Token: 0x1700172B RID: 5931
		// (get) Token: 0x06008DF2 RID: 36338 RVA: 0x0032FF08 File Offset: 0x0032E108
		public override string Label
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel;
				}
				if (this.MainSitePartDef == SitePartDefOf.PreciousLump && this.MainSitePart.parms.preciousLumpResources != null)
				{
					return "PreciousLumpLabel".Translate(this.MainSitePart.parms.preciousLumpResources.label);
				}
				return this.MainSitePartDef.label;
			}
		}

		// Token: 0x1700172C RID: 5932
		// (get) Token: 0x06008DF3 RID: 36339 RVA: 0x0032FF7D File Offset: 0x0032E17D
		public override Texture2D ExpandingIcon
		{
			get
			{
				return this.MainSitePartDef.ExpandingIconTexture;
			}
		}

		// Token: 0x1700172D RID: 5933
		// (get) Token: 0x06008DF4 RID: 36340 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool HandlesConditionCausers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700172E RID: 5934
		// (get) Token: 0x06008DF5 RID: 36341 RVA: 0x0032FF8C File Offset: 0x0032E18C
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (this.MainSitePartDef.applyFactionColorToSiteTexture && base.Faction != null)
					{
						color = base.Faction.Color;
					}
					else
					{
						color = Color.white;
					}
					this.cachedMat = MaterialPool.MatFrom(this.MainSitePartDef.siteTexture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x1700172F RID: 5935
		// (get) Token: 0x06008DF6 RID: 36342 RVA: 0x0032FFF7 File Offset: 0x0032E1F7
		public override bool AppendFactionToInspectString
		{
			get
			{
				return this.MainSitePartDef.applyFactionColorToSiteTexture || this.MainSitePartDef.showFactionInInspectString;
			}
		}

		// Token: 0x17001730 RID: 5936
		// (get) Token: 0x06008DF7 RID: 36343 RVA: 0x00330014 File Offset: 0x0032E214
		private SitePart MainSitePart
		{
			get
			{
				if (!this.parts.Any<SitePart>())
				{
					Log.ErrorOnce("Site without any SitePart at " + base.Tile, this.ID ^ 93890909);
					return null;
				}
				if (this.parts[0].hidden)
				{
					Log.ErrorOnce("Site with first SitePart hidden at " + base.Tile, this.ID ^ 48471239);
					return this.parts[0];
				}
				return this.parts[0];
			}
		}

		// Token: 0x17001731 RID: 5937
		// (get) Token: 0x06008DF8 RID: 36344 RVA: 0x003300A8 File Offset: 0x0032E2A8
		public SitePartDef MainSitePartDef
		{
			get
			{
				return this.MainSitePart.def;
			}
		}

		// Token: 0x17001732 RID: 5938
		// (get) Token: 0x06008DF9 RID: 36345 RVA: 0x003300B5 File Offset: 0x0032E2B5
		public override IEnumerable<GenStepWithParams> ExtraGenStepDefs
		{
			get
			{
				foreach (GenStepWithParams genStepWithParams in base.ExtraGenStepDefs)
				{
					yield return genStepWithParams;
				}
				IEnumerator<GenStepWithParams> enumerator = null;
				int num;
				for (int i = 0; i < this.parts.Count; i = num + 1)
				{
					GenStepParams partGenStepParams = default(GenStepParams);
					partGenStepParams.sitePart = this.parts[i];
					List<GenStepDef> partGenStepDefs = this.parts[i].def.ExtraGenSteps;
					for (int j = 0; j < partGenStepDefs.Count; j = num + 1)
					{
						yield return new GenStepWithParams(partGenStepDefs[j], partGenStepParams);
						num = j;
					}
					partGenStepParams = default(GenStepParams);
					partGenStepDefs = null;
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17001733 RID: 5939
		// (get) Token: 0x06008DFA RID: 36346 RVA: 0x003300C8 File Offset: 0x0032E2C8
		public string ApproachOrderString
		{
			get
			{
				return this.MainSitePartDef.approachOrderString.NullOrEmpty() ? "ApproachSite".Translate(this.Label) : this.MainSitePartDef.approachOrderString.Formatted(this.Label);
			}
		}

		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x06008DFB RID: 36347 RVA: 0x00330120 File Offset: 0x0032E320
		public string ApproachingReportString
		{
			get
			{
				return this.MainSitePartDef.approachingReportString.NullOrEmpty() ? "ApproachingSite".Translate(this.Label) : this.MainSitePartDef.approachingReportString.Formatted(this.Label);
			}
		}

		// Token: 0x17001735 RID: 5941
		// (get) Token: 0x06008DFC RID: 36348 RVA: 0x00330178 File Offset: 0x0032E378
		public float ActualThreatPoints
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.parts.Count; i++)
				{
					num += this.parts[i].parms.threatPoints;
				}
				return num;
			}
		}

		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x06008DFD RID: 36349 RVA: 0x003301BC File Offset: 0x0032E3BC
		public bool IncreasesPopulation
		{
			get
			{
				if (base.HasMap)
				{
					return false;
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].def.Worker.IncreasesPopulation(this.parts[i].parms))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x06008DFE RID: 36350 RVA: 0x0033021C File Offset: 0x0032E41C
		public bool BadEvenIfNoMap
		{
			get
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].def.badEvenIfNoMap)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x06008DFF RID: 36351 RVA: 0x0033025A File Offset: 0x0032E45A
		public bool HasWorldObjectTimeout
		{
			get
			{
				return this.WorldObjectTimeoutTicksLeft != -1;
			}
		}

		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x06008E00 RID: 36352 RVA: 0x00330268 File Offset: 0x0032E468
		public int WorldObjectTimeoutTicksLeft
		{
			get
			{
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					Quest quest = questsListForReading[i];
					if (quest.State == QuestState.Ongoing)
					{
						for (int j = 0; j < quest.PartsListForReading.Count; j++)
						{
							QuestPart_WorldObjectTimeout questPart_WorldObjectTimeout = quest.PartsListForReading[j] as QuestPart_WorldObjectTimeout;
							if (questPart_WorldObjectTimeout != null && questPart_WorldObjectTimeout.State == QuestPartState.Enabled && questPart_WorldObjectTimeout.worldObject == this)
							{
								return questPart_WorldObjectTimeout.TicksLeft;
							}
						}
					}
				}
				return -1;
			}
		}

		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x06008E01 RID: 36353 RVA: 0x003302EC File Offset: 0x0032E4EC
		public IntVec3 PreferredMapSize
		{
			get
			{
				IntVec3 defaultMapSize = Site.DefaultMapSize;
				for (int i = 0; i < this.parts.Count; i++)
				{
					SitePart sitePart = this.parts[i];
					if (sitePart.def.minMapSize != null)
					{
						IntVec3 value = sitePart.def.minMapSize.Value;
						defaultMapSize.x = Mathf.Max(value.x, defaultMapSize.x);
						defaultMapSize.y = Mathf.Max(value.y, defaultMapSize.y);
						defaultMapSize.z = Mathf.Max(value.z, defaultMapSize.z);
					}
				}
				return defaultMapSize;
			}
		}

		// Token: 0x06008E02 RID: 36354 RVA: 0x00330394 File Offset: 0x0032E594
		public override void Destroy()
		{
			base.Destroy();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostDestroy(this.parts[i]);
			}
			for (int j = 0; j < this.parts.Count; j++)
			{
				this.parts[j].PostDestroy();
			}
		}

		// Token: 0x06008E03 RID: 36355 RVA: 0x0033040C File Offset: 0x0032E60C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", null, false);
			Scribe_Deep.Look<SiteCoreBackCompat>(ref this.coreBackCompat, "core", Array.Empty<object>());
			Scribe_Collections.Look<SitePart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.anyEnemiesInitially, "anyEnemiesInitially", false, false);
			Scribe_Values.Look<bool>(ref this.caravanAssaultSuccessfulTaleRecorded, "caravanAssaultSuccessfulTaleRecorded", false, false);
			Scribe_Values.Look<bool>(ref this.allEnemiesDefeatedSignalSent, "allEnemiesDefeatedSignalSent", false, false);
			Scribe_Values.Look<bool>(ref this.factionMustRemainHostile, "factionMustRemainHostile", false, false);
			Scribe_Values.Look<float>(ref this.desiredThreatPoints, "desiredThreatPoints", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.coreBackCompat != null && this.coreBackCompat.def != null)
				{
					this.parts.Insert(0, new SitePart(this, this.coreBackCompat.def, this.coreBackCompat.parms));
					this.coreBackCompat = null;
				}
				if (this.parts.RemoveAll((SitePart x) => x == null || x.def == null) != 0)
				{
					Log.Error("Some site parts were null after loading.");
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					this.parts[i].site = this;
				}
				BackCompatibility.PostExposeData(this);
			}
		}

		// Token: 0x06008E04 RID: 36356 RVA: 0x0033056D File Offset: 0x0032E76D
		public void AddPart(SitePart part)
		{
			this.parts.Add(part);
			part.def.Worker.Init(this, part);
		}

		// Token: 0x06008E05 RID: 36357 RVA: 0x00330590 File Offset: 0x0032E790
		public override void Tick()
		{
			base.Tick();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].SitePartTick();
			}
			for (int j = 0; j < this.parts.Count; j++)
			{
				this.parts[j].def.Worker.SitePartWorkerTick(this.parts[j]);
			}
			if (base.HasMap)
			{
				this.CheckRecordAssaultSuccessfulTale();
				this.CheckAllEnemiesDefeated();
			}
		}

		// Token: 0x06008E06 RID: 36358 RVA: 0x0033061C File Offset: 0x0032E81C
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			Map map = base.Map;
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostMapGenerate(map);
			}
			float num = 0f;
			for (int j = 0; j < this.parts.Count; j++)
			{
				num = Mathf.Max(num, this.parts[j].def.forceExitAndRemoveMapCountdownDurationDays);
			}
			num *= MapParentTuning.SiteDetectionCountdownMultiplier.RandomInRange;
			if (!this.parts.Any((SitePart p) => p.def.disallowsAutomaticDetectionTimerStart))
			{
				int ticks = Mathf.RoundToInt(num * 60000f);
				base.GetComponent<TimedDetectionRaids>().StartDetectionCountdown(ticks, -1);
			}
			this.allEnemiesDefeatedSignalSent = false;
		}

		// Token: 0x06008E07 RID: 36359 RVA: 0x00330704 File Offset: 0x0032E904
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostDrawExtraSelectionOverlays(this.parts[i]);
			}
		}

		// Token: 0x06008E08 RID: 36360 RVA: 0x00330754 File Offset: 0x0032E954
		public override void Notify_MyMapAboutToBeRemoved()
		{
			base.Notify_MyMapAboutToBeRemoved();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.Notify_SiteMapAboutToBeRemoved(this.parts[i]);
			}
		}

		// Token: 0x06008E09 RID: 36361 RVA: 0x003307A4 File Offset: 0x0032E9A4
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			if (!base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				alsoRemoveWorldObject = !this.parts.Any((SitePart x) => x.def.Worker is SitePartWorker_ConditionCauser && x.conditionCauser != null && !x.conditionCauser.Destroyed);
				if (this.parts.Any(delegate(SitePart x)
				{
					SitePartWorker_AncientAltar sitePartWorker_AncientAltar;
					return (sitePartWorker_AncientAltar = (x.def.Worker as SitePartWorker_AncientAltar)) != null && sitePartWorker_AncientAltar.ShouldKeepMapForRelic(x);
				}))
				{
					alsoRemoveWorldObject = false;
				}
				return true;
			}
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x06008E0A RID: 36362 RVA: 0x00330828 File Offset: 0x0032EA28
		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			for (int i = 0; i < this.parts.Count; i++)
			{
				outChildren.Add(this.parts[i]);
			}
		}

		// Token: 0x06008E0B RID: 36363 RVA: 0x00330864 File Offset: 0x0032EA64
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(caravan))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (!base.HasMap)
			{
				foreach (FloatMenuOption floatMenuOption2 in CaravanArrivalAction_VisitSite.GetFloatMenuOptions(caravan, this))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06008E0C RID: 36364 RVA: 0x0033087B File Offset: 0x0032EA7B
		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetTransportPodsFloatMenuOptions(pods, representative))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalAction_VisitSite.GetFloatMenuOptions(representative, pods, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008E0D RID: 36365 RVA: 0x00330899 File Offset: 0x0032EA99
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.HasMap && Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return SettleInExistingMapUtility.SettleCommand(base.Map, true);
			}
			yield break;
			yield break;
		}

		// Token: 0x06008E0E RID: 36366 RVA: 0x003308AC File Offset: 0x0032EAAC
		private void CheckRecordAssaultSuccessfulTale()
		{
			if (this.anyEnemiesInitially && !this.caravanAssaultSuccessfulTaleRecorded && !GenHostility.AnyHostileActiveThreatToPlayer(base.Map, false, true))
			{
				this.caravanAssaultSuccessfulTaleRecorded = true;
				if (base.Map.mapPawns.FreeColonists.Any<Pawn>())
				{
					TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, new object[]
					{
						base.Map.mapPawns.FreeColonists.RandomElement<Pawn>()
					});
				}
			}
		}

		// Token: 0x06008E0F RID: 36367 RVA: 0x00330920 File Offset: 0x0032EB20
		private void CheckAllEnemiesDefeated()
		{
			if (this.allEnemiesDefeatedSignalSent || !base.HasMap || GenHostility.AnyHostileActiveThreatToPlayer(base.Map, true, true))
			{
				return;
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "AllEnemiesDefeated", this.Named("SUBJECT"));
			this.allEnemiesDefeatedSignalSent = true;
		}

		// Token: 0x06008E10 RID: 36368 RVA: 0x00330970 File Offset: 0x0032EB70
		public override bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			Site site = other as Site;
			return site != null && site.MainSitePartDef == this.MainSitePartDef;
		}

		// Token: 0x06008E11 RID: 36369 RVA: 0x00330998 File Offset: 0x0032EB98
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			Site.tmpSitePartsLabels.Clear();
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (!this.parts[i].hidden)
				{
					if (this.MainSitePart == this.parts[i] && !this.parts[i].def.mainPartAllThreatsLabel.NullOrEmpty() && this.ActualThreatPoints > 0f)
					{
						stringBuilder.Length = 0;
						stringBuilder.Append(this.parts[i].def.mainPartAllThreatsLabel.CapitalizeFirst());
						break;
					}
					string postProcessedThreatLabel = this.parts[i].def.Worker.GetPostProcessedThreatLabel(this, this.parts[i]);
					if (!postProcessedThreatLabel.NullOrEmpty())
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.Append(postProcessedThreatLabel.CapitalizeFirst());
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06008E12 RID: 36370 RVA: 0x00330AB0 File Offset: 0x0032ECB0
		public override string GetDescription()
		{
			string text = this.MainSitePartDef.description;
			string description = base.GetDescription();
			if (!description.NullOrEmpty())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += description;
			}
			return text;
		}

		// Token: 0x040059AC RID: 22956
		public string customLabel;

		// Token: 0x040059AD RID: 22957
		public List<SitePart> parts = new List<SitePart>();

		// Token: 0x040059AE RID: 22958
		public bool sitePartsKnown = true;

		// Token: 0x040059AF RID: 22959
		public bool factionMustRemainHostile;

		// Token: 0x040059B0 RID: 22960
		public float desiredThreatPoints;

		// Token: 0x040059B1 RID: 22961
		private SiteCoreBackCompat coreBackCompat;

		// Token: 0x040059B2 RID: 22962
		private bool anyEnemiesInitially;

		// Token: 0x040059B3 RID: 22963
		private bool caravanAssaultSuccessfulTaleRecorded;

		// Token: 0x040059B4 RID: 22964
		private bool allEnemiesDefeatedSignalSent;

		// Token: 0x040059B5 RID: 22965
		private Material cachedMat;

		// Token: 0x040059B6 RID: 22966
		private static readonly IntVec3 DefaultMapSize = new IntVec3(120, 1, 120);

		// Token: 0x040059B7 RID: 22967
		private static List<string> tmpSitePartsLabels = new List<string>();
	}
}
