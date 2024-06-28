using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OriNoco
{
	public static class Configuration
	{
		public static string configPath = GetRidOfLastPath(GetRidOfLastPath(System.Windows.Forms.Application.UserAppDataPath)) + Path.DirectorySeparatorChar + "config.json";
		public static string dirPath = GetRidOfLastPath(GetRidOfLastPath(System.Windows.Forms.Application.UserAppDataPath));

		// A string field class to store the data;
		[Serializable]
		public class StringField
		{
			public string name;
			public string value;
		}

		// A prefs data class for saving the entire fields into one data.
		[Serializable]
		public class PrefsData
		{
			public string computerUniqueID;
			public List<StringField> prefs;
		}

		/// <summary>
		/// Saving a string into the CustomPlayerPrefs database.
		/// </summary>
		/// <param name="name">The prefs name</param>
		/// <param name="value">The prefs value</param>
		public static void SetString(string name, string value)
		{
			// Check if config existed
			try {
				if (ConfigExists()) {
					// Fetch the data from the config
					var data = JsonConvert.DeserializeObject<PrefsData>(GetFileData());
					// Search the data in the config.
					int result = ValueExists(data.prefs, name);
					// If result is above -1 then the value exist. Commit modify the field.
					if (result > -1) {
						data.prefs[result].value = value;
					} else {
						// If result is -1 then the value didn't exist. Create a new field.
						var field = new StringField {
							name = name,
							value = value
						};
						data.prefs.Add(field);
					}
					// Save the config file.
					SaveFile(JsonConvert.SerializeObject(data));
				} else {
					// Create a new config as there's no config saved.
					// Initialize the prefs data.
					var data = new PrefsData {
						prefs = new List<StringField>()
					};
					// Make a new field.
					var field = new StringField {
						name = name,
						value = value
					};
					data.prefs.Add(field);
					// Save the config file.
					SaveFile(JsonConvert.SerializeObject(data));
				}
			} catch {
				// Create a new config as there's no config saved.
				// Initialize the prefs data.
				var data = new PrefsData {
					prefs = new List<StringField>()
				};
				//data.computerUniqueID = SystemInfo.deviceUniqueIdentifier;
				// Make a new field.
				var field = new StringField {
					name = name,
					value = value
				};
				data.prefs.Add(field);
				// Save the config file.
				SaveFile(JsonConvert.SerializeObject(data));
			}
		}

		/// <summary>
		/// Saving an integer into the CustomPlayerPrefs database.
		/// </summary>
		/// <param name="name">The prefs name</param>
		/// <param name="value">The prefs value</param>
		public static void SetInt(string name, int value)
		{
			// Check if config existed
			try {
				if (ConfigExists()) {
					// Fetch the data from the config
					var data = JsonConvert.DeserializeObject<PrefsData>(GetFileData());
					// Search the data in the config.
					int result = ValueExists(data.prefs, name);
					// If result is above -1 then the value exist. Commit modify the field.
					if (result > -1) {
						data.prefs[result].value = value.ToString();
					} else {
						// If result is -1 then the value didn't exist. Create a new field.
						var field = new StringField {
							name = name,
							value = value.ToString()
						};
						data.prefs.Add(field);
					}
					// Save the config file.
					SaveFile(JsonConvert.SerializeObject(data));
				} else {
					// Create a new config as there's no config saved.
					// Initialize the prefs data.
					var data = new PrefsData {
						prefs = new List<StringField>()
					};
					// Make a new field.
					var field = new StringField {
						name = name,
						value = value.ToString()
					};
					data.prefs.Add(field);
					// Save the config file.
					SaveFile(JsonConvert.SerializeObject(data));
				}
			} catch {
				// Create a new config as there's no config saved.
				// Initialize the prefs data.
				var data = new PrefsData {
					prefs = new List<StringField>()
				};
				//data.computerUniqueID = SystemInfo.deviceUniqueIdentifier;
				// Make a new field.
				var field = new StringField {
					name = name,
					value = value.ToString()
				};
				data.prefs.Add(field);
				// Save the config file.
				SaveFile(JsonConvert.SerializeObject(data));
			}
		}
		/// <summary>
		/// Saving a float into the CustomPlayerPrefs database.
		/// </summary>
		/// <param name="name">The prefs name</param>
		/// <param name="value">The prefs value</param>
		public static void SetFloat(string name, float value)
		{
			// Check if config existed
			try {
				if (ConfigExists()) {
					// Fetch the data from the config
					var data = JsonConvert.DeserializeObject<PrefsData>(GetFileData());
					// Search the data in the config.
					int result = ValueExists(data.prefs, name);
					// If result is above -1 then the value exist. Commit modify the field.
					if (result > -1) {
						data.prefs[result].value = value.ToString();
					} else {
						// If result is -1 then the value didn't exist. Create a new field.
						var field = new StringField {
							name = name,
							value = value.ToString()
						};
						data.prefs.Add(field);
					}
					// Save the config file.
					SaveFile(JsonConvert.SerializeObject(data));
				} else {
					// Create a new config as there's no config saved.
					// Initialize the prefs data.
					var data = new PrefsData {
						prefs = new List<StringField>()
					};
					// Make a new field.
					var field = new StringField {
						name = name,
						value = value.ToString()
					};
					data.prefs.Add(field);
					// Save the config file.
					SaveFile(JsonConvert.SerializeObject(data));
				}
			} catch {
				// Create a new config as there's no config saved.
				// Initialize the prefs data.
				var data = new PrefsData {
					prefs = new List<StringField>()
				};
				//data.computerUniqueID = SystemInfo.deviceUniqueIdentifier;
				// Make a new field.
				var field = new StringField {
					name = name,
					value = value.ToString()
				};
				data.prefs.Add(field);
				// Save the config file.
				SaveFile(JsonConvert.SerializeObject(data));
			}
		}
		/// <summary>
		/// Saving a bool into the CustomPlayerPrefs database.
		/// </summary>
		/// <param name="name">The prefs name</param>
		/// <param name="value">The prefs value</param>
		public static void SetBool(string name, bool value)
		{
			// Check if config existed
			try {
				if (ConfigExists()) {
					// Fetch the data from the config
					var data = JsonConvert.DeserializeObject<PrefsData>(GetFileData());
					// Search the data in the config.
					int result = ValueExists(data.prefs, name);
					// If result is above -1 then the value exist. Commit modify the field.
					if (result > -1) {
						data.prefs[result].value = value.ToString();
					} else {
						// If result is -1 then the value didn't exist. Create a new field.
						var field = new StringField {
							name = name,
							value = value.ToString()
						};
						data.prefs.Add(field);
					}
					// Save the config file.
					SaveFile(JsonConvert.SerializeObject(data));
				} else {
					// Create a new config as there's no config saved.
					// Initialize the prefs data.
					var data = new PrefsData {
						prefs = new List<StringField>()
					};
					// Make a new field.
					var field = new StringField {
						name = name,
						value = value.ToString()
					};
					data.prefs.Add(field);
					// Save the config file.
					SaveFile(JsonConvert.SerializeObject(data));
				}
			} catch {
				// Create a new config as there's no config saved.
				// Initialize the prefs data.
				var data = new PrefsData {
					prefs = new List<StringField>()
				};
				//data.computerUniqueID = SystemInfo.deviceUniqueIdentifier;
				// Make a new field.
				var field = new StringField {
					name = name,
					value = value.ToString()
				};
				data.prefs.Add(field);
				// Save the config file.
				SaveFile(JsonConvert.SerializeObject(data));
			}
		}
		/// <summary>
		/// Getting a string from the CustomPlayerPrefs database
		/// </summary>
		/// <param name="name">Field name</param>
		/// <param name="defaultValue">If there's no data found, return the default value</param>
		/// <returns>The fetched data, if there's no default value set, return null.</returns>
		public static string GetString(string name, string defaultValue = null)
		{
			string result = defaultValue;
			try {
				if (ConfigExists()) {
					var data = JsonConvert.DeserializeObject<PrefsData>(GetFileData());
					int index = ValueExists(data.prefs, name);
					if (index > -1) {
						result = data.prefs[index].value;
					}
				}
			} catch {
				return defaultValue;
			}
			return result;
		}
		/// <summary>
		/// Getting an integer from the CustomPlayerPrefs database
		/// </summary>
		/// <param name="name">Field name</param>
		/// <param name="defaultValue">If there's no data found, return the default value</param>
		/// <returns>The fetched data, if there's no default value set, return 0.</returns>
		public static int GetInt(string name, int defaultValue = 0)
		{
			int result = defaultValue;
			try {
				if (ConfigExists()) {
					var data = JsonConvert.DeserializeObject<PrefsData>(GetFileData());
					int index = ValueExists(data.prefs, name);
					if (index > -1) {
						result = int.Parse(data.prefs[index].value);
					}
				}
			} catch {
				return defaultValue;
			}
			return result;
		}
		/// <summary>
		/// Getting a float from the CustomPlayerPrefs database
		/// </summary>
		/// <param name="name">Field name</param>
		/// <param name="defaultValue">If there's no data found, return the default value</param>
		/// <returns>The fetched data, if there's no default value set, return 0.</returns>
		public static float GetFloat(string name, float defaultValue = 0)
		{
			float result = defaultValue;
			try {
				if (ConfigExists()) {
					var data = JsonConvert.DeserializeObject<PrefsData>(GetFileData());
					int index = ValueExists(data.prefs, name);
					if (index > -1) {
						result = float.Parse(data.prefs[index].value);
					}
				}
			} catch {
				return defaultValue;
			}
			return result;
		}
		/// <summary>
		/// Getting a bool from the CustomPlayerPrefs database
		/// </summary>
		/// <param name="name">Field name</param>
		/// <param name="defaultValue">If there's no data found, return the default value</param>
		/// <returns>The fetched data, if there's no default value set, return false.</returns>
		public static bool GetBool(string name, bool defaultValue = false)
		{
			bool result = defaultValue;
			try {
				if (ConfigExists()) {
					var data = JsonConvert.DeserializeObject<PrefsData>(GetFileData());
					int index = ValueExists(data.prefs, name);
					if (index > -1) {
						result = bool.Parse(data.prefs[index].value);
					}
				}
			} catch {
				return defaultValue;
			}
			return result;
		}
	
		static int ValueExists(List<StringField> val, string name)
		{
			int index = 0;
			foreach (StringField a in val) {
				if (a.name == name) {
					return index;
				}
				index++;
			}
			return (-1);
		}
		static bool ConfigExists()
		{
			return File.Exists(configPath);
		}
		static string GetFileData()
		{
			// Read a file from the path, and the return the decrypted string.
			return File.ReadAllText(configPath);
		}
		public static string GetRidOfLastPath(string dir)
		{
			var e = dir.Split(Path.DirectorySeparatorChar);
			var y = dir.Remove(dir.Length - e[e.Length - 1].Length, e[e.Length - 1].Length);
			if (y.EndsWith("\\", StringComparison.CurrentCulture) || y.EndsWith("/", StringComparison.CurrentCulture)) {
				y = y.Remove(y.Length - 1, 1);
			}
			return y;
		}
		static void SaveFile(string data)
		{
			// Encrypting the data and then saving the string into a file on the specified path.
			if (!Directory.Exists(dirPath))
				Directory.CreateDirectory(dirPath);
			File.WriteAllText(configPath, data);
		}
	}
}