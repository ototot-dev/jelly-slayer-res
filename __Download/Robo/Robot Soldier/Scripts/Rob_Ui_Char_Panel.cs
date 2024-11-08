using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
 
//creates the buttons on panel of animations
public class Rob_Ui_Char_Panel : MonoBehaviour {

	public GameObject character;
	 
	public Transform acts_table;
	public Transform BT_weap_table;
    bool BT_weap_table_bool;
	public Transform weap_table;
	public Transform mat_table;
    public Button buttonPrefab;
    public Material[] MaterialsBody;
    public Material[] MaterialsHead;
    public Material[] MaterialsWeap;
 
    Button sel_btm;

    Rob_Actions actions; 


	void Start () {
        actions = character.GetComponent<Rob_Actions>();

        CreateActionButton("Idle1");
		CreateActionButton("Idle2");
		CreateActionButton("Idle3");
	
        CreateActionButton("Walk");
        CreateActionButton("WalkBack");
        CreateActionButton("Run");
        CreateActionButton("Kick");
 
        CreateActionButton("CrouchIdle");
        CreateActionButton("CrouchMove");
 
        CreateActionButton("TurnLeft");
		CreateActionButton("TurnRight");
		CreateActionButton("StrafeLeft");
		CreateActionButton("StrafeRight");
		CreateActionButton("Hit1");
		CreateActionButton("Hit2");
		CreateActionButton("Hit3");
		CreateActionButton("Hit4");
		CreateActionButton("Death1");
		CreateActionButton("Death2");



        CreateBTWeapButton("Weapon");
 

        CreateWeapButton("ArmIdle");
        CreateWeapButton("ArmAim");
        CreateWeapButton("ArmShot");
	    CreateWeapButton("IdleAim");
	    CreateWeapButton("RunAim");
	    CreateWeapButton("CrouchIdleAim");
		CreateWeapButton("Death3");
		CreateWeapButton("Death4");

        CreateMatButton("Mat1", 0);
        CreateMatButton("Mat2", 1);
        CreateMatButton("Mat3", 2);
        CreateMatButton("Mat4", 3);
        CreateMatButton("Mat5", 4);
        CreateMatButton("MatAnim", 5);

    }

 
 
    void CreateWeapButton(string name)
    {
        Button    button = CreateButton(name, weap_table );
        if (name == "ArmIdle" || name == "ArmAim" || name == "ArmShot")
            button.GetComponentInChildren<Image>().color = new Color(.5f, .3f, .3f);
        button.GetComponentInChildren<Text>().fontSize = 12;

        
        button.onClick.AddListener(() => actions.SendMessage(name, SendMessageOptions.DontRequireReceiver));


    }

   void CreateBTWeapButton(string name )
    {

        Button button = CreateButton(name, BT_weap_table);
        button.GetComponentInChildren<Image>().color = new Color(.1f, .5f, .5f);
        button.GetComponentInChildren<Text>().fontSize = 16;
        button.onClick.AddListener(() => ButtonClickedBTWeap());


    }
        void ButtonClickedBTWeap()
    {
            BT_weap_table_bool=!BT_weap_table_bool;
            weap_table.gameObject.SetActive(BT_weap_table_bool);
         var   anim = character.GetComponent<Animator>();
        if(BT_weap_table_bool) anim.SetLayerWeight(1, 1); else anim.SetLayerWeight(1, 0);
            Transform[] ts = character.GetComponentsInChildren<Transform>(true);
          foreach (Transform t in ts)        if (t.gameObject.name == "Rob_Weap1" )    t.gameObject.SetActive(BT_weap_table_bool);
 
    }
   void CreateMatButton(string name, int mat_n)
    {

        Button button = CreateButton(name, mat_table);
        button.GetComponentInChildren<Image>().color = new Color(.3f, .3f, .5f);
        button.GetComponentInChildren<Text>().fontSize = 12;
        button.onClick.AddListener(() => ButtonClicked(mat_n));


    }

 
    void ButtonClicked(int mat_n)
    {
   
 
        Transform[] ts = character.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts ){
               
             if (t.GetComponent<SkinnedMeshRenderer>())
                if(t.GetComponent<SkinnedMeshRenderer>().material.name.IndexOf("Weap1")<0)
            {

                if(t.GetComponent<SkinnedMeshRenderer>().material.name.IndexOf("Head2")>0) t.GetComponent<SkinnedMeshRenderer>().material = MaterialsHead[mat_n]; 
                else
                t.GetComponent<SkinnedMeshRenderer>().material = MaterialsBody[mat_n];
      
            }
            else
            {
               var m =0 ;
                if(mat_n==2) m=2;
                if(mat_n==3) m=1;
                if(mat_n==4) m=3;
                if(mat_n==5) m=4;
                t.GetComponent<SkinnedMeshRenderer>().material = MaterialsWeap[m]; 
            }
 

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



 
   public void OpenPublisherPage() {
		Application.OpenURL ("https://assetstore.unity.com/publishers/27420");
	}

 
}
