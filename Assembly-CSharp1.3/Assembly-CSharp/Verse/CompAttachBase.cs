using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x0200037F RID: 895
	public class CompAttachBase : ThingComp
	{
		// Token: 0x06001A44 RID: 6724 RVA: 0x000992BC File Offset: 0x000974BC
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

		// Token: 0x06001A45 RID: 6725 RVA: 0x00099304 File Offset: 0x00097504
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

		// Token: 0x06001A46 RID: 6726 RVA: 0x0009934C File Offset: 0x0009754C
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

		// Token: 0x06001A47 RID: 6727 RVA: 0x000993A4 File Offset: 0x000975A4
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

		// Token: 0x06001A48 RID: 6728 RVA: 0x000993F1 File Offset: 0x000975F1
		public bool HasAttachment(ThingDef def)
		{
			return this.GetAttachment(def) != null;
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x000993FD File Offset: 0x000975FD
		public void AddAttachment(AttachableThing t)
		{
			if (this.attachments == null)
			{
				this.attachments = new List<AttachableThing>();
			}
			this.attachments.Add(t);
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x0009941E File Offset: 0x0009761E
		public void RemoveAttachment(AttachableThing t)
		{
			this.attachments.Remove(t);
		}

		// Token: 0x0400112A RID: 4394
		public List<AttachableThing> attachments;
	}
}
