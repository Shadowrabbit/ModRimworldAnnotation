using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CC RID: 4300
	public class QuestPart_ExtraFaction : QuestPartActivable
	{
		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06005DC0 RID: 24000 RVA: 0x00041025 File Offset: 0x0003F225
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

		// Token: 0x06005DC1 RID: 24001 RVA: 0x001DD7F4 File Offset: 0x001DB9F4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.affectedPawns.Contains(item))
			{
				this.affectedPawns.Remove(item);
			}
		}

		// Token: 0x06005DC2 RID: 24002 RVA: 0x00041035 File Offset: 0x0003F235
		public override bool QuestPartReserves(Faction f)
		{
			return this.extraFaction.faction == f;
		}

		// Token: 0x06005DC3 RID: 24003 RVA: 0x001DD84C File Offset: 0x001DBA4C
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

		// Token: 0x06005DC4 RID: 24004 RVA: 0x00041045 File Offset: 0x0003F245
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.affectedPawns.Replace(replace, with);
		}

		// Token: 0x06005DC5 RID: 24005 RVA: 0x00041055 File Offset: 0x0003F255
		public override void Cleanup()
		{
			base.Cleanup();
			this.SetRelationsGainTickForPawns();
		}

		// Token: 0x06005DC6 RID: 24006 RVA: 0x00041063 File Offset: 0x0003F263
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.extraFaction.faction == faction)
			{
				this.extraFaction.faction = null;
			}
		}

		// Token: 0x06005DC7 RID: 24007 RVA: 0x0004107F File Offset: 0x0003F27F
		protected override void Disable()
		{
			base.Disable();
			this.SetRelationsGainTickForPawns();
		}

		// Token: 0x06005DC8 RID: 24008 RVA: 0x001DD8E4 File Offset: 0x001DBAE4
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

		// Token: 0x04003EB5 RID: 16053
		public ExtraFaction extraFaction;

		// Token: 0x04003EB6 RID: 16054
		public List<Pawn> affectedPawns = new List<Pawn>();

		// Token: 0x04003EB7 RID: 16055
		public bool areHelpers;

		// Token: 0x04003EB8 RID: 16056
		public string inSignalRemovePawn;

		// Token: 0x04003EB9 RID: 16057
		private const int RelationsGainAvailableInTicks = 1800000;
	}
}
