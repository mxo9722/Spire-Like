using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MapCameraControl : Singleton<MapCameraControl>
{
	[SerializeField] private float _maxZoom;
	[SerializeField] private float _minZoom;
	[SerializeField] private float _panSpeed = -1;

	[SerializeField] private Camera _cam;

	[SerializeField] private RectTransform _topBar;

	private float _baseZoom;

	private Vector3 _bottomLeft;
	private Vector3 _topRight;

	private float _cameraMaxY;
	private float _cameraMinY;
	private float _cameraMaxX;
	private float _cameraMinX;

	protected override void Awake()
	{
		base.Awake();

		//set max camera bounds (assumes camera is max zoom and centered on Start)
		_topRight = _cam.ScreenToWorldPoint(new Vector3(_cam.pixelWidth, _cam.pixelHeight, -transform.position.z));
		_bottomLeft = _cam.ScreenToWorldPoint(new Vector3(0, 0, -transform.position.z));
		_baseZoom = _cam.orthographicSize;
		_cameraMaxX = _topRight.x;
		_cameraMaxY = _topRight.y;
		_cameraMinX = _bottomLeft.x;
		_cameraMinY = _bottomLeft.y;
	}

	private void Update()
	{
		//click and drag
		if (Input.GetMouseButton(0))
		{
			float x = Input.GetAxis("Mouse X") * _panSpeed * Camera.main.orthographicSize / _baseZoom;
			float y = Input.GetAxis("Mouse Y") * _panSpeed * Camera.main.orthographicSize / _baseZoom;
			transform.Translate(x, y, 0);
		}

		//zoom
		if ((Input.GetAxis("Mouse ScrollWheel") > 0) && Camera.main.orthographicSize > _baseZoom / _minZoom) // forward
        {
			Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 0.5f, _baseZoom / _minZoom);
		}

		if ((Input.GetAxis("Mouse ScrollWheel") < 0) && Camera.main.orthographicSize < _baseZoom / _maxZoom) // back            
        {
			Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize + 0.5f, _baseZoom / _maxZoom);
		}


		//check if camera is out-of-bounds, if so, move back in-bounds
		_topRight = _cam.ScreenToWorldPoint(new Vector3(_cam.pixelWidth, _cam.pixelHeight, -transform.position.z));
		_bottomLeft = _cam.ScreenToWorldPoint(new Vector3(0, 0, -transform.position.z));

		if (_topRight.x > _cameraMaxX)
		{
			transform.position = new Vector3(transform.position.x - (_topRight.x - _cameraMaxX), transform.position.y, transform.position.z);
		}

		if (_topRight.y > _cameraMaxY)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y - (_topRight.y - _cameraMaxY), transform.position.z);
		}

		if (_bottomLeft.x < _cameraMinX)
		{
			transform.position = new Vector3(transform.position.x + (_cameraMinX - _bottomLeft.x), transform.position.y, transform.position.z);
		}

		if (_bottomLeft.y < _cameraMinY)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y + (_cameraMinY - _bottomLeft.y), transform.position.z);
		}
	}

	public void SetViewSize(Bounds bounds, float duration = 0)
	{
		Vector3 center = bounds.center;

		Vector3 newPos = new Vector3(center.x, center.y, transform.position.z);

		//if (duration == 0)
		//	transform.position = newPos;
		//else
		//	transform.DOMove(newPos, duration);

		Vector3 halfTopBar = new(0, _topBar.sizeDelta.y);
		float screenHeight = Screen.height - _topBar.sizeDelta.y;

		float camAspect = Screen.width / screenHeight;
		float boundsAspect = bounds.extents.x / bounds.extents.y;

		float targetOrtho = 0;

		if (boundsAspect <= camAspect)
		{
			targetOrtho = bounds.extents.y;
			
		}
		else
		{
			targetOrtho = bounds.extents.x / camAspect;
		}

		newPos += halfTopBar * ((targetOrtho) / Screen.height);
		targetOrtho *= camAspect / _cam.aspect;

		if (duration == 0)
		{
			transform.position = newPos;
			_cam.orthographicSize = targetOrtho;
		}
		else
		{
			transform.DOMove(newPos, duration);
			_cam.DOOrthoSize(targetOrtho, duration);

		}
	}
}