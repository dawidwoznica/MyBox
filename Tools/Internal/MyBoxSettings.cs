﻿#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MyBox.Internal
{
	[Serializable]
	public class MyBoxSettings : ScriptableObject
	{
		[SerializeField] private bool _autoSaveEnabled = true;
		[SerializeField] private bool _cleanEmptyDirectoriesFeature = true;
		[SerializeField] private bool _iPrepareBeforePlaymode;
		[SerializeField] private bool _iPrepareBeforeBuild;
		[SerializeField] private bool _iPrepareOnSave;

		public static bool AutoSaveEnabled
		{
			get { return Instance._autoSaveEnabled; }
			set
			{
				if (Instance._autoSaveEnabled == value) return;
				Instance._autoSaveEnabled = value;
				Save();
			}
		}

		public static bool CleanEmptyDirectoriesFeature
		{
			get { return Instance._cleanEmptyDirectoriesFeature; }
			set
			{
				if (Instance._cleanEmptyDirectoriesFeature == value) return;
				Instance._cleanEmptyDirectoriesFeature = value;
				Save();
			}
		}
		

		public static bool IPrepareBeforePlaymode
		{
			get { return Instance._iPrepareBeforePlaymode; }
			set
			{
				if (Instance._iPrepareBeforePlaymode == value) return;
				Instance._iPrepareBeforePlaymode = value;
				Save();
			}
		}
		public static bool IPrepareBeforeBuild
		{
			get { return Instance._iPrepareBeforeBuild; }
			set
			{
				if (Instance._iPrepareBeforeBuild == value) return;
				Instance._iPrepareBeforeBuild = value;
				Save();
			}
		}
		public static bool IPrepareOnSave
		{
			get { return Instance._iPrepareOnSave; }
			set
			{
				if (Instance._iPrepareOnSave == value) return;
				Instance._iPrepareOnSave = value;
				Save();
			}
		}


		#region Instance

		private static MyBoxSettings Instance
		{
			get
			{
				if (_instance != null) return _instance;
				_instance = LoadOrCreate();
				return _instance;
			}
		}

		private static readonly string Directory = "ProjectSettings";
		private static readonly string Path = Directory + "/MyBoxSettings.asset";
		private static MyBoxSettings _instance;

		private static void Save()
		{
			var instance = _instance;
			if (!System.IO.Directory.Exists(Directory)) System.IO.Directory.CreateDirectory(Directory);
			try
			{
				UnityEditorInternal.InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] {instance}, Path, true);
			}
			catch (Exception ex)
			{
				Debug.LogError("Unable to save MyBoxSettings!\n" + ex);
			}
		}

		private static MyBoxSettings LoadOrCreate()
		{
			var settings = !File.Exists(Path) ? CreateNewSettings() : LoadSettings();
			if (settings == null)
			{
				DeleteFile(Path);
				settings = CreateNewSettings();
			}

			settings.hideFlags = HideFlags.HideAndDontSave;

			return settings;
		}


		private static MyBoxSettings LoadSettings()
		{
			MyBoxSettings settingsInstance;
			try
			{
				settingsInstance = (MyBoxSettings) UnityEditorInternal.InternalEditorUtility.LoadSerializedFileAndForget(Path)[0];
			}
			catch (Exception ex)
			{
				Debug.LogError("Unable to read MyBoxSettings, set to defaults" + ex);
				settingsInstance = null;
			}

			return settingsInstance;
		}

		private static MyBoxSettings CreateNewSettings()
		{
			_instance = CreateInstance<MyBoxSettings>();
			Save();

			return _instance;
		}

		private static void DeleteFile(string path)
		{
			if (!File.Exists(path)) return;

			var attributes = File.GetAttributes(path);
			if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				File.SetAttributes(path, attributes & ~FileAttributes.ReadOnly);

			File.Delete(path);
		}

		#endregion
	}
}
#endif