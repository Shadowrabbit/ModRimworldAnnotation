using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x0200090D RID: 2317
	public abstract class AudioGrain
	{
		// Token: 0x06003984 RID: 14724
		public abstract IEnumerable<ResolvedGrain> GetResolvedGrains();
	}
}
