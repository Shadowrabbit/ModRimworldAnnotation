using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001059 RID: 4185
	public class Building_Storage : Building, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x0600632A RID: 25386 RVA: 0x0021917E File Offset: 0x0021737E
		public Building_Storage()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x0600632B RID: 25387 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x0600632C RID: 25388 RVA: 0x00219192 File Offset: 0x00217392
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return this.def.building.ignoreStoredThingsBeauty;
			}
		}

		// Token: 0x0600632D RID: 25389 RVA: 0x002191A4 File Offset: 0x002173A4
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x0600632E RID: 25390 RVA: 0x002191AC File Offset: 0x002173AC
		public virtual void Notify_ReceivedThing(Thing newItem)
		{
			if (base.Faction == Faction.OfPlayer && newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x0600632F RID: 25391 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x06006330 RID: 25392 RVA: 0x002191D9 File Offset: 0x002173D9
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

		// Token: 0x06006331 RID: 25393 RVA: 0x002191E9 File Offset: 0x002173E9
		public List<IntVec3> AllSlotCellsList()
		{
			if (this.cachedOccupiedCells == null)
			{
				this.cachedOccupiedCells = this.AllSlotCells().ToList<IntVec3>();
			}
			return this.cachedOccupiedCells;
		}

		// Token: 0x06006332 RID: 25394 RVA: 0x0021920A File Offset: 0x0021740A
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06006333 RID: 25395 RVA: 0x00219214 File Offset: 0x00217414
		public StorageSettings GetParentStoreSettings()
		{
			StorageSettings fixedStorageSettings = this.def.building.fixedStorageSettings;
			if (fixedStorageSettings != null)
			{
				return fixedStorageSettings;
			}
			return StorageSettings.EverStorableFixedSettings();
		}

		// Token: 0x06006334 RID: 25396 RVA: 0x00029737 File Offset: 0x00027937
		public string SlotYielderLabel()
		{
			return this.LabelCap;
		}

		// Token: 0x06006335 RID: 25397 RVA: 0x0021923C File Offset: 0x0021743C
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x06006336 RID: 25398 RVA: 0x0021924C File Offset: 0x0021744C
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x06006337 RID: 25399 RVA: 0x00219298 File Offset: 0x00217498
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			this.cachedOccupiedCells = null;
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x06006338 RID: 25400 RVA: 0x002192A9 File Offset: 0x002174A9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x06006339 RID: 25401 RVA: 0x002192CB File Offset: 0x002174CB
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

		// Token: 0x04003841 RID: 14401
		public StorageSettings settings;

		// Token: 0x04003842 RID: 14402
		public SlotGroup slotGroup;

		// Token: 0x04003843 RID: 14403
		private List<IntVec3> cachedOccupiedCells;
	}
}
