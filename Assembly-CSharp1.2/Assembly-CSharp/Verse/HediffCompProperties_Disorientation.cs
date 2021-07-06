using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003CF RID: 975
	public class HediffCompProperties_Disorientation : HediffCompProperties
	{
		// Token: 0x06001822 RID: 6178 RVA: 0x00016F55 File Offset: 0x00015155
		public HediffCompProperties_Disorientation()
		{
			this.compClass = typeof(HediffComp_Disorientation);
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x00016F7F File Offset: 0x0001517F
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.wanderMtbHours <= 0f)
			{
				yield return "wanderMtbHours must be greater than zero";
			}
			if (this.singleWanderDurationTicks <= 0)
			{
				yield return "singleWanderDurationTicks must be greater than zero";
			}
			if (this.wanderRadius <= 0f)
			{
				yield return "wanderRadius must be greater than zero";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400124E RID: 4686
		public float wanderMtbHours = -1f;

		// Token: 0x0400124F RID: 4687
		public float wanderRadius;

		// Token: 0x04001250 RID: 4688
		public int singleWanderDurationTicks = -1;
	}
}
