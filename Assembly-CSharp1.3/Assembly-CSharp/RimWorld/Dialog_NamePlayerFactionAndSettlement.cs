using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F7 RID: 4855
	public class Dialog_NamePlayerFactionAndSettlement : Dialog_GiveName
	{
		// Token: 0x060074A6 RID: 29862 RVA: 0x0027A3FC File Offset: 0x002785FC
		public Dialog_NamePlayerFactionAndSettlement(Settlement settlement)
		{
			this.settlement = settlement;
			if (settlement.HasMap && settlement.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				this.suggestingPawn = settlement.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			}
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, new Predicate<string>(this.IsValidName), false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
			this.useSecondName = true;
			this.secondNameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, new Predicate<string>(this.IsValidSecondName), false, null, null));
			this.curSecondName = this.secondNameGenerator();
			this.secondNameMessageKey = "NamePlayerFactionBaseMessage_NameFactionContinuation";
			this.invalidSecondNameMessageKey = "PlayerFactionBaseNameIsInvalid";
			this.gainedNameMessageKey = "PlayerFactionAndBaseGainsName";
		}

		// Token: 0x060074A7 RID: 29863 RVA: 0x0027A4CF File Offset: 0x002786CF
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.settlement.Map != null)
			{
				Current.Game.CurrentMap = this.settlement.Map;
			}
		}

		// Token: 0x060074A8 RID: 29864 RVA: 0x0027A31F File Offset: 0x0027851F
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x060074A9 RID: 29865 RVA: 0x0027A4F9 File Offset: 0x002786F9
		protected override bool IsValidSecondName(string s)
		{
			return NamePlayerSettlementDialogUtility.IsValidName(s);
		}

		// Token: 0x060074AA RID: 29866 RVA: 0x0027A327 File Offset: 0x00278527
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}

		// Token: 0x060074AB RID: 29867 RVA: 0x0027A501 File Offset: 0x00278701
		protected override void NamedSecond(string s)
		{
			NamePlayerSettlementDialogUtility.Named(this.settlement, s);
		}

		// Token: 0x0400404C RID: 16460
		private Settlement settlement;
	}
}
