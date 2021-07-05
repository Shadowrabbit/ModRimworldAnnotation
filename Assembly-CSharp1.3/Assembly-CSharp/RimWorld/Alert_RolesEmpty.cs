using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001288 RID: 4744
	public class Alert_RolesEmpty : Alert
	{
		// Token: 0x06007155 RID: 29013 RVA: 0x0025C628 File Offset: 0x0025A828
		private void GetTargets()
		{
			this.emptyRoles.Clear();
			this.targets.Clear();
			this.targetLabels.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Role precept_Role;
					if (!precept.def.leaderRole && precept.def.createsRoleEmptyThought && (precept_Role = (precept as Precept_Role)) != null && precept_Role.ChosenPawnSingle() == null && precept_Role.Active)
					{
						this.emptyRoles.Add(precept_Role);
					}
				}
			}
			if (this.emptyRoles.Any<Precept_Role>())
			{
				using (List<Pawn>.Enumerator enumerator3 = Find.CurrentMap.mapPawns.FreeColonistsSpawned.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Pawn p = enumerator3.Current;
						if (!p.IsSlave && this.emptyRoles.Any((Precept_Role x) => x.ideo == p.Ideo))
						{
							this.targets.Add(p);
							this.targetLabels.Add(p.NameFullColored.Resolve());
						}
					}
				}
			}
		}

		// Token: 0x06007156 RID: 29014 RVA: 0x0025C7D4 File Offset: 0x0025A9D4
		public override string GetLabel()
		{
			if (this.emptyRoles.Count == 1)
			{
				return "IdeoRoleEmpty".Translate(this.emptyRoles[0].LabelCap);
			}
			return "IdeoRolesEmpty".Translate(this.emptyRoles.Count);
		}

		// Token: 0x06007157 RID: 29015 RVA: 0x0025C834 File Offset: 0x0025AA34
		public override TaggedString GetExplanation()
		{
			return "IdeoRolesEmptyDesc".Translate((from x in this.emptyRoles
			select x.LabelCap.ApplyTag(x.ideo).Resolve()).ToLineList(" -  ", false)) + ":\n" + this.targetLabels.ToLineList(" -  ");
		}

		// Token: 0x06007158 RID: 29016 RVA: 0x0025C8A4 File Offset: 0x0025AAA4
		public override AlertReport GetReport()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			if (GenDate.DaysPassed < 10)
			{
				return false;
			}
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04003E53 RID: 15955
		private List<Precept_Role> emptyRoles = new List<Precept_Role>();

		// Token: 0x04003E54 RID: 15956
		private List<string> targetLabels = new List<string>();

		// Token: 0x04003E55 RID: 15957
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04003E56 RID: 15958
		public const int DayEnable = 10;
	}
}
