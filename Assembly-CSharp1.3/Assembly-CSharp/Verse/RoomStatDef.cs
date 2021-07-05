using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000FE RID: 254
	public class RoomStatDef : Def
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x000210EA File Offset: 0x0001F2EA
		public RoomStatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomStatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002111C File Offset: 0x0001F31C
		public RoomStatScoreStage GetScoreStage(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				return null;
			}
			return this.scoreStages[this.GetScoreStageIndex(score)];
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00021140 File Offset: 0x0001F340
		public int GetScoreStageIndex(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				throw new InvalidOperationException("No score stages available.");
			}
			int result = 0;
			int num = 0;
			while (num < this.scoreStages.Count && score >= this.scoreStages[num].minScore)
			{
				result = num;
				num++;
			}
			return result;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00021194 File Offset: 0x0001F394
		public string ScoreToString(float score)
		{
			if (this.displayRounded)
			{
				return Mathf.RoundToInt(score).ToString();
			}
			return score.ToString("F2");
		}

		// Token: 0x04000611 RID: 1553
		public Type workerClass;

		// Token: 0x04000612 RID: 1554
		public float updatePriority;

		// Token: 0x04000613 RID: 1555
		public bool displayRounded;

		// Token: 0x04000614 RID: 1556
		public bool isHidden;

		// Token: 0x04000615 RID: 1557
		public float roomlessScore;

		// Token: 0x04000616 RID: 1558
		public List<RoomStatScoreStage> scoreStages;

		// Token: 0x04000617 RID: 1559
		public RoomStatDef inputStat;

		// Token: 0x04000618 RID: 1560
		public SimpleCurve curve;

		// Token: 0x04000619 RID: 1561
		[Unsaved(false)]
		private RoomStatWorker workerInt;
	}
}
