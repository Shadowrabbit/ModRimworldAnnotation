using System;

namespace Verse.AI
{
	// Token: 0x02000AAF RID: 2735
	public interface IAttackTargetSearcher
	{
		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060040A5 RID: 16549
		Thing Thing { get; }

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060040A6 RID: 16550
		Verb CurrentEffectiveVerb { get; }

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060040A7 RID: 16551
		LocalTargetInfo LastAttackedTarget { get; }

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060040A8 RID: 16552
		int LastAttackTargetTick { get; }
	}
}
