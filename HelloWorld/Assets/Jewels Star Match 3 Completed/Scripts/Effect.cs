using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour
{
	public static int bonusLighting = 0;
    public static int SetCount = 0;

    
	public static void SpawnNumber (Vector2 pos, GameObject obj, Sprite[] s, float time)
	{
             
			EffectTimer.isResetCombo = false;
			GameObject tmp = Instantiate (obj) as GameObject;
			int idx = EffectTimer.Combo / 3;
			if (idx > 12)
						idx = 12;
            if(!Menu.IsLose && !Menu.IsWin)            
			    MapLoader.score += (idx+1) * 10;
            
            if (MapLoader.Mode ==0)
                 GameObject.Find("top").GetComponent<Menu>().scoreinc((idx + 1) * 5);
			tmp.transform.Find ("Render").GetComponent<SpriteRenderer> ().sprite = s[idx];
            tmp.transform.localPosition = new Vector3(pos.x, pos.y, -1.2f);

            GameObject.Find("Screen").GetComponent<MapLoader>().Scoreupdate();
            
			EffectTimer.Combo++;
			bonusLighting++;
            SetCount++;

			if (bonusLighting == 21) {
						Editor.LightingRandomPoint ();
						bonusLighting=0;
				}
            if (SetCount == 30)
            {
                SetCount = 0;
                Editor.addLighting();
            }
		   
			Destroy (tmp, time);
	}

	public static void SpawnSet(GameObject obj ,GameObject SetPref,int power)
	{
        for (int i = 0; i < obj.transform.Find("Render").transform.childCount; i++)
            if (obj.transform.Find("Render").transform.GetChild(i) != null)
                Destroy(obj.transform.Find("Render").transform.GetChild(i).gameObject);

		GameObject tmp = Instantiate (SetPref) as GameObject;
		tmp.transform.parent = obj.transform.Find ("Render");
		tmp.transform.localPosition = new Vector3 (0,0, -1.2f);
		if (power == 2)
						tmp.transform.Rotate (0, 0, 110);
        else
                          tmp.transform.Rotate(0, 0, 20);
	}

	public static void SpawnStarWin(GameObject obj ,GameObject StarPref,bool isshow ) 
	{
        for (int i = 0; i < obj.transform.Find("Render").transform.childCount; i++)
            if (obj.transform.Find("Render").transform.GetChild(i) != null)
                Destroy(obj.transform.Find("Render").transform.GetChild(i).gameObject);

        if (isshow)
            StarPref.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        else
            StarPref.transform.GetChild(0).GetComponent<Animator>().enabled = false;
		GameObject tmp = Instantiate (StarPref) as GameObject;
		//obj.transform.Find ("Render").GetComponent<SpriteRenderer> ().sprite = null;
		tmp.transform.parent = obj.transform.Find ("Render");
		tmp.transform.localPosition = new Vector3 (0,0, -1.2f);
	}

	public static void SpawnBoom (Vector2 pos ,GameObject obj , float time)
	{
		Sound.sound.boom ();
		GameObject tmp = Instantiate (obj) as GameObject;
		//tmp.transform.Find ("Render").GetComponent<SpriteRenderer> ().sprite = s;
		tmp.transform.localPosition = new Vector3 (pos.x, pos.y, -1.2f);
		Destroy (tmp, time);
    
	}

		

	public static void RowLighting(float y ,GameObject obj) {

		Sound.sound.elec ();
		GameObject tmp = Instantiate (obj) as GameObject;
		tmp.transform.localPosition = new Vector3 (0, y, -1.2f);
		Destroy (tmp, 0.4f);
	}
	public static void ColumnLighting(float x ,GameObject obj) {


		Sound.sound.elec ();
		GameObject tmp = Instantiate (obj) as GameObject;
		tmp.transform.localPosition = new Vector3 (x, 0, -1.2f);
		Destroy (tmp, 0.4f);
	}
	public static void LightingPoint(Vector3 pos ,GameObject obj) {
		Sound.sound.bliz ();
		GameObject tmp = Instantiate (obj) as GameObject;
		tmp.transform.localPosition = new Vector3 (pos.x,pos.y, -1.2f);
		Destroy (tmp, 0.5f);
	}


		public static void SpawnEnchan(GameObject obj, GameObject parent){
            for (int i = 0; i < parent.transform.Find("Render").transform.childCount; i++)
                if (parent.transform.Find("Render").transform.GetChild(i) != null)
                    Destroy(parent.transform.Find("Render").transform.GetChild(i).gameObject);

		GameObject tmp = Instantiate (obj) as GameObject;
        tmp.transform.parent = parent.transform.Find("Render");
		tmp.transform.localPosition = new Vector3 (0.13f, 0.13f, -1.1f);
		tmp.transform.localScale = new Vector3 (2, 2, 1);
		}
	public static void SpawnType9(GameObject obj, GameObject parent){
        for (int i = 0; i < parent.transform.Find("Render").transform.childCount; i++)
            if (parent.transform.Find("Render").transform.GetChild(i) != null)
                Destroy(parent.transform.Find("Render").transform.GetChild(i).gameObject);

		GameObject tmp = Instantiate (obj) as GameObject;
		tmp.transform.parent = parent.transform.Find ("Render");
		tmp.transform.localPosition = new Vector3 (0, 0, -1.1f);
		tmp.transform.localScale = new Vector3 (1.44f, 1.44f, 1);
	}
	public static  void SpawnClock (GameObject obj, GameObject parent, Vector3 q)
	{
        for (int i = 0; i < parent.transform.Find("Render").transform.childCount; i++)
            if (parent.transform.Find("Render").transform.GetChild(i) != null)
                Destroy(parent.transform.Find("Render").transform.GetChild(i).gameObject);

		GameObject tmp = Instantiate (obj) as GameObject;
        tmp.transform.parent = parent.transform.Find("Render");
		tmp.transform.Rotate (q);
		tmp.transform.localPosition = new Vector3 (0, 0, -1.1f);
		tmp.transform.localScale = new Vector3 (1, 1, 1);
	}


}
