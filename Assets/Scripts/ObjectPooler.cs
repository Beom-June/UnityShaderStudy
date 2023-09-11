using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 전처리하여 인스펙터 수정이 가능함
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ObjectPooler))]
public class ObjectPoolerEditor : Editor
{
	const string INFO = "풀링한 오브젝트에 다음을 적으세요 \nvoid OnDisable()\n{\n" +
		"    ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 \n" +
		"    CancelInvoke();    // Monobehaviour에 Invoke가 있다면 \n}";

	public override void OnInspectorGUI()
	{
		EditorGUILayout.HelpBox(INFO, MessageType.Info);
		base.OnInspectorGUI();
	}
}
#endif

public class ObjectPooler : MonoBehaviour
{
	static ObjectPooler _inst;					//	내부에서만 __inst 접근 가능
	void Awake() => _inst = this;

	[Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	[SerializeField] Pool[] _pools;
	List<GameObject> _spawnObjects;
	Dictionary<string, Queue<GameObject>> _poolDictionary;						//	Queue로 제작 (FIFO)
	readonly string INFO = " 오브젝트에 다음을 적으세요 \nvoid OnDisable()\n{\n" +
		"    ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 \n" +
		"    CancelInvoke();    // Monobehaviour에 Invoke가 있다면 \n}";



	public static GameObject SpawnFromPool(string tag, Vector3 position) =>
		_inst._SpawnFromPool(tag, position, Quaternion.identity);

	public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) =>
		_inst._SpawnFromPool(tag, position, rotation);

	//// Component만 들어올 수 있음
	public static T SpawnFromPool<T>(string tag, Vector3 position) where T : Component
	{
		GameObject obj = _inst._SpawnFromPool(tag, position, Quaternion.identity);
		if (obj.TryGetComponent(out T component))
			return component;
		else
		{
			obj.SetActive(false);
			throw new Exception($"Component not found");
		}
	}


	// Component만 들어올 수 있음. Quaternion 추가
	public static T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation) where T : Component
	{
		GameObject _obj = _inst._SpawnFromPool(tag, position, rotation);
		if (_obj.TryGetComponent(out T component))
			return component;
		else
		{
			_obj.SetActive(false);
			throw new Exception($"Component not found");
		}
	}

	public static List<GameObject> GetAllPools(string tag)
	{
		if (!_inst._poolDictionary.ContainsKey(tag))
			throw new Exception($"Pool with tag {tag} doesn't exist.");

		return _inst._spawnObjects.FindAll(x => x.name == tag);
	}

	public static List<T> GetAllPools<T>(string tag) where T : Component
	{
		List<GameObject> objects = GetAllPools(tag);

		if (!objects[0].TryGetComponent(out T component))
			throw new Exception("Component not found");

		return objects.ConvertAll(x => x.GetComponent<T>());
	}

	public static void ReturnToPool(GameObject obj)
	{
		if (!_inst._poolDictionary.ContainsKey(obj.name))
			throw new Exception($"Pool with tag {obj.name} doesn't exist.");

		_inst._poolDictionary[obj.name].Enqueue(obj);
	}

	// 우클릭해서 갯수 알아보는 부분
	[ContextMenu("GetSpawnObjectsInfo")]
	void GetSpawnObjectsInfo()
	{
		foreach (var pool in _pools)
		{
			int count = _spawnObjects.FindAll(x => x.name == pool.tag).Count;
			Debug.Log($"{pool.tag} count : {count}");
		}
	}

	GameObject _SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
	{
		// 태그 없으면 예외 던짐
		if (!_poolDictionary.ContainsKey(tag))
			throw new Exception($"Pool with tag {tag} doesn't exist.");

		// 큐에 없으면 새로 추가
		Queue<GameObject> _poolQueue = _poolDictionary[tag];
		if (_poolQueue.Count <= 0)
		{
			Pool _pool = Array.Find(_pools, x => x.tag == tag);
			var _obj = CreateNewObject(_pool.tag, _pool.prefab);
			ArrangePool(_obj);									//	정렬
		}

		// 큐에서 꺼내서 사용
		GameObject _objectToSpawn = _poolQueue.Dequeue();
		_objectToSpawn.transform.position = position;
		_objectToSpawn.transform.rotation = rotation;
		_objectToSpawn.SetActive(true);

		return _objectToSpawn;
	}

	void Start()
	{
		_spawnObjects = new List<GameObject>();
		_poolDictionary = new Dictionary<string, Queue<GameObject>>();

		// 미리 생성
		foreach (Pool pool in _pools)
		{
			_poolDictionary.Add(pool.tag, new Queue<GameObject>());
			for (int i = 0; i < pool.size; i++)
			{
				var obj = CreateNewObject(pool.tag, pool.prefab);
				ArrangePool(obj);
			}

			// OnDisable에 ReturnToPool 구현여부와 중복구현 검사
			if (_poolDictionary[pool.tag].Count <= 0)
            {
				Debug.LogError($"{pool.tag}{INFO}");
            }
			else if (_poolDictionary[pool.tag].Count != pool.size)
            {
				Debug.LogError($"{pool.tag}에 ReturnToPool이 중복됩니다");
            }
		}
	}

	GameObject CreateNewObject(string tag, GameObject prefab)
	{
		var _obj = Instantiate(prefab, transform);
		_obj.name = tag;
		_obj.SetActive(false); // 비활성화시 ReturnToPool을 하므로 Enqueue가 됨
		return _obj;
	}

	// 정렬
	void ArrangePool(GameObject obj)
	{
		// 추가된 오브젝트 묶어서 정렬
		bool _isFind = false;
		for (int i = 0; i < transform.childCount; i++)
		{
			if (i == transform.childCount - 1)
			{
				obj.transform.SetSiblingIndex(i);
				_spawnObjects.Insert(i, obj);
				break;
			}
			else if (transform.GetChild(i).name == obj.name)
				_isFind = true;
			else if (_isFind)
			{
				obj.transform.SetSiblingIndex(i);
				_spawnObjects.Insert(i, obj);
				break;
			}
		}
	}
}

