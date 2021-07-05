using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200070E RID: 1806
	public class FeedbackFloaters
	{
		// Token: 0x06002DAD RID: 11693 RVA: 0x00023FF3 File Offset: 0x000221F3
		public void AddFeedback(FeedbackItem newFeedback)
		{
			this.feeders.Add(newFeedback);
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x00134C38 File Offset: 0x00132E38
		public void FeedbackUpdate()
		{
			for (int i = this.feeders.Count - 1; i >= 0; i--)
			{
				this.feeders[i].Update();
				if (this.feeders[i].TimeLeft <= 0f)
				{
					this.feeders.Remove(this.feeders[i]);
				}
			}
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x00134CA0 File Offset: 0x00132EA0
		public void FeedbackOnGUI()
		{
			foreach (FeedbackItem feedbackItem in this.feeders)
			{
				feedbackItem.FeedbackOnGUI();
			}
		}

		// Token: 0x04001F22 RID: 7970
		protected List<FeedbackItem> feeders = new List<FeedbackItem>();
	}
}
