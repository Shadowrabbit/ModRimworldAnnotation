using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E99 RID: 3737
	public class Thought_IdeoRoleApparelRequirementNotMet : Thought_Situational
	{
		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x060057CB RID: 22475 RVA: 0x001DDA40 File Offset: 0x001DBC40
		public Precept_Role Role
		{
			get
			{
				return (Precept_Role)this.sourcePrecept;
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x060057CC RID: 22476 RVA: 0x001DDC6B File Offset: 0x001DBE6B
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.Role.Named("ROLE"));
			}
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x060057CD RID: 22477 RVA: 0x001DDC94 File Offset: 0x001DBE94
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Role.Named("ROLE")) + ":\n\n" + this.GetAllRequiredApparel(this.pawn).ToLineList(" -  ");
			}
		}

		// Token: 0x060057CE RID: 22478 RVA: 0x001DDCEB File Offset: 0x001DBEEB
		protected override ThoughtState CurrentStateInternal()
		{
			return this.Role.ChosenPawnSingle() == this.pawn && this.GetAllRequiredApparel(this.pawn).Count > 0;
		}

		// Token: 0x060057CF RID: 22479 RVA: 0x001DDD1C File Offset: 0x001DBF1C
		private List<string> GetAllRequiredApparel(Pawn p)
		{
			this.unmetReqsListTmp.Clear();
			if (ModsConfig.IdeologyActive)
			{
				Ideo ideo = p.Ideo;
				Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(p) : null;
				if (precept_Role != null && !precept_Role.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
				{
					for (int i = 0; i < precept_Role.apparelRequirements.Count; i++)
					{
						ApparelRequirement requirement = precept_Role.apparelRequirements[i].requirement;
						if (requirement.IsActive(p) && !requirement.IsMet(p))
						{
							if (!requirement.groupLabel.NullOrEmpty())
							{
								this.unmetReqsListTmp.Add(requirement.groupLabel);
							}
							else
							{
								foreach (ThingDef thingDef in requirement.AllRequiredApparelForPawn(p, false, false))
								{
									this.unmetReqsListTmp.Add(thingDef.LabelCap);
								}
							}
						}
					}
				}
			}
			return this.unmetReqsListTmp;
		}

		// Token: 0x040033C7 RID: 13255
		private List<string> unmetReqsListTmp = new List<string>();
	}
}
