using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC5 RID: 4037
	public class PawnsArrivalModeDef : Def
	{
		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x0600584A RID: 22602 RVA: 0x0003D55D File Offset: 0x0003B75D
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

		// Token: 0x04003A4E RID: 14926
		public Type workerClass = typeof(PawnsArrivalModeWorker);

		// Token: 0x04003A4F RID: 14927
		public SimpleCurve selectionWeightCurve;

		// Token: 0x04003A50 RID: 14928
		public SimpleCurve pointsFactorCurve;

		// Token: 0x04003A51 RID: 14929
		public TechLevel minTechLevel;

		// Token: 0x04003A52 RID: 14930
		public bool forQuickMilitaryAid;

		// Token: 0x04003A53 RID: 14931
		public bool walkIn;

		// Token: 0x04003A54 RID: 14932
		[MustTranslate]
		public string textEnemy;

		// Token: 0x04003A55 RID: 14933
		[MustTranslate]
		public string textFriendly;

		// Token: 0x04003A56 RID: 14934
		[MustTranslate]
		public string textWillArrive;

		// Token: 0x04003A57 RID: 14935
		[Unsaved(false)]
		private PawnsArrivalModeWorker workerInt;
	}
}
