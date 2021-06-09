using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D68 RID: 7528
	public class StatWorker_MinimumHandlingSkill : StatWorker
	{
		// Token: 0x0600A3AD RID: 41901 RVA: 0x0006CA1F File Offset: 0x0006AC1F
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return this.ValueFromReq(req);
		}

		// Token: 0x0600A3AE RID: 41902 RVA: 0x002FA844 File Offset: 0x002F8A44
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			float wildness = ((ThingDef)req.Def).race.wildness;
			return "Wildness".Translate() + " " + wildness.ToStringPercent() + ": " + this.ValueFromReq(req).ToString("F0");
		}

		// Token: 0x0600A3AF RID: 41903 RVA: 0x002FA8B0 File Offset: 0x002F8AB0
		private float ValueFromReq(StatRequest req)
		{
			float wildness = ((ThingDef)req.Def).race.wildness;
			return Mathf.Clamp(GenMath.LerpDouble(0.15f, 1f, 0f, 10f, wildness), 0f, 20f);
		}
	}
}
