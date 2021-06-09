using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F7 RID: 1271
	public struct MoteAttachLink
	{
		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x0001BE6C File Offset: 0x0001A06C
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x0001BE79 File Offset: 0x0001A079
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x0001BE81 File Offset: 0x0001A081
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x0001BE89 File Offset: 0x0001A089
		public static MoteAttachLink Invalid
		{
			get
			{
				return new MoteAttachLink(TargetInfo.Invalid);
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0001BE95 File Offset: 0x0001A095
		public MoteAttachLink(TargetInfo target)
		{
			this.targetInt = target;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x001009D8 File Offset: 0x000FEBD8
		public void UpdateDrawPos()
		{
			if (this.targetInt.HasThing)
			{
				this.lastDrawPosInt = this.targetInt.Thing.DrawPos;
				return;
			}
			this.lastDrawPosInt = this.targetInt.Cell.ToVector3Shifted();
		}

		// Token: 0x0400163B RID: 5691
		private TargetInfo targetInt;

		// Token: 0x0400163C RID: 5692
		private Vector3 lastDrawPosInt;
	}
}
