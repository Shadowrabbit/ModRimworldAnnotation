using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001840 RID: 6208
	[StaticConstructorOnStartup]
	public class CompShuttle : ThingComp
	{
		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x060089A1 RID: 35233 RVA: 0x0005C65E File Offset: 0x0005A85E
		public bool Autoload
		{
			get
			{
				return this.autoload;
			}
		}

		// Token: 0x17001597 RID: 5527
		// (get) Token: 0x060089A2 RID: 35234 RVA: 0x0005C666 File Offset: 0x0005A866
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.Transporter.LoadingInProgressOrReadyToLaunch;
			}
		}

		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x060089A3 RID: 35235 RVA: 0x0005C673 File Offset: 0x0005A873
		public List<CompTransporter> TransportersInGroup
		{
			get
			{
				return this.Transporter.TransportersInGroup(this.parent.Map);
			}
		}

		// Token: 0x17001599 RID: 5529
		// (get) Token: 0x060089A4 RID: 35236 RVA: 0x0005C68B File Offset: 0x0005A88B
		public bool CanAutoLoot
		{
			get
			{
				return (this.permitShuttle || this.IsMissionShuttle) && !this.parent.Map.IsPlayerHome && !GenHostility.AnyHostileActiveThreatToPlayer(this.parent.Map, true);
			}
		}

		// Token: 0x1700159A RID: 5530
		// (get) Token: 0x060089A5 RID: 35237 RVA: 0x0005C6C5 File Offset: 0x0005A8C5
		public bool ShowLoadingGizmos
		{
			get
			{
				return !this.hideControls && (this.parent.Faction == null || this.parent.Faction == Faction.OfPlayer);
			}
		}

		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x060089A6 RID: 35238 RVA: 0x0005C6F3 File Offset: 0x0005A8F3
		public CompTransporter Transporter
		{
			get
			{
				if (this.cachedCompTransporter == null)
				{
					this.cachedCompTransporter = this.parent.GetComp<CompTransporter>();
				}
				return this.cachedCompTransporter;
			}
		}

		// Token: 0x1700159C RID: 5532
		// (get) Token: 0x060089A7 RID: 35239 RVA: 0x00283454 File Offset: 0x00281654
		public bool AnyInGroupIsUnderRoof
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					if (transportersInGroup[i].parent.Position.Roofed(this.parent.Map))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700159D RID: 5533
		// (get) Token: 0x060089A8 RID: 35240 RVA: 0x002834A0 File Offset: 0x002816A0
		private bool Autoloadable
		{
			get
			{
				if (this.cachedTransporterList == null)
				{
					this.cachedTransporterList = new List<CompTransporter>
					{
						this.Transporter
					};
				}
				foreach (Pawn thing in TransporterUtility.AllSendablePawns_NewTmp(this.cachedTransporterList, this.parent.Map, false))
				{
					if (!this.IsRequired(thing))
					{
						return false;
					}
				}
				foreach (Thing thing2 in TransporterUtility.AllSendableItems_NewTmp(this.cachedTransporterList, this.parent.Map, false))
				{
					if (!this.IsRequired(thing2))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x1700159E RID: 5534
		// (get) Token: 0x060089A9 RID: 35241 RVA: 0x0028357C File Offset: 0x0028177C
		public bool AllRequiredThingsLoaded
		{
			get
			{
				ThingOwner innerContainer = this.Transporter.innerContainer;
				for (int i = 0; i < this.requiredPawns.Count; i++)
				{
					if (!this.requiredPawns[i].Dead && !innerContainer.Contains(this.requiredPawns[i]))
					{
						return false;
					}
				}
				if (this.requiredColonistCount > 0)
				{
					int num = 0;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && pawn.IsFreeColonist)
						{
							num++;
						}
					}
					if (num < this.requiredColonistCount)
					{
						return false;
					}
				}
				CompShuttle.tmpRequiredItemsWithoutDuplicates.Clear();
				for (int k = 0; k < this.requiredItems.Count; k++)
				{
					bool flag = false;
					for (int l = 0; l < CompShuttle.tmpRequiredItemsWithoutDuplicates.Count; l++)
					{
						if (CompShuttle.tmpRequiredItemsWithoutDuplicates[l].ThingDef == this.requiredItems[k].ThingDef)
						{
							CompShuttle.tmpRequiredItemsWithoutDuplicates[l] = CompShuttle.tmpRequiredItemsWithoutDuplicates[l].WithCount(CompShuttle.tmpRequiredItemsWithoutDuplicates[l].Count + this.requiredItems[k].Count);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						CompShuttle.tmpRequiredItemsWithoutDuplicates.Add(this.requiredItems[k]);
					}
				}
				for (int m = 0; m < CompShuttle.tmpRequiredItemsWithoutDuplicates.Count; m++)
				{
					int num2 = 0;
					for (int n = 0; n < innerContainer.Count; n++)
					{
						if (innerContainer[n].def == CompShuttle.tmpRequiredItemsWithoutDuplicates[m].ThingDef)
						{
							num2 += innerContainer[n].stackCount;
						}
					}
					if (num2 < CompShuttle.tmpRequiredItemsWithoutDuplicates[m].Count)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x060089AA RID: 35242 RVA: 0x00283784 File Offset: 0x00281984
		public TaggedString RequiredThingsLabel
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.requiredPawns.Count; i++)
				{
					if (!this.requiredPawns[i].Dead)
					{
						stringBuilder.AppendLine("  - " + this.requiredPawns[i].NameShortColored.Resolve());
					}
				}
				for (int j = 0; j < this.requiredItems.Count; j++)
				{
					stringBuilder.AppendLine("  - " + this.requiredItems[j].LabelCap);
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
		}

		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x060089AB RID: 35243 RVA: 0x0005C714 File Offset: 0x0005A914
		public bool IsMissionShuttle
		{
			get
			{
				return this.missionShuttleTarget != null || this.missionShuttleHome != null;
			}
		}

		// Token: 0x060089AC RID: 35244 RVA: 0x0005C729 File Offset: 0x0005A929
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shuttle is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 8811221, false);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x060089AD RID: 35245 RVA: 0x0005C74A File Offset: 0x0005A94A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.ShowLoadingGizmos)
			{
				if (this.Autoloadable)
				{
					yield return new Command_Toggle
					{
						defaultLabel = "CommandAutoloadTransporters".Translate(),
						defaultDesc = "CommandAutoloadTransportersDesc".Translate(),
						icon = CompShuttle.AutoloadToggleTex,
						isActive = (() => this.autoload),
						toggleAction = delegate()
						{
							this.autoload = !this.autoload;
							if (this.autoload && !this.LoadingInProgressOrReadyToLaunch)
							{
								TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
							}
							this.CheckAutoload();
						}
					};
				}
				if (!this.IsMissionShuttle)
				{
					Command_Action command_Action = new Command_Action();
					command_Action.defaultLabel = "CommandSendShuttle".Translate();
					command_Action.defaultDesc = "CommandSendShuttleDesc".Translate();
					command_Action.icon = CompShuttle.SendCommandTex;
					command_Action.alsoClickIfOtherInGroupClicked = false;
					command_Action.action = delegate()
					{
						this.Send();
					};
					if (!this.LoadingInProgressOrReadyToLaunch || !this.AllRequiredThingsLoaded)
					{
						command_Action.Disable("CommandSendShuttleFailMissingRequiredThing".Translate());
					}
					yield return command_Action;
				}
			}
			foreach (Gizmo gizmo2 in QuestUtility.GetQuestRelatedGizmos(this.parent))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060089AE RID: 35246 RVA: 0x0005C75A File Offset: 0x0005A95A
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!selPawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				yield break;
			}
			string text = "EnterShuttle".Translate();
			if (!this.IsAllowedNow(selPawn))
			{
				yield return new FloatMenuOption(text + " (" + "NotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield break;
			}
			yield return new FloatMenuOption(text, delegate()
			{
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
				}
				Job job = JobMaker.MakeJob(JobDefOf.EnterTransporter, this.parent);
				selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield break;
		}

		// Token: 0x060089AF RID: 35247 RVA: 0x0005C771 File Offset: 0x0005A971
		public override IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(List<Pawn> selPawns)
		{
			CompShuttle.tmpAllowedPawns.Clear();
			string text = "EnterShuttle".Translate();
			for (int i = 0; i < selPawns.Count; i++)
			{
				if (selPawns[i].CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					CompShuttle.tmpAllowedPawns.Add(selPawns[i]);
				}
			}
			if (!CompShuttle.tmpAllowedPawns.Any<Pawn>())
			{
				yield return new FloatMenuOption(text + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield break;
			}
			for (int j = CompShuttle.tmpAllowedPawns.Count - 1; j >= 0; j--)
			{
				if (!this.IsAllowedNow(CompShuttle.tmpAllowedPawns[j]))
				{
					CompShuttle.tmpAllowedPawns.RemoveAt(j);
				}
			}
			if (!CompShuttle.tmpAllowedPawns.Any<Pawn>())
			{
				yield return new FloatMenuOption(text + " (" + "NotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield break;
			}
			yield return new FloatMenuOption(text, delegate()
			{
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
				}
				for (int k = 0; k < CompShuttle.tmpAllowedPawns.Count; k++)
				{
					Pawn pawn = CompShuttle.tmpAllowedPawns[k];
					if (pawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) && !pawn.Downed && !pawn.Dead && pawn.Spawned)
					{
						Job job = JobMaker.MakeJob(JobDefOf.EnterTransporter, this.parent);
						CompShuttle.tmpAllowedPawns[k].jobs.TryTakeOrderedJob(job, JobTag.Misc);
					}
				}
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield break;
		}

		// Token: 0x060089B0 RID: 35248 RVA: 0x00283838 File Offset: 0x00281A38
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(120))
			{
				this.CheckAutoload();
			}
			if (this.parent.Spawned && (this.dropEverythingOnArrival || (this.sendAwayIfQuestFinished != null && this.sendAwayIfQuestFinished.Historical)) && this.parent.IsHashIntervalTick(60))
			{
				this.OffloadShuttleOrSend();
			}
			if (this.leaveASAP && this.parent.Spawned)
			{
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
				}
				this.Send();
			}
			if (this.leaveAfterTicks > 0 && this.parent.Spawned && !this.IsMissionShuttle)
			{
				this.leaveAfterTicks--;
				if (this.leaveAfterTicks == 0)
				{
					if (!this.LoadingInProgressOrReadyToLaunch)
					{
						TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
					}
					this.Send();
				}
			}
			if (!this.IsMissionShuttle && !this.dropEverythingOnArrival)
			{
				if (!this.leaveImmediatelyWhenSatisfied || !this.AllRequiredThingsLoaded)
				{
					if (!this.sendAwayIfAllDespawned.NullOrEmpty<Thing>())
					{
						if (this.sendAwayIfAllDespawned.All(delegate(Thing p)
						{
							Pawn pawn;
							return !p.Spawned && ((pawn = (p as Pawn)) == null || pawn.CarriedBy == null);
						}))
						{
							goto IL_15B;
						}
					}
					if (this.sendAwayIfAllPawnsLeftToLoadAreNotOfFaction == null || !this.requiredPawns.All((Pawn p) => this.Transporter.innerContainer.Contains(p) || p.Faction != this.sendAwayIfAllPawnsLeftToLoadAreNotOfFaction))
					{
						return;
					}
				}
				IL_15B:
				this.Send();
			}
		}

		// Token: 0x060089B1 RID: 35249 RVA: 0x002839A8 File Offset: 0x00281BA8
		private void OffloadShuttleOrSend()
		{
			CompShuttle.<>c__DisplayClass61_0 CS$<>8__locals1 = new CompShuttle.<>c__DisplayClass61_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.thingToDrop = null;
			float num = 0f;
			for (int i = 0; i < this.Transporter.innerContainer.Count; i++)
			{
				Thing thing = this.Transporter.innerContainer[i];
				float num2 = CS$<>8__locals1.<OffloadShuttleOrSend>g__GetDropPriority|0(thing);
				if (num2 > num)
				{
					CS$<>8__locals1.thingToDrop = thing;
					num = num2;
				}
			}
			if (CS$<>8__locals1.thingToDrop != null)
			{
				IntVec3 dropLoc = this.parent.Position + CompShuttle.DropoffSpotOffset;
				Thing thing2;
				if (this.Transporter.innerContainer.TryDrop_NewTmp(CS$<>8__locals1.thingToDrop, dropLoc, this.parent.Map, ThingPlaceMode.Near, out thing2, null, delegate(IntVec3 c)
				{
					Pawn pawn2;
					return !c.Fogged(CS$<>8__locals1.<>4__this.parent.Map) && ((pawn2 = (CS$<>8__locals1.thingToDrop as Pawn)) == null || !pawn2.Downed || c.GetFirstPawn(CS$<>8__locals1.<>4__this.parent.Map) == null);
				}, false))
				{
					this.Transporter.Notify_ThingRemoved(CS$<>8__locals1.thingToDrop);
					this.droppedOnArrival.Add(CS$<>8__locals1.thingToDrop);
					Pawn pawn;
					if ((pawn = (CS$<>8__locals1.thingToDrop as Pawn)) != null)
					{
						if (pawn.IsColonist && pawn.Spawned && !this.parent.Map.IsPlayerHome)
						{
							pawn.drafter.Drafted = true;
						}
						if (pawn.guest != null && pawn.guest.IsPrisoner)
						{
							pawn.guest.WaitInsteadOfEscapingForDefaultTicks();
							return;
						}
					}
				}
			}
			else
			{
				if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
				}
				if (!this.stayAfterDroppedEverythingOnArrival || (this.sendAwayIfQuestFinished != null && this.sendAwayIfQuestFinished.Historical))
				{
					this.Send();
					return;
				}
				this.dropEverythingOnArrival = false;
			}
		}

		// Token: 0x060089B2 RID: 35250 RVA: 0x00283B44 File Offset: 0x00281D44
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			CompShuttle.tmpRequiredLabels.Clear();
			if (this.requiredColonistCount > 0)
			{
				CompShuttle.tmpRequiredLabels.Add(this.requiredColonistCount + " " + ((this.requiredColonistCount > 1) ? Faction.OfPlayer.def.pawnsPlural : Faction.OfPlayer.def.pawnSingular));
			}
			for (int i = 0; i < this.requiredPawns.Count; i++)
			{
				if (!this.requiredPawns[i].Dead && !this.Transporter.innerContainer.Contains(this.requiredPawns[i]))
				{
					CompShuttle.tmpRequiredLabels.Add(this.requiredPawns[i].LabelShort);
				}
			}
			for (int j = 0; j < this.requiredItems.Count; j++)
			{
				if (this.Transporter.innerContainer.TotalStackCountOfDef(this.requiredItems[j].ThingDef) < this.requiredItems[j].Count)
				{
					CompShuttle.tmpRequiredLabels.Add(this.requiredItems[j].Label);
				}
			}
			if (CompShuttle.tmpRequiredLabels.Any<string>())
			{
				stringBuilder.Append("Required".Translate() + ": " + CompShuttle.tmpRequiredLabels.ToCommaList(false).CapitalizeFirst());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060089B3 RID: 35251 RVA: 0x00283CCC File Offset: 0x00281ECC
		public void Send()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shuttle is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 8811221, false);
				return;
			}
			if (this.sending)
			{
				return;
			}
			if (!this.parent.Spawned)
			{
				Log.Error("Tried to send " + this.parent + ", but it's unspawned.", false);
				return;
			}
			List<CompTransporter> transportersInGroup = this.TransportersInGroup;
			if (transportersInGroup == null)
			{
				Log.Error("Tried to send " + this.parent + ", but it's not in any group.", false);
				return;
			}
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return;
			}
			if (!this.AllRequiredThingsLoaded)
			{
				if (this.dropEverythingIfUnsatisfied)
				{
					this.Transporter.CancelLoad();
				}
				else if (this.dropNonRequiredIfUnsatisfied)
				{
					for (int i = 0; i < transportersInGroup.Count; i++)
					{
						for (int j = transportersInGroup[i].innerContainer.Count - 1; j >= 0; j--)
						{
							Thing thing = transportersInGroup[i].innerContainer[j];
							Pawn pawn;
							if (!this.IsRequired(thing) && (this.requiredColonistCount <= 0 || (pawn = (thing as Pawn)) == null || !pawn.IsColonist))
							{
								Thing thing2;
								transportersInGroup[i].innerContainer.TryDrop(thing, ThingPlaceMode.Near, out thing2, null, null);
							}
						}
					}
				}
			}
			this.sending = true;
			Map map = this.parent.Map;
			this.Transporter.TryRemoveLord(map);
			this.SendLaunchedSignals(transportersInGroup);
			List<Pawn> list = new List<Pawn>();
			for (int k = 0; k < transportersInGroup.Count; k++)
			{
				CompTransporter compTransporter = transportersInGroup[k];
				for (int l = transportersInGroup[k].innerContainer.Count - 1; l >= 0; l--)
				{
					Pawn pawn2 = transportersInGroup[k].innerContainer[l] as Pawn;
					if (pawn2 != null)
					{
						if (pawn2.IsColonist && !this.requiredPawns.Contains(pawn2))
						{
							list.Add(pawn2);
						}
						pawn2.ExitMap(false, Rot4.Invalid);
					}
				}
				compTransporter.innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
				DropPodLeaving dropPodLeaving = (DropPodLeaving)ThingMaker.MakeThing(ThingDefOf.ShuttleLeaving, null);
				dropPodLeaving.createWorldObject = (this.permitShuttle && compTransporter.innerContainer.Any<Thing>());
				dropPodLeaving.worldObjectDef = WorldObjectDefOf.TravelingShuttle;
				compTransporter.CleanUpLoadingVars(map);
				compTransporter.parent.Destroy(DestroyMode.QuestLogic);
				GenSpawn.Spawn(dropPodLeaving, compTransporter.parent.Position, map, WipeMode.Vanish);
			}
			if (list.Count != 0)
			{
				for (int m = 0; m < transportersInGroup.Count; m++)
				{
					QuestUtility.SendQuestTargetSignals(transportersInGroup[m].parent.questTags, "SentWithExtraColonists", transportersInGroup[m].parent.Named("SUBJECT"), list.Named("SENTCOLONISTS"));
				}
			}
			this.sending = false;
		}

		// Token: 0x060089B4 RID: 35252 RVA: 0x00283FA0 File Offset: 0x002821A0
		public void SendLaunchedSignals(List<CompTransporter> transporters)
		{
			string signalPart = this.AllRequiredThingsLoaded ? "SentSatisfied" : "SentUnsatisfied";
			for (int i = 0; i < transporters.Count; i++)
			{
				QuestUtility.SendQuestTargetSignals(transporters[i].parent.questTags, signalPart, transporters[i].parent.Named("SUBJECT"), transporters[i].innerContainer.ToList<Thing>().Named("SENT"));
			}
		}

		// Token: 0x060089B5 RID: 35253 RVA: 0x0028401C File Offset: 0x0028221C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<ThingDefCount>(ref this.requiredItems, "requiredItems", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.requiredPawns, "requiredPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAllDespawned, "sendAwayIfAllDespawned", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.droppedOnArrival, "droppedOnArrival", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.requiredColonistCount, "requiredColonistCount", 0, false);
			Scribe_Values.Look<bool>(ref this.acceptColonists, "acceptColonists", false, false);
			Scribe_Values.Look<bool>(ref this.onlyAcceptColonists, "onlyAcceptColonists", false, false);
			Scribe_Values.Look<bool>(ref this.leaveImmediatelyWhenSatisfied, "leaveImmediatelyWhenSatisfied", false, false);
			Scribe_Values.Look<bool>(ref this.autoload, "autoload", false, false);
			Scribe_Values.Look<bool>(ref this.dropEverythingIfUnsatisfied, "dropEverythingIfUnsatisfied", false, false);
			Scribe_Values.Look<bool>(ref this.dropNonRequiredIfUnsatisfied, "dropNonRequiredIfUnsatisfied", false, false);
			Scribe_Values.Look<bool>(ref this.leaveASAP, "leaveASAP", false, false);
			Scribe_Values.Look<int>(ref this.leaveAfterTicks, "leaveAfterTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.dropEverythingOnArrival, "dropEverythingOnArrival", false, false);
			Scribe_Values.Look<bool>(ref this.stayAfterDroppedEverythingOnArrival, "stayAfterDroppedEverythingOnArrival", false, false);
			Scribe_Values.Look<bool>(ref this.permitShuttle, "permitShuttle", false, false);
			Scribe_References.Look<WorldObject>(ref this.missionShuttleTarget, "missionShuttleTarget", false);
			Scribe_References.Look<WorldObject>(ref this.missionShuttleHome, "missionShuttleHome", false);
			Scribe_Values.Look<bool>(ref this.hideControls, "hideControls", false, false);
			Scribe_Values.Look<int>(ref this.maxColonistCount, "maxColonistCount", -1, false);
			Scribe_References.Look<Faction>(ref this.sendAwayIfAllPawnsLeftToLoadAreNotOfFaction, "sendAwayIfAllPawnsLeftToLoadAreNotOfFaction", false);
			Scribe_References.Look<Quest>(ref this.sendAwayIfQuestFinished, "sendAwayIfQuestFinished", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.requiredPawns.RemoveAll((Pawn x) => x == null);
			}
			if (this.maxColonistCount == 0)
			{
				this.maxColonistCount = -1;
			}
		}

		// Token: 0x060089B6 RID: 35254 RVA: 0x0028420C File Offset: 0x0028240C
		private void CheckAutoload()
		{
			if (!this.autoload || !this.LoadingInProgressOrReadyToLaunch || !this.parent.Spawned)
			{
				return;
			}
			CompShuttle.tmpRequiredItems.Clear();
			CompShuttle.tmpRequiredItems.AddRange(this.requiredItems);
			CompShuttle.tmpRequiredPawns.Clear();
			CompShuttle.tmpRequiredPawns.AddRange(this.requiredPawns);
			ThingOwner innerContainer = this.Transporter.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Pawn pawn = innerContainer[i] as Pawn;
				if (pawn != null)
				{
					CompShuttle.tmpRequiredPawns.Remove(pawn);
				}
				else
				{
					int num = innerContainer[i].stackCount;
					for (int j = 0; j < CompShuttle.tmpRequiredItems.Count; j++)
					{
						if (CompShuttle.tmpRequiredItems[j].ThingDef == innerContainer[i].def)
						{
							int num2 = Mathf.Min(CompShuttle.tmpRequiredItems[j].Count, num);
							if (num2 > 0)
							{
								CompShuttle.tmpRequiredItems[j] = CompShuttle.tmpRequiredItems[j].WithCount(CompShuttle.tmpRequiredItems[j].Count - num2);
								num -= num2;
							}
						}
					}
				}
			}
			for (int k = CompShuttle.tmpRequiredItems.Count - 1; k >= 0; k--)
			{
				if (CompShuttle.tmpRequiredItems[k].Count <= 0)
				{
					CompShuttle.tmpRequiredItems.RemoveAt(k);
				}
			}
			if (CompShuttle.tmpRequiredItems.Any<ThingDefCount>() || CompShuttle.tmpRequiredPawns.Any<Pawn>())
			{
				if (this.Transporter.leftToLoad != null)
				{
					this.Transporter.leftToLoad.Clear();
				}
				CompShuttle.tmpAllSendablePawns.Clear();
				CompShuttle.tmpAllSendablePawns.AddRange(TransporterUtility.AllSendablePawns_NewTmp(this.TransportersInGroup, this.parent.Map, false));
				CompShuttle.tmpAllSendableItems.Clear();
				CompShuttle.tmpAllSendableItems.AddRange(TransporterUtility.AllSendableItems_NewTmp(this.TransportersInGroup, this.parent.Map, false));
				CompShuttle.tmpAllSendableItems.AddRange(TransporterUtility.ThingsBeingHauledTo(this.TransportersInGroup, this.parent.Map));
				CompShuttle.tmpRequiredPawnsPossibleToSend.Clear();
				for (int l = 0; l < CompShuttle.tmpRequiredPawns.Count; l++)
				{
					if (CompShuttle.tmpAllSendablePawns.Contains(CompShuttle.tmpRequiredPawns[l]))
					{
						TransferableOneWay transferableOneWay = new TransferableOneWay();
						transferableOneWay.things.Add(CompShuttle.tmpRequiredPawns[l]);
						this.Transporter.AddToTheToLoadList(transferableOneWay, 1);
						CompShuttle.tmpRequiredPawnsPossibleToSend.Add(CompShuttle.tmpRequiredPawns[l]);
					}
				}
				for (int m = 0; m < CompShuttle.tmpRequiredItems.Count; m++)
				{
					if (CompShuttle.tmpRequiredItems[m].Count > 0)
					{
						int num3 = 0;
						for (int n = 0; n < CompShuttle.tmpAllSendableItems.Count; n++)
						{
							if (CompShuttle.tmpAllSendableItems[n].def == CompShuttle.tmpRequiredItems[m].ThingDef)
							{
								num3 += CompShuttle.tmpAllSendableItems[n].stackCount;
							}
						}
						if (num3 > 0)
						{
							TransferableOneWay transferableOneWay2 = new TransferableOneWay();
							for (int num4 = 0; num4 < CompShuttle.tmpAllSendableItems.Count; num4++)
							{
								if (CompShuttle.tmpAllSendableItems[num4].def == CompShuttle.tmpRequiredItems[m].ThingDef)
								{
									transferableOneWay2.things.Add(CompShuttle.tmpAllSendableItems[num4]);
								}
							}
							int count = Mathf.Min(CompShuttle.tmpRequiredItems[m].Count, num3);
							this.Transporter.AddToTheToLoadList(transferableOneWay2, count);
						}
					}
				}
				TransporterUtility.MakeLordsAsAppropriate(CompShuttle.tmpRequiredPawnsPossibleToSend, this.TransportersInGroup, this.parent.Map);
				CompShuttle.tmpAllSendablePawns.Clear();
				CompShuttle.tmpAllSendableItems.Clear();
				CompShuttle.tmpRequiredItems.Clear();
				CompShuttle.tmpRequiredPawns.Clear();
				CompShuttle.tmpRequiredPawnsPossibleToSend.Clear();
				return;
			}
			if (this.Transporter.leftToLoad != null)
			{
				this.Transporter.leftToLoad.Clear();
			}
			TransporterUtility.MakeLordsAsAppropriate(CompShuttle.tmpRequiredPawnsPossibleToSend, this.TransportersInGroup, this.parent.Map);
		}

		// Token: 0x060089B7 RID: 35255 RVA: 0x00284678 File Offset: 0x00282878
		public bool IsRequired(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				return this.requiredPawns.Contains(pawn);
			}
			for (int i = 0; i < this.requiredItems.Count; i++)
			{
				if (this.requiredItems[i].ThingDef == thing.def && this.requiredItems[i].Count != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060089B8 RID: 35256 RVA: 0x002846E8 File Offset: 0x002828E8
		public bool IsAllowed(Thing t)
		{
			if (this.IsRequired(t))
			{
				return true;
			}
			if (this.acceptColonists)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null && (pawn.IsColonist || (!this.onlyAcceptColonists && pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer)) && (!this.onlyAcceptColonists || !pawn.IsQuestLodger()) && (!this.onlyAcceptHealthy || this.PawnIsHealthyEnoughForShuttle(pawn)))
				{
					return true;
				}
			}
			if (this.permitShuttle)
			{
				Pawn pawn2;
				if ((pawn2 = (t as Pawn)) == null)
				{
					return true;
				}
				if (pawn2.Faction == Faction.OfPlayer && !pawn2.IsQuestLodger())
				{
					return true;
				}
			}
			return this.IsMissionShuttle && !(t is Pawn) && this.CanAutoLoot;
		}

		// Token: 0x060089B9 RID: 35257 RVA: 0x002847A8 File Offset: 0x002829A8
		public bool IsAllowedNow(Thing t)
		{
			if (!this.IsAllowed(t))
			{
				return false;
			}
			if (this.maxColonistCount == -1)
			{
				return true;
			}
			int num = 0;
			foreach (Thing thing in ((IEnumerable<Thing>)this.Transporter.innerContainer))
			{
				if (thing != t)
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null && pawn.IsColonist)
					{
						num++;
					}
				}
			}
			foreach (Pawn pawn2 in this.parent.Map.mapPawns.AllPawns)
			{
				if (pawn2.jobs != null && pawn2.jobs.curDriver != null && pawn2 != t)
				{
					foreach (QueuedJob queuedJob in pawn2.jobs.jobQueue)
					{
						if (this.<IsAllowedNow>g__CheckJob|75_0(queuedJob.job))
						{
							num++;
						}
					}
					if (this.<IsAllowedNow>g__CheckJob|75_0(pawn2.jobs.curJob))
					{
						num++;
					}
				}
			}
			return num < this.maxColonistCount;
		}

		// Token: 0x060089BA RID: 35258 RVA: 0x00284908 File Offset: 0x00282B08
		private bool PawnIsHealthyEnoughForShuttle(Pawn p)
		{
			return !p.Downed && !p.InMentalState && p.health.capacities.CanBeAwake && p.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}

		// Token: 0x060089BB RID: 35259 RVA: 0x0005C788 File Offset: 0x0005A988
		public void CleanUpLoadingVars()
		{
			this.autoload = false;
		}

		// Token: 0x060089C4 RID: 35268 RVA: 0x00284AA4 File Offset: 0x00282CA4
		[CompilerGenerated]
		private bool <IsAllowedNow>g__CheckJob|75_0(Job job)
		{
			return typeof(JobDriver_EnterTransporter).IsAssignableFrom(job.def.driverClass) && job.GetTarget(TargetIndex.A).Thing == this.parent;
		}

		// Token: 0x04005831 RID: 22577
		public List<ThingDefCount> requiredItems = new List<ThingDefCount>();

		// Token: 0x04005832 RID: 22578
		public List<Pawn> requiredPawns = new List<Pawn>();

		// Token: 0x04005833 RID: 22579
		public List<Thing> sendAwayIfAllDespawned;

		// Token: 0x04005834 RID: 22580
		public Faction sendAwayIfAllPawnsLeftToLoadAreNotOfFaction;

		// Token: 0x04005835 RID: 22581
		public Quest sendAwayIfQuestFinished;

		// Token: 0x04005836 RID: 22582
		public int requiredColonistCount;

		// Token: 0x04005837 RID: 22583
		public int maxColonistCount = -1;

		// Token: 0x04005838 RID: 22584
		public bool acceptColonists;

		// Token: 0x04005839 RID: 22585
		public bool onlyAcceptColonists;

		// Token: 0x0400583A RID: 22586
		public bool onlyAcceptHealthy;

		// Token: 0x0400583B RID: 22587
		public bool dropEverythingIfUnsatisfied;

		// Token: 0x0400583C RID: 22588
		public bool dropNonRequiredIfUnsatisfied = true;

		// Token: 0x0400583D RID: 22589
		public bool leaveImmediatelyWhenSatisfied;

		// Token: 0x0400583E RID: 22590
		public bool dropEverythingOnArrival;

		// Token: 0x0400583F RID: 22591
		public bool stayAfterDroppedEverythingOnArrival;

		// Token: 0x04005840 RID: 22592
		public bool permitShuttle;

		// Token: 0x04005841 RID: 22593
		public WorldObject missionShuttleTarget;

		// Token: 0x04005842 RID: 22594
		public WorldObject missionShuttleHome;

		// Token: 0x04005843 RID: 22595
		public bool hideControls;

		// Token: 0x04005844 RID: 22596
		private bool autoload;

		// Token: 0x04005845 RID: 22597
		public bool leaveASAP;

		// Token: 0x04005846 RID: 22598
		public int leaveAfterTicks;

		// Token: 0x04005847 RID: 22599
		private List<Thing> droppedOnArrival = new List<Thing>();

		// Token: 0x04005848 RID: 22600
		private CompTransporter cachedCompTransporter;

		// Token: 0x04005849 RID: 22601
		private List<CompTransporter> cachedTransporterList;

		// Token: 0x0400584A RID: 22602
		private bool sending;

		// Token: 0x0400584B RID: 22603
		private const int CheckAutoloadIntervalTicks = 120;

		// Token: 0x0400584C RID: 22604
		private const int DropInterval = 60;

		// Token: 0x0400584D RID: 22605
		private static readonly Texture2D AutoloadToggleTex = ContentFinder<Texture2D>.Get("UI/Commands/Autoload", true);

		// Token: 0x0400584E RID: 22606
		private static readonly Texture2D SendCommandTex = CompLaunchable.LaunchCommandTex;

		// Token: 0x0400584F RID: 22607
		public static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment", true);

		// Token: 0x04005850 RID: 22608
		public static readonly IntVec3 DropoffSpotOffset = IntVec3.South * 2;

		// Token: 0x04005851 RID: 22609
		private static List<ThingDefCount> tmpRequiredItemsWithoutDuplicates = new List<ThingDefCount>();

		// Token: 0x04005852 RID: 22610
		private static List<Pawn> tmpAllowedPawns = new List<Pawn>();

		// Token: 0x04005853 RID: 22611
		private static List<string> tmpRequiredLabels = new List<string>();

		// Token: 0x04005854 RID: 22612
		private static List<ThingDefCount> tmpRequiredItems = new List<ThingDefCount>();

		// Token: 0x04005855 RID: 22613
		private static List<Pawn> tmpRequiredPawns = new List<Pawn>();

		// Token: 0x04005856 RID: 22614
		private static List<Pawn> tmpAllSendablePawns = new List<Pawn>();

		// Token: 0x04005857 RID: 22615
		private static List<Thing> tmpAllSendableItems = new List<Thing>();

		// Token: 0x04005858 RID: 22616
		private static List<Pawn> tmpRequiredPawnsPossibleToSend = new List<Pawn>();
	}
}
