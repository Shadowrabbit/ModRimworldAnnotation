using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200070D RID: 1805
	public class FeedbackItem_HealthGain : FeedbackItem
	{
		// Token: 0x06002DAB RID: 11691 RVA: 0x00023FDC File Offset: 0x000221DC
		public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer) : base(ScreenPos)
		{
			this.Amount = Amount;
			this.Healer = Healer;
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x00134BF0 File Offset: 0x00132DF0
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

		// Token: 0x04001F20 RID: 7968
		protected Pawn Healer;

		// Token: 0x04001F21 RID: 7969
		protected int Amount;
	}
}
