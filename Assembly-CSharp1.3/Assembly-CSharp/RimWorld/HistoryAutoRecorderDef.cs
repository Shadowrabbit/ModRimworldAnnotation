using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A72 RID: 2674
	public class HistoryAutoRecorderDef : Def
	{
		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06004020 RID: 16416 RVA: 0x0015B4EA File Offset: 0x001596EA
		public HistoryAutoRecorderWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (HistoryAutoRecorderWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x0015B510 File Offset: 0x00159710
		public static HistoryAutoRecorderDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderDef>.GetNamed(defName, true);
		}

		// Token: 0x04002463 RID: 9315
		public Type workerClass;

		// Token: 0x04002464 RID: 9316
		public int recordTicksFrequency = 60000;

		// Token: 0x04002465 RID: 9317
		public Color graphColor = Color.green;

		// Token: 0x04002466 RID: 9318
		[MustTranslate]
		public string graphLabelY;

		// Token: 0x04002467 RID: 9319
		public string valueFormat;

		// Token: 0x04002468 RID: 9320
		[Unsaved(false)]
		private HistoryAutoRecorderWorker workerInt;
	}
}
