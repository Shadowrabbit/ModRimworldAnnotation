using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004C3 RID: 1219
	public abstract class AttachableThing : Thing
	{
		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001E35 RID: 7733 RVA: 0x0001AD4B File Offset: 0x00018F4B
		public override Vector3 DrawPos
		{
			get
			{
				if (this.parent != null)
				{
					return this.parent.DrawPos + Vector3.up * 0.042857144f * 0.9f;
				}
				return base.DrawPos;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001E36 RID: 7734
		public abstract string InspectStringAddon { get; }

		// Token: 0x06001E37 RID: 7735 RVA: 0x0001AD85 File Offset: 0x00018F85
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.parent, "parent", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.parent != null)
			{
				this.AttachTo(this.parent);
			}
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x000FB5B4 File Offset: 0x000F97B4
		public virtual void AttachTo(Thing parent)
		{
			this.parent = parent;
			CompAttachBase compAttachBase = parent.TryGetComp<CompAttachBase>();
			if (compAttachBase == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot attach ",
					this,
					" to ",
					parent,
					": parent has no CompAttachBase."
				}), false);
				return;
			}
			compAttachBase.AddAttachment(this);
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x0001ADBA File Offset: 0x00018FBA
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.parent != null)
			{
				this.parent.TryGetComp<CompAttachBase>().RemoveAttachment(this);
			}
		}

		// Token: 0x04001585 RID: 5509
		public Thing parent;
	}
}
