using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FC RID: 4348
	public class CompProperties_AssignableToPawn : CompProperties
	{
		// Token: 0x06006846 RID: 26694 RVA: 0x00234312 File Offset: 0x00232512
		public CompProperties_AssignableToPawn()
		{
			this.compClass = typeof(CompAssignableToPawn);
		}

		// Token: 0x06006847 RID: 26695 RVA: 0x001575A6 File Offset: 0x001557A6
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			return base.ConfigErrors(parentDef);
		}

		// Token: 0x06006848 RID: 26696 RVA: 0x0023433F File Offset: 0x0023253F
		public override void PostLoadSpecial(ThingDef parent)
		{
			if (parent.thingClass == typeof(Building_Bed))
			{
				this.maxAssignedPawnsCount = BedUtility.GetSleepingSlotsCount(parent.size);
			}
		}

		// Token: 0x04003A93 RID: 14995
		public int maxAssignedPawnsCount = 1;

		// Token: 0x04003A94 RID: 14996
		public bool drawAssignmentOverlay = true;

		// Token: 0x04003A95 RID: 14997
		public bool drawUnownedAssignmentOverlay = true;

		// Token: 0x04003A96 RID: 14998
		public string singleton;
	}
}
