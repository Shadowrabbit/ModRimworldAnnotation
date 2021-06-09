using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200168F RID: 5775
	public interface IHaulDestination : IStoreSettingsParent
	{
		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x06007E59 RID: 32345
		IntVec3 Position { get; }

		// Token: 0x17001386 RID: 4998
		// (get) Token: 0x06007E5A RID: 32346
		Map Map { get; }

		// Token: 0x06007E5B RID: 32347
		bool Accepts(Thing t);
	}
}
