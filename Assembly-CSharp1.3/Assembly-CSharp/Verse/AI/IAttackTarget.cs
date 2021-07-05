using System;

namespace Verse.AI
{
	// Token: 0x0200065D RID: 1629
	public interface IAttackTarget : ILoadReferenceable
	{
		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002E0D RID: 11789
		Thing Thing { get; }

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002E0E RID: 11790
		LocalTargetInfo TargetCurrentlyAimingAt { get; }

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002E0F RID: 11791
		float TargetPriorityFactor { get; }

		// Token: 0x06002E10 RID: 11792
		bool ThreatDisabled(IAttackTargetSearcher disabledFor);
	}
}
