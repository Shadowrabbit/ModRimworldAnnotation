using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001648 RID: 5704
	public class TaleReference : IExposable
	{
		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x06007C03 RID: 31747 RVA: 0x000533E9 File Offset: 0x000515E9
		public static TaleReference Taleless
		{
			get
			{
				return new TaleReference(null);
			}
		}

		// Token: 0x06007C04 RID: 31748 RVA: 0x00006B8B File Offset: 0x00004D8B
		public TaleReference()
		{
		}

		// Token: 0x06007C05 RID: 31749 RVA: 0x000533F1 File Offset: 0x000515F1
		public TaleReference(Tale tale)
		{
			this.tale = tale;
			this.seed = Rand.Range(0, int.MaxValue);
		}

		// Token: 0x06007C06 RID: 31750 RVA: 0x00053411 File Offset: 0x00051611
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
			Scribe_References.Look<Tale>(ref this.tale, "tale", false);
		}

		// Token: 0x06007C07 RID: 31751 RVA: 0x00053436 File Offset: 0x00051636
		public void ReferenceDestroyed()
		{
			if (this.tale != null)
			{
				this.tale.Notify_ReferenceDestroyed();
				this.tale = null;
			}
		}

		// Token: 0x06007C08 RID: 31752 RVA: 0x00053452 File Offset: 0x00051652
		public TaggedString GenerateText(TextGenerationPurpose purpose, RulePackDef extraInclude)
		{
			return TaleTextGenerator.GenerateTextFromTale(purpose, this.tale, this.seed, extraInclude);
		}

		// Token: 0x06007C09 RID: 31753 RVA: 0x00253254 File Offset: 0x00251454
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"TaleReference(tale=",
				(this.tale == null) ? "null" : this.tale.ToString(),
				", seed=",
				this.seed,
				")"
			});
		}

		// Token: 0x04005143 RID: 20803
		private Tale tale;

		// Token: 0x04005144 RID: 20804
		private int seed;
	}
}
