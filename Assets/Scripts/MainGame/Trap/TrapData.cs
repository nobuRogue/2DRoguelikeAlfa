using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapData {

	public int ID { get; private set; } = -1;

	public Entity_TrapData.Param masterData { get; private set; } = null;

	public int posX { get; private set; } = -1;
	public int posY { get; private set; } = -1;

	public void Setup( int ID, Entity_TrapData.Param trapMaster ) {
		this.ID = ID;
		masterData = trapMaster;
	}

	public void Teardown() {
		RemoveSquare();
		ID = -1;
		masterData = null;
	}

	public void SetSquare( MapSquareData square ) {
		square.SetTrap( ID );
		posX = square.posX;
		posY = square.posY;
	}

	public void RemoveSquare() {
		MapSquareData square = MapSquareManager.instance.GetSquareData( posX, posY );
		square.RemoveObject();
		posX = -1;
		posY = -1;
	}

}
