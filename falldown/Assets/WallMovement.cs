using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallMovement : MonoBehaviour 
{
	public float moveSpeed = 10f;
	public float moveAmount = 10f;

	public enum Direction
	{
		Left,
		Right,
		Down, 
		Up
	}

	public Direction moveDirection = Direction.Up;

	private static Dictionary<Direction, Vector2> m_DirLookup;

	private Vector2 m_Direction;
	private bool m_ShouldUpdate = false;
	private Direction m_InverseDir;

	private Vector2 m_InitialPosition;

	void Start()
	{
		m_InitialPosition = transform.position;
		
		if (m_DirLookup == null) {
			m_DirLookup = new Dictionary<Direction, Vector2> ();

			m_DirLookup.Add (Direction.Up, Vector2.up);
			m_DirLookup.Add (Direction.Left, Vector2.left);
			m_DirLookup.Add (Direction.Down, Vector2.down);
			m_DirLookup.Add (Direction.Right, Vector2.right);
		}

		m_Direction = m_DirLookup [moveDirection];

	}

	public void DoMovement()
	{
		Reset ();
		m_ShouldUpdate = true;
	}

	public void Reset()
	{
		transform.position = m_InitialPosition;
	}

	void Update()
	{
		if (m_ShouldUpdate) {
			transform.position += (Vector3)m_Direction * moveSpeed * Time.deltaTime;

			ClampMovement ();
		}
	}

	private void ClampMovement()
	{
		switch (moveDirection) {
		case Direction.Up:

			if (transform.position.y > -30f) {
				transform.position = new Vector3(transform.position.x, -30f);
				m_ShouldUpdate = false;
			}

			break;
		case Direction.Left:

			if (transform.position.x < 30f) {
				transform.position = new Vector3(30f, transform.position.y);
				m_ShouldUpdate = false;
			}

			break;
		case Direction.Down:

			if (transform.position.y < 30f) {
				transform.position = new Vector3(transform.position.x, 30f);
				m_ShouldUpdate = false;
			}

			break;
		case Direction.Right:

			if (transform.position.x > -30f) {
				transform.position = new Vector3(-30f, transform.position.y);
				m_ShouldUpdate = false;
			}
			break;
		}
	}
}
