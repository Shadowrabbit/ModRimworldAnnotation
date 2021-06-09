﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x020004B4 RID: 1204
	public class ScribeSaver
	{
		// Token: 0x06001DED RID: 7661 RVA: 0x000F8D3C File Offset: 0x000F6F3C
		public void InitSaving(string filePath, string documentElementName)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitSaving() but current mode is " + Scribe.mode, false);
				Scribe.ForceStop();
			}
			if (this.curPath != null)
			{
				Log.Error("Current path is not null in InitSaving", false);
				this.curPath = null;
				this.savedNodes.Clear();
				this.nextListElementTemporaryId = 0;
			}
			try
			{
				Scribe.mode = LoadSaveMode.Saving;
				this.saveStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Indent = true;
				xmlWriterSettings.IndentChars = "\t";
				this.writer = XmlWriter.Create(this.saveStream, xmlWriterSettings);
				this.writer.WriteStartDocument();
				this.EnterNode(documentElementName);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init saving file: ",
					filePath,
					"\n",
					ex
				}), false);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x000F8E34 File Offset: 0x000F7034
		public void FinalizeSaving()
		{
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error("Called FinalizeSaving() but current mode is " + Scribe.mode, false);
				return;
			}
			if (this.anyInternalException)
			{
				this.ForceStop();
				throw new Exception("Can't finalize saving due to internal exception. The whole file would be most likely corrupted anyway.");
			}
			try
			{
				if (this.writer != null)
				{
					this.ExitNode();
					this.writer.WriteEndDocument();
					this.writer.Flush();
					this.writer.Close();
					this.writer = null;
				}
				if (this.saveStream != null)
				{
					this.saveStream.Flush();
					this.saveStream.Close();
					this.saveStream = null;
				}
				Scribe.mode = LoadSaveMode.Inactive;
				this.savingForDebug = false;
				this.loadIDsErrorsChecker.CheckForErrorsAndClear();
				this.curPath = null;
				this.savedNodes.Clear();
				this.nextListElementTemporaryId = 0;
				this.anyInternalException = false;
			}
			catch (Exception arg)
			{
				Log.Error("Exception in FinalizeLoading(): " + arg, false);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x000F8F40 File Offset: 0x000F7140
		public void WriteElement(string elementName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteElemenet(), but writer is null.", false);
				return;
			}
			try
			{
				this.writer.WriteElementString(elementName, value);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x000F8F8C File Offset: 0x000F718C
		public void WriteAttribute(string attributeName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteAttribute(), but writer is null.", false);
				return;
			}
			try
			{
				this.writer.WriteAttributeString(attributeName, value);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x000F8FD8 File Offset: 0x000F71D8
		public string DebugOutputFor(IExposable saveable)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("DebugOutput needs current mode to be Inactive", false);
				return "";
			}
			string result;
			try
			{
				using (StringWriter stringWriter = new StringWriter())
				{
					XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
					xmlWriterSettings.Indent = true;
					xmlWriterSettings.IndentChars = "  ";
					xmlWriterSettings.OmitXmlDeclaration = true;
					try
					{
						using (this.writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
						{
							Scribe.mode = LoadSaveMode.Saving;
							this.savingForDebug = true;
							Scribe_Deep.Look<IExposable>(ref saveable, "saveable", Array.Empty<object>());
						}
						result = stringWriter.ToString();
					}
					finally
					{
						this.ForceStop();
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception while getting debug output: " + arg, false);
				this.ForceStop();
				result = "";
			}
			return result;
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x000F90D4 File Offset: 0x000F72D4
		public bool EnterNode(string nodeName)
		{
			if (this.writer == null)
			{
				return false;
			}
			try
			{
				this.writer.WriteStartElement(nodeName);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
			return true;
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x000F9118 File Offset: 0x000F7318
		public void ExitNode()
		{
			if (this.writer == null)
			{
				return;
			}
			try
			{
				this.writer.WriteEndElement();
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x000F9158 File Offset: 0x000F7358
		public void ForceStop()
		{
			if (this.writer != null)
			{
				this.writer.Close();
				this.writer = null;
			}
			if (this.saveStream != null)
			{
				this.saveStream.Close();
				this.saveStream = null;
			}
			this.savingForDebug = false;
			this.loadIDsErrorsChecker.Clear();
			this.curPath = null;
			this.savedNodes.Clear();
			this.nextListElementTemporaryId = 0;
			this.anyInternalException = false;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}

		// Token: 0x04001561 RID: 5473
		public DebugLoadIDsSavingErrorsChecker loadIDsErrorsChecker = new DebugLoadIDsSavingErrorsChecker();

		// Token: 0x04001562 RID: 5474
		public bool savingForDebug;

		// Token: 0x04001563 RID: 5475
		private Stream saveStream;

		// Token: 0x04001564 RID: 5476
		private XmlWriter writer;

		// Token: 0x04001565 RID: 5477
		private string curPath;

		// Token: 0x04001566 RID: 5478
		private HashSet<string> savedNodes = new HashSet<string>();

		// Token: 0x04001567 RID: 5479
		private int nextListElementTemporaryId;

		// Token: 0x04001568 RID: 5480
		private bool anyInternalException;
	}
}
