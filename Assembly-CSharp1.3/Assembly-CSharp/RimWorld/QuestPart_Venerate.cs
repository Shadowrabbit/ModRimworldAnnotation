using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B5B RID: 2907
	public class QuestPart_Venerate : QuestPart_MakeLord
	{
		// Token: 0x060043FC RID: 17404 RVA: 0x00169850 File Offset: 0x00167A50
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_Venerate(this.target, this.venerateDurationTicks, this.outSignalVenerationCompleted, this.inSignalForceExit), base.Map, null);
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x00169881 File Offset: 0x00167A81
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<int>(ref this.venerateDurationTicks, "venerateDurationTicks", 0, false);
			Scribe_Values.Look<string>(ref this.inSignalForceExit, "inSignalForceExit", null, false);
		}

		// Token: 0x04002942 RID: 10562
		public Thing target;

		// Token: 0x04002943 RID: 10563
		public int venerateDurationTicks;

		// Token: 0x04002944 RID: 10564
		public string outSignalVenerationCompleted;

		// Token: 0x04002945 RID: 10565
		public string inSignalForceExit;
	}
}
