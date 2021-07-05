using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200134E RID: 4942
	public class ITab_Pawn_Training : ITab
	{
		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x060077A6 RID: 30630 RVA: 0x002A2558 File Offset: 0x002A0758
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.training != null && base.SelPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x060077A7 RID: 30631 RVA: 0x002A257B File Offset: 0x002A077B
		public ITab_Pawn_Training()
		{
			this.labelKey = "TabTraining";
			this.tutorTag = "Training";
		}

		// Token: 0x060077A8 RID: 30632 RVA: 0x002A259C File Offset: 0x002A079C
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(17f);
			rect.yMin += 10f;
			TrainingCardUtility.DrawTrainingCard(rect, base.SelPawn);
		}

		// Token: 0x060077A9 RID: 30633 RVA: 0x002A25F8 File Offset: 0x002A07F8
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = new Vector2(300f, TrainingCardUtility.TotalHeightForPawn(base.SelPawn));
		}
	}
}
