using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200172D RID: 5933
	public interface IActiveDropPod : IThingHolder
	{
		// Token: 0x1700145B RID: 5211
		// (get) Token: 0x060082DF RID: 33503
		ActiveDropPodInfo Contents { get; }
	}
}
