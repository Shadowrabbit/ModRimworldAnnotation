using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B40 RID: 2880
	public abstract class QuestPart_MTB : QuestPartActivable
	{
		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06004357 RID: 17239
		protected abstract float MTBDays { get; }

		// Token: 0x06004358 RID: 17240 RVA: 0x0016744C File Offset: 0x0016564C
		public override void QuestPartTick()
		{
			float mtbdays = this.MTBDays;
			if (mtbdays > 0f && Find.TickManager.TicksGame % 10 == 0 && Rand.MTBEventOccurs(mtbdays, 60000f, 10f))
			{
				base.Complete();
			}
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x00167490 File Offset: 0x00165690
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "MTB occurs " + this.ToString(), true, true, true))
			{
				base.Complete();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x040028EE RID: 10478
		private const int CheckIntervalTicks = 10;
	}
}
