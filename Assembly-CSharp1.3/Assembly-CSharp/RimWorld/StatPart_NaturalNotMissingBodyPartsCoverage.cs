using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D8 RID: 5336
	public class StatPart_NaturalNotMissingBodyPartsCoverage : StatPart
	{
		// Token: 0x06007F3B RID: 32571 RVA: 0x002CFC5C File Offset: 0x002CDE5C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06007F3C RID: 32572 RVA: 0x002CFC7C File Offset: 0x002CDE7C
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetValue(req, out f))
			{
				return "StatsReport_MissingBodyParts".Translate() + ": x" + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06007F3D RID: 32573 RVA: 0x002CFCBC File Offset: 0x002CDEBC
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x.RaceProps.body.corePart), (ThingDef x) => 1f, out value);
		}
	}
}
