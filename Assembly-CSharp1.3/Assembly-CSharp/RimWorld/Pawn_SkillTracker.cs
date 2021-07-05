using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E82 RID: 3714
	public class Pawn_SkillTracker : IExposable
	{
		// Token: 0x060056FA RID: 22266 RVA: 0x001D8380 File Offset: 0x001D6580
		public Pawn_SkillTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			foreach (SkillDef def in DefDatabase<SkillDef>.AllDefs)
			{
				this.skills.Add(new SkillRecord(this.pawn, def));
			}
		}

		// Token: 0x060056FB RID: 22267 RVA: 0x001D83FC File Offset: 0x001D65FC
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
					Log.Error("Some skills were null after loading for " + this.pawn.ToStringSafe<Pawn>());
				}
				if (this.skills.RemoveAll((SkillRecord x) => x.def == null) != 0)
				{
					Log.Error("Some skills had null def after loading for " + this.pawn.ToStringSafe<Pawn>());
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
						Log.Warning(this.pawn.ToStringSafe<Pawn>() + " had no " + allDefsListForReading[i].ToStringSafe<SkillDef>() + " skill. Adding.");
						this.skills.Add(new SkillRecord(this.pawn, allDefsListForReading[i]));
					}
				}
			}
		}

		// Token: 0x060056FC RID: 22268 RVA: 0x001D8570 File Offset: 0x001D6770
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
			}));
			return this.skills[0];
		}

		// Token: 0x060056FD RID: 22269 RVA: 0x001D85F4 File Offset: 0x001D67F4
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

		// Token: 0x060056FE RID: 22270 RVA: 0x001D86A7 File Offset: 0x001D68A7
		public void Learn(SkillDef sDef, float xp, bool direct = false)
		{
			this.GetSkill(sDef).Learn(xp, direct);
		}

		// Token: 0x060056FF RID: 22271 RVA: 0x001D86B8 File Offset: 0x001D68B8
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

		// Token: 0x06005700 RID: 22272 RVA: 0x001D8720 File Offset: 0x001D6920
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

		// Token: 0x06005701 RID: 22273 RVA: 0x001D8774 File Offset: 0x001D6974
		public void Notify_SkillDisablesChanged()
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				this.skills[i].Notify_SkillDisablesChanged();
			}
		}

		// Token: 0x04003357 RID: 13143
		private Pawn pawn;

		// Token: 0x04003358 RID: 13144
		public List<SkillRecord> skills = new List<SkillRecord>();

		// Token: 0x04003359 RID: 13145
		private int lastXpSinceMidnightResetTimestamp = -1;
	}
}
