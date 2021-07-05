using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A00 RID: 2560
	public class CompProperties_Facility : CompProperties
	{
		// Token: 0x06003EEA RID: 16106 RVA: 0x00157B4C File Offset: 0x00155D4C
		public CompProperties_Facility()
		{
			this.compClass = typeof(CompFacility);
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x00157B78 File Offset: 0x00155D78
		public override void ResolveReferences(ThingDef parentDef)
		{
			this.linkableBuildings = new List<ThingDef>();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				CompProperties_AffectedByFacilities compProperties = allDefsListForReading[i].GetCompProperties<CompProperties_AffectedByFacilities>();
				if (compProperties != null && compProperties.linkableFacilities != null)
				{
					for (int j = 0; j < compProperties.linkableFacilities.Count; j++)
					{
						if (compProperties.linkableFacilities[j] == parentDef)
						{
							this.linkableBuildings.Add(allDefsListForReading[i]);
							break;
						}
					}
				}
			}
		}

		// Token: 0x040021CF RID: 8655
		[Unsaved(false)]
		public List<ThingDef> linkableBuildings;

		// Token: 0x040021D0 RID: 8656
		public List<StatModifier> statOffsets;

		// Token: 0x040021D1 RID: 8657
		public int maxSimultaneous = 1;

		// Token: 0x040021D2 RID: 8658
		public bool mustBePlacedAdjacent;

		// Token: 0x040021D3 RID: 8659
		public bool mustBePlacedAdjacentCardinalToBedHead;

		// Token: 0x040021D4 RID: 8660
		public bool mustBePlacedAdjacentCardinalToAndFacingBedHead;

		// Token: 0x040021D5 RID: 8661
		public bool canLinkToMedBedsOnly;

		// Token: 0x040021D6 RID: 8662
		public float maxDistance = 8f;
	}
}
