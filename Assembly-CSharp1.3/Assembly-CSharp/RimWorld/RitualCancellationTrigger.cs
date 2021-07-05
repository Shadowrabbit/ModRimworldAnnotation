using System;
using System.Collections.Generic;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F2D RID: 3885
	public abstract class RitualCancellationTrigger
	{
		// Token: 0x06005C65 RID: 23653
		public abstract IEnumerable<Trigger> CancellationTriggers(RitualRoleAssignments assignments);
	}
}
