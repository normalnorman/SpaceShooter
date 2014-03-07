using UnityEngine;
using System.Collections;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Done_Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public Transform cachedTransform;
	public Vector3 startingPoint;
	public float _horizontalLimit = 2.5f, _verticalLimit = 2.5f, dragSpeed = 0.05f;
	 
	private float nextFire;

	void Start()
	{
		cachedTransform = transform;

		startingPoint = cachedTransform.position;
	}
	
	void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			audio.Play ();
		}

		if (Input.touchCount > 0) {
			Vector3 deltaPosition = Input.GetTouch(0).deltaPosition;
			Debug.Log ("object touched", null);
			switch (Input.GetTouch(0).phase) 
			{
				case TouchPhase.Began:
					break;
				case TouchPhase.Moved:
					DragObject(deltaPosition);
					break;
				case TouchPhase.Ended:
					break;
				default:
					break;
			}
		}
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speed;
		
		rigidbody.position = new Vector3
		(
			Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
		);
		
		rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);
	}

	void DragObject (Vector3 deltaPosition)
	{
		Debug.Log ("object dragged", null);
		cachedTransform.position = new Vector3 
		(
				Mathf.Clamp ((deltaPosition.x * dragSpeed) + cachedTransform.position.x, startingPoint.x - boundary.xMax, startingPoint.x + boundary.xMax),
				Mathf.Clamp ((deltaPosition.y * dragSpeed) + cachedTransform.position.y, startingPoint.y - boundary.zMax, startingPoint.y + boundary.zMax),
				Mathf.Clamp ((deltaPosition.z * dragSpeed) + cachedTransform.position.z, startingPoint.z - boundary.zMax, startingPoint.z + boundary.zMax)
		);

		rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);
		/*Vector3 movement = new Vector3 (deltaPosition.x, 0.0f, deltaPosition.y);
		rigidbody.velocity = movement * speed;

		rigidbody.position = new Vector3 (deltaPosition.x*/
		/*float moveHorizontal = deltaPosition.x - startingPoint.x;//Input.GetAxis ("Horizontal");
		float moveVertical = deltaPosition.z - startingPoint.z;//Input.GetAxis ("Vertical");
		
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speed;
		
		rigidbody.position = new Vector3
			(
				Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
				);
		
		rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);*/
	}

}
