using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012CA RID: 4810
	public abstract class Designator_ZoneAddStockpile : Designator_ZoneAdd
	{
		// Token: 0x1700142C RID: 5164
		// (get) Token: 0x06007315 RID: 29461 RVA: 0x00266E62 File Offset: 0x00265062
		protected override string NewZoneLabel
		{
			get
			{
				return this.preset.PresetName();
			}
		}

		// Token: 0x06007316 RID: 29462 RVA: 0x00266E6F File Offset: 0x0026506F
		protected override Zone MakeNewZone()
		{
			return new Zone_Stockpile(this.preset, Find.CurrentMap.zoneManager);
		}

		// Token: 0x06007317 RID: 29463 RVA: 0x00266E86 File Offset: 0x00265086
		public Designator_ZoneAddStockpile()
		{
			this.zoneTypeToPlace = typeof(Zone_Stockpile);
		}

		// Token: 0x06007318 RID: 29464 RVA: 0x00266EA0 File Offset: 0x002650A0
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

		// Token: 0x06007319 RID: 29465 RVA: 0x00266F1E File Offset: 0x0026511E
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Stockpiles, KnowledgeAmount.Total);
		}

		// Token: 0x04003ED5 RID: 16085
		protected StorageSettingsPreset preset;
	}
}
