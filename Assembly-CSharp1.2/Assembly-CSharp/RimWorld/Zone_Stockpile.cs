using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001351 RID: 4945
	public class Zone_Stockpile : Zone, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x06006B54 RID: 27476 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x06006B55 RID: 27477 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x06006B56 RID: 27478 RVA: 0x000490E1 File Offset: 0x000472E1
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextStorageZoneColor();
			}
		}

		// Token: 0x06006B57 RID: 27479 RVA: 0x000490E8 File Offset: 0x000472E8
		public Zone_Stockpile()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x06006B58 RID: 27480 RVA: 0x000490FC File Offset: 0x000472FC
		public Zone_Stockpile(StorageSettingsPreset preset, ZoneManager zoneManager) : base(preset.PresetName(), zoneManager)
		{
			this.settings = new StorageSettings(this);
			this.settings.SetFromPreset(preset);
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x06006B59 RID: 27481 RVA: 0x0004912F File Offset: 0x0004732F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x06006B5A RID: 27482 RVA: 0x00049151 File Offset: 0x00047351
		public override void AddCell(IntVec3 sq)
		{
			base.AddCell(sq);
			if (this.slotGroup != null)
			{
				this.slotGroup.Notify_AddedCell(sq);
			}
		}

		// Token: 0x06006B5B RID: 27483 RVA: 0x0004916E File Offset: 0x0004736E
		public override void RemoveCell(IntVec3 sq)
		{
			base.RemoveCell(sq);
			this.slotGroup.Notify_LostCell(sq);
		}

		// Token: 0x06006B5C RID: 27484 RVA: 0x00049183 File Offset: 0x00047383
		public override void PostDeregister()
		{
			base.PostDeregister();
			BillUtility.Notify_ZoneStockpileRemoved(this);
		}

		// Token: 0x06006B5D RID: 27485 RVA: 0x00049191 File Offset: 0x00047391
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return Zone_Stockpile.ITabs;
		}

		// Token: 0x06006B5E RID: 27486 RVA: 0x00049198 File Offset: 0x00047398
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

		// Token: 0x06006B5F RID: 27487 RVA: 0x000491A8 File Offset: 0x000473A8
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Expand>();
			yield break;
		}

		// Token: 0x06006B60 RID: 27488 RVA: 0x000491B1 File Offset: 0x000473B1
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x06006B61 RID: 27489 RVA: 0x000491B9 File Offset: 0x000473B9
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

		// Token: 0x06006B62 RID: 27490 RVA: 0x000491C9 File Offset: 0x000473C9
		public List<IntVec3> AllSlotCellsList()
		{
			return this.cells;
		}

		// Token: 0x06006B63 RID: 27491 RVA: 0x0000C32E File Offset: 0x0000A52E
		public StorageSettings GetParentStoreSettings()
		{
			return null;
		}

		// Token: 0x06006B64 RID: 27492 RVA: 0x000491D1 File Offset: 0x000473D1
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06006B65 RID: 27493 RVA: 0x000491D9 File Offset: 0x000473D9
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x06006B66 RID: 27494 RVA: 0x00014AE8 File Offset: 0x00012CE8
		public string SlotYielderLabel()
		{
			return this.label;
		}

		// Token: 0x06006B67 RID: 27495 RVA: 0x000491E7 File Offset: 0x000473E7
		public void Notify_ReceivedThing(Thing newItem)
		{
			if (newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x06006B68 RID: 27496 RVA: 0x00006A05 File Offset: 0x00004C05
		public void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x04004770 RID: 18288
		public StorageSettings settings;

		// Token: 0x04004771 RID: 18289
		public SlotGroup slotGroup;

		// Token: 0x04004772 RID: 18290
		private static readonly ITab[] ITabs = new ITab[]
		{
			new ITab_Storage()
		};
	}
}
