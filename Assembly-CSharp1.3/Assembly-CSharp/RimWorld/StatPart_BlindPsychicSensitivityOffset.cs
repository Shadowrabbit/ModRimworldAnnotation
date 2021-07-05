using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C6 RID: 5318
	public class StatPart_BlindPsychicSensitivityOffset : StatPart
	{
		// Token: 0x06007EF0 RID: 32496 RVA: 0x002CEF90 File Offset: 0x002CD190
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetPsychicOffset(req.Thing, out num))
			{
				val += num;
			}
		}

		// Token: 0x06007EF1 RID: 32497 RVA: 0x002CEFB4 File Offset: 0x002CD1B4
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (req.HasThing && this.TryGetPsychicOffset(req.Thing, out f))
			{
				return "StatsReport_BlindPsychicSensitivityOffset".Translate() + (": +" + f.ToStringPercent());
			}
			return null;
		}

		// Token: 0x06007EF2 RID: 32498 RVA: 0x002CF004 File Offset: 0x002CD204
		private bool TryGetPsychicOffset(Thing t, out float offset)
		{
			offset = 0f;
			Pawn pawn;
			if ((pawn = (t as Pawn)) == null)
			{
				return false;
			}
			if (!this.ConsideredBlind(pawn) || pawn.Ideo == null)
			{
				return false;
			}
			foreach (Precept precept in pawn.Ideo.PreceptsListForReading)
			{
				offset += precept.def.blindPsychicSensitivityOffset;
			}
			return !Mathf.Approximately(offset, 0f);
		}

		// Token: 0x06007EF3 RID: 32499 RVA: 0x002CF09C File Offset: 0x002CD29C
		private bool ConsideredBlind(Pawn pawn)
		{
			foreach (BodyPartRecord part in pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.SightSource))
			{
				if (!pawn.health.hediffSet.PartIsMissing(part))
				{
					return false;
				}
			}
			return true;
		}
	}
}
