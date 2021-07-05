using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000205 RID: 517
	public class RegionLinkDatabase
	{
		// Token: 0x06000EA5 RID: 3749 RVA: 0x000531D4 File Offset: 0x000513D4
		public RegionLink LinkFrom(EdgeSpan span)
		{
			ulong key = span.UniqueHashCode();
			RegionLink regionLink;
			if (!this.links.TryGetValue(key, out regionLink))
			{
				regionLink = new RegionLink();
				regionLink.span = span;
				this.links.Add(key, regionLink);
			}
			return regionLink;
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00053214 File Offset: 0x00051414
		public void Notify_LinkHasNoRegions(RegionLink link)
		{
			this.links.Remove(link.UniqueHashCode());
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00053228 File Offset: 0x00051428
		public void DebugLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ulong, RegionLink> keyValuePair in this.links)
			{
				stringBuilder.AppendLine(keyValuePair.ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04000BCF RID: 3023
		private Dictionary<ulong, RegionLink> links = new Dictionary<ulong, RegionLink>();
	}
}
