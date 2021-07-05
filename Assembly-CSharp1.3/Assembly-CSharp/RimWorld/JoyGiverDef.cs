using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A86 RID: 2694
	public class JoyGiverDef : Def
	{
		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06004057 RID: 16471 RVA: 0x0015C33B File Offset: 0x0015A53B
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

		// Token: 0x06004058 RID: 16472 RVA: 0x0015C36D File Offset: 0x0015A56D
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

		// Token: 0x040024DB RID: 9435
		public Type giverClass;

		// Token: 0x040024DC RID: 9436
		public float baseChance;

		// Token: 0x040024DD RID: 9437
		public bool requireChair = true;

		// Token: 0x040024DE RID: 9438
		public List<ThingDef> thingDefs;

		// Token: 0x040024DF RID: 9439
		public JobDef jobDef;

		// Token: 0x040024E0 RID: 9440
		public bool desireSit = true;

		// Token: 0x040024E1 RID: 9441
		public float pctPawnsEverDo = 1f;

		// Token: 0x040024E2 RID: 9442
		public bool unroofedOnly;

		// Token: 0x040024E3 RID: 9443
		public JoyKindDef joyKind;

		// Token: 0x040024E4 RID: 9444
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x040024E5 RID: 9445
		public bool canDoWhileInBed;

		// Token: 0x040024E6 RID: 9446
		private JoyGiver workerInt;
	}
}
