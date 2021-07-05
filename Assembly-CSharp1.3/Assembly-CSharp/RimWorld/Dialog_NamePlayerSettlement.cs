using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F8 RID: 4856
	public class Dialog_NamePlayerSettlement : Dialog_GiveName
	{
		// Token: 0x060074AE RID: 29870 RVA: 0x0027A538 File Offset: 0x00278738
		public Dialog_NamePlayerSettlement(Settlement settlement)
		{
			this.settlement = settlement;
			if (settlement.HasMap && settlement.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				this.suggestingPawn = settlement.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			}
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, new Predicate<string>(this.IsValidName), false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionBaseMessage";
			this.gainedNameMessageKey = "PlayerFactionBaseGainsName";
			this.invalidNameMessageKey = "PlayerFactionBaseNameIsInvalid";
		}

		// Token: 0x060074AF RID: 29871 RVA: 0x0027A5CB File Offset: 0x002787CB
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.settlement.Map != null)
			{
				Current.Game.CurrentMap = this.settlement.Map;
			}
		}

		// Token: 0x060074B0 RID: 29872 RVA: 0x0027A4F9 File Offset: 0x002786F9
		protected override bool IsValidName(string s)
		{
			return NamePlayerSettlementDialogUtility.IsValidName(s);
		}

		// Token: 0x060074B1 RID: 29873 RVA: 0x0027A5F5 File Offset: 0x002787F5
		protected override void Named(string s)
		{
			NamePlayerSettlementDialogUtility.Named(this.settlement, s);
		}

		// Token: 0x0400404D RID: 16461
		private Settlement settlement;
	}
}
