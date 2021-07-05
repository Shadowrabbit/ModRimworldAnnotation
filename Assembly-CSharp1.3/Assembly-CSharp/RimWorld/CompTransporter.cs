using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020011C4 RID: 4548
	[StaticConstructorOnStartup]
	public class CompTransporter : ThingComp, IThingHolder
	{
		// Token: 0x17001300 RID: 4864
		// (get) Token: 0x06006D84 RID: 28036 RVA: 0x0024AE56 File Offset: 0x00249056
		public CompProperties_Transporter Props
		{
			get
			{
				return (CompProperties_Transporter)this.props;
			}
		}

		// Token: 0x17001301 RID: 4865
		// (get) Token: 0x06006D85 RID: 28037 RVA: 0x0024AE63 File Offset: 0x00249063
		public Map Map
		{
			get
			{
				return this.parent.MapHeld;
			}
		}

		// Token: 0x17001302 RID: 4866
		// (get) Token: 0x06006D86 RID: 28038 RVA: 0x0024AE70 File Offset: 0x00249070
		public bool AnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoad != null;
			}
		}

		// Token: 0x17001303 RID: 4867
		// (get) Token: 0x06006D87 RID: 28039 RVA: 0x0024AE7B File Offset: 0x0024907B
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.groupID >= 0;
			}
		}

		// Token: 0x17001304 RID: 4868
		// (get) Token: 0x06006D88 RID: 28040 RVA: 0x0024AE89 File Offset: 0x00249089
		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoadInGroup != null;
			}
		}

		// Token: 0x17001305 RID: 4869
		// (get) Token: 0x06006D89 RID: 28041 RVA: 0x0024AE94 File Offset: 0x00249094
		public CompLaunchable Launchable
		{
			get
			{
				if (this.cachedCompLaunchable == null)
				{
					this.cachedCompLaunchable = this.parent.GetComp<CompLaunchable>();
				}
				return this.cachedCompLaunchable;
			}
		}

		// Token: 0x17001306 RID: 4870
		// (get) Token: 0x06006D8A RID: 28042 RVA: 0x0024AEB5 File Offset: 0x002490B5
		public CompShuttle Shuttle
		{
			get
			{
				if (this.cachedCompShuttle == null)
				{
					this.cachedCompShuttle = this.parent.GetComp<CompShuttle>();
				}
				return this.cachedCompShuttle;
			}
		}

		// Token: 0x17001307 RID: 4871
		// (get) Token: 0x06006D8B RID: 28043 RVA: 0x0024AED8 File Offset: 0x002490D8
		public Thing FirstThingLeftToLoad
		{
			get
			{
				if (this.leftToLoad == null)
				{
					return null;
				}
				for (int i = 0; i < this.leftToLoad.Count; i++)
				{
					if (this.leftToLoad[i].CountToTransfer != 0 && this.leftToLoad[i].HasAnyThing)
					{
						return this.leftToLoad[i].AnyThing;
					}
				}
				return null;
			}
		}

		// Token: 0x17001308 RID: 4872
		// (get) Token: 0x06006D8C RID: 28044 RVA: 0x0024AF40 File Offset: 0x00249140
		public Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				for (int i = 0; i < list.Count; i++)
				{
					Thing firstThingLeftToLoad = list[i].FirstThingLeftToLoad;
					if (firstThingLeftToLoad != null)
					{
						return firstThingLeftToLoad;
					}
				}
				return null;
			}
		}

		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x06006D8D RID: 28045 RVA: 0x0024AF84 File Offset: 0x00249184
		public bool AnyInGroupNotifiedCantLoadMore
		{
			get
			{
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].notifiedCantLoadMore)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x06006D8E RID: 28046 RVA: 0x0024AFC8 File Offset: 0x002491C8
		public bool AnyPawnCanLoadAnythingNow
		{
			get
			{
				if (!this.AnythingLeftToLoad)
				{
					return false;
				}
				if (!this.parent.Spawned)
				{
					return false;
				}
				List<Pawn> allPawnsSpawned = this.parent.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
					{
						CompTransporter transporter = ((JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter;
						if (transporter != null && transporter.groupID == this.groupID)
						{
							return true;
						}
					}
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.EnterTransporter)
					{
						CompTransporter transporter2 = ((JobDriver_EnterTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter;
						if (transporter2 != null && transporter2.groupID == this.groupID)
						{
							return true;
						}
					}
				}
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				for (int j = 0; j < allPawnsSpawned.Count; j++)
				{
					if (allPawnsSpawned[j].mindState.duty != null && allPawnsSpawned[j].mindState.duty.transportersGroup == this.groupID)
					{
						CompTransporter compTransporter = JobGiver_EnterTransporter.FindMyTransporter(list, allPawnsSpawned[j]);
						if (compTransporter != null && allPawnsSpawned[j].CanReach(compTransporter.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							return true;
						}
					}
				}
				for (int k = 0; k < allPawnsSpawned.Count; k++)
				{
					if (allPawnsSpawned[k].IsColonist)
					{
						for (int l = 0; l < list.Count; l++)
						{
							if (LoadTransportersJobUtility.HasJobOnTransporter(allPawnsSpawned[k], list[l]))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06006D8F RID: 28047 RVA: 0x0024B185 File Offset: 0x00249385
		public CompTransporter()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06006D90 RID: 28048 RVA: 0x0024B1B8 File Offset: 0x002493B8
		public override void PostExposeData()
		{
			base.PostExposeData();
			bool flag = !this.parent.SpawnedOrAnyParentSpawned;
			if (flag && Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpThings.Clear();
				this.tmpThings.AddRange(this.innerContainer);
				this.tmpSavedPawns.Clear();
				for (int i = 0; i < this.tmpThings.Count; i++)
				{
					Pawn pawn;
					if ((pawn = (this.tmpThings[i] as Pawn)) != null)
					{
						this.innerContainer.Remove(pawn);
						this.tmpSavedPawns.Add(pawn);
						if (!pawn.IsWorldPawn())
						{
							Log.Error("Trying to save a non-world pawn (" + pawn + ") as a reference in a transporter.");
						}
					}
				}
				this.tmpThings.Clear();
			}
			Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Collections.Look<TransferableOneWay>(ref this.leftToLoad, "leftToLoad", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.notifiedCantLoadMore, "notifiedCantLoadMore", false, false);
			if (flag && (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving))
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.innerContainer.TryAdd(this.tmpSavedPawns[j], true);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		// Token: 0x06006D91 RID: 28049 RVA: 0x0024B335 File Offset: 0x00249535
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06006D92 RID: 28050 RVA: 0x0024B33D File Offset: 0x0024953D
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06006D93 RID: 28051 RVA: 0x0024B34C File Offset: 0x0024954C
		public override void CompTick()
		{
			base.CompTick();
			this.innerContainer.ThingOwnerTick(true);
			if (this.Props.restEffectiveness != 0f)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Pawn pawn = this.innerContainer[i] as Pawn;
					if (pawn != null && !pawn.Dead && pawn.needs.rest != null)
					{
						pawn.needs.rest.TickResting(this.Props.restEffectiveness);
					}
				}
			}
			if (this.parent.IsHashIntervalTick(60) && this.parent.Spawned && this.LoadingInProgressOrReadyToLaunch && this.AnyInGroupHasAnythingLeftToLoad && !this.AnyInGroupNotifiedCantLoadMore && !this.AnyPawnCanLoadAnythingNow && (this.Shuttle == null || !this.Shuttle.Autoload))
			{
				this.notifiedCantLoadMore = true;
				Messages.Message("MessageCantLoadMoreIntoTransporters".Translate(this.FirstThingLeftToLoadInGroup.LabelNoCount, Faction.OfPlayer.def.pawnsPlural, this.FirstThingLeftToLoadInGroup), this.parent, MessageTypeDefOf.CautionInput, true);
			}
		}

		// Token: 0x06006D94 RID: 28052 RVA: 0x0024B490 File Offset: 0x00249690
		public List<CompTransporter> TransportersInGroup(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return null;
			}
			TransporterUtility.GetTransportersInGroup(this.groupID, map, CompTransporter.tmpTransportersInGroup);
			return CompTransporter.tmpTransportersInGroup;
		}

		// Token: 0x06006D95 RID: 28053 RVA: 0x0024B4B2 File Offset: 0x002496B2
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.Shuttle != null && !this.Shuttle.ShowLoadingGizmos)
			{
				yield break;
			}
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				if (this.Shuttle == null || !this.Shuttle.Autoload)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandCancelLoad".Translate(),
						defaultDesc = "CommandCancelLoadDesc".Translate(),
						icon = CompTransporter.CancelLoadCommandTex,
						action = delegate()
						{
							SoundDefOf.Designate_Cancel.PlayOneShotOnCamera(null);
							this.CancelLoad();
						}
					};
				}
				if (!this.Props.max1PerGroup)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandSelectPreviousTransporter".Translate(),
						defaultDesc = "CommandSelectPreviousTransporterDesc".Translate(),
						icon = CompTransporter.SelectPreviousInGroupCommandTex,
						action = delegate()
						{
							this.SelectPreviousInGroup();
						}
					};
					yield return new Command_Action
					{
						defaultLabel = "CommandSelectAllTransporters".Translate(),
						defaultDesc = "CommandSelectAllTransportersDesc".Translate(),
						icon = CompTransporter.SelectAllInGroupCommandTex,
						action = delegate()
						{
							this.SelectAllInGroup();
						}
					};
					yield return new Command_Action
					{
						defaultLabel = "CommandSelectNextTransporter".Translate(),
						defaultDesc = "CommandSelectNextTransporterDesc".Translate(),
						icon = CompTransporter.SelectNextInGroupCommandTex,
						action = delegate()
						{
							this.SelectNextInGroup();
						}
					};
				}
				if (this.Props.canChangeAssignedThingsAfterStarting && (this.Shuttle == null || !this.Shuttle.Autoload))
				{
					yield return new Command_LoadToTransporter
					{
						defaultLabel = "CommandSetToLoadTransporter".Translate(),
						defaultDesc = "CommandSetToLoadTransporterDesc".Translate(),
						icon = CompTransporter.LoadCommandTex,
						transComp = this
					};
				}
			}
			else
			{
				Command_LoadToTransporter command_LoadToTransporter = new Command_LoadToTransporter();
				if (this.Props.max1PerGroup)
				{
					if (this.Props.canChangeAssignedThingsAfterStarting)
					{
						command_LoadToTransporter.defaultLabel = "CommandSetToLoadTransporter".Translate();
						command_LoadToTransporter.defaultDesc = "CommandSetToLoadTransporterDesc".Translate();
					}
					else
					{
						command_LoadToTransporter.defaultLabel = "CommandLoadTransporterSingle".Translate();
						command_LoadToTransporter.defaultDesc = "CommandLoadTransporterSingleDesc".Translate();
					}
				}
				else
				{
					int num = 0;
					for (int i = 0; i < Find.Selector.NumSelected; i++)
					{
						Thing thing = Find.Selector.SelectedObjectsListForReading[i] as Thing;
						if (thing != null && thing.def == this.parent.def)
						{
							CompLaunchable compLaunchable = thing.TryGetComp<CompLaunchable>();
							if (compLaunchable == null || (compLaunchable.FuelingPortSource != null && compLaunchable.FuelingPortSourceHasAnyFuel))
							{
								num++;
							}
						}
					}
					command_LoadToTransporter.defaultLabel = "CommandLoadTransporter".Translate(num.ToString());
					command_LoadToTransporter.defaultDesc = "CommandLoadTransporterDesc".Translate();
				}
				command_LoadToTransporter.icon = CompTransporter.LoadCommandTex;
				command_LoadToTransporter.transComp = this;
				CompLaunchable launchable = this.Launchable;
				if (launchable != null)
				{
					if (!launchable.ConnectedToFuelingPort)
					{
						command_LoadToTransporter.Disable("CommandLoadTransporterFailNotConnectedToFuelingPort".Translate());
					}
					else if (!launchable.FuelingPortSourceHasAnyFuel)
					{
						command_LoadToTransporter.Disable("CommandLoadTransporterFailNoFuel".Translate());
					}
				}
				yield return command_LoadToTransporter;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006D96 RID: 28054 RVA: 0x0024B4C4 File Offset: 0x002496C4
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.CancelLoad(map) && this.Shuttle == null)
			{
				if (this.Props.max1PerGroup)
				{
					Messages.Message("MessageTransporterSingleLoadCanceled_TransporterDestroyed".Translate(), MessageTypeDefOf.NegativeEvent, true);
				}
				else
				{
					Messages.Message("MessageTransportersLoadCanceled_TransporterDestroyed".Translate(), MessageTypeDefOf.NegativeEvent, true);
				}
			}
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null, true);
		}

		// Token: 0x06006D97 RID: 28055 RVA: 0x0024B548 File Offset: 0x00249748
		public override string CompInspectStringExtra()
		{
			return "Contents".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst();
		}

		// Token: 0x06006D98 RID: 28056 RVA: 0x0024B578 File Offset: 0x00249778
		public void AddToTheToLoadList(TransferableOneWay t, int count)
		{
			if (!t.HasAnyThing || count <= 0)
			{
				return;
			}
			if (this.leftToLoad == null)
			{
				this.leftToLoad = new List<TransferableOneWay>();
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t.AnyThing, this.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay != null)
			{
				for (int i = 0; i < t.things.Count; i++)
				{
					if (!transferableOneWay.things.Contains(t.things[i]))
					{
						transferableOneWay.things.Add(t.things[i]);
					}
				}
				if (transferableOneWay.CanAdjustBy(count).Accepted)
				{
					transferableOneWay.AdjustBy(count);
					return;
				}
			}
			else
			{
				TransferableOneWay transferableOneWay2 = new TransferableOneWay();
				this.leftToLoad.Add(transferableOneWay2);
				transferableOneWay2.things.AddRange(t.things);
				transferableOneWay2.AdjustTo(count);
			}
		}

		// Token: 0x06006D99 RID: 28057 RVA: 0x0024B644 File Offset: 0x00249844
		public bool LeftToLoadContains(Thing thing)
		{
			if (this.leftToLoad == null)
			{
				return false;
			}
			for (int i = 0; i < this.leftToLoad.Count; i++)
			{
				for (int j = 0; j < this.leftToLoad[i].things.Count; j++)
				{
					if (this.leftToLoad[i].things[j] == thing)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006D9A RID: 28058 RVA: 0x0024B6B0 File Offset: 0x002498B0
		public void Notify_ThingAdded(Thing t)
		{
			this.SubtractFromToLoadList(t, t.stackCount, true);
			if (this.parent.Spawned && this.Props.pawnLoadedSound != null && t is Pawn)
			{
				this.Props.pawnLoadedSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "ThingAdded", t.Named("SUBJECT"));
		}

		// Token: 0x06006D9B RID: 28059 RVA: 0x0024B740 File Offset: 0x00249940
		public void Notify_ThingRemoved(Thing t)
		{
			if (this.Props.pawnExitSound != null && t is Pawn)
			{
				this.Props.pawnExitSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
		}

		// Token: 0x06006D9C RID: 28060 RVA: 0x0024B793 File Offset: 0x00249993
		public void Notify_ThingAddedAndMergedWith(Thing t, int mergedCount)
		{
			this.SubtractFromToLoadList(t, mergedCount, true);
		}

		// Token: 0x06006D9D RID: 28061 RVA: 0x0024B7A0 File Offset: 0x002499A0
		public bool CancelLoad()
		{
			CompShuttle shuttle = this.Shuttle;
			if (shuttle == null)
			{
				return this.CancelLoad(this.Map);
			}
			if (shuttle.shipParent != null)
			{
				shuttle.shipParent.ForceJob_DelayCurrent(ShipJobMaker.MakeShipJob(ShipJobDefOf.Unload));
				return true;
			}
			return this.CancelLoad(this.Map);
		}

		// Token: 0x06006D9E RID: 28062 RVA: 0x0024B7F0 File Offset: 0x002499F0
		public bool CancelLoad(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return false;
			}
			this.TryRemoveLord(map);
			List<CompTransporter> list = this.TransportersInGroup(map);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].CleanUpLoadingVars(map);
			}
			this.CleanUpLoadingVars(map);
			return true;
		}

		// Token: 0x06006D9F RID: 28063 RVA: 0x0024B83C File Offset: 0x00249A3C
		public void TryRemoveLord(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return;
			}
			Lord lord = TransporterUtility.FindLord(this.groupID, map);
			if (lord != null)
			{
				map.lordManager.RemoveLord(lord);
			}
		}

		// Token: 0x06006DA0 RID: 28064 RVA: 0x0024B870 File Offset: 0x00249A70
		public void CleanUpLoadingVars(Map map)
		{
			this.groupID = -1;
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null, true);
			if (this.leftToLoad != null)
			{
				this.leftToLoad.Clear();
			}
			CompShuttle shuttle = this.Shuttle;
			if (shuttle != null)
			{
				shuttle.CleanUpLoadingVars();
			}
		}

		// Token: 0x06006DA1 RID: 28065 RVA: 0x0024B8C4 File Offset: 0x00249AC4
		public int SubtractFromToLoadList(Thing t, int count, bool sendMessageOnFinished = true)
		{
			if (this.leftToLoad == null)
			{
				return 0;
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(t, this.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay == null)
			{
				return 0;
			}
			if (transferableOneWay.CountToTransfer <= 0)
			{
				return 0;
			}
			int num = Mathf.Min(count, transferableOneWay.CountToTransfer);
			transferableOneWay.AdjustBy(-num);
			if (transferableOneWay.CountToTransfer <= 0)
			{
				this.leftToLoad.Remove(transferableOneWay);
			}
			if (sendMessageOnFinished && !this.AnyInGroupHasAnythingLeftToLoad)
			{
				CompShuttle comp = this.parent.GetComp<CompShuttle>();
				if (comp == null || comp.AllRequiredThingsLoaded)
				{
					if (this.Props.max1PerGroup)
					{
						Messages.Message("MessageFinishedLoadingTransporterSingle".Translate(), this.parent, MessageTypeDefOf.TaskCompletion, true);
					}
					else
					{
						Messages.Message("MessageFinishedLoadingTransporters".Translate(), this.parent, MessageTypeDefOf.TaskCompletion, true);
					}
				}
			}
			return num;
		}

		// Token: 0x06006DA2 RID: 28066 RVA: 0x0024B9A0 File Offset: 0x00249BA0
		private void SelectPreviousInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect(list[GenMath.PositiveMod(num - 1, list.Count)].parent);
		}

		// Token: 0x06006DA3 RID: 28067 RVA: 0x0024B9E8 File Offset: 0x00249BE8
		private void SelectAllInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			Selector selector = Find.Selector;
			selector.ClearSelection();
			for (int i = 0; i < list.Count; i++)
			{
				selector.Select(list[i].parent, true, true);
			}
		}

		// Token: 0x06006DA4 RID: 28068 RVA: 0x0024BA34 File Offset: 0x00249C34
		private void SelectNextInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect(list[(num + 1) % list.Count].parent);
		}

		// Token: 0x04003CD6 RID: 15574
		public int groupID = -1;

		// Token: 0x04003CD7 RID: 15575
		public ThingOwner innerContainer;

		// Token: 0x04003CD8 RID: 15576
		public List<TransferableOneWay> leftToLoad;

		// Token: 0x04003CD9 RID: 15577
		private bool notifiedCantLoadMore;

		// Token: 0x04003CDA RID: 15578
		private CompLaunchable cachedCompLaunchable;

		// Token: 0x04003CDB RID: 15579
		private CompShuttle cachedCompShuttle;

		// Token: 0x04003CDC RID: 15580
		public static readonly Texture2D CancelLoadCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04003CDD RID: 15581
		private static readonly Texture2D LoadCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LoadTransporter", true);

		// Token: 0x04003CDE RID: 15582
		private static readonly Texture2D SelectPreviousInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectPreviousTransporter", true);

		// Token: 0x04003CDF RID: 15583
		private static readonly Texture2D SelectAllInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectAllTransporters", true);

		// Token: 0x04003CE0 RID: 15584
		private static readonly Texture2D SelectNextInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectNextTransporter", true);

		// Token: 0x04003CE1 RID: 15585
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04003CE2 RID: 15586
		private List<Pawn> tmpSavedPawns = new List<Pawn>();

		// Token: 0x04003CE3 RID: 15587
		private static List<CompTransporter> tmpTransportersInGroup = new List<CompTransporter>();
	}
}
