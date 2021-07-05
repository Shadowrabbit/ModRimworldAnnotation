using System;

namespace Verse
{
	// Token: 0x0200052B RID: 1323
	public class CompWindSource : ThingComp
	{
		// Token: 0x060021EB RID: 8683 RVA: 0x0001D5C2 File Offset: 0x0001B7C2
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.wind, "wind", 0f, false);
		}

		// Token: 0x04001700 RID: 5888
		public float wind;
	}
}
