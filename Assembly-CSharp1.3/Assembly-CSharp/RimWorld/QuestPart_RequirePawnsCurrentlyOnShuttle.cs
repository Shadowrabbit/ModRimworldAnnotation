using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B96 RID: 2966
	public class QuestPart_RequirePawnsCurrentlyOnShuttle : QuestPart
	{
		// Token: 0x06004553 RID: 17747 RVA: 0x0016FE3C File Offset: 0x0016E03C
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
			if (compShuttle.requiredPawns.Contains(pawn))
			{
				compShuttle.requiredPawns.Remove(pawn);
			}
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x0016FE70 File Offset: 0x0016E070
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal && this.shuttle != null)
			{
				CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
				compShuttle.requiredColonistCount = this.requiredColonistCount;
				compShuttle.requiredItems.Clear();
				compShuttle.requiredPawns.Clear();
				using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)this.shuttle.TryGetComp<CompTransporter>().innerContainer).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn item;
						if ((item = (enumerator.Current as Pawn)) != null && !compShuttle.requiredPawns.Contains(item))
						{
							compShuttle.requiredPawns.Add(item);
						}
					}
				}
			}
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x0016FF34 File Offset: 0x0016E134
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x0016FF4E File Offset: 0x0016E14E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignl", null, false);
			Scribe_Values.Look<int>(ref this.requiredColonistCount, "requiredColonistCount", 0, false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
		}

		// Token: 0x04002A41 RID: 10817
		public string inSignal;

		// Token: 0x04002A42 RID: 10818
		public Thing shuttle;

		// Token: 0x04002A43 RID: 10819
		public int requiredColonistCount;
	}
}
