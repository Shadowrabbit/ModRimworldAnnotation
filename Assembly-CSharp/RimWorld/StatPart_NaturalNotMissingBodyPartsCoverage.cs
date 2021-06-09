using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D41 RID: 7489
	public class StatPart_NaturalNotMissingBodyPartsCoverage : StatPart
	{
		// Token: 0x0600A2BB RID: 41659 RVA: 0x002F5BC0 File Offset: 0x002F3DC0
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600A2BC RID: 41660 RVA: 0x002F5BE0 File Offset: 0x002F3DE0
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetValue(req, out f))
			{
				return "StatsReport_MissingBodyParts".Translate() + ": x" + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600A2BD RID: 41661 RVA: 0x002F5C20 File Offset: 0x002F3E20
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x.RaceProps.body.corePart), (ThingDef x) => 1f, out value);
		}
	}
}
