using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C1 RID: 5313
	public class StatPart_AddedBodyPartsMass : StatPart
	{
		// Token: 0x06007ED7 RID: 32471 RVA: 0x002CE944 File Offset: 0x002CCB44
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x06007ED8 RID: 32472 RVA: 0x002CE964 File Offset: 0x002CCB64
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetValue(req, out num) && num != 0f)
			{
				return "StatsReport_AddedBodyPartsMass".Translate() + ": " + num.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x06007ED9 RID: 32473 RVA: 0x002CE9AA File Offset: 0x002CCBAA
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => this.GetAddedBodyPartsMass(x), (ThingDef x) => 0f, out value);
		}

		// Token: 0x06007EDA RID: 32474 RVA: 0x002CE9E0 File Offset: 0x002CCBE0
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

		// Token: 0x04004F5E RID: 20318
		private const float AddedBodyPartMassFactor = 0.9f;
	}
}
