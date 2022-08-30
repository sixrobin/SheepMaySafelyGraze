namespace RSLib.Jumble
{
	using UnityEngine;

	[RequireComponent(typeof(Camera))]
	public class BasicThirdPersonCamera : MonoBehaviour
	{
		[SerializeField] private Camera _camera = null;
		[SerializeField] private Transform _target = null;
		[SerializeField] private float _height = 0.5f;
		[SerializeField] private float _distance = 2f;
		[SerializeField] private float _minimumPitch = 90f;
		[SerializeField] private float _maximumPitch = 90f;
		[SerializeField] private Vector2 _sensitivity = Vector2.one;
		[SerializeField] private Vector3 _localOffset = Vector3.zero;
		[SerializeField] private LayerMask _cameraMask = 0;

		private float _x;
		private float _y;
		private Vector3[] _corners;

		private Vector3 _currentPosition;
		private Quaternion _currentRotation;
		private float _currentDistance;

		private void Rotate(float x, float y)
		{
			_x += x;
			_y += y;
			_x = Mathf.Clamp(_x, -_minimumPitch, _maximumPitch);
		}

		private void ResetPosition()
		{
			_currentPosition = _target.transform.position;
		}

		private void GoToPivotPosition()
		{
			_currentPosition += Vector3.up * _height;
			transform.position = Vector3.Lerp(transform.position, _currentPosition, 0.4f);
		}

		private void GoToRotation()
		{
			_currentRotation = Quaternion.Euler(_x, _y, 0f);
			transform.rotation = Quaternion.Lerp(transform.rotation, _currentRotation, 0.12f);
		}

		private void GoBack()
		{
			_currentDistance = Mathf.Lerp(_currentDistance, GetCameraCollisionDistance(), 0.2f);
			transform.position -= transform.forward * _currentDistance;
		}

		private float GetCameraCollisionDistance()
		{
			for (int i = 0; i < _corners.Length; ++i)
                if (Physics.Raycast(_camera.ViewportToWorldPoint(_corners[i]), -transform.forward, out RaycastHit hit, _distance, _cameraMask))
					return hit.distance;

			return _distance;
		}

		private void ApplyLocalCameraOffset()
		{
			transform.position += transform.TransformDirection(_localOffset);
		}

		private void UpdateCameraPosition()
		{
			float x = -Input.GetAxis("Mouse Y") * _sensitivity.y;
			float y = Input.GetAxis("Mouse X") * _sensitivity.x;

			if (x != 0f || y != 0f)
				Rotate(x, y);

			ResetPosition();
			GoToPivotPosition();
			GoToRotation();
			GoBack();
			ApplyLocalCameraOffset();
		}

		private void Start()
		{
			_corners = new Vector3[4]
			{
				new Vector3(0f, 0f),
				new Vector3(0f, 1f),
				new Vector3(1f, 1f),
				new Vector3(1f, 0f)
			};
		}

		private void FixedUpdate()
		{
			UpdateCameraPosition();
		}
	}
}