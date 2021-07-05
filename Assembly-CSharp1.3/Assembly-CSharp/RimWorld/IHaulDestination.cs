using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105B RID: 4187
	public interface IHaulDestination : IStoreSettingsParent
	{
		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x0600634C RID: 25420
		IntVec3 Position { get; }

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x0600634D RID: 25421
		Map Map { get; }

		// Token: 0x0600634E RID: 25422
		bool Accepts(Thing t);
	}
}
