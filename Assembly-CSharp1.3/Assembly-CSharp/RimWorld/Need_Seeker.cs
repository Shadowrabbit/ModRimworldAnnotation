using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E58 RID: 3672
	public abstract class Need_Seeker : Need
	{
		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x060054FB RID: 21755 RVA: 0x001CC8E8 File Offset: 0x001CAAE8
		public override int GUIChangeArrow
		{
			get
			{
				if (!this.pawn.Awake())
				{
					return 0;
				}
				float curInstantLevelPercentage = base.CurInstantLevelPercentage;
				if (curInstantLevelPercentage > base.CurLevelPercentage + 0.05f)
				{
					return 1;
				}
				if (curInstantLevelPercentage < base.CurLevelPercentage - 0.05f)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x060054FC RID: 21756 RVA: 0x001CAD4D File Offset: 0x001C8F4D
		public Need_Seeker(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x060054FD RID: 21757 RVA: 0x001CC930 File Offset: 0x001CAB30
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				float curInstantLevel = this.CurInstantLevel;
				if (curInstantLevel > this.CurLevel)
				{
					this.CurLevel += this.def.seekerRisePerHour * 0.06f;
					this.CurLevel = Mathf.Min(this.CurLevel, curInstantLevel);
				}
				if (curInstantLevel < this.CurLevel)
				{
					this.CurLevel -= this.def.seekerFallPerHour * 0.06f;
					this.CurLevel = Mathf.Max(this.CurLevel, curInstantLevel);
				}
			}
		}

		// Token: 0x0400325C RID: 12892
		private const float GUIArrowTolerance = 0.05f;
	}
}
