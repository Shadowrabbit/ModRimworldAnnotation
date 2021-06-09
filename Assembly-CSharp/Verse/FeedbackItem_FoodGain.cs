using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200070C RID: 1804
	public class FeedbackItem_FoodGain : FeedbackItem
	{
		// Token: 0x06002DA9 RID: 11689 RVA: 0x00023FCC File Offset: 0x000221CC
		public FeedbackItem_FoodGain(Vector2 ScreenPos, int Amount) : base(ScreenPos)
		{
			this.Amount = Amount;
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x00134BC0 File Offset: 0x00132DC0
		public override void FeedbackOnGUI()
		{
			string str = this.Amount + " food";
			base.DrawFloatingText(str, Color.yellow);
		}

		// Token: 0x04001F1F RID: 7967
		protected int Amount;
	}
}
