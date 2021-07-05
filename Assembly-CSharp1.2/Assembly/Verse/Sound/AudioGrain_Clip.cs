using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200090E RID: 2318
	public class AudioGrain_Clip : AudioGrain
	{
		// Token: 0x06003986 RID: 14726 RVA: 0x0002C678 File Offset: 0x0002A878
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			AudioClip audioClip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			if (audioClip != null)
			{
				yield return new ResolvedGrain_Clip(audioClip);
			}
			else
			{
				Log.Error("Grain couldn't resolve: Clip not found at " + this.clipPath, false);
			}
			yield break;
		}

		// Token: 0x040027D6 RID: 10198
		[NoTranslate]
		public string clipPath = "";
	}
}
