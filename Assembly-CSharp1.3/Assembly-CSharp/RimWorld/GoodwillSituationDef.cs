using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A6F RID: 2671
	public class GoodwillSituationDef : Def
	{
		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x0600401C RID: 16412 RVA: 0x0015B498 File Offset: 0x00159698
		public GoodwillSituationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (GoodwillSituationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x0400245B RID: 9307
		public Type workerClass = typeof(GoodwillSituationWorker);

		// Token: 0x0400245C RID: 9308
		public int baseMaxGoodwill = 100;

		// Token: 0x0400245D RID: 9309
		public MemeDef meme;

		// Token: 0x0400245E RID: 9310
		public MemeDef otherMeme;

		// Token: 0x0400245F RID: 9311
		public int naturalGoodwillOffset;

		// Token: 0x04002460 RID: 9312
		public bool versusAll;

		// Token: 0x04002461 RID: 9313
		[Unsaved(false)]
		private GoodwillSituationWorker workerInt;
	}
}
