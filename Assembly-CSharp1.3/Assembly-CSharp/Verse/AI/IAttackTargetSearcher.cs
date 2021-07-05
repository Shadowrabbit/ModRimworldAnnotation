using System;

namespace Verse.AI
{
	// Token: 0x02000652 RID: 1618
	public interface IAttackTargetSearcher
	{
		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002DBE RID: 11710
		Thing Thing { get; }

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06002DBF RID: 11711
		Verb CurrentEffectiveVerb { get; }

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06002DC0 RID: 11712
		LocalTargetInfo LastAttackedTarget { get; }

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06002DC1 RID: 11713
		int LastAttackTargetTick { get; }
	}
}
