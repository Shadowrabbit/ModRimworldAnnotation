using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA0 RID: 4000
	public class HistoryAutoRecorderDef : Def
	{
		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x060057A0 RID: 22432 RVA: 0x0003CC17 File Offset: 0x0003AE17
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

		// Token: 0x060057A1 RID: 22433 RVA: 0x0003CC3D File Offset: 0x0003AE3D
		public static HistoryAutoRecorderDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderDef>.GetNamed(defName, true);
		}

		// Token: 0x0400394E RID: 14670
		public Type workerClass;

		// Token: 0x0400394F RID: 14671
		public int recordTicksFrequency = 60000;

		// Token: 0x04003950 RID: 14672
		public Color graphColor = Color.green;

		// Token: 0x04003951 RID: 14673
		[MustTranslate]
		public string graphLabelY;

		// Token: 0x04003952 RID: 14674
		public string valueFormat;

		// Token: 0x04003953 RID: 14675
		[Unsaved(false)]
		private HistoryAutoRecorderWorker workerInt;
	}
}
