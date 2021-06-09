using System;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000383 RID: 899
	public class PawnUIOverlay
	{
		// Token: 0x0600168C RID: 5772 RVA: 0x00015F7B File Offset: 0x0001417B
		public PawnUIOverlay(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x000D70E4 File Offset: 0x000D52E4
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

		// Token: 0x04001154 RID: 4436
		private Pawn pawn;

		// Token: 0x04001155 RID: 4437
		private const float PawnLabelOffsetY = -0.6f;

		// Token: 0x04001156 RID: 4438
		private const int PawnStatBarWidth = 32;

		// Token: 0x04001157 RID: 4439
		private const float ActivityIconSize = 13f;

		// Token: 0x04001158 RID: 4440
		private const float ActivityIconOffsetY = 12f;
	}
}
