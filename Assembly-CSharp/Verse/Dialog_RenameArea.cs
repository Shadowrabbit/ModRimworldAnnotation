using System;

namespace Verse
{
	// Token: 0x02000792 RID: 1938
	public class Dialog_RenameArea : Dialog_Rename
	{
		// Token: 0x060030E4 RID: 12516 RVA: 0x00026832 File Offset: 0x00024A32
		public Dialog_RenameArea(Area area)
		{
			this.area = area;
			this.curName = area.Label;
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x00143864 File Offset: 0x00141A64
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

		// Token: 0x060030E6 RID: 12518 RVA: 0x0002684D File Offset: 0x00024A4D
		protected override void SetName(string name)
		{
			this.area.SetLabel(this.curName);
		}

		// Token: 0x040021A4 RID: 8612
		private Area area;
	}
}
