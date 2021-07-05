using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000436 RID: 1078
	public static class OriginalEventUtility
	{
		// Token: 0x0600206D RID: 8301 RVA: 0x000C8F6C File Offset: 0x000C716C
		public static void RecordOriginalEvent(Event e)
		{
			OriginalEventUtility.originalType = new EventType?(e.type);
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x0600206E RID: 8302 RVA: 0x000C8F80 File Offset: 0x000C7180
		public static EventType EventType
		{
			get
			{
				EventType? eventType = OriginalEventUtility.originalType;
				if (eventType == null)
				{
					return Event.current.rawType;
				}
				return eventType.GetValueOrDefault();
			}
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x000C8FAE File Offset: 0x000C71AE
		public static void Reset()
		{
			OriginalEventUtility.originalType = null;
		}

		// Token: 0x040013AD RID: 5037
		private static EventType? originalType;
	}
}
