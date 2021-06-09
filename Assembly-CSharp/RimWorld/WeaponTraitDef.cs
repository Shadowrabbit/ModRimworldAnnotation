using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100D RID: 4109
	public class WeaponTraitDef : Def
	{
		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x060059A9 RID: 22953 RVA: 0x0003E443 File Offset: 0x0003C643
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

		// Token: 0x060059AA RID: 22954 RVA: 0x001D2E88 File Offset: 0x001D1088
		public bool Overlaps(WeaponTraitDef other)
		{
			return other == this || (!this.exclusionTags.NullOrEmpty<string>() && !other.exclusionTags.NullOrEmpty<string>() && this.exclusionTags.Any((string x) => other.exclusionTags.Contains(x)));
		}

		// Token: 0x060059AB RID: 22955 RVA: 0x0003E475 File Offset: 0x0003C675
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

		// Token: 0x04003C55 RID: 15445
		public List<StatModifier> equippedStatOffsets;

		// Token: 0x04003C56 RID: 15446
		public List<HediffDef> equippedHediffs;

		// Token: 0x04003C57 RID: 15447
		public List<HediffDef> bondedHediffs;

		// Token: 0x04003C58 RID: 15448
		public ThoughtDef bondedThought;

		// Token: 0x04003C59 RID: 15449
		public ThoughtDef killThought;

		// Token: 0x04003C5A RID: 15450
		public Type workerClass = typeof(WeaponTraitWorker);

		// Token: 0x04003C5B RID: 15451
		public List<string> exclusionTags;

		// Token: 0x04003C5C RID: 15452
		public float commonality;

		// Token: 0x04003C5D RID: 15453
		public float marketValueOffset;

		// Token: 0x04003C5E RID: 15454
		public bool neverBond;

		// Token: 0x04003C5F RID: 15455
		private WeaponTraitWorker worker;
	}
}
