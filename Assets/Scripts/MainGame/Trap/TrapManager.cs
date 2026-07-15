using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrapManager : MonoBehaviour {
	public static TrapManager instance = null;

	[SerializeField]
	private Transform _trapRoot = null;

	[SerializeField]
	private TrapObject _trapOrigin = null;

	private List<TrapObject> _useList = null;

	private List<TrapObject> _unuseList = null;

	private Sprite[] _trapSprite = null;

	public void Initialize() {
		instance = this;
		_trapSprite = Resources.LoadAll<Sprite>( "Design/Sprites/Trap/trap" );
		_useList = new List<TrapObject>();
		_unuseList = new List<TrapObject>();
		for (int i = 0; i < 32; i++) {
			TrapObject trap = Instantiate( _trapOrigin, _trapRoot );
			trap.Initialise();
			_unuseList.Add( trap );
		}
	}

	public TrapObject GetTrap( int ID ) {
		if (!CommonModule.IsEnableIndex( _useList, ID )) return null;

		return _useList[ID];
	}

	public void CreateTrap( int masterID, MapSquareData square ) {
		TrapObject trap;
		if (CommonModule.IsEmpty( _unuseList )) {
			// 生成
			trap = Instantiate( _trapOrigin, _trapRoot );
			trap.Initialise();
		} else {
			trap = _unuseList[0];
			_unuseList.RemoveAt( 0 );
		}
		int useID = -1;
		for (int i = 0; i < _useList.Count; i++) {
			if (_useList[i] != null) continue;

			_useList[i] = trap;
			useID = i;
			break;
		}
		if (useID < 0) {
			useID = _useList.Count;
			_useList.Add( trap );
		}
		trap.Setup( useID, masterID, square );
	}

	public void RemoveTrap( TrapObject trap ) {
		_useList[trap.trapData.ID] = null; ;
		trap.Teardown();
		_unuseList.Add( trap );
	}

	public Sprite GetTrapSprite() {
		return _trapSprite[0];
	}

	public void ExecuteAllTrap( System.Action<TrapObject> action ) {
		if (CommonModule.IsEmpty( _useList ) || action == null) return;

		for (int i = 0; i < _useList.Count; i++) {
			if (_useList[i] == null) continue;

			action( _useList[i] );
		}
	}

}
