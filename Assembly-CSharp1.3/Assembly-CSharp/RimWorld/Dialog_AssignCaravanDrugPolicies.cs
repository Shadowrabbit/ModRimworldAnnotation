using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012D9 RID: 4825
	public class Dialog_AssignCaravanDrugPolicies : Window
	{
		// Token: 0x17001433 RID: 5171
		// (get) Token: 0x0600734E RID: 29518 RVA: 0x00268451 File Offset: 0x00266651
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(550f, 500f);
			}
		}

		// Token: 0x0600734F RID: 29519 RVA: 0x00268462 File Offset: 0x00266662
		public Dialog_AssignCaravanDrugPolicies(Caravan caravan)
		{
			this.caravan = caravan;
			this.doCloseButton = true;
		}

		// Token: 0x06007350 RID: 29520 RVA: 0x00268478 File Offset: 0x00266678
		public override void DoWindowContents(Rect rect)
		{
			rect.height -= Window.CloseButSize.y;
			float num = 0f;
			if (Widgets.ButtonText(new Rect(rect.width - 354f - 16f, num, 354f, 32f), "ManageDrugPolicies".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(null));
			}
			num += 42f;
			Rect outRect = new Rect(0f, num, rect.width, rect.height - num);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, this.lastHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPos, viewRect, true);
			float num2 = 0f;
			for (int i = 0; i < this.caravan.pawns.Count; i++)
			{
				if (this.caravan.pawns[i].drugs != null)
				{
					if (num2 + 30f >= this.scrollPos.y && num2 <= this.scrollPos.y + outRect.height)
					{
						this.DoRow(new Rect(0f, num2, viewRect.width, 30f), this.caravan.pawns[i]);
					}
					num2 += 30f;
				}
			}
			this.lastHeight = num2;
			Widgets.EndScrollView();
		}

		// Token: 0x06007351 RID: 29521 RVA: 0x002685F8 File Offset: 0x002667F8
		private void DoRow(Rect rect, Pawn pawn)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width - 354f, 30f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect2, pawn.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			DrugPolicyUIUtility.DoAssignDrugPolicyButtons(new Rect(rect.x + rect.width - 354f, rect.y, 354f, 30f), pawn);
		}

		// Token: 0x04003EE6 RID: 16102
		private Caravan caravan;

		// Token: 0x04003EE7 RID: 16103
		private Vector2 scrollPos;

		// Token: 0x04003EE8 RID: 16104
		private float lastHeight;

		// Token: 0x04003EE9 RID: 16105
		private const float RowHeight = 30f;

		// Token: 0x04003EEA RID: 16106
		private const float AssignDrugPolicyButtonsTotalWidth = 354f;

		// Token: 0x04003EEB RID: 16107
		private const int ManageDrugPoliciesButtonHeight = 32;
	}
}
