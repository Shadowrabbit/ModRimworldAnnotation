using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A9D RID: 2717
	public class PawnGroupKindDef : Def
	{
		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x060040A6 RID: 16550 RVA: 0x0015D940 File Offset: 0x0015BB40
		public PawnGroupKindWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnGroupKindWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x0400258B RID: 9611
		public Type workerClass = typeof(PawnGroupKindWorker);

		// Token: 0x0400258C RID: 9612
		[Unsaved(false)]
		private PawnGroupKindWorker workerInt;
	}
}
