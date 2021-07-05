using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000910 RID: 2320
	public class AudioGrain_Folder : AudioGrain
	{
		// Token: 0x06003990 RID: 14736 RVA: 0x0002C6C5 File Offset: 0x0002A8C5
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

		// Token: 0x040027DB RID: 10203
		[LoadAlias("clipPath")]
		[NoTranslate]
		public string clipFolderPath = "";
	}
}
