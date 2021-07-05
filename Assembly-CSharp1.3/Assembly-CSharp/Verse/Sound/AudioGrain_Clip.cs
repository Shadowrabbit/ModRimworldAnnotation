using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200053F RID: 1343
	public class AudioGrain_Clip : AudioGrain
	{
		// Token: 0x06002869 RID: 10345 RVA: 0x000F6792 File Offset: 0x000F4992
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			AudioClip audioClip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			if (audioClip != null)
			{
				yield return new ResolvedGrain_Clip(audioClip);
			}
			else
			{
				Log.Error("Grain couldn't resolve: Clip not found at " + this.clipPath);
			}
			yield break;
		}

		// Token: 0x040018EF RID: 6383
		[NoTranslate]
		public string clipPath = "";
	}
}
