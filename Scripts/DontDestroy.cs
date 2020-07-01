using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// DonDestroy Class
//  by seeper
//
// 목적
//   다른 씬이 로드 될때 전체를 관리하기 위한 오브젝트가 필요할때.
//   특히 이 오브젝트를 아무때나 접근 하고 싶을때.
//
// 사용방법 
//   1) DontDestroyOnLoad()를 걸 MonoBehaviour의 클래스에 상속한다.
//   2) MainRoutine.Instance.LoadStage() 처럼 호출 가능하다.
//
// 주의
//   상속받은 클래스는 절대 Awake(), Start()를 사용하지 마시고 반드시
//   override protected void OnAwake(), 
//   override protected void OnStart()로 사용
//
///////////////////////////////////////////////////////////////////////////////

public class DontDestroy<T> : MonoBehaviour where T : DontDestroy<T>
{
	static public T Instance { get; private set; }

	void Awake()
	{
		if (Instance == null)
		{
			Instance = (T)this;
			DontDestroyOnLoad(gameObject);
			OnAwake();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		if (Instance == (T)this)
		{
			OnStart();
		}
	}

	virtual protected void OnAwake()
	{
	}

	virtual protected void OnStart()
	{
	}
}
