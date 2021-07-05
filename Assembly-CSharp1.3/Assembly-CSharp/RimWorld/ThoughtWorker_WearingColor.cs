using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D8 RID: 2520
	public abstract class ThoughtWorker_WearingColor : ThoughtWorker
	{
		// Token: 0x06003E5D RID: 15965
		protected abstract Color? Color(Pawn p);

		// Token: 0x06003E5E RID: 15966 RVA: 0x00155074 File Offset: 0x00153274
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Color? color = this.Color(p);
			if (color == null)
			{
				return false;
			}
			int num = 0;
			foreach (Apparel thing in p.apparel.WornApparel)
			{
				CompColorable compColorable = thing.TryGetComp<CompColorable>();
				if (compColorable.Active && compColorable.Color == color)
				{
					num++;
				}
			}
			return (float)num / (float)p.apparel.WornApparelCount >= 0.6f;
		}

		// Token: 0x040020EC RID: 8428
		public const float RequiredMinPercentage = 0.6f;
	}
}
