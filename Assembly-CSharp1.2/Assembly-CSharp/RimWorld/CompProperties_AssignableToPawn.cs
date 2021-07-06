using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200178F RID: 6031
	public class CompProperties_AssignableToPawn : CompProperties
	{
		// Token: 0x0600852B RID: 34091 RVA: 0x000593D4 File Offset: 0x000575D4
		public CompProperties_AssignableToPawn()
		{
			this.compClass = typeof(CompAssignableToPawn);
		}

		// Token: 0x0600852C RID: 34092 RVA: 0x0003B05D File Offset: 0x0003925D
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			return base.ConfigErrors(parentDef);
		}

		// Token: 0x0600852D RID: 34093 RVA: 0x00059401 File Offset: 0x00057601
		public override void PostLoadSpecial(ThingDef parent)
		{
			if (parent.thingClass == typeof(Building_Bed))
			{
				this.maxAssignedPawnsCount = BedUtility.GetSleepingSlotsCount(parent.size);
			}
		}

		// Token: 0x0400561B RID: 22043
		public int maxAssignedPawnsCount = 1;

		// Token: 0x0400561C RID: 22044
		public bool drawAssignmentOverlay = true;

		// Token: 0x0400561D RID: 22045
		public bool drawUnownedAssignmentOverlay = true;

		// Token: 0x0400561E RID: 22046
		public string singleton;
	}
}
