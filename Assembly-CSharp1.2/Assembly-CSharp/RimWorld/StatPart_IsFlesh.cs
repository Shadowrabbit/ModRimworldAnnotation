using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D3C RID: 7484
	public class StatPart_IsFlesh : StatPart
	{
		// Token: 0x0600A2A1 RID: 41633 RVA: 0x002F59BC File Offset: 0x002F3BBC
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600A2A2 RID: 41634 RVA: 0x002F59DC File Offset: 0x002F3BDC
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num) && num != 1f)
			{
				return "StatsReport_NotFlesh".Translate() + ": x" + num.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600A2A3 RID: 41635 RVA: 0x002F5A24 File Offset: 0x002F3C24
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
