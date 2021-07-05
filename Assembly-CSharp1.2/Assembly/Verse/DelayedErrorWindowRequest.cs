using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000540 RID: 1344
	public static class DelayedErrorWindowRequest
	{
		// Token: 0x06002281 RID: 8833 RVA: 0x0010A2FC File Offset: 0x001084FC
		public static void DelayedErrorWindowRequestOnGUI()
		{
			try
			{
				for (int i = 0; i < DelayedErrorWindowRequest.requests.Count; i++)
				{
					Find.WindowStack.Add(new Dialog_MessageBox(DelayedErrorWindowRequest.requests[i].text, "OK".Translate(), null, null, null, DelayedErrorWindowRequest.requests[i].title, false, null, null));
				}
			}
			finally
			{
				DelayedErrorWindowRequest.requests.Clear();
			}
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x0010A384 File Offset: 0x00108584
		public static void Add(string text, string title = null)
		{
			DelayedErrorWindowRequest.Request item = default(DelayedErrorWindowRequest.Request);
			item.text = text;
			item.title = title;
			DelayedErrorWindowRequest.requests.Add(item);
		}

		// Token: 0x04001769 RID: 5993
		private static List<DelayedErrorWindowRequest.Request> requests = new List<DelayedErrorWindowRequest.Request>();

		// Token: 0x02000541 RID: 1345
		private struct Request
		{
			// Token: 0x0400176A RID: 5994
			public string text;

			// Token: 0x0400176B RID: 5995
			public string title;
		}
	}
}
