using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020017C9 RID: 6089
	[StaticConstructorOnStartup]
	public class MapParent : WorldObject, IThingHolder
	{
		// Token: 0x17001703 RID: 5891
		// (get) Token: 0x06008D5F RID: 36191 RVA: 0x0032DB73 File Offset: 0x0032BD73
		public bool HasMap
		{
			get
			{
				return this.Map != null;
			}
		}

		// Token: 0x17001704 RID: 5892
		// (get) Token: 0x06008D60 RID: 36192 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001705 RID: 5893
		// (get) Token: 0x06008D61 RID: 36193 RVA: 0x0032DB7E File Offset: 0x0032BD7E
		public Map Map
		{
			get
			{
				return Current.Game.FindMap(this);
			}
		}

		// Token: 0x17001706 RID: 5894
		// (get) Token: 0x06008D62 RID: 36194 RVA: 0x0032DB8B File Offset: 0x0032BD8B
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

		// Token: 0x17001707 RID: 5895
		// (get) Token: 0x06008D63 RID: 36195 RVA: 0x0032DBAB File Offset: 0x0032BDAB
		public virtual IEnumerable<GenStepWithParams> ExtraGenStepDefs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17001708 RID: 5896
		// (get) Token: 0x06008D64 RID: 36196 RVA: 0x0032DBB4 File Offset: 0x0032BDB4
		public override bool ExpandMore
		{
			get
			{
				return base.ExpandMore || this.HasMap;
			}
		}

		// Token: 0x17001709 RID: 5897
		// (get) Token: 0x06008D65 RID: 36197 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool HandlesConditionCausers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008D66 RID: 36198 RVA: 0x0032DBC8 File Offset: 0x0032BDC8
		public virtual void PostMapGenerate()
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMapGenerate();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "MapGenerated", this.Named("SUBJECT"));
		}

		// Token: 0x06008D67 RID: 36199 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MyMapAboutToBeRemoved()
		{
		}

		// Token: 0x06008D68 RID: 36200 RVA: 0x0032DC14 File Offset: 0x0032BE14
		public virtual void Notify_MyMapRemoved(Map map)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMyMapRemoved();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "MapRemoved", this.Named("SUBJECT"));
		}

		// Token: 0x06008D69 RID: 36201 RVA: 0x0032DC60 File Offset: 0x0032BE60
		public virtual void Notify_CaravanFormed(Caravan caravan)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostCaravanFormed(caravan);
			}
		}

		// Token: 0x06008D6A RID: 36202 RVA: 0x0032DC92 File Offset: 0x0032BE92
		public virtual void Notify_HibernatableChanged()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x06008D6B RID: 36203 RVA: 0x0032DC92 File Offset: 0x0032BE92
		public virtual void FinalizeLoading()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x06008D6C RID: 36204 RVA: 0x001DE61B File Offset: 0x001DC81B
		public virtual bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x06008D6D RID: 36205 RVA: 0x0032DC9A File Offset: 0x0032BE9A
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.HasMap)
			{
				Current.Game.DeinitAndRemoveMap(this.Map);
			}
		}

		// Token: 0x06008D6E RID: 36206 RVA: 0x0032DCBA File Offset: 0x0032BEBA
		public override void Tick()
		{
			base.Tick();
			this.CheckRemoveMapNow();
		}

		// Token: 0x06008D6F RID: 36207 RVA: 0x0032DCC8 File Offset: 0x0032BEC8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forceRemoveWorldObjectWhenMapRemoved, "forceRemoveWorldObjectWhenMapRemoved", false, false);
			Scribe_Values.Look<bool>(ref this.doorsAlwaysOpenForPlayerPawns, "doorsAlwaysOpenForPlayerPawns", false, false);
		}

		// Token: 0x06008D70 RID: 36208 RVA: 0x0032DCF4 File Offset: 0x0032BEF4
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

		// Token: 0x06008D71 RID: 36209 RVA: 0x0032DD04 File Offset: 0x0032BF04
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

		// Token: 0x06008D72 RID: 36210 RVA: 0x0032DD14 File Offset: 0x0032BF14
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

		// Token: 0x06008D73 RID: 36211 RVA: 0x0032DD2B File Offset: 0x0032BF2B
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
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
			yield break;
		}

		// Token: 0x06008D74 RID: 36212 RVA: 0x0032DD49 File Offset: 0x0032BF49
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
					targeter.BeginTargeting(targetParams, action, highlightAction, targetValidator, null, null, CompLaunchable.TargeterMouseAttachment, true);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
		}

		// Token: 0x06008D75 RID: 36213 RVA: 0x0032DD68 File Offset: 0x0032BF68
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

		// Token: 0x06008D76 RID: 36214 RVA: 0x0032DDAC File Offset: 0x0032BFAC
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

		// Token: 0x06008D77 RID: 36215 RVA: 0x0032DE9C File Offset: 0x0032C09C
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

		// Token: 0x06008D78 RID: 36216 RVA: 0x00002688 File Offset: 0x00000888
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06008D79 RID: 36217 RVA: 0x0032DF12 File Offset: 0x0032C112
		public virtual void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.HasMap)
			{
				outChildren.Add(this.Map);
			}
		}

		// Token: 0x06008D7A RID: 36218 RVA: 0x0032DF34 File Offset: 0x0032C134
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

		// Token: 0x04005988 RID: 22920
		public bool forceRemoveWorldObjectWhenMapRemoved;

		// Token: 0x04005989 RID: 22921
		public bool doorsAlwaysOpenForPlayerPawns;

		// Token: 0x0400598A RID: 22922
		private HashSet<IncidentTargetTagDef> hibernatableIncidentTargets;

		// Token: 0x0400598B RID: 22923
		private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap", true);
	}
}
