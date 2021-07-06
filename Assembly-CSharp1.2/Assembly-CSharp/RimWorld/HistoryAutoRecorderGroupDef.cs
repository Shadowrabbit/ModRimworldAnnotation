using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA2 RID: 4002
	public class HistoryAutoRecorderGroupDef : Def
	{
		// Token: 0x060057A5 RID: 22437 RVA: 0x0003CC64 File Offset: 0x0003AE64
		public static HistoryAutoRecorderGroupDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderGroupDef>.GetNamed(defName, true);
		}

		// Token: 0x04003954 RID: 14676
		public bool useFixedScale;

		// Token: 0x04003955 RID: 14677
		public Vector2 fixedScale;

		// Token: 0x04003956 RID: 14678
		public bool integersOnly;

		// Token: 0x04003957 RID: 14679
		public bool onlyPositiveValues = true;

		// Token: 0x04003958 RID: 14680
		public bool devModeOnly;

		// Token: 0x04003959 RID: 14681
		public List<HistoryAutoRecorderDef> historyAutoRecorderDefs = new List<HistoryAutoRecorderDef>();
	}
}
