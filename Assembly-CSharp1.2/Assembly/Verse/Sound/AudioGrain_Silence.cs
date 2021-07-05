using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000912 RID: 2322
	public class AudioGrain_Silence : AudioGrain
	{
		// Token: 0x0600399B RID: 14747 RVA: 0x0002C72E File Offset: 0x0002A92E
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			yield return new ResolvedGrain_Silence(this);
			yield break;
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x0002C73E File Offset: 0x0002A93E
		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}

		// Token: 0x040027E1 RID: 10209
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);
	}
}
