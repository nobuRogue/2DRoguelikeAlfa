using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class FloorData_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/MasterData/FloorData.xlsx";
	private static readonly string exportPath = "Assets/Resources/MasterData/FloorData.asset";
	private static readonly string[] sheetNames = { "FloorData", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_FloorData data = (Entity_FloorData)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_FloorData));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_FloorData> ();
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

					Entity_FloorData.Sheet s = new Entity_FloorData.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_FloorData.Param p = new Entity_FloorData.Param ();
						
					cell = row.GetCell(0); p.floorCount = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.spriteIndex = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.enemyTableID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.itemTableID = (int)(cell == null ? 0 : cell.NumericCellValue);
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
