using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200014F RID: 335
	public class MentalBreakDef : Def
	{
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0000CCC0 File Offset: 0x0000AEC0
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

		// Token: 0x0600089A RID: 2202 RVA: 0x0000CD00 File Offset: 0x0000AF00
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

		// Token: 0x040006D8 RID: 1752
		public Type workerClass = typeof(MentalBreakWorker);

		// Token: 0x040006D9 RID: 1753
		public MentalStateDef mentalState;

		// Token: 0x040006DA RID: 1754
		public float baseCommonality;

		// Token: 0x040006DB RID: 1755
		public SimpleCurve commonalityFactorPerPopulationCurve;

		// Token: 0x040006DC RID: 1756
		public MentalBreakIntensity intensity;

		// Token: 0x040006DD RID: 1757
		public TraitDef requiredTrait;

		// Token: 0x040006DE RID: 1758
		private MentalBreakWorker workerInt;
	}
}
