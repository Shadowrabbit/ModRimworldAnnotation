using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200010B RID: 267
	public class MoteProperties
	{
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0000BF75 File Offset: 0x0000A175
		public float Lifespan
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}

		// Token: 0x040004A5 RID: 1189
		public bool realTime;

		// Token: 0x040004A6 RID: 1190
		public float fadeInTime;

		// Token: 0x040004A7 RID: 1191
		public float solidTime = 1f;

		// Token: 0x040004A8 RID: 1192
		public float fadeOutTime;

		// Token: 0x040004A9 RID: 1193
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x040004AA RID: 1194
		public float speedPerTime;

		// Token: 0x040004AB RID: 1195
		public float growthRate;

		// Token: 0x040004AC RID: 1196
		public bool collide;

		// Token: 0x040004AD RID: 1197
		public SoundDef landSound;

		// Token: 0x040004AE RID: 1198
		public Vector3 unattachedDrawOffset = Vector3.zero;

		// Token: 0x040004AF RID: 1199
		public Vector3 attachedDrawOffset;

		// Token: 0x040004B0 RID: 1200
		public bool needsMaintenance;

		// Token: 0x040004B1 RID: 1201
		public bool rotateTowardsTarget;

		// Token: 0x040004B2 RID: 1202
		public bool rotateTowardsMoveDirection;

		// Token: 0x040004B3 RID: 1203
		public bool scaleToConnectTargets;

		// Token: 0x040004B4 RID: 1204
		public bool attachedToHead;
	}
}
