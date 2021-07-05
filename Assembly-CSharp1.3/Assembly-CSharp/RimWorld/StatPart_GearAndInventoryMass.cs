using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014CF RID: 5327
	public class StatPart_GearAndInventoryMass : StatPart
	{
		// Token: 0x06007F12 RID: 32530 RVA: 0x002CF58C File Offset: 0x002CD78C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x06007F13 RID: 32531 RVA: 0x002CF5AC File Offset: 0x002CD7AC
		public override string ExplanationPart(StatRequest req)
		{
			float mass;
			if (this.TryGetValue(req, out mass))
			{
				return "StatsReport_GearAndInventoryMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x06007F14 RID: 32532 RVA: 0x002CF5EC File Offset: 0x002CD7EC
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => MassUtility.GearAndInventoryMass(x), (ThingDef x) => 0f, out value);
		}
	}
}
