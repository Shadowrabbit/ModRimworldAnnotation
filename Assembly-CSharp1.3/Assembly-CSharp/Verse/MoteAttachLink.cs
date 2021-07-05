using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000367 RID: 871
	public struct MoteAttachLink
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x00091C4A File Offset: 0x0008FE4A
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060018B2 RID: 6322 RVA: 0x00091C57 File Offset: 0x0008FE57
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x00091C5F File Offset: 0x0008FE5F
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x00091C67 File Offset: 0x0008FE67
		public MoteAttachLink(TargetInfo target, Vector3 offset)
		{
			this.targetInt = target;
			this.offsetInt = offset;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x00091C94 File Offset: 0x0008FE94
		public void UpdateDrawPos()
		{
			if (this.targetInt.HasThing)
			{
				this.lastDrawPosInt = this.targetInt.Thing.DrawPos + this.offsetInt;
				return;
			}
			this.lastDrawPosInt = this.targetInt.Cell.ToVector3Shifted() + this.offsetInt;
		}

		// Token: 0x040010C0 RID: 4288
		private TargetInfo targetInt;

		// Token: 0x040010C1 RID: 4289
		private Vector3 offsetInt;

		// Token: 0x040010C2 RID: 4290
		private Vector3 lastDrawPosInt;

		// Token: 0x040010C3 RID: 4291
		public static readonly MoteAttachLink Invalid = new MoteAttachLink(TargetInfo.Invalid, Vector3.zero);
	}
}
