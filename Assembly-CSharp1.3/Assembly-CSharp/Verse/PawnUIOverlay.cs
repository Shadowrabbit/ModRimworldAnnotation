using System;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200025E RID: 606
	public class PawnUIOverlay
	{
		// Token: 0x06001133 RID: 4403 RVA: 0x00061CFC File Offset: 0x0005FEFC
		public PawnUIOverlay(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x00061D0C File Offset: 0x0005FF0C
		public void DrawPawnGUIOverlay()
		{
			if (!this.pawn.Spawned || this.pawn.Map.fogGrid.IsFogged(this.pawn.Position))
			{
				return;
			}
			if (!this.pawn.RaceProps.Humanlike)
			{
				switch (Prefs.AnimalNameMode)
				{
				case AnimalNameDisplayMode.None:
					return;
				case AnimalNameDisplayMode.TameNamed:
					if (this.pawn.Name == null || this.pawn.Name.Numerical)
					{
						return;
					}
					break;
				case AnimalNameDisplayMode.TameAll:
					if (this.pawn.Name == null)
					{
						return;
					}
					break;
				}
			}
			Vector2 pos = GenMapUI.LabelDrawPosFor(this.pawn, -0.6f);
			GenMapUI.DrawPawnLabel(this.pawn, pos, 1f, 9999f, null, GameFont.Tiny, true, true);
			if (this.pawn.CanTradeNow)
			{
				this.pawn.Map.overlayDrawer.DrawOverlay(this.pawn, OverlayTypes.QuestionMark);
			}
			Lord lord = this.pawn.GetLord();
			if (lord != null && lord.CurLordToil != null)
			{
				lord.CurLordToil.DrawPawnGUIOverlay(this.pawn);
			}
		}

		// Token: 0x04000D17 RID: 3351
		private Pawn pawn;

		// Token: 0x04000D18 RID: 3352
		private const float PawnLabelOffsetY = -0.6f;

		// Token: 0x04000D19 RID: 3353
		private const int PawnStatBarWidth = 32;

		// Token: 0x04000D1A RID: 3354
		private const float ActivityIconSize = 13f;

		// Token: 0x04000D1B RID: 3355
		private const float ActivityIconOffsetY = 12f;
	}
}
