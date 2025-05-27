using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class CharacterData_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/MasterData/CharacterData.xlsx";
	private static readonly string exportPath = "Assets/Resources/MasterData/CharacterData.asset";
	private static readonly string[] sheetNames = { "CharacterData", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_CharacterData data = (Entity_CharacterData)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_CharacterData));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_CharacterData> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					Entity_CharacterData.Sheet s = new Entity_CharacterData.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_CharacterData.Param p = new Entity_CharacterData.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.nameID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.spriteName = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.HP = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.Attack = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.Defense = (int)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
