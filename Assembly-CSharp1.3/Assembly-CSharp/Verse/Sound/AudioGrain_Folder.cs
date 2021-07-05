using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000540 RID: 1344
	public class AudioGrain_Folder : AudioGrain
	{
		// Token: 0x0600286B RID: 10347 RVA: 0x000F67B5 File Offset: 0x000F49B5
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			foreach (AudioClip clip in ContentFinder<AudioClip>.GetAllInFolder(this.clipFolderPath))
			{
				yield return new ResolvedGrain_Clip(clip);
			}
			IEnumerator<AudioClip> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040018F0 RID: 6384
		[LoadAlias("clipPath")]
		[NoTranslate]
		public string clipFolderPath = "";
	}
}
