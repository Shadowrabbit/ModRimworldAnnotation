using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE9 RID: 2793
	public class WeaponTraitDef : Def
	{
		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x060041BA RID: 16826 RVA: 0x001603F7 File Offset: 0x0015E5F7
		public WeaponTraitWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (WeaponTraitWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x0016042C File Offset: 0x0015E62C
		public bool Overlaps(WeaponTraitDef other)
		{
			return other == this || (!this.exclusionTags.NullOrEmpty<string>() && !other.exclusionTags.NullOrEmpty<string>() && this.exclusionTags.Any((string x) => other.exclusionTags.Contains(x)));
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x00160489 File Offset: 0x0015E689
		public override IEnumerable<string> ConfigErrors()
		{
			if (!typeof(WeaponTraitWorker).IsAssignableFrom(this.workerClass))
			{
				yield return string.Format("WeaponTraitDef {0} has worker class {1}, which is not deriving from {2}", this.defName, this.workerClass, typeof(WeaponTraitWorker).FullName);
			}
			if (this.commonality <= 0f)
			{
				yield return string.Format("WeaponTraitDef {0} has a commonality <= 0.", this.defName);
			}
			yield break;
		}

		// Token: 0x04002803 RID: 10243
		public List<StatModifier> equippedStatOffsets;

		// Token: 0x04002804 RID: 10244
		public List<HediffDef> equippedHediffs;

		// Token: 0x04002805 RID: 10245
		public List<HediffDef> bondedHediffs;

		// Token: 0x04002806 RID: 10246
		public ThoughtDef bondedThought;

		// Token: 0x04002807 RID: 10247
		public ThoughtDef killThought;

		// Token: 0x04002808 RID: 10248
		public Type workerClass = typeof(WeaponTraitWorker);

		// Token: 0x04002809 RID: 10249
		public List<string> exclusionTags;

		// Token: 0x0400280A RID: 10250
		public float commonality;

		// Token: 0x0400280B RID: 10251
		public float marketValueOffset;

		// Token: 0x0400280C RID: 10252
		public bool neverBond;

		// Token: 0x0400280D RID: 10253
		private WeaponTraitWorker worker;
	}
}
