using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D13 RID: 3347
	public class Zone_Stockpile : Zone, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x06004E4C RID: 20044 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x06004E4D RID: 20045 RVA: 0x0001276E File Offset: 0x0001096E
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x06004E4E RID: 20046 RVA: 0x001A43C3 File Offset: 0x001A25C3
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextStorageZoneColor();
			}
		}

		// Token: 0x06004E4F RID: 20047 RVA: 0x001A43CA File Offset: 0x001A25CA
		public Zone_Stockpile()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x001A43DE File Offset: 0x001A25DE
		public Zone_Stockpile(StorageSettingsPreset preset, ZoneManager zoneManager) : base(preset.PresetName(), zoneManager)
		{
			this.settings = new StorageSettings(this);
			this.settings.SetFromPreset(preset);
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x06004E51 RID: 20049 RVA: 0x001A4411 File Offset: 0x001A2611
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x06004E52 RID: 20050 RVA: 0x001A4433 File Offset: 0x001A2633
		public override void AddCell(IntVec3 sq)
		{
			base.AddCell(sq);
			if (this.slotGroup != null)
			{
				this.slotGroup.Notify_AddedCell(sq);
			}
		}

		// Token: 0x06004E53 RID: 20051 RVA: 0x001A4450 File Offset: 0x001A2650
		public override void RemoveCell(IntVec3 sq)
		{
			base.RemoveCell(sq);
			this.slotGroup.Notify_LostCell(sq);
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x001A4465 File Offset: 0x001A2665
		public override void PostDeregister()
		{
			base.PostDeregister();
			BillUtility.Notify_ZoneStockpileRemoved(this);
		}

		// Token: 0x06004E55 RID: 20053 RVA: 0x001A4473 File Offset: 0x001A2673
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return Zone_Stockpile.ITabs;
		}

		// Token: 0x06004E56 RID: 20054 RVA: 0x001A447A File Offset: 0x001A267A
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(this.settings))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06004E57 RID: 20055 RVA: 0x001A448A File Offset: 0x001A268A
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Expand>();
			yield break;
		}

		// Token: 0x06004E58 RID: 20056 RVA: 0x001A4493 File Offset: 0x001A2693
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x001A449B File Offset: 0x001A269B
		public IEnumerable<IntVec3> AllSlotCells()
		{
			int num;
			for (int i = 0; i < this.cells.Count; i = num + 1)
			{
				yield return this.cells[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x001A44AB File Offset: 0x001A26AB
		public List<IntVec3> AllSlotCellsList()
		{
			return this.cells;
		}

		// Token: 0x06004E5B RID: 20059 RVA: 0x001A44B3 File Offset: 0x001A26B3
		public StorageSettings GetParentStoreSettings()
		{
			return StorageSettings.EverStorableFixedSettings();
		}

		// Token: 0x06004E5C RID: 20060 RVA: 0x001A44BA File Offset: 0x001A26BA
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06004E5D RID: 20061 RVA: 0x001A44C2 File Offset: 0x001A26C2
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x0005A350 File Offset: 0x00058550
		public string SlotYielderLabel()
		{
			return this.label;
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x001A44D0 File Offset: 0x001A26D0
		public void Notify_ReceivedThing(Thing newItem)
		{
			if (newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x06004E60 RID: 20064 RVA: 0x0000313F File Offset: 0x0000133F
		public void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x04002F44 RID: 12100
		public StorageSettings settings;

		// Token: 0x04002F45 RID: 12101
		public SlotGroup slotGroup;

		// Token: 0x04002F46 RID: 12102
		private static readonly ITab[] ITabs = new ITab[]
		{
			new ITab_Storage()
		};
	}
}
