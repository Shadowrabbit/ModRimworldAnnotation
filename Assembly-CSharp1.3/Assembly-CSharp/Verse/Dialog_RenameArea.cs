using System;

namespace Verse
{
	// Token: 0x02000448 RID: 1096
	public class Dialog_RenameArea : Dialog_Rename
	{
		// Token: 0x0600213D RID: 8509 RVA: 0x000D0003 File Offset: 0x000CE203
		public Dialog_RenameArea(Area area)
		{
			this.area = area;
			this.curName = area.Label;
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x000D0020 File Offset: 0x000CE220
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (this.area.Map.areaManager.AllAreas.Any((Area a) => a != this.area && a.Label == name))
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x000D0097 File Offset: 0x000CE297
		protected override void SetName(string name)
		{
			this.area.SetLabel(this.curName);
		}

		// Token: 0x0400149F RID: 5279
		private Area area;
	}
}
