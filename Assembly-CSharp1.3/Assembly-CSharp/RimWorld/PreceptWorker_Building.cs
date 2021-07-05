using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EEC RID: 3820
	public class PreceptWorker_Building : PreceptWorker
	{
		// Token: 0x06005AB4 RID: 23220 RVA: 0x001F5CE8 File Offset: 0x001F3EE8
		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo)
		{
			bool flag = false;
			using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Precept_Building precept_Building;
					if ((precept_Building = (enumerator.Current as Precept_Building)) != null && precept_Building.ThingDef != null && precept_Building.ThingDef.isAltar)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag && def.isAltar)
			{
				return new AcceptanceReport("IdeoAlreadyHasAltar".Translate());
			}
			if (!flag)
			{
				return def.isAltar;
			}
			if (!def.isAltar)
			{
				return true;
			}
			if (flag && def.isAltar)
			{
				return false;
			}
			return true;
		}
	}
}
