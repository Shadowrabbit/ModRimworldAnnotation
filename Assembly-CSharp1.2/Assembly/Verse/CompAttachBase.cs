using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000522 RID: 1314
	public class CompAttachBase : ThingComp
	{
		// Token: 0x060021B5 RID: 8629 RVA: 0x001077DC File Offset: 0x001059DC
		public override void CompTick()
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					this.attachments[i].Position = this.parent.Position;
				}
			}
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x00107824 File Offset: 0x00105A24
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.attachments != null)
			{
				for (int i = this.attachments.Count - 1; i >= 0; i--)
				{
					this.attachments[i].Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x0010786C File Offset: 0x00105A6C
		public override string CompInspectStringExtra()
		{
			if (this.attachments != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.attachments.Count; i++)
				{
					stringBuilder.AppendLine(this.attachments[i].InspectStringAddon);
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
			return null;
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x001078C4 File Offset: 0x00105AC4
		public Thing GetAttachment(ThingDef def)
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					if (this.attachments[i].def == def)
					{
						return this.attachments[i];
					}
				}
			}
			return null;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x0001D2E0 File Offset: 0x0001B4E0
		public bool HasAttachment(ThingDef def)
		{
			return this.GetAttachment(def) != null;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x0001D2EC File Offset: 0x0001B4EC
		public void AddAttachment(AttachableThing t)
		{
			if (this.attachments == null)
			{
				this.attachments = new List<AttachableThing>();
			}
			this.attachments.Add(t);
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x0001D30D File Offset: 0x0001B50D
		public void RemoveAttachment(AttachableThing t)
		{
			this.attachments.Remove(t);
		}

		// Token: 0x040016F7 RID: 5879
		public List<AttachableThing> attachments;
	}
}
