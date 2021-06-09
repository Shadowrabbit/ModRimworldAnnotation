using System;

namespace RimWorld
{
	// Token: 0x02001A04 RID: 6660
	public class Dialog_NamePlayerFaction : Dialog_GiveName
	{
		// Token: 0x06009345 RID: 37701 RVA: 0x002A6DA0 File Offset: 0x002A4FA0
		public Dialog_NamePlayerFaction()
		{
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, new Predicate<string>(this.IsValidName), false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.gainedNameMessageKey = "PlayerFactionGainsName";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
		}

		// Token: 0x06009346 RID: 37702 RVA: 0x00062A3C File Offset: 0x00060C3C
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x06009347 RID: 37703 RVA: 0x00062A44 File Offset: 0x00060C44
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}
	}
}
