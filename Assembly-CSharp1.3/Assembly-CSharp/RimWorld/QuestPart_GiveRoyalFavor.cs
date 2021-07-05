using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC1 RID: 3009
	public class QuestPart_GiveRoyalFavor : QuestPart
	{
		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06004670 RID: 18032 RVA: 0x00174A70 File Offset: 0x00172C70
		public override bool RequiresAccepter
		{
			get
			{
				return this.giveToAccepter;
			}
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06004671 RID: 18033 RVA: 0x00174A78 File Offset: 0x00172C78
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x00174A88 File Offset: 0x00172C88
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Pawn pawn = this.giveToAccepter ? this.quest.AccepterPawn : this.giveTo;
				if (pawn == null)
				{
					signal.args.TryGetArg<Pawn>("CHOSEN", out pawn);
				}
				if (pawn != null && pawn.royalty != null)
				{
					pawn.royalty.GainFavor(this.faction, this.amount);
				}
			}
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x00174B04 File Offset: 0x00172D04
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.giveTo = PawnsFinder.AllMaps_FreeColonists.RandomElement<Pawn>();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			this.amount = 10;
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x00174B60 File Offset: 0x00172D60
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.giveTo, "giveTo", false);
			Scribe_Values.Look<bool>(ref this.giveToAccepter, "giveToAccepter", false, false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x00174BCB File Offset: 0x00172DCB
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.giveTo == replace)
			{
				this.giveTo = with;
			}
		}

		// Token: 0x04002AF5 RID: 10997
		public Pawn giveTo;

		// Token: 0x04002AF6 RID: 10998
		public bool giveToAccepter;

		// Token: 0x04002AF7 RID: 10999
		public string inSignal;

		// Token: 0x04002AF8 RID: 11000
		public int amount;

		// Token: 0x04002AF9 RID: 11001
		public Faction faction;
	}
}
