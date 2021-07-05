using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E59 RID: 3673
	public class Need_Suppression : Need
	{
		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x060054FE RID: 21758 RVA: 0x001CC9BE File Offset: 0x001CABBE
		public bool CanBeSuppressedNow
		{
			get
			{
				return this.CurLevel < 0.5f;
			}
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x060054FF RID: 21759 RVA: 0x001CC89E File Offset: 0x001CAA9E
		public bool IsHigh
		{
			get
			{
				return this.CurLevel < 0.3f;
			}
		}

		// Token: 0x06005500 RID: 21760 RVA: 0x001CAD4D File Offset: 0x001C8F4D
		public Need_Suppression(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x06005501 RID: 21761 RVA: 0x001CC9CD File Offset: 0x001CABCD
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= 0.0025f * this.pawn.GetStatValue(StatDefOf.SlaveSuppressionFallRate, true);
			}
		}

		// Token: 0x06005502 RID: 21762 RVA: 0x001CC9FB File Offset: 0x001CABFB
		public void DrawSuppressionBar(Rect rect)
		{
			Widgets.FillableBar(rect, base.CurLevelPercentage, GuestUtility.SlaveSuppressionFillTex);
			base.DrawBarThreshold(rect, 0.3f);
			base.DrawBarThreshold(rect, 0.15f);
		}

		// Token: 0x0400325D RID: 12893
		private const float CanSuppressMaxThreshold = 0.5f;

		// Token: 0x0400325E RID: 12894
		private const float SuppressCriticalThreshold = 0.3f;
	}
}
