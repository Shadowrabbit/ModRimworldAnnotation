using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000195 RID: 405
	public struct FleckCreationData
	{
		// Token: 0x04000980 RID: 2432
		public FleckDef def;

		// Token: 0x04000981 RID: 2433
		public Vector3 spawnPosition;

		// Token: 0x04000982 RID: 2434
		public float rotation;

		// Token: 0x04000983 RID: 2435
		public float scale;

		// Token: 0x04000984 RID: 2436
		public Vector3? exactScale;

		// Token: 0x04000985 RID: 2437
		public Color? instanceColor;

		// Token: 0x04000986 RID: 2438
		public float velocityAngle;

		// Token: 0x04000987 RID: 2439
		public float velocitySpeed;

		// Token: 0x04000988 RID: 2440
		public Vector3? velocity;

		// Token: 0x04000989 RID: 2441
		public float rotationRate;

		// Token: 0x0400098A RID: 2442
		public float? solidTimeOverride;

		// Token: 0x0400098B RID: 2443
		public float? airTimeLeft;

		// Token: 0x0400098C RID: 2444
		public FleckAttachLink link;

		// Token: 0x0400098D RID: 2445
		public float targetSize;
	}
}
