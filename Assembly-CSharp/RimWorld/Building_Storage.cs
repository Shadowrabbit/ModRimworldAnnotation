using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200168A RID: 5770
	public class Building_Storage : Building, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x06007E1C RID: 32284 RVA: 0x00054CA4 File Offset: 0x00052EA4
		public Building_Storage()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x06007E1D RID: 32285 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001377 RID: 4983
		// (get) Token: 0x06007E1E RID: 32286 RVA: 0x00054CB8 File Offset: 0x00052EB8
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return this.def.building.ignoreStoredThingsBeauty;
			}
		}

		// Token: 0x06007E1F RID: 32287 RVA: 0x00054CCA File Offset: 0x00052ECA
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x06007E20 RID: 32288 RVA: 0x00054CD2 File Offset: 0x00052ED2
		public virtual void Notify_ReceivedThing(Thing newItem)
		{
			if (base.Faction == Faction.OfPlayer && newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x06007E21 RID: 32289 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x06007E22 RID: 32290 RVA: 0x00054CFF File Offset: 0x00052EFF
		public virtual IEnumerable<IntVec3> AllSlotCells()
		{
			foreach (IntVec3 intVec in GenAdj.CellsOccupiedBy(this))
			{
				yield return intVec;
			}
			IEnumerator<IntVec3> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007E23 RID: 32291 RVA: 0x00054D0F File Offset: 0x00052F0F
		public List<IntVec3> AllSlotCellsList()
		{
			if (this.cachedOccupiedCells == null)
			{
				this.cachedOccupiedCells = this.AllSlotCells().ToList<IntVec3>();
			}
			return this.cachedOccupiedCells;
		}

		// Token: 0x06007E24 RID: 32292 RVA: 0x00054D30 File Offset: 0x00052F30
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06007E25 RID: 32293 RVA: 0x00054D38 File Offset: 0x00052F38
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x06007E26 RID: 32294 RVA: 0x0000FC1E File Offset: 0x0000DE1E
		public string SlotYielderLabel()
		{
			return this.LabelCap;
		}

		// Token: 0x06007E27 RID: 32295 RVA: 0x00054D4A File Offset: 0x00052F4A
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x06007E28 RID: 32296 RVA: 0x00259548 File Offset: 0x00257748
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x06007E29 RID: 32297 RVA: 0x00054D58 File Offset: 0x00052F58
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			this.cachedOccupiedCells = null;
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x06007E2A RID: 32298 RVA: 0x00054D69 File Offset: 0x00052F69
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x06007E2B RID: 32299 RVA: 0x00054D8B File Offset: 0x00052F8B
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

		// Token: 0x04005225 RID: 21029
		public StorageSettings settings;

		// Token: 0x04005226 RID: 21030
		public SlotGroup slotGroup;

		// Token: 0x04005227 RID: 21031
		private List<IntVec3> cachedOccupiedCells;
	}
}
