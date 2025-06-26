using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//creates the buttons on panel of animations
public class RC_Ui_Char_Panel : MonoBehaviour {

	public GameObject character;
	public Transform acts_table;
	public Transform weap_table;
	public Transform mat_table;
    public Button buttonPrefab;
    public Material[] Materials;
 
    Button sel_btm;

    RC_Actions actions;


	void Start () {
        actions = character.GetComponent<RC_Actions>();

        CreateActionButton("Idle1");
		CreateActionButton("Idle2");
		CreateActionButton("Idle3");
        CreateActionButton("Move");
        CreateActionButton("MoveBack");
        CreateActionButton("MoveLimp");
        CreateActionButton("Run");
		CreateActionButton("TurnLeft");
		CreateActionButton("TurnRight");
		CreateActionButton("StrafeLeft");
		CreateActionButton("StrafeRight");
		CreateActionButton("DMG1");
		CreateActionButton("DMG2");
		CreateActionButton("DMG3");
		CreateActionButton("DMG4");
		CreateActionButton("Dead1");
		CreateActionButton("Dead2");
		CreateActionButton("Dead3");
		CreateActionButton("Dead4");
 

        CreateWeapButton("Shot");
        CreateWeapButton("BigShot");
        CreateWeapButton("GetDamage");
        CreateWeapButton("Resurrect");

        CreateMatButton("Mat1", 0);
        CreateMatButton("Mat2", 1);
        CreateMatButton("Mat3", 2);
        CreateMatButton("Mat4", 3);

    }

 
 
    void CreateWeapButton(string name)
    {

        Button button = CreateButton(name, weap_table);
        if (name == "Shot" || name == "BigShot")
            button.GetComponentInChildren<Image>().color = new Color(.5f, .3f, .3f);
        button.GetComponentInChildren<Text>().fontSize = 16;
        button.onClick.AddListener(() => actions.SendMessage(name, SendMessageOptions.DontRequireReceiver));


    }

   void CreateMatButton(string name, int mat_n)
    {

        Button button = CreateButton(name, mat_table);
        button.GetComponentInChildren<Image>().color = new Color(.3f, .3f, .5f);
        button.GetComponentInChildren<Text>().fontSize = 16;
        button.onClick.AddListener(() => ButtonClicked(mat_n));


    }

    void ButtonClicked(int mat_n)
    {
    
        Transform[] ts = character.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
            if (t.GetComponent<MeshRenderer>())
            {
                t.GetComponent<MeshRenderer>().material = Materials[mat_n];
            }
    }

    void CreateActionButton(string name ) {

	//	if (name=="") Button button = CreateButton (name, acts_table);
	//	else
		Transform group;
 		group = acts_table;
		Button button = CreateButton (name, group);

		if (name == "Idle1")
		{
			sel_btm = button;
			button.GetComponentInChildren<Image>().color = new Color(.5f, .5f, .5f);
		}
		button.GetComponentInChildren<Text>().fontSize = 12;
		button.onClick.AddListener(()  => actions.SendMessage(name, SendMessageOptions.DontRequireReceiver));
		button.onClick.AddListener(() => select_btm(button)  );


	}
    void select_btm(Button btm)
    {
		sel_btm.GetComponentInChildren<Image>().color = new Color(.345f, .345f, .345f);
		btm.GetComponentInChildren<Image>().color = new Color(.5f, .5f, .5f);
        sel_btm = btm;
    }

 
    Button CreateButton(string name, Transform group) {
		GameObject obj = (GameObject) Instantiate (buttonPrefab.gameObject);
		obj.name = name;
		obj.transform.SetParent(group);
		obj.transform.localScale = Vector3.one;
		Text text = obj.transform.GetChild (0).GetComponent<Text> ();
		text.text = name;
		return obj.GetComponent<Button> ();
	}



    public void OpenAsset() {
		Application.OpenURL ("https://www.assetstore.unity3d.com/#!/content/121829");
	}
   public void OpenPublisherPage() {
		Application.OpenURL ("https://assetstore.unity.com/publishers/27420");
	}

 
}
