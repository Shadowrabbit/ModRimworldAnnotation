using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E98 RID: 3736
	public class Thought_IdeoLeaderResentment : Thought_Situational
	{
		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x060057C7 RID: 22471 RVA: 0x001DDC07 File Offset: 0x001DBE07
		public Pawn Leader
		{
			get
			{
				return this.pawn.Faction.leader;
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x060057C8 RID: 22472 RVA: 0x001DDC19 File Offset: 0x001DBE19
		public override string LabelCap
		{
			get
			{
				return "IdeoLeaderDifferentIdeoThoughtLabel".Translate(this.Leader.Ideo.memberName);
			}
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x060057C9 RID: 22473 RVA: 0x001DDC3F File Offset: 0x001DBE3F
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Leader.Ideo.memberName);
			}
		}
	}
}
