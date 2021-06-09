using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Steamworks;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x020015C5 RID: 5573
	public class Scenario : IExposable, WorkshopUploadable
	{
		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x060078EC RID: 30956 RVA: 0x0005172F File Offset: 0x0004F92F
		public IEnumerable<System.Version> SupportedVersions
		{
			get
			{
				yield return new System.Version(VersionControl.CurrentMajor, VersionControl.CurrentMinor);
				yield break;
			}
		}

		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x060078ED RID: 30957 RVA: 0x00051738 File Offset: 0x0004F938
		public FileInfo File
		{
			get
			{
				return new FileInfo(GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x060078EE RID: 30958 RVA: 0x0005174A File Offset: 0x0004F94A
		public IEnumerable<ScenPart> AllParts
		{
			get
			{
				yield return this.playerFaction;
				int num;
				for (int i = 0; i < this.parts.Count; i = num + 1)
				{
					yield return this.parts[i];
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x060078EF RID: 30959 RVA: 0x0005175A File Offset: 0x0004F95A
		// (set) Token: 0x060078F0 RID: 30960 RVA: 0x0005177B File Offset: 0x0004F97B
		public ScenarioCategory Category
		{
			get
			{
				if (this.categoryInt == ScenarioCategory.Undefined)
				{
					Log.Error("Category is Undefined on Scenario " + this, false);
				}
				return this.categoryInt;
			}
			set
			{
				this.categoryInt = value;
			}
		}

		// Token: 0x060078F2 RID: 30962 RVA: 0x0024B674 File Offset: 0x00249874
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<string>(ref this.summary, "summary", null, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
			Scribe_Values.Look<PublishedFileId_t>(ref this.publishedFileIdInt, "publishedFileId", PublishedFileId_t.Invalid, false);
			Scribe_Deep.Look<ScenPart_PlayerFaction>(ref this.playerFaction, "playerFaction", Array.Empty<object>());
			Scribe_Collections.Look<ScenPart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.parts.RemoveAll((ScenPart x) => x == null) != 0)
				{
					Log.Warning("Some ScenParts were null after loading.", false);
				}
				if (this.parts.RemoveAll((ScenPart x) => x.HasNullDefs()) != 0)
				{
					Log.Warning("Some ScenParts had null defs.", false);
				}
			}
		}

		// Token: 0x060078F3 RID: 30963 RVA: 0x000517B0 File Offset: 0x0004F9B0
		public IEnumerable<string> ConfigErrors()
		{
			if (this.name.NullOrEmpty())
			{
				yield return "no title";
			}
			if (this.parts.NullOrEmpty<ScenPart>())
			{
				yield return "no parts";
			}
			if (this.playerFaction == null)
			{
				yield return "no playerFaction";
			}
			foreach (ScenPart scenPart in this.AllParts)
			{
				foreach (string text in scenPart.ConfigErrors())
				{
					yield return text;
				}
				IEnumerator<string> enumerator2 = null;
			}
			IEnumerator<ScenPart> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060078F4 RID: 30964 RVA: 0x0024B770 File Offset: 0x00249970
		public string GetFullInformationText()
		{
			string result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.description);
				stringBuilder.AppendLine();
				foreach (ScenPart scenPart in this.AllParts)
				{
					scenPart.summarized = false;
				}
				foreach (ScenPart scenPart2 in from p in this.AllParts
				orderby p.def.summaryPriority descending, p.def.defName
				where p.visible
				select p)
				{
					string text = scenPart2.Summary(this);
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text);
					}
				}
				result = stringBuilder.ToString().TrimEndNewlines();
			}
			catch (Exception ex)
			{
				Log.ErrorOnce("Exception in Scenario.GetFullInformationText():\n" + ex.ToString(), 10395878, false);
				result = "Cannot read data.";
			}
			return result;
		}

		// Token: 0x060078F5 RID: 30965 RVA: 0x000517C0 File Offset: 0x0004F9C0
		public string GetSummary()
		{
			return this.summary;
		}

		// Token: 0x060078F6 RID: 30966 RVA: 0x0024B8F4 File Offset: 0x00249AF4
		public Scenario CopyForEditing()
		{
			Scenario scenario = new Scenario();
			scenario.name = this.name;
			scenario.summary = this.summary;
			scenario.description = this.description;
			scenario.playerFaction = (ScenPart_PlayerFaction)this.playerFaction.CopyForEditing();
			scenario.parts.AddRange(from p in this.parts
			select p.CopyForEditing());
			scenario.categoryInt = ScenarioCategory.CustomLocal;
			return scenario;
		}

		// Token: 0x060078F7 RID: 30967 RVA: 0x0024B97C File Offset: 0x00249B7C
		public void PreConfigure()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreConfigure();
			}
		}

		// Token: 0x060078F8 RID: 30968 RVA: 0x0024B9C8 File Offset: 0x00249BC8
		public Page GetFirstConfigPage()
		{
			List<Page> list = new List<Page>();
			list.Add(new Page_SelectStoryteller());
			list.Add(new Page_CreateWorldParams());
			list.Add(new Page_SelectStartingSite());
			foreach (Page item in this.parts.SelectMany((ScenPart p) => p.GetConfigPages()))
			{
				list.Add(item);
			}
			Page page = PageUtility.StitchedPages(list);
			if (page != null)
			{
				Page page2 = page;
				while (page2.next != null)
				{
					page2 = page2.next;
				}
				page2.nextAct = delegate()
				{
					PageUtility.InitGameStart();
				};
			}
			return page;
		}

		// Token: 0x060078F9 RID: 30969 RVA: 0x0024BAA8 File Offset: 0x00249CA8
		public bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			using (IEnumerator<ScenPart> enumerator = this.AllParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060078FA RID: 30970 RVA: 0x0024BB00 File Offset: 0x00249D00
		public void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_NewPawnGenerating(pawn, context);
			}
		}

		// Token: 0x060078FB RID: 30971 RVA: 0x0024BB4C File Offset: 0x00249D4C
		public void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_PawnGenerated(pawn, context, redressed);
			}
		}

		// Token: 0x060078FC RID: 30972 RVA: 0x0024BB9C File Offset: 0x00249D9C
		public void Notify_PawnDied(Corpse corpse)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnDied(corpse);
			}
		}

		// Token: 0x060078FD RID: 30973 RVA: 0x0024BBD4 File Offset: 0x00249DD4
		public void PostWorldGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostWorldGenerate();
			}
		}

		// Token: 0x060078FE RID: 30974 RVA: 0x0024BC20 File Offset: 0x00249E20
		public void PreMapGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreMapGenerate();
			}
		}

		// Token: 0x060078FF RID: 30975 RVA: 0x0024BC6C File Offset: 0x00249E6C
		public void GenerateIntoMap(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.GenerateIntoMap(map);
			}
		}

		// Token: 0x06007900 RID: 30976 RVA: 0x0024BCB8 File Offset: 0x00249EB8
		public void PostMapGenerate(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostMapGenerate(map);
			}
		}

		// Token: 0x06007901 RID: 30977 RVA: 0x0024BD04 File Offset: 0x00249F04
		public void PostGameStart()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostGameStart();
			}
		}

		// Token: 0x06007902 RID: 30978 RVA: 0x0024BD50 File Offset: 0x00249F50
		public float GetStatFactor(StatDef stat)
		{
			float num = 1f;
			for (int i = 0; i < this.parts.Count; i++)
			{
				ScenPart_StatFactor scenPart_StatFactor = this.parts[i] as ScenPart_StatFactor;
				if (scenPart_StatFactor != null)
				{
					num *= scenPart_StatFactor.GetStatFactor(stat);
				}
			}
			return num;
		}

		// Token: 0x06007903 RID: 30979 RVA: 0x0024BD9C File Offset: 0x00249F9C
		public void TickScenario()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Tick();
			}
		}

		// Token: 0x06007904 RID: 30980 RVA: 0x000517C8 File Offset: 0x0004F9C8
		public void RemovePart(ScenPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Cannot remove: " + part, false);
			}
			this.parts.Remove(part);
		}

		// Token: 0x06007905 RID: 30981 RVA: 0x0024BDD0 File Offset: 0x00249FD0
		public bool CanReorder(ScenPart part, ReorderDirection dir)
		{
			if (!part.def.PlayerAddRemovable)
			{
				return false;
			}
			int num = this.parts.IndexOf(part);
			if (dir == ReorderDirection.Up)
			{
				return num != 0 && (num <= 0 || this.parts[num - 1].def.PlayerAddRemovable);
			}
			if (dir == ReorderDirection.Down)
			{
				return num != this.parts.Count - 1;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06007906 RID: 30982 RVA: 0x0024BE40 File Offset: 0x0024A040
		public void Reorder(ScenPart part, ReorderDirection dir)
		{
			int num = this.parts.IndexOf(part);
			this.parts.RemoveAt(num);
			if (dir == ReorderDirection.Up)
			{
				this.parts.Insert(num - 1, part);
			}
			if (dir == ReorderDirection.Down)
			{
				this.parts.Insert(num + 1, part);
			}
		}

		// Token: 0x06007907 RID: 30983 RVA: 0x0024BE8C File Offset: 0x0024A08C
		public bool CanToUploadToWorkshop()
		{
			return this.Category != ScenarioCategory.FromDef && this.TryUploadReport().Accepted && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x06007908 RID: 30984 RVA: 0x0024BEC8 File Offset: 0x0024A0C8
		public void PrepareForWorkshopUpload()
		{
			string path = this.name + Rand.RangeInclusive(100, 999).ToString();
			this.tempUploadDir = Path.Combine(GenFilePaths.TempFolderPath, path);
			DirectoryInfo directoryInfo = new DirectoryInfo(this.tempUploadDir);
			if (directoryInfo.Exists)
			{
				directoryInfo.Delete();
			}
			directoryInfo.Create();
			string text = Path.Combine(this.tempUploadDir, this.name);
			text += ".rsc";
			GameDataSaveLoader.SaveScenario(this, text);
		}

		// Token: 0x06007909 RID: 30985 RVA: 0x0024BF4C File Offset: 0x0024A14C
		public AcceptanceReport TryUploadReport()
		{
			if (this.name == null || this.name.Length < 3 || this.summary == null || this.summary.Length < 3 || this.description == null || this.description.Length < 3)
			{
				return "TextFieldsMustBeFilled".TranslateSimple();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600790A RID: 30986 RVA: 0x000517F6 File Offset: 0x0004F9F6
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x0600790B RID: 30987 RVA: 0x000517FE File Offset: 0x0004F9FE
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			this.publishedFileIdInt = newPfid;
			if (this.Category == ScenarioCategory.CustomLocal && !this.fileName.NullOrEmpty())
			{
				GameDataSaveLoader.SaveScenario(this, GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x0600790C RID: 30988 RVA: 0x0005182E File Offset: 0x0004FA2E
		public string GetWorkshopName()
		{
			return this.name;
		}

		// Token: 0x0600790D RID: 30989 RVA: 0x00051836 File Offset: 0x0004FA36
		public string GetWorkshopDescription()
		{
			return this.GetFullInformationText();
		}

		// Token: 0x0600790E RID: 30990 RVA: 0x0005183E File Offset: 0x0004FA3E
		public string GetWorkshopPreviewImagePath()
		{
			return GenFilePaths.ScenarioPreviewImagePath;
		}

		// Token: 0x0600790F RID: 30991 RVA: 0x00051845 File Offset: 0x0004FA45
		public IList<string> GetWorkshopTags()
		{
			return new List<string>
			{
				"Scenario"
			};
		}

		// Token: 0x06007910 RID: 30992 RVA: 0x00051857 File Offset: 0x0004FA57
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return new DirectoryInfo(this.tempUploadDir);
		}

		// Token: 0x06007911 RID: 30993 RVA: 0x00051864 File Offset: 0x0004FA64
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x06007912 RID: 30994 RVA: 0x00051880 File Offset: 0x0004FA80
		public override string ToString()
		{
			if (this.name.NullOrEmpty())
			{
				return "LabellessScenario";
			}
			return this.name;
		}

		// Token: 0x06007913 RID: 30995 RVA: 0x0024BFB0 File Offset: 0x0024A1B0
		public override int GetHashCode()
		{
			int num = 6126121;
			if (this.name != null)
			{
				num ^= this.name.GetHashCode();
			}
			if (this.summary != null)
			{
				num ^= this.summary.GetHashCode();
			}
			if (this.description != null)
			{
				num ^= this.description.GetHashCode();
			}
			return num ^ this.publishedFileIdInt.GetHashCode();
		}

		// Token: 0x04004F96 RID: 20374
		[MustTranslate]
		public string name;

		// Token: 0x04004F97 RID: 20375
		[MustTranslate]
		public string summary;

		// Token: 0x04004F98 RID: 20376
		[MustTranslate]
		public string description;

		// Token: 0x04004F99 RID: 20377
		internal ScenPart_PlayerFaction playerFaction;

		// Token: 0x04004F9A RID: 20378
		internal List<ScenPart> parts = new List<ScenPart>();

		// Token: 0x04004F9B RID: 20379
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x04004F9C RID: 20380
		private ScenarioCategory categoryInt;

		// Token: 0x04004F9D RID: 20381
		[NoTranslate]
		public string fileName;

		// Token: 0x04004F9E RID: 20382
		private WorkshopItemHook workshopHookInt;

		// Token: 0x04004F9F RID: 20383
		[NoTranslate]
		private string tempUploadDir;

		// Token: 0x04004FA0 RID: 20384
		public bool enabled = true;

		// Token: 0x04004FA1 RID: 20385
		public bool showInUI = true;

		// Token: 0x04004FA2 RID: 20386
		public const int NameMaxLength = 55;

		// Token: 0x04004FA3 RID: 20387
		public const int SummaryMaxLength = 300;

		// Token: 0x04004FA4 RID: 20388
		public const int DescriptionMaxLength = 1000;
	}
}
