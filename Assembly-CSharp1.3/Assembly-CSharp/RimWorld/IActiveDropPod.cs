using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CA RID: 4298
	public interface IActiveDropPod : IThingHolder
	{
		// Token: 0x170011A6 RID: 4518
		// (get) Token: 0x060066D7 RID: 26327
		ActiveDropPodInfo Contents { get; }
	}
}
