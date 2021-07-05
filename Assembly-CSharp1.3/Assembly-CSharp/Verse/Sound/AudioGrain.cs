using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x0200053E RID: 1342
	public abstract class AudioGrain
	{
		// Token: 0x06002867 RID: 10343
		public abstract IEnumerable<ResolvedGrain> GetResolvedGrains();
	}
}
