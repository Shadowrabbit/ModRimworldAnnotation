using System;

namespace Verse
{
	// Token: 0x0200038A RID: 906
	public class CompWindSource : ThingComp
	{
		// Token: 0x06001A85 RID: 6789 RVA: 0x00099D4D File Offset: 0x00097F4D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.wind, "wind", 0f, false);
		}

		// Token: 0x04001138 RID: 4408
		public float wind;
	}
}
