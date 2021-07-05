using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C7 RID: 5319
	public class StatPart_BodySize : StatPart
	{
		// Token: 0x06007EF5 RID: 32501 RVA: 0x002CF10C File Offset: 0x002CD30C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetBodySize(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06007EF6 RID: 32502 RVA: 0x002CF12C File Offset: 0x002CD32C
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetBodySize(req, out f))
			{
				return "StatsReport_BodySize".Translate(f.ToString("F2")) + ": x" + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06007EF7 RID: 32503 RVA: 0x002CF17C File Offset: 0x002CD37C
		private bool TryGetBodySize(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.BodySize, (ThingDef x) => x.race.baseBodySize, out bodySize);
		}
	}
}
