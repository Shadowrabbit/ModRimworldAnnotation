using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019CE RID: 6606
	public class Designator_ZoneAdd_Growing : Designator_ZoneAdd
	{
		// Token: 0x17001731 RID: 5937
		// (get) Token: 0x06009212 RID: 37394 RVA: 0x00061D85 File Offset: 0x0005FF85
		protected override string NewZoneLabel
		{
			get
			{
				return "GrowingZone".Translate();
			}
		}

		// Token: 0x06009213 RID: 37395 RVA: 0x0029F670 File Offset: 0x0029D870
		public Designator_ZoneAdd_Growing()
		{
			this.zoneTypeToPlace = typeof(Zone_Growing);
			this.defaultLabel = "GrowingZone".Translate();
			this.defaultDesc = "DesignatorGrowingZoneDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true);
			this.tutorTag = "ZoneAdd_Growing";
			this.hotKey = KeyBindingDefOf.Misc2;
		}

		// Token: 0x06009214 RID: 37396 RVA: 0x0029F6E4 File Offset: 0x0029D8E4
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!base.CanDesignateCell(c).Accepted)
			{
				return false;
			}
			if (base.Map.fertilityGrid.FertilityAt(c) < ThingDefOf.Plant_Potato.plant.fertilityMin)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06009215 RID: 37397 RVA: 0x00061D96 File Offset: 0x0005FF96
		protected override Zone MakeNewZone()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
			return new Zone_Growing(Find.CurrentMap.zoneManager);
		}
	}
}
