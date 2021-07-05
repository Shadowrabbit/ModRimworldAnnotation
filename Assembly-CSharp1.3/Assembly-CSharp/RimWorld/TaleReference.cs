using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102C RID: 4140
	public class TaleReference : IExposable
	{
		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x060061C5 RID: 25029 RVA: 0x00213CC7 File Offset: 0x00211EC7
		public static TaleReference Taleless
		{
			get
			{
				return new TaleReference(null);
			}
		}

		// Token: 0x060061C6 RID: 25030 RVA: 0x000033AC File Offset: 0x000015AC
		public TaleReference()
		{
		}

		// Token: 0x060061C7 RID: 25031 RVA: 0x00213CCF File Offset: 0x00211ECF
		public TaleReference(Tale tale)
		{
			this.tale = tale;
			this.seed = Rand.Range(0, int.MaxValue);
		}

		// Token: 0x060061C8 RID: 25032 RVA: 0x00213CEF File Offset: 0x00211EEF
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
			Scribe_References.Look<Tale>(ref this.tale, "tale", false);
		}

		// Token: 0x060061C9 RID: 25033 RVA: 0x00213D14 File Offset: 0x00211F14
		public void ReferenceDestroyed()
		{
			if (this.tale != null)
			{
				this.tale.Notify_ReferenceDestroyed();
				this.tale = null;
			}
		}

		// Token: 0x060061CA RID: 25034 RVA: 0x00213D30 File Offset: 0x00211F30
		public TaggedString GenerateText(TextGenerationPurpose purpose, RulePackDef extraInclude)
		{
			return TaleTextGenerator.GenerateTextFromTale(purpose, this.tale, this.seed, extraInclude);
		}

		// Token: 0x060061CB RID: 25035 RVA: 0x00213D48 File Offset: 0x00211F48
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

		// Token: 0x040037BE RID: 14270
		private Tale tale;

		// Token: 0x040037BF RID: 14271
		private int seed;
	}
}
