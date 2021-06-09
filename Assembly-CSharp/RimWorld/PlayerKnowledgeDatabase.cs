using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE6 RID: 7142
	public static class PlayerKnowledgeDatabase
	{
		// Token: 0x06009D30 RID: 40240 RVA: 0x000689E0 File Offset: 0x00066BE0
		static PlayerKnowledgeDatabase()
		{
			PlayerKnowledgeDatabase.ReloadAndRebind();
		}

		// Token: 0x06009D31 RID: 40241 RVA: 0x002DFA20 File Offset: 0x002DDC20
		public static void ReloadAndRebind()
		{
			PlayerKnowledgeDatabase.data = DirectXmlLoader.ItemFromXmlFile<PlayerKnowledgeDatabase.ConceptKnowledge>(GenFilePaths.ConceptKnowledgeFilePath, true);
			foreach (ConceptDef conceptDef in DefDatabase<ConceptDef>.AllDefs)
			{
				if (!PlayerKnowledgeDatabase.data.knowledge.ContainsKey(conceptDef.defName))
				{
					Log.Warning("Knowledge data was missing key " + conceptDef + ". Adding it...", false);
					PlayerKnowledgeDatabase.data.knowledge.Add(conceptDef.defName, 0f);
				}
			}
		}

		// Token: 0x06009D32 RID: 40242 RVA: 0x002DFABC File Offset: 0x002DDCBC
		public static void ResetPersistent()
		{
			FileInfo fileInfo = new FileInfo(GenFilePaths.ConceptKnowledgeFilePath);
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
			PlayerKnowledgeDatabase.data = new PlayerKnowledgeDatabase.ConceptKnowledge();
		}

		// Token: 0x06009D33 RID: 40243 RVA: 0x002DFAEC File Offset: 0x002DDCEC
		public static void Save()
		{
			try
			{
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(PlayerKnowledgeDatabase.data, typeof(PlayerKnowledgeDatabase.ConceptKnowledge));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.ConceptKnowledgeFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.ConceptKnowledgeFilePath, ex.ToString()));
				Log.Error("Exception saving knowledge database: " + ex, false);
			}
		}

		// Token: 0x06009D34 RID: 40244 RVA: 0x000689E7 File Offset: 0x00066BE7
		public static float GetKnowledge(ConceptDef def)
		{
			return PlayerKnowledgeDatabase.data.knowledge[def.defName];
		}

		// Token: 0x06009D35 RID: 40245 RVA: 0x002DFB74 File Offset: 0x002DDD74
		public static void SetKnowledge(ConceptDef def, float value)
		{
			float num = PlayerKnowledgeDatabase.data.knowledge[def.defName];
			float num2 = Mathf.Clamp01(value);
			PlayerKnowledgeDatabase.data.knowledge[def.defName] = num2;
			if (num < 0.999f && num2 >= 0.999f)
			{
				PlayerKnowledgeDatabase.NewlyLearned(def);
			}
		}

		// Token: 0x06009D36 RID: 40246 RVA: 0x000689FE File Offset: 0x00066BFE
		public static bool IsComplete(ConceptDef conc)
		{
			return PlayerKnowledgeDatabase.data.knowledge[conc.defName] > 0.999f;
		}

		// Token: 0x06009D37 RID: 40247 RVA: 0x00068A1C File Offset: 0x00066C1C
		private static void NewlyLearned(ConceptDef conc)
		{
			TutorSystem.Notify_Event("ConceptLearned-" + conc.defName);
			if (Find.Tutor != null)
			{
				Find.Tutor.learningReadout.Notify_ConceptNewlyLearned(conc);
			}
		}

		// Token: 0x06009D38 RID: 40248 RVA: 0x002DFBC8 File Offset: 0x002DDDC8
		public static void KnowledgeDemonstrated(ConceptDef conc, KnowledgeAmount know)
		{
			float num;
			switch (know)
			{
			case KnowledgeAmount.FrameDisplayed:
				num = ((Event.current.type == EventType.Repaint) ? 0.004f : 0f);
				break;
			case KnowledgeAmount.FrameInteraction:
				num = 0.008f;
				break;
			case KnowledgeAmount.TinyInteraction:
				num = 0.03f;
				break;
			case KnowledgeAmount.SmallInteraction:
				num = 0.1f;
				break;
			case KnowledgeAmount.SpecificInteraction:
				num = 0.4f;
				break;
			case KnowledgeAmount.Total:
				num = 1f;
				break;
			case KnowledgeAmount.NoteClosed:
				num = 0.5f;
				break;
			case KnowledgeAmount.NoteTaught:
				num = 1f;
				break;
			default:
				throw new NotImplementedException();
			}
			if (num <= 0f)
			{
				return;
			}
			PlayerKnowledgeDatabase.SetKnowledge(conc, PlayerKnowledgeDatabase.GetKnowledge(conc) + num);
			LessonAutoActivator.Notify_KnowledgeDemonstrated(conc);
			if (Find.ActiveLesson != null)
			{
				Find.ActiveLesson.Notify_KnowledgeDemonstrated(conc);
			}
		}

		// Token: 0x0400640E RID: 25614
		private static PlayerKnowledgeDatabase.ConceptKnowledge data;

		// Token: 0x02001BE7 RID: 7143
		private class ConceptKnowledge
		{
			// Token: 0x06009D39 RID: 40249 RVA: 0x002DFC88 File Offset: 0x002DDE88
			public ConceptKnowledge()
			{
				foreach (ConceptDef conceptDef in DefDatabase<ConceptDef>.AllDefs)
				{
					this.knowledge.Add(conceptDef.defName, 0f);
				}
			}

			// Token: 0x0400640F RID: 25615
			public Dictionary<string, float> knowledge = new Dictionary<string, float>();
		}
	}
}
