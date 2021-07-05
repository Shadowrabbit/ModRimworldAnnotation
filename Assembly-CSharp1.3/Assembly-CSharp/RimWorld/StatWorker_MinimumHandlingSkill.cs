using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FB RID: 5371
	public class StatWorker_MinimumHandlingSkill : StatWorker
	{
		// Token: 0x06008003 RID: 32771 RVA: 0x002D53BC File Offset: 0x002D35BC
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return this.ValueFromReq(req);
		}

		// Token: 0x06008004 RID: 32772 RVA: 0x002D53C8 File Offset: 0x002D35C8
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			float wildness = ((ThingDef)req.Def).race.wildness;
			return "Wildness".Translate() + " " + wildness.ToStringPercent() + ": " + this.ValueFromReq(req).ToString("F0");
		}

		// Token: 0x06008005 RID: 32773 RVA: 0x002D5434 File Offset: 0x002D3634
		private float ValueFromReq(StatRequest req)
		{
			float wildness = ((ThingDef)req.Def).race.wildness;
			return Mathf.Clamp(GenMath.LerpDouble(0.15f, 1f, 0f, 10f, wildness), 0f, 20f);
		}
	}
}
