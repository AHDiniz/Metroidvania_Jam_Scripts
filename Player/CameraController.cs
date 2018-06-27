using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Properties")]
	public float followSpeed;
	public float offsetX;
	public float offsetY;
	public float zDistance;

	[Header("References")]
	public Transform targetPos;

	private void LateUpdate()
	{
		Vector3 origin = transform.position;
		Vector3 target = new Vector3
		{
			x = targetPos.position.x + offsetX,
			y = targetPos.position.y + offsetY,
			z = targetPos.position.z - zDistance
		};
		transform.position = Vector3.Lerp(origin, target, followSpeed * Time.deltaTime);
	}
}
