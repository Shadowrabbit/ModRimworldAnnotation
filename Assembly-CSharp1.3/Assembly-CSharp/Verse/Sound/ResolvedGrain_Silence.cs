using System;

namespace Verse.Sound
{
	// Token: 0x02000544 RID: 1348
	public class ResolvedGrain_Silence : ResolvedGrain
	{
		// Token: 0x06002875 RID: 10357 RVA: 0x000F6898 File Offset: 0x000F4A98
		public ResolvedGrain_Silence(AudioGrain_Silence sourceGrain)
		{
			this.sourceGrain = sourceGrain;
			this.duration = sourceGrain.durationRange.RandomInRange;
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x000F68B8 File Offset: 0x000F4AB8
		public override string ToString()
		{
			return "Silence";
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000F68C0 File Offset: 0x000F4AC0
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			ResolvedGrain_Silence resolvedGrain_Silence = obj as ResolvedGrain_Silence;
			return resolvedGrain_Silence != null && resolvedGrain_Silence.sourceGrain == this.sourceGrain;
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x000F68EC File Offset: 0x000F4AEC
		public override int GetHashCode()
		{
			return this.sourceGrain.GetHashCode();
		}

		// Token: 0x040018F4 RID: 6388
		public AudioGrain_Silence sourceGrain;
	}
}
