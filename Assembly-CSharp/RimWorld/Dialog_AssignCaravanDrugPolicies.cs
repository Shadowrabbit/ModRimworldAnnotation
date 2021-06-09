using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019DA RID: 6618
	public class Dialog_AssignCaravanDrugPolicies : Window
	{
		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x06009241 RID: 37441 RVA: 0x0006203F File Offset: 0x0006023F
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(550f, 500f);
			}
		}

		// Token: 0x06009242 RID: 37442 RVA: 0x00062050 File Offset: 0x00060250
		public Dialog_AssignCaravanDrugPolicies(Caravan caravan)
		{
			this.caravan = caravan;
			this.doCloseButton = true;
		}

		// Token: 0x06009243 RID: 37443 RVA: 0x002A0584 File Offset: 0x0029E784
		public override void DoWindowContents(Rect rect)
		{
			rect.height -= this.CloseButSize.y;
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

		// Token: 0x06009244 RID: 37444 RVA: 0x002A0704 File Offset: 0x0029E904
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

		// Token: 0x04005C77 RID: 23671
		private Caravan caravan;

		// Token: 0x04005C78 RID: 23672
		private Vector2 scrollPos;

		// Token: 0x04005C79 RID: 23673
		private float lastHeight;

		// Token: 0x04005C7A RID: 23674
		private const float RowHeight = 30f;

		// Token: 0x04005C7B RID: 23675
		private const float AssignDrugPolicyButtonsTotalWidth = 354f;

		// Token: 0x04005C7C RID: 23676
		private const int ManageDrugPoliciesButtonHeight = 32;
	}
}
