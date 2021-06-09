using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D36 RID: 7478
	public class StatPart_GearAndInventoryMass : StatPart
	{
		// Token: 0x0600A282 RID: 41602 RVA: 0x002F56C8 File Offset: 0x002F38C8
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x0600A283 RID: 41603 RVA: 0x002F56E8 File Offset: 0x002F38E8
		public override string ExplanationPart(StatRequest req)
		{
			float mass;
			if (this.TryGetValue(req, out mass))
			{
				return "StatsReport_GearAndInventoryMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x0600A284 RID: 41604 RVA: 0x002F5728 File Offset: 0x002F3928
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => MassUtility.GearAndInventoryMass(x), (ThingDef x) => 0f, out value);
		}
	}
}
