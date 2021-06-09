using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001140 RID: 4416
	public class QuestPart_GiveRoyalFavor : QuestPart
	{
		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x060060D3 RID: 24787 RVA: 0x00042C3E File Offset: 0x00040E3E
		public override bool RequiresAccepter
		{
			get
			{
				return this.giveToAccepter;
			}
		}

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x060060D4 RID: 24788 RVA: 0x00042C46 File Offset: 0x00040E46
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

		// Token: 0x060060D5 RID: 24789 RVA: 0x001E5E40 File Offset: 0x001E4040
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

		// Token: 0x060060D6 RID: 24790 RVA: 0x001E5EBC File Offset: 0x001E40BC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.giveTo = PawnsFinder.AllMaps_FreeColonists.RandomElement<Pawn>();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			this.amount = 10;
		}

		// Token: 0x060060D7 RID: 24791 RVA: 0x001E5F18 File Offset: 0x001E4118
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.giveTo, "giveTo", false);
			Scribe_Values.Look<bool>(ref this.giveToAccepter, "giveToAccepter", false, false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
		}

		// Token: 0x060060D8 RID: 24792 RVA: 0x00042C56 File Offset: 0x00040E56
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.giveTo == replace)
			{
				this.giveTo = with;
			}
		}

		// Token: 0x040040B4 RID: 16564
		public Pawn giveTo;

		// Token: 0x040040B5 RID: 16565
		public bool giveToAccepter;

		// Token: 0x040040B6 RID: 16566
		public string inSignal;

		// Token: 0x040040B7 RID: 16567
		public int amount;

		// Token: 0x040040B8 RID: 16568
		public Faction faction;
	}
}
