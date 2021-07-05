using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001185 RID: 4485
	public class CompRitualSeat : ThingComp
	{
		// Token: 0x06006BEC RID: 27628 RVA: 0x0024339C File Offset: 0x0024159C
		public override string CompInspectStringExtra()
		{
			string ideosString = this.GetIdeosString(null);
			if (string.IsNullOrEmpty(ideosString))
			{
				return null;
			}
			return "RitualSeatOf".Translate(ideosString.Named("IDEOS")).Resolve();
		}

		// Token: 0x06006BED RID: 27629 RVA: 0x002433D8 File Offset: 0x002415D8
		private string GetIdeosString(List<Ideo> outIdeos = null)
		{
			CompRitualSeat.tmpStringBuilder.Clear();
			foreach (Ideo ideo in Find.IdeoManager.IdeosInViewOrder)
			{
				bool flag = false;
				using (List<Precept>.Enumerator enumerator2 = ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Precept_RitualSeat precept_RitualSeat;
						if ((precept_RitualSeat = (enumerator2.Current as Precept_RitualSeat)) != null && precept_RitualSeat.ThingDef == this.parent.def)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					if (outIdeos != null)
					{
						outIdeos.Add(ideo);
					}
					if (CompRitualSeat.tmpStringBuilder.Length > 0)
					{
						CompRitualSeat.tmpStringBuilder.Append(", ");
					}
					CompRitualSeat.tmpStringBuilder.Append(ideo.name.ApplyTag(ideo).Resolve());
				}
			}
			return CompRitualSeat.tmpStringBuilder.ToString();
		}

		// Token: 0x06006BEE RID: 27630 RVA: 0x002434E8 File Offset: 0x002416E8
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompRitualSeat.tmpIdeos.Clear();
			string ideosString = this.GetIdeosString(CompRitualSeat.tmpIdeos);
			if (string.IsNullOrEmpty(ideosString))
			{
				yield break;
			}
			Dialog_InfoCard.Hyperlink[] array = new Dialog_InfoCard.Hyperlink[CompRitualSeat.tmpIdeos.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Dialog_InfoCard.Hyperlink(CompRitualSeat.tmpIdeos[i]);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Thing_RelatedToIdeos_Name".Translate(), ideosString, "Stat_Thing_RelatedToIdeos_Desc".Translate(), 1110, null, array, false);
			yield break;
		}

		// Token: 0x04003C0B RID: 15371
		private static StringBuilder tmpStringBuilder = new StringBuilder();

		// Token: 0x04003C0C RID: 15372
		private static List<Ideo> tmpIdeos = new List<Ideo>();
	}
}
