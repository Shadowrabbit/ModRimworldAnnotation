using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012CE RID: 4814
	public class Designator_ZoneAdd_Growing : Designator_ZoneAdd
	{
		// Token: 0x1700142D RID: 5165
		// (get) Token: 0x0600731F RID: 29471 RVA: 0x00267029 File Offset: 0x00265229
		protected override string NewZoneLabel
		{
			get
			{
				return "GrowingZone".Translate();
			}
		}

		// Token: 0x06007320 RID: 29472 RVA: 0x0026703C File Offset: 0x0026523C
		public Designator_ZoneAdd_Growing()
		{
			this.zoneTypeToPlace = typeof(Zone_Growing);
			this.defaultLabel = "GrowingZone".Translate();
			this.defaultDesc = "DesignatorGrowingZoneDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true);
			this.tutorTag = "ZoneAdd_Growing";
			this.hotKey = KeyBindingDefOf.Misc2;
		}

		// Token: 0x06007321 RID: 29473 RVA: 0x002670B0 File Offset: 0x002652B0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!base.CanDesignateCell(c).Accepted)
			{
				return false;
			}
			float num = ThingDefOf.Plant_Potato.plant.fertilityMin;
			if (ModsConfig.IdeologyActive && BuildCopyCommandUtility.FindAllowedDesignator(TerrainDefOf.FungalGravel, true) != null)
			{
				num = Mathf.Min(num, ThingDefOf.Nutrifungus.plant.fertilityMin);
			}
			if (base.Map.fertilityGrid.FertilityAt(c) < num)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007322 RID: 29474 RVA: 0x00267130 File Offset: 0x00265330
		protected override Zone MakeNewZone()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
			return new Zone_Growing(Find.CurrentMap.zoneManager);
		}
	}
}
