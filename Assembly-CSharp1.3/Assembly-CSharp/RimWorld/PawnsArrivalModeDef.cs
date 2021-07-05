using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA2 RID: 2722
	public class PawnsArrivalModeDef : Def
	{
		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x060040C0 RID: 16576 RVA: 0x0015DCD5 File Offset: 0x0015BED5
		public PawnsArrivalModeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnsArrivalModeWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x040025A5 RID: 9637
		public Type workerClass = typeof(PawnsArrivalModeWorker);

		// Token: 0x040025A6 RID: 9638
		public SimpleCurve selectionWeightCurve;

		// Token: 0x040025A7 RID: 9639
		public SimpleCurve pointsFactorCurve;

		// Token: 0x040025A8 RID: 9640
		public TechLevel minTechLevel;

		// Token: 0x040025A9 RID: 9641
		public bool forQuickMilitaryAid;

		// Token: 0x040025AA RID: 9642
		public bool walkIn;

		// Token: 0x040025AB RID: 9643
		[MustTranslate]
		public string textEnemy;

		// Token: 0x040025AC RID: 9644
		[MustTranslate]
		public string textFriendly;

		// Token: 0x040025AD RID: 9645
		[MustTranslate]
		public string textWillArrive;

		// Token: 0x040025AE RID: 9646
		[Unsaved(false)]
		private PawnsArrivalModeWorker workerInt;
	}
}
