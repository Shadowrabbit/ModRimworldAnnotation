using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B74 RID: 2932
	public class QuestPart_EndGame : QuestPart
	{
		// Token: 0x06004498 RID: 17560 RVA: 0x0016BD84 File Offset: 0x00169F84
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
				ShipCountdown.InitiateCountdown(GameVictoryUtility.MakeEndCredits(this.introText, this.endingText, stringBuilder.ToString(), "GameOverColonistsEscaped", null));
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

		// Token: 0x06004499 RID: 17561 RVA: 0x0016BE8A File Offset: 0x0016A08A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.introText, "introText", null, false);
			Scribe_Values.Look<string>(ref this.endingText, "endingText", null, false);
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x0016BEC8 File Offset: 0x0016A0C8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x0400299E RID: 10654
		public string inSignal;

		// Token: 0x0400299F RID: 10655
		public string introText;

		// Token: 0x040029A0 RID: 10656
		public string endingText;
	}
}
