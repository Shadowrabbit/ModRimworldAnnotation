using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CE RID: 4302
	public class ActiveDropPodInfo : IThingHolder, IExposable
	{
		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x060066E9 RID: 26345 RVA: 0x0022C1CE File Offset: 0x0022A3CE
		// (set) Token: 0x060066EA RID: 26346 RVA: 0x0022C203 File Offset: 0x0022A403
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
					Log.Error("ContainedThing used on a DropPodInfo holding > 1 thing.");
				}
				return this.innerContainer[0];
			}
			set
			{
				this.innerContainer.Clear();
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x170011AB RID: 4523
		// (get) Token: 0x060066EB RID: 26347 RVA: 0x0022C21E File Offset: 0x0022A41E
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x060066EC RID: 26348 RVA: 0x0022C226 File Offset: 0x0022A426
		public ActiveDropPodInfo()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x060066ED RID: 26349 RVA: 0x0022C258 File Offset: 0x0022A458
		public ActiveDropPodInfo(IThingHolder parent)
		{
			this.innerContainer = new ThingOwner<Thing>(this);
			this.parent = parent;
		}

		// Token: 0x060066EE RID: 26350 RVA: 0x0022C294 File Offset: 0x0022A494
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

		// Token: 0x060066EF RID: 26351 RVA: 0x0022C45B File Offset: 0x0022A65B
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060066F0 RID: 26352 RVA: 0x0022C463 File Offset: 0x0022A663
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04003A15 RID: 14869
		public IThingHolder parent;

		// Token: 0x04003A16 RID: 14870
		public ThingOwner innerContainer;

		// Token: 0x04003A17 RID: 14871
		public int openDelay = 110;

		// Token: 0x04003A18 RID: 14872
		public bool leaveSlag;

		// Token: 0x04003A19 RID: 14873
		public bool savePawnsWithReferenceMode;

		// Token: 0x04003A1A RID: 14874
		public bool despawnPodBeforeSpawningThing;

		// Token: 0x04003A1B RID: 14875
		public WipeMode? spawnWipeMode;

		// Token: 0x04003A1C RID: 14876
		public Rot4? setRotation;

		// Token: 0x04003A1D RID: 14877
		public bool moveItemsAsideBeforeSpawning;

		// Token: 0x04003A1E RID: 14878
		public List<string> questTags;

		// Token: 0x04003A1F RID: 14879
		public const int DefaultOpenDelay = 110;

		// Token: 0x04003A20 RID: 14880
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04003A21 RID: 14881
		private List<Pawn> tmpSavedPawns = new List<Pawn>();
	}
}
