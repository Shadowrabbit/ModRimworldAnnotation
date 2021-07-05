using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D9 RID: 5081
	public static class PlayerKnowledgeDatabase
	{
		// Token: 0x06007B8F RID: 31631 RVA: 0x002B8D5A File Offset: 0x002B6F5A
		static PlayerKnowledgeDatabase()
		{
			PlayerKnowledgeDatabase.ReloadAndRebind();
		}

		// Token: 0x06007B90 RID: 31632 RVA: 0x002B8D64 File Offset: 0x002B6F64
		public static void ReloadAndRebind()
		{
			PlayerKnowledgeDatabase.data = DirectXmlLoader.ItemFromXmlFile<PlayerKnowledgeDatabase.ConceptKnowledge>(GenFilePaths.ConceptKnowledgeFilePath, true);
			foreach (ConceptDef conceptDef in DefDatabase<ConceptDef>.AllDefs)
			{
				if (!PlayerKnowledgeDatabase.data.knowledge.ContainsKey(conceptDef.defName))
				{
					Log.Warning("Knowledge data was missing key " + conceptDef + ". Adding it...");
					PlayerKnowledgeDatabase.data.knowledge.Add(conceptDef.defName, 0f);
				}
			}
		}

		// Token: 0x06007B91 RID: 31633 RVA: 0x002B8E00 File Offset: 0x002B7000
		public static void ResetPersistent()
		{
			FileInfo fileInfo = new FileInfo(GenFilePaths.ConceptKnowledgeFilePath);
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
			PlayerKnowledgeDatabase.data = new PlayerKnowledgeDatabase.ConceptKnowledge();
		}

		// Token: 0x06007B92 RID: 31634 RVA: 0x002B8E30 File Offset: 0x002B7030
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
				Log.Error("Exception saving knowledge database: " + ex);
			}
		}

		// Token: 0x06007B93 RID: 31635 RVA: 0x002B8EB8 File Offset: 0x002B70B8
		public static float GetKnowledge(ConceptDef def)
		{
			return PlayerKnowledgeDatabase.data.knowledge[def.defName];
		}

		// Token: 0x06007B94 RID: 31636 RVA: 0x002B8ED0 File Offset: 0x002B70D0
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

		// Token: 0x06007B95 RID: 31637 RVA: 0x002B8F24 File Offset: 0x002B7124
		public static bool IsComplete(ConceptDef conc)
		{
			return PlayerKnowledgeDatabase.data.knowledge[conc.defName] > 0.999f;
		}

		// Token: 0x06007B96 RID: 31638 RVA: 0x002B8F42 File Offset: 0x002B7142
		private static void NewlyLearned(ConceptDef conc)
		{
			TutorSystem.Notify_Event("ConceptLearned-" + conc.defName);
			if (Find.Tutor != null)
			{
				Find.Tutor.learningReadout.Notify_ConceptNewlyLearned(conc);
			}
		}

		// Token: 0x06007B97 RID: 31639 RVA: 0x002B8F78 File Offset: 0x002B7178
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

		// Token: 0x04004462 RID: 17506
		private static PlayerKnowledgeDatabase.ConceptKnowledge data;

		// Token: 0x020027C3 RID: 10179
		private class ConceptKnowledge
		{
			// Token: 0x0600DAC8 RID: 56008 RVA: 0x004159AC File Offset: 0x00413BAC
			public ConceptKnowledge()
			{
				foreach (ConceptDef conceptDef in DefDatabase<ConceptDef>.AllDefs)
				{
					this.knowledge.Add(conceptDef.defName, 0f);
				}
			}

			// Token: 0x04009652 RID: 38482
			public Dictionary<string, float> knowledge = new Dictionary<string, float>();
		}
	}
}
