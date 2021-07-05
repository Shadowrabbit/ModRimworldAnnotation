using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A5B RID: 2651
	public class ComplexThreatDef : Def
	{
		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06003FC1 RID: 16321 RVA: 0x0015A0E9 File Offset: 0x001582E9
		public ComplexThreatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (ComplexThreatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x0015A11B File Offset: 0x0015831B
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.delayChance > 0f && this.delayTickOptions == null)
			{
				yield return "Chance to have a delayed threat is > 0 but no signal delay tick options are set.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04002376 RID: 9078
		public Type workerClass = typeof(ComplexThreatWorker);

		// Token: 0x04002377 RID: 9079
		public FactionDef faction;

		// Token: 0x04002378 RID: 9080
		public float postSpawnPassiveThreatFactor = 1f;

		// Token: 0x04002379 RID: 9081
		public int minPoints;

		// Token: 0x0400237A RID: 9082
		public float spawnInOtherRoomChance;

		// Token: 0x0400237B RID: 9083
		public bool allowPassive = true;

		// Token: 0x0400237C RID: 9084
		public bool fallbackToRoomEnteredTrigger = true;

		// Token: 0x0400237D RID: 9085
		public float delayChance;

		// Token: 0x0400237E RID: 9086
		public List<int> delayTickOptions;

		// Token: 0x0400237F RID: 9087
		public SimpleCurve threatFactorOverDelayTicksCurve;

		// Token: 0x04002380 RID: 9088
		public SignalActionAmbushType signalActionAmbushType;

		// Token: 0x04002381 RID: 9089
		public bool spawnAroundComplex;

		// Token: 0x04002382 RID: 9090
		public bool useDropPods;

		// Token: 0x04002383 RID: 9091
		[Unsaved(false)]
		private ComplexThreatWorker workerInt;
	}
}
