using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8F RID: 3983
	public class RitualRoleTag : RitualRole
	{
		// Token: 0x06005E5C RID: 24156 RVA: 0x00206058 File Offset: 0x00204258
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null)
		{
			if (ritual != null && p != null && p.Ideo != ritual.ideo)
			{
				reason = "MessageRitualRoleMustHaveIdeoToDoRole".Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle(ritual.ideo.memberName, false, false), Find.ActiveLanguageWorker.WithIndefiniteArticle(base.Label, false, false));
				return false;
			}
			if ((role == null || role.def.roleTags == null || !role.def.roleTags.Contains(this.tag)) && (!this.substitutable || p == null || ritual == null || p.Ideo != ritual.ideo))
			{
				if (p != null)
				{
					IEnumerable<PreceptDef> source = from d in DefDatabase<PreceptDef>.AllDefsListForReading
					where typeof(Precept_Role).IsAssignableFrom(d.preceptClass) && d.roleTags != null && d.roleTags.Contains(this.tag)
					select d;
					reason = "MessageRitualRoleRequired".Translate(p) + ": " + source.Select(delegate(PreceptDef r)
					{
						Precept_Ritual ritual2 = ritual;
						string text;
						if (ritual2 == null)
						{
							text = null;
						}
						else
						{
							Precept precept = ritual2.ideo.GetPrecept(r);
							text = ((precept != null) ? precept.LabelCap : null);
						}
						return text ?? r.LabelCap;
					}).ToCommaList(false, false);
				}
				else
				{
					reason = null;
				}
				return false;
			}
			if (p != null && !p.Faction.IsPlayerSafe())
			{
				reason = "MessageRitualRoleMustBeColonist".Translate(base.Label);
				return false;
			}
			reason = null;
			return true;
		}

		// Token: 0x06005E5D RID: 24157 RVA: 0x002061CA File Offset: 0x002043CA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
		}

		// Token: 0x04003680 RID: 13952
		public string tag;
	}
}
