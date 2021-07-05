using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012CB RID: 4811
	public class Designator_ZoneAddStockpile_Resources : Designator_ZoneAddStockpile
	{
		// Token: 0x0600731A RID: 29466 RVA: 0x00266F34 File Offset: 0x00265134
		public Designator_ZoneAddStockpile_Resources()
		{
			this.preset = StorageSettingsPreset.DefaultStockpile;
			this.defaultLabel = this.preset.PresetName();
			this.defaultDesc = "DesignatorZoneCreateStorageResourcesDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
			this.hotKey = KeyBindingDefOf.Misc1;
			this.tutorTag = "ZoneAddStockpile_Resources";
		}

		// Token: 0x0600731B RID: 29467 RVA: 0x00266F9B File Offset: 0x0026519B
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.StorageTab, OpportunityType.GoodToKnow);
		}
	}
}
