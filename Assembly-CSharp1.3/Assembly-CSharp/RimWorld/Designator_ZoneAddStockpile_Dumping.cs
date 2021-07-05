using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012CC RID: 4812
	public class Designator_ZoneAddStockpile_Dumping : Designator_ZoneAddStockpile
	{
		// Token: 0x0600731C RID: 29468 RVA: 0x00266FB0 File Offset: 0x002651B0
		public Designator_ZoneAddStockpile_Dumping()
		{
			this.preset = StorageSettingsPreset.DumpingStockpile;
			this.defaultLabel = this.preset.PresetName();
			this.defaultDesc = "DesignatorZoneCreateStorageDumpingDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
		}

		// Token: 0x0600731D RID: 29469 RVA: 0x00266F9B File Offset: 0x0026519B
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.StorageTab, OpportunityType.GoodToKnow);
		}
	}
}
