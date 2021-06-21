using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002156 RID: 8534
	[StaticConstructorOnStartup]
	public class Site : MapParent
	{
		// Token: 0x17001AD7 RID: 6871
		// (get) Token: 0x0600B5BD RID: 46525 RVA: 0x003495A4 File Offset: 0x003477A4
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

		// Token: 0x17001AD8 RID: 6872
		// (get) Token: 0x0600B5BE RID: 46526 RVA: 0x0007604A File Offset: 0x0007424A
		public override Texture2D ExpandingIcon
		{
			get
			{
				return this.MainSitePartDef.ExpandingIconTexture;
			}
		}

		// Token: 0x17001AD9 RID: 6873
		// (get) Token: 0x0600B5BF RID: 46527 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool HandlesConditionCausers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001ADA RID: 6874
		// (get) Token: 0x0600B5C0 RID: 46528 RVA: 0x0034961C File Offset: 0x0034781C
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

		// Token: 0x17001ADB RID: 6875
		// (get) Token: 0x0600B5C1 RID: 46529 RVA: 0x00076057 File Offset: 0x00074257
		public override bool AppendFactionToInspectString
		{
			get
			{
				return this.MainSitePartDef.applyFactionColorToSiteTexture || this.MainSitePartDef.showFactionInInspectString;
			}
		}

		//主要地点部分
		private SitePart MainSitePart
		{
			get
			{
				if (!this.parts.Any<SitePart>())
				{
					Log.ErrorOnce("Site without any SitePart at " + base.Tile, this.ID ^ 93890909, false);
					return null;
				}
				if (this.parts[0].hidden)
				{
					Log.ErrorOnce("Site with first SitePart hidden at " + base.Tile, this.ID ^ 48471239, false);
					return this.parts[0];
				}
				return this.parts[0];
			}
		}

		// Token: 0x17001ADD RID: 6877
		// (get) Token: 0x0600B5C3 RID: 46531 RVA: 0x00076073 File Offset: 0x00074273
		public SitePartDef MainSitePartDef
		{
			get
			{
				return this.MainSitePart.def;
			}
		}

		// Token: 0x17001ADE RID: 6878
		// (get) Token: 0x0600B5C4 RID: 46532 RVA: 0x00076080 File Offset: 0x00074280
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

		// Token: 0x17001ADF RID: 6879
		// (get) Token: 0x0600B5C5 RID: 46533 RVA: 0x00349720 File Offset: 0x00347920
		public string ApproachOrderString
		{
			get
			{
				return this.MainSitePartDef.approachOrderString.NullOrEmpty() ? "ApproachSite".Translate(this.Label) : this.MainSitePartDef.approachOrderString.Formatted(this.Label);
			}
		}

		// Token: 0x17001AE0 RID: 6880
		// (get) Token: 0x0600B5C6 RID: 46534 RVA: 0x00349778 File Offset: 0x00347978
		public string ApproachingReportString
		{
			get
			{
				return this.MainSitePartDef.approachingReportString.NullOrEmpty() ? "ApproachingSite".Translate(this.Label) : this.MainSitePartDef.approachingReportString.Formatted(this.Label);
			}
		}

		// Token: 0x17001AE1 RID: 6881
		// (get) Token: 0x0600B5C7 RID: 46535 RVA: 0x003497D0 File Offset: 0x003479D0
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

		// Token: 0x17001AE2 RID: 6882
		// (get) Token: 0x0600B5C8 RID: 46536 RVA: 0x00349814 File Offset: 0x00347A14
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

		// Token: 0x17001AE3 RID: 6883
		// (get) Token: 0x0600B5C9 RID: 46537 RVA: 0x00349874 File Offset: 0x00347A74
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

		// Token: 0x17001AE4 RID: 6884
		// (get) Token: 0x0600B5CA RID: 46538 RVA: 0x00076090 File Offset: 0x00074290
		public bool HasWorldObjectTimeout
		{
			get
			{
				return this.WorldObjectTimeoutTicksLeft != -1;
			}
		}

		// Token: 0x17001AE5 RID: 6885
		// (get) Token: 0x0600B5CB RID: 46539 RVA: 0x003498B4 File Offset: 0x00347AB4
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

		// Token: 0x0600B5CC RID: 46540 RVA: 0x00349938 File Offset: 0x00347B38
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

		// Token: 0x0600B5CD RID: 46541 RVA: 0x003499B0 File Offset: 0x00347BB0
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
					Log.Error("Some site parts were null after loading.", false);
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					this.parts[i].site = this;
				}
				BackCompatibility.PostExposeData(this);
			}
		}

		// Token: 0x0600B5CE RID: 46542 RVA: 0x0007609E File Offset: 0x0007429E
		public void AddPart(SitePart part)
		{
			this.parts.Add(part);
			part.def.Worker.Init(this, part);
		}

		// Token: 0x0600B5CF RID: 46543 RVA: 0x00349B14 File Offset: 0x00347D14
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

		// Token: 0x0600B5D0 RID: 46544 RVA: 0x00349BA0 File Offset: 0x00347DA0
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
			int ticks = Mathf.RoundToInt(num * 60000f);
			base.GetComponent<TimedDetectionRaids>().StartDetectionCountdown(ticks, -1);
			this.allEnemiesDefeatedSignalSent = false;
		}

		// Token: 0x0600B5D1 RID: 46545 RVA: 0x00349C5C File Offset: 0x00347E5C
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostDrawExtraSelectionOverlays(this.parts[i]);
			}
		}

		// Token: 0x0600B5D2 RID: 46546 RVA: 0x00349CAC File Offset: 0x00347EAC
		public override void Notify_MyMapAboutToBeRemoved()
		{
			base.Notify_MyMapAboutToBeRemoved();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.Notify_SiteMapAboutToBeRemoved(this.parts[i]);
			}
		}

		// Token: 0x0600B5D3 RID: 46547 RVA: 0x00349CFC File Offset: 0x00347EFC
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			if (!base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				alsoRemoveWorldObject = !this.parts.Any((SitePart x) => x.def.Worker is SitePartWorker_ConditionCauser && x.conditionCauser != null && !x.conditionCauser.Destroyed);
				return true;
			}
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x0600B5D4 RID: 46548 RVA: 0x00349D50 File Offset: 0x00347F50
		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			for (int i = 0; i < this.parts.Count; i++)
			{
				outChildren.Add(this.parts[i]);
			}
		}

		// Token: 0x0600B5D5 RID: 46549 RVA: 0x000760BE File Offset: 0x000742BE
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

		// Token: 0x0600B5D6 RID: 46550 RVA: 0x000760D5 File Offset: 0x000742D5
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

		// Token: 0x0600B5D7 RID: 46551 RVA: 0x000760F3 File Offset: 0x000742F3
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

		// Token: 0x0600B5D8 RID: 46552 RVA: 0x00349D8C File Offset: 0x00347F8C
		private void CheckRecordAssaultSuccessfulTale()
		{
			if (this.anyEnemiesInitially && !this.caravanAssaultSuccessfulTaleRecorded && !GenHostility.AnyHostileActiveThreatToPlayer(base.Map, false))
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

		// Token: 0x0600B5D9 RID: 46553 RVA: 0x00349E00 File Offset: 0x00348000
		private void CheckAllEnemiesDefeated()
		{
			if (this.allEnemiesDefeatedSignalSent || !base.HasMap || GenHostility.AnyHostileActiveThreatToPlayer(base.Map, true))
			{
				return;
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "AllEnemiesDefeated", this.Named("SUBJECT"));
			this.allEnemiesDefeatedSignalSent = true;
		}

		// Token: 0x0600B5DA RID: 46554 RVA: 0x00349E50 File Offset: 0x00348050
		public override bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			Site site = other as Site;
			return site != null && site.MainSitePartDef == this.MainSitePartDef;
		}

		// Token: 0x0600B5DB RID: 46555 RVA: 0x00349E78 File Offset: 0x00348078
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

		// Token: 0x0600B5DC RID: 46556 RVA: 0x00349F90 File Offset: 0x00348190
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

		// Token: 0x04007C8A RID: 31882
		public string customLabel;

		// Token: 0x04007C8B RID: 31883
		public List<SitePart> parts = new List<SitePart>();

		// Token: 0x04007C8C RID: 31884
		public bool sitePartsKnown = true;

		// Token: 0x04007C8D RID: 31885
		public bool factionMustRemainHostile;

		// Token: 0x04007C8E RID: 31886
		public float desiredThreatPoints;

		// Token: 0x04007C8F RID: 31887
		private SiteCoreBackCompat coreBackCompat;

		// Token: 0x04007C90 RID: 31888
		private bool anyEnemiesInitially;

		// Token: 0x04007C91 RID: 31889
		private bool caravanAssaultSuccessfulTaleRecorded;

		// Token: 0x04007C92 RID: 31890
		private bool allEnemiesDefeatedSignalSent;

		// Token: 0x04007C93 RID: 31891
		private Material cachedMat;

		// Token: 0x04007C94 RID: 31892
		private static List<string> tmpSitePartsLabels = new List<string>();
	}
}
