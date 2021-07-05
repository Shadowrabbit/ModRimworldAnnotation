using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000383 RID: 899
	public class CompProperties_ColorableAnimated : CompProperties
	{
		// Token: 0x06001A5A RID: 6746 RVA: 0x0009969A File Offset: 0x0009789A
		public CompProperties_ColorableAnimated()
		{
			this.compClass = typeof(CompColorable_Animated);
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x000996C4 File Offset: 0x000978C4
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.colors.Count == 0)
			{
				yield return "there should be at least one color specified in colors list";
			}
			yield break;
		}

		// Token: 0x0400112E RID: 4398
		public int changeInterval = 1;

		// Token: 0x0400112F RID: 4399
		public bool startWithRandom;

		// Token: 0x04001130 RID: 4400
		public List<Color> colors = new List<Color>();
	}
}
