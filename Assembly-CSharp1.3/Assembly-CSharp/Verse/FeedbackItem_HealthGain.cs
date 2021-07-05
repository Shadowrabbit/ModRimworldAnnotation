using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F1 RID: 1009
	public class FeedbackItem_HealthGain : FeedbackItem
	{
		// Token: 0x06001E61 RID: 7777 RVA: 0x000BE1B7 File Offset: 0x000BC3B7
		public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer) : base(ScreenPos)
		{
			this.Amount = Amount;
			this.Healer = Healer;
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x000BE1D0 File Offset: 0x000BC3D0
		public override void FeedbackOnGUI()
		{
			string text;
			if (this.Amount >= 0)
			{
				text = "+";
			}
			else
			{
				text = "-";
			}
			text += this.Amount;
			base.DrawFloatingText(text, Color.red);
		}

		// Token: 0x04001277 RID: 4727
		protected Pawn Healer;

		// Token: 0x04001278 RID: 4728
		protected int Amount;
	}
}
