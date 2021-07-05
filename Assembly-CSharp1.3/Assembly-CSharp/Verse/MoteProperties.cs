using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A5 RID: 165
	public class MoteProperties
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0001B85B File Offset: 0x00019A5B
		public float Lifespan
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}

		// Token: 0x040002C2 RID: 706
		public bool realTime;

		// Token: 0x040002C3 RID: 707
		public float fadeInTime;

		// Token: 0x040002C4 RID: 708
		public float solidTime = 1f;

		// Token: 0x040002C5 RID: 709
		public float fadeOutTime;

		// Token: 0x040002C6 RID: 710
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x040002C7 RID: 711
		public float speedPerTime;

		// Token: 0x040002C8 RID: 712
		public float growthRate;

		// Token: 0x040002C9 RID: 713
		public bool collide;

		// Token: 0x040002CA RID: 714
		public SoundDef landSound;

		// Token: 0x040002CB RID: 715
		public Vector3 unattachedDrawOffset = Vector3.zero;

		// Token: 0x040002CC RID: 716
		public Vector3 attachedDrawOffset;

		// Token: 0x040002CD RID: 717
		public bool needsMaintenance;

		// Token: 0x040002CE RID: 718
		public bool rotateTowardsTarget;

		// Token: 0x040002CF RID: 719
		public bool rotateTowardsMoveDirection;

		// Token: 0x040002D0 RID: 720
		public bool scaleToConnectTargets;

		// Token: 0x040002D1 RID: 721
		public bool attachedToHead;

		// Token: 0x040002D2 RID: 722
		public bool fadeOutUnmaintained;
	}
}
