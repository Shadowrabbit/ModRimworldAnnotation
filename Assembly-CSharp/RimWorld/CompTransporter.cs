using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001883 RID: 6275
	[StaticConstructorOnStartup]
	public class CompTransporter : ThingComp, IThingHolder
	{
		// Token: 0x170015E4 RID: 5604
		// (get) Token: 0x06008B34 RID: 35636 RVA: 0x0005D62E File Offset: 0x0005B82E
		public CompProperties_Transporter Props
		{
			get
			{
				return (CompProperties_Transporter)this.props;
			}
		}

		// Token: 0x170015E5 RID: 5605
		// (get) Token: 0x06008B35 RID: 35637 RVA: 0x0005D63B File Offset: 0x0005B83B
		public Map Map
		{
			get
			{
				return this.parent.MapHeld;
			}
		}

		// Token: 0x170015E6 RID: 5606
		// (get) Token: 0x06008B36 RID: 35638 RVA: 0x0005D648 File Offset: 0x0005B848
		public bool AnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoad != null;
			}
		}

		// Token: 0x170015E7 RID: 5607
		// (get) Token: 0x06008B37 RID: 35639 RVA: 0x0005D653 File Offset: 0x0005B853
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.groupID >= 0;
			}
		}

		// Token: 0x170015E8 RID: 5608
		// (get) Token: 0x06008B38 RID: 35640 RVA: 0x0005D661 File Offset: 0x0005B861
		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoadInGroup != null;
			}
		}

		// Token: 0x170015E9 RID: 5609
		// (get) Token: 0x06008B39 RID: 35641 RVA: 0x0005D66C File Offset: 0x0005B86C
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

		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x06008B3A RID: 35642 RVA: 0x0005D68D File Offset: 0x0005B88D
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

		// Token: 0x170015EB RID: 5611
		// (get) Token: 0x06008B3B RID: 35643 RVA: 0x00288BD8 File Offset: 0x00286DD8
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

		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x06008B3C RID: 35644 RVA: 0x00288C40 File Offset: 0x00286E40
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

		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x06008B3D RID: 35645 RVA: 0x00288C84 File Offset: 0x00286E84
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

		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x06008B3E RID: 35646 RVA: 0x00288CC8 File Offset: 0x00286EC8
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
						if (compTransporter != null && allPawnsSpawned[j].CanReach(compTransporter.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
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

		// Token: 0x06008B3F RID: 35647 RVA: 0x0005D6AE File Offset: 0x0005B8AE
		public CompTransporter()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06008B40 RID: 35648 RVA: 0x00288E84 File Offset: 0x00287084
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Collections.Look<TransferableOneWay>(ref this.leftToLoad, "leftToLoad", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.notifiedCantLoadMore, "notifiedCantLoadMore", false, false);
		}

		// Token: 0x06008B41 RID: 35649 RVA: 0x0005D6C9 File Offset: 0x0005B8C9
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06008B42 RID: 35650 RVA: 0x0005D6D1 File Offset: 0x0005B8D1
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06008B43 RID: 35651 RVA: 0x00288EEC File Offset: 0x002870EC
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

		// Token: 0x06008B44 RID: 35652 RVA: 0x0005D6DF File Offset: 0x0005B8DF
		public List<CompTransporter> TransportersInGroup(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return null;
			}
			TransporterUtility.GetTransportersInGroup(this.groupID, map, CompTransporter.tmpTransportersInGroup);
			return CompTransporter.tmpTransportersInGroup;
		}

		// Token: 0x06008B45 RID: 35653 RVA: 0x0005D701 File Offset: 0x0005B901
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.Shuttle != null && !this.Shuttle.ShowLoadingGizmos && !this.Shuttle.permitShuttle)
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

		// Token: 0x06008B46 RID: 35654 RVA: 0x00289030 File Offset: 0x00287230
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.CancelLoad(map) && (this.Shuttle == null || this.Shuttle.permitShuttle))
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
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null);
		}

		// Token: 0x06008B47 RID: 35655 RVA: 0x0005D711 File Offset: 0x0005B911
		public override string CompInspectStringExtra()
		{
			return "Contents".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst();
		}

		// Token: 0x06008B48 RID: 35656 RVA: 0x002890C0 File Offset: 0x002872C0
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

		// Token: 0x06008B49 RID: 35657 RVA: 0x0028918C File Offset: 0x0028738C
		public void Notify_ThingAdded(Thing t)
		{
			this.SubtractFromToLoadList(t, t.stackCount, true);
			if (this.Props.pawnLoadedSound != null && t is Pawn)
			{
				this.Props.pawnLoadedSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
		}

		// Token: 0x06008B4A RID: 35658 RVA: 0x002891F0 File Offset: 0x002873F0
		public void Notify_ThingRemoved(Thing t)
		{
			if (this.Props.pawnExitSound != null && t is Pawn)
			{
				this.Props.pawnExitSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
		}

		// Token: 0x06008B4B RID: 35659 RVA: 0x0005D741 File Offset: 0x0005B941
		public void Notify_ThingAddedAndMergedWith(Thing t, int mergedCount)
		{
			this.SubtractFromToLoadList(t, mergedCount, true);
		}

		// Token: 0x06008B4C RID: 35660 RVA: 0x0005D74D File Offset: 0x0005B94D
		public bool CancelLoad()
		{
			return this.CancelLoad(this.Map);
		}

		// Token: 0x06008B4D RID: 35661 RVA: 0x00289244 File Offset: 0x00287444
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

		// Token: 0x06008B4E RID: 35662 RVA: 0x00289290 File Offset: 0x00287490
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

		// Token: 0x06008B4F RID: 35663 RVA: 0x002892C4 File Offset: 0x002874C4
		public void CleanUpLoadingVars(Map map)
		{
			this.groupID = -1;
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null);
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

		// Token: 0x06008B50 RID: 35664 RVA: 0x00289318 File Offset: 0x00287518
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

		// Token: 0x06008B51 RID: 35665 RVA: 0x002893F4 File Offset: 0x002875F4
		private void SelectPreviousInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect(list[GenMath.PositiveMod(num - 1, list.Count)].parent);
		}

		// Token: 0x06008B52 RID: 35666 RVA: 0x0028943C File Offset: 0x0028763C
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

		// Token: 0x06008B53 RID: 35667 RVA: 0x00289488 File Offset: 0x00287688
		private void SelectNextInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			int num = list.IndexOf(this);
			CameraJumper.TryJumpAndSelect(list[(num + 1) % list.Count].parent);
		}

		// Token: 0x04005939 RID: 22841
		public int groupID = -1;

		// Token: 0x0400593A RID: 22842
		public ThingOwner innerContainer;

		// Token: 0x0400593B RID: 22843
		public List<TransferableOneWay> leftToLoad;

		// Token: 0x0400593C RID: 22844
		private bool notifiedCantLoadMore;

		// Token: 0x0400593D RID: 22845
		private CompLaunchable cachedCompLaunchable;

		// Token: 0x0400593E RID: 22846
		private CompShuttle cachedCompShuttle;

		// Token: 0x0400593F RID: 22847
		public static readonly Texture2D CancelLoadCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04005940 RID: 22848
		private static readonly Texture2D LoadCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LoadTransporter", true);

		// Token: 0x04005941 RID: 22849
		private static readonly Texture2D SelectPreviousInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectPreviousTransporter", true);

		// Token: 0x04005942 RID: 22850
		private static readonly Texture2D SelectAllInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectAllTransporters", true);

		// Token: 0x04005943 RID: 22851
		private static readonly Texture2D SelectNextInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectNextTransporter", true);

		// Token: 0x04005944 RID: 22852
		private static List<CompTransporter> tmpTransportersInGroup = new List<CompTransporter>();
	}
}
