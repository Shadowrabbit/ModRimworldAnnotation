using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x0200212D RID: 8493
	[StaticConstructorOnStartup]
	public class MapParent : WorldObject, IThingHolder
	{
		// Token: 0x17001A91 RID: 6801
		// (get) Token: 0x0600B46C RID: 46188 RVA: 0x000752F8 File Offset: 0x000734F8
		public bool HasMap
		{
			get
			{
				return this.Map != null;
			}
		}

		// Token: 0x17001A92 RID: 6802
		// (get) Token: 0x0600B46D RID: 46189 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A93 RID: 6803
		// (get) Token: 0x0600B46E RID: 46190 RVA: 0x00075303 File Offset: 0x00073503
		public Map Map
		{
			get
			{
				return Current.Game.FindMap(this);
			}
		}

		// Token: 0x17001A94 RID: 6804
		// (get) Token: 0x0600B46F RID: 46191 RVA: 0x00075310 File Offset: 0x00073510
		public virtual MapGeneratorDef MapGeneratorDef
		{
			get
			{
				if (this.def.mapGenerator == null)
				{
					return MapGeneratorDefOf.Encounter;
				}
				return this.def.mapGenerator;
			}
		}

		// Token: 0x17001A95 RID: 6805
		// (get) Token: 0x0600B470 RID: 46192 RVA: 0x00075330 File Offset: 0x00073530
		public virtual IEnumerable<GenStepWithParams> ExtraGenStepDefs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17001A96 RID: 6806
		// (get) Token: 0x0600B471 RID: 46193 RVA: 0x00075339 File Offset: 0x00073539
		public override bool ExpandMore
		{
			get
			{
				return base.ExpandMore || this.HasMap;
			}
		}

		// Token: 0x17001A97 RID: 6807
		// (get) Token: 0x0600B472 RID: 46194 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool HandlesConditionCausers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600B473 RID: 46195 RVA: 0x003456A8 File Offset: 0x003438A8
		public virtual void PostMapGenerate()
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMapGenerate();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "MapGenerated", this.Named("SUBJECT"));
		}

		// Token: 0x0600B474 RID: 46196 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_MyMapAboutToBeRemoved()
		{
		}

		// Token: 0x0600B475 RID: 46197 RVA: 0x003456F4 File Offset: 0x003438F4
		public virtual void Notify_MyMapRemoved(Map map)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMyMapRemoved();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "MapRemoved", this.Named("SUBJECT"));
		}

		// Token: 0x0600B476 RID: 46198 RVA: 0x00345740 File Offset: 0x00343940
		public virtual void Notify_CaravanFormed(Caravan caravan)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostCaravanFormed(caravan);
			}
		}

		// Token: 0x0600B477 RID: 46199 RVA: 0x0007534B File Offset: 0x0007354B
		public virtual void Notify_HibernatableChanged()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x0600B478 RID: 46200 RVA: 0x0007534B File Offset: 0x0007354B
		public virtual void FinalizeLoading()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x0600B479 RID: 46201 RVA: 0x00050420 File Offset: 0x0004E620
		public virtual bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x0600B47A RID: 46202 RVA: 0x00075353 File Offset: 0x00073553
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.HasMap)
			{
				Current.Game.DeinitAndRemoveMap(this.Map);
			}
		}

		// Token: 0x0600B47B RID: 46203 RVA: 0x00075373 File Offset: 0x00073573
		public override void Tick()
		{
			base.Tick();
			this.CheckRemoveMapNow();
		}

		// Token: 0x0600B47C RID: 46204 RVA: 0x00075381 File Offset: 0x00073581
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forceRemoveWorldObjectWhenMapRemoved, "forceRemoveWorldObjectWhenMapRemoved", false, false);
		}

		// Token: 0x0600B47D RID: 46205 RVA: 0x0007539B File Offset: 0x0007359B
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.HasMap)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandShowMap".Translate(),
					defaultDesc = "CommandShowMapDesc".Translate(),
					icon = MapParent.ShowMapCommand,
					hotKey = KeyBindingDefOf.Misc1,
					action = delegate()
					{
						Current.Game.CurrentMap = this.Map;
						if (!CameraJumper.TryHideWorld())
						{
							SoundDefOf.TabClose.PlayOneShotOnCamera(null);
						}
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B47E RID: 46206 RVA: 0x000753AB File Offset: 0x000735AB
		public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			foreach (IncidentTargetTagDef incidentTargetTagDef in base.IncidentTargetTags())
			{
				yield return incidentTargetTagDef;
			}
			IEnumerator<IncidentTargetTagDef> enumerator = null;
			if (this.hibernatableIncidentTargets != null && this.hibernatableIncidentTargets.Count > 0)
			{
				foreach (IncidentTargetTagDef incidentTargetTagDef2 in this.hibernatableIncidentTargets)
				{
					yield return incidentTargetTagDef2;
				}
				HashSet<IncidentTargetTagDef>.Enumerator enumerator2 = default(HashSet<IncidentTargetTagDef>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B47F RID: 46207 RVA: 0x000753BB File Offset: 0x000735BB
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(caravan))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.UseGenericEnterMapFloatMenuOption)
			{
				foreach (FloatMenuOption floatMenuOption2 in CaravanArrivalAction_Enter.GetFloatMenuOptions(caravan, this))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B480 RID: 46208 RVA: 0x000753D2 File Offset: 0x000735D2
		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetTransportPodsFloatMenuOptions(pods, representative))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (TransportPodsArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this))
			{
				Action<LocalTargetInfo> <>9__1;
				yield return new FloatMenuOption("LandInExistingMap".Translate(this.Label), delegate()
				{
					Map myMap = representative.parent.Map;
					Map map = this.Map;
					Current.Game.CurrentMap = map;
					CameraJumper.TryHideWorld();
					Targeter targeter = Find.Targeter;
					TargetingParameters targetParams = TargetingParameters.ForDropPodsDestination();
					Action<LocalTargetInfo> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(LocalTargetInfo x)
						{
							representative.TryLaunch(this.Tile, new TransportPodsArrivalAction_LandInSpecificCell(this, x.Cell, representative.parent.TryGetComp<CompShuttle>() != null));
						});
					}
					targeter.BeginTargeting(targetParams, action, null, delegate()
					{
						if (Find.Maps.Contains(myMap))
						{
							Current.Game.CurrentMap = myMap;
						}
					}, CompLaunchable.TargeterMouseAttachment);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B481 RID: 46209 RVA: 0x000753F0 File Offset: 0x000735F0
		public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<int, TransportPodsArrivalAction> launchAction)
		{
			if (TransportPodsArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this))
			{
				Action<LocalTargetInfo> <>9__1;
				Action<LocalTargetInfo> <>9__2;
				Func<LocalTargetInfo, bool> <>9__3;
				yield return new FloatMenuOption("LandInExistingMap".Translate(this.Label), delegate()
				{
					Map map = this.Map;
					Current.Game.CurrentMap = map;
					CameraJumper.TryHideWorld();
					Targeter targeter = Find.Targeter;
					TargetingParameters targetParams = TargetingParameters.ForDropPodsDestination();
					Action<LocalTargetInfo> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(LocalTargetInfo x)
						{
							launchAction(this.Tile, new TransportPodsArrivalAction_LandInSpecificCell(this, x.Cell, true));
						});
					}
					Action<LocalTargetInfo> highlightAction;
					if ((highlightAction = <>9__2) == null)
					{
						highlightAction = (<>9__2 = delegate(LocalTargetInfo x)
						{
							RoyalTitlePermitWorker_CallShuttle.DrawShuttleGhost(x, this.Map);
						});
					}
					Func<LocalTargetInfo, bool> targetValidator;
					if ((targetValidator = <>9__3) == null)
					{
						targetValidator = (<>9__3 = delegate(LocalTargetInfo x)
						{
							AcceptanceReport acceptanceReport = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(x, this.Map);
							if (!acceptanceReport.Accepted)
							{
								Messages.Message(acceptanceReport.Reason, new LookTargets(this), MessageTypeDefOf.RejectInput, false);
							}
							return acceptanceReport.Accepted;
						});
					}
					targeter.BeginTargeting(targetParams, action, highlightAction, targetValidator, null, null, CompLaunchable.TargeterMouseAttachment);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			yield break;
		}

		// Token: 0x0600B482 RID: 46210 RVA: 0x00345774 File Offset: 0x00343974
		public void CheckRemoveMapNow()
		{
			bool flag;
			if (this.HasMap && this.ShouldRemoveMapNow(out flag))
			{
				Map map = this.Map;
				Current.Game.DeinitAndRemoveMap(map);
				if (flag || this.forceRemoveWorldObjectWhenMapRemoved)
				{
					this.Destroy();
				}
			}
		}

		// Token: 0x0600B483 RID: 46211 RVA: 0x003457B8 File Offset: 0x003439B8
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.EnterCooldownBlocksEntering())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "EnterCooldown".Translate(this.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true));
			}
			if (!this.HandlesConditionCausers && this.HasMap)
			{
				List<Thing> list = this.Map.listerThings.ThingsInGroup(ThingRequestGroup.ConditionCauser);
				for (int i = 0; i < list.Count; i++)
				{
					text += "\n" + list[i].LabelShortCap + " (" + "ConditionCauserRadius".Translate(list[i].TryGetComp<CompCauseGameCondition>().Props.worldRange) + ")";
				}
			}
			return text;
		}

		// Token: 0x0600B484 RID: 46212 RVA: 0x003458A8 File Offset: 0x00343AA8
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (!this.HandlesConditionCausers && this.HasMap)
			{
				int num = 0;
				List<Thing> list = this.Map.listerThings.ThingsInGroup(ThingRequestGroup.ConditionCauser);
				for (int i = 0; i < list.Count; i++)
				{
					num = Mathf.Max(num, list[i].TryGetComp<CompCauseGameCondition>().Props.worldRange);
				}
				if (num > 0)
				{
					GenDraw.DrawWorldRadiusRing(base.Tile, num);
				}
			}
		}

		// Token: 0x0600B485 RID: 46213 RVA: 0x0000C32E File Offset: 0x0000A52E
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x0600B486 RID: 46214 RVA: 0x0007540E File Offset: 0x0007360E
		public virtual void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.HasMap)
			{
				outChildren.Add(this.Map);
			}
		}

		// Token: 0x0600B487 RID: 46215 RVA: 0x00345920 File Offset: 0x00343B20
		private void RecalculateHibernatableIncidentTargets()
		{
			this.hibernatableIncidentTargets = null;
			foreach (ThingWithComps thing in this.Map.listerThings.ThingsOfDef(ThingDefOf.Ship_Reactor).OfType<ThingWithComps>())
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Starting && compHibernatable.Props.incidentTargetWhileStarting != null)
				{
					if (this.hibernatableIncidentTargets == null)
					{
						this.hibernatableIncidentTargets = new HashSet<IncidentTargetTagDef>();
					}
					this.hibernatableIncidentTargets.Add(compHibernatable.Props.incidentTargetWhileStarting);
				}
			}
		}

		// Token: 0x04007BDE RID: 31710
		public bool forceRemoveWorldObjectWhenMapRemoved;

		// Token: 0x04007BDF RID: 31711
		private HashSet<IncidentTargetTagDef> hibernatableIncidentTargets;

		// Token: 0x04007BE0 RID: 31712
		private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap", true);
	}
}
