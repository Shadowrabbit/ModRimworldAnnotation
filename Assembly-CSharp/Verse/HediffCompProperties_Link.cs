using System;

namespace Verse
{
	// Token: 0x020003E8 RID: 1000
	public class HediffCompProperties_Link : HediffCompProperties
	{
		// Token: 0x0600187B RID: 6267 RVA: 0x00017399 File Offset: 0x00015599
		public HediffCompProperties_Link()
		{
			this.compClass = typeof(HediffComp_Link);
		}

		// Token: 0x04001287 RID: 4743
		public bool showName = true;

		// Token: 0x04001288 RID: 4744
		public float maxDistance = -1f;
	}
}
