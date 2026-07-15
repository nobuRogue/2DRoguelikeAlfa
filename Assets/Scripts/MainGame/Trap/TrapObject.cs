using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObject : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer _trapSprite = null;

	public TrapData trapData { get; private set; } = null;

	public void Initialise() {
		trapData = new TrapData();
		gameObject.SetActive( false );
	}

	public void Setup( int ID, int masterID, MapSquareData square ) {
		_trapSprite.sprite = TrapManager.instance.GetTrapSprite();
		trapData.Setup( ID, MasterDataManager.GetTrapData( masterID ) );
		gameObject.SetActive( false );
		SetSquare( square );
	}

	public void Teardown() {
		trapData.Teardown();
		gameObject.SetActive( false );
	}

	public void SetSquare( MapSquareData square ) {
		transform.position = square.GetObjectRoot().position;
		trapData.SetSquare( square );
	}

	public void Show() {
		gameObject.SetActive( true );
	}

}
