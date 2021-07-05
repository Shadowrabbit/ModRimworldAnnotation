using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B79 RID: 2937
	public class QuestPart_ExtraFaction : QuestPartActivable
	{
		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x060044A4 RID: 17572 RVA: 0x0016C0E4 File Offset: 0x0016A2E4
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.extraFaction != null && this.extraFaction.faction != null)
				{
					yield return this.extraFaction.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x0016C0F4 File Offset: 0x0016A2F4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn pawn;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out pawn) && this.affectedPawns.Contains(pawn))
			{
				this.affectedPawns.Remove(pawn);
				this.extraFaction.faction.Notify_MemberLeftExtraFaction(pawn);
			}
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x0016C15C File Offset: 0x0016A35C
		public override bool QuestPartReserves(Faction f)
		{
			return this.extraFaction.faction == f;
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x0016C16C File Offset: 0x0016A36C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ExtraFaction>(ref this.extraFaction, "extraFaction", Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.affectedPawns, "affectedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.areHelpers, "areHelpers", false, false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.affectedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x0016C201 File Offset: 0x0016A401
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.affectedPawns.Replace(replace, with);
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x0016C211 File Offset: 0x0016A411
		public override void Cleanup()
		{
			base.Cleanup();
			this.SetRelationsGainTickForPawns();
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x0016C21F File Offset: 0x0016A41F
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.extraFaction.faction == faction)
			{
				this.extraFaction.faction = null;
			}
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x0016C23B File Offset: 0x0016A43B
		protected override void Disable()
		{
			base.Disable();
			this.SetRelationsGainTickForPawns();
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x0016C24C File Offset: 0x0016A44C
		private void SetRelationsGainTickForPawns()
		{
			foreach (Pawn pawn in this.affectedPawns)
			{
				if (pawn.mindState != null)
				{
					pawn.mindState.SetNoAidRelationsGainUntilTick(Find.TickManager.TicksGame + 1800000);
				}
			}
		}

		// Token: 0x040029A9 RID: 10665
		public ExtraFaction extraFaction;

		// Token: 0x040029AA RID: 10666
		public List<Pawn> affectedPawns = new List<Pawn>();

		// Token: 0x040029AB RID: 10667
		public bool areHelpers;

		// Token: 0x040029AC RID: 10668
		public string inSignalRemovePawn;

		// Token: 0x040029AD RID: 10669
		private const int RelationsGainAvailableInTicks = 1800000;
	}
}
