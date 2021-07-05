using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200028D RID: 653
	public class HediffCompProperties_Disorientation : HediffCompProperties
	{
		// Token: 0x06001255 RID: 4693 RVA: 0x00069FAA File Offset: 0x000681AA
		public HediffCompProperties_Disorientation()
		{
			this.compClass = typeof(HediffComp_Disorientation);
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00069FD4 File Offset: 0x000681D4
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

		// Token: 0x04000DE4 RID: 3556
		public float wanderMtbHours = -1f;

		// Token: 0x04000DE5 RID: 3557
		public float wanderRadius;

		// Token: 0x04000DE6 RID: 3558
		public int singleWanderDurationTicks = -1;
	}
}
