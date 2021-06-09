using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B1C RID: 6940
	public class ITab_Pawn_Training : ITab
	{
		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x060098AD RID: 39085 RVA: 0x00065C44 File Offset: 0x00063E44
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.training != null && base.SelPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x060098AE RID: 39086 RVA: 0x00065C67 File Offset: 0x00063E67
		public ITab_Pawn_Training()
		{
			this.labelKey = "TabTraining";
			this.tutorTag = "Training";
		}

		// Token: 0x060098AF RID: 39087 RVA: 0x002CE164 File Offset: 0x002CC364
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(17f);
			rect.yMin += 10f;
			TrainingCardUtility.DrawTrainingCard(rect, base.SelPawn);
		}

		// Token: 0x060098B0 RID: 39088 RVA: 0x00065C85 File Offset: 0x00063E85
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = new Vector2(300f, TrainingCardUtility.TotalHeightForPawn(base.SelPawn));
		}
	}
}
