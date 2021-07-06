using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0C RID: 3852
	public class CompProperties_Facility : CompProperties
	{
		// Token: 0x0600553E RID: 21822 RVA: 0x0003B1AB File Offset: 0x000393AB
		public CompProperties_Facility()
		{
			this.compClass = typeof(CompFacility);
		}

		// Token: 0x0600553F RID: 21823 RVA: 0x001C7DC4 File Offset: 0x001C5FC4
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

		// Token: 0x04003654 RID: 13908
		[Unsaved(false)]
		public List<ThingDef> linkableBuildings;

		// Token: 0x04003655 RID: 13909
		public List<StatModifier> statOffsets;

		// Token: 0x04003656 RID: 13910
		public int maxSimultaneous = 1;

		// Token: 0x04003657 RID: 13911
		public bool mustBePlacedAdjacent;

		// Token: 0x04003658 RID: 13912
		public bool mustBePlacedAdjacentCardinalToBedHead;

		// Token: 0x04003659 RID: 13913
		public bool canLinkToMedBedsOnly;

		// Token: 0x0400365A RID: 13914
		public float maxDistance = 8f;
	}
}
