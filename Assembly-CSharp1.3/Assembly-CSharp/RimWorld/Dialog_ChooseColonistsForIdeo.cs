using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012DD RID: 4829
	public class Dialog_ChooseColonistsForIdeo : Window
	{
		// Token: 0x1700143B RID: 5179
		// (get) Token: 0x0600738A RID: 29578 RVA: 0x0026CE50 File Offset: 0x0026B050
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight - 17f);
			}
		}

		// Token: 0x0600738B RID: 29579 RVA: 0x0026CE68 File Offset: 0x0026B068
		public Dialog_ChooseColonistsForIdeo(Ideo ideo, IEnumerable<Pawn> pawns, Action<List<Pawn>> chosen)
		{
			this.pawnsTransfer = new TransferableOneWayWidget(null, null, null, null, false, IgnorePawnsInventoryMode.DontIgnore, false, null, 0f, false, -1, false, false, false, false, false, false, false, true);
			this.forcePause = true;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
			this.ideo = ideo;
			this.pawns.AddRange(pawns);
			this.chosen = chosen;
		}

		// Token: 0x0600738C RID: 29580 RVA: 0x0026CEF8 File Offset: 0x0026B0F8
		public override void PostOpen()
		{
			foreach (Pawn thing in this.pawns)
			{
				this.AddTransferable(thing);
			}
			this.pawnsTransfer.AddSection("ColonistsSection".Translate(), this.transferables);
		}

		// Token: 0x0600738D RID: 29581 RVA: 0x0026CF6C File Offset: 0x0026B16C
		private void AddTransferable(Thing thing)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(thing, this.transferables, TransferAsOneMode.Normal);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			transferableOneWay.things.Add(thing);
		}

		// Token: 0x0600738E RID: 29582 RVA: 0x0026CFA8 File Offset: 0x0026B1A8
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 50f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "ChooseColonistsForIdeo".Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			inRect.yMin += 82f;
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			Rect inRect2 = rect2;
			inRect2.yMax -= 21f;
			bool flag;
			this.pawnsTransfer.OnGUI(inRect2, out flag);
			if (Widgets.ButtonText(new Rect(rect2.width / 2f - this.BottomButtonSize.x / 2f, rect2.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y), "AcceptButton".Translate(), true, true, true) && this.TryAccept())
			{
				this.Close(true);
			}
			GUI.EndGroup();
		}

		// Token: 0x0600738F RID: 29583 RVA: 0x0026D0B0 File Offset: 0x0026B2B0
		private bool TryAccept()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			this.chosen(pawnsFromTransferables);
			return true;
		}

		// Token: 0x04003F38 RID: 16184
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x04003F39 RID: 16185
		private List<TransferableOneWay> transferables = new List<TransferableOneWay>();

		// Token: 0x04003F3A RID: 16186
		private Ideo ideo;

		// Token: 0x04003F3B RID: 16187
		private List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003F3C RID: 16188
		private Action<List<Pawn>> chosen;

		// Token: 0x04003F3D RID: 16189
		private const float TitleRectHeight = 50f;

		// Token: 0x04003F3E RID: 16190
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04003F3F RID: 16191
		private const float BottomAreaHeight = 55f;
	}
}
