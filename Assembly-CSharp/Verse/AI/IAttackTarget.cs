using System;

namespace Verse.AI
{
	// Token: 0x02000AC1 RID: 2753
	public interface IAttackTarget : ILoadReferenceable
	{
		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060040F0 RID: 16624
		Thing Thing { get; }

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060040F1 RID: 16625
		LocalTargetInfo TargetCurrentlyAimingAt { get; }

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060040F2 RID: 16626
		float TargetPriorityFactor { get; }

		// Token: 0x060040F3 RID: 16627
		bool ThreatDisabled(IAttackTargetSearcher disabledFor);
	}
}
