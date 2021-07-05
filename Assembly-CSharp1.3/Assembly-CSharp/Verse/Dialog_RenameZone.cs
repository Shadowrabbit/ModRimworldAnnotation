using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000447 RID: 1095
	public class Dialog_RenameZone : Dialog_Rename
	{
		// Token: 0x0600213A RID: 8506 RVA: 0x000CFF39 File Offset: 0x000CE139
		public Dialog_RenameZone(Zone zone)
		{
			this.zone = zone;
			this.curName = zone.label;
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x000CFF54 File Offset: 0x000CE154
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

		// Token: 0x0600213C RID: 8508 RVA: 0x000CFFCB File Offset: 0x000CE1CB
		protected override void SetName(string name)
		{
			this.zone.label = this.curName;
			Messages.Message("ZoneGainsName".Translate(this.curName), MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x0400149E RID: 5278
		private Zone zone;
	}
}
