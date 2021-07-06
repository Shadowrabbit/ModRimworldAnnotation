using System;

namespace Verse.Sound
{
	// Token: 0x02000916 RID: 2326
	public class ResolvedGrain_Silence : ResolvedGrain
	{
		// Token: 0x060039AB RID: 14763 RVA: 0x0002C7E7 File Offset: 0x0002A9E7
		public ResolvedGrain_Silence(AudioGrain_Silence sourceGrain)
		{
			this.sourceGrain = sourceGrain;
			this.duration = sourceGrain.durationRange.RandomInRange;
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x0002C807 File Offset: 0x0002AA07
		public override string ToString()
		{
			return "Silence";
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x00167CF4 File Offset: 0x00165EF4
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			ResolvedGrain_Silence resolvedGrain_Silence = obj as ResolvedGrain_Silence;
			return resolvedGrain_Silence != null && resolvedGrain_Silence.sourceGrain == this.sourceGrain;
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x0002C80E File Offset: 0x0002AA0E
		public override int GetHashCode()
		{
			return this.sourceGrain.GetHashCode();
		}

		// Token: 0x040027E8 RID: 10216
		public AudioGrain_Silence sourceGrain;
	}
}
