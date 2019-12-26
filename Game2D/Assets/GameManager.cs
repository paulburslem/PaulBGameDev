using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
	List<Transform> followTargets = new List<Transform>();
	public Canvas ui;
	Camera cam;
	//CinemaMachineTargetGroup
	void Start()
	{
		cam = Camera.main;

	}

	// Update is called once per frame
	void Update()
	{

	}
	UnityEngine.SceneManagement.Scene currentScene;
	public void LoadLevel(string scene)
	{
		if (currentScene.IsValid())
			UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
		if (!string.IsNullOrEmpty(scene))
		{
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
			currentScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(UnityEngine.SceneManagement.SceneManager.sceneCount - 1);
			ui.enabled = false;
		}
		else
		{
			currentScene = default;
			ui.enabled = true;
		}
	} 
	public void PlayerJoined(PlayerInput p)
	{
		if (!currentScene.IsValid())
			LoadLevel("level1");
		followTargets.Add(p.transform);
	}

	private void LateUpdate()
	{
		if (followTargets.Count == 0)
			return;
		Vector2 pos = Vector2.zero;
		foreach (var t in followTargets)
			pos += new Vector2(t.position.x, t.position.y);
		pos /= followTargets.Count;
		var camPos = new Vector2(cam.transform.position.x, cam.transform.position.y);
		var dir = pos - camPos;
		var dist = dir.magnitude;
		if (dist < .1f)
			return;
		var pow = dist / 10;
		var pow2 = pow * pow;
		var move = dir.normalized * pow2 * Time.deltaTime* 50;
		cam.transform.Translate(move.x, move.y, 0);
		if (followTargets.Count == 1)
			return;
		Vector2 far = Vector2.zero;
		float fd2 = 0;
		foreach (var t in followTargets)
		{
			var tp = new Vector2(t.position.x, t.position.y);
			var d2 = (tp - camPos).sqrMagnitude;
			if (d2 > fd2)
			{
				far = tp;
				fd2 = d2;
			}
		}
		var size = Mathf.Sqrt(fd2) * 2;
		cam.orthographicSize =  Mathf.Clamp(size, 15, 30);
	}
}
