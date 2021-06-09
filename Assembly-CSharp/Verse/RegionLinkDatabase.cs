using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x020002DD RID: 733
	public class RegionLinkDatabase
	{
		// Token: 0x060012AC RID: 4780 RVA: 0x000C7A64 File Offset: 0x000C5C64
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

		// Token: 0x060012AD RID: 4781 RVA: 0x0001367F File Offset: 0x0001187F
		public void Notify_LinkHasNoRegions(RegionLink link)
		{
			this.links.Remove(link.UniqueHashCode());
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x000C7AA4 File Offset: 0x000C5CA4
		public void DebugLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ulong, RegionLink> keyValuePair in this.links)
			{
				stringBuilder.AppendLine(keyValuePair.ToString());
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04000EF7 RID: 3831
		private Dictionary<ulong, RegionLink> links = new Dictionary<ulong, RegionLink>();
	}
}
