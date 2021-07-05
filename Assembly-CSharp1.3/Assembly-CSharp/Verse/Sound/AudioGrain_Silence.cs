using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000541 RID: 1345
	public class AudioGrain_Silence : AudioGrain
	{
		// Token: 0x0600286D RID: 10349 RVA: 0x000F67D8 File Offset: 0x000F49D8
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			yield return new ResolvedGrain_Silence(this);
			yield break;
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000F67E8 File Offset: 0x000F49E8
		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}

		// Token: 0x040018F1 RID: 6385
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);
	}
}
