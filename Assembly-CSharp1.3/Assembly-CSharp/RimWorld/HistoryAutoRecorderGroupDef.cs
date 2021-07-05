using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A74 RID: 2676
	public class HistoryAutoRecorderGroupDef : Def
	{
		// Token: 0x06004025 RID: 16421 RVA: 0x0015B537 File Offset: 0x00159737
		public static HistoryAutoRecorderGroupDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderGroupDef>.GetNamed(defName, true);
		}

		// Token: 0x04002469 RID: 9321
		public bool useFixedScale;

		// Token: 0x0400246A RID: 9322
		public Vector2 fixedScale;

		// Token: 0x0400246B RID: 9323
		public bool integersOnly;

		// Token: 0x0400246C RID: 9324
		public bool onlyPositiveValues = true;

		// Token: 0x0400246D RID: 9325
		public bool devModeOnly;

		// Token: 0x0400246E RID: 9326
		public List<HistoryAutoRecorderDef> historyAutoRecorderDefs = new List<HistoryAutoRecorderDef>();
	}
}
