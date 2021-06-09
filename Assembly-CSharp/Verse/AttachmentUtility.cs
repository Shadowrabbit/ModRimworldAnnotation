using System;

namespace Verse
{
	// Token: 0x02000523 RID: 1315
	public static class AttachmentUtility
	{
		// Token: 0x060021BD RID: 8637 RVA: 0x00107914 File Offset: 0x00105B14
		public static Thing GetAttachment(this Thing t, ThingDef def)
		{
			CompAttachBase compAttachBase = t.TryGetComp<CompAttachBase>();
			if (compAttachBase == null)
			{
				return null;
			}
			return compAttachBase.GetAttachment(def);
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x0001D31C File Offset: 0x0001B51C
		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}
