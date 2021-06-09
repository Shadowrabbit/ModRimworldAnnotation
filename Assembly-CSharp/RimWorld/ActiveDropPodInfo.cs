using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001733 RID: 5939
	public class ActiveDropPodInfo : IThingHolder, IExposable
	{
		// Token: 0x17001460 RID: 5216
		// (get) Token: 0x060082FA RID: 33530 RVA: 0x00057EBB File Offset: 0x000560BB
		// (set) Token: 0x060082FB RID: 33531 RVA: 0x00057EF1 File Offset: 0x000560F1
		public Thing SingleContainedThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				if (this.innerContainer.Count > 1)
				{
					Log.Error("ContainedThing used on a DropPodInfo holding > 1 thing.", false);
				}
				return this.innerContainer[0];
			}
			set
			{
				this.innerContainer.Clear();
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x17001461 RID: 5217
		// (get) Token: 0x060082FC RID: 33532 RVA: 0x00057F0C File Offset: 0x0005610C
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x060082FD RID: 33533 RVA: 0x00057F14 File Offset: 0x00056114
		public ActiveDropPodInfo()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x060082FE RID: 33534 RVA: 0x00057F46 File Offset: 0x00056146
		public ActiveDropPodInfo(IThingHolder parent)
		{
			this.innerContainer = new ThingOwner<Thing>(this);
			this.parent = parent;
		}

		// Token: 0x060082FF RID: 33535 RVA: 0x0026D364 File Offset: 0x0026B564
		public void ExposeData()
		{
			if (this.savePawnsWithReferenceMode && Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpThings.Clear();
				this.tmpThings.AddRange(this.innerContainer);
				this.tmpSavedPawns.Clear();
				for (int i = 0; i < this.tmpThings.Count; i++)
				{
					Pawn pawn = this.tmpThings[i] as Pawn;
					if (pawn != null)
					{
						this.innerContainer.Remove(pawn);
						this.tmpSavedPawns.Add(pawn);
					}
				}
				this.tmpThings.Clear();
			}
			Scribe_Values.Look<bool>(ref this.savePawnsWithReferenceMode, "savePawnsWithReferenceMode", false, false);
			if (this.savePawnsWithReferenceMode)
			{
				Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, Array.Empty<object>());
			}
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.openDelay, "openDelay", 110, false);
			Scribe_Values.Look<bool>(ref this.leaveSlag, "leaveSlag", false, false);
			Scribe_Values.Look<WipeMode?>(ref this.spawnWipeMode, "spawnWipeMode", null, false);
			Scribe_Values.Look<bool>(ref this.despawnPodBeforeSpawningThing, "despawnPodBeforeSpawningThing", false, false);
			Scribe_Values.Look<Rot4?>(ref this.setRotation, "setRotation", null, false);
			Scribe_Values.Look<bool>(ref this.moveItemsAsideBeforeSpawning, "moveItemsAsideBeforeSpawning", false, false);
			Scribe_References.Look<WorldObject>(ref this.missionShuttleTarget, "missionShuttleTarget", false);
			Scribe_References.Look<WorldObject>(ref this.missionShuttleHome, "missionShuttleHome", false);
			Scribe_References.Look<Quest>(ref this.sendAwayIfQuestFinished, "sendAwayIfQuestFinished", false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			if (this.savePawnsWithReferenceMode && (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving))
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.innerContainer.TryAdd(this.tmpSavedPawns[j], true);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		// Token: 0x06008300 RID: 33536 RVA: 0x00057F7F File Offset: 0x0005617F
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06008301 RID: 33537 RVA: 0x00057F87 File Offset: 0x00056187
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x040054E4 RID: 21732
		public IThingHolder parent;

		// Token: 0x040054E5 RID: 21733
		public ThingOwner innerContainer;

		// Token: 0x040054E6 RID: 21734
		public int openDelay = 110;

		// Token: 0x040054E7 RID: 21735
		public bool leaveSlag;

		// Token: 0x040054E8 RID: 21736
		public bool savePawnsWithReferenceMode;

		// Token: 0x040054E9 RID: 21737
		public bool despawnPodBeforeSpawningThing;

		// Token: 0x040054EA RID: 21738
		public WipeMode? spawnWipeMode;

		// Token: 0x040054EB RID: 21739
		public Rot4? setRotation;

		// Token: 0x040054EC RID: 21740
		public bool moveItemsAsideBeforeSpawning;

		// Token: 0x040054ED RID: 21741
		public WorldObject missionShuttleTarget;

		// Token: 0x040054EE RID: 21742
		public WorldObject missionShuttleHome;

		// Token: 0x040054EF RID: 21743
		public Quest sendAwayIfQuestFinished;

		// Token: 0x040054F0 RID: 21744
		public List<string> questTags;

		// Token: 0x040054F1 RID: 21745
		public const int DefaultOpenDelay = 110;

		// Token: 0x040054F2 RID: 21746
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x040054F3 RID: 21747
		private List<Pawn> tmpSavedPawns = new List<Pawn>();
	}
}
