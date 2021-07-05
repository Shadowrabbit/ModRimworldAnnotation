using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000915 RID: 2325
	public class ResolvedGrain_Clip : ResolvedGrain
	{
		// Token: 0x060039A7 RID: 14759 RVA: 0x0002C798 File Offset: 0x0002A998
		public ResolvedGrain_Clip(AudioClip clip)
		{
			this.clip = clip;
			this.duration = clip.length;
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x0002C7B3 File Offset: 0x0002A9B3
		public override string ToString()
		{
			return "Clip:" + this.clip.name;
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x00167CC4 File Offset: 0x00165EC4
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			ResolvedGrain_Clip resolvedGrain_Clip = obj as ResolvedGrain_Clip;
			return resolvedGrain_Clip != null && resolvedGrain_Clip.clip == this.clip;
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x0002C7CA File Offset: 0x0002A9CA
		public override int GetHashCode()
		{
			if (this.clip == null)
			{
				return 0;
			}
			return this.clip.GetHashCode();
		}

		// Token: 0x040027E7 RID: 10215
		public AudioClip clip;
	}
}
