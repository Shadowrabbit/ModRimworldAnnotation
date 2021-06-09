using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC0 RID: 4032
	public class PawnGroupKindDef : Def
	{
		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06005829 RID: 22569 RVA: 0x0003D35B File Offset: 0x0003B55B
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

		// Token: 0x04003A32 RID: 14898
		public Type workerClass = typeof(PawnGroupKindWorker);

		// Token: 0x04003A33 RID: 14899
		[Unsaved(false)]
		private PawnGroupKindWorker workerInt;
	}
}
