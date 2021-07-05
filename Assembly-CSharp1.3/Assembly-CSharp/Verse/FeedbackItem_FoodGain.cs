using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F0 RID: 1008
	public class FeedbackItem_FoodGain : FeedbackItem
	{
		// Token: 0x06001E5F RID: 7775 RVA: 0x000BE178 File Offset: 0x000BC378
		public FeedbackItem_FoodGain(Vector2 ScreenPos, int Amount) : base(ScreenPos)
		{
			this.Amount = Amount;
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000BE188 File Offset: 0x000BC388
		public override void FeedbackOnGUI()
		{
			string str = this.Amount + " food";
			base.DrawFloatingText(str, Color.yellow);
		}

		// Token: 0x04001276 RID: 4726
		protected int Amount;
	}
}
