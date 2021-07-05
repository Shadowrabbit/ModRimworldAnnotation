using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000079 RID: 121
	public class RememberedCameraPos : IExposable
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x0001825C File Offset: 0x0001645C
		public RememberedCameraPos(Map map)
		{
			this.rootPos = map.Center.ToVector3Shifted();
			this.rootSize = 24f;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00018290 File Offset: 0x00016490
		public void ExposeData()
		{
			Scribe_Values.Look<Vector3>(ref this.rootPos, "rootPos", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.rootSize, "rootSize", 0f, false);
		}

		// Token: 0x04000194 RID: 404
		public Vector3 rootPos;

		// Token: 0x04000195 RID: 405
		public float rootSize;
	}
}
