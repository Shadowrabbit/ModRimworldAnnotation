using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C8 RID: 4296
	public class QuestPart_EndGame : QuestPart
	{
		// Token: 0x06005DB8 RID: 23992 RVA: 0x001DD6F4 File Offset: 0x001DB8F4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && !ShipCountdown.CountingDown)
			{
				if (!Find.TickManager.Paused)
				{
					Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
				}
				List<Pawn> list;
				if (!signal.args.TryGetArg<List<Pawn>>("SENTCOLONISTS", out list))
				{
					list = null;
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						stringBuilder.AppendLine("   " + list[i].LabelCap);
					}
					Find.StoryWatcher.statsRecord.colonistsLaunched += list.Count;
				}
				ShipCountdown.InitiateCountdown(GameVictoryUtility.MakeEndCredits(this.introText, this.endingText, stringBuilder.ToString()));
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (!list[j].Destroyed)
						{
							list[j].Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x06005DB9 RID: 23993 RVA: 0x00040F67 File Offset: 0x0003F167
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.introText, "introText", null, false);
			Scribe_Values.Look<string>(ref this.endingText, "endingText", null, false);
		}

		// Token: 0x06005DBA RID: 23994 RVA: 0x00040FA5 File Offset: 0x0003F1A5
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04003EAC RID: 16044
		public string inSignal;

		// Token: 0x04003EAD RID: 16045
		public string introText;

		// Token: 0x04003EAE RID: 16046
		public string endingText;
	}
}
