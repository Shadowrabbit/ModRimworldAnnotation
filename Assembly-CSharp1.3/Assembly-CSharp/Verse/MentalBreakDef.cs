using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000DE RID: 222
	public class MentalBreakDef : Def
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x0001EE62 File Offset: 0x0001D062
		public MentalBreakWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalBreakWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001EEA2 File Offset: 0x0001D0A2
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.intensity == MentalBreakIntensity.None)
			{
				yield return "intensity not set";
			}
			yield break;
			yield break;
		}

		// Token: 0x040004DC RID: 1244
		public Type workerClass = typeof(MentalBreakWorker);

		// Token: 0x040004DD RID: 1245
		public MentalStateDef mentalState;

		// Token: 0x040004DE RID: 1246
		public float baseCommonality;

		// Token: 0x040004DF RID: 1247
		public SimpleCurve commonalityFactorPerPopulationCurve;

		// Token: 0x040004E0 RID: 1248
		public MentalBreakIntensity intensity;

		// Token: 0x040004E1 RID: 1249
		public TraitDef requiredTrait;

		// Token: 0x040004E2 RID: 1250
		private MentalBreakWorker workerInt;
	}
}
