using System;

namespace RimWorld
{
	// Token: 0x020012F5 RID: 4853
	public class Dialog_NamePlayerFaction : Dialog_GiveName
	{
		// Token: 0x060074A0 RID: 29856 RVA: 0x0027A2C8 File Offset: 0x002784C8
		public Dialog_NamePlayerFaction()
		{
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, new Predicate<string>(this.IsValidName), false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.gainedNameMessageKey = "PlayerFactionGainsName";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
		}

		// Token: 0x060074A1 RID: 29857 RVA: 0x0027A31F File Offset: 0x0027851F
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x060074A2 RID: 29858 RVA: 0x0027A327 File Offset: 0x00278527
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}
	}
}
