using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000312 RID: 786
	public class TemporaryThingDrawable : IExposable
	{
		// Token: 0x0600140E RID: 5134 RVA: 0x000CD1D0 File Offset: 0x000CB3D0
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<Vector3>(ref this.position, "position", default(Vector3), false);
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x04000FCB RID: 4043
		public Thing thing;

		// Token: 0x04000FCC RID: 4044
		public Vector3 position;

		// Token: 0x04000FCD RID: 4045
		public int ticksLeft;
	}
}
