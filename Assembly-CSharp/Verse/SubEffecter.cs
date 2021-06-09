using System;

namespace Verse
{
	// Token: 0x02000810 RID: 2064
	public class SubEffecter
	{
		// Token: 0x060033F1 RID: 13297 RVA: 0x00028B5B File Offset: 0x00026D5B
		public SubEffecter(SubEffecterDef subDef, Effecter parent)
		{
			this.def = subDef;
			this.parent = parent;
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void SubEffectTick(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void SubTrigger(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void SubCleanup()
		{
		}

		// Token: 0x04002401 RID: 9217
		public Effecter parent;

		// Token: 0x04002402 RID: 9218
		public SubEffecterDef def;
	}
}
