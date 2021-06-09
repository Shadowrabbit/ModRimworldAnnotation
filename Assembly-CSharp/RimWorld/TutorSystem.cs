using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BEB RID: 7147
	public static class TutorSystem
	{
		// Token: 0x170018B2 RID: 6322
		// (get) Token: 0x06009D46 RID: 40262 RVA: 0x00068B42 File Offset: 0x00066D42
		public static bool TutorialMode
		{
			get
			{
				return Find.Storyteller != null && Find.Storyteller.def != null && Find.Storyteller.def.tutorialMode;
			}
		}

		// Token: 0x170018B3 RID: 6323
		// (get) Token: 0x06009D47 RID: 40263 RVA: 0x00068B68 File Offset: 0x00066D68
		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return Prefs.AdaptiveTrainingEnabled && (Find.Storyteller == null || Find.Storyteller.def == null || !Find.Storyteller.def.disableAdaptiveTraining);
			}
		}

		// Token: 0x06009D48 RID: 40264 RVA: 0x00068B9A File Offset: 0x00066D9A
		public static void Notify_Event(string eventTag, IntVec3 cell)
		{
			TutorSystem.Notify_Event(new EventPack(eventTag, cell));
		}

		// Token: 0x06009D49 RID: 40265 RVA: 0x002DFE2C File Offset: 0x002DE02C
		public static void Notify_Event(EventPack ep)
		{
			if (!TutorSystem.TutorialMode)
			{
				return;
			}
			if (DebugViewSettings.logTutor)
			{
				Log.Message("Notify_Event: " + ep, false);
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

		// Token: 0x06009D4A RID: 40266 RVA: 0x002DFF10 File Offset: 0x002DE110
		public static bool AllowAction(EventPack ep)
		{
			if (!TutorSystem.TutorialMode)
			{
				return true;
			}
			if (DebugViewSettings.logTutor)
			{
				Log.Message("AllowAction: " + ep, false);
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
