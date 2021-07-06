using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C6 RID: 198
	public class RememberedCameraPos : IExposable
	{
		// Token: 0x060005FD RID: 1533 RVA: 0x0008D9C0 File Offset: 0x0008BBC0
		public RememberedCameraPos(Map map)
		{
			this.rootPos = map.Center.ToVector3Shifted();
			this.rootSize = 24f;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0008D9F4 File Offset: 0x0008BBF4
		public void ExposeData()
		{
			Scribe_Values.Look<Vector3>(ref this.rootPos, "rootPos", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.rootSize, "rootSize", 0f, false);
		}

		// Token: 0x0400030E RID: 782
		public Vector3 rootPos;

		// Token: 0x0400030F RID: 783
		public float rootSize;
	}
}
