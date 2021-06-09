using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153F RID: 5439
	public class Pawn_SkillTracker : IExposable
	{
		// Token: 0x060075C9 RID: 30153 RVA: 0x0023D878 File Offset: 0x0023BA78
		public Pawn_SkillTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			foreach (SkillDef def in DefDatabase<SkillDef>.AllDefs)
			{
				this.skills.Add(new SkillRecord(this.pawn, def));
			}
		}

		// Token: 0x060075CA RID: 30154 RVA: 0x0023D8F4 File Offset: 0x0023BAF4
		public void ExposeData()
		{
			Scribe_Collections.Look<SkillRecord>(ref this.skills, "skills", LookMode.Deep, new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.lastXpSinceMidnightResetTimestamp, "lastXpSinceMidnightResetTimestamp", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.skills.RemoveAll((SkillRecord x) => x == null) != 0)
				{
					Log.Error("Some skills were null after loading for " + this.pawn.ToStringSafe<Pawn>(), false);
				}
				if (this.skills.RemoveAll((SkillRecord x) => x.def == null) != 0)
				{
					Log.Error("Some skills had null def after loading for " + this.pawn.ToStringSafe<Pawn>(), false);
				}
				List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					bool flag = false;
					for (int j = 0; j < this.skills.Count; j++)
					{
						if (this.skills[j].def == allDefsListForReading[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Log.Warning(this.pawn.ToStringSafe<Pawn>() + " had no " + allDefsListForReading[i].ToStringSafe<SkillDef>() + " skill. Adding.", false);
						this.skills.Add(new SkillRecord(this.pawn, allDefsListForReading[i]));
					}
				}
			}
		}

		// Token: 0x060075CB RID: 30155 RVA: 0x0023DA6C File Offset: 0x0023BC6C
		public SkillRecord GetSkill(SkillDef skillDef)
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				if (this.skills[i].def == skillDef)
				{
					return this.skills[i];
				}
			}
			Log.Error(string.Concat(new object[]
			{
				"Did not find skill of def ",
				skillDef,
				", returning ",
				this.skills[0]
			}), false);
			return this.skills[0];
		}

		// Token: 0x060075CC RID: 30156 RVA: 0x0023DAF0 File Offset: 0x0023BCF0
		public void SkillsTick()
		{
			if (this.pawn.IsHashIntervalTick(200))
			{
				if (GenLocalDate.HourInteger(this.pawn) == 0 && (this.lastXpSinceMidnightResetTimestamp < 0 || Find.TickManager.TicksGame - this.lastXpSinceMidnightResetTimestamp >= 30000))
				{
					for (int i = 0; i < this.skills.Count; i++)
					{
						this.skills[i].xpSinceMidnight = 0f;
					}
					this.lastXpSinceMidnightResetTimestamp = Find.TickManager.TicksGame;
				}
				for (int j = 0; j < this.skills.Count; j++)
				{
					this.skills[j].Interval();
				}
			}
		}

		// Token: 0x060075CD RID: 30157 RVA: 0x0004F721 File Offset: 0x0004D921
		public void Learn(SkillDef sDef, float xp, bool direct = false)
		{
			this.GetSkill(sDef).Learn(xp, direct);
		}

		// Token: 0x060075CE RID: 30158 RVA: 0x0023DBA4 File Offset: 0x0023BDA4
		public float AverageOfRelevantSkillsFor(WorkTypeDef workDef)
		{
			if (workDef.relevantSkills.Count == 0)
			{
				return 3f;
			}
			float num = 0f;
			for (int i = 0; i < workDef.relevantSkills.Count; i++)
			{
				num += (float)this.GetSkill(workDef.relevantSkills[i]).Level;
			}
			return num / (float)workDef.relevantSkills.Count;
		}

		// Token: 0x060075CF RID: 30159 RVA: 0x0023DC0C File Offset: 0x0023BE0C
		public Passion MaxPassionOfRelevantSkillsFor(WorkTypeDef workDef)
		{
			if (workDef.relevantSkills.Count == 0)
			{
				return Passion.None;
			}
			Passion passion = Passion.None;
			for (int i = 0; i < workDef.relevantSkills.Count; i++)
			{
				Passion passion2 = this.GetSkill(workDef.relevantSkills[i]).passion;
				if (passion2 > passion)
				{
					passion = passion2;
				}
			}
			return passion;
		}

		// Token: 0x060075D0 RID: 30160 RVA: 0x0023DC60 File Offset: 0x0023BE60
		public void Notify_SkillDisablesChanged()
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				this.skills[i].Notify_SkillDisablesChanged();
			}
		}

		// Token: 0x04004DB0 RID: 19888
		private Pawn pawn;

		// Token: 0x04004DB1 RID: 19889
		public List<SkillRecord> skills = new List<SkillRecord>();

		// Token: 0x04004DB2 RID: 19890
		private int lastXpSinceMidnightResetTimestamp = -1;
	}
}
