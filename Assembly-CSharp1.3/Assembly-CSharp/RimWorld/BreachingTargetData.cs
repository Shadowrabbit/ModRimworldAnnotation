using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000773 RID: 1907
	public class BreachingTargetData : IExposable
	{
		// Token: 0x0600348B RID: 13451 RVA: 0x000033AC File Offset: 0x000015AC
		public BreachingTargetData()
		{
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x00129E3D File Offset: 0x0012803D
		public BreachingTargetData(Thing target, IntVec3 firingPosition)
		{
			this.target = target;
			this.firingPosition = firingPosition;
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x00129E53 File Offset: 0x00128053
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<IntVec3>(ref this.firingPosition, "firingPosition", IntVec3.Invalid, false);
		}

		// Token: 0x04001E55 RID: 7765
		public Thing target;

		// Token: 0x04001E56 RID: 7766
		public IntVec3 firingPosition;
	}
}
