using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000449 RID: 1097
	public class Dialog_RenameAnimalPen : Dialog_Rename
	{
		// Token: 0x06002140 RID: 8512 RVA: 0x000D00AA File Offset: 0x000CE2AA
		public Dialog_RenameAnimalPen(Map map, CompAnimalPenMarker marker)
		{
			this.map = map;
			this.marker = marker;
			this.curName = marker.label;
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000D00CC File Offset: 0x000CE2CC
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (name != this.marker.label && this.map.animalPenManager.GetPenNamed(name) != null)
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x000D0128 File Offset: 0x000CE328
		protected override void SetName(string name)
		{
			this.marker.label = name;
		}

		// Token: 0x040014A0 RID: 5280
		private readonly Map map;

		// Token: 0x040014A1 RID: 5281
		private readonly CompAnimalPenMarker marker;
	}
}
