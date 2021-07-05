using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FAF RID: 4015
	public class JoyGiverDef : Def
	{
		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x060057D6 RID: 22486 RVA: 0x0003CEB1 File Offset: 0x0003B0B1
		public JoyGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (JoyGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060057D7 RID: 22487 RVA: 0x0003CEE3 File Offset: 0x0003B0E3
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.jobDef != null && this.jobDef.joyKind != this.joyKind)
			{
				yield return string.Concat(new object[]
				{
					"jobDef ",
					this.jobDef,
					" has joyKind ",
					this.jobDef.joyKind,
					" which does not match our joyKind ",
					this.joyKind
				});
			}
			yield break;
			yield break;
		}

		// Token: 0x040039BD RID: 14781
		public Type giverClass;

		// Token: 0x040039BE RID: 14782
		public float baseChance;

		// Token: 0x040039BF RID: 14783
		public bool requireChair = true;

		// Token: 0x040039C0 RID: 14784
		public List<ThingDef> thingDefs;

		// Token: 0x040039C1 RID: 14785
		public JobDef jobDef;

		// Token: 0x040039C2 RID: 14786
		public bool desireSit = true;

		// Token: 0x040039C3 RID: 14787
		public float pctPawnsEverDo = 1f;

		// Token: 0x040039C4 RID: 14788
		public bool unroofedOnly;

		// Token: 0x040039C5 RID: 14789
		public JoyKindDef joyKind;

		// Token: 0x040039C6 RID: 14790
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x040039C7 RID: 14791
		public bool canDoWhileInBed;

		// Token: 0x040039C8 RID: 14792
		private JoyGiver workerInt;
	}
}
