using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001062 RID: 4194
	public class StorageSettings : IExposable
	{
		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x06006369 RID: 25449 RVA: 0x0021990B File Offset: 0x00217B0B
		private IHaulDestination HaulDestinationOwner
		{
			get
			{
				return this.owner as IHaulDestination;
			}
		}

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x0600636A RID: 25450 RVA: 0x00219918 File Offset: 0x00217B18
		private ISlotGroupParent SlotGroupParentOwner
		{
			get
			{
				return this.owner as ISlotGroupParent;
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x0600636B RID: 25451 RVA: 0x00219925 File Offset: 0x00217B25
		// (set) Token: 0x0600636C RID: 25452 RVA: 0x00219930 File Offset: 0x00217B30
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

		// Token: 0x0600636D RID: 25453 RVA: 0x002199B3 File Offset: 0x00217BB3
		public StorageSettings()
		{
			this.filter = new ThingFilter(new Action(this.TryNotifyChanged));
		}

		// Token: 0x0600636E RID: 25454 RVA: 0x002199DC File Offset: 0x00217BDC
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

		// Token: 0x0600636F RID: 25455 RVA: 0x00219A0F File Offset: 0x00217C0F
		public void ExposeData()
		{
			Scribe_Values.Look<StoragePriority>(ref this.priorityInt, "priority", StoragePriority.Unstored, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", new object[]
			{
				new Action(this.TryNotifyChanged)
			});
		}

		// Token: 0x06006370 RID: 25456 RVA: 0x00219A48 File Offset: 0x00217C48
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			this.filter.SetFromPreset(preset);
			this.TryNotifyChanged();
		}

		// Token: 0x06006371 RID: 25457 RVA: 0x00219A5C File Offset: 0x00217C5C
		public void CopyFrom(StorageSettings other)
		{
			this.Priority = other.Priority;
			this.filter.CopyAllowancesFrom(other.filter);
			this.TryNotifyChanged();
		}

		// Token: 0x06006372 RID: 25458 RVA: 0x00219A84 File Offset: 0x00217C84
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

		// Token: 0x06006373 RID: 25459 RVA: 0x00219AC4 File Offset: 0x00217CC4
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

		// Token: 0x06006374 RID: 25460 RVA: 0x00219B04 File Offset: 0x00217D04
		private void TryNotifyChanged()
		{
			if (this.owner != null && this.SlotGroupParentOwner != null && this.SlotGroupParentOwner.GetSlotGroup() != null && this.SlotGroupParentOwner.Map != null)
			{
				this.SlotGroupParentOwner.Map.listerHaulables.Notify_SlotGroupChanged(this.SlotGroupParentOwner.GetSlotGroup());
			}
		}

		// Token: 0x06006375 RID: 25461 RVA: 0x00219B5B File Offset: 0x00217D5B
		public static StorageSettings EverStorableFixedSettings()
		{
			if (StorageSettings.cachedEverStorableFixedSettings == null)
			{
				StorageSettings.cachedEverStorableFixedSettings = new StorageSettings(null)
				{
					filter = ThingFilter.CreateOnlyEverStorableThingFilter()
				};
			}
			return StorageSettings.cachedEverStorableFixedSettings;
		}

		// Token: 0x06006376 RID: 25462 RVA: 0x00219B7F File Offset: 0x00217D7F
		public static void ResetStaticData()
		{
			StorageSettings.cachedEverStorableFixedSettings = null;
		}

		// Token: 0x04003852 RID: 14418
		private static StorageSettings cachedEverStorableFixedSettings;

		// Token: 0x04003853 RID: 14419
		public IStoreSettingsParent owner;

		// Token: 0x04003854 RID: 14420
		public ThingFilter filter;

		// Token: 0x04003855 RID: 14421
		[LoadAlias("priority")]
		private StoragePriority priorityInt = StoragePriority.Normal;
	}
}
