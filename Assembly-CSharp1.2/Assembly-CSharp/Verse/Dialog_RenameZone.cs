using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000790 RID: 1936
	public class Dialog_RenameZone : Dialog_Rename
	{
		// Token: 0x060030DF RID: 12511 RVA: 0x000267BC File Offset: 0x000249BC
		public Dialog_RenameZone(Zone zone)
		{
			this.zone = zone;
			this.curName = zone.label;
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x001437EC File Offset: 0x001419EC
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (this.zone.Map.zoneManager.AllZones.Any((Zone z) => z != this.zone && z.label == name))
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x000267D7 File Offset: 0x000249D7
		protected override void SetName(string name)
		{
			this.zone.label = this.curName;
			Messages.Message("ZoneGainsName".Translate(this.curName), MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x040021A1 RID: 8609
		private Zone zone;
	}
}
