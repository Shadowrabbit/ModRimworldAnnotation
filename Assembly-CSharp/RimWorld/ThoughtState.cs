using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000E7B RID: 3707
	public struct ThoughtState
	{
		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x0600532F RID: 21295 RVA: 0x0003A166 File Offset: 0x00038366
		public bool Active
		{
			get
			{
				return this.stageIndex != -99999;
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06005330 RID: 21296 RVA: 0x0003A178 File Offset: 0x00038378
		public int StageIndex
		{
			get
			{
				return this.stageIndex;
			}
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06005331 RID: 21297 RVA: 0x0003A180 File Offset: 0x00038380
		public string Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06005332 RID: 21298 RVA: 0x0003A188 File Offset: 0x00038388
		public static ThoughtState ActiveDefault
		{
			get
			{
				return ThoughtState.ActiveAtStage(0);
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x06005333 RID: 21299 RVA: 0x001C0174 File Offset: 0x001BE374
		public static ThoughtState Inactive
		{
			get
			{
				return new ThoughtState
				{
					stageIndex = -99999
				};
			}
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x001C0198 File Offset: 0x001BE398
		public static ThoughtState ActiveAtStage(int stageIndex)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex
			};
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x001C01B8 File Offset: 0x001BE3B8
		public static ThoughtState ActiveAtStage(int stageIndex, string reason)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex,
				reason = reason
			};
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x001C01E0 File Offset: 0x001BE3E0
		public static ThoughtState ActiveWithReason(string reason)
		{
			ThoughtState activeDefault = ThoughtState.ActiveDefault;
			activeDefault.reason = reason;
			return activeDefault;
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x0003A190 File Offset: 0x00038390
		public static implicit operator ThoughtState(bool value)
		{
			if (value)
			{
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x001C01FC File Offset: 0x001BE3FC
		public bool ActiveFor(ThoughtDef thoughtDef)
		{
			if (!this.Active)
			{
				return false;
			}
			int num = this.StageIndexFor(thoughtDef);
			return num >= 0 && thoughtDef.stages[num] != null;
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x0003A1A0 File Offset: 0x000383A0
		public int StageIndexFor(ThoughtDef thoughtDef)
		{
			return Mathf.Min(this.StageIndex, thoughtDef.stages.Count - 1);
		}

		// Token: 0x04003505 RID: 13573
		private int stageIndex;

		// Token: 0x04003506 RID: 13574
		private string reason;

		// Token: 0x04003507 RID: 13575
		private const int InactiveIndex = -99999;
	}
}
