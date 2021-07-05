using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D4 RID: 5332
	public class StatPart_IsFlesh : StatPart
	{
		// Token: 0x06007F29 RID: 32553 RVA: 0x002CF974 File Offset: 0x002CDB74
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06007F2A RID: 32554 RVA: 0x002CF994 File Offset: 0x002CDB94
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num) && num != 1f)
			{
				return "StatsReport_NotFlesh".Translate() + ": x" + num.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06007F2B RID: 32555 RVA: 0x002CF9DC File Offset: 0x002CDBDC
		private bool TryGetIsFleshFactor(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, delegate(Pawn x)
			{
				if (!x.RaceProps.IsFlesh)
				{
					return 0f;
				}
				return 1f;
			}, delegate(ThingDef x)
			{
				if (!x.race.IsFlesh)
				{
					return 0f;
				}
				return 1f;
			}, out bodySize);
		}
	}
}
