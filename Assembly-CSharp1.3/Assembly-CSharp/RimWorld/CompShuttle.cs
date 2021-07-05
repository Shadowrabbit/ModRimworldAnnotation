using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001197 RID: 4503
	[StaticConstructorOnStartup]
	public class CompShuttle : ThingComp
	{
		// Token: 0x170012BE RID: 4798
		// (get) Token: 0x06006C57 RID: 27735 RVA: 0x00245508 File Offset: 0x00243708
		public bool Autoload
		{
			get
			{
				return this.autoload;
			}
		}

		// Token: 0x170012BF RID: 4799
		// (get) Token: 0x06006C58 RID: 27736 RVA: 0x00245510 File Offset: 0x00243710
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.Transporter.LoadingInProgressOrReadyToLaunch;
			}
		}

		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x06006C59 RID: 27737 RVA: 0x0024551D File Offset: 0x0024371D
		public List<CompTransporter> TransportersInGroup
		{
			get
			{
				return this.Transporter.TransportersInGroup(this.parent.Map);
			}
		}

		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x06006C5A RID: 27738 RVA: 0x00245535 File Offset: 0x00243735
		public bool CanAutoLoot
		{
			get
			{
				return !this.parent.Map.IsPlayerHome && !GenHostility.AnyHostileActiveThreatToPlayer(this.parent.Map, true, true);
			}
		}

		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x06006C5B RID: 27739 RVA: 0x00245560 File Offset: 0x00243760
		public bool ShowLoadingGizmos
		{
			get
			{
				return (this.shipParent == null || this.shipParent.ShowGizmos) && (this.parent.Faction == null || this.parent.Faction == Faction.OfPlayer);
			}
		}

		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x06006C5C RID: 27740 RVA: 0x0024559A File Offset: 0x0024379A
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

		// Token: 0x170012C4 RID: 4804
		// (get) Token: 0x06006C5D RID: 27741 RVA: 0x002455BC File Offset: 0x002437BC
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

		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x06006C5E RID: 27742 RVA: 0x00245608 File Offset: 0x00243808
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
				foreach (Pawn thing in TransporterUtility.AllSendablePawns(this.cachedTransporterList, this.parent.Map, false))
				{
					if (!this.IsRequired(thing))
					{
						return false;
					}
				}
				foreach (Thing thing2 in TransporterUtility.AllSendableItems(this.cachedTransporterList, this.parent.Map, false))
				{
					if (!this.IsRequired(thing2))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x06006C5F RID: 27743 RVA: 0x002456E4 File Offset: 0x002438E4
		private int ContainedColonistCount
		{
			get
			{
				int num = 0;
				ThingOwner innerContainer = this.Transporter.innerContainer;
				for (int i = 0; i < innerContainer.Count; i++)
				{
					Pawn pawn = innerContainer[i] as Pawn;
					if (pawn != null && pawn.IsFreeColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x06006C60 RID: 27744 RVA: 0x00245730 File Offset: 0x00243930
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
				if (this.requireAllColonistsOnMap || this.requiredColonistCount > 0)
				{
					int containedColonistCount = this.ContainedColonistCount;
					if (this.requireAllColonistsOnMap && containedColonistCount < this.parent.Map.mapPawns.FreeColonistsCount)
					{
						return false;
					}
					if (this.requiredColonistCount > 0 && containedColonistCount < this.requiredColonistCount)
					{
						return false;
					}
				}
				CompShuttle.tmpRequiredItemsWithoutDuplicates.Clear();
				for (int j = 0; j < this.requiredItems.Count; j++)
				{
					bool flag = false;
					for (int k = 0; k < CompShuttle.tmpRequiredItemsWithoutDuplicates.Count; k++)
					{
						if (CompShuttle.tmpRequiredItemsWithoutDuplicates[k].ThingDef == this.requiredItems[j].ThingDef)
						{
							CompShuttle.tmpRequiredItemsWithoutDuplicates[k] = CompShuttle.tmpRequiredItemsWithoutDuplicates[k].WithCount(CompShuttle.tmpRequiredItemsWithoutDuplicates[k].Count + this.requiredItems[j].Count);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						CompShuttle.tmpRequiredItemsWithoutDuplicates.Add(this.requiredItems[j]);
					}
				}
				for (int l = 0; l < CompShuttle.tmpRequiredItemsWithoutDuplicates.Count; l++)
				{
					int num = 0;
					for (int m = 0; m < innerContainer.Count; m++)
					{
						if (innerContainer[m].def == CompShuttle.tmpRequiredItemsWithoutDuplicates[l].ThingDef)
						{
							num += innerContainer[m].stackCount;
						}
					}
					if (num < CompShuttle.tmpRequiredItemsWithoutDuplicates[l].Count)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x06006C61 RID: 27745 RVA: 0x00245938 File Offset: 0x00243B38
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

		// Token: 0x06006C62 RID: 27746 RVA: 0x002459EA File Offset: 0x00243BEA
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckRoyaltyOrIdeology("Shuttle"))
			{
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06006C63 RID: 27747 RVA: 0x00245A00 File Offset: 0x00243C00
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.shipParent != null && this.shipParent.ShowGizmos)
			{
				IEnumerable<Gizmo> jobGizmos = this.shipParent.curJob.GetJobGizmos();
				if (jobGizmos != null)
				{
					foreach (Gizmo gizmo2 in jobGizmos)
					{
						yield return gizmo2;
					}
					enumerator = null;
				}
			}
			if (this.ShowLoadingGizmos && this.Autoloadable)
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
			foreach (Gizmo gizmo3 in QuestUtility.GetQuestRelatedGizmos(this.parent))
			{
				yield return gizmo3;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006C64 RID: 27748 RVA: 0x00245A10 File Offset: 0x00243C10
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!selPawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				yield break;
			}
			string text = "EnterShuttle".Translate();
			if (!this.IsAllowedNow(selPawn))
			{
				yield return new FloatMenuOption(text + " (" + "NotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			yield return new FloatMenuOption(text, delegate()
			{
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
				}
				Job job = JobMaker.MakeJob(JobDefOf.EnterTransporter, this.parent);
				selPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield break;
		}

		// Token: 0x06006C65 RID: 27749 RVA: 0x00245A27 File Offset: 0x00243C27
		public override IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(List<Pawn> selPawns)
		{
			CompShuttle.tmpAllowedPawns.Clear();
			string text = "EnterShuttle".Translate();
			for (int i = 0; i < selPawns.Count; i++)
			{
				if (selPawns[i].CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					CompShuttle.tmpAllowedPawns.Add(selPawns[i]);
				}
			}
			if (!CompShuttle.tmpAllowedPawns.Any<Pawn>())
			{
				yield return new FloatMenuOption(text + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
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
				yield return new FloatMenuOption(text + " (" + "NotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
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
					if (pawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && !pawn.Downed && !pawn.Dead && pawn.Spawned)
					{
						Job job = JobMaker.MakeJob(JobDefOf.EnterTransporter, this.parent);
						CompShuttle.tmpAllowedPawns[k].jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					}
				}
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield break;
		}

		// Token: 0x06006C66 RID: 27750 RVA: 0x00245A3E File Offset: 0x00243C3E
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(120))
			{
				this.CheckAutoload();
			}
		}

		// Token: 0x06006C67 RID: 27751 RVA: 0x00245A5C File Offset: 0x00243C5C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.shipParent != null && this.shipParent.curJob != null)
			{
				string text = this.shipParent.curJob.ShipThingExtraInspectString();
				if (!text.NullOrEmpty())
				{
					stringBuilder.Append(text);
				}
			}
			CompShuttle.tmpRequiredLabels.Clear();
			if (this.requireAllColonistsOnMap)
			{
				int freeColonistsCount = this.parent.Map.mapPawns.FreeColonistsCount;
				CompShuttle.tmpRequiredLabels.Add(freeColonistsCount + " " + ((freeColonistsCount > 1) ? Faction.OfPlayer.def.pawnsPlural : Faction.OfPlayer.def.pawnSingular));
			}
			else if (this.requiredColonistCount > 0)
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
				stringBuilder.AppendInNewLine("Required".Translate() + ": " + CompShuttle.tmpRequiredLabels.ToCommaList(false, false).CapitalizeFirst());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006C68 RID: 27752 RVA: 0x00245C80 File Offset: 0x00243E80
		public void SendLaunchedSignals()
		{
			List<CompTransporter> transportersInGroup = this.TransportersInGroup;
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < transportersInGroup.Count; i++)
			{
				for (int j = 0; j < transportersInGroup[i].innerContainer.Count; j++)
				{
					Pawn pawn;
					if ((pawn = (transportersInGroup[i].innerContainer[j] as Pawn)) != null && pawn.IsColonist && !this.requiredPawns.Contains(pawn))
					{
						list.Add(pawn);
					}
				}
			}
			if (list.Count != 0)
			{
				for (int k = 0; k < transportersInGroup.Count; k++)
				{
					QuestUtility.SendQuestTargetSignals(transportersInGroup[k].parent.questTags, "SentWithExtraColonists", transportersInGroup[k].parent.Named("SUBJECT"), list.Named("SENTCOLONISTS"));
				}
			}
			string signalPart = this.AllRequiredThingsLoaded ? "SentSatisfied" : "SentUnsatisfied";
			for (int l = 0; l < transportersInGroup.Count; l++)
			{
				QuestUtility.SendQuestTargetSignals(transportersInGroup[l].parent.questTags, signalPart, transportersInGroup[l].parent.Named("SUBJECT"), transportersInGroup[l].innerContainer.ToList<Thing>().Named("SENT"));
			}
		}

		// Token: 0x06006C69 RID: 27753 RVA: 0x00245DD8 File Offset: 0x00243FD8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<ThingDefCount>(ref this.requiredItems, "requiredItems", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.requiredPawns, "requiredPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.requiredColonistCount, "requiredColonistCount", 0, false);
			Scribe_Values.Look<bool>(ref this.requireAllColonistsOnMap, "requireAllColonistsOnMap", false, false);
			Scribe_Values.Look<bool>(ref this.acceptColonists, "acceptColonists", false, false);
			Scribe_Values.Look<bool>(ref this.onlyAcceptColonists, "onlyAcceptColonists", false, false);
			Scribe_Values.Look<bool>(ref this.allowSlaves, "allowSlaves", false, false);
			Scribe_Values.Look<bool>(ref this.autoload, "autoload", false, false);
			Scribe_Values.Look<bool>(ref this.permitShuttle, "permitShuttle", false, false);
			Scribe_Values.Look<int>(ref this.maxColonistCount, "maxColonistCount", -1, false);
			Scribe_References.Look<TransportShip>(ref this.shipParent, "shipParent", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.requiredPawns.RemoveAll((Pawn x) => x == null);
			}
			if (this.maxColonistCount == 0)
			{
				this.maxColonistCount = -1;
			}
		}

		// Token: 0x06006C6A RID: 27754 RVA: 0x00245EFC File Offset: 0x002440FC
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
				CompShuttle.tmpAllSendablePawns.AddRange(TransporterUtility.AllSendablePawns(this.TransportersInGroup, this.parent.Map, false));
				CompShuttle.tmpAllSendableItems.Clear();
				CompShuttle.tmpAllSendableItems.AddRange(TransporterUtility.AllSendableItems(this.TransportersInGroup, this.parent.Map, false));
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

		// Token: 0x06006C6B RID: 27755 RVA: 0x00246368 File Offset: 0x00244568
		public virtual bool IsRequired(Thing thing)
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

		// Token: 0x06006C6C RID: 27756 RVA: 0x002463D8 File Offset: 0x002445D8
		public virtual bool IsAllowed(Thing t)
		{
			if (this.IsRequired(t))
			{
				return true;
			}
			if (this.acceptColonists)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null && (pawn.IsColonist || (!this.onlyAcceptColonists && pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer)) && (!pawn.IsSlave || this.allowSlaves) && (!this.onlyAcceptColonists || !pawn.IsQuestLodger()) && (!this.onlyAcceptHealthy || this.PawnIsHealthyEnoughForShuttle(pawn)))
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
			return !(t is Pawn) && this.CanAutoLoot;
		}

		// Token: 0x06006C6D RID: 27757 RVA: 0x002464A0 File Offset: 0x002446A0
		public virtual bool IsAllowedNow(Thing t)
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
						if (this.<IsAllowedNow>g__CheckJob|57_0(queuedJob.job))
						{
							num++;
						}
					}
					if (this.<IsAllowedNow>g__CheckJob|57_0(pawn2.jobs.curJob))
					{
						num++;
					}
				}
			}
			return num < this.maxColonistCount;
		}

		// Token: 0x06006C6E RID: 27758 RVA: 0x00246600 File Offset: 0x00244800
		private bool PawnIsHealthyEnoughForShuttle(Pawn p)
		{
			return !p.Downed && !p.InMentalState && p.health.capacities.CanBeAwake && p.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}

		// Token: 0x06006C6F RID: 27759 RVA: 0x00246660 File Offset: 0x00244860
		public virtual void CleanUpLoadingVars()
		{
			this.autoload = false;
		}

		// Token: 0x06006C76 RID: 27766 RVA: 0x002467E8 File Offset: 0x002449E8
		[CompilerGenerated]
		private bool <IsAllowedNow>g__CheckJob|57_0(Job job)
		{
			return typeof(JobDriver_EnterTransporter).IsAssignableFrom(job.def.driverClass) && job.GetTarget(TargetIndex.A).Thing == this.parent;
		}

		// Token: 0x04003C2E RID: 15406
		public List<ThingDefCount> requiredItems = new List<ThingDefCount>();

		// Token: 0x04003C2F RID: 15407
		public List<Pawn> requiredPawns = new List<Pawn>();

		// Token: 0x04003C30 RID: 15408
		public int requiredColonistCount;

		// Token: 0x04003C31 RID: 15409
		public bool requireAllColonistsOnMap;

		// Token: 0x04003C32 RID: 15410
		public int maxColonistCount = -1;

		// Token: 0x04003C33 RID: 15411
		public bool acceptColonists;

		// Token: 0x04003C34 RID: 15412
		public bool onlyAcceptColonists;

		// Token: 0x04003C35 RID: 15413
		public bool onlyAcceptHealthy;

		// Token: 0x04003C36 RID: 15414
		public bool permitShuttle;

		// Token: 0x04003C37 RID: 15415
		public bool allowSlaves;

		// Token: 0x04003C38 RID: 15416
		private bool autoload;

		// Token: 0x04003C39 RID: 15417
		public TransportShip shipParent;

		// Token: 0x04003C3A RID: 15418
		private CompTransporter cachedCompTransporter;

		// Token: 0x04003C3B RID: 15419
		private List<CompTransporter> cachedTransporterList;

		// Token: 0x04003C3C RID: 15420
		private const int CheckAutoloadIntervalTicks = 120;

		// Token: 0x04003C3D RID: 15421
		private static readonly Texture2D AutoloadToggleTex = ContentFinder<Texture2D>.Get("UI/Commands/Autoload", true);

		// Token: 0x04003C3E RID: 15422
		private static List<ThingDefCount> tmpRequiredItemsWithoutDuplicates = new List<ThingDefCount>();

		// Token: 0x04003C3F RID: 15423
		private static List<Pawn> tmpAllowedPawns = new List<Pawn>();

		// Token: 0x04003C40 RID: 15424
		private static List<string> tmpRequiredLabels = new List<string>();

		// Token: 0x04003C41 RID: 15425
		private static List<ThingDefCount> tmpRequiredItems = new List<ThingDefCount>();

		// Token: 0x04003C42 RID: 15426
		private static List<Pawn> tmpRequiredPawns = new List<Pawn>();

		// Token: 0x04003C43 RID: 15427
		private static List<Pawn> tmpAllSendablePawns = new List<Pawn>();

		// Token: 0x04003C44 RID: 15428
		private static List<Thing> tmpAllSendableItems = new List<Thing>();

		// Token: 0x04003C45 RID: 15429
		private static List<Pawn> tmpRequiredPawnsPossibleToSend = new List<Pawn>();
	}
}
