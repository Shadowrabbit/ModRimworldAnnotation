using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000393 RID: 915
	public static class DelayedErrorWindowRequest
	{
		// Token: 0x06001AE0 RID: 6880 RVA: 0x0009C448 File Offset: 0x0009A648
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

		// Token: 0x06001AE1 RID: 6881 RVA: 0x0009C4D0 File Offset: 0x0009A6D0
		public static void Add(string text, string title = null)
		{
			DelayedErrorWindowRequest.Request item = default(DelayedErrorWindowRequest.Request);
			item.text = text;
			item.title = title;
			DelayedErrorWindowRequest.requests.Add(item);
		}

		// Token: 0x04001180 RID: 4480
		private static List<DelayedErrorWindowRequest.Request> requests = new List<DelayedErrorWindowRequest.Request>();

		// Token: 0x02001A93 RID: 6803
		private struct Request
		{
			// Token: 0x040065B8 RID: 26040
			public string text;

			// Token: 0x040065B9 RID: 26041
			public string title;
		}
	}
}
