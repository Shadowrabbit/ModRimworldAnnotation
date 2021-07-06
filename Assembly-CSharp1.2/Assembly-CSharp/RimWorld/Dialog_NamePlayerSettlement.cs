using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A08 RID: 6664
	public class Dialog_NamePlayerSettlement : Dialog_GiveName
	{
		// Token: 0x06009356 RID: 37718 RVA: 0x002A6FA0 File Offset: 0x002A51A0
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

		// Token: 0x06009357 RID: 37719 RVA: 0x00062B1D File Offset: 0x00060D1D
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.settlement.Map != null)
			{
				Current.Game.CurrentMap = this.settlement.Map;
			}
		}

		// Token: 0x06009358 RID: 37720 RVA: 0x00062AE1 File Offset: 0x00060CE1
		protected override bool IsValidName(string s)
		{
			return NamePlayerSettlementDialogUtility.IsValidName(s);
		}

		// Token: 0x06009359 RID: 37721 RVA: 0x00062B47 File Offset: 0x00060D47
		protected override void Named(string s)
		{
			NamePlayerSettlementDialogUtility.Named(this.settlement, s);
		}

		// Token: 0x04005D59 RID: 23897
		private Settlement settlement;
	}
}
