using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D2E RID: 7470
	public class StatPart_BodySize : StatPart
	{
		// Token: 0x0600A265 RID: 41573 RVA: 0x002F53C4 File Offset: 0x002F35C4
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetBodySize(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600A266 RID: 41574 RVA: 0x002F53E4 File Offset: 0x002F35E4
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetBodySize(req, out f))
			{
				return "StatsReport_BodySize".Translate(f.ToString("F2")) + ": x" + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600A267 RID: 41575 RVA: 0x002F5434 File Offset: 0x002F3634
		private bool TryGetBodySize(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.BodySize, (ThingDef x) => x.race.baseBodySize, out bodySize);
		}
	}
}
