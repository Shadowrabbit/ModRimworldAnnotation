using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D4F RID: 7503
	public class StatPart_WildManOffset : StatPart
	{
		// Token: 0x0600A2F9 RID: 41721 RVA: 0x0006C364 File Offset: 0x0006A564
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsWildMan(req))
			{
				val += this.offset;
			}
		}

		// Token: 0x0600A2FA RID: 41722 RVA: 0x0006C37A File Offset: 0x0006A57A
		public override string ExplanationPart(StatRequest req)
		{
			if (this.IsWildMan(req))
			{
				return "StatsReport_WildMan".Translate() + ": " + this.offset.ToStringWithSign("0.##");
			}
			return null;
		}

		// Token: 0x0600A2FB RID: 41723 RVA: 0x002F6948 File Offset: 0x002F4B48
		private bool IsWildMan(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsWildMan();
		}

		// Token: 0x04006EA6 RID: 28326
		public float offset;
	}
}
