using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// ��ó���Ͽ� �ν����� ������ ������
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ObjectPooler))]
public class ObjectPoolerEditor : Editor
{
	const string INFO = "Ǯ���� ������Ʈ�� ������ �������� \nvoid OnDisable()\n{\n" +
		"    ObjectPooler.ReturnToPool(gameObject);    // �� ��ü�� �ѹ��� \n" +
		"    CancelInvoke();    // Monobehaviour�� Invoke�� �ִٸ� \n}";

	public override void OnInspectorGUI()
	{
		EditorGUILayout.HelpBox(INFO, MessageType.Info);
		base.OnInspectorGUI();
	}
}
#endif

public class ObjectPooler : MonoBehaviour
{
	static ObjectPooler _inst;					//	���ο����� __inst ���� ����
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
	Dictionary<string, Queue<GameObject>> _poolDictionary;						//	Queue�� ���� (FIFO)
	readonly string INFO = " ������Ʈ�� ������ �������� \nvoid OnDisable()\n{\n" +
		"    ObjectPooler.ReturnToPool(gameObject);    // �� ��ü�� �ѹ��� \n" +
		"    CancelInvoke();    // Monobehaviour�� Invoke�� �ִٸ� \n}";



	public static GameObject SpawnFromPool(string tag, Vector3 position) =>
		_inst._SpawnFromPool(tag, position, Quaternion.identity);

	public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) =>
		_inst._SpawnFromPool(tag, position, rotation);

	//// Component�� ���� �� ����
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


	// Component�� ���� �� ����. Quaternion �߰�
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

	// ��Ŭ���ؼ� ���� �˾ƺ��� �κ�
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
		// �±� ������ ���� ����
		if (!_poolDictionary.ContainsKey(tag))
			throw new Exception($"Pool with tag {tag} doesn't exist.");

		// ť�� ������ ���� �߰�
		Queue<GameObject> _poolQueue = _poolDictionary[tag];
		if (_poolQueue.Count <= 0)
		{
			Pool _pool = Array.Find(_pools, x => x.tag == tag);
			var _obj = CreateNewObject(_pool.tag, _pool.prefab);
			ArrangePool(_obj);									//	����
		}

		// ť���� ������ ���
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

		// �̸� ����
		foreach (Pool pool in _pools)
		{
			_poolDictionary.Add(pool.tag, new Queue<GameObject>());
			for (int i = 0; i < pool.size; i++)
			{
				var obj = CreateNewObject(pool.tag, pool.prefab);
				ArrangePool(obj);
			}

			// OnDisable�� ReturnToPool �������ο� �ߺ����� �˻�
			if (_poolDictionary[pool.tag].Count <= 0)
            {
				Debug.LogError($"{pool.tag}{INFO}");
            }
			else if (_poolDictionary[pool.tag].Count != pool.size)
            {
				Debug.LogError($"{pool.tag}�� ReturnToPool�� �ߺ��˴ϴ�");
            }
		}
	}

	GameObject CreateNewObject(string tag, GameObject prefab)
	{
		var _obj = Instantiate(prefab, transform);
		_obj.name = tag;
		_obj.SetActive(false); // ��Ȱ��ȭ�� ReturnToPool�� �ϹǷ� Enqueue�� ��
		return _obj;
	}

	// ����
	void ArrangePool(GameObject obj)
	{
		// �߰��� ������Ʈ ��� ����
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

