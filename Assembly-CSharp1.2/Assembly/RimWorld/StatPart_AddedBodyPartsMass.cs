using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D26 RID: 7462
	public class StatPart_AddedBodyPartsMass : StatPart
	{
		// Token: 0x0600A23A RID: 41530 RVA: 0x002F4CE8 File Offset: 0x002F2EE8
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x0600A23B RID: 41531 RVA: 0x002F4D08 File Offset: 0x002F2F08
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetValue(req, out num) && num != 0f)
			{
				return "StatsReport_AddedBodyPartsMass".Translate() + ": " + num.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x0600A23C RID: 41532 RVA: 0x0006BCE5 File Offset: 0x00069EE5
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => this.GetAddedBodyPartsMass(x), (ThingDef x) => 0f, out value);
		}

		// Token: 0x0600A23D RID: 41533 RVA: 0x002F4D50 File Offset: 0x002F2F50
		private float GetAddedBodyPartsMass(Pawn p)
		{
			float num = 0f;
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_AddedPart hediff_AddedPart = hediffs[i] as Hediff_AddedPart;
				if (hediff_AddedPart != null && hediff_AddedPart.def.spawnThingOnRemoved != null)
				{
					num += hediff_AddedPart.def.spawnThingOnRemoved.GetStatValueAbstract(StatDefOf.Mass, null) * 0.9f;
				}
			}
			return num;
		}

		// Token: 0x04006E54 RID: 28244
		private const float AddedBodyPartMassFactor = 0.9f;
	}
}
