using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013DB RID: 5083
	public static class TutorSystem
	{
		// Token: 0x170015A8 RID: 5544
		// (get) Token: 0x06007B9C RID: 31644 RVA: 0x002B90DD File Offset: 0x002B72DD
		public static bool TutorialMode
		{
			get
			{
				return Find.Storyteller != null && Find.Storyteller.def != null && Find.Storyteller.def.tutorialMode;
			}
		}

		// Token: 0x170015A9 RID: 5545
		// (get) Token: 0x06007B9D RID: 31645 RVA: 0x002B9103 File Offset: 0x002B7303
		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return Prefs.AdaptiveTrainingEnabled && (Find.Storyteller == null || Find.Storyteller.def == null || !Find.Storyteller.def.disableAdaptiveTraining);
			}
		}

		// Token: 0x06007B9E RID: 31646 RVA: 0x002B9135 File Offset: 0x002B7335
		public static void Notify_Event(string eventTag, IntVec3 cell)
		{
			TutorSystem.Notify_Event(new EventPack(eventTag, cell));
		}

		// Token: 0x06007B9F RID: 31647 RVA: 0x002B9144 File Offset: 0x002B7344
		public static void Notify_Event(EventPack ep)
		{
			if (!TutorSystem.TutorialMode)
			{
				return;
			}
			if (DebugViewSettings.logTutor)
			{
				Log.Message("Notify_Event: " + ep);
			}
			if (Current.Game == null)
			{
				return;
			}
			Lesson lesson = Find.ActiveLesson.Current;
			if (Find.ActiveLesson.Current != null)
			{
				Find.ActiveLesson.Current.Notify_Event(ep);
			}
			foreach (InstructionDef instructionDef in DefDatabase<InstructionDef>.AllDefs)
			{
				if (instructionDef.eventTagInitiate == ep.Tag && (instructionDef.eventTagInitiateSource == null || (lesson != null && instructionDef.eventTagInitiateSource == lesson.Instruction)) && (TutorSystem.TutorialMode || !instructionDef.tutorialModeOnly))
				{
					Find.ActiveLesson.Activate(instructionDef);
					break;
				}
			}
		}

		// Token: 0x06007BA0 RID: 31648 RVA: 0x002B9228 File Offset: 0x002B7428
		public static bool AllowAction(EventPack ep)
		{
			if (!TutorSystem.TutorialMode)
			{
				return true;
			}
			if (DebugViewSettings.logTutor)
			{
				Log.Message("AllowAction: " + ep);
			}
			if (ep.Cells != null && ep.Cells.Count<IntVec3>() == 1)
			{
				return TutorSystem.AllowAction(new EventPack(ep.Tag, ep.Cells.First<IntVec3>()));
			}
			if (Find.ActiveLesson.Current != null)
			{
				AcceptanceReport acceptanceReport = Find.ActiveLesson.Current.AllowAction(ep);
				if (!acceptanceReport.Accepted)
				{
					Messages.Message((!acceptanceReport.Reason.NullOrEmpty()) ? acceptanceReport.Reason : Find.ActiveLesson.Current.DefaultRejectInputMessage, MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			return true;
		}
	}
}
