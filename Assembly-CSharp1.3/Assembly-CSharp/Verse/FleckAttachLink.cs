using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000198 RID: 408
	public struct FleckAttachLink
	{
		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0003E584 File Offset: 0x0003C784
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x0003E591 File Offset: 0x0003C791
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0003E599 File Offset: 0x0003C799
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0003E5A1 File Offset: 0x0003C7A1
		public FleckAttachLink(TargetInfo target)
		{
			this.targetInt = target;
			this.detachAfterTicks = -1;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0003E5CC File Offset: 0x0003C7CC
		public void UpdateDrawPos()
		{
			if (this.targetInt.HasThing)
			{
				this.lastDrawPosInt = this.targetInt.Thing.DrawPos;
				return;
			}
			this.lastDrawPosInt = this.targetInt.Cell.ToVector3Shifted();
		}

		// Token: 0x04000991 RID: 2449
		private TargetInfo targetInt;

		// Token: 0x04000992 RID: 2450
		private Vector3 lastDrawPosInt;

		// Token: 0x04000993 RID: 2451
		public int detachAfterTicks;

		// Token: 0x04000994 RID: 2452
		public static readonly FleckAttachLink Invalid = new FleckAttachLink(TargetInfo.Invalid);
	}
}
