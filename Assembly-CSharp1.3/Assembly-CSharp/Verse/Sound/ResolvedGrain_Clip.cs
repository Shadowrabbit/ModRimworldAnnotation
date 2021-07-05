using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000543 RID: 1347
	public class ResolvedGrain_Clip : ResolvedGrain
	{
		// Token: 0x06002871 RID: 10353 RVA: 0x000F6818 File Offset: 0x000F4A18
		public ResolvedGrain_Clip(AudioClip clip)
		{
			this.clip = clip;
			this.duration = clip.length;
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000F6833 File Offset: 0x000F4A33
		public override string ToString()
		{
			return "Clip:" + this.clip.name;
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000F684C File Offset: 0x000F4A4C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			ResolvedGrain_Clip resolvedGrain_Clip = obj as ResolvedGrain_Clip;
			return resolvedGrain_Clip != null && resolvedGrain_Clip.clip == this.clip;
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000F687B File Offset: 0x000F4A7B
		public override int GetHashCode()
		{
			if (this.clip == null)
			{
				return 0;
			}
			return this.clip.GetHashCode();
		}

		// Token: 0x040018F3 RID: 6387
		public AudioClip clip;
	}
}
