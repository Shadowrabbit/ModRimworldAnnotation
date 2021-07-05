using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000339 RID: 825
	public abstract class AttachableThing : Thing
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x0600175D RID: 5981 RVA: 0x0008B829 File Offset: 0x00089A29
		public override Vector3 DrawPos
		{
			get
			{
				if (this.parent != null)
				{
					return this.parent.DrawPos + Vector3.up * 0.04054054f * 0.9f;
				}
				return base.DrawPos;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x0600175E RID: 5982
		public abstract string InspectStringAddon { get; }

		// Token: 0x0600175F RID: 5983 RVA: 0x0008B863 File Offset: 0x00089A63
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.parent, "parent", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.parent != null)
			{
				this.AttachTo(this.parent);
			}
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x0008B898 File Offset: 0x00089A98
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
				}));
				return;
			}
			compAttachBase.AddAttachment(this);
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x0008B8EE File Offset: 0x00089AEE
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.parent != null)
			{
				this.parent.TryGetComp<CompAttachBase>().RemoveAttachment(this);
			}
		}

		// Token: 0x04001028 RID: 4136
		public Thing parent;
	}
}
