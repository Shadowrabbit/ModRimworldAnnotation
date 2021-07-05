using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F4 RID: 5364
	public abstract class Need_Seeker : Need
	{
		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x0600738C RID: 29580 RVA: 0x00234588 File Offset: 0x00232788
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

		// Token: 0x0600738D RID: 29581 RVA: 0x0004D8EE File Offset: 0x0004BAEE
		public Need_Seeker(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600738E RID: 29582 RVA: 0x002345D0 File Offset: 0x002327D0
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

		// Token: 0x04004C57 RID: 19543
		private const float GUIArrowTolerance = 0.05f;
	}
}
