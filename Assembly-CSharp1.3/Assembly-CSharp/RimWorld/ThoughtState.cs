using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000934 RID: 2356
	public struct ThoughtState
	{
		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06003CA0 RID: 15520 RVA: 0x0014FDCB File Offset: 0x0014DFCB
		public bool Active
		{
			get
			{
				return this.stageIndex != -99999;
			}
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x0014FDDD File Offset: 0x0014DFDD
		public int StageIndex
		{
			get
			{
				return this.stageIndex;
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06003CA2 RID: 15522 RVA: 0x0014FDE5 File Offset: 0x0014DFE5
		public string Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06003CA3 RID: 15523 RVA: 0x0014FDED File Offset: 0x0014DFED
		public static ThoughtState ActiveDefault
		{
			get
			{
				return ThoughtState.ActiveAtStage(0);
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06003CA4 RID: 15524 RVA: 0x0014FDF8 File Offset: 0x0014DFF8
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

		// Token: 0x06003CA5 RID: 15525 RVA: 0x0014FE1C File Offset: 0x0014E01C
		public static ThoughtState ActiveAtStage(int stageIndex)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex
			};
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x0014FE3C File Offset: 0x0014E03C
		public static ThoughtState ActiveAtStage(int stageIndex, string reason)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex,
				reason = reason
			};
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x0014FE64 File Offset: 0x0014E064
		public static ThoughtState ActiveWithReason(string reason)
		{
			ThoughtState activeDefault = ThoughtState.ActiveDefault;
			activeDefault.reason = reason;
			return activeDefault;
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x0014FE80 File Offset: 0x0014E080
		public static implicit operator ThoughtState(bool value)
		{
			if (value)
			{
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x0014FE90 File Offset: 0x0014E090
		public bool ActiveFor(ThoughtDef thoughtDef)
		{
			if (!this.Active)
			{
				return false;
			}
			int num = this.StageIndexFor(thoughtDef);
			return num >= 0 && thoughtDef.stages[num] != null;
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x0014FEC4 File Offset: 0x0014E0C4
		public int StageIndexFor(ThoughtDef thoughtDef)
		{
			return Mathf.Min(this.StageIndex, thoughtDef.stages.Count - 1);
		}

		// Token: 0x040020B5 RID: 8373
		private int stageIndex;

		// Token: 0x040020B6 RID: 8374
		private string reason;

		// Token: 0x040020B7 RID: 8375
		private const int InactiveIndex = -99999;
	}
}
