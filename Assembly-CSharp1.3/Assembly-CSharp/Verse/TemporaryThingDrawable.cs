using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000221 RID: 545
	public class TemporaryThingDrawable : IExposable
	{
		// Token: 0x06000F88 RID: 3976 RVA: 0x000584A4 File Offset: 0x000566A4
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<Vector3>(ref this.position, "position", default(Vector3), false);
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x04000C40 RID: 3136
		public Thing thing;

		// Token: 0x04000C41 RID: 3137
		public Vector3 position;

		// Token: 0x04000C42 RID: 3138
		public int ticksLeft;
	}
}
