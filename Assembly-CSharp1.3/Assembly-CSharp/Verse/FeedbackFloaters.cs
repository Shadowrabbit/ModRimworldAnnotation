using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003F2 RID: 1010
	public class FeedbackFloaters
	{
		// Token: 0x06001E63 RID: 7779 RVA: 0x000BE218 File Offset: 0x000BC418
		public void AddFeedback(FeedbackItem newFeedback)
		{
			this.feeders.Add(newFeedback);
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x000BE228 File Offset: 0x000BC428
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

		// Token: 0x06001E65 RID: 7781 RVA: 0x000BE290 File Offset: 0x000BC490
		public void FeedbackOnGUI()
		{
			foreach (FeedbackItem feedbackItem in this.feeders)
			{
				feedbackItem.FeedbackOnGUI();
			}
		}

		// Token: 0x04001279 RID: 4729
		protected List<FeedbackItem> feeders = new List<FeedbackItem>();
	}
}
