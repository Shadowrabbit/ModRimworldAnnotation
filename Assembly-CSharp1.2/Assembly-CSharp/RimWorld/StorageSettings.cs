using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200169A RID: 5786
	public class StorageSettings : IExposable
	{
		// Token: 0x17001394 RID: 5012
		// (get) Token: 0x06007E8E RID: 32398 RVA: 0x000550A7 File Offset: 0x000532A7
		private IHaulDestination HaulDestinationOwner
		{
			get
			{
				return this.owner as IHaulDestination;
			}
		}

		// Token: 0x17001395 RID: 5013
		// (get) Token: 0x06007E8F RID: 32399 RVA: 0x000550B4 File Offset: 0x000532B4
		private ISlotGroupParent SlotGroupParentOwner
		{
			get
			{
				return this.owner as ISlotGroupParent;
			}
		}

		// Token: 0x17001396 RID: 5014
		// (get) Token: 0x06007E90 RID: 32400 RVA: 0x000550C1 File Offset: 0x000532C1
		// (set) Token: 0x06007E91 RID: 32401 RVA: 0x0025A124 File Offset: 0x00258324
		public StoragePriority Priority
		{
			get
			{
				return this.priorityInt;
			}
			set
			{
				this.priorityInt = value;
				if (Current.ProgramState == ProgramState.Playing && this.HaulDestinationOwner != null && this.HaulDestinationOwner.Map != null)
				{
					this.HaulDestinationOwner.Map.haulDestinationManager.Notify_HaulDestinationChangedPriority();
				}
				if (Current.ProgramState == ProgramState.Playing && this.SlotGroupParentOwner != null && this.SlotGroupParentOwner.Map != null)
				{
					this.SlotGroupParentOwner.Map.listerHaulables.RecalcAllInCells(this.SlotGroupParentOwner.AllSlotCells());
				}
			}
		}

		// Token: 0x06007E92 RID: 32402 RVA: 0x000550C9 File Offset: 0x000532C9
		public StorageSettings()
		{
			this.filter = new ThingFilter(new Action(this.TryNotifyChanged));
		}

		// Token: 0x06007E93 RID: 32403 RVA: 0x0025A1A8 File Offset: 0x002583A8
		public StorageSettings(IStoreSettingsParent owner) : this()
		{
			this.owner = owner;
			if (owner != null)
			{
				StorageSettings parentStoreSettings = owner.GetParentStoreSettings();
				if (parentStoreSettings != null)
				{
					this.priorityInt = parentStoreSettings.priorityInt;
				}
			}
		}

		// Token: 0x06007E94 RID: 32404 RVA: 0x000550EF File Offset: 0x000532EF
		public void ExposeData()
		{
			Scribe_Values.Look<StoragePriority>(ref this.priorityInt, "priority", StoragePriority.Unstored, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", new object[]
			{
				new Action(this.TryNotifyChanged)
			});
		}

		// Token: 0x06007E95 RID: 32405 RVA: 0x00055128 File Offset: 0x00053328
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			this.filter.SetFromPreset(preset);
			this.TryNotifyChanged();
		}

		// Token: 0x06007E96 RID: 32406 RVA: 0x0005513C File Offset: 0x0005333C
		public void CopyFrom(StorageSettings other)
		{
			this.Priority = other.Priority;
			this.filter.CopyAllowancesFrom(other.filter);
			this.TryNotifyChanged();
		}

		// Token: 0x06007E97 RID: 32407 RVA: 0x0025A1DC File Offset: 0x002583DC
		public bool AllowedToAccept(Thing t)
		{
			if (!this.filter.Allows(t))
			{
				return false;
			}
			if (this.owner != null)
			{
				StorageSettings parentStoreSettings = this.owner.GetParentStoreSettings();
				if (parentStoreSettings != null && !parentStoreSettings.AllowedToAccept(t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007E98 RID: 32408 RVA: 0x0025A21C File Offset: 0x0025841C
		public bool AllowedToAccept(ThingDef t)
		{
			if (!this.filter.Allows(t))
			{
				return false;
			}
			if (this.owner != null)
			{
				StorageSettings parentStoreSettings = this.owner.GetParentStoreSettings();
				if (parentStoreSettings != null && !parentStoreSettings.AllowedToAccept(t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007E99 RID: 32409 RVA: 0x0025A25C File Offset: 0x0025845C
		private void TryNotifyChanged()
		{
			if (this.owner != null && this.SlotGroupParentOwner != null && this.SlotGroupParentOwner.GetSlotGroup() != null && this.SlotGroupParentOwner.Map != null)
			{
				this.SlotGroupParentOwner.Map.listerHaulables.Notify_SlotGroupChanged(this.SlotGroupParentOwner.GetSlotGroup());
			}
		}

		// Token: 0x0400525C RID: 21084
		public IStoreSettingsParent owner;

		// Token: 0x0400525D RID: 21085
		public ThingFilter filter;

		// Token: 0x0400525E RID: 21086
		[LoadAlias("priority")]
		private StoragePriority priorityInt = StoragePriority.Normal;
	}
}
