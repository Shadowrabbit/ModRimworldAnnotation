using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F21 RID: 3873
	public class RitualBehaviorDef : Def
	{
		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x06005C1D RID: 23581 RVA: 0x001FCBA0 File Offset: 0x001FADA0
		public RitualPosition_Lectern FirstLecternPosition
		{
			get
			{
				foreach (RitualStage ritualStage in this.stages)
				{
					if (ritualStage.roleBehaviors != null)
					{
						foreach (RitualRoleBehavior ritualRoleBehavior in ritualStage.roleBehaviors)
						{
							if (ritualRoleBehavior.CustomPositionsForReading != null)
							{
								using (List<RitualPosition>.Enumerator enumerator3 = ritualRoleBehavior.CustomPositionsForReading.GetEnumerator())
								{
									while (enumerator3.MoveNext())
									{
										RitualPosition_Lectern result;
										if ((result = (enumerator3.Current as RitualPosition_Lectern)) != null)
										{
											return result;
										}
									}
								}
							}
						}
					}
				}
				return null;
			}
		}

		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x06005C1E RID: 23582 RVA: 0x001FCC8C File Offset: 0x001FAE8C
		public bool UsesLectern
		{
			get
			{
				return this.FirstLecternPosition != null;
			}
		}

		// Token: 0x06005C1F RID: 23583 RVA: 0x001FCC97 File Offset: 0x001FAE97
		public RitualBehaviorWorker GetInstance()
		{
			return (RitualBehaviorWorker)Activator.CreateInstance(this.workerClass, new object[]
			{
				this
			});
		}

		// Token: 0x06005C20 RID: 23584 RVA: 0x001FCCB3 File Offset: 0x001FAEB3
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.durationTicks.min <= 0 || this.durationTicks.max <= 0)
			{
				yield return "durationTicks must always resolve into a positive number";
			}
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			using (List<RitualStage>.Enumerator enumerator2 = this.stages.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.endTriggers.NullOrEmpty<StageEndTrigger>())
					{
						yield return "ritual stage with no endTrigger";
					}
				}
			}
			List<RitualStage>.Enumerator enumerator2 = default(List<RitualStage>.Enumerator);
			if (!string.IsNullOrEmpty(this.spectatorsLabel) && string.IsNullOrEmpty(this.spectatorGerund))
			{
				yield return "ritual has spectatorLabel but no spectatorGerund";
			}
			if (!string.IsNullOrEmpty(this.spectatorGerund) && string.IsNullOrEmpty(this.spectatorsLabel))
			{
				yield return "ritual has spectatorGerund but no spectatorLabel";
			}
			if (!this.roles.NullOrEmpty<RitualRole>())
			{
				if (this.roles.Count((RitualRole x) => x.defaultForSelectedColonist) > 1)
				{
					yield return ">1 default role for selected pawn";
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x001FCCC4 File Offset: 0x001FAEC4
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.stages.NullOrEmpty<RitualStage>())
			{
				return;
			}
			for (int i = 0; i < this.stages.Count; i++)
			{
				this.stages[i].parent = this;
			}
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x001FCD0D File Offset: 0x001FAF0D
		public IEnumerable<RitualRole> RequiredRoles()
		{
			if (this.roles.NullOrEmpty<RitualRole>())
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.roles.Count; i = num + 1)
			{
				if (this.roles[i].required)
				{
					yield return this.roles[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x040035AA RID: 13738
		public Type workerClass = typeof(RitualBehaviorWorker);

		// Token: 0x040035AB RID: 13739
		public List<RitualRole> roles;

		// Token: 0x040035AC RID: 13740
		public List<RitualStage> stages;

		// Token: 0x040035AD RID: 13741
		public IntRange durationTicks = new IntRange(7500, 7500);

		// Token: 0x040035AE RID: 13742
		public List<PreceptRequirement> preceptRequirements;

		// Token: 0x040035AF RID: 13743
		public List<RitualCancellationTrigger> cancellationTriggers;

		// Token: 0x040035B0 RID: 13744
		public int displayOrder;

		// Token: 0x040035B1 RID: 13745
		[MustTranslate]
		public string letterTitle;

		// Token: 0x040035B2 RID: 13746
		[MustTranslate]
		public string letterText;

		// Token: 0x040035B3 RID: 13747
		[MustTranslate]
		public string spectatorsLabel;

		// Token: 0x040035B4 RID: 13748
		[MustTranslate]
		public string spectatorGerund;

		// Token: 0x040035B5 RID: 13749
		public List<SoundDef> soundDefsPerEnhancerCount;

		// Token: 0x040035B6 RID: 13750
		public int maxEnhancerDistance;

		// Token: 0x040035B7 RID: 13751
		public RitualSpectatorFilter spectatorFilter;
	}
}
