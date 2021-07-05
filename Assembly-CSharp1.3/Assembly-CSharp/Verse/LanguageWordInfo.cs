using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x02000155 RID: 341
	public class LanguageWordInfo
	{
		// Token: 0x06000982 RID: 2434 RVA: 0x0003249C File Offset: 0x0003069C
		public void LoadFrom(Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			VirtualDirectory directory = dir.Item1.GetDirectory("WordInfo").GetDirectory("Gender");
			this.TryLoadFromFile(directory.GetFile("Male.txt"), Gender.Male, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Female.txt"), Gender.Female, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Neuter.txt"), Gender.None, dir, lang);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00032500 File Offset: 0x00030700
		public Gender ResolveGender(string str, string fallback = null)
		{
			Gender result;
			if (!this.TryResolveGender(str, out result) && fallback != null)
			{
				this.TryResolveGender(str, out result);
			}
			return result;
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00032528 File Offset: 0x00030728
		private bool TryResolveGender(string str, out Gender gender)
		{
			LanguageWordInfo.tmpLowercase.Length = 0;
			for (int i = 0; i < str.Length; i++)
			{
				LanguageWordInfo.tmpLowercase.Append(char.ToLower(str[i]));
			}
			string key = LanguageWordInfo.tmpLowercase.ToString();
			if (this.genders.TryGetValue(key, out gender))
			{
				return true;
			}
			gender = Gender.Male;
			return false;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00032588 File Offset: 0x00030788
		private void TryLoadFromFile(VirtualFile file, Gender gender, Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			string[] array;
			try
			{
				array = file.ReadAllLines();
			}
			catch (DirectoryNotFoundException)
			{
				return;
			}
			catch (FileNotFoundException)
			{
				return;
			}
			if (!lang.TryRegisterFileIfNew(dir, file.FullPath))
			{
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].NullOrEmpty() && !this.genders.ContainsKey(array[i]))
				{
					this.genders.Add(array[i], gender);
				}
			}
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00032608 File Offset: 0x00030808
		public void RegisterLut(string name)
		{
			if (this.lookupTables.ContainsKey(name.ToLower()))
			{
				Log.Error("Tried registering language look up table named " + name + " twice.");
				return;
			}
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			try
			{
				foreach (Tuple<VirtualDirectory, ModContentPack, string> tuple in activeLanguage.AllDirectories)
				{
					VirtualFile file = tuple.Item1.GetDirectory("WordInfo").GetFile(name + ".txt");
					if (file.Exists)
					{
						foreach (string text in GenText.LinesFromString(file.ReadAllText()))
						{
							string[] array;
							if (GenText.TryGetSeparatedValues(text, ';', out array))
							{
								string key = array[0].ToLower();
								if (!dictionary.ContainsKey(key))
								{
									dictionary.Add(key, array);
								}
							}
							else
							{
								Log.ErrorOnce(string.Concat(new string[]
								{
									"Failed parsing lookup items from line ",
									text,
									" in ",
									file.FullPath,
									". Line: ",
									text
								}), name.GetHashCode() ^ 1857221523);
							}
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception parsing a language lookup table: " + arg);
			}
			this.lookupTables.Add(name.ToLower(), dictionary);
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x000327C4 File Offset: 0x000309C4
		public Dictionary<string, string[]> GetLookupTable(string name)
		{
			string text = name.ToLower();
			if (this.lookupTables.ContainsKey(text))
			{
				return this.lookupTables[text];
			}
			this.RegisterLut(text);
			if (this.lookupTables.ContainsKey(text))
			{
				return this.lookupTables[text];
			}
			return null;
		}

		// Token: 0x04000870 RID: 2160
		private Dictionary<string, Gender> genders = new Dictionary<string, Gender>();

		// Token: 0x04000871 RID: 2161
		private Dictionary<string, Dictionary<string, string[]>> lookupTables = new Dictionary<string, Dictionary<string, string[]>>();

		// Token: 0x04000872 RID: 2162
		private const string FolderName = "WordInfo";

		// Token: 0x04000873 RID: 2163
		private const string GendersFolderName = "Gender";

		// Token: 0x04000874 RID: 2164
		private const string MaleFileName = "Male.txt";

		// Token: 0x04000875 RID: 2165
		private const string FemaleFileName = "Female.txt";

		// Token: 0x04000876 RID: 2166
		private const string NeuterFileName = "Neuter.txt";

		// Token: 0x04000877 RID: 2167
		private static StringBuilder tmpLowercase = new StringBuilder();
	}
}
