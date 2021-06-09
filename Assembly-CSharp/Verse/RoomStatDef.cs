using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200017E RID: 382
	public class RoomStatDef : Def
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0000D81F File Offset: 0x0000BA1F
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

		// Token: 0x0600099C RID: 2460 RVA: 0x0000D851 File Offset: 0x0000BA51
		public RoomStatScoreStage GetScoreStage(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				return null;
			}
			return this.scoreStages[this.GetScoreStageIndex(score)];
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00099B64 File Offset: 0x00097D64
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

		// Token: 0x0600099E RID: 2462 RVA: 0x00099BB8 File Offset: 0x00097DB8
		public string ScoreToString(float score)
		{
			if (this.displayRounded)
			{
				return Mathf.RoundToInt(score).ToString();
			}
			return score.ToString("F2");
		}

		// Token: 0x04000838 RID: 2104
		public Type workerClass;

		// Token: 0x04000839 RID: 2105
		public float updatePriority;

		// Token: 0x0400083A RID: 2106
		public bool displayRounded;

		// Token: 0x0400083B RID: 2107
		public bool isHidden;

		// Token: 0x0400083C RID: 2108
		public float roomlessScore;

		// Token: 0x0400083D RID: 2109
		public List<RoomStatScoreStage> scoreStages;

		// Token: 0x0400083E RID: 2110
		public RoomStatDef inputStat;

		// Token: 0x0400083F RID: 2111
		public SimpleCurve curve;

		// Token: 0x04000840 RID: 2112
		[Unsaved(false)]
		private RoomStatWorker workerInt;
	}
}
