using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3A RID: 3642
	public class Pawn_InventoryStockTracker : IExposable
	{
		// Token: 0x06005451 RID: 21585 RVA: 0x001C96A1 File Offset: 0x001C78A1
		public Pawn_InventoryStockTracker()
		{
		}

		// Token: 0x06005452 RID: 21586 RVA: 0x001C96B4 File Offset: 0x001C78B4
		public Pawn_InventoryStockTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005453 RID: 21587 RVA: 0x001C96CE File Offset: 0x001C78CE
		public InventoryStockEntry GetCurrentEntryFor(InventoryStockGroupDef group)
		{
			if (!this.stockEntries.ContainsKey(group))
			{
				this.stockEntries[group] = this.CreateDefaultEntryFor(group);
			}
			return this.stockEntries[group];
		}

		// Token: 0x06005454 RID: 21588 RVA: 0x001C96FD File Offset: 0x001C78FD
		public int GetDesiredCountForGroup(InventoryStockGroupDef group)
		{
			return this.GetCurrentEntryFor(group).count;
		}

		// Token: 0x06005455 RID: 21589 RVA: 0x001C970B File Offset: 0x001C790B
		public ThingDef GetDesiredThingForGroup(InventoryStockGroupDef group)
		{
			return this.GetCurrentEntryFor(group).thingDef;
		}

		// Token: 0x06005456 RID: 21590 RVA: 0x001C971C File Offset: 0x001C791C
		public void SetCountForGroup(InventoryStockGroupDef group, int count)
		{
			InventoryStockEntry currentEntryFor = this.GetCurrentEntryFor(group);
			int count2 = Mathf.Clamp(count, group.min, group.max);
			currentEntryFor.count = count2;
		}

		// Token: 0x06005457 RID: 21591 RVA: 0x001C974C File Offset: 0x001C794C
		public void SetThingForGroup(InventoryStockGroupDef group, ThingDef thing)
		{
			if (!group.thingDefs.Contains(thing))
			{
				Log.Error(string.Concat(new string[]
				{
					"Inventory stock group ",
					group.defName,
					" does not contain ",
					thing.defName,
					"."
				}));
				return;
			}
			this.GetCurrentEntryFor(group).thingDef = thing;
		}

		// Token: 0x06005458 RID: 21592 RVA: 0x001C97AF File Offset: 0x001C79AF
		private InventoryStockEntry CreateDefaultEntryFor(InventoryStockGroupDef group)
		{
			return new InventoryStockEntry
			{
				count = group.min,
				thingDef = group.DefaultThingDef
			};
		}

		// Token: 0x06005459 RID: 21593 RVA: 0x001C97D0 File Offset: 0x001C79D0
		public bool AnyThingsRequiredNow()
		{
			foreach (KeyValuePair<InventoryStockGroupDef, InventoryStockEntry> keyValuePair in this.stockEntries)
			{
				if (this.pawn.inventory.Count(keyValuePair.Value.thingDef) < keyValuePair.Value.count)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600545A RID: 21594 RVA: 0x001C9850 File Offset: 0x001C7A50
		public void ExposeData()
		{
			Scribe_Collections.Look<InventoryStockGroupDef, InventoryStockEntry>(ref this.stockEntries, "stockEntries", LookMode.Def, LookMode.Deep, ref this.tmpInventoryStockGroups, ref this.tmpInventoryStock);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				foreach (InventoryStockGroupDef inventoryStockGroupDef in DefDatabase<InventoryStockGroupDef>.AllDefs)
				{
					InventoryStockEntry currentEntryFor = this.GetCurrentEntryFor(inventoryStockGroupDef);
					if (!inventoryStockGroupDef.thingDefs.Contains(currentEntryFor.thingDef))
					{
						currentEntryFor.thingDef = inventoryStockGroupDef.DefaultThingDef;
					}
				}
			}
		}

		// Token: 0x040031B3 RID: 12723
		public Pawn pawn;

		// Token: 0x040031B4 RID: 12724
		public Dictionary<InventoryStockGroupDef, InventoryStockEntry> stockEntries = new Dictionary<InventoryStockGroupDef, InventoryStockEntry>();

		// Token: 0x040031B5 RID: 12725
		private List<InventoryStockGroupDef> tmpInventoryStockGroups;

		// Token: 0x040031B6 RID: 12726
		private List<InventoryStockEntry> tmpInventoryStock;
	}
}
