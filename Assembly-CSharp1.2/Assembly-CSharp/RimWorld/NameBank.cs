using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001400 RID: 5120
	public class NameBank
	{
		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x06006EC2 RID: 28354 RVA: 0x0004AFE9 File Offset: 0x000491E9
		private IEnumerable<List<string>> AllNameLists
		{
			get
			{
				int num;
				for (int i = 0; i < NameBank.numGenders; i = num + 1)
				{
					for (int j = 0; j < NameBank.numSlots; j = num + 1)
					{
						yield return this.names[i, j];
						num = j;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06006EC3 RID: 28355 RVA: 0x0021F64C File Offset: 0x0021D84C
		public NameBank(PawnNameCategory ID)
		{
			this.nameType = ID;
			this.names = new List<string>[NameBank.numGenders, NameBank.numSlots];
			for (int i = 0; i < NameBank.numGenders; i++)
			{
				for (int j = 0; j < NameBank.numSlots; j++)
				{
					this.names[i, j] = new List<string>();
				}
			}
		}

		// Token: 0x06006EC4 RID: 28356 RVA: 0x0021F6B0 File Offset: 0x0021D8B0
		public void ErrorCheck()
		{
			foreach (List<string> list in this.AllNameLists)
			{
				foreach (string str in (from x in list
				group x by x into g
				where g.Count<string>() > 1
				select g.Key).ToList<string>())
				{
					Log.Error("Duplicated name: " + str, false);
				}
				foreach (string text in list)
				{
					if (text.Trim() != text)
					{
						Log.Error("Trimmable whitespace on name: [" + text + "]", false);
					}
				}
			}
		}

		// Token: 0x06006EC5 RID: 28357 RVA: 0x0004AFF9 File Offset: 0x000491F9
		private List<string> NamesFor(PawnNameSlot slot, Gender gender)
		{
			return this.names[(int)gender, (int)slot];
		}

		// Token: 0x06006EC6 RID: 28358 RVA: 0x0021F838 File Offset: 0x0021DA38
		public void AddNames(PawnNameSlot slot, Gender gender, IEnumerable<string> namesToAdd)
		{
			foreach (string item in namesToAdd)
			{
				this.NamesFor(slot, gender).Add(item);
			}
		}

		// Token: 0x06006EC7 RID: 28359 RVA: 0x0004B008 File Offset: 0x00049208
		public void AddNamesFromFile(PawnNameSlot slot, Gender gender, string fileName)
		{
			this.AddNames(slot, gender, GenFile.LinesFromFile("Names/" + fileName));
		}

		// Token: 0x06006EC8 RID: 28360 RVA: 0x0021F888 File Offset: 0x0021DA88
		public string GetName(PawnNameSlot slot, Gender gender = Gender.None, bool checkIfAlreadyUsed = true)
		{
			List<string> list = this.NamesFor(slot, gender);
			int num = 0;
			if (list.Count == 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Name list for gender=",
					gender,
					" slot=",
					slot,
					" is empty."
				}), false);
				return "Errorname";
			}
			string text;
			for (;;)
			{
				text = list.RandomElement<string>();
				if (checkIfAlreadyUsed && !NameUseChecker.NameWordIsUsed(text))
				{
					break;
				}
				num++;
				if (num > 50)
				{
					return text;
				}
			}
			return text;
		}

		// Token: 0x04004916 RID: 18710
		public PawnNameCategory nameType;

		// Token: 0x04004917 RID: 18711
		private List<string>[,] names;

		// Token: 0x04004918 RID: 18712
		private static readonly int numGenders = Enum.GetValues(typeof(Gender)).Length;

		// Token: 0x04004919 RID: 18713
		private static readonly int numSlots = Enum.GetValues(typeof(PawnNameSlot)).Length;
	}
}
