using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003FB RID: 1019
	public class Command_VerbTarget : Command
	{
		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001E9F RID: 7839 RVA: 0x000BF5BE File Offset: 0x000BD7BE
		public override Color IconDrawColor
		{
			get
			{
				if (this.verb.EquipmentSource != null)
				{
					return this.verb.EquipmentSource.DrawColor;
				}
				return base.IconDrawColor;
			}
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x000BF5E4 File Offset: 0x000BD7E4
		public override void GizmoUpdateOnMouseover()
		{
			if (!this.drawRadius)
			{
				return;
			}
			this.verb.verbProps.DrawRadiusRing(this.verb.caster.Position);
			if (!this.groupedVerbs.NullOrEmpty<Verb>())
			{
				foreach (Verb verb in this.groupedVerbs)
				{
					verb.verbProps.DrawRadiusRing(verb.caster.Position);
				}
			}
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000BF67C File Offset: 0x000BD87C
		public override void MergeWith(Gizmo other)
		{
			base.MergeWith(other);
			Command_VerbTarget command_VerbTarget = other as Command_VerbTarget;
			if (command_VerbTarget == null)
			{
				Log.ErrorOnce("Tried to merge Command_VerbTarget with unexpected type", 73406263);
				return;
			}
			if (this.groupedVerbs == null)
			{
				this.groupedVerbs = new List<Verb>();
			}
			this.groupedVerbs.Add(command_VerbTarget.verb);
			if (command_VerbTarget.groupedVerbs != null)
			{
				this.groupedVerbs.AddRange(command_VerbTarget.groupedVerbs);
			}
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000BF6E8 File Offset: 0x000BD8E8
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Targeter targeter = Find.Targeter;
			if (this.verb.CasterIsPawn && targeter.targetingSource != null && targeter.targetingSource.GetVerb.verbProps == this.verb.verbProps)
			{
				Pawn casterPawn = this.verb.CasterPawn;
				if (!targeter.IsPawnTargeting(casterPawn))
				{
					targeter.targetingSourceAdditionalPawns.Add(casterPawn);
					return;
				}
			}
			else
			{
				Find.Targeter.BeginTargeting(this.verb, null);
			}
		}

		// Token: 0x040012A4 RID: 4772
		public Verb verb;

		// Token: 0x040012A5 RID: 4773
		private List<Verb> groupedVerbs;

		// Token: 0x040012A6 RID: 4774
		public bool drawRadius = true;
	}
}
