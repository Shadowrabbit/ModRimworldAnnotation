using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019CA RID: 6602
	public abstract class Designator_ZoneAddStockpile : Designator_ZoneAdd
	{
		// Token: 0x17001730 RID: 5936
		// (get) Token: 0x06009208 RID: 37384 RVA: 0x00061CFB File Offset: 0x0005FEFB
		protected override string NewZoneLabel
		{
			get
			{
				return this.preset.PresetName();
			}
		}

		// Token: 0x06009209 RID: 37385 RVA: 0x00061D08 File Offset: 0x0005FF08
		protected override Zone MakeNewZone()
		{
			return new Zone_Stockpile(this.preset, Find.CurrentMap.zoneManager);
		}

		// Token: 0x0600920A RID: 37386 RVA: 0x00061D1F File Offset: 0x0005FF1F
		public Designator_ZoneAddStockpile()
		{
			this.zoneTypeToPlace = typeof(Zone_Stockpile);
		}

		// Token: 0x0600920B RID: 37387 RVA: 0x0029F534 File Offset: 0x0029D734
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result = base.CanDesignateCell(c);
			if (!result.Accepted)
			{
				return result;
			}
			if (c.GetTerrain(base.Map).passability == Traversability.Impassable)
			{
				return false;
			}
			List<Thing> list = base.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].def.CanOverlapZones)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600920C RID: 37388 RVA: 0x00061D37 File Offset: 0x0005FF37
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Stockpiles, KnowledgeAmount.Total);
		}

		// Token: 0x04005C66 RID: 23654
		protected StorageSettingsPreset preset;
	}
}
