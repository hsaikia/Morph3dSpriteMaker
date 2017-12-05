using UnityEngine;
using System.Collections;
using MORPH3D;
using MORPH3D.FOUNDATIONS;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController instance;


	//[SerializeField]
	//private GameObject playerController;
	[SerializeField]
	private GameObject modelPrefab;
	[SerializeField]
	private GameObject shirtPack;
	[SerializeField]
	private GameObject pantsPack;
	[SerializeField]
	private GameObject hairContentPack;

	private M3DCharacterManager manager;
	private GameObject playerModel;
    private bool isReady = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
	{
		CreatePlayerModel();
		AddPlayerHair();
		AddPlayerClothings (pantsPack);
		AddPlayerClothings (shirtPack);
        isReady = true;
	}

    public bool getIsReady()
    {
        return isReady;
    }

    public void SetBlendshapeValue (float value) {
        Debug.Log("Set BlendShape called with value " + value);
		manager.SetBlendshapeValue ("FBMHeavy", value);
	}

	void CreatePlayerModel()
	{
		GameObject playerInstance;
		playerInstance = Instantiate(modelPrefab);
		playerInstance.transform.position = new Vector3(0, 0, 0);
		playerInstance.transform.eulerAngles = new Vector3(0, 180, 0);
		//Animator animator = GetComponent<Animator> ();
		//animator.avatar = playerInstance.GetComponent<Animator> ().avatar;
		manager = playerInstance.GetComponent<M3DCharacterManager>();
		playerModel = playerInstance;
	}

	void AddPlayerHair()
	{
		ContentPack hairPack = new ContentPack();
		hairPack.setupWithGameObject(hairContentPack);
		foreach (var hair in hairPack.availableHair)
		{
			manager.AttachCIHair(hair, true);
		}
	}


	public void AddPlayerClothings(GameObject clothingContentPack)
	{
		ContentPack clothingPack = new ContentPack();
		clothingPack.setupWithGameObject(clothingContentPack);
		foreach (MORPH3D.COSTUMING.CIclothing clothing in clothingPack.availableClothing)
		{
			manager.AttachCIClothing(clothing, true);
		}
		manager.DetectAttachedClothing();
	}

	public void RemovePlayerClothings()
	{
		List<MORPH3D.COSTUMING.CIclothing> clothingList = manager.GetAllClothing();
		foreach (MORPH3D.COSTUMING.CIclothing clothing in clothingList)
		{
			Debug.Log(clothing.name);
			if (clothing.name != "G2FSimplifiedEyes")
				DestroyObject(clothing.gameObject);
		}
		manager.DetectAttachedClothing();
	}
}
