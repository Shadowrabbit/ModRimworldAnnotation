using System;

namespace Verse
{
	// Token: 0x02000380 RID: 896
	public static class AttachmentUtility
	{
		// Token: 0x06001A4C RID: 6732 RVA: 0x00099430 File Offset: 0x00097630
		public static Thing GetAttachment(this Thing t, ThingDef def)
		{
			CompAttachBase compAttachBase = t.TryGetComp<CompAttachBase>();
			if (compAttachBase == null)
			{
				return null;
			}
			return compAttachBase.GetAttachment(def);
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x00099450 File Offset: 0x00097650
		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}
